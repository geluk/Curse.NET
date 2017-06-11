using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curse.NET.Model
{
	public class ContactResponse : RequestObject
	{
		public int UserID { get; set; }
		public string Username { get; set; }
		public object DisplayName { get; set; }
		public string Name { get; set; }
		public object City { get; set; }
		public object State { get; set; }
		public string CountryCode { get; set; }
		public object AboutMe { get; set; }
		public int FriendCount { get; set; }
		public int LastGameID { get; set; }
		public object[] Identities { get; set; }
		public object[] MutualFriends { get; set; }
		public string[] MutualGroupIDs { get; set; }
	}
}
