using System;
using Curse.NET.ExtensionMethods;

namespace Curse.NET.Model
{
	public static class ModelExtensions
	{
		internal static CurseApi Api { get; set; }

		public static Message[] GetMessages(this Channel channel, DateTime start, DateTime end, int pageSize)
		{
			var rs = Api.Get<Message[]>($"https://conversations-v1.curseapp.net/conversations/{channel.GroupID}" +
			        $"?startTimestamp={start.ToTimestamp()}" +
			        $"&endTimestamp={end.ToTimestamp()}" +
			        $"&pageSize={pageSize}");

			return rs;
		}

		public static UserResponse LookupUser(this Group group, int userId)
		{
			var rs = Api.Get<UserResponse>($"https://groups-v1.curseapp.net/groups/{group.GroupID}/members/{userId}");
			return rs;
		}
	}
}
