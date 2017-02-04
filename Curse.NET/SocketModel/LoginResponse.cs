using System;

namespace Curse.NET.SocketModel
{
	public class LoginResponse : ResponseBody
	{
		public int Status { get; set; }
		public DateTime ServerTime { get; set; }
		public object EncryptedSessionKey { get; set; }
	}

}
