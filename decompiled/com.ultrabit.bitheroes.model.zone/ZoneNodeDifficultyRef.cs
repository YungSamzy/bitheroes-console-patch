using System.Collections.Generic;
using com.ultrabit.bitheroes.model.dungeon;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.team;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml.zone;

namespace com.ultrabit.bitheroes.model.zone;

public class ZoneNodeDifficultyRef
{
	private int _zoneID;

	private int _nodeID;

	private List<int> _requiredDifficulties;

	private List<GameModifier> _modifiers;

	private List<ItemData> _rewards;

	private TeamRules _teamRules;

	private ZoneDifficultyRef _difficultyRef;

	private DungeonRef _dungeonRef;

	private ZoneXMLData.Difficulty difficultyData;

	public int zoneID => _zoneID;

	public int nodeID => _nodeID;

	public int energy => int.Parse(difficultyData.energy);

	public TeamRules teamRules => _teamRules;

	public List<int> requiredDifficulties => _requiredDifficulties;

	public List<GameModifier> modifiers => _modifiers;

	public List<ItemData> rewards => _rewards;

	public ZoneDifficultyRef difficultyRef => _difficultyRef;

	public DungeonRef dungeonRef => _dungeonRef;

	public ZoneNodeDifficultyRef(int zoneID, int nodeID, ZoneXMLData.Difficulty difficultyData)
	{
		_zoneID = zoneID;
		_nodeID = nodeID;
		this.difficultyData = difficultyData;
		_difficultyRef = ZoneBook.LookupDifficultyLink(difficultyData.type);
		_dungeonRef = DungeonBook.LookupLink(difficultyData.dungeon);
		int intFromStringProperty = Util.GetIntFromStringProperty(difficultyData.slots, 1);
		int intFromStringProperty2 = Util.GetIntFromStringProperty(difficultyData.size, intFromStringProperty);
		bool boolFromStringProperty = Util.GetBoolFromStringProperty(difficultyData.allowFamiliars, defaultValue: true);
		bool boolFromStringProperty2 = Util.GetBoolFromStringProperty(difficultyData.allowFriends, defaultValue: true);
		bool boolFromStringProperty3 = Util.GetBoolFromStringProperty(difficultyData.allowGuildmates, defaultValue: true);
		bool boolFromStringProperty4 = Util.GetBoolFromStringProperty(difficultyData.statBalance);
		_teamRules = new TeamRules(intFromStringProperty, intFromStringProperty2, boolFromStringProperty, boolFromStringProperty2, boolFromStringProperty3, boolFromStringProperty4);
		_requiredDifficulties = ZoneBook.GetDifficultyList(difficultyData.requiredDifficulties);
		_modifiers = GameModifier.GetGameModifierFromData(difficultyData.modifiers, difficultyData.lstModifier);
		_rewards = new List<ItemData>();
		if (difficultyData.rewards == null)
		{
			return;
		}
		foreach (ZoneXMLData.Item item in difficultyData.rewards.lstItem)
		{
			ItemRef itemRef = ItemBook.Lookup(item.id, item.type);
			if (itemRef != null)
			{
				_rewards.Add(new ItemData(itemRef, item.qty));
			}
		}
	}

	public ZoneRef getZoneRef()
	{
		return ZoneBook.Lookup(_zoneID);
	}

	public ZoneNodeRef getNodeRef()
	{
		return ZoneBook.Lookup(_zoneID).getNodeRef(_nodeID);
	}
}
