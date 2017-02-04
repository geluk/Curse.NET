using System;

namespace Curse.NET.SocketModel
{
	public class ChannelReferenceResponse : ResponseBody
	{
		public string GroupID { get; set; }
		public int FriendID { get; set; }
		public DateTime Timestamp { get; set; }
		public string ConversationID { get; set; }
	}
}
