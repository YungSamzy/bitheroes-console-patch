using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.guild;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using Sfs2X.Entities.Data;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.admin;

public class AdminGuildData
{
	private int _id;

	private string _name;

	private string _initials;

	private int _level;

	private long _exp;

	private int _points;

	private GuildPerks _perks;

	private List<GuildMemberData> _members;

	private DateTime _createDate;

	private DateTime _loginDate;

	public int id => _id;

	public string name => _name;

	public string initials => _initials;

	public int level => _level;

	public long exp => _exp;

	public int points => _points;

	public GuildPerks perks => _perks;

	public List<GuildMemberData> members => _members;

	public DateTime createDate => _createDate;

	public DateTime loginDate => _loginDate;

	public AdminGuildData(int id, string name, string initials, int level, long exp, int points, GuildPerks perks, List<GuildMemberData> members, DateTime createDate, DateTime loginDate)
	{
		_id = id;
		_name = name;
		_initials = initials;
		_level = level;
		_exp = exp;
		_points = points;
		_perks = perks;
		_members = members;
		_createDate = createDate;
		_loginDate = loginDate;
	}

	public int getMemberLimit()
	{
		int num = VariableBook.guildMemberLimit;
		if (_perks != null)
		{
			num += (int)Mathf.Round(GameModifier.getTypeTotal(_perks.getModifiers(), 42));
		}
		return num;
	}

	public static AdminGuildData fromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("gui0");
		string utfString = sfsob.GetUtfString("gui2");
		string utfString2 = sfsob.GetUtfString("gui3");
		int int2 = sfsob.GetInt("gui7");
		long @long = sfsob.GetLong("gui8");
		int int3 = sfsob.GetInt("gui14");
		GuildPerks guildPerks = GuildPerks.fromSFSObject(sfsob);
		List<GuildMemberData> list = GuildMemberData.listFromSFSObject(sfsob);
		DateTime dateFromString = Util.GetDateFromString(sfsob.GetUtfString("cha76"));
		DateTime dateFromString2 = Util.GetDateFromString(sfsob.GetUtfString("cha77"));
		return new AdminGuildData(@int, utfString, utfString2, int2, @long, int3, guildPerks, list, dateFromString, dateFromString2);
	}
}
