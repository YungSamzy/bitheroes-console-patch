using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.enchant;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui.enchant;

public class EnchantSlotSelectWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI descTxt;

	private List<EnchantTile> _tiles;

	private int _selectedSlot = -1;

	public Transform placeholderSlots;

	public GameObject enchantTileBase;

	private EnchantData enchantData;

	private UnityAction onComplete;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = Language.GetString("ui_equip");
		descTxt.text = Language.GetString("ui_enchant_slot_desc", new string[1] { ItemRef.GetItemName(11) });
		ListenForBack(OnClose);
		CreateWindow();
		CreateTiles();
	}

	public void LoadDetails(EnchantData newEnchant, UnityAction onComplete)
	{
		enchantData = newEnchant;
		this.onComplete = onComplete;
	}

	private void CreateTiles()
	{
		Enchants enchants = GameData.instance.PROJECT.character.enchants;
		for (int i = 0; i < 6; i++)
		{
			EnchantData slot = enchants.getSlot(i);
			GameObject gameObject = Object.Instantiate(enchantTileBase);
			EnchantTile itemIcon = gameObject.GetComponent<EnchantTile>();
			itemIcon.LoadDetails(i, enchants, slot, changeable: false, selectable: true);
			if (itemIcon.item != null)
			{
				itemIcon.OverrideClickBehavior(10, OnSlotSelected);
			}
			else
			{
				itemIcon.SetItemActionType(0);
				itemIcon.onClick.RemoveAllListeners();
				itemIcon.onClick.AddListener(delegate
				{
					OnSlotSelected(itemIcon);
				});
			}
			gameObject.transform.parent = placeholderSlots;
			gameObject.transform.localScale = Vector3.one;
			gameObject.SetActive(value: true);
		}
	}

	private void OnSlotSelected(EnchantTile enchantTile)
	{
		if (!GameData.instance.PROJECT.character.enchants.getSlotUnlocked(GameData.instance.PROJECT.character, enchantTile.slot))
		{
			GameData.instance.audioManager.PlaySoundLink("buttonclick");
			EnchantSlotRef enchantSlotRef = EnchantBook.LookupSlot(enchantTile.slot);
			GameData.instance.windowGenerator.ShowError(Language.GetString("error_requirement_level", new string[1] { Util.NumberFormat(enchantSlotRef.levelReq) }));
			return;
		}
		if (GameData.instance.windowGenerator.enchantsWindow != null)
		{
			GameData.instance.windowGenerator.enchantsWindow.DoEnchantEquip(enchantTile.slot, enchantData);
		}
		else
		{
			CharacterDALC.instance.doEnchantEquip(enchantTile.slot, enchantData);
			CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(30), OnEnchantEquip);
		}
		onComplete();
		base.OnClose();
	}

	private void OnEnchantEquip(BaseEvent baseEvent)
	{
		DALCEvent obj = baseEvent as DALCEvent;
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(30), OnEnchantEquip);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		List<long> enchantSlots = Util.arrayToNumberVector(sfsob.GetLongArray("ench8"));
		GameData.instance.PROJECT.character.enchants.setEnchantSlots(enchantSlots);
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
