using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.music;

public class MusicBook
{
	private static List<MusicRef> _music;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_music = new List<MusicRef>();
		foreach (MusicBookData.Music item in XMLBook.instance.musicBook.lstMusic)
		{
			_music.Add(new MusicRef(item));
		}
		yield return null;
		if (onUpdatedProgress != null && onUpdatedProgress.Target != null && !onUpdatedProgress.Target.Equals(null))
		{
			onUpdatedProgress(XMLBook.instance.UpdateProgress());
		}
	}

	public static MusicRef Lookup(string link)
	{
		if (link == null)
		{
			return null;
		}
		foreach (MusicRef item in _music)
		{
			if (item.link.Equals(link))
			{
				return item;
			}
		}
		return null;
	}
}
