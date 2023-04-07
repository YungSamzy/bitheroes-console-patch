using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.encounter;
using com.ultrabit.bitheroes.model.music;
using com.ultrabit.bitheroes.model.sound;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using com.ultrabit.bitheroes.ui.assets;

namespace com.ultrabit.bitheroes.model.dungeon;

[DebuggerDisplay("{link} (DungeonRef)")]
public class DungeonRef : IEquatable<DungeonRef>, IComparable<DungeonRef>
{
	private DungeonBookData.Dungeon _dungeonData;

	private int _id;

	private string _link;

	private string _asset;

	private MusicRef _music;

	private string _battleBG;

	private MusicRef _battleMusic;

	private MusicRef _bossMusic;

	private bool _loadLocal;

	private string _color;

	private bool _playRandom;

	private bool _pauseChildren;

	private float _statMult;

	private SoundPoolRef _footstepsDefault;

	private List<DungeonOverlayRef> _overlays;

	private List<DungeonEnemyRef> _enemies;

	private DungeonBossRef _boss;

	public int id => _id;

	public string link => _link;

	public MusicRef music => _music;

	public DungeonBossRef boss => _boss;

	public string color => _color;

	public uint colorUint => Util.colorUint(_color);

	public bool playRandom => _playRandom;

	public bool pauseChildren => _pauseChildren;

	public float statMult => _statMult;

	public SoundPoolRef footstepsDefault => _footstepsDefault;

	public List<DungeonOverlayRef> overlays => _overlays;

	public List<DungeonEnemyRef> enemies => _enemies;

	public MusicRef battleMusic => _battleMusic;

	public MusicRef bossMusic => _bossMusic;

	public string asset => _asset;

	public string battleBGURL
	{
		get
		{
			if (_battleBG == null)
			{
				return null;
			}
			return _battleBG;
		}
	}

	public DungeonRef(int id, DungeonBookData.Dungeon dungeonData)
	{
		_dungeonData = dungeonData;
		_id = id;
		_link = dungeonData.link;
		_asset = ((dungeonData.asset != null) ? dungeonData.asset : null);
		_music = MusicBook.Lookup((dungeonData.music != null) ? dungeonData.music : "dungeon");
		_battleBG = ((dungeonData.battleBG != null) ? dungeonData.battleBG : null);
		_battleMusic = MusicBook.Lookup((dungeonData.battleMusic != null) ? dungeonData.battleMusic : "battle");
		_bossMusic = MusicBook.Lookup((dungeonData.bossMusic != null) ? dungeonData.bossMusic : "boss");
		_color = dungeonData.color;
		_playRandom = Util.GetBoolFromStringProperty(dungeonData.playRandom, defaultValue: true);
		_pauseChildren = Util.GetBoolFromStringProperty(dungeonData.pauseChildren, defaultValue: true);
		_statMult = dungeonData.statMult;
		_footstepsDefault = SoundBook.LookupPoolLink(dungeonData.footstepsDefault);
		if (dungeonData.overlays != null && dungeonData.overlays.lstOverlay != null)
		{
			_overlays = new List<DungeonOverlayRef>();
			foreach (DungeonBookData.Overlay item in dungeonData.overlays.lstOverlay)
			{
				if (item != null)
				{
					_overlays.Add(new DungeonOverlayRef(item));
				}
			}
		}
		if (dungeonData.enemies != null && dungeonData.enemies.lstEnemy != null)
		{
			_enemies = new List<DungeonEnemyRef>();
			foreach (DungeonBookData.Enemy item2 in dungeonData.enemies.lstEnemy)
			{
				int count = _enemies.Count;
				EncounterRef encounter = EncounterBook.LookupLink(item2.encounter);
				_enemies.Add(new DungeonEnemyRef(count, encounter));
			}
		}
		if (dungeonData.boss != null)
		{
			EncounterRef encounter2 = EncounterBook.LookupLink(dungeonData.boss.encounter);
			_boss = new DungeonBossRef(encounter2);
		}
	}

	public DungeonEnemyRef getEnemy(int id)
	{
		if (id < 0 || id >= _enemies.Count)
		{
			return null;
		}
		return _enemies[id];
	}

	public SWFAsset getAsset(string definition = null)
	{
		return null;
	}

	public Asset getBattleBGAsset(bool center = false, float scale = 1f)
	{
		return null;
	}

	public bool Equals(DungeonRef other)
	{
		if (other == null)
		{
			return false;
		}
		return id.Equals(other.id);
	}

	public int CompareTo(DungeonRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return id.CompareTo(other.id);
	}
}
