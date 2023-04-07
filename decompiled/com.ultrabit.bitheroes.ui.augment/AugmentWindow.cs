using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.augment;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.craft;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.tutorial;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.augment;

public class AugmentWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI rerollDescTxt;

	public TextMeshProUGUI costTxt;

	public TextMeshProUGUI rerollTxt;

	public Button helpBtn;

	public Button destroyBtn;

	public Button equipBtn;

	public Button unequipBtn;

	public Button rerollBtn;

	public RectTransform placeholderCost;

	public Transform itemCraftTilePrefab;

	public AugmentDataTile augmentTile;

	private AugmentData _augmentData;

	private FamiliarRef _familiarRef;

	private int _slot;

	private List<ItemCraftTile> _requirementTiles = new List<ItemCraftTile>();

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(AugmentData augmentData)
	{
		_augmentData = augmentData;
		topperTxt.text = ItemRef.GetItemName(15);
		rerollDescTxt.text = Language.GetString("ui_reroll_desc", new string[1] { ItemRef.GetItemName(15) });
		costTxt.text = Language.GetString("ui_cost") + ":";
		helpBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_question_mark");
		destroyBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_exchange");
		equipBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_equip");
		unequipBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_unequip");
		rerollBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_reroll");
		AugmentSelectWindow augmentSelectWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(AugmentSelectWindow)) as AugmentSelectWindow;
		if ((bool)augmentSelectWindow)
		{
			_familiarRef = augmentSelectWindow.familiarRef;
			_slot = augmentSelectWindow.slot;
		}
		AugmentOptionsWindow augmentOptionsWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(AugmentOptionsWindow)) as AugmentOptionsWindow;
		if ((bool)augmentOptionsWindow)
		{
			_familiarRef = augmentOptionsWindow.familiarRef;
			_slot = augmentOptionsWindow.slot;
		}
		DoUpdate();
		SCROLL_IN_COMPLETE.AddListener(OnScrollInComplete);
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void OnScrollInComplete(object e)
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		CheckTutorial();
	}

	public void OnHelpBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewTextWindow(ItemRef.GetItemNamePlural(15), Util.parseMultiLine(Language.GetString("augment_help_desc")));
	}

	public void OnDestroyBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoAugmentDestroyConfirm();
	}

	public void OnEquipBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (_familiarRef != null && _slot >= 0)
		{
			DoAugmentEquip(_augmentData, _familiarRef, _slot);
		}
		else
		{
			DoFamiliarSelect();
		}
	}

	public void OnUnequipBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoAugmentEquip(null, _augmentData.familiarRef, _augmentData.slot);
	}

	public void OnRerollBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoAugmentRerollConfirm();
	}

	private void DoUpdate()
	{
		CreateTile();
		UpdateText();
		if (_augmentData.equipped)
		{
			equipBtn.gameObject.SetActive(value: false);
			unequipBtn.gameObject.SetActive(value: true);
		}
		else
		{
			equipBtn.gameObject.SetActive(value: true);
			unequipBtn.gameObject.SetActive(value: false);
		}
		augmentTile.UpdateItemIconComparission(_augmentData.equipped);
		foreach (ItemCraftTile requirementTile in _requirementTiles)
		{
			Object.Destroy(requirementTile.gameObject);
		}
		_requirementTiles.Clear();
		CraftRerollRef itemRerollRef = CraftBook.GetItemRerollRef(_augmentData.augmentRef);
		if (itemRerollRef != null && itemRerollRef.RequirementsMet(_augmentData.rerolls))
		{
			Util.SetButton(rerollBtn);
		}
		else
		{
			Util.SetButton(rerollBtn, enabled: false);
		}
		if (itemRerollRef == null)
		{
			return;
		}
		foreach (ItemData requiredItem in itemRerollRef.requiredItems)
		{
			Transform obj = Object.Instantiate(itemCraftTilePrefab);
			obj.SetParent(placeholderCost, worldPositionStays: false);
			ItemCraftTile component = obj.GetComponent<ItemCraftTile>();
			component.LoadDetails(requiredItem, locked: false, _augmentData.rerolls + 1);
			_requirementTiles.Add(component);
		}
	}

	private void UpdateText()
	{
		rerollTxt.text = Language.GetString("ui_reroll");
		if (_augmentData.rerolls > 0)
		{
			TextMeshProUGUI textMeshProUGUI = rerollTxt;
			textMeshProUGUI.text = textMeshProUGUI.text + " (" + Util.NumberFormat(_augmentData.rerolls) + ")";
		}
	}

	private void CreateTile()
	{
		augmentTile.LoadDetails(_augmentData);
	}

	private void DoAugmentDestroyConfirm()
	{
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_destroy_confirm", new string[1] { Language.GetString(_augmentData.augmentRef.coloredName) }), null, null, delegate
		{
			DoAugmentDestroy();
		});
	}

	private void DoAugmentDestroy()
	{
		GameData.instance.main.ShowLoading();
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(46), OnAugmentDestroy);
		CharacterDALC.instance.doAugmentDestroy(_augmentData);
	}

	private void OnAugmentDestroy(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(46), OnAugmentDestroy);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		OnClose();
		AugmentOptionsWindow augmentOptionsWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(AugmentOptionsWindow)) as AugmentOptionsWindow;
		if ((bool)augmentOptionsWindow)
		{
			augmentOptionsWindow.OnClose();
		}
		long @long = sfsob.GetLong("aug1");
		List<ItemData> list = ItemData.listFromSFSObject(sfsob);
		GameData.instance.PROJECT.character.augments.removeAugment(@long);
		if (list != null && list.Count > 0)
		{
			GameData.instance.PROJECT.character.addItems(list);
			GameData.instance.windowGenerator.ShowItems(list, compare: true, added: true);
		}
	}

	private void DoAugmentRerollConfirm()
	{
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_reroll_confirm", new string[1] { Language.GetString(_augmentData.augmentRef.coloredName) }), null, null, delegate
		{
			DoAugmentReroll();
		});
	}

	private void DoAugmentReroll()
	{
		GameData.instance.main.ShowLoading();
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(47), OnAugmentReroll);
		CharacterDALC.instance.doAugmentReroll(_augmentData);
	}

	private void OnAugmentReroll(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(47), OnAugmentReroll);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		AugmentData augmentData = AugmentData.fromSFSObject(sfsob);
		List<ItemData> items = ItemData.listFromSFSObject(sfsob.GetSFSObject("ite5"));
		GameData.instance.PROJECT.character.augments.updateAugmentData(augmentData);
		GameData.instance.PROJECT.character.removeItems(items);
		GameData.instance.audioManager.PlaySoundLink("exchange");
		DoUpdate();
	}

	public void DoAugmentEquip(AugmentData augmentData, FamiliarRef familiarRef, int slot)
	{
		GameData.instance.main.ShowLoading();
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(45), OnAugmentEquip);
		CharacterDALC.instance.doAugmentEquip(familiarRef, slot, augmentData);
	}

	private void OnAugmentEquip(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(45), OnAugmentEquip);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		AugmentSlotSelectWindow augmentSlotSelectWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(AugmentSlotSelectWindow)) as AugmentSlotSelectWindow;
		if ((bool)augmentSlotSelectWindow)
		{
			augmentSlotSelectWindow.OnClose();
		}
		GameData.instance.PROJECT.character.augments.updateAugmentList(AugmentData.listFromSFSObject(sfsob));
		if (_augmentData.equipped)
		{
			GameData.instance.audioManager.PlaySoundLink("equip");
		}
		else
		{
			GameData.instance.audioManager.PlaySoundLink("unequip");
		}
		DoUpdate();
	}

	private void DoFamiliarSelect()
	{
		List<ItemData> itemsByType = GameData.instance.PROJECT.character.inventory.GetItemsByType(6, -1, checkQty: false);
		if (itemsByType.Count <= 0)
		{
			return;
		}
		List<AugmentSlotRef> typeSlots = AugmentBook.GetTypeSlots(_augmentData.augmentRef.typeRef.id);
		List<ItemData> list = new List<ItemData>();
		foreach (ItemData item in itemsByType)
		{
			FamiliarRef familiarRef = item.itemRef as FamiliarRef;
			bool flag = false;
			foreach (AugmentSlotRef item2 in typeSlots)
			{
				if (familiarRef.allowAugmentSlot(item2.id))
				{
					flag = true;
				}
			}
			if (flag)
			{
				list.Add(item);
			}
		}
		ItemSearchWindow itemSearchWindow = GameData.instance.windowGenerator.NewItemSearchWindow(list, adminWindow: false, Language.GetString("ui_select"), showQty: false);
		itemSearchWindow.SELECT.AddListener(OnFamiliarSelect);
		itemSearchWindow.SCROLL_OUT_START.AddListener(OnItemSearchWindowClosing);
	}

	private void OnItemSearchWindowClosing(object e)
	{
		ItemSearchWindow obj = e as ItemSearchWindow;
		obj.SELECT.RemoveListener(OnFamiliarSelect);
		obj.SCROLL_OUT_START.RemoveListener(OnItemSearchWindowClosing);
	}

	private void OnFamiliarSelect(object e)
	{
		ItemSearchWindow obj = e as ItemSearchWindow;
		FamiliarRef familiarRef = obj.selectedItem as FamiliarRef;
		obj.OnClose();
		if (familiarRef == null)
		{
			return;
		}
		List<AugmentSlotRef> typeSlots = AugmentBook.GetTypeSlots(_augmentData.augmentRef.typeRef.id);
		if (typeSlots.Count > 0)
		{
			AugmentSlotRef augmentSlotRef = null;
			if (typeSlots.Count == 1)
			{
				augmentSlotRef = typeSlots[0];
				typeSlots.RemoveAt(0);
			}
			if (augmentSlotRef != null)
			{
				DoAugmentEquip(_augmentData, familiarRef, augmentSlotRef.id);
			}
			else
			{
				DoAugmentSlotSelect(familiarRef);
			}
		}
	}

	private void DoAugmentSlotSelect(FamiliarRef familiarRef)
	{
		AugmentSlotSelectWindow augmentSlotSelectWindow = GameData.instance.windowGenerator.NewAugmentSlotSelectWindow(_augmentData, familiarRef);
		augmentSlotSelectWindow.SELECT.AddListener(OnAugmentSlotSelect);
		augmentSlotSelectWindow.SCROLL_OUT_START.AddListener(OnAugmentSlotSelectClosing);
	}

	private void OnAugmentSlotSelectClosing(object e)
	{
		AugmentSlotSelectWindow obj = e as AugmentSlotSelectWindow;
		obj.SELECT.RemoveListener(OnAugmentSlotSelect);
		obj.SCROLL_OUT_START.RemoveListener(OnAugmentSlotSelectClosing);
	}

	private void OnAugmentSlotSelect(object e)
	{
		AugmentSlotSelectWindow augmentSlotSelectWindow = e as AugmentSlotSelectWindow;
		int selectedSlot = augmentSlotSelectWindow.selectedSlot;
		if (selectedSlot < 0)
		{
			augmentSlotSelectWindow.OnClose();
		}
		else
		{
			DoAugmentEquip(_augmentData, augmentSlotSelectWindow.familiarRef, selectedSlot);
		}
	}

	private void CheckTutorial()
	{
		if (!(GameData.instance.tutorialManager == null) && !GameData.instance.tutorialManager.hasPopup && !(GameData.instance.tutorialManager.canvas == null))
		{
			TutorialRef tutorialRef = VariableBook.LookUpTutorial("augment_shop");
			if (tutorialRef != null && tutorialRef.areConditionsMet && equipBtn.gameObject.activeSelf && GameData.instance.PROJECT.character.tutorial.GetState(74) && !GameData.instance.PROJECT.character.tutorial.GetState(75))
			{
				GameData.instance.PROJECT.character.tutorial.SetState(75);
				GameData.instance.tutorialManager.ShowTutorialForButton(equipBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(75), 4, equipBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
				GameData.instance.PROJECT.CheckTutorialChanges();
			}
		}
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
		destroyBtn.interactable = true;
		equipBtn.interactable = true;
		unequipBtn.interactable = true;
		rerollBtn.interactable = true;
		augmentTile.DoEnable();
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		helpBtn.interactable = false;
		destroyBtn.interactable = false;
		equipBtn.interactable = false;
		unequipBtn.interactable = false;
		rerollBtn.interactable = false;
		augmentTile.DoDisable();
	}
}
