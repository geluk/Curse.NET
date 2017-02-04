namespace Curse.NET.Model
{
	public class ConversationsRequest : RequestObject
	{
		public Message[] Messages { get; set; }
	}

	public class Message
	{
		public object ClientID { get; set; }
		public string ServerID { get; set; }
		public string ConversationID { get; set; }
		public string ContactID { get; set; }
		public int ConversationType { get; set; }
		public string RootConversationID { get; set; }
		public long Timestamp { get; set; }
		public int SenderID { get; set; }
		public string SenderName { get; set; }
		public int SenderPermissions { get; set; }
		public int[] SenderRoles { get; set; }
		public int SenderVanityRole { get; set; }
		public int?[] Mentions { get; set; }
		public int RecipientID { get; set; }
		public string Body { get; set; }
		public bool IsDeleted { get; set; }
		public int DeletedTimestamp { get; set; }
		public int DeletedUserID { get; set; }
		public object DeletedUsername { get; set; }
		public int EditedTimestamp { get; set; }
		public int EditedUserID { get; set; }
		public object EditedUsername { get; set; }
		public int LikeCount { get; set; }
		public object[] LikeUserIDs { get; set; }
		public object[] LikeUsernames { get; set; }
		public int?[] ContentTags { get; set; }
		public object[] Attachments { get; set; }
		public int NotificationType { get; set; }
		public object[] EmoteSubstitutions { get; set; }
		public object ExternalChannelID { get; set; }
		public object ExternalUserID { get; set; }
		public object ExternalUsername { get; set; }
		public object ExternalUserDisplayName { get; set; }
		public object ExternalUserColor { get; set; }
		public object[] Badges { get; set; }
		public int BitsUsed { get; set; }

		public override string ToString() => $"{SenderName}: {Body}";
	}

}
