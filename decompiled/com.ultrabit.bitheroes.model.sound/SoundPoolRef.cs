using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.sound;

[DebuggerDisplay("{link} (SoundPoolRef)")]
public class SoundPoolRef : IEquatable<SoundPoolRef>, IComparable<SoundPoolRef>
{
	public const string DAMAGE = "damage";

	private int _id;

	private string _link;

	private List<SoundRef> _sounds;

	public int id => _id;

	public string link => _link;

	public SoundPoolRef(int id, SoundBookData.SoundPool poolData)
	{
		_id = id;
		_link = poolData.link;
		if (poolData.sounds != null)
		{
			_sounds = new List<SoundRef>();
			string[] stringArrayFromStringProperty = Util.GetStringArrayFromStringProperty(poolData.sounds);
			foreach (string text in stringArrayFromStringProperty)
			{
				_sounds.Add(SoundBook.Lookup(text));
			}
		}
	}

	public SoundRef getRandomSound()
	{
		if (_sounds == null || _sounds.Count <= 0)
		{
			return null;
		}
		return _sounds[Util.randomInt(0, _sounds.Count - 1)];
	}

	public bool Equals(SoundPoolRef other)
	{
		if (other == null)
		{
			return false;
		}
		return id.Equals(other.id);
	}

	public int CompareTo(SoundPoolRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return id.CompareTo(other.id);
	}
}
