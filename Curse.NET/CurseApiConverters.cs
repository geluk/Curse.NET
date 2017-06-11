using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Curse.NET
{
	internal class MillisecondEpochConverter : DateTimeConverterBase
	{
		private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteRawValue(((int)(((DateTime)value - epoch).TotalMilliseconds)).ToString());
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.Value == null || (long) reader.Value == 0)
			{
				return DateTime.MinValue;
			}
			return epoch.AddMilliseconds((long)reader.Value);
		}
	}

	/// <summary>
	/// Converter for JSON integer properties that are set to null when they should've been set to zero.
	/// </summary>
	internal class NullableIntConverter : DateTimeConverterBase
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteRawValue(((int)value).ToString());
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.Value == null)
			{
				return 0;
			}
			// Double cast is necessary to convert a boxed long first to a long, then to an int.
			return (int)(long)reader.Value;
		}
	}
}
