using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Curse.NET.Model;
using Curse.NET.SocketModel;

namespace Curse.NET
{
	public class CurseClient : IDisposable
	{
		private readonly SocketApi socketApi = new SocketApi();
		private LoginResponse login;
		private Timer loginRenwalTimer;

		public event Action<Group, Channel, MessageResponse> MessageReceived;
		public event Action<ContactResponse, MessageResponse> PrivateMessageReceived;
		public event Action WebsocketReconnected;
		public event Action ConnectionLost;

		public CurseApi CurseApi { get; } = new CurseApi();
		public bool AutoReconnect { get; set; } = true;
		public TimeSpan WaitTimeout { get; set; } = new TimeSpan(0, 0, 0, 10);

		public IReadOnlyList<Group> Groups { get; private set; }
		public IReadOnlyList<Friend> Friends { get; private set; }
		public IReadOnlyDictionary<string, Channel> ChannelMap { get; private set; }
		public IReadOnlyDictionary<string, Group> GroupMap { get; private set; }

		public bool Connected => socketApi.Connected;
		public SessionUser Self => CurseApi.Session.User;

		public const string Version = "0.1.4";

		#region Connection methods
		public void Connect(string username, string password)
		{
			login = CurseApi.Login(username, password);
			if (login.StatusMessage != null)
			{
				throw new Exception(login.StatusMessage);
			}
			if (login.Status != 1)
			{
				throw new Exception("Unable to connect");
			}
			// Connected successfully; set the API auth token
			CurseApi.SetAuthToken(login.Session.Token);
			// Load server list and friends list
			LoadContacts();
			// Create a session
			CurseApi.CreateSession();
			// Connect the Websocket
			ConnectSocket();
			ScheduleTokenRenewal(new NetworkCredential(username, password));
		}

		private void LoadContacts()
		{
			var contacts = CurseApi.LoadContacts();

			Groups = contacts.Groups;
			Friends = contacts.Friends;

			ChannelMap = Groups.SelectMany(g => g.Channels).ToDictionary(c => c.GroupID);
			GroupMap = Groups.ToDictionary(g => g.GroupID);
		}

		private void ScheduleTokenRenewal(NetworkCredential credentials)
		{
			// Renew the auth token an hour before it times out.
			var timeout = login.Session.RenewAfter - DateTime.Now - TimeSpan.FromHours(1);
			loginRenwalTimer = new Timer(HandleTokenRenewal, credentials, timeout, TimeSpan.FromMilliseconds(-1));
		}

		private void HandleTokenRenewal(object state)
		{
			// TODO: renew the API token
		}

		private void ConnectSocket()
		{
			ForwardEvents();
			socketApi.Connect(new Uri(CurseApi.Session.NotificationServiceUrl), login.Session.Token);

			var semaphore = new SemaphoreSlim(0, 1);
			SocketLoginResponse response = null;
			socketApi.LoginReceived += rs =>
			{
				response = rs;
				semaphore.Release();
			};
			socketApi.Login(CurseApi.Session.MachineKey, CurseApi.Session.SessionID, CurseApi.Session.User.UserID);
			if (semaphore.Wait(TimeSpan.FromSeconds(30)))
			{
				if (response.Status != 1)
				{
					throw new LoginException("Unable to connect to the Curse Websocket server. The server denied the login request.", response.Status);
				}
			}
		}
		#endregion

		#region Socket event handlers
		private void ForwardEvents()
		{
			socketApi.SocketClosed += HandleSocketClosed;
			socketApi.MessageReceived += HandleMessageReceived;
			socketApi.ChannelMarkedRead += HandleChannelMarkedRead;
			socketApi.MessageChanged += HandleMessageChanged;
			socketApi.UserActivityChange += HandleUserActivityChange;
		}

		private void HandleSocketClosed()
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

		private void HandleMessageReceived(MessageResponse message)
		{
			if (message.SenderID == login.Session.UserID) return;

			var split = message.ConversationID.Split(':');
			if (split.Length == 1)
			{
				// Regular message
				var channel = ChannelMap[message.ConversationID];
				var group = GroupMap[message.RootConversationID];
				if (message.DeletedUserID != 0 || message.EditedUserID != 0)
				{
					// TODO: handle edits/deletes
					return;
				}
				MessageReceived?.Invoke(group, channel, message);
			}
			else
			{
				if (message.RootConversationID != null)
				{
					// API seems to have changed, private messages should no longer be associated with a group.
					throw new InvalidOperationException("Expected groupID to be empty.");
				}
				var otherUserId = split.Select(int.Parse).First(uid => uid != Self.UserID);
				var otherUser = CurseApi.GetContact(otherUserId);
				PrivateMessageReceived?.Invoke(otherUser, message);
			}
		}

		private void HandleChannelMarkedRead(ChannelMarkedReadResponse channel) { }

		private void HandleMessageChanged(MessageChangedResponse change)
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

		private void HandleUserActivityChange(UserActivityChangeResponse activity)
		{
			//var user = GetUser(GroupMap[activity.GroupID], activity.Users.First().UserID);
			// TODO: handle activity change
		}
		#endregion

		#region API wrappers
		public void KickUser(Group group, UserResponse user)
		{
			CurseApi.KickUser(group.GroupID, user.UserID);
		}

		public void BanUser(Group group, UserResponse user)
		{
			CurseApi.BanUser(group.RootGroupID, user.UserID);
		}

		public void DeleteMessage(MessageResponse message)
		{
			CurseApi.DeleteMessage(message.ConversationID, message.ServerID, message.Timestamp);
		}

		public async Task<MessageStatusResponse> SendMessageAsync(Channel clientChannel, string message)
		{
			var ev = new ManualResetEventSlim();
			MessageStatusResponse status = null;
			var handler = new Action<MessageStatusResponse>(rs =>
			{
				if (clientChannel.GroupID == rs.ConversationID)
				{
					status = rs;
					ev.Set();
				}
			});

			socketApi.MessageStatusReceived += handler;
			CurseApi.SendMessage(clientChannel.GroupID, message);
			var result = await Task.Run(() => ev.Wait(WaitTimeout));
			socketApi.MessageStatusReceived -= handler;

			if (!result)
			{
				throw new CurseDotNetException("Message may not have been delivered. A timeout occurred while awaiting a message status response.");
			}
			return status;
		}

		public async Task<MessageStatusResponse> SendMessageAsync(UserResponse user, string message)
		{
			var ev = new ManualResetEventSlim();
			MessageStatusResponse status = null;
			var handler = new Action<MessageStatusResponse>(rs =>
			{
				var split = rs.ConversationID.Split(':');
				var otherUserId = split.Where(part => part.All(char.IsDigit)).Select(int.Parse).FirstOrDefault(id => id != Self.UserID);

				if (otherUserId == user.UserID)
				{
					status = rs;
					ev.Set();
				}
			});

			socketApi.MessageStatusReceived += handler;
			CurseApi.SendMessage(user.UserID, message);
			var result = await Task.Run(() => ev.Wait(WaitTimeout));
			socketApi.MessageStatusReceived -= handler;
			if (!result)
			{
				throw new CurseDotNetException("Message may not have been delivered. A timeout occurred while awaiting a message status response.");
			}
			if (message == "test")
			{
				throw new Exception("test");
			}
			return status;
		}

		public UserResponse[] FindMembers(Group group, string query)
		{
			return CurseApi.FindMembers(group.GroupID, query);
		}

		public CreateInviteResponse CreateInvite(Channel channel, TimeSpan lifespan, int maxUses = 0, bool autoRemoveMembers = false)
		{
			return CurseApi.CreateInvite(channel.RootGroupID, channel.GroupID, (int)Math.Round(lifespan.TotalMinutes), maxUses, autoRemoveMembers);
		}

		public CreateInviteResponse CreateInvite(Channel channel, int maxUses = 0, bool autoRemoveMembers = false)
		{
			return CurseApi.CreateInvite(channel.RootGroupID, channel.GroupID, 0, maxUses, autoRemoveMembers);
		}

        public void LikeMessage(MessageResponse message)
        {
            CurseApi.LikeMessage(message.ConversationID, message.ServerID, message.Timestamp);
        }

        public void UnlikeMessage(MessageResponse message)
        {
            CurseApi.UnlikeMessage(message.ConversationID, message.ServerID, message.Timestamp);
        }
		#endregion

		public void Dispose()
		{
			loginRenwalTimer?.Dispose();
			socketApi?.Dispose();
		}

		#region Lookup methods
		public UserResponse GetUser(Group group, int userId)
		{
			// TODO: cache these results
			return CurseApi.GetMember(group.GroupID, userId);
		}

		/// <summary>
		/// Returns the first group with the given name, or null no group with that name exist.
		/// </summary>
		public Group FindGroup(string name) => Groups.FirstOrDefault(g => g.GroupTitle == name);

		public UserResponse FindMember(Group group, string name)
		{
			var matches = FindMembers(group, name);
			if (matches.Length > 1) throw new ArgumentException("Ambiguous username");
			return matches.FirstOrDefault();
		}
		#endregion
	}
}
