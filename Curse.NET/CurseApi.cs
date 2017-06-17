using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Curse.NET.ExtensionMethods;
using Curse.NET.Model;
using Newtonsoft.Json;

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

		public void SendMessage(int userId, string message)
		{
			httpApi.Post($"https://conversations-v1.curseapp.net/conversations/{userId}:{Session.User.UserID}", new SendMessageRequest
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

		public void BanUser(string serverId, int userId)
		{
			httpApi.Post($"https://groups-v1/curseapp.net/servers/{serverId}/bans/{userId}");
		}

		public UserResponse[] FindMembers(string groupId, string name)
		{
			return httpApi.Get<UserResponse[]>($"https://groups-v1.curseapp.net/groups/{groupId}/members/simple-search?query={name}");
		}

		public ContactsResponse LoadContacts()
		{
			return httpApi.Get<ContactsResponse>("https://contacts-v1.curseapp.net/contacts");
		}

		public ContactResponse GetContact(int userId)
		{
			return httpApi.Get<ContactResponse>($"https://contacts-v1.curseapp.net/users/{userId}");
		}

		public string FriendSync()
		{
			// TODO: Parse this into a model
			return httpApi.Get("https://contacts-v1.curseapp.net/friend-sync");
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

			try
			{
				var rs = httpApi.Get<Message[]>($"https://conversations-v1.curseapp.net/conversations/{channelId}?startTimestamp={startTs}&endTimestamp={endTs}&pageSize={pageSize}");
				return rs;
			}
			catch (WebException e) when((e.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.Forbidden)
			{
				throw new NotAuthorisedException("You are not authorised to view the messages for this channel.");
			}
		}

        public CreateInviteResponse CreateInvite(string serverId, string channelId, int lifespanMinutes = 0, int maxUses = 0, bool autoRemoveMembers = false)
        {
	        try
	        {
		        var rs = httpApi.Post<CreateInviteResponse>($"https://groups-v1.curseapp.net/servers/{serverId}/invites", new CreateInviteRequest
		        {
			        ChannelId = channelId,
			        LifespanMinutes = lifespanMinutes,
			        MaxUses = maxUses,
			        AutoRemoveMembers = autoRemoveMembers,
			        AdminDescription = "",
			        ReadableWordDescription = 1
		        });
		        return rs;
	        }
	        catch (WebException e) when ((e.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.BadRequest)
	        {
		        using (var rsStream = e.Response.GetResponseStream())
		        {
			        if (rsStream == null) throw new InvalidRequestException(e.Message, e);

			        var reader = new StreamReader(rsStream);
			        var responseText = reader.ReadToEnd();
			        if (string.IsNullOrEmpty(responseText)) throw new InvalidRequestException(e.Message, e);

			        var error = JsonConvert.DeserializeObject<ErrorResponse>(responseText);
			        switch (error.ErrorType)
			        {
				        case ErrorType.NotAccessibleByGuests:
					        throw new InvalidRequestException(error.Message, e);
				        default:
					        throw new InvalidRequestException(error.Message, e);
			        }
		        }
	        }
        }

        public void LikeMessage(string conversationId, string serverId, DateTime timestamp)
        {
            httpApi.Post($"https://conversations-v1.curseapp.net/conversations/{conversationId}/{serverId}-{timestamp.ToTimestamp()}/like");
        }

        public void UnlikeMessage(string conversationId, string serverId, DateTime timestamp)
        {
            httpApi.Post($"https://conversations-v1.curseapp.net/conversations/{conversationId}/{serverId}-{timestamp.ToTimestamp()}/unlike");
        }
    }
}
