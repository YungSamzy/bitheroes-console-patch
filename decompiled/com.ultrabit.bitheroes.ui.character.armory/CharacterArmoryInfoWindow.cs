using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.armory;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.character.armory;

public class CharacterArmoryInfoWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public Transform infoListContent;

	public CharacterInfoTile characterInfoItemPrefab;

	public Image loadingIcon;

	private ArmoryEquipment _armoryEquipment;

	private Character _character;

	private List<CharacterInfoTile> _tiles = new List<CharacterInfoTile>();

	public override void Start()
	{
		base.Start();
	}

	public void LoadDetails(ArmoryEquipment aequip, Character character)
	{
		Disable();
		_armoryEquipment = aequip;
		_character = character;
		topperTxt.text = Language.GetString("ui_info");
		DoGetStats();
		ListenForBack(OnClose);
		CreateWindow(closeWord: false, "", scroll: false, stayUp: true);
	}

	public void DoGetStats()
	{
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(11), OnGetStats);
		CharacterDALC.instance.doGetStats();
	}

	private void OnGetStats(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		loadingIcon.gameObject.SetActive(value: false);
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(11), OnGetStats);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		List<GameModifier> armoryModifiers = _character.getArmoryModifiers(_armoryEquipment);
		int offset = 90;
		int offset2 = 100;
		bool outline = true;
		bool abbreviate = false;
		CharacterInfoData characterInfoData = new CharacterInfoData(Language.GetString("ui_general"), outline, offset2);
		characterInfoData.addValue(Language.GetString("ui_level"), Util.NumberFormat(_character.level, abbreviate));
		characterInfoData.addValue(Language.GetString("ui_experience"), Util.NumberFormat(_character.exp, abbreviate));
		characterInfoData.addValue(Language.GetString("ui_next_level"), Util.NumberFormat(Character.getLevelExp(_character.level + 1), abbreviate));
		characterInfoData.addValue(Language.GetString("ui_time_played"), Util.TimeFormatClean(sfsob.GetLong("cha63")));
		characterInfoData.addValue(Language.GetString("ui_friends"), Util.NumberFormat(_character.friends.Count, abbreviate) + " / " + Util.NumberFormat(VariableBook.friendsMax, abbreviate));
		characterInfoData.addValue(ItemRef.GetItemNamePlural(6), Util.NumberFormat(_character.getFamiliarCount(), abbreviate) + " / " + Util.NumberFormat(FamiliarBook.count, abbreviate));
		CharacterInfoData characterInfoData2 = new CharacterInfoData(Language.GetString("ui_stats"), outline, offset);
		characterInfoData2.addValue(Character.getStatName(0), Util.NumberFormat(_character.getArmoryTotalPower(_armoryEquipment), abbreviate));
		characterInfoData2.addValue(Character.getStatName(1), Util.NumberFormat(_character.getArmoryTotalStamina(_armoryEquipment), abbreviate));
		characterInfoData2.addValue(Character.getStatName(2), Util.NumberFormat(_character.getArmoryTotalAgility(_armoryEquipment), abbreviate));
		characterInfoData2.addValue(Language.GetString("ui_total_stats"), Util.NumberFormat(_character.getArmoryTotalStats(_armoryEquipment), abbreviate));
		characterInfoData2.addValue(GameModifier.getTypeName(17), GameModifier.getTypeValueString(17, GameModifier.getTypeTotal(armoryModifiers, 17), colored: false, identifier: false) + "%");
		characterInfoData2.addValue(GameModifier.getTypeName(18), GameModifier.getTypeValueString(18, GameModifier.getTypeTotal(armoryModifiers, 18), colored: false, identifier: false) + "%");
		characterInfoData2.addValue(GameModifier.getTypeName(19), GameModifier.getTypeValueString(19, GameModifier.getTypeTotal(armoryModifiers, 19), colored: false, identifier: false) + "%");
		characterInfoData2.addValue(GameModifier.getTypeName(4), GameModifier.getTypeValueString(4, GameModifier.getTypeTotal(armoryModifiers, 4), colored: false, identifier: false) + "%");
		characterInfoData2.addValue(GameModifier.getTypeName(5), GameModifier.getTypeValueString(5, GameModifier.getTypeTotal(armoryModifiers, 5), colored: false, identifier: false) + "%");
		characterInfoData2.addValue(GameModifier.getTypeName(3), GameModifier.getTypeValueString(3, GameModifier.getTypeTotal(armoryModifiers, 3), colored: false, identifier: false) + "%");
		characterInfoData2.addValue(GameModifier.getTypeName(1), GameModifier.getTypeValueString(1, GameModifier.getTypeTotal(armoryModifiers, 1), colored: false, identifier: false) + "%");
		characterInfoData2.addValue(GameModifier.getTypeName(16), GameModifier.getTypeValueString(16, GameModifier.getTypeTotal(armoryModifiers, 16), colored: false, identifier: false) + "%");
		characterInfoData2.addValue(GameModifier.getTypeName(21), GameModifier.getTypeValueString(21, GameModifier.getTypeTotal(armoryModifiers, 21), colored: false, identifier: false) + "%");
		characterInfoData2.addValue(GameModifier.getTypeName(25), GameModifier.getTypeValueString(25, GameModifier.getTypeTotal(armoryModifiers, 25), colored: false, identifier: false) + "%");
		characterInfoData2.addValue(GameModifier.getTypeName(43), GameModifier.getTypeValueString(43, GameModifier.getTypeTotal(armoryModifiers, 43), colored: false, identifier: false) + "%");
		characterInfoData2.addValue(GameModifier.getTypeName(32), GameModifier.getTypeValueString(32, GameModifier.getTypeTotal(armoryModifiers, 32), colored: false, identifier: false) + "%");
		characterInfoData2.addValue(GameModifier.getTypeName(26), GameModifier.getTypeValueString(26, GameModifier.getTypeTotal(armoryModifiers, 26), colored: false, identifier: false) + "%");
		characterInfoData2.addValue(GameModifier.getTypeName(27), GameModifier.getTypeValueString(27, GameModifier.getTypeTotal(armoryModifiers, 27), colored: false, identifier: false) + "%");
		characterInfoData2.addValue(GameModifier.getTypeName(36), GameModifier.getTypeValueString(36, GameModifier.getTypeTotal(armoryModifiers, 36), colored: false, identifier: false) + "%");
		characterInfoData2.addValue(GameModifier.getTypeName(52), GameModifier.getTypeValueString(52, GameModifier.getTypeTotal(armoryModifiers, 52), colored: false, identifier: false) + "%");
		characterInfoData2.addValue(GameModifier.getTypeName(127), GameModifier.getTypeValueString(127, GameModifier.getTypeTotal(armoryModifiers, 127), colored: false, identifier: false) + "%");
		characterInfoData2.addValue(GameModifier.getTypeName(84), GameModifier.getTypeValueString(84, GameModifier.getTypeTotal(armoryModifiers, 84), colored: false, identifier: false) + "%");
		characterInfoData2.addValue(GameModifier.getTypeName(85), GameModifier.getTypeValueString(85, GameModifier.getTypeTotal(armoryModifiers, 85), colored: false, identifier: false) + "%");
		characterInfoData2.addValue(GameModifier.getTypeName(88), GameModifier.getTypeValueString(88, GameModifier.getTypeTotal(armoryModifiers, 88), colored: false, identifier: false) + "%");
		characterInfoData2.addValue(GameModifier.getTypeName(89), GameModifier.getTypeValueString(89, GameModifier.getTypeTotal(armoryModifiers, 89), colored: false, identifier: false) + "%");
		characterInfoData2.addValue(GameModifier.getTypeName(86), GameModifier.getTypeValueString(86, GameModifier.getTypeTotal(armoryModifiers, 86), colored: false, identifier: false) + "%");
		characterInfoData2.addValue(GameModifier.getTypeName(87), GameModifier.getTypeValueString(87, GameModifier.getTypeTotal(armoryModifiers, 87), colored: false, identifier: false) + "%");
		characterInfoData2.addValue(GameModifier.getTypeName(92), GameModifier.getTypeValueString(92, GameModifier.getTypeTotal(armoryModifiers, 92), colored: false, identifier: false) + "%");
		characterInfoData2.addValue(GameModifier.getTypeName(93), GameModifier.getTypeValueString(93, GameModifier.getTypeTotal(armoryModifiers, 93), colored: false, identifier: false) + "%");
		characterInfoData2.addValue(GameModifier.getTypeName(90), GameModifier.getTypeValueString(90, GameModifier.getTypeTotal(armoryModifiers, 90), colored: false, identifier: false) + "%");
		characterInfoData2.addValue(GameModifier.getTypeName(91), GameModifier.getTypeValueString(91, GameModifier.getTypeTotal(armoryModifiers, 91), colored: false, identifier: false) + "%");
		CharacterInfoData characterInfoData3 = new CharacterInfoData(Language.GetString("ui_bonuses"), outline, offset);
		characterInfoData3.addValue(GameModifier.getTypeName(6), GameModifier.getTypeValueString(6, GameModifier.getTypeTotal(armoryModifiers, 6), colored: false, identifier: false) + "%");
		characterInfoData3.addValue(GameModifier.getTypeName(8), GameModifier.getTypeValueString(8, GameModifier.getTypeTotal(armoryModifiers, 8), colored: false, identifier: false) + "%");
		characterInfoData3.addValue(GameModifier.getTypeName(7), GameModifier.getTypeValueString(7, GameModifier.getTypeTotal(armoryModifiers, 7), colored: false, identifier: false) + "%");
		characterInfoData3.addValue(GameModifier.getTypeName(9), GameModifier.getTypeValueString(9, GameModifier.getTypeTotal(armoryModifiers, 9), colored: false, identifier: false) + "%");
		characterInfoData3.addValue(GameModifier.getTypeName(20), GameModifier.getTypeValueString(20, GameModifier.getTypeTotal(armoryModifiers, 20), colored: false, identifier: false) + "%");
		CharacterInfoData characterInfoData4 = new CharacterInfoData(Language.GetString("ui_other"), outline, offset);
		characterInfoData4.addValue(Language.GetString("stat_pve_wins"), Util.NumberFormat(sfsob.GetInt("cha50"), abbreviate));
		characterInfoData4.addValue(Language.GetString("stat_pve_losses"), Util.NumberFormat(sfsob.GetInt("cha51"), abbreviate));
		characterInfoData4.addValue(Language.GetString("stat_pvp_wins"), Util.NumberFormat(sfsob.GetInt("cha52"), abbreviate));
		characterInfoData4.addValue(Language.GetString("stat_pvp_losses"), Util.NumberFormat(sfsob.GetInt("cha53"), abbreviate));
		characterInfoData4.addValue(Language.GetString("stat_gvg_wins"), Util.NumberFormat(sfsob.GetInt("cha85"), abbreviate));
		characterInfoData4.addValue(Language.GetString("stat_gvg_losses"), Util.NumberFormat(sfsob.GetInt("cha86"), abbreviate));
		characterInfoData4.addValue(Language.GetString("stat_duel_wins"), Util.NumberFormat(sfsob.GetInt("cha69"), abbreviate));
		characterInfoData4.addValue(Language.GetString("stat_duel_losses"), Util.NumberFormat(sfsob.GetInt("cha70"), abbreviate));
		characterInfoData4.addValue(Language.GetString("stat_dungeons_completed"), Util.NumberFormat(sfsob.GetInt("cha54"), abbreviate));
		characterInfoData4.addValue(Language.GetString("stat_raids_completed"), Util.NumberFormat(sfsob.GetInt("cha55"), abbreviate));
		characterInfoData4.addValue(Language.GetString("stat_gauntlets_completed"), Util.NumberFormat(sfsob.GetInt("cha87"), abbreviate));
		characterInfoData4.addValue(Language.GetString("stat_rifts_completed"), Util.NumberFormat(sfsob.GetInt("cha88"), abbreviate));
		characterInfoData4.addValue(Language.GetString("stat_damage_given"), Util.NumberFormat(sfsob.GetLong("cha56"), abbreviate));
		characterInfoData4.addValue(Language.GetString("stat_damage_received"), Util.NumberFormat(sfsob.GetLong("cha57"), abbreviate));
		characterInfoData4.addValue(Language.GetString("stat_healing_given"), Util.NumberFormat(sfsob.GetLong("cha58"), abbreviate));
		characterInfoData4.addValue(Language.GetString("stat_healing_received"), Util.NumberFormat(sfsob.GetLong("cha59"), abbreviate));
		characterInfoData4.addValue(Language.GetString("stat_consumables_used"), Util.NumberFormat(sfsob.GetInt("cha60"), abbreviate));
		characterInfoData4.addValue(Language.GetString("stat_enemies_killed"), Util.NumberFormat(sfsob.GetInt("cha61"), abbreviate));
		List<CharacterInfoData> list = new List<CharacterInfoData>();
		list.Add(characterInfoData);
		list.Add(characterInfoData2);
		list.Add(characterInfoData3);
		list.Add(characterInfoData4);
		CreateTiles(list);
		StartCoroutine(WaitToFixText());
	}

	private void CreateTiles(List<CharacterInfoData> info)
	{
		ClearTiles();
		for (int i = 0; i < info.Count; i++)
		{
			CharacterInfoData statData = info[i];
			CharacterInfoTile characterInfoTile = Object.Instantiate(characterInfoItemPrefab, infoListContent);
			characterInfoTile.LoadDetails(statData);
			_tiles.Add(characterInfoTile);
		}
	}

	private void ClearTiles()
	{
		foreach (CharacterInfoTile tile in _tiles)
		{
			Object.Destroy(tile.gameObject);
		}
		_tiles.Clear();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
	}

	private IEnumerator WaitToFixText()
	{
		yield return new WaitForSeconds(0.1f);
		infoListContent.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.MinSize;
		ForceScrollDown();
	}
}
