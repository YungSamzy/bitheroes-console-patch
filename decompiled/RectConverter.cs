using System;
using Newtonsoft.Json;
using UnityEngine;

public class RectConverter : JsonConverter
{
	public override bool CanRead => false;

	public override bool CanConvert(Type objectType)
	{
		return objectType == typeof(Rect);
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
	}

	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		Rect rect = (Rect)value;
		writer.WriteStartObject();
		writer.WritePropertyName("x");
		writer.WriteValue(rect.x);
		writer.WritePropertyName("y");
		writer.WriteValue(rect.y);
		writer.WritePropertyName("width");
		writer.WriteValue(rect.width);
		writer.WritePropertyName("height");
		writer.WriteValue(rect.height);
		writer.WriteEndObject();
	}
}
