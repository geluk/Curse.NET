﻿using System;

namespace Curse.NET.SocketModel
{

	public class FriendshipResponse
	{
		public Friendship Friendship { get; set; }
	}

	public class Friendship
	{
		public int Status { get; set; }
		public int OtherUserID { get; set; }
		public string OtherUsername { get; set; }
		public string OtherUserNickname { get; set; }
		public int OtherUserRegionID { get; set; }
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
		public long OtherUserConnectionStatusTimestamp { get; set; }
		public long RequestedTimestamp { get; set; }
		public long AvatarTimestamp { get; set; }
	}

}