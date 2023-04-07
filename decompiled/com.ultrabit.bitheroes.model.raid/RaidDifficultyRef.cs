using System.Collections.Generic;
using com.ultrabit.bitheroes.model.dungeon;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.team;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using com.ultrabit.bitheroes.model.xml.common;
using com.ultrabit.bitheroes.model.zone;

namespace com.ultrabit.bitheroes.model.raid;

public class RaidDifficultyRef
{
	private int _raidID;

	private TeamRules _teamRules;

	private ZoneDifficultyRef _difficultyRef;

	private DungeonRef _dungeonRef;

	private List<GameModifier> _modifiers;

	private RaidBookData.Difficulty raidDifficultyData;

	public int shards => Util.GetIntFromStringProperty(raidDifficultyData.shards);

	public TeamRules teamRules => _teamRules;

	public ZoneDifficultyRef difficultyRef => _difficultyRef;

	public DungeonRef dungeonRef => _dungeonRef;

	public List<GameModifier> modifiers => _modifiers;

	public RaidDifficultyRef(int raidID, RaidBookData.Difficulty raidDifficultyData)
	{
		this.raidDifficultyData = raidDifficultyData;
		_raidID = raidID;
		int intFromStringProperty = Util.GetIntFromStringProperty(raidDifficultyData.slots, 1);
		int intFromStringProperty2 = Util.GetIntFromStringProperty(raidDifficultyData.size, intFromStringProperty);
		bool boolFromStringProperty = Util.GetBoolFromStringProperty(raidDifficultyData.allowFamiliars, defaultValue: true);
		bool boolFromStringProperty2 = Util.GetBoolFromStringProperty(raidDifficultyData.allowFriends, defaultValue: true);
		bool boolFromStringProperty3 = Util.GetBoolFromStringProperty(raidDifficultyData.allowGuildmates, defaultValue: true);
		bool boolFromStringProperty4 = Util.GetBoolFromStringProperty(raidDifficultyData.statBalance);
		_teamRules = new TeamRules(intFromStringProperty, intFromStringProperty2, boolFromStringProperty, boolFromStringProperty2, boolFromStringProperty3, boolFromStringProperty4);
		_difficultyRef = ZoneBook.LookupDifficultyLink(raidDifficultyData.type);
		_dungeonRef = DungeonBook.LookupLink(raidDifficultyData.dungeon);
		_modifiers = new List<GameModifier>();
		if (raidDifficultyData.modifiers == null || raidDifficultyData.modifiers.lstModifier.Count <= 0)
		{
			return;
		}
		foreach (GameModifierData item in raidDifficultyData.modifiers.lstModifier)
		{
			if (item != null)
			{
				_modifiers.Add(new GameModifier(item));
			}
		}
	}

	public RaidRef getRaidRef()
	{
		return RaidBook.LookUp(_raidID);
	}
}
