using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.news;

public class NewsBook
{
	private static string _version;

	private static string _notes;

	private static string _viewed;

	private static List<NewsRef> _news;

	public static string VERSION => _version;

	public static string NOTES => _notes;

	public static string VIEWED => _viewed;

	public static int size => _news.Count;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_version = XMLBook.instance.newsBook.version;
		_notes = Util.parseMultiLine(Language.GetString(XMLBook.instance.newsBook.notes));
		_news = new List<NewsRef>();
		foreach (ShopBookData.Promo item in XMLBook.instance.newsBook.promos.lstPromo)
		{
			_news.Add(new NewsRef(item.id, item));
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static NewsRef Lookup(int id)
	{
		if (id < 0 || id > size)
		{
			return null;
		}
		return _news[id];
	}

	public static void SetViewed(string version)
	{
		_viewed = version;
	}
}
