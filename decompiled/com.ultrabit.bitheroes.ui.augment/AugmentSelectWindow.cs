using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.augment;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.lists.augmentslist;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using com.ultrabit.bitheroes.ui.tutorial;
using com.ultrabit.bitheroes.ui.utility;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.augment;

public class AugmentSelectWindow : WindowsMain
{
	public const int SORT_TYPE = 0;

	public const int SORT_RARITY = 1;

	private static Dictionary<int, string> SORT_NAMES = new Dictionary<int, string>
	{
		[0] = "ui_type",
		[1] = "ui_rarity"
	};

	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI countTxt;

	public Button helpBtn;

	public Button createBtn;

	public Button filterBtn;

	public TextMeshProUGUI emptyTextDesc;

	public Image sortDropdown;

	public AugmentsList augmentsList;

	private Augments _augments;

	private bool _changeable;

	private FamiliarRef _familiarRef;

	private int _slot;

	private bool _hasFilteredAugments;

	private int currentSort;

	private Transform dropdownWindow;

	private ItemAugmentFilterWindow _panel;

	[HideInInspector]
	public UnityCustomEvent SELECT = new UnityCustomEvent();

	public FamiliarRef familiarRef => _familiarRef;

	public int slot => _slot;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(Augments augments, bool changeable = false, FamiliarRef familiarRef = null, int slot = -1)
	{
		_augments = augments;
		_changeable = changeable;
		_familiarRef = familiarRef;
		_slot = slot;
		topperTxt.text = ItemRef.GetItemNamePlural(15);
		helpBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_question_mark");
		createBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_identify");
		filterBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_filter");
		if (_changeable)
		{
			GameData.instance.PROJECT.character.AddListener("AUGMENTS_CHANGE", OnAugmentsChange);
			GameData.instance.PROJECT.character.augments.OnChange.AddListener(OnAugmentsChange);
		}
		sortDropdown.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString(SORT_NAMES[currentSort]);
		augmentsList.InitList(OnAugmentWindowSelected);
		CreateList();
		UpdateText();
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
		if (!(GameData.instance.tutorialManager == null) && !GameData.instance.tutorialManager.hasPopup && !(GameData.instance.tutorialManager.canvas == null) && GameData.instance.PROJECT.character.tutorial.GetState(72) && !GameData.instance.PROJECT.character.tutorial.GetState(73))
		{
			GameData.instance.PROJECT.character.tutorial.SetState(73);
			GameData.instance.tutorialManager.ShowTutorialForButton(createBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(73), 4, createBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
		}
	}

	public void OnSortDropdown()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		dropdownWindow = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_sort"));
		DropdownList componentInChildren = dropdownWindow.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, currentSort, OnSortDropdownChange);
		foreach (int key in SORT_NAMES.Keys)
		{
			componentInChildren.Data.InsertOneAtEnd(new MyDropdownItemModel
			{
				id = key,
				title = Language.GetString(SORT_NAMES[key]),
				btnHelp = false
			});
		}
	}

	private void OnSortDropdownChange(MyDropdownItemModel model)
	{
		currentSort = model.id;
		sortDropdown.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString(SORT_NAMES[currentSort]);
		CreateList();
		if (dropdownWindow != null)
		{
			dropdownWindow.GetComponent<DropdownWindow>().OnClose();
		}
	}

	public void OnHelpBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewTextWindow(ItemRef.GetItemNamePlural(15), Util.parseMultiLine(Language.GetString("augment_help_desc")));
	}

	public void OnCreateBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (GameData.instance.PROJECT.character.augments.augments.Count >= VariableBook.augmentMax)
		{
			GameData.instance.windowGenerator.ShowErrorCode(119);
			return;
		}
		AugmentSlotRef augmentSlotRef = AugmentBook.LookupSlot(_slot);
		List<ItemData> itemsByType = GameData.instance.PROJECT.character.inventory.GetItemsByType(15, augmentSlotRef?.typeRef.id ?? (-1));
		if (itemsByType.Count <= 0)
		{
			GameData.instance.windowGenerator.showAugmentError(Language.GetString("error_augment_insert_unavailable"));
		}
		else
		{
			GameData.instance.windowGenerator.ShowItems(itemsByType, compare: false, added: true, Language.GetString("ui_identify"), large: true, forceNonEquipment: true, Util.parseMultiLine(Language.GetString("augment_help_desc")));
		}
	}

	public void OnFilterBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_panel = GameData.instance.windowGenerator.NewItemAugmentFilterWindow();
		_panel.OnEventClose.AddListener(OnFilterWindow);
	}

	private void OnFilterWindow()
	{
		CreateList();
		UpdateText();
	}

	private void OnAugmentsChange()
	{
		CreateList();
		UpdateText();
	}

	public void CreateList()
	{
		augmentsList.ClearList();
		augmentsList._augments = _augments;
		augmentsList._slot = _slot;
		augmentsList._changeable = _changeable;
		augmentsList._familiarRef = _familiarRef;
		AugmentSlotRef augmentSlotRef = AugmentBook.LookupSlot(_slot);
		List<AugmentData> augments = _augments.augments;
		List<AugmentsListTile> list = new List<AugmentsListTile>();
		_hasFilteredAugments = false;
		foreach (AugmentData item in augments)
		{
			AugmentsListTile augmentsListTile = new AugmentsListTile();
			int augmentFilter = GameData.instance.SAVE_STATE.GetAugmentFilter(GameData.instance.PROJECT.character.id);
			AdvancedFilterSettings augmentAdvancedFilter = GameData.instance.SAVE_STATE.GetAugmentAdvancedFilter(GameData.instance.PROJECT.character.id);
			augmentsListTile._augmentData = item;
			augmentsListTile.subtype = item.augmentRef.subtype;
			augmentsListTile.rarity = item.augmentRef.rarityRef.id;
			augmentsListTile.alpha = 1;
			if (augmentSlotRef != null && item.augmentRef.typeRef.id != augmentSlotRef.typeRef.id)
			{
				augmentsListTile.alpha = 0;
			}
			else if (_augments.getFamiliarAugmentSlot(_familiarRef, _slot) == item)
			{
				augmentsListTile.alpha = 0;
			}
			if (GameData.instance.SAVE_STATE.GetIsAugmentFiltered(GameData.instance.PROJECT.character.id, item, augmentFilter, augmentAdvancedFilter, ItemAugmentFilterWindow.AUGMENT_SUBTYPE_FILTERS))
			{
				_hasFilteredAugments = true;
			}
			else
			{
				list.Add(augmentsListTile);
			}
		}
		string[] names = null;
		switch (currentSort)
		{
		case 1:
			names = new string[3] { "alpha", "rarity", "subtype" };
			break;
		case 0:
			names = new string[3] { "alpha", "subtype", "rarity" };
			break;
		}
		emptyTextDesc.gameObject.SetActive(list.Count <= 0);
		if (emptyTextDesc.gameObject.activeSelf)
		{
			if (_hasFilteredAugments)
			{
				emptyTextDesc.SetText(Language.GetString("ui_no_filtered_augments"));
			}
			else
			{
				emptyTextDesc.SetText(Language.GetString("ui_no_identified_augments"));
			}
		}
		List<AugmentsListTile> list2 = Util.SortVector(list, names, Util.ARRAY_DESCENDING);
		List<AugmentItem> list3 = new List<AugmentItem>();
		for (int i = 0; i < list2.Count; i++)
		{
			list3.Add(new AugmentItem
			{
				augmentData = list2[i]._augmentData,
				slotRef = augmentSlotRef
			});
		}
		augmentsList.Data.InsertItemsAtStart(list3);
	}

	private bool IsInFilters(AugmentData augmentData)
	{
		if (IsInRarityFilter(augmentData) && IsInTypeFilter(augmentData))
		{
			return true;
		}
		return false;
	}

	private bool IsInTypeFilter(AugmentData augmentData)
	{
		return false;
	}

	private bool IsInRarityFilter(AugmentData augmentData)
	{
		return true;
	}

	private void UpdateText()
	{
		countTxt.text = Util.NumberFormat(_augments.augments.Count) + "/" + Util.NumberFormat(VariableBook.augmentMax);
	}

	private void OnAugmentWindowSelected(BaseModelData itemData)
	{
		SELECT.Invoke(itemData);
	}

	public override void DoDestroy()
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		if (_changeable)
		{
			GameData.instance.PROJECT.character.RemoveListener("AUGMENTS_CHANGE", OnAugmentsChange);
			GameData.instance.PROJECT.character.augments.OnChange.RemoveListener(OnAugmentsChange);
		}
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		helpBtn.interactable = true;
		createBtn.interactable = true;
		filterBtn.interactable = true;
		sortDropdown.GetComponent<EventTrigger>().enabled = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		helpBtn.interactable = false;
		createBtn.interactable = false;
		filterBtn.interactable = false;
		sortDropdown.GetComponent<EventTrigger>().enabled = false;
	}
}
