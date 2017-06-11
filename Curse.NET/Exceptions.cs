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

		public CurseDotNetException(string message, Exception innerException) : base(message, innerException) { }
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

	public class NotAuthorisedException : CurseDotNetException
	{
		public NotAuthorisedException(string message) : base(message) { }
	}

	public class InvalidRequestException : CurseDotNetException
	{
		public InvalidRequestException(string message, Exception innerException) : base(message, innerException) { }
	}
}
