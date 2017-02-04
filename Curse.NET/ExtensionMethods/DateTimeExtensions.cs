using System;

namespace Curse.NET.ExtensionMethods
{
	internal static class DateTimeExtensions
	{
		private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public static long ToTimestamp(this DateTime date)
		{
			if (date == DateTime.MinValue) return 0;

			var ms = (date - Epoch).TotalMilliseconds;
			return (long) ms;
		}

		public static DateTime FromTimestamp(long timestamp)
		{
			return Epoch.AddMilliseconds(timestamp);
		}
	}
}
