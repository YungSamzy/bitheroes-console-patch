using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.craft;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.mount;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using com.ultrabit.bitheroes.ui.lists.mountselectlist;
using com.ultrabit.bitheroes.ui.tutorial;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.mount;

public class MountSelectWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI countTxt;

	public TextMeshProUGUI dropTxt;

	public Button helpBtn;

	public Button cosmeticBtn;

	public Button tradeBtn;

	public Button createBtn;

	public Image sortDropdown;

	private Mounts _mounts;

	private bool _changeable;

	private bool _equippable;

	private Transform dropdownWindow;

	public MountSelectList mountSelectList;

	private bool _isArmory;

	public const int SORT_STATS = 0;

	public const int SORT_POWER = 1;

	public const int SORT_STAMINA = 2;

	public const int SORT_AGILITY = 3;

	public const int SORT_RARITY = 4;

	private static Dictionary<int, string> SORT_NAMES = new Dictionary<int, string>
	{
		{
			0,
			Language.GetString("ui_stats")
		},
		{
			1,
			Language.GetString("stat_power")
		},
		{
			2,
			Language.GetString("stat_stamina")
		},
		{
			3,
			Language.GetString("stat_agility")
		},
		{
			4,
			Language.GetString("ui_rarity")
		}
	};

	public override void Start()
	{
		topperTxt.text = ItemRef.GetItemNamePlural(8);
		helpBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_question_mark");
		tradeBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_craft");
		createBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_summon");
		int mountSelectSort = GameData.instance.SAVE_STATE.GetMountSelectSort(GameData.instance.PROJECT.character.id);
		if (SORT_NAMES.ContainsKey(mountSelectSort))
		{
			dropTxt.text = Language.GetString(SORT_NAMES[mountSelectSort]);
		}
		else
		{
			dropTxt.text = Language.GetString("ui_sort");
		}
		base.Start();
		Disable();
	}

	public void LoadDetails(Mounts mounts, bool changeable = false, bool equippable = false, bool isArmory = false)
	{
		_mounts = mounts;
		_changeable = changeable;
		_equippable = equippable;
		_isArmory = isArmory;
		mountSelectList.InitList();
		GameData.instance.PROJECT.character.mounts.OnChange.AddListener(CreateTiles);
		_ = _changeable;
		if (_isArmory)
		{
			tradeBtn.gameObject.SetActive(value: false);
			createBtn.gameObject.SetActive(value: false);
		}
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
		if (!(GameData.instance.tutorialManager == null) && !GameData.instance.tutorialManager.hasPopup && !(GameData.instance.tutorialManager.canvas == null) && _changeable && _equippable && !GameData.instance.PROJECT.character.tutorial.GetState(53))
		{
			GameData.instance.PROJECT.character.tutorial.SetState(53);
			GameData.instance.tutorialManager.ShowTutorialForButton(createBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(53), 4, createBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
			GameData.instance.PROJECT.CheckTutorialChanges();
		}
	}

	public void OnSortDropdown()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		dropdownWindow = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_sort"));
		DropdownList componentInChildren = dropdownWindow.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, GameData.instance.SAVE_STATE.GetMountSelectSort(GameData.instance.PROJECT.character.id), OnSortSelected);
		componentInChildren.ClearList();
		foreach (KeyValuePair<int, string> sORT_NAME in SORT_NAMES)
		{
			componentInChildren.Data.InsertOneAtEnd(new MyDropdownItemModel
			{
				id = sORT_NAME.Key,
				title = Language.GetString(sORT_NAME.Value)
			});
		}
	}

	private void OnSortSelected(MyDropdownItemModel model)
	{
		GameData.instance.SAVE_STATE.SetMountSelectSort(model.id);
		dropTxt.text = model.title;
		CreateTiles();
		if (dropdownWindow != null)
		{
			dropdownWindow.GetComponent<DropdownWindow>().OnClose();
		}
	}

	private string[] GetSortingProperties()
	{
		return GameData.instance.SAVE_STATE.GetMountSelectSort(GameData.instance.PROJECT.character.id) switch
		{
			0 => new string[2] { "total", "rarity" }, 
			1 => new string[3] { "power", "total", "rarity" }, 
			2 => new string[3] { "stamina", "total", "rarity" }, 
			3 => new string[3] { "agility", "total", "rarity" }, 
			4 => new string[2] { "rarity", "total" }, 
			_ => null, 
		};
	}

	private void CreateTiles()
	{
		mountSelectList.ClearList();
		List<MountData> list = Util.SortVector(_mounts.mounts, GetSortingProperties(), Util.ARRAY_DESCENDING);
		List<MountListItem> list2 = new List<MountListItem>();
		foreach (MountData item in list)
		{
			if (_isArmory)
			{
				item.mountRef.OverrideItemType(17);
			}
			else
			{
				item.mountRef.OverrideItemType(8);
			}
			list2.Add(new MountListItem
			{
				mountData = item
			});
		}
		mountSelectList.Data.InsertItemsAtEnd(list2);
		countTxt.text = _mounts.mounts.Count + "/" + VariableBook.mountMax;
	}

	public void OnHelpBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewTextWindow(ItemRef.GetItemNamePlural(8), Util.parseMultiLine(Language.GetString("mount_help_desc")), base.gameObject);
	}

	public void OnCosmeticsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		MountData mountData = GameData.instance.PROJECT.character.mounts.getMountEquipped();
		if (_isArmory)
		{
			mountData = GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.GetMountDataEquipped();
		}
		if (mountData == null)
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("error_blank_equipment_cosmetic"));
			return;
		}
		OnClose();
		GameData.instance.windowGenerator.NewCosmeticsWindow(null, mountData.mountRef, -1, _isArmory);
	}

	public void OnTradeBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewCraftTradeWindow(CraftBook.GetTradeRefsByType(1), base.gameObject);
	}

	public void OnCreateBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (GameData.instance.PROJECT.character.mounts.mounts.Count >= VariableBook.mountMax)
		{
			GameData.instance.windowGenerator.ShowErrorCode(117);
		}
		List<ItemData> itemsByType = GameData.instance.PROJECT.character.inventory.GetItemsByType(8);
		GameData.instance.windowGenerator.ShowItems(itemsByType, compare: false, added: true, Language.GetString("ui_summon"), large: true, forceNonEquipment: false, Util.parseMultiLine(Language.GetString("mount_help_desc")));
	}

	public override void DoDestroy()
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		helpBtn.interactable = true;
		cosmeticBtn.interactable = true;
		tradeBtn.interactable = true;
		createBtn.interactable = true;
		sortDropdown.GetComponent<EventTrigger>().enabled = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		helpBtn.interactable = false;
		cosmeticBtn.interactable = false;
		tradeBtn.interactable = false;
		createBtn.interactable = false;
		sortDropdown.GetComponent<EventTrigger>().enabled = false;
	}
}
