using System.Collections.Generic;
using com.ultrabit.bitheroes.messenger;
using com.ultrabit.bitheroes.model.events;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.team;

public class Teams : Messenger
{
	private List<TeamData> _teams;

	public Teams(List<TeamData> teams)
	{
		_teams = teams;
	}

	public TeamData getTeam(int type, TeamRules teamRules, bool copy = true)
	{
		foreach (TeamData team in _teams)
		{
			if (team.matches(type, teamRules))
			{
				return copy ? team.Copy() : team;
			}
		}
		return null;
	}

	public bool hasTeam(int type, TeamRules teamRules)
	{
		if (getTeam(type, teamRules) != null)
		{
			return true;
		}
		return false;
	}

	public TeamData setTeam(int type, TeamRules teamRules, List<TeammateData> teammates)
	{
		TeamData team = getTeam(type, teamRules, copy: false);
		if (team != null)
		{
			team.setTeammates(teammates);
			Broadcast(CustomSFSXEvent.CHANGE);
			return team;
		}
		team = new TeamData(type, teamRules, teammates);
		_teams.Add(team);
		Broadcast(CustomSFSXEvent.CHANGE);
		return team;
	}

	public List<TeammateData> getTeammates(int type, TeamRules teamRules)
	{
		return getTeam(type, teamRules)?.Copy().teammates;
	}

	public static Teams fromSFSObject(ISFSObject sfsob)
	{
		ISFSArray sFSArray = sfsob.GetSFSArray("cha62");
		List<TeamData> list = new List<TeamData>();
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			ISFSObject sFSObject = sFSArray.GetSFSObject(i);
			list.Add(TeamData.fromSFSObject(sFSObject));
		}
		return new Teams(list);
	}
}
