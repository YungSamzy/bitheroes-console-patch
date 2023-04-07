using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.craft;
using com.ultrabit.bitheroes.model.enchant;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.game;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.tutorial;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.enchant;

public class EnchantWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI rerollDescTxt;

	public TextMeshProUGUI costTxt;

	public TextMeshProUGUI rerollTxt;

	public Button destroyBtn;

	public Button equipBtn;

	public Button unequipBtn;

	public Button rerollBtn;

	private EnchantData enchantData;

	public TextMeshProUGUI txtPower;

	public TextMeshProUGUI txtStamina;

	public TextMeshProUGUI txtAgility;

	public ItemIcon mainItemIcon;

	public GameObject gameModifierBtnBase;

	public GameObject rerollReqBase;

	public Transform costList;

	private List<GameObject> createdStats;

	public override void Start()
	{
		base.Start();
		Disable();
		if (topperTxt != null)
		{
			topperTxt.text = ItemRef.GetItemName(11);
		}
		if (enchantData.allowReroll)
		{
			if (rerollDescTxt != null)
			{
				rerollDescTxt.text = Language.GetString("ui_reroll_desc", new string[1] { ItemRef.GetItemName(11) });
			}
			if (costTxt != null)
			{
				costTxt.text = "<color=#66FF66>" + Language.GetString("ui_cost") + ":</color>";
			}
		}
		else
		{
			if (rerollDescTxt != null)
			{
				rerollDescTxt.text = Language.GetString("ui_no_reroll_desc");
			}
			if (costTxt != null)
			{
				costTxt.text = "";
			}
		}
		if (destroyBtn != null && destroyBtn.gameObject != null)
		{
			destroyBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_exchange");
		}
		if (equipBtn != null && equipBtn.gameObject != null)
		{
			equipBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_equip");
		}
		if (unequipBtn != null && unequipBtn.gameObject != null)
		{
			unequipBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_unequip");
		}
		if (rerollBtn != null && rerollBtn.gameObject != null)
		{
			rerollBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_reroll");
		}
		SCROLL_IN_COMPLETE.AddListener(OnScrollInComplete);
		ListenForBack(OnClose);
		CreateWindow();
	}

	public void LoadDetails(EnchantData enchantData)
	{
		if (createdStats == null)
		{
			createdStats = new List<GameObject>();
		}
		else
		{
			foreach (GameObject createdStat in createdStats)
			{
				Object.Destroy(createdStat);
			}
			createdStats.Clear();
		}
		this.enchantData = enchantData;
		bool enchantEquipped = GameData.instance.PROJECT.character.enchants.getEnchantEquipped(enchantData);
		unequipBtn.gameObject.SetActive(enchantEquipped);
		equipBtn.gameObject.SetActive(!enchantEquipped);
		Util.SetButton(destroyBtn, !enchantEquipped);
		string text = Language.GetString("ui_reroll");
		if (enchantData.rerolls > 0)
		{
			text = text + "(" + Util.NumberFormat(enchantData.rerolls) + ")";
		}
		rerollTxt.text = "<color=#FFFF99>" + text + "</color>";
		txtPower.text = Util.NumberFormat(enchantData.power);
		txtStamina.text = Util.NumberFormat(enchantData.stamina);
		txtAgility.text = Util.NumberFormat(enchantData.agility);
		mainItemIcon.SetEnchantData(enchantData);
		mainItemIcon.SetItemActionType(0);
		if (enchantData.modifiers != null && enchantData.modifiers.Count > 0)
		{
			foreach (EnchantModifierRef modifier in enchantData.modifiers)
			{
				foreach (GameModifier modifier2 in modifier.modifiers)
				{
					GameObject gameObject = Object.Instantiate(gameModifierBtnBase);
					gameObject.GetComponent<GameModifierBtn>().SetText(modifier2.GetTileDesc());
					gameObject.transform.parent = gameModifierBtnBase.transform.parent;
					gameObject.transform.localScale = Vector3.one;
					gameObject.SetActive(value: true);
					createdStats.Add(gameObject);
				}
			}
		}
		CraftRerollRef itemRerollRef = CraftBook.GetItemRerollRef(enchantData.enchantRef);
		if (enchantData.allowReroll)
		{
			foreach (ItemData requiredItem in itemRerollRef.requiredItems)
			{
				GameObject gameObject2 = Object.Instantiate(rerollReqBase);
				gameObject2.GetComponent<ItemCraftTile>().LoadDetails(requiredItem, locked: false, enchantData.rerolls + 1);
				gameObject2.transform.SetParent(costList, worldPositionStays: false);
				gameObject2.transform.localScale = Vector3.one;
				gameObject2.SetActive(value: true);
				createdStats.Add(gameObject2);
				bool flag = itemRerollRef.RequirementsMet(enchantData.rerolls);
				Util.SetButton(rerollBtn, flag);
			}
			return;
		}
		Util.SetButton(rerollBtn, enabled: false);
	}

	private void OnScrollInComplete(object e)
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		CheckTutorial();
	}

	private void CheckTutorial()
	{
		if (!(GameData.instance.tutorialManager == null) && !GameData.instance.tutorialManager.hasPopup && !(GameData.instance.tutorialManager.canvas == null) && !GameData.instance.PROJECT.character.tutorial.GetState(98))
		{
			GameData.instance.PROJECT.character.tutorial.SetState(98);
			GameData.instance.tutorialManager.ShowTutorialForButton(equipBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(98), 4, equipBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
			GameData.instance.PROJECT.CheckTutorialChanges();
		}
	}

	public void OnDestroyBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_destroy_confirm", new string[1] { enchantData.enchantRef.coloredName }), null, null, delegate
		{
			DoEnchantDestroy();
		});
	}

	private void DoEnchantDestroy()
	{
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(31), OnEnchantDestroy);
		CharacterDALC.instance.doEnchantDestroy(enchantData);
	}

	private void OnEnchantDestroy(BaseEvent baseEvent)
	{
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(31), OnEnchantDestroy);
		SFSObject sfsob = (baseEvent as DALCEvent).sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		long @long = sfsob.GetLong("ench1");
		List<long> enchantSlots = Util.arrayToNumberVector(sfsob.GetLongArray("ench8"));
		List<ItemData> list = ItemData.listFromSFSObject(sfsob);
		GameData.instance.PROJECT.character.enchants.setEnchantSlots(enchantSlots);
		GameData.instance.PROJECT.character.enchants.removeEnchant(@long);
		if (list.Count > 0)
		{
			GameData.instance.PROJECT.character.addItems(list);
			GameData.instance.windowGenerator.ShowItems(list, compare: true, added: true);
		}
		base.OnClose();
	}

	public void OnEquipBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewEnchantSlotSelectWindow(enchantData, OnEquippedComplete);
	}

	private void OnEquippedComplete()
	{
		equipBtn.gameObject.SetActive(value: false);
		unequipBtn.gameObject.SetActive(value: true);
		Util.SetButton(destroyBtn, enabled: false);
		mainItemIcon.PlayComparison("E");
	}

	public void OnUnequipBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		equipBtn.gameObject.SetActive(value: true);
		unequipBtn.gameObject.SetActive(value: false);
		Util.SetButton(destroyBtn);
		int enchantSlot = GameData.instance.PROJECT.character.enchants.getEnchantSlot(enchantData);
		GameData.instance.windowGenerator.enchantsWindow.DoEnchantEquip(enchantSlot, null);
		mainItemIcon.HideComparison();
	}

	public void OnRerollBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_reroll_confirm", new string[1] { enchantData.enchantRef.coloredName }), null, null, delegate
		{
			DoEnchantReroll();
		});
	}

	private void DoEnchantReroll()
	{
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(32), OnEnchantReroll);
		CharacterDALC.instance.doEnchantReroll(enchantData);
	}

	private void OnEnchantReroll(BaseEvent baseEvent)
	{
		DALCEvent obj = baseEvent as DALCEvent;
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(32), OnEnchantReroll);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		EnchantData enchantData = EnchantData.fromSFSObject(sfsob);
		List<ItemData> items = ItemData.listFromSFSObject(sfsob.GetSFSObject("ite5"));
		GameData.instance.PROJECT.character.enchants.updateEnchant(enchantData);
		GameData.instance.PROJECT.character.removeItems(items);
		LoadDetails(enchantData);
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
		destroyBtn.interactable = true;
		equipBtn.interactable = true;
		unequipBtn.interactable = true;
		rerollBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		destroyBtn.interactable = false;
		equipBtn.interactable = false;
		unequipBtn.interactable = false;
		rerollBtn.interactable = false;
	}
}
