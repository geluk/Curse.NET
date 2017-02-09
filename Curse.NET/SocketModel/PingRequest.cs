using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curse.NET.SocketModel
{
	class PingRequest : RequestBody
	{
		public bool Signal { get; set; }

		public static SocketRequest Create()
		{
			return new SocketRequest
			{
				TypeID = RequestType.Ping,
				Body = new PingRequest
				{
					Signal = true
				}
			};
		}
	}
}
