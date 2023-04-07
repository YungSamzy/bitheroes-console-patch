using System;
using Newtonsoft.Json;
using UnityEngine;

internal class Color32Converter : JsonConverter
{
	public override bool CanRead => false;

	public override bool CanConvert(Type objectType)
	{
		return objectType == typeof(Color32);
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
	}

	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		Color32 color = (Color32)value;
		writer.WriteStartObject();
		writer.WritePropertyName("r");
		writer.WriteValue(color.r);
		writer.WritePropertyName("g");
		writer.WriteValue(color.g);
		writer.WritePropertyName("b");
		writer.WriteValue(color.b);
		writer.WritePropertyName("a");
		writer.WriteValue(color.a);
		writer.WriteEndObject();
	}
}
