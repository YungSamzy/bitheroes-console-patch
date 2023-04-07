using System.Collections.Generic;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.team;

public class TeamData
{
	public const int TYPE_NONE = 0;

	public const int TYPE_PVE = 1;

	public const int TYPE_PVP = 2;

	public const int TYPE_RAID = 3;

	public const int TYPE_RIFT = 4;

	public const int TYPE_GAUNTLET = 5;

	public const int TYPE_GVG = 6;

	public const int TYPE_INVASION = 7;

	public const int TYPE_GVE = 8;

	private int _type;

	private TeamRules _teamRules;

	private List<TeammateData> _teammates;

	public int type => _type;

	public TeamRules teamRules => _teamRules;

	public List<TeammateData> teammates => _teammates;

	public TeamData(int type, TeamRules teamRules, List<TeammateData> teammates)
	{
		_type = type;
		_teamRules = teamRules;
		_teammates = teammates;
	}

	public TeamData Copy()
	{
		List<TeammateData> list = new List<TeammateData>();
		foreach (TeammateData teammate in _teammates)
		{
			if (teammate != null)
			{
				TeammateData item = new TeammateData(teammate.id, teammate.type, -1L, forceCalculate: false, _type);
				list.Add(item);
			}
		}
		return new TeamData(_type, _teamRules, list);
	}

	public bool matches(int type, TeamRules teamRules)
	{
		if (_type == type && _teamRules.matches(teamRules))
		{
			return true;
		}
		return false;
	}

	public void setTeammates(List<TeammateData> v)
	{
		_teammates = v;
	}

	public SFSObject toSFSObject(SFSObject sfsob)
	{
		sfsob.PutInt("team0", _type);
		sfsob = _teamRules.toSFSObject(sfsob);
		sfsob = TeammateData.listToSFSObject(sfsob, _teammates);
		return sfsob;
	}

	public static TeamData fromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("team0");
		TeamRules teamRules = TeamRules.fromSFSObject(sfsob);
		List<TeammateData> list = TeammateData.listFromSFSObject(sfsob);
		return new TeamData(@int, teamRules, list);
	}
}
