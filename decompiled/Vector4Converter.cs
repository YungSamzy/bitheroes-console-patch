using System;
using Newtonsoft.Json;
using UnityEngine;

public class Vector4Converter : JsonConverter
{
	public override bool CanRead => false;

	public override bool CanConvert(Type objectType)
	{
		return objectType == typeof(Vector4);
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
	}

	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		Vector4 vector = (Vector4)value;
		writer.WriteStartObject();
		writer.WritePropertyName("w");
		writer.WriteValue(vector.w);
		writer.WritePropertyName("x");
		writer.WriteValue(vector.x);
		writer.WritePropertyName("y");
		writer.WriteValue(vector.y);
		writer.WritePropertyName("z");
		writer.WriteValue(vector.z);
		writer.WriteEndObject();
	}
}
