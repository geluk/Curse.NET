using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curse.NET
{
	public class CurseDotNetException :Exception
	{
		public CurseDotNetException(string message) : base(message) { }
	}

	public class CurseWebSocketException : CurseDotNetException
	{
		public CurseWebSocketException(string message) : base(message) { }
	}

	public class LoginException : CurseDotNetException
	{
		public int StatusCode { get; }

		public LoginException(string message, int statusCode) : base(message)
		{
			StatusCode = statusCode;
		}
	}
}
