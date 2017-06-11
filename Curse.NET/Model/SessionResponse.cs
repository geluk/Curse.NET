using System;
using System.CodeDom;
using Newtonsoft.Json;

namespace Curse.NET.Model
{
	public class SessionResponse
	{
		public string SessionID { get; set; }
		public string MachineKey { get; set; }
		public SessionUser User { get; set; }
		public int[] Platforms { get; set; }
		public string NotificationServiceUrl { get; set; }
	}

	public class SessionUser
	{
		public int UserID { get; set; }
		public string Username { get; set; }
		public int ConnectionStatus { get; set; }
		public object CustomStatusMessage { get; set; }
		public DateTime CustomStatusTimestamp { get; set; }
		public int FriendCount { get; set; }
		public object AvatarUrl { get; set; }
		public int CurrentGameID { get; set; }
		public int CurrentGameState { get; set; }
		public object CurrentGameStatusMessage { get; set; }
		public DateTime CurrentGameTimestamp { get; set; }
		public int GroupMessagePushPreference { get; set; }
		public int FriendMessagePushPreference { get; set; }
		public bool FriendRequestPushEnabled { get; set; }
		public object MentionsPushEnabled { get; set; }
		[JsonConverter(typeof(MillisecondEpochConverter))]
		public DateTime AvatarTimestamp { get; set; }
	}
}
