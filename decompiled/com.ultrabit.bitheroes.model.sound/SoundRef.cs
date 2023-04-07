using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.audio;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.sound;

[DebuggerDisplay("{link} (SoundRef)")]
public class SoundRef : AudioRef, IEquatable<SoundRef>, IComparable<SoundRef>
{
	public const string BUTTON_CLICK = "buttonclick";

	public const string SCROLL_IN = "scrollin";

	public const string SCROLL_OUT = "scrollout";

	public const string CLICK_INDICATOR = "clickindicator";

	public const string ENCOUNTER = "encounter";

	public const string TREASURE = "treasure";

	public const string SHRINE = "shrine";

	public const string HEAL = "heal";

	public const string CRITICAL = "critical";

	public const string BLOCK = "block";

	public const string EVADE = "evade";

	public const string SHEEN = "sheen";

	public const string GOLD = "gold";

	public const string CREDITS = "credits";

	public const string PURCHASE = "purchase";

	public const string BATTLE_TRANSITION = "battletransition";

	public const string VICTORY = "victory";

	public const string VICTORY_SWORD = "victorysword";

	public const string VICTORY_SHIELD = "victoryshield";

	public const string MESSAGE = "message";

	public const string MESSAGE_PRIVATE = "messageprivate";

	public const string MESSAGE_GUILD = "messageguild";

	public const string LEVEL_UP = "levelup";

	public const string UPGRADE_ITEM = "upgradeitem";

	public const string UPGRADE_STAT = "upgradestat";

	public const string EXCHANGE = "exchange";

	public const string EQUIP = "equip";

	public const string UNEQUIP = "unequip";

	public const string FAMILIAR_SUCCESS = "familiarsuccess";

	public const string FAMILIAR_FAIL = "familiarfail";

	public const string FAMILIAR_DECLINE = "familiardecline";

	public const string DUNGEON_ENTER = "dungeonenter";

	public const string DUNGEON_COMPLETE = "dungeoncomplete";

	public const string FUSION = "fusion";

	private SoundBookData.Sound soundData;

	private float _musicVolume;

	public float musicVolume => _musicVolume;

	public SoundRef(SoundBookData.Sound soundData)
		: base(soundData.link, AssetURL.GetPath(AssetURL.AUDIO_SOUND, soundData.url), soundData.volume, soundData.duration, null, soundData.value, soundData.loadLocal, soundData.loadImmediately)
	{
		this.soundData = soundData;
		_musicVolume = soundData.musicVolume;
	}

	public bool Equals(SoundRef other)
	{
		if (other == null)
		{
			return false;
		}
		return Equals((AudioRef)other);
	}

	public int CompareTo(SoundRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return CompareTo((AudioRef)other);
	}
}
