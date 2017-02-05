using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Curse.NET.SocketModel
{
	public class UserActivityChangeResponse : ResponseBody
	{
		public string GroupID { get; set; }
		public UserActivityChange[] Users { get; set; }
	}

	public class UserActivityChange
	{
		public int UserID { get; set; }
		public int ConnectionStatus { get; set; }
		public int GameID { get; set; }
		[JsonConverter(typeof(MicrosecondEpochConverter))]
		public DateTime DateLastSeen { get; set; }
		public bool IsActive { get; set; }
	}
}
