using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.brawl;

public class BrawlBook
{
	private static Dictionary<int, BrawlRef> _brawls;

	private static Dictionary<string, BrawlDifficultyRef> _difficulties;

	public static int size => _brawls.Count;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_brawls = new Dictionary<int, BrawlRef>();
		_difficulties = new Dictionary<string, BrawlDifficultyRef>();
		foreach (BrawlBookData.Difficulty item in XMLBook.instance.brawlBook.difficulties.lstDifficulty)
		{
			BrawlDifficultyRef brawlDifficultyRef = new BrawlDifficultyRef(item.id, item);
			brawlDifficultyRef.LoadDetails(item);
			_difficulties.Add(item.link, brawlDifficultyRef);
		}
		foreach (BrawlBookData.Brawl item2 in XMLBook.instance.brawlBook.brawls.lstBrawl)
		{
			BrawlRef brawlRef = new BrawlRef(item2.id, item2);
			brawlRef.LoadDetails(item2);
			_brawls.Add(item2.id, brawlRef);
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static BrawlRef Lookup(int id)
	{
		if (_brawls.ContainsKey(id))
		{
			return _brawls[id];
		}
		return null;
	}

	public static BrawlDifficultyRef LookupDifficultyLink(string link)
	{
		if (link != null && _difficulties.ContainsKey(link))
		{
			return _difficulties[link];
		}
		return null;
	}

	public static BrawlRef GetFirstBrawl()
	{
		using (Dictionary<int, BrawlRef>.Enumerator enumerator = _brawls.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				return enumerator.Current.Value;
			}
		}
		return null;
	}
}
