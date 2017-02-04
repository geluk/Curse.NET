using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Curse.NET.Model;

namespace Curse.NET
{
	public class CurseClient
	{
		private readonly CurseApi curseApi = new CurseApi();
		private readonly SocketApi socketApi = new SocketApi();

		private LoginResponse login;
		private SessionResponse session;

		public event MessageReceivedEvent MessageReceived;

		public IReadOnlyList<Group> Groups { get; private set; }
		public IReadOnlyList<Friend> Friends { get; private set; }
		public IReadOnlyDictionary<string, Channel> ChannelMap { get; private set; }
		//public IReadOnlyDictionary<string, User> UserMap { get; private set; }
		public IReadOnlyDictionary<string, Group> GroupMap { get; private set; }

		public const string Version = "0.1.1";

		public void Connect(string username, string password)
		{
			username = WebUtility.UrlEncode(username);
			password = WebUtility.UrlEncode(password);
			login = curseApi.Post<LoginResponse>("https://logins-v1.curseapp.net/login", $"username={username}&password={password}");
			if (login.StatusMessage != null)
			{
				throw new Exception(login.StatusMessage);
			}
			if (login.Status != 1)
			{
				throw new Exception("Unable to connect");
			}
			// Connected successfully; set the API auth token
			curseApi.AuthToken = login.Session.Token;
			// Bit of a hack. Oh well.
			ModelExtensions.Api = curseApi;
			// Load server list and friends list
			LoadContacts();
			var mcfStaff = Groups.First(g => g.GroupTitle == "Minecraft Forum").Channels.First(c => c.GroupTitle == "staff-offtopic");
			var messages = mcfStaff.GetMessages(DateTime.MinValue, DateTime.Now, 30);
			// Forward socket events
			ForwardEvents();

			session = curseApi.Post<SessionResponse>("https://sessions-v1.curseapp.net/sessions", SessionRequest.Create());
			socketApi.Connect(new Uri(session.NotificationServiceUrl), login.Session.Token);
			socketApi.Login(session.MachineKey, session.SessionID, session.User.UserID);
			socketApi.Listen();
		}

		private void LoadContacts()
		{
			var contacts = curseApi.Get<ContactsRequest>("https://contacts-v1.curseapp.net/contacts");
			Groups = contacts.Groups;
			Friends = contacts.Friends;

			ChannelMap = Groups.SelectMany(g => g.Channels).ToDictionary(c => c.GroupID);
			GroupMap = Groups.ToDictionary(g => g.GroupID);
		}

		private void ForwardEvents()
		{
			socketApi.MessageReceived += message =>
			{
				var channel = ChannelMap[message.ConversationID];
				var group = GroupMap[message.ServerID];
				MessageReceived?.Invoke(group, channel, message);
			};
		}

		/// <summary>
		/// Returns the first group with the given name, or null no group with that name exist.
		/// </summary>
		public Group FindGroup(string name) => Groups.FirstOrDefault(g => g.GroupTitle == name);

		public void SendMessage(string channelId, string message)
		{
			var rs = curseApi.Post($"https://conversations-v1.curseapp.net/conversations/{channelId}", new SendMessageRequest
			{
				Body = message,
				// TODO: Verify that SessionID should be used here
				ClientID = session.SessionID,
				MachineKey = session.MachineKey
			});
		}

		public void SendMessage(Channel clientChannel, string message)
		{
			SendMessage(clientChannel.GroupID, message);
		}
	}
}
