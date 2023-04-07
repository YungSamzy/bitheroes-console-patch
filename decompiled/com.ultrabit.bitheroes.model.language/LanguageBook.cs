using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.language;

public class LanguageBook
{
	private static List<LanguageRef> _languages;

	public static int size => _languages.Count;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_languages = new List<LanguageRef>();
		foreach (LanguageBookData.Language language in XMLBook.instance.languageBook.languages)
		{
			_languages.Add(new LanguageRef(language.id, language));
			if (XMLBook.instance.UpdateProcessingCount())
			{
				yield return null;
			}
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static LanguageRef Lookup(int i)
	{
		if (i < 0 || i > size)
		{
			return null;
		}
		return _languages[i];
	}

	public static LanguageRef GetCurrentLanguageRef()
	{
		foreach (LanguageRef language in _languages)
		{
			if (language.lang.Equals(AppInfo.GetLanguage()))
			{
				return language;
			}
		}
		return null;
	}
}
