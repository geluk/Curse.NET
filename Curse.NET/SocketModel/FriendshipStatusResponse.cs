using System;
using Newtonsoft.Json;

namespace Curse.NET.SocketModel
{

	public class FriendshipStatusResponse : ResponseBody
	{
		public Friendship Friendship { get; set; }
	}

	public class Friendship
	{
		public FriendshipStatus Status { get; set; }
		public int OtherUserID { get; set; }
		public string OtherUsername { get; set; }
		public string OtherUserNickname { get; set; }
		public RegionId OtherUserRegionID { get; set; }
		public int OtherUserConnectionStatus { get; set; }
		public object InvitationMessage { get; set; }
		public bool IsFavorite { get; set; }
		public object OtherUserStatusMessage { get; set; }
		public int OtherUserGameID { get; set; }
		public object OtherUserGameStatusMessage { get; set; }
		public int OtherUserGameState { get; set; }
		public DateTime OtherUserGameTimestamp { get; set; }
		public object OtherUserAvatarUrl { get; set; }
		public DateTime DateConfirmed { get; set; }
		public DateTime DateMessaged { get; set; }
		public DateTime DateRead { get; set; }
		public int UnreadCount { get; set; }
		public int MutualFriendCount { get; set; }
		[JsonConverter(typeof(MillisecondEpochConverter))]
		public DateTime OtherUserConnectionStatusTimestamp { get; set; }
		[JsonConverter(typeof(MillisecondEpochConverter))]
		public DateTime RequestedTimestamp { get; set; }
		[JsonConverter(typeof(MillisecondEpochConverter))]
		public DateTime AvatarTimestamp { get; set; }

		//public bool IsRequest => Status == FriendshipStatus.RequestReceived && DateConfirmed == DateTime.MinValue;
	}

	public enum FriendshipStatus
	{
		RequestReceived = 0,
		RequestSent = 1,
		Friend = 2
	}
}
