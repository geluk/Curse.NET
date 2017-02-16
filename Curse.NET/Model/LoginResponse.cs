using System;
using Newtonsoft.Json;

namespace Curse.NET.Model
{
	public class LoginResponse
	{
		public int Status { get; set; }
		public string StatusMessage { get; set; }
		public LoginSession Session { get; set; }
		public long Timestamp { get; set; }
	}

	public class LoginSession
	{
		public int UserID { get; set; }
		public string Username { get; set; }
		public string SessionID { get; set; }
		public string Token { get; set; }
		public string EmailAddress { get; set; }
		public bool EffectivePremiumStatus { get; set; }
		public bool ActualPremiumStatus { get; set; }
		public int SubscriptionToken { get; set; }
		[JsonConverter(typeof(MillisecondEpochConverter))]
		public DateTime Expires { get; set; }
		[JsonConverter(typeof(MillisecondEpochConverter))]
		public DateTime RenewAfter { get; set; }
		public bool IsTemporaryAccount { get; set; }
	}
}
