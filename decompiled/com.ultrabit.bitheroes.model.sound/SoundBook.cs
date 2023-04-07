using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.sound;

public class SoundBook
{
	private static Dictionary<int, SoundPoolRef> _pools;

	private static Dictionary<string, SoundRef> _sounds;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_pools = new Dictionary<int, SoundPoolRef>();
		_sounds = new Dictionary<string, SoundRef>();
		foreach (SoundBookData.Sound item in XMLBook.instance.soundBook.lstSound)
		{
			if (!_sounds.ContainsKey(item.link))
			{
				_sounds.Add(item.link, new SoundRef(item));
			}
		}
		foreach (SoundBookData.SoundPool item2 in XMLBook.instance.soundBook.pool)
		{
			_pools.Add(item2.id, new SoundPoolRef(item2.id, item2));
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static SoundRef Lookup(string link)
	{
		if (_sounds == null)
		{
			return null;
		}
		if (link != null && _sounds.ContainsKey(link))
		{
			return _sounds[link];
		}
		return null;
	}

	public static SoundPoolRef LookupPool(int id)
	{
		if (id < 0 || id >= _pools.Count)
		{
			return null;
		}
		return _pools[id];
	}

	public static SoundPoolRef LookupPoolLink(string link)
	{
		if (link == null)
		{
			return null;
		}
		foreach (KeyValuePair<int, SoundPoolRef> pool in _pools)
		{
			if (pool.Value.link.Equals(link))
			{
				return pool.Value;
			}
		}
		return null;
	}
}
