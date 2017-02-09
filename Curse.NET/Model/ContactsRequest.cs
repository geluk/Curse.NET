using System;
using Curse.NET.SocketModel;

namespace Curse.NET.Model
{

	public class ContactsRequest : RequestObject
	{
		public Friend[] Friends { get; set; }
		public Group[] Groups { get; set; }
	}

	public class Friend
	{
		public FriendStatus Status { get; set; }
		public int OtherUserID { get; set; }
		public string OtherUsername { get; set; }
		public string OtherUserNickname { get; set; }
		public RegionId OtherUserRegionID { get; set; }
		public int OtherUserConnectionStatus { get; set; }
		public string InvitationMessage { get; set; }
		public bool IsFavorite { get; set; }
		public string OtherUserStatusMessage { get; set; }
		public int OtherUserGameID { get; set; }
		public object OtherUserGameStatusMessage { get; set; }
		public int OtherUserGameState { get; set; }
		public DateTime OtherUserGameTimestamp { get; set; }
		public string OtherUserAvatarUrl { get; set; }
		public DateTime DateConfirmed { get; set; }
		public DateTime DateMessaged { get; set; }
		public DateTime DateRead { get; set; }
		public int UnreadCount { get; set; }
		public int MutualFriendCount { get; set; }
		public long OtherUserConnectionStatusTimestamp { get; set; }
		public long RequestedTimestamp { get; set; }
		public long AvatarTimestamp { get; set; }

		public override string ToString() => $"{OtherUserNickname ?? OtherUsername} ({Status})";
	}

	public enum FriendStatus
	{
		FriendRequest = 0,
		Friend = 2
	}

	public class Group
	{
		public string GroupTitle { get; set; }
		public string GroupID { get; set; }
		public RegionId HomeRegionID { get; set; }
		public string HomeRegionKey { get; set; }
		public string ParentGroupID { get; set; }
		public string RootGroupID { get; set; }
		public object VoiceSessionCode { get; set; }
		public object MessageOfTheDay { get; set; }
		public int GroupType { get; set; }
		public int GroupSubtype { get; set; }
		public int DisplayOrder { get; set; }
		public bool MetaDataOnly { get; set; }
		public bool AllowTemporaryChildGroups { get; set; }
		public bool ForcePushToTalk { get; set; }
		public int Status { get; set; }
		public bool IsDefaultChannel { get; set; }
		public object Roles { get; set; }
		public Rolepermissions RolePermissions { get; set; }
		public ServerMembership Membership { get; set; }
		public int MemberCount { get; set; }
		public object Emotes { get; set; }
		public object Members { get; set; }
		public Channel[] Channels { get; set; }
		public int GroupMode { get; set; }
		public bool IsPublic { get; set; }
		public string UrlPath { get; set; }
		public string UrlHost { get; set; }
		public bool ChatThrottleEnabled { get; set; }
		public int ChatThrottleSeconds { get; set; }
		public bool IsStreaming { get; set; }
		public object LinkedCommunities { get; set; }
		public int AfkTimerMins { get; set; }
		public long AvatarTimestamp { get; set; }
		public bool FlaggedAsInappropriate { get; set; }
		public int MembersOnline { get; set; }
		public bool HideNoAccess { get; set; }
		public bool HideCallMembersNoAccess { get; set; }
		public object LinkedGuilds { get; set; }
		public object ExternalChannelID { get; set; }

		public override string ToString() => $"{GroupTitle}";
	}

	public class Rolepermissions
	{
		public int _1 { get; set; }
		public int _2 { get; set; }
		public int _3 { get; set; }
		public int _4 { get; set; }
		public int _5 { get; set; }
		public int _6 { get; set; }
		public int _7 { get; set; }
		public int _8 { get; set; }
		public int _9 { get; set; }
		public int _10 { get; set; }
	}

	public class ServerMembership
	{
		public string Nickname { get; set; }
		public bool CanChangeNickname { get; set; }
		public int BestRole { get; set; }
		public int[] Roles { get; set; }
		public DateTime DateJoined { get; set; }
		public DateTime DateMessaged { get; set; }
		public DateTime DateRead { get; set; }
		public DateTime DateRemoved { get; set; }
		public int UnreadCount { get; set; }
		public bool IsFavorite { get; set; }
		public int NotificationPreference { get; set; }
		public object[] NotificationFilters { get; set; }
		public DateTime NotificationMuteDate { get; set; }
		public bool IsVoiceMuted { get; set; }
		public bool IsVoiceDeafened { get; set; }
		public bool IsBanned { get; set; }
	}

	public class Channel
	{
		public string GroupTitle { get; set; }
		public string GroupID { get; set; }
		public string ParentGroupID { get; set; }
		public string RootGroupID { get; set; }
		public string VoiceSessionCode { get; set; }
		public string MessageOfTheDay { get; set; }
		public int GroupMode { get; set; }
		public int GroupType { get; set; }
		public int GroupStatus { get; set; }
		public int DisplayOrder { get; set; }
		public string DisplayCategoryID { get; set; }
		public string DisplayCategory { get; set; }
		public int DisplayCategoryRank { get; set; }
		public bool AllowTemporaryChildGroups { get; set; }
		public bool ForcePushToTalk { get; set; }
		public bool IsDefaultChannel { get; set; }
		public Rolepermissions RolePermissions { get; set; }
		public bool IsPublic { get; set; }
		public ChannelMembership Membership { get; set; }
		public string UrlPath { get; set; }
		public object VoiceMembers { get; set; }
		public bool HideNoAccess { get; set; }
		public bool HideCallMembersNoAccess { get; set; }
		public string ExternalChannelID { get; set; }

		public override string ToString() => $"{GroupTitle}";
	}

	public class ChannelMembership
	{
		public DateTime DateMessaged { get; set; }
		public DateTime DateRead { get; set; }
		public int UnreadCount { get; set; }
		public bool IsFavorite { get; set; }
		public int NotificationPreference { get; set; }
		public object[] NotificationFilters { get; set; }
		public DateTime NotificationMuteDate { get; set; }
	}



}
