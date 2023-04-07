using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

public static class JsonUtil
{
	public const string ASSEMBLY_SUFFIX = ", Assembly-CSharp";

	public static readonly JsonSerializerSettings settings = new JsonSerializerSettings
	{
		Converters = new List<JsonConverter>
		{
			new BoundsConverter(),
			new ColorConverter(),
			new Color32Converter(),
			new RectConverter(),
			new StringEnumConverter(),
			new Vector2Converter(),
			new Vector3Converter(),
			new Vector4Converter()
		},
		DefaultValueHandling = DefaultValueHandling.Ignore,
		NullValueHandling = NullValueHandling.Ignore,
		ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
		TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
		TypeNameHandling = TypeNameHandling.Auto
	};

	public static readonly JsonSerializerSettings settingsShowNull = new JsonSerializerSettings
	{
		Converters = new List<JsonConverter>
		{
			new BoundsConverter(),
			new ColorConverter(),
			new Color32Converter(),
			new RectConverter(),
			new StringEnumConverter(),
			new Vector2Converter(),
			new Vector3Converter(),
			new Vector4Converter()
		},
		DefaultValueHandling = DefaultValueHandling.Include,
		NullValueHandling = NullValueHandling.Include,
		ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
		TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
		TypeNameHandling = TypeNameHandling.Auto
	};

	private static readonly Regex m_typeWithoutAssemblyRegex = new Regex("(\"\\$type\"\\s*:\\s*\"\\w+)(\")");

	private static readonly string m_typeWithAssemblyReplacement = "$1, Assembly-CSharp$2";

	private static readonly Regex m_unpackedObjectRegex = new Regex("\\{[^\\{\\}\\[\\]]+\\}");

	public static object DeserializeObject(Type type, string value)
	{
		object result = null;
		if (type != null)
		{
			if (!string.IsNullOrEmpty(value))
			{
				value = InjectAssemblySuffixes(value);
				try
				{
					result = JsonConvert.DeserializeObject(value, type, settings);
					return result;
				}
				catch (Exception ex)
				{
					Debug.LogErrorFormat("Failed to deserialize {0}! {1}", type.Name, ex.Message);
					return result;
				}
			}
		}
		else
		{
			Debug.LogWarning("Cannot deserialize; type is null.");
		}
		return result;
	}

	public static T DeserializeObject<T>(string value)
	{
		return (T)DeserializeObject(typeof(T), value);
	}

	public static string SerializeObject(object value, Formatting formatting = Formatting.None)
	{
		return JsonConvert.SerializeObject(value, formatting, settings);
	}

	public static string SerializeObjectShowNull(object value, Formatting formatting = Formatting.None)
	{
		return JsonConvert.SerializeObject(value, formatting, settingsShowNull);
	}

	public static string SerializeObjectIndented(object obj)
	{
		return SerializeObject(obj, Formatting.Indented);
	}

	public static string SerializeAndPackObject(object value)
	{
		string text = SerializeObjectIndented(value);
		if (!string.IsNullOrEmpty(text))
		{
			text = text.Replace("  ", "\t");
			text = StripAssemblySuffixes(text);
			text = PackObjects(text);
		}
		return text;
	}

	public static string InjectAssemblySuffixes(string input)
	{
		string result = input;
		if (m_typeWithoutAssemblyRegex != null && !string.IsNullOrEmpty(input))
		{
			result = m_typeWithoutAssemblyRegex.Replace(input, m_typeWithAssemblyReplacement);
		}
		return result;
	}

	public static string StripAssemblySuffixes(string input)
	{
		string result = input;
		if (!string.IsNullOrEmpty(", Assembly-CSharp"))
		{
			result = input.Replace(", Assembly-CSharp", "");
		}
		return result;
	}

	public static string PackObjects(string input)
	{
		string text = input;
		if (m_unpackedObjectRegex != null && !string.IsNullOrEmpty(input))
		{
			MatchCollection matchCollection = m_unpackedObjectRegex.Matches(text);
			if (matchCollection != null)
			{
				foreach (Match item in matchCollection)
				{
					string value = item.Value;
					string newValue = value.Replace("\n", " ").Replace("\t", "");
					text = text.Replace(value, newValue);
				}
				return text;
			}
		}
		return text;
	}
}
