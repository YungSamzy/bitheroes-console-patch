using System;
using com.ultrabit.bitheroes.model.utility;

public class XMLDLCConfig
{
	private static string[] _whiteList = new string[1] { "SoundBook.xml" };

	public static void OverrideWhiteList(string[] whitelist)
	{
		int num = _whiteList.Length;
		_whiteList = whitelist;
		D.Log("all", $"Overriding XML DLC Whitelist, previous count: {num} - new count: {_whiteList.Length}");
	}

	public static string[] GetWhitelist()
	{
		if (_whiteList != null && _whiteList.Length != 0)
		{
			ProcessWhiteList();
			return _whiteList;
		}
		return Array.Empty<string>();
	}

	public static bool ContainsKey(string key)
	{
		return Array.Exists(_whiteList, (string element) => element.Equals(key));
	}

	private static void ProcessWhiteList()
	{
		for (int i = 0; i < _whiteList.Length; i++)
		{
			if (_whiteList[i].StartsWith("Zone_"))
			{
				_whiteList[i] = "zones/" + _whiteList[i];
			}
		}
	}
}
