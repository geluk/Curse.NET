using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Curse.NET.SocketModel
{
	public class SocketResponse
	{
		public ResponseType TypeID { get; set; }
		public ResponseBody Body { get; set; }

		internal static SocketResponse Deserialise(string message)
		{
			var body = JObject.Parse(message)["Body"].ToString();
			var obj = JsonConvert.DeserializeObject<SocketResponse>(message);
			switch (obj.TypeID)
			{
				case ResponseType.ChatMessage:
					obj.Body = JsonConvert.DeserializeObject<MessageResponse>(body);
					break;
				case ResponseType.Login:
					obj.Body = JsonConvert.DeserializeObject<LoginResponse>(body);
					break;
				case ResponseType.UnknownChange:
					obj.Body = JsonConvert.DeserializeObject<UnknownChangeResponse>(body);
					break;
				case ResponseType.UserActivityChange:
					break;
				case ResponseType.ChannelReference:
					obj.Body = JsonConvert.DeserializeObject<ChannelReferenceResponse>(body);
					break;
				default:
					break;
			}
			return obj;
		}
	}
}
