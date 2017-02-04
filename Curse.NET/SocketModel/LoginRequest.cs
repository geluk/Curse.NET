namespace Curse.NET.SocketModel
{
	public class LoginRequest : RequestBody
	{
		public int CipherAlgorithm { get; set; }
		public int CipherStrength { get; set; }
		public string ClientVersion { get; set; }
		public object PublicKey { get; set; }
		public string MachineKey { get; set; }
		public int UserID { get; set; }
		public string SessionID { get; set; }

		public static SocketRequest Create(string machineKey, string sessionId, int userID)
		{
			return new SocketRequest
			{
				TypeID = RequestType.Login,
				Body = new LoginRequest
				{
					ClientVersion = "7.0.140",
					MachineKey = machineKey,
					SessionID = sessionId,
					UserID = userID
				}
			};
		}
	}
}
