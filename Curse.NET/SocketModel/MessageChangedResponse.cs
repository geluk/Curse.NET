using System;
using System.Linq;
using Newtonsoft.Json;

namespace Curse.NET.SocketModel
{
	public enum ChangeType
	{
		Deleted = 2,
		ChannelChange = 5,
	}

	public class MessageChangedResponse : ResponseBody
	{
		public ChangeType ChangeType { get; set; }
		public int SenderID { get; set; }
		public string SenderName { get; set; }
		public GroupChange Group { get; set; }
		public Member[] Members { get; set; }
		public DateTime TimeStamp { get; set; }
		public object[] ChildGroups { get; set; }
		public int RemovedReason { get; set; }
		public object MessageToUsers { get; set; }

		public override string ToString() => $"({ChangeType}) Message by {SenderName} on server {Group.GroupTitle}";
	}

	public class GroupChange
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
		public Role[] Roles { get; set; }
		public GroupRolePermissions RolePermissions { get; set; }
		public object Membership { get; set; }
		public int MemberCount { get; set; }
		public object Emotes { get; set; }
		public object Members { get; set; }
		public ChannelChange[] Channels { get; set; }
		public int GroupMode { get; set; }
		public bool IsPublic { get; set; }
		public string UrlPath { get; set; }
		public string UrlHost { get; set; }
		public bool ChatThrottleEnabled { get; set; }
		public int ChatThrottleSeconds { get; set; }
		public bool IsStreaming { get; set; }
		public object LinkedCommunities { get; set; }
		public int AfkTimerMins { get; set; }
		[JsonConverter(typeof(MicrosecondEpochConverter))]
		public DateTime AvatarTimestamp { get; set; }
		public bool FlaggedAsInappropriate { get; set; }
		public int MembersOnline { get; set; }
		public bool HideNoAccess { get; set; }
		public bool HideCallMembersNoAccess { get; set; }
		public object LinkedGuilds { get; set; }
		public object ExternalChannelID { get; set; }
	}

	public class GroupRolePermissions
	{
		public int _1 { get; set; }
		public int _2 { get; set; }
		public int _3 { get; set; }
		public int _4 { get; set; }
		public int _5 { get; set; }
		public int _6 { get; set; }
		public int _7 { get; set; }
		public int _8 { get; set; }
	}

	public class Member
	{
		public int UserID { get; set; }
		public string Username { get; set; }
		public object Nickname { get; set; }
		public int BestRole { get; set; }
		public int[] Roles { get; set; }
		[JsonConverter(typeof(MicrosecondEpochConverter))]
		public DateTime DateJoined { get; set; }
		public int ConnectionStatus { get; set; }
		[JsonConverter(typeof(MicrosecondEpochConverter))]
		public DateTime DateLastSeen { get; set; }
		[JsonConverter(typeof(MicrosecondEpochConverter))]
		public DateTime DateLastActive { get; set; }
		[JsonConverter(typeof(MicrosecondEpochConverter))]
		public DateTime DateRemoved { get; set; }
		public bool IsActive { get; set; }
		public int CurrentGameID { get; set; }
		public bool IsVoiceMuted { get; set; }
		public bool IsVoiceDeafened { get; set; }
		[JsonConverter(typeof(MicrosecondEpochConverter))]
		public DateTime AvatarTimestamp { get; set; }
		public object ExternalAccounts { get; set; }
		public bool IsVerified { get; set; }
		public bool IsBanned { get; set; }
	}

	public class Role
	{
		public int RoleID { get; set; }
		public string Name { get; set; }
		public int Rank { get; set; }
		public int VanityColor { get; set; }
		public int VanityBadge { get; set; }
		public bool HasCustomVanityBadge { get; set; }
		public bool IsDefault { get; set; }
		public bool IsOwner { get; set; }
		public int Tag { get; set; }
		public int Source { get; set; }
		public bool IsHidden { get; set; }
		public object ExternalID { get; set; }
	}

	public class ChannelChange
	{
		public string GroupTitle { get; set; }
		public string GroupID { get; set; }
		public string ParentGroupID { get; set; }
		public string RootGroupID { get; set; }
		public object VoiceSessionCode { get; set; }
		public string MessageOfTheDay { get; set; }
		public int GroupMode { get; set; }
		public int GroupType { get; set; }
		public int GroupStatus { get; set; }
		public int DisplayOrder { get; set; }
		public string DisplayCategoryID { get; set; }
		public object DisplayCategory { get; set; }
		public int DisplayCategoryRank { get; set; }
		public bool AllowTemporaryChildGroups { get; set; }
		public bool ForcePushToTalk { get; set; }
		public bool IsDefaultChannel { get; set; }
		public ChannelRolePermissions RolePermissions { get; set; }
		public bool IsPublic { get; set; }
		public object Membership { get; set; }
		public string UrlPath { get; set; }
		public object VoiceMembers { get; set; }
		public bool HideNoAccess { get; set; }
		public bool HideCallMembersNoAccess { get; set; }
		public object ExternalChannelID { get; set; }
	}

	public class ChannelRolePermissions
	{
		public int _2 { get; set; }
		public int _3 { get; set; }
		public int _1 { get; set; }
	}

}
