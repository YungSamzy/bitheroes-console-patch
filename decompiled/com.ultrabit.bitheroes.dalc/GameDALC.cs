using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.instance;
using com.ultrabit.bitheroes.model.raid;
using com.ultrabit.bitheroes.model.team;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.zone;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.dalc;

public class GameDALC : BaseDALC
{
	public const int ENTER_BATTLE = 0;

	public const int ENTER_DUNGEON = 1;

	public const int ENTER_INSTANCE = 2;

	public const int NOTIFICATION = 3;

	public const int GAME_UPDATE = 4;

	public const int ENTER_ZONE_NODE = 5;

	public const int ENTER_RAID = 6;

	public const int PLAYERS_ONLINE = 7;

	public const int PLAYER_LOGIN = 8;

	public const int PLAYER_LOGOUT = 9;

	public const int PLAYER_UPDATE = 10;

	public const int DAILY_QUESTS_UPDATE = 11;

	public const int IDLE_RESPONSE = 12;

	public const int TEST_RESPONSE = 13;

	public const int RECONNECT_DUNGEON = 14;

	public const int CUSTOMCONSUM_UPDATE = 15;

	public const int CHARACTER_ACHIEVEMENTS_UPDATE = 16;

	public const int HERO_FROZEN = 17;

	private static GameDALC _instance;

	public static GameDALC instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new GameDALC();
			}
			return _instance;
		}
	}

	public void RefreshMyBounties()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 11);
		send(sFSObject);
	}

	public void doEnterInstance(InstanceRef instanceRef, bool transition = true, CharacterData charData = null)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 2);
		sFSObject.PutInt("ins0", instanceRef.id);
		if (charData != null)
		{
			sFSObject.PutInt("cha1", charData.charID);
		}
		sFSObject.PutBool("ins4", transition);
		send(sFSObject);
	}

	public void doEnterZoneNode(ZoneNodeDifficultyRef nodeDifficultyRef, List<TeammateData> teammates)
	{
		GameData.instance.PROJECT.lastZone = GameData.instance.PROJECT.character.zones.getHighestCompletedZoneID();
		GameData.instance.PROJECT.character.selectedTeammates = teammates;
		D.Log("all", "doEnterZoneNode zoneID " + nodeDifficultyRef.zoneID);
		D.Log("all", "doEnterZoneNode ZONE_DIFFICULTY " + nodeDifficultyRef.difficultyRef.id);
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 5);
		sFSObject.PutInt("zon0", nodeDifficultyRef.zoneID);
		sFSObject.PutInt("zon1", nodeDifficultyRef.nodeID);
		sFSObject.PutInt("zon2", nodeDifficultyRef.difficultyRef.id);
		sFSObject = TeammateData.listToSFSObject(sFSObject, teammates);
		send(sFSObject);
	}

	public void doEnterRaid(RaidDifficultyRef raidDifficultyRef, List<TeammateData> teammates)
	{
		GameData.instance.PROJECT.character.selectedTeammates = teammates;
		SFSObject sFSObject = new SFSObject();
		D.Log("all", "doEnterZoneNode RAID_ID " + raidDifficultyRef.getRaidRef().id);
		D.Log("all", "doEnterZoneNode RAID_DIFFICULTY " + raidDifficultyRef.difficultyRef.id);
		sFSObject.PutInt("act0", 6);
		sFSObject.PutInt("rai0", raidDifficultyRef.getRaidRef().id);
		sFSObject.PutInt("zon1", raidDifficultyRef.difficultyRef.id);
		sFSObject = TeammateData.listToSFSObject(sFSObject, teammates);
		send(sFSObject);
	}

	public void doPlayersOnline(bool update = false)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 7);
		sFSObject.PutBool("act4", update);
		send(sFSObject);
	}

	public void doIdleResponse()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 12);
		send(sFSObject);
	}

	public void doTestResponse(int type, List<TeammateData> teammates, int difficulty, int battles)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 13);
		sFSObject.PutInt("team0", type);
		sFSObject.PutInt("batr7", difficulty);
		sFSObject.PutInt("batr1", battles);
		sFSObject = TeammateData.listToSFSObject(sFSObject, teammates);
		send(sFSObject);
	}

	public void doReconnectDungeon(bool cancel)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 14);
		sFSObject.PutBool("act6", cancel);
		send(sFSObject);
	}
}
