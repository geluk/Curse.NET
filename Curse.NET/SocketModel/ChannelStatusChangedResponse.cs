using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curse.NET.SocketModel
{
	public class ChannelStatusChangedResponse : ResponseBody
	{
		public string GroupID { get; set; }
		/// <summary>
		/// 1 if the channel is muted, 0 if it is not muted
		/// </summary>
		public int Preference { get; set; }
		public bool IsFavorite { get; set; }
		public DateTime Timestamp { get; set; }
	}
}
