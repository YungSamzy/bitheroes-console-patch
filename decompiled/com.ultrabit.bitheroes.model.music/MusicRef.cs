using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.audio;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.music;

[DebuggerDisplay("{link} (MusicRef)")]
public class MusicRef : AudioRef, IEquatable<MusicRef>, IComparable<MusicRef>
{
	public const string LINK_MENU = "menu";

	public const string LINK_BATTLE = "battle";

	public const string LINK_BOSS = "boss";

	public const string LINK_DUNGEON = "dungeon";

	public const string LINK_VICTORY = "victory";

	public MusicRef(MusicBookData.Music musicData)
		: base(musicData.link, AssetURL.GetPath(AssetURL.AUDIO_MUSIC, musicData.url), musicData.volume, musicData.duration, null, musicData.value, musicData.loadLocal, musicData.loadImmediately)
	{
	}

	public bool Equals(MusicRef other)
	{
		if (other == null)
		{
			return false;
		}
		return Equals((AudioRef)other);
	}

	public int CompareTo(MusicRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return CompareTo((AudioRef)other);
	}
}
