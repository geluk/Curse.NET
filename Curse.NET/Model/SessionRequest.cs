using System;

namespace Curse.NET.Model
{
	public class SessionRequest : RequestObject
	{
		public string MachineKey { get; private set; }
		public DevicePlatform Platform { get; private set; }
		public string DeviceID { get; private set; }
		public string PushKitToken { get; private set; }

		public static SessionRequest Create()
		{
			return new SessionRequest
			{
				MachineKey = Guid.NewGuid().ToString(),
				Platform = DevicePlatform.Chrome,
			};
		}
	}
}
