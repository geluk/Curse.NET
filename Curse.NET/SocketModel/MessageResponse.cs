using System;
using Newtonsoft.Json;

namespace Curse.NET.SocketModel
{
	public class MessageResponse : ResponseBody
	{
		public string ClientID { get; set; }
		/// <summary>
		/// Despite its name, this actually seems to be more of a message ID.
		/// </summary>
		public string ServerID { get; set; }
		public string ConversationID { get; set; }
		public string ContactID { get; set; }
		public int ConversationType { get; set; }
		public string RootConversationID { get; set; }
		[JsonConverter(typeof(MillisecondEpochConverter))]
		public DateTime Timestamp { get; set; }
		public int SenderID { get; set; }
		public string SenderName { get; set; }
		public int SenderPermissions { get; set; }
		public int[] SenderRoles { get; set; }
		public int SenderVanityRole { get; set; }
		public int[] Mentions { get; set; }
		public int RecipientID { get; set; }
		public string Body { get; set; }
		public bool IsDeleted { get; set; }
		[JsonConverter(typeof(MillisecondEpochConverter))]
		public DateTime DeletedTimestamp { get; set; }
		public int DeletedUserID { get; set; }
		public object DeletedUsername { get; set; }
		[JsonConverter(typeof(MillisecondEpochConverter))]
		public DateTime EditedTimestamp { get; set; }
		public int EditedUserID { get; set; }
		public object EditedUsername { get; set; }
		public int LikeCount { get; set; }
		public int[] LikeUserIDs { get; set; }
		public string[] LikeUsernames { get; set; }
		public object[] ContentTags { get; set; }
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
