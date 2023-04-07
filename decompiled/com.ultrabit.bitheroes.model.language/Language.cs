using System.Collections.Generic;
using System.Xml;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.language;

public class Language
{
	private static Dictionary<string, string> dictionary = new Dictionary<string, string>();

	public static void Init(XmlDocument xml)
	{
		LanguageXML type = null;
		XMLBook.instance.GetObjectFromXMLDocument(ref type, xml);
		if (dictionary != null)
		{
			dictionary.Clear();
		}
		else
		{
			dictionary = new Dictionary<string, string>();
		}
		foreach (LanguageXML.Item item in type.items)
		{
			string text = ((item.translation != null) ? item.translation : item.translationAttribute);
			string link = GetLink(item.link);
			if (dictionary.ContainsKey(link))
			{
				D.LogWarning("david", "Duplicate Key Language Added to Dictionary " + link + " - Current Value: " + dictionary[link] + " --- New Value: " + text);
			}
			else
			{
				dictionary.Add(item.link.ToLower().Trim(), text);
			}
		}
	}

	private static string GetLink(string link)
	{
		return link?.ToLowerInvariant().Trim();
	}

	public static string GetString(string link, string[] values = null, bool color = false)
	{
		string link2 = GetLink(link);
		if (link2 != null && dictionary.ContainsKey(link2))
		{
			string text = dictionary[link2];
			List<string> list = new List<string>();
			if (values != null && values.Length != 0)
			{
				foreach (string item in values)
				{
					list.Add(item);
				}
			}
			return Util.ParseStringValues(text, list, color);
		}
		return link;
	}

	public static string LoadLangFromDLC(bool assetBundlesEnabled)
	{
		string language = AppInfo.GetLanguage();
		TextAsset textAsset = SingletonMonoBehaviour<AssetManager>.instance.LoadAsset<TextAsset>("languages/" + language + ".xml");
		if (!assetBundlesEnabled && textAsset == null)
		{
			textAsset = (TextAsset)Resources.Load("xml-resources/languages/" + language);
		}
		if (textAsset == null)
		{
			return null;
		}
		return textAsset.text;
	}

	public static string LoadLangFromBundledResources()
	{
		return "";
	}
}
