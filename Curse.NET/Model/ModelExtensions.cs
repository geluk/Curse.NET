using System;
using Curse.NET.ExtensionMethods;

namespace Curse.NET.Model
{
	public static class ModelExtensions
	{
		internal static CurseApi Api { get; set; }

		public static Message[] GetMessages(this Channel channel, DateTime start, DateTime end, int pageSize)
		{
			var rs = Api.GetMessages(channel.GroupID, start, end, pageSize); 
			return rs;
		}
	}
}
