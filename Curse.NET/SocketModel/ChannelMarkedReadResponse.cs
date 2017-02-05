using System;

namespace Curse.NET.SocketModel
{
	public class ChannelMarkedReadResponse : ResponseBody
	{
		public string GroupID { get; set; }
		public int FriendID { get; set; }
		/// <summary>
		/// The timestamp of the last message in the channel
		/// </summary>
		public DateTime Timestamp { get; set; }
		/// <summary>
		/// The Conversation ID of the channel that was marked
		/// </summary>
		public string ConversationID { get; set; }
	}
}
