using System;
using Newtonsoft.Json;

namespace Curse.NET.Model
{
	public class CreateInviteResponse
	{
		public string InviteCode { get; set; }
		public int CreatorID { get; set; }
		public string CreatorName { get; set; }
		public string GroupID { get; set; }
		public Group Group { get; set; }
		public string ChannelID { get; set; }
		public Channel Channel { get; set; }
		[JsonConverter(typeof(MillisecondEpochConverter))]
		public DateTime DateCreated { get; set; }
		[JsonConverter(typeof(MillisecondEpochConverter))]
		public DateTime DateExpires { get; set; }
		[JsonConverter(typeof(NullableIntConverter))]
		public int MaxUses { get; set; }
		public int TimesUsed { get; set; }
		public bool IsRedeemable { get; set; }
		public string InviteUrl { get; set; }
		public string AdminDescription { get; set; }
	}
}