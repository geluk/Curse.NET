using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using Curse.NET.ExtensionMethods;
using Curse.NET.Model;
using Curse.NET.SocketModel;

namespace Curse.NET
{
	public class CurseClient : IDisposable
	{
		private readonly CurseApi curseApi = new CurseApi();
		private readonly SocketApi socketApi = new SocketApi();
		private LoginResponse login;
		private SessionResponse session;
		private Timer loginRenwalTimer;
		private string username;
		private string password;

		public event MessageReceivedEvent MessageReceived;
		public event Action WebsocketReconnected;
		public event Action ConnectionLost;

		public bool AutoReconnect { get; set; } = true;

		public IReadOnlyList<Group> Groups { get; private set; }
		public IReadOnlyList<Friend> Friends { get; private set; }
		public IReadOnlyDictionary<string, Channel> ChannelMap { get; private set; }
		// TODO: implement this
		public IReadOnlyDictionary<string, Group> GroupMap { get; private set; }

		public bool Connected => socketApi.Connected;

		public const string Version = "0.1.4";

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
			// Create a session
			session = curseApi.Post<SessionResponse>("https://sessions-v1.curseapp.net/sessions", SessionRequest.Create());
			// Connect the Websocket
			ConnectSocket();
			// Renew the auth token an hour before it times out.
			loginRenwalTimer = new Timer(HandleTokenRenewal, null, login.Session.RenewAfter - DateTime.Now - TimeSpan.FromHours(1), TimeSpan.FromMilliseconds(-1));
		}

		private void HandleTokenRenewal(object state)
		{
			// TODO: renew the API token
		}

		private void ConnectSocket()
		{
			ForwardEvents();
			socketApi.Connect(new Uri(session.NotificationServiceUrl), login.Session.Token);

			var semaphore = new SemaphoreSlim(0, 1);
			SocketLoginResponse response = null;
			socketApi.LoginReceived += rs =>
			{
				response = rs;
				semaphore.Release();
			};
			socketApi.Login(session.MachineKey, session.SessionID, session.User.UserID);
			if (semaphore.Wait(TimeSpan.FromSeconds(30)))
			{
				if (response.Status != 1)
				{
					throw new LoginException("Unable to connect to the Curse Websocket server. The server denied the login request.", response.Status);
				}
			}
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
			socketApi.SocketClosed += SocketApiOnSocketClosed;
			socketApi.MessageReceived += OnMessageReceived;
			socketApi.ChannelMarkedRead += OnChannelMarkedRead;
			socketApi.MessageChanged += OnMessageChanged;
			socketApi.UserActivityChange += OnUserActivityChange;
		}

		private void SocketApiOnSocketClosed()
		{
			socketApi.Dispose();
			if (AutoReconnect)
			{
				ConnectSocket();
				WebsocketReconnected?.Invoke();
			}
			else
			{
				ConnectionLost?.Invoke();
			}
		}

		private void OnUserActivityChange(UserActivityChangeResponse activity)
		{
			var group = GroupMap[activity.GroupID];
			var user = group.LookupUser(activity.Users.First().UserID);
			// TODO: handle user activity change
			return;
		}

		private void OnMessageChanged(MessageChangedResponse change)
		{
			switch (change.ChangeType)
			{
				case ChangeType.Deleted:
					break;
				case ChangeType.ChannelChange:
					break;
				default:
					break;
			}
		}

		private void OnChannelMarkedRead(ChannelMarkedReadResponse channel)
		{
			var ch = ChannelMap[channel.ConversationID];
			;
		}

		private void OnMessageReceived(MessageResponse message)
		{
			var channel = ChannelMap[message.ConversationID];
			var group = GroupMap[message.RootConversationID];
			if (message.DeletedUserID != 0 || message.EditedUserID != 0)
			{
				;
			}
			MessageReceived?.Invoke(@group, channel, message);

		}

		/// <summary>
		/// Returns the first group with the given name, or null no group with that name exist.
		/// </summary>
		public Group FindGroup(string name) => Groups.FirstOrDefault(g => g.GroupTitle == name);


		public void SendMessage(string groupId, int userId, string message)
		{
			var rs = curseApi.Post($"https://conversations-v1.curseapp.net/conversations/{groupId}:{userId}:{session.User.UserID}", new SendMessageRequest
			{
				Body = message,
				// TODO: Verify that SessionID should be used here
				ClientID = session.SessionID,
				MachineKey = session.MachineKey
			});
		}

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

		public void DeleteMessage(string conversationId, string serverId, DateTime timestamp)
		{
			var channel = ChannelMap[conversationId];
			var group = GroupMap[channel.RootGroupID];
			curseApi.Delete($"https://conversations-v1.curseapp.net/conversations/{channel.GroupID}/{serverId}-{timestamp.ToTimestamp()}");
		}

		public FoundMessage[] GetMessages(string channelId, DateTime start, DateTime end, int pageSize)
		{
			var startTs = start == DateTime.MinValue ? 0 : start.ToTimestamp();
			var endTs = end == DateTime.MaxValue ? 0 : end.ToTimestamp();

			var rs = curseApi.Get<FoundMessage[]>($"https://conversations-v1.curseapp.net/conversations/{channelId}?startTimestamp={startTs}&endTimestamp={endTs}&pageSize={pageSize}");
			return rs;
		}

		public void Dispose()
		{
			socketApi?.Dispose();
		}
	}
}
