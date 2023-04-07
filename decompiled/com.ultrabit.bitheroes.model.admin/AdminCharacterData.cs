using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.enchant;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.mount;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.admin;

public class AdminCharacterData
{
	private int _id;

	private string _name;

	private string _herotag;

	private int _platform;

	private string _ipAddress;

	private string _system;

	private string _language;

	private int _level;

	private long _exp;

	private int _gold;

	private int _credits;

	private int _creditsPurchased;

	private int _creditsSpent;

	private int _dollarsSpent;

	private int _points;

	private int _energy;

	private int _tickets;

	private int _shards;

	private int _seals;

	private int _tokens;

	private int _badges;

	private bool _banned;

	private Enchants _enchants;

	private Mounts _mounts;

	private FamiliarStable _familiarStable;

	private CharacterGuildInfo _guildInfo;

	private List<CharacterPlatformData> _platforms;

	private DateTime _createDate;

	private DateTime _loginDate;

	public int id => _id;

	public string name => _name;

	public string herotag => _herotag;

	public int platform => _platform;

	public string ipAddress => _ipAddress;

	public string system => _system;

	public string language => _language;

	public int level => _level;

	public long exp => _exp;

	public int gold => _gold;

	public int credits => _credits;

	public int creditsPurchased => _creditsPurchased;

	public int creditsSpent => _creditsSpent;

	public int dollarsSpent => _dollarsSpent;

	public int points => _points;

	public int energy => _energy;

	public int tickets => _tickets;

	public int shards => _shards;

	public int seals => _seals;

	public int tokens => _tokens;

	public int badges => _badges;

	public bool banned => _banned;

	public Enchants enchants => _enchants;

	public Mounts mounts => _mounts;

	public FamiliarStable familiarStable => _familiarStable;

	public CharacterGuildInfo guildInfo => _guildInfo;

	public List<CharacterPlatformData> platforms => _platforms;

	public DateTime createDate => _createDate;

	public DateTime loginDate => _loginDate;

	public AdminCharacterData(int id, string name, string herotag, int platform, string ipAddress, string system, string language, int level, long exp, int gold, int credits, int creditsPurchased, int creditsSpent, int dollarsSpent, int points, int energy, int tickets, int shards, int seals, int tokens, int badges, bool banned, Enchants enchants, Mounts mounts, FamiliarStable familiarStable, CharacterGuildInfo guildInfo, List<CharacterPlatformData> platforms, DateTime createDate, DateTime loginDate)
	{
		_id = id;
		_name = name;
		_herotag = herotag;
		_platform = platform;
		_ipAddress = ipAddress;
		_system = system;
		_language = language;
		_level = level;
		_exp = exp;
		_gold = gold;
		_credits = credits;
		_creditsPurchased = creditsPurchased;
		_creditsSpent = creditsSpent;
		_dollarsSpent = dollarsSpent;
		_points = points;
		_energy = energy;
		_tickets = tickets;
		_shards = shards;
		_seals = seals;
		_tokens = tokens;
		_badges = badges;
		_banned = banned;
		_enchants = enchants;
		_mounts = mounts;
		_familiarStable = familiarStable;
		_guildInfo = guildInfo;
		_platforms = platforms;
		_createDate = createDate;
		_loginDate = loginDate;
	}

	public static AdminCharacterData fromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("cha1");
		string utfString = sfsob.GetUtfString("cha2");
		string utfString2 = sfsob.GetUtfString("cha109");
		int int2 = sfsob.GetInt("cha31");
		string utfString3 = sfsob.GetUtfString("use9");
		string utfString4 = sfsob.GetUtfString("use7");
		string utfString5 = sfsob.GetUtfString("use8");
		int int3 = sfsob.GetInt("cha4");
		long @long = sfsob.GetLong("cha5");
		int int4 = sfsob.GetInt("cha9");
		int int5 = sfsob.GetInt("cha10");
		int int6 = sfsob.GetInt("cha78");
		int int7 = sfsob.GetInt("cha79");
		int int8 = sfsob.GetInt("cha80");
		int int9 = sfsob.GetInt("cha19");
		int int10 = sfsob.GetInt("cha27");
		int int11 = sfsob.GetInt("cha29");
		int int12 = sfsob.GetInt("cha67");
		int int13 = sfsob.GetInt("cha122");
		int int14 = sfsob.GetInt("cha71");
		int int15 = sfsob.GetInt("cha83");
		bool @bool = sfsob.GetBool("cha92");
		Enchants enchants = Enchants.fromSFSObject(sfsob);
		Mounts mounts = Mounts.fromSFSObject(sfsob);
		FamiliarStable familiarStable = FamiliarStable.fromSFSObject(sfsob);
		CharacterGuildInfo characterGuildInfo = CharacterGuildInfo.fromSFSObject(sfsob);
		List<CharacterPlatformData> list = CharacterPlatformData.listFromSFSObject(sfsob);
		DateTime dateFromString = Util.GetDateFromString(sfsob.GetUtfString("cha76"));
		DateTime dateFromString2 = Util.GetDateFromString(sfsob.GetUtfString("cha77"));
		return new AdminCharacterData(@int, utfString, utfString2, int2, utfString3, utfString4, utfString5, int3, @long, int4, int5, int6, int7, int8, int9, int10, int11, int12, int13, int14, int15, @bool, enchants, mounts, familiarStable, characterGuildInfo, list, dateFromString, dateFromString2);
	}
}
