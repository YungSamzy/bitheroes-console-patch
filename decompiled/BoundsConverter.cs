using System;
using Newtonsoft.Json;
using UnityEngine;

public class BoundsConverter : JsonConverter
{
	public override bool CanRead => false;

	public override bool CanConvert(Type objectType)
	{
		return objectType == typeof(Bounds);
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
	}

	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		Bounds bounds = (Bounds)value;
		writer.WriteStartObject();
		writer.WritePropertyName("center");
		writer.WriteStartObject();
		writer.WritePropertyName("x");
		writer.WriteValue(bounds.center.x);
		writer.WritePropertyName("y");
		writer.WriteValue(bounds.center.y);
		writer.WritePropertyName("z");
		writer.WriteValue(bounds.center.z);
		writer.WriteEndObject();
		writer.WritePropertyName("extents");
		writer.WriteStartObject();
		writer.WritePropertyName("x");
		writer.WriteValue(bounds.extents.x);
		writer.WritePropertyName("y");
		writer.WriteValue(bounds.extents.y);
		writer.WritePropertyName("z");
		writer.WriteValue(bounds.extents.z);
		writer.WriteEndObject();
		writer.WriteEndObject();
	}
}
