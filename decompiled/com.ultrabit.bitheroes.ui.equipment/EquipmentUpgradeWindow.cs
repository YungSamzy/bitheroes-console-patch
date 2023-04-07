using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.ui.lists.equipmentupgradelist;
using com.ultrabit.bitheroes.ui.tutorial;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.equipment;

public class EquipmentUpgradeWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI upgradeTxt;

	private List<ItemData> currentItems;

	private EquipmentRef _equipmentRef;

	public EquipmentUpgradeList itemList;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(EquipmentRef equipmentRef)
	{
		_equipmentRef = equipmentRef;
		topperTxt.text = Language.GetString("ui_upgrade");
		upgradeTxt.text = Language.GetString("ui_select_item_upgrade", new string[1] { _equipmentRef.coloredName });
		itemList.UPGRADE.AddListener(OnItemUpgraded);
		itemList.StartList();
		CreateTiles();
		SCROLL_IN_COMPLETE.AddListener(OnScrollInComplete);
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void OnScrollInComplete(object e)
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		CheckTutorial();
	}

	private void CheckTutorial()
	{
		if (!(GameData.instance.tutorialManager == null) && !GameData.instance.tutorialManager.hasPopup && !(GameData.instance.tutorialManager.canvas == null) && !GameData.instance.PROJECT.character.tutorial.GetState(106) && GameData.instance.PROJECT.character.tutorial.GetState(105))
		{
			MyListItemViewsHolder equippedUpgradeTile = GetEquippedUpgradeTile();
			if (equippedUpgradeTile != null)
			{
				GameData.instance.PROJECT.character.tutorial.SetState(106);
				GameData.instance.tutorialManager.ShowTutorialForButton(equippedUpgradeTile.craftBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(106), 3, equippedUpgradeTile.craftBtn.gameObject), stageTrigger: true, null, funcSameAsTargetFunc: false, null, shadow: false, tween: true);
				GameData.instance.PROJECT.CheckTutorialChanges();
			}
		}
	}

	private MyListItemViewsHolder GetEquippedUpgradeTile()
	{
		for (int i = 0; i < itemList.Data.Count; i++)
		{
			if (itemList.Data[i].upgradeRef.getUpgradeRef().RequirementsMet())
			{
				return itemList.GetBaseItemViewsHolderIfVisible(i) as MyListItemViewsHolder;
			}
		}
		return null;
	}

	private void CreateTiles()
	{
		itemList.ClearList();
		List<UpgradeItemModel> list = new List<UpgradeItemModel>();
		MonoBehaviour.print(_equipmentRef.upgrades.Count);
		for (int i = 0; i < _equipmentRef.upgrades.Count; i++)
		{
			ItemUpgradeRef upgradeRef = _equipmentRef.getUpgradeRef(i);
			if (upgradeRef != null)
			{
				list.Add(new UpgradeItemModel
				{
					equipmentRef = _equipmentRef,
					upgradeRef = upgradeRef
				});
			}
		}
		itemList.Data.InsertItems(0, list);
	}

	private void OnItemUpgraded()
	{
		base.OnClose();
	}

	public override void DoDestroy()
	{
		itemList.UPGRADE.RemoveListener(OnItemUpgraded);
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		base.DoDestroy();
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
}
