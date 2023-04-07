using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.core;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.audio;

[DebuggerDisplay("{link} (AudioRef)")]
public class AudioRef : IEquatable<AudioRef>, IComparable<AudioRef>
{
	private string _link;

	private string _url;

	private float _volume;

	private int _duration;

	private string _value;

	private bool _loadLocal;

	private AudioClip _sound;

	private bool _loaded;

	private bool _local;

	public string link => _link;

	public string url => _url;

	public float volume => _volume;

	public AudioClip sound => _sound;

	public string value => _value;

	public bool loaded => _loaded;

	public bool local => _local;

	public float duration
	{
		get
		{
			if (_duration > 0)
			{
				return _duration / 1000;
			}
			if (!(_sound != null))
			{
				return 0f;
			}
			return _sound.length;
		}
	}

	public AudioRef(string link, string url, float volume, int duration, AudioClip sound, string value, bool loadLocal, bool loadImmediately)
	{
		_link = link;
		_url = url;
		_volume = volume;
		_duration = duration;
		_sound = sound;
		_value = value;
		_loadLocal = loadLocal;
		if (_sound != null)
		{
			_loaded = true;
		}
		else if (loadImmediately)
		{
			Load();
		}
	}

	public void Load(bool local = true)
	{
		if (!(_sound != null))
		{
			_sound = GameData.instance.main.assetLoader.GetAudioClip(url);
			_loaded = true;
		}
	}

	public void Destroy()
	{
		if (!(_sound == null))
		{
			_sound = null;
			_loaded = false;
		}
	}

	public bool Equals(AudioRef other)
	{
		if (other == null)
		{
			return false;
		}
		return link.Equals(other.link);
	}

	public int CompareTo(AudioRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return link.CompareTo(other.link);
	}
}
