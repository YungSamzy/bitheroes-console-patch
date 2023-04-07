using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.filter;

public class FilterBook
{
	private static List<FilterRef> _WORDS;

	private static List<string> _ALLOW;

	private static List<FilterRef> _FORBIDDEN_NAMES;

	public static List<FilterRef> words => _WORDS;

	public static List<string> allow => _ALLOW;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		if (_WORDS == null)
		{
			_WORDS = new List<FilterRef>();
		}
		else
		{
			_WORDS.Clear();
		}
		if (_ALLOW == null)
		{
			_ALLOW = new List<string>();
		}
		else
		{
			_ALLOW.Clear();
		}
		if (_FORBIDDEN_NAMES == null)
		{
			_FORBIDDEN_NAMES = new List<FilterRef>();
		}
		else
		{
			_FORBIDDEN_NAMES.Clear();
		}
		List<string> list = new List<string>();
		list.AddRange(XMLBook.instance.filterBook.lstWord);
		while (list.Count > 0)
		{
			string text = list[0];
			list.RemoveAt(0);
			string text2 = text;
			string text3 = "";
			for (int i = 0; i < text2.Length; i++)
			{
				text3 += "*";
			}
			_WORDS.Add(new FilterRef(text2, text3));
		}
		_ALLOW.AddRange(XMLBook.instance.filterBook.lstAllow);
		List<string> list2 = new List<string>();
		list2.AddRange(XMLBook.instance.forbiddenCharacterName.lstWord);
		while (list2.Count > 0)
		{
			string text4 = list2[0];
			list2.RemoveAt(0);
			string text5 = text4;
			string text6 = "";
			for (int j = 0; j < text5.Length; j++)
			{
				text6 += "*";
			}
			_FORBIDDEN_NAMES.Add(new FilterRef(text5, text6));
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static bool allowWord(string word)
	{
		foreach (string item in _ALLOW)
		{
			if (word.ToLowerInvariant().IndexOf(item.ToLowerInvariant()) >= 0)
			{
				return true;
			}
		}
		return false;
	}

	public static bool allowName(string name)
	{
		foreach (FilterRef fORBIDDEN_NAME in _FORBIDDEN_NAMES)
		{
			if (fORBIDDEN_NAME.word.ToLowerInvariant() == name.ToLowerInvariant())
			{
				return false;
			}
		}
		return true;
	}
}
