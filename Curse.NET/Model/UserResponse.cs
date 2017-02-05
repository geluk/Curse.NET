using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Curse.NET.Model
{
	public class UserResponse
	{
		public int UserID { get; set; }
		public string Username { get; set; }
		public string Nickname { get; set; }
		public int BestRole { get; set; }
		public int[] Roles { get; set; }
		[JsonConverter(typeof(MillisecondEpochConverter))]
		public DateTime DateJoined { get; set; }
		public int ConnectionStatus { get; set; }
		[JsonConverter(typeof(MillisecondEpochConverter))]
		public DateTime DateLastSeen { get; set; }
		[JsonConverter(typeof(MillisecondEpochConverter))]
		public DateTime DateLastActive { get; set; }
		[JsonConverter(typeof(MillisecondEpochConverter))]
		public DateTime DateRemoved { get; set; }
		public bool IsActive { get; set; }
		public int CurrentGameID { get; set; }
		public bool IsVoiceMuted { get; set; }
		public bool IsVoiceDeafened { get; set; }
		[JsonConverter(typeof(MillisecondEpochConverter))]
		public DateTime AvatarTimestamp { get; set; }
		public Externalaccount[] ExternalAccounts { get; set; }
		public bool IsVerified { get; set; }
		public bool IsBanned { get; set; }

		public override string ToString() => $"{Nickname ?? Username}";
	}

	public class Externalaccount
	{
		public string ExternalID { get; set; }
		public string ExternalName { get; set; }
		public string ExternalDisplayName { get; set; }
		public int Type { get; set; }
	}

}
