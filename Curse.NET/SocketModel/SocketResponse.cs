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
					obj.Body = JsonConvert.DeserializeObject<SocketLoginResponse>(body);
					break;
				case ResponseType.MessageChanged:
					obj.Body = JsonConvert.DeserializeObject<MessageChangedResponse>(body);
					break;
				case ResponseType.UserActivityChange:
					obj.Body = JsonConvert.DeserializeObject<UserActivityChangeResponse>(body);
					break;
				case ResponseType.ChannelMarkedRead:
					obj.Body = JsonConvert.DeserializeObject<ChannelMarkedReadResponse>(body);
					break;
				case ResponseType.Unknown1:
					break;
				case ResponseType.ChannelStatusChanged:
					obj.Body = JsonConvert.DeserializeObject<ChannelStatusChangedResponse>(body);
					break;
				default:
					break;
			}
			return obj;
		}
	}
}
