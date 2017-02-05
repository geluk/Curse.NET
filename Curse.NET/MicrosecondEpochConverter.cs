using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Curse.NET
{
	internal class MicrosecondEpochConverter : DateTimeConverterBase
	{
		private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteRawValue(((int)(((DateTime)value - epoch).TotalMilliseconds * 1000)).ToString());
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.Value == null) { return null; }
			return epoch.AddMilliseconds((long)reader.Value / 1000d);
		}
	}

	internal class MillisecondEpochConverter : DateTimeConverterBase
	{
		private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteRawValue(((int)(((DateTime)value - epoch).TotalMilliseconds)).ToString());
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.Value == null) { return null; }
			return epoch.AddMilliseconds((long)reader.Value);
		}
	}
}
