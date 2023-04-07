using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.craft;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.mount;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.item;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.mount;

public class MountUpgradeWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI upgradeTxt;

	public TextMeshProUGUI upgradeDescTxt;

	public TextMeshProUGUI costTxt;

	public Button upgradeBtn;

	public MountDataTile mountDataTile;

	public Transform costList;

	private MountData _mountData;

	public GameObject requerimentBase;

	private List<GameObject> requirementsAdded;

	public ItemTooltipStatTile[] stats;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(MountData mountData)
	{
		_mountData = mountData;
		topperTxt.text = _mountData.mountRef.coloredName;
		upgradeDescTxt.text = Language.GetString("ui_upgrade_desc", new string[1] { ItemRef.GetItemName(8) });
		costTxt.text = "<color=#66FF66>" + Language.GetString("ui_cost") + ":</color>";
		upgradeBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_upgrade");
		UpdateInformation();
		ListenForBack(OnClose);
		CreateWindow();
	}

	public void UpdateInformation()
	{
		if (requirementsAdded == null)
		{
			requirementsAdded = new List<GameObject>();
		}
		else
		{
			foreach (GameObject item in requirementsAdded)
			{
				Object.Destroy(item);
			}
		}
		requirementsAdded.Clear();
		UpdateText();
		CraftUpgradeRef upgradeRef = _mountData.getUpgradeRef();
		if (upgradeRef != null)
		{
			foreach (ItemData requiredItem in upgradeRef.requiredItems)
			{
				GameObject obj = Object.Instantiate(requerimentBase);
				ItemCraftTile component = obj.GetComponent<ItemCraftTile>();
				component.LoadDetails(requiredItem);
				obj.transform.SetParent(costList, worldPositionStays: false);
				requirementsAdded.Add(component.gameObject);
			}
		}
		if (upgradeRef != null && upgradeRef.RequirementsMet())
		{
			Util.SetButton(upgradeBtn);
		}
		else
		{
			Util.SetButton(upgradeBtn, enabled: false);
		}
		if (_mountData.rank < _mountData.rankMax)
		{
			MountData mountData = new MountData(0L, _mountData.mountRef, _mountData.powerMult, _mountData.staminaMult, _mountData.agilityMult, _mountData.modifiers, _mountData.rank + 1, _mountData.rerolls);
			int stat = mountData?.power ?? _mountData.power;
			int stat2 = mountData?.stamina ?? _mountData.stamina;
			int stat3 = mountData?.agility ?? _mountData.agility;
			int power = _mountData.power;
			int stamina = _mountData.stamina;
			int agility = _mountData.agility;
			stats[0].LoadDetails(ItemTooltipStatTile.STAT_TYPE.POWER, stat, power);
			stats[1].LoadDetails(ItemTooltipStatTile.STAT_TYPE.STAMINA, stat2, stamina);
			stats[2].LoadDetails(ItemTooltipStatTile.STAT_TYPE.AGILITY, stat3, agility);
		}
		mountDataTile.SetMountDataTime(_mountData, 1, hasAbilities: true);
	}

	public override void UpdateSortingLayers(int layer)
	{
		base.UpdateSortingLayers(layer);
		mountDataTile.SetLayer(layer);
	}

	public void UpgradeBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (GameData.instance.PROJECT.dungeon != null)
		{
			GameData.instance.windowGenerator.ShowErrorCode(109);
			return;
		}
		if (GameData.instance.PROJECT.battle != null)
		{
			GameData.instance.windowGenerator.ShowErrorCode(110);
			return;
		}
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_upgrade_confirm", new string[1] { _mountData.mountRef.coloredName }), null, null, delegate
		{
			CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(42), OnUpgrade);
			CharacterDALC.instance.doMountUpgrade(_mountData);
		});
	}

	private void OnUpgrade(BaseEvent baseEvent)
	{
		DALCEvent obj = baseEvent as DALCEvent;
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(42), OnUpgrade);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		MountData mountData = MountData.fromSFSObject(sfsob);
		List<ItemData> list = ItemData.listFromSFSObject(sfsob.GetSFSObject("ite5"));
		GameData.instance.PROJECT.character.removeItems(list);
		GameData.instance.PROJECT.character.mounts.updateMount(mountData);
		ItemListWindow itemListWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(ItemListWindow)) as ItemListWindow;
		if (itemListWindow != null)
		{
			itemListWindow.RemoveItems(list);
		}
		KongregateAnalytics.checkEconomyTransaction("Craft Upgrade", list, new List<ItemData>(), sfsob, "Craft", 1);
		GameData.instance.audioManager.PlaySoundLink("upgradeitem");
		_mountData = mountData;
		if (_mountData.rank >= _mountData.rankMax)
		{
			OnClose();
		}
		else
		{
			UpdateInformation();
		}
	}

	private void UpdateText()
	{
		upgradeTxt.text = "<color=#FFFF99>" + Language.GetString("ui_upgrade");
		TextMeshProUGUI textMeshProUGUI = upgradeTxt;
		textMeshProUGUI.text = textMeshProUGUI.text + " (" + Util.NumberFormat(_mountData.rank) + "/" + Util.NumberFormat(_mountData.rankMax) + ")</color>";
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		upgradeBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		upgradeBtn.interactable = false;
	}
}
