using System;
using System.Collections.Generic;
using System.Linq;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.armory;
using com.ultrabit.bitheroes.model.armory.enchant;
using com.ultrabit.bitheroes.model.armory.rune;
using com.ultrabit.bitheroes.model.augment;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.character.achievements;
using com.ultrabit.bitheroes.model.consumable;
using com.ultrabit.bitheroes.model.daily;
using com.ultrabit.bitheroes.model.enchant;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.mount;
using com.ultrabit.bitheroes.model.rune;
using com.ultrabit.bitheroes.model.team;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.dalc;

public class CharacterDALC : BaseDALC
{
	public const int CUSTOMIZE = 1;

	public const int SAVE_TUTORIAL = 2;

	public const int SAVE_STATS = 3;

	public const int SAVE_EQUIPMENT = 4;

	public const int DAILY_REWARD = 5;

	public const int SAVE_CONFIG = 6;

	public const int USE_CONSUMABLE = 7;

	public const int UPDATE_SHOP_ROTATION = 8;

	public const int GET_PROFILE = 9;

	public const int SAVE_ZONE_ID = 10;

	public const int GET_STATS = 11;

	public const int SAVE_TEAM = 12;

	public const int DAILY_QUEST_CHECK = 13;

	public const int DAILY_QUEST_LOOT = 14;

	public const int AD_REWARD = 15;

	public const int AD_SKIP = 16;

	public const int RUNE_USE = 17;

	public const int RUNE_CHANGE = 18;

	public const int PLATFORM_LINK = 19;

	public const int INVENTORY_CHECK = 20;

	public const int NBP_CHECK = 21;

	public const int RUNE_EXCHANGE = 22;

	public const int ADJUST_ITEMS = 23;

	public const int GET_ONLINE = 24;

	public const int GET_SERVER_INSTANCE = 25;

	public const int UPDATE_TIMERS = 26;

	public const int FAMILIAR_STABLE_RELEASE = 27;

	public const int FAMILIAR_STABLE_BOARD = 28;

	public const int ENCHANT_USE = 29;

	public const int ENCHANT_EQUIP = 30;

	public const int ENCHANT_DESTROY = 31;

	public const int ENCHANT_REROLL = 32;

	public const int UPDATE_CURRENCIES = 33;

	public const int CHECK_OFFERWALL = 34;

	public const int UPDATE = 35;

	public const int ITEM_REWARD = 36;

	public const int MOUNT_USE = 37;

	public const int MOUNT_EQUIP = 38;

	public const int MOUNT_DESTROY = 39;

	public const int MOUNT_REROLL = 40;

	public const int MOUNT_COSMETIC = 41;

	public const int MOUNT_UPGRADE = 42;

	public const int DAILY_FISHING_REWARD = 43;

	public const int AUGMENT_USE = 44;

	public const int AUGMENT_EQUIP = 45;

	public const int AUGMENT_DESTROY = 46;

	public const int AUGMENT_REROLL = 47;

	public const int UPDATE_ARMORY = 48;

	public const int ARMORY_RUNE_EXCHANGE = 49;

	public const int ARMORY_ENCHANT_USE = 50;

	public const int ARMORY_ENCHANT_EQUIP = 51;

	public const int ARMORY_ENCHANT_DESTROY = 52;

	public const int ARMORY_ENCHANT_REROLL = 53;

	public const int CREATE_ARMORY = 54;

	public const int UPDATE_ARMORY_RUNES = 55;

	public const int UPDATE_ARMORY_ENCHANTS = 56;

	public const int DO_CHARACTER_EQUIPMENT_GLOBAL_SAVE = 57;

	public const int CHANGE_HEROTAG_NAME = 58;

	public const int GET_PROFILESPOSIBLES = 59;

	public const int GET_PROFILE_NAME_HISTORY = 60;

	public const int UPDATE_ADGOR_CONSUMABLE_MODIFIER = 61;

	public const int USE_ADGOR_CONSUMABLE_MODIFIER = 62;

	public const int CHECK_ADGOR_COOLDOWN = 63;

	public const int GET_FAMILIAR_ENCOUNTER_INFO = 64;

	public const int GET_POSSIBLE_BATTLE_LOOT = 65;

	public const int SHOW_NAMEPLATE = 66;

	public const int ADGOR_TIMER_FINISH_ACTION = 67;

	public const int UPDATE_ENERGY = 68;

	public const int EXTRA_PLATFORM_LINK = 69;

	public const int REQUEST_CURRENT_BOOSTERS = 70;

	public const int CHARACTER_ACHIEVEMENT_CHECK = 71;

	public const int CHARACTER_ACHIEVEMENT_LOOT = 72;

	public const int CHARACTER_ACHIEVEMENT_CHECK_LOOT = 73;

	public const int CHARACTER_ACHIEVEMENTS_MULTIPLE_LOOT = 74;

	private static CharacterDALC _instance;

	public static CharacterDALC instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new CharacterDALC();
			}
			return _instance;
		}
	}

	public void doCustomize(string name, bool genderMale, int hairID, int hairColorID, int skinColorID, string customconsum)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 1);
		sFSObject.PutUtfString("cha2", name);
		sFSObject.PutBool("cha12", genderMale);
		sFSObject.PutInt("cha20", hairID);
		sFSObject.PutInt("cha21", hairColorID);
		sFSObject.PutInt("cha22", skinColorID);
		sFSObject.PutUtfString("char107", customconsum);
		send(sFSObject);
	}

	public void doSaveHerotagName(string name)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 58);
		sFSObject.PutUtfString("cha2", name);
		send(sFSObject);
	}

	public void doSaveTutorial(Dictionary<int, bool> states)
	{
		bool[] array = new bool[states.Keys.Max() + 1];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = states.ContainsKey(i) && states[i];
		}
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 2);
		sFSObject.PutBoolArray("cha11", array);
		send(sFSObject);
	}

	public void doSaveStats(int power, int stamina, int agility, int points)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 3);
		sFSObject.PutInt("cha6", power);
		sFSObject.PutInt("cha7", stamina);
		sFSObject.PutInt("cha8", agility);
		sFSObject.PutInt("cha19", points);
		send(sFSObject);
	}

	public void doCreateArmorySlot(uint pos, string name)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 54);
		sFSObject.PutInt("position", (int)pos);
		sFSObject.PutUtfString("name", name);
		send(sFSObject);
	}

	public void doCharacterEquipmentGlobalSave()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 57);
		int[] array = new int[8];
		int[] array2 = new int[8];
		bool[] array3 = new bool[8];
		bool[] array4 = new bool[8];
		bool flag = false;
		Equipment equipment = GameData.instance.PROJECT.character.equipment;
		for (int i = 0; i < 8; i++)
		{
			EquipmentRef equipmentSlot = equipment.getEquipmentSlot(i);
			if (equipmentSlot != null)
			{
				if (!array3[equipmentSlot.subtype - 1])
				{
					array3[equipmentSlot.subtype - 1] = true;
				}
				else
				{
					flag = true;
				}
				array[i] = equipmentSlot.id;
			}
			EquipmentRef cosmeticSlot = equipment.getCosmeticSlot(i);
			if (cosmeticSlot != null)
			{
				if (!array4[cosmeticSlot.subtype - 1])
				{
					array4[cosmeticSlot.subtype - 1] = true;
				}
				else
				{
					flag = true;
				}
				array2[i] = cosmeticSlot.id;
			}
		}
		if (!flag)
		{
			for (int j = 0; j < 8; j++)
			{
				EquipmentRef equipmentSlot2 = equipment.getEquipmentSlot(j);
				if (equipmentSlot2 != null)
				{
					array[equipmentSlot2.subtype - 1] = equipmentSlot2.id;
				}
				EquipmentRef cosmeticSlot2 = equipment.getCosmeticSlot(j);
				if (cosmeticSlot2 != null)
				{
					array2[cosmeticSlot2.subtype - 1] = cosmeticSlot2.id;
				}
			}
		}
		sFSObject.PutIntArray("cha17", array);
		sFSObject.PutIntArray("cha64", array2);
		RuneRef runeSlot = GameData.instance.PROJECT.character.runes.getRuneSlot(0);
		if (runeSlot != null)
		{
			sFSObject.PutInt("run_slot_0", runeSlot.id);
		}
		runeSlot = GameData.instance.PROJECT.character.runes.getRuneSlot(1);
		if (runeSlot != null)
		{
			sFSObject.PutInt("run_slot_1", runeSlot.id);
		}
		runeSlot = GameData.instance.PROJECT.character.runes.getRuneSlot(2);
		if (runeSlot != null)
		{
			sFSObject.PutInt("run_slot_2", runeSlot.id);
		}
		runeSlot = GameData.instance.PROJECT.character.runes.getRuneSlot(3);
		if (runeSlot != null)
		{
			sFSObject.PutInt("run_slot_3", runeSlot.id);
		}
		runeSlot = GameData.instance.PROJECT.character.runes.getRuneSlot(4);
		if (runeSlot != null)
		{
			sFSObject.PutInt("run_slot_4", runeSlot.id);
		}
		runeSlot = GameData.instance.PROJECT.character.runes.getRuneSlot(5);
		if (runeSlot != null)
		{
			sFSObject.PutInt("run_slot_5", runeSlot.id);
		}
		runeSlot = GameData.instance.PROJECT.character.runes.getRuneSlot(6);
		if (runeSlot != null)
		{
			sFSObject.PutInt("run_slot_6", runeSlot.id);
		}
		runeSlot = GameData.instance.PROJECT.character.runes.getRuneSlot(7);
		if (runeSlot != null)
		{
			sFSObject.PutInt("run_slot_7", runeSlot.id);
		}
		runeSlot = GameData.instance.PROJECT.character.runes.getRuneSlot(8);
		if (runeSlot != null)
		{
			sFSObject.PutInt("run_slot_8", runeSlot.id);
		}
		ArmoryEquipment currentArmoryEquipmentSlot = GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot;
		sFSObject.PutLong("ench_slot_0", currentArmoryEquipmentSlot.enchants.getSlot(0)?.uid ?? (-1));
		sFSObject.PutLong("ench_slot_1", currentArmoryEquipmentSlot.enchants.getSlot(1)?.uid ?? (-1));
		sFSObject.PutLong("ench_slot_2", currentArmoryEquipmentSlot.enchants.getSlot(2)?.uid ?? (-1));
		sFSObject.PutLong("ench_slot_3", currentArmoryEquipmentSlot.enchants.getSlot(3)?.uid ?? (-1));
		sFSObject.PutLong("ench_slot_4", currentArmoryEquipmentSlot.enchants.getSlot(4)?.uid ?? (-1));
		sFSObject.PutLong("ench_slot_5", currentArmoryEquipmentSlot.enchants.getSlot(5)?.uid ?? (-1));
		sFSObject.PutInt("moun2", (GameData.instance.PROJECT.character.mounts.cosmetic != null) ? GameData.instance.PROJECT.character.mounts.cosmetic.id : 0);
		sFSObject.PutLong("moun1", GameData.instance.PROJECT.character.mounts.getMountEquipped()?.uid ?? (-1));
		send(sFSObject);
	}

	public void doSaveEquipment(Equipment equipment)
	{
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		for (int i = 0; i < 8; i++)
		{
			EquipmentRef equipmentSlot = equipment.getEquipmentSlot(i);
			list.Add((equipmentSlot != null) ? equipmentSlot.id : 0);
			EquipmentRef cosmeticSlot = equipment.getCosmeticSlot(i);
			list2.Add((cosmeticSlot != null) ? cosmeticSlot.id : 0);
		}
		int[] array = new int[list.Count];
		for (int j = 0; j < list.Count; j++)
		{
			array[j] = list[j];
		}
		int[] array2 = new int[list2.Count];
		for (int k = 0; k < list2.Count; k++)
		{
			array2[k] = list2[k];
		}
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 4);
		sFSObject.PutIntArray("cha17", array);
		sFSObject.PutIntArray("cha64", array2);
		send(sFSObject);
	}

	public void updateArmoryRunes(ArmoryEquipment armoryEquip)
	{
		ArmoryRunes runes = armoryEquip.runes;
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 55);
		sFSObject.PutInt("id", (int)armoryEquip.id);
		if (runes.getRuneSlot(0) != null)
		{
			sFSObject.PutInt("MajorA", runes.getRuneSlot(0).id);
		}
		else
		{
			sFSObject.PutInt("MajorA", 0);
		}
		if (runes.getRuneSlot(1) != null)
		{
			sFSObject.PutInt("MajorB", runes.getRuneSlot(1).id);
		}
		else
		{
			sFSObject.PutInt("MajorB", 0);
		}
		if (runes.getRuneSlot(2) != null)
		{
			sFSObject.PutInt("MajorC", runes.getRuneSlot(2).id);
		}
		else
		{
			sFSObject.PutInt("MajorC", 0);
		}
		if (runes.getRuneSlot(3) != null)
		{
			sFSObject.PutInt("MajorD", runes.getRuneSlot(3).id);
		}
		else
		{
			sFSObject.PutInt("MajorD", 0);
		}
		if (runes.getRuneSlot(4) != null)
		{
			sFSObject.PutInt("MinorA", runes.getRuneSlot(4).id);
		}
		else
		{
			sFSObject.PutInt("MinorA", 0);
		}
		if (runes.getRuneSlot(5) != null)
		{
			sFSObject.PutInt("MinorB", runes.getRuneSlot(5).id);
		}
		else
		{
			sFSObject.PutInt("MinorB", 0);
		}
		if (runes.getRuneSlot(6) != null)
		{
			sFSObject.PutInt("Meta", runes.getRuneSlot(6).id);
		}
		else
		{
			sFSObject.PutInt("Meta", 0);
		}
		if (runes.getRuneSlot(8) != null)
		{
			sFSObject.PutInt("Artifact", runes.getRuneSlot(8).id);
		}
		else
		{
			sFSObject.PutInt("Artifact", 0);
		}
		if (runes.getRuneSlot(7) != null)
		{
			sFSObject.PutInt("Relic", runes.getRuneSlot(7).id);
		}
		else
		{
			sFSObject.PutInt("Relic", 0);
		}
		send(sFSObject);
	}

	public void updateArmoryEnchants(ArmoryEquipment armoryEquip)
	{
		ArmoryEnchants enchants = armoryEquip.enchants;
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 56);
		sFSObject.PutInt("id", (int)armoryEquip.id);
		if (enchants.getSlot(0) != null)
		{
			sFSObject.PutInt("slota", (int)enchants.getSlot(0).uid);
		}
		else
		{
			sFSObject.PutInt("slota", 0);
		}
		if (enchants.getSlot(1) != null)
		{
			sFSObject.PutInt("slotb", (int)enchants.getSlot(1).uid);
		}
		else
		{
			sFSObject.PutInt("slotb", 0);
		}
		if (enchants.getSlot(2) != null)
		{
			sFSObject.PutInt("slotc", (int)enchants.getSlot(2).uid);
		}
		else
		{
			sFSObject.PutInt("slotc", 0);
		}
		if (enchants.getSlot(3) != null)
		{
			sFSObject.PutInt("slotd", (int)enchants.getSlot(3).uid);
		}
		else
		{
			sFSObject.PutInt("slotd", 0);
		}
		if (enchants.getSlot(4) != null)
		{
			sFSObject.PutInt("slote", (int)enchants.getSlot(4).uid);
		}
		else
		{
			sFSObject.PutInt("slote", 0);
		}
		if (enchants.getSlot(5) != null)
		{
			sFSObject.PutInt("slotf", (int)enchants.getSlot(5).uid);
		}
		else
		{
			sFSObject.PutInt("slotf", 0);
		}
		send(sFSObject);
	}

	public void doSaveArmory(ArmoryEquipment armoryEquip, bool dispatch = true)
	{
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		for (int i = 0; i < 8; i++)
		{
			ArmoryRef armoryEquipmentSlot = armoryEquip.GetArmoryEquipmentSlot(i);
			list.Add((armoryEquipmentSlot != null) ? armoryEquipmentSlot.id : 0);
			ArmoryRef cosmeticSlot = armoryEquip.GetCosmeticSlot(i);
			list2.Add((cosmeticSlot != null) ? cosmeticSlot.id : 0);
		}
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("id", (int)armoryEquip.id);
		sFSObject.PutInt("position", (int)armoryEquip.position);
		sFSObject.PutUtfString("name", armoryEquip.name);
		sFSObject.PutBool("unlocked", armoryEquip.unlocked);
		sFSObject.PutInt("mount", (int)armoryEquip.mount);
		sFSObject.PutInt("mountCosmetic", (int)armoryEquip.mountCosmetic);
		sFSObject.PutBool("armoryPrivate", armoryEquip.pprivate);
		sFSObject.PutInt("armoryBattleType", (int)armoryEquip.battleType);
		sFSObject.PutInt("act0", 48);
		sFSObject.PutIntArray("cha103", list.ToArray());
		sFSObject.PutIntArray("cha104", list2.ToArray());
		send(sFSObject);
		if (dispatch)
		{
			GameData.instance.PROJECT.character.Broadcast("armoryEquipmentChange");
		}
		instance.dispatch(CustomSFSXEvent.getEvent(48), sFSObject);
	}

	public void doDailyReward()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 5);
		send(sFSObject);
	}

	public void doSaveConfig(Character character)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 6);
		sFSObject.PutBool("cha32", character.autoPilot);
		sFSObject.PutBool("cha35", character.chatEnabled);
		sFSObject.PutInt("cha36", character.chatChannel);
		sFSObject.PutBool("cha40", character.friendRequestsEnabled);
		sFSObject.PutBool("cha82", character.duelRequestsEnabled);
		sFSObject.PutBool("cha48", character.showHelm);
		sFSObject.PutBool("cha93", character.showMount);
		sFSObject.PutBool("cha133", character.showBody);
		sFSObject.PutBool("cha134", character.showAccessory);
		sFSObject = ItemRef.listToSFSObject(sFSObject, character.lockedItems, "cha95");
		send(sFSObject);
	}

	internal static void doEnchantReroll(object enchantData)
	{
		throw new NotImplementedException();
	}

	public void doUseConsumable(ConsumableRef consumableRef, int qty = 1)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 7);
		sFSObject.PutInt("ite0", consumableRef.id);
		sFSObject.PutInt("ite2", qty);
		send(sFSObject);
	}

	public void doUseAdGorConsumableModifier(ConsumableRef consumableRef, int step = 1)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 62);
		sFSObject.PutInt("ite0", consumableRef.id);
		sFSObject.PutInt("ite2", 1);
		if (GameData.instance.PROJECT.character.toCharacterData().nftIsAdFree)
		{
			sFSObject.PutBool("cha130", val: true);
			sFSObject.PutInt("adgor01", 5);
		}
		else
		{
			sFSObject.PutInt("adgor01", step);
		}
		send(sFSObject);
	}

	public void doCheckAdGorCooldown()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 63);
		send(sFSObject);
	}

	public void doUpdateAdGorConsumableModifier()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 61);
		send(sFSObject);
	}

	public void doUpdateShopRotation()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 8);
		send(sFSObject);
	}

	public void doGetProfile(int charID = 0, string name = null)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 9);
		sFSObject.PutInt("cha1", charID);
		if (name != null)
		{
			sFSObject.PutUtfString("cha2", name);
		}
		send(sFSObject);
	}

	public void doGetProfilesPosibles(string name = null)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 59);
		sFSObject.PutUtfString("cha2", name);
		send(sFSObject);
	}

	public void doGetNameHistory(int charID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 60);
		sFSObject.PutInt("cha1", charID);
		send(sFSObject);
	}

	public void doSaveZoneID(int zoneID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 10);
		sFSObject.PutInt("cha24", zoneID);
		send(sFSObject);
	}

	public void doGetStats()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 11);
		send(sFSObject);
	}

	public void doSaveTeam(TeamData teamData)
	{
		if (teamData != null)
		{
			SFSObject sFSObject = new SFSObject();
			sFSObject.PutInt("act0", 12);
			sFSObject = teamData.toSFSObject(sFSObject);
			send(sFSObject);
		}
	}

	public void doDailyQuestCheck()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 13);
		send(sFSObject);
	}

	public void doDailyQuestLoot(DailyQuestData questData)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 14);
		sFSObject.PutInt("dail1", questData.questRef.id);
		send(sFSObject);
	}

	public void doCharacterAchievementLoot(CharacterAchievementData achievementData)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 72);
		sFSObject.PutInt("achive1", achievementData.achievementRef.id);
		send(sFSObject);
	}

	public void doCharacterAchievementsMultipleLoot(int[] achievementIDs)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 74);
		sFSObject.PutIntArray("achive5", achievementIDs);
		send(sFSObject);
	}

	public void doCharacterAchievementCheckLoot(CharacterAchievementData achievementData)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 73);
		sFSObject.PutInt("achive1", achievementData.achievementRef.id);
		send(sFSObject);
	}

	public void doAdReward(int duration)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 15);
		sFSObject.PutInt("cha65", duration);
		if (GameData.instance.PROJECT.character.toCharacterData().nftIsAdFree)
		{
			sFSObject.PutBool("cha130", val: true);
		}
		send(sFSObject);
	}

	public void doAdSkip()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 16);
		send(sFSObject);
	}

	public void doRuneUse(RuneRef runeRef, int slot)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 17);
		sFSObject.PutInt("ite0", runeRef.id);
		sFSObject.PutInt("run2", slot);
		send(sFSObject);
	}

	public void doArmoryRuneUse(RuneRef runeRef, int slot)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 49);
		sFSObject.PutInt("ite0", runeRef.id);
		sFSObject.PutInt("cha103", (int)GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.id);
		sFSObject.PutInt("run2", slot);
		sFSObject.PutInt("cha105", GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.runes.id);
		send(sFSObject);
		GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.runes.setRuneSlot(runeRef, slot);
		GameData.instance.PROJECT.character.Broadcast("armoryRuneChange");
	}

	public void doArmoryRuneChange(RuneRef runeRef, int slot, int aID = -1)
	{
		if (aID == -1)
		{
			aID = (int)GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.id;
		}
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 49);
		if (runeRef != null)
		{
			sFSObject.PutInt("ite0", runeRef.id);
		}
		else
		{
			sFSObject.PutInt("ite0", 0);
		}
		sFSObject.PutInt("run2", slot);
		sFSObject.PutInt("cha103", aID);
		send(sFSObject);
		instance.dispatch(CustomSFSXEvent.getEvent(49), sFSObject);
	}

	public void doRuneChange(RuneRef runeRef, int slot)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 18);
		sFSObject.PutInt("ite0", runeRef.id);
		sFSObject.PutInt("run2", slot);
		send(sFSObject);
	}

	public void doExtraPlatformLink(string userID, string userName, int userPlatform, int charID, string adID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 69);
		sFSObject.PutUtfString("use3", userID);
		sFSObject.PutUtfString("use0", userName);
		sFSObject.PutUtfString("use7", AppInfo.GetSystem());
		sFSObject.PutInt("use2", userPlatform);
		sFSObject.PutInt("cha1", charID);
		sFSObject.PutUtfString("use14", adID);
		send(sFSObject);
	}

	public void doInventoryCheck()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 20);
		send(sFSObject);
	}

	public void doRuneExchange(RuneRef runeRef, int slot)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 22);
		sFSObject.PutInt("ite0", runeRef.id);
		sFSObject.PutInt("run2", slot);
		send(sFSObject);
	}

	public void doArmoryRuneExchange(RuneRef runeRef, int slot)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 49);
		sFSObject.PutInt("ite0", runeRef.id);
		sFSObject.PutInt("run2", slot);
		sFSObject.PutInt("cha105", GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.runes.id);
		send(sFSObject);
		GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.runes.setRuneSlot(runeRef, slot);
		GameData.instance.PROJECT.character.Broadcast("armoryRuneChange");
		instance.dispatch(CustomSFSXEvent.getEvent(49), sFSObject);
	}

	public void doGetOnline(int charID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 24);
		sFSObject.PutInt("cha1", charID);
		send(sFSObject);
	}

	public void doGetServerInstance(int charID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 25);
		sFSObject.PutInt("cha1", charID);
		send(sFSObject);
	}

	public void doUpdateTimers()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 26);
		send(sFSObject);
	}

	public void doFamiliarStableRelease(FamiliarRef familiarRef)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 27);
		sFSObject.PutInt("fam1", familiarRef.id);
		send(sFSObject);
	}

	public void doFamiliarStableBoard(FamiliarRef familiarRef)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 28);
		sFSObject.PutInt("fam1", familiarRef.id);
		send(sFSObject);
	}

	public void doEnchantUse(EnchantRef enchantRef)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 29);
		sFSObject.PutInt("ench2", enchantRef.id);
		send(sFSObject);
	}

	public void doArmoryEnchantUse(EnchantRef enchantRef)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("cha102", (int)GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.id);
		sFSObject.PutInt("act0", 50);
		sFSObject.PutInt("ench2", enchantRef.id);
		send(sFSObject);
	}

	public void doEnchantEquip(int slot, EnchantData enchantData = null)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 30);
		sFSObject.PutInt("ench9", slot);
		sFSObject.PutLong("ench1", enchantData?.uid ?? (-1));
		send(sFSObject);
	}

	public void doArmoryEnchantEquip(int slot, EnchantData enchantData = null)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("cha102", (int)GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.id);
		sFSObject.PutInt("act0", 51);
		sFSObject.PutInt("ench9", slot);
		sFSObject.PutLong("ench1", enchantData?.uid ?? (-1));
		send(sFSObject);
	}

	public void doEnchantDestroy(EnchantData enchantData)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 31);
		sFSObject.PutLong("ench1", enchantData.uid);
		send(sFSObject);
	}

	public void doEnchantReroll(EnchantData enchantData)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 32);
		sFSObject.PutLong("ench1", enchantData.uid);
		send(sFSObject);
	}

	public void doUpdateCurrencies()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 33);
		send(sFSObject);
	}

	public void doCheckOfferwall()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 34);
		send(sFSObject);
	}

	public void doUpdate()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 35);
		sFSObject.PutBool("serv9", AppInfo.TESTING);
		send(sFSObject, idleTimer: false);
	}

	public void doItemReward(ItemRewardRef rewardRef)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 36);
		sFSObject.PutInt("ite10", rewardRef.id);
		send(sFSObject);
	}

	public void doMountUse(MountRef mountRef)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 37);
		sFSObject.PutInt("moun2", mountRef.id);
		send(sFSObject);
	}

	public void doMountEquip(MountData mountData = null)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 38);
		sFSObject.PutLong("moun1", mountData?.uid ?? (-1));
		send(sFSObject);
	}

	public void doArmoryMountEquip(MountData mountData = null)
	{
		ArmoryEquipment currentArmoryEquipmentSlot = GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot;
		if (mountData != null)
		{
			currentArmoryEquipmentSlot.mount = mountData.uid;
		}
		else
		{
			currentArmoryEquipmentSlot.mount = 0L;
		}
		instance.doSaveArmory(currentArmoryEquipmentSlot);
	}

	public void doMountDestroy(MountData mountData)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 39);
		sFSObject.PutLong("moun1", mountData.uid);
		send(sFSObject);
	}

	public void doMountReroll(MountData mountData)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 40);
		sFSObject.PutLong("moun1", mountData.uid);
		send(sFSObject);
	}

	public void doMountCosmetic(MountRef mountRef = null)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 41);
		sFSObject.PutInt("moun2", (mountRef != null) ? mountRef.id : 0);
		send(sFSObject);
	}

	public void doMountUpgrade(MountData mountData)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 42);
		sFSObject.PutLong("moun1", mountData.uid);
		send(sFSObject);
	}

	public void doDailyFishingReward()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 43);
		send(sFSObject);
	}

	public void doAugmentUse(AugmentRef augmentRef)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 44);
		sFSObject.PutInt("aug2", augmentRef.id);
		send(sFSObject);
	}

	public void doAugmentEquip(FamiliarRef familiarRef, int slot, AugmentData augmentData = null)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 45);
		sFSObject.PutInt("aug8", (familiarRef != null) ? familiarRef.id : (-1));
		sFSObject.PutInt("aug6", slot);
		sFSObject.PutLong("aug1", augmentData?.uid ?? (-1));
		send(sFSObject);
	}

	public void doAugmentDestroy(AugmentData augmentData)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 46);
		sFSObject.PutLong("aug1", augmentData.uid);
		send(sFSObject);
	}

	public void doAugmentReroll(AugmentData augmentData)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 47);
		sFSObject.PutLong("aug1", augmentData.uid);
		send(sFSObject);
	}

	public void doGetFamiliarEncounterInfo(int familiarID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 64);
		sFSObject.PutInt("fam1", familiarID);
		send(sFSObject);
	}

	public void doGetPossibleBattleLoot(int battleType, int zoneID, int nodeID, string link, int difficultyID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 65);
		sFSObject.PutInt("bat1", battleType);
		sFSObject.PutInt("zon0", zoneID);
		sFSObject.PutInt("zon1", nodeID);
		sFSObject.PutInt("eve11", difficultyID);
		send(sFSObject);
	}

	public void doAdgorTimerFinish()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 67);
		send(sFSObject);
	}

	public void doShowNameplate(bool value)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 66);
		sFSObject.PutBool("showNameplate", value);
		send(sFSObject);
	}

	public void doUpdateEnergy()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 68);
		send(sFSObject);
	}

	public void getCurrentBoosters()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 70);
		send(sFSObject);
	}
}
