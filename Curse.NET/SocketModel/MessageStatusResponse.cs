using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curse.NET.SocketModel
{
	public class MessageStatusResponse : ResponseBody
	{
		public string ConversationID { get; set; }
		public MessageStatus Status { get; set; }
		public string ClientID { get; set; }
		public string ServerID { get; set; }
		public object RetryAfter { get; set; }
		public int ForbiddenReason { get; set; }
		public int MissingPermission { get; set; }
	}

	public enum MessageStatus
	{
		MessageRefused = 2,
		Accepted = 4,
	}
}
