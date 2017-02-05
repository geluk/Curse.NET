using System;

namespace Curse.NET.SocketModel
{
	public class SocketLoginResponse : ResponseBody
	{
		public int Status { get; set; }
		public DateTime ServerTime { get; set; }
		public object EncryptedSessionKey { get; set; }
	}

}
