using System.Collections.Generic;
using System.Linq;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.rarity;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using com.ultrabit.bitheroes.ui.lists.familiarlist;
using com.ultrabit.bitheroes.ui.menu;
using com.ultrabit.bitheroes.ui.tutorial;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.familiar;

public class FamiliarsWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI countTxt;

	public TextMeshProUGUI dropTxt;

	public TextMeshProUGUI emptyTxt;

	public Button fusionBtn;

	public Button stableBtn;

	public Button augmentsBtn;

	public Image rarityDropdown;

	public FamiliarList itemList;

	public TMP_InputField searchTxt;

	public Image fusionBadge_bg;

	public TextMeshProUGUI fusionBadge;

	private List<FamiliarRef> completeFamiliarList;

	private Transform window;

	private List<MyDropdownItemModel> raritiesObjs = new List<MyDropdownItemModel>();

	private MyDropdownItemModel selectedRarity;

	private MenuInterfaceFamiliarTile _menu;

	public override void Start()
	{
		base.Start();
		Disable();
		completeFamiliarList = FamiliarBook.GetCompleteFamiliarList();
		_menu = GameData.instance.PROJECT.menuInterface.GetButton(typeof(MenuInterfaceFamiliarTile)) as MenuInterfaceFamiliarTile;
		GameData.instance.PROJECT.character.inventory.GetItemsByType(6);
		topperTxt.text = ItemRef.GetItemNamePlural(6);
		searchTxt.text = "";
		emptyTxt.text = Language.GetString("ui_familiar_list_empty_filter");
		fusionBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_fusion");
		stableBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_stable");
		augmentsBtn.GetComponentInChildren<TextMeshProUGUI>().text = ItemRef.GetItemNamePlural(15);
		fusionBtn.gameObject.SetActive(VariableBook.GameRequirementMet(19));
		stableBtn.gameObject.SetActive(VariableBook.GameRequirementMet(31));
		augmentsBtn.gameObject.SetActive(GameData.instance.PROJECT.character.hasAugmentOrHad());
		GameData.instance.PROJECT.character.AddListener("INVENTORY_CHANGE", onInventoryChange);
		GameData.instance.PROJECT.character.AddListener("FAMILIAR_STABLE_CHANGE", onFamiliarStableChange);
		GameData.instance.PROJECT.PauseDungeon();
		CreateDropdown();
		CreateTiles();
		UpdateText();
		SCROLL_IN_COMPLETE.AddListener(OnScrollInComplete);
		ListenForBack(OnClose);
		CreateWindow();
	}

	public void UpdateText()
	{
		int num = 0;
		int itemType = ItemRef.getItemType(GameRequirement.GetTypeName(19));
		if (itemType > 0 && GameData.instance.SAVE_STATE.notificationsFusions && !GameData.instance.SAVE_STATE.notificationsDisabled)
		{
			num = GameData.instance.PROJECT.character.inventory.getItemTypeQty(itemType) - GameData.instance.SAVE_STATE.GetSeenSchematics(GameData.instance.PROJECT.character.id).Count;
		}
		fusionBadge.text = num.ToString();
		fusionBadge_bg.gameObject.SetActive(num > 0);
		fusionBadge.gameObject.SetActive(num > 0);
	}

	private void OnScrollInComplete(object e)
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		CheckTutorial();
	}

	public void CheckTutorial()
	{
		if (!(GameData.instance.tutorialManager == null) && !GameData.instance.tutorialManager.hasPopup && !(GameData.instance.tutorialManager.canvas == null))
		{
			if (GameData.instance.PROJECT.character.tutorial.GetState(65) && !GameData.instance.PROJECT.character.tutorial.GetState(66) && fusionBtn.gameObject.activeSelf)
			{
				GameData.instance.PROJECT.character.tutorial.SetState(66);
				GameData.instance.tutorialManager.ShowTutorialForButton(fusionBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(66), 4, fusionBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
			}
			if (GameData.instance.PROJECT.character.tutorial.GetState(68) && !GameData.instance.PROJECT.character.tutorial.GetState(72) && augmentsBtn.gameObject.activeSelf)
			{
				GameData.instance.PROJECT.character.tutorial.SetState(72);
				GameData.instance.tutorialManager.ShowTutorialForButton(augmentsBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(72), 3, augmentsBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
			}
		}
	}

	private void onFamiliarStableChange()
	{
		CreateTiles();
	}

	private void onInventoryChange()
	{
		augmentsBtn.gameObject.SetActive(GameData.instance.PROJECT.character.hasAugmentOrHad());
		CreateTiles();
		UpdateText();
	}

	public void OnSearchChange()
	{
		CancelInvoke("DoSearch");
		Invoke("DoSearch", Util.SEARCHBOX_ACTION_DELAY);
	}

	private void DoSearch()
	{
		CreateTiles();
	}

	private void CreateDropdown()
	{
		selectedRarity = new MyDropdownItemModel
		{
			id = -1,
			title = Language.GetString("ui_all")
		};
		raritiesObjs.Add(selectedRarity);
		dropTxt.text = raritiesObjs[0].title;
		for (int i = 0; i < RarityBook.size; i++)
		{
			RarityRef rarityRef = RarityBook.LookupID(i);
			if (rarityRef != null && FamiliarBook.GetRarityCount(rarityRef) > 0)
			{
				raritiesObjs.Add(new MyDropdownItemModel
				{
					id = i,
					title = rarityRef.coloredName,
					data = rarityRef
				});
			}
		}
	}

	public void OnRarityDropdown()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		window = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_sort"));
		DropdownList componentInChildren = window.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, selectedRarity.id, OnRarityDropdownClick);
		componentInChildren.ClearList();
		componentInChildren.Data.InsertItemsAtEnd(raritiesObjs);
	}

	public void OnRarityDropdownClick(MyDropdownItemModel model)
	{
		selectedRarity = model;
		dropTxt.text = model.title;
		CreateTiles();
		if (window != null)
		{
			window.GetComponent<DropdownWindow>().OnClose();
		}
	}

	private void CreateTiles()
	{
		itemList.StartList();
		itemList.ClearList();
		RarityRef rarityRef = selectedRarity.data as RarityRef;
		List<FamiliarRef> list = new List<FamiliarRef>();
		for (int i = 0; i < completeFamiliarList.Count; i++)
		{
			FamiliarRef familiarRef = completeFamiliarList[i];
			if (!(familiarRef == null) && familiarRef.obtainable && (rarityRef == null || familiarRef.rarityRef.id == rarityRef.id))
			{
				list.Add(familiarRef);
			}
		}
		list = (from familiar in list
			orderby familiar.rarityRef.id, familiar.id
			select familiar).ToList();
		int num = 0;
		int num2 = 0;
		List<FamiliarListItem> list2 = new List<FamiliarListItem>();
		Dictionary<int, bool> seenFamiliars = GameData.instance.SAVE_STATE.GetSeenFamiliars(GameData.instance.PROJECT.character.id);
		foreach (FamiliarRef item in list)
		{
			string text = searchTxt.text;
			bool flag = true;
			if (flag && text.Length > 0 && item.name.ToLowerInvariant().IndexOf(text.ToLowerInvariant()) < 0)
			{
				flag = false;
			}
			if (flag)
			{
				list2.Add(new FamiliarListItem
				{
					itemData = new ItemData(item, GameData.instance.PROJECT.character.getItemQty(item)),
					seen = seenFamiliars.ContainsKey(item.id)
				});
			}
			if (GameData.instance.PROJECT.character.inventory.hasOwnedItem(item))
			{
				num2++;
			}
			num++;
		}
		countTxt.text = Util.NumberFormat(num2) + "/" + Util.NumberFormat(num);
		itemList.Data.InsertItems(0, list2);
		emptyTxt.gameObject.SetActive(itemList.Data.Count == 0);
	}

	public void OnFusionBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (GameData.instance.PROJECT.dungeon != null)
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("dungeon_restriction"));
		}
		else if (GameData.instance.PROJECT.CheckGameRequirement(19))
		{
			GameData.instance.PROJECT.ShowFusionWindow(base.gameObject);
		}
	}

	public void OnStableBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (GameData.instance.PROJECT.dungeon != null)
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("dungeon_restriction"));
		}
		else if (GameData.instance.PROJECT.CheckGameRequirement(31))
		{
			GameData.instance.PROJECT.ShowFamiliarStableWindow(base.gameObject);
		}
	}

	public void OnAugmentsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (GameData.instance.PROJECT.dungeon != null)
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("dungeon_restriction"));
		}
		else
		{
			GameData.instance.PROJECT.ShowAugmentsWindow(base.gameObject);
		}
	}

	public override void OnClose()
	{
		GameData.instance.SAVE_STATE.SetSeenFamiliars(GameData.instance.PROJECT.character.id, itemList.seen);
		if (_menu != null)
		{
			_menu.UpdateText();
		}
		base.OnClose();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		fusionBtn.interactable = true;
		stableBtn.interactable = true;
		augmentsBtn.interactable = true;
		rarityDropdown.GetComponent<EventTrigger>().enabled = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		fusionBtn.interactable = false;
		stableBtn.interactable = false;
		augmentsBtn.interactable = false;
		rarityDropdown.GetComponent<EventTrigger>().enabled = false;
	}

	public override void DoDestroy()
	{
		GameData.instance.PROJECT.character.RemoveListener("INVENTORY_CHANGE", onInventoryChange);
		GameData.instance.PROJECT.character.RemoveListener("FAMILIAR_STABLE_CHANGE", onFamiliarStableChange);
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		GameData.instance.PROJECT.ResumeDungeon();
		base.DoDestroy();
	}
}
