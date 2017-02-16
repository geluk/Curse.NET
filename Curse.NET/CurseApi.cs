using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Curse.NET.ExtensionMethods;
using Curse.NET.Model;

namespace Curse.NET
{
	public class CurseApi
	{
		private readonly HttpApi httpApi = new HttpApi();
		public SessionResponse Session { get; private set; }

		public LoginResponse Login(string username, string password)
		{
			username = WebUtility.UrlEncode(username);
			password = WebUtility.UrlEncode(password);
			return httpApi.Post<LoginResponse>("https://logins-v1.curseapp.net/login", $"username={username}&password={password}");
		}

		public void CreateSession()
		{
			Session = httpApi.Post<SessionResponse>("https://sessions-v1.curseapp.net/sessions", SessionRequest.Create());
		}

		public void SetAuthToken(string authToken)
		{
			httpApi.AuthToken = authToken;
		}

		public void SendMessage(string groupId, int userId, string message)
		{
			httpApi.Post($"https://conversations-v1.curseapp.net/conversations/{groupId}:{userId}:{Session.User.UserID}", new SendMessageRequest
			{
				Body = message,
				// TODO: Verify that SessionID should be used here
				ClientID = Session.SessionID,
				MachineKey = Session.MachineKey
			});
		}

		public void SendMessage(string channelId, string message)
		{
			httpApi.Post($"https://conversations-v1.curseapp.net/conversations/{channelId}", new SendMessageRequest
			{
				Body = message,
				// TODO: Verify that SessionID should be used here
				ClientID = Session.SessionID,
				MachineKey = Session.MachineKey
			});
		}

		public Message[] FindMessages(string  groupId, DateTime start, DateTime end, int pageSize)
		{
			return httpApi.Get<Message[]>($"https://conversations-v1.curseapp.net/conversations/{groupId}" +
					$"?startTimestamp={start.ToTimestamp()}" +
					$"&endTimestamp={end.ToTimestamp()}" +
					$"&pageSize={pageSize}");
		}

		public UserResponse[] GetMembers(string groupId, bool actives, int pageNumber, int pageSize)
		{
			return httpApi.Get<UserResponse[]>($"https://groups-v1.curseapp.net/groups/{groupId}/members?actives={actives}&page={pageNumber}&pageSize={pageSize}");
		}

		public UserResponse GetMember(string groupId, int userId)
		{
			return httpApi.Get<UserResponse>($"https://groups-v1.curseapp.net/groups/{groupId}/members/{userId}");
		}

		public void KickUser(string groupId, int userId)
		{
			httpApi.Delete($"https://groups-v1/curseapp.net/groups/{groupId}/members/{userId}");
		}

		public UserResponse[] FindMembers(string groupId, string name)
		{
			return httpApi.Get<UserResponse[]>($"https://groups-v1.curseapp.net/groups/{groupId}/members/simple-search?query={name}");
		}

		public ContactsResponse LoadContacts()
		{
			return httpApi.Get<ContactsResponse>("https://contacts-v1.curseapp.net/contacts");
		}

		public void DeleteMessage(string conversationId, string serverId, DateTime timestamp)
		{
			try
			{
				httpApi.Delete($"https://conversations-v1.curseapp.net/conversations/{conversationId}/{serverId}-{timestamp.ToTimestamp()}");
			}
			catch (WebException e) when ((e.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
			{
				throw new NotAuthorisedException("You are not authorised to delete this message.");
			}
		}

		public Message[] GetMessages(string channelId, DateTime start, DateTime end, int pageSize)
		{
			// TODO: check max page size
			var startTs = start == DateTime.MinValue ? 0 : start.ToTimestamp();
			var endTs = end == DateTime.MaxValue ? 0 : end.ToTimestamp();

			var rs = httpApi.Get<Message[]>($"https://conversations-v1.curseapp.net/conversations/{channelId}?startTimestamp={startTs}&endTimestamp={endTs}&pageSize={pageSize}");
			return rs;
		}
	}
}
