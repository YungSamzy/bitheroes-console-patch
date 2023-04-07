using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.item;

public class ItemExchangeWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI itemNameTxt;

	public TextMeshProUGUI exchangeTxt;

	public TextMeshProUGUI qtyTxt;

	public Button minBtn;

	public Button minusBtn;

	public Button plusBtn;

	public Button maxBtn;

	public Button exchangeBtn;

	public ItemIcon itemIcon;

	private ItemData _itemData;

	private int _qtyMax = 1;

	private int _qtyMin = 1;

	private int _qty = 1;

	private Color alpha = new Color(1f, 1f, 1f, 0.5f);

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(ItemData itemData)
	{
		_itemData = itemData;
		if (itemData.itemRef.itemType == 1)
		{
			EquipmentRef equipmentRef = EquipmentBook.Lookup(_itemData.itemRef.id);
			int availableSlot = Equipment.getAvailableSlot(equipmentRef.equipmentType);
			EquipmentRef equipmentSlot = GameData.instance.PROJECT.character.equipment.getEquipmentSlot(availableSlot);
			_ = equipmentRef == equipmentSlot;
		}
		_qtyMax = itemData.qty;
		topperTxt.text = Language.GetString("ui_exchange");
		itemNameTxt.text = _itemData.itemRef.coloredName;
		exchangeTxt.text = Language.GetString("ui_exchange_desc");
		itemIcon.isThumbnail = true;
		itemIcon.SetItemData(itemData, locked: false, itemData.qty);
		itemIcon.SetItemActionType(0);
		minBtn.GetComponentInChildren<TextMeshProUGUI>().text = Util.NumberFormat(_qtyMin);
		minusBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_minus");
		plusBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_plus");
		maxBtn.GetComponentInChildren<TextMeshProUGUI>().text = Util.NumberFormat(_qtyMax);
		exchangeBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_exchange");
		SetQty(_qtyMax);
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void SetQty(int qty)
	{
		if (qty < _qtyMin)
		{
			qty = _qtyMin;
		}
		if (qty > _qtyMax)
		{
			qty = _qtyMax;
		}
		_qty = qty;
		qtyTxt.text = Util.NumberFormat(_qty);
		if (_qty <= _qtyMin)
		{
			Util.SetButton(minBtn, enabled: false);
			Util.SetButton(minusBtn, enabled: false);
		}
		else
		{
			Util.SetButton(minBtn);
			Util.SetButton(minusBtn);
		}
		if (_qty >= _qtyMax)
		{
			Util.SetButton(maxBtn, enabled: false);
			Util.SetButton(plusBtn, enabled: false);
		}
		else
		{
			Util.SetButton(maxBtn);
			Util.SetButton(plusBtn);
		}
	}

	public void OnMinBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SetQty(_qtyMin);
	}

	public void OnMaxBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SetQty(_qtyMax);
	}

	public void OnPlusBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SetQty(_qty + 1);
	}

	public void OnMinusBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SetQty(_qty - 1);
	}

	public void OnExchangeBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoItemExchange();
	}

	private void DoItemExchange()
	{
		List<ItemData> items = new List<ItemData>();
		items.Add(new ItemData(_itemData.itemRef, _qty));
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_exchange_confirm_count", new string[1] { Util.NumberFormat(_qty) }, color: true), null, null, delegate
		{
			OnDialogYes(items);
		});
	}

	private void OnDialogYes(List<ItemData> items)
	{
		GameData.instance.main.ShowLoading();
		List<ItemData> list = new List<ItemData>();
		foreach (ItemData item in items)
		{
			if (item.qty > 0)
			{
				list.Add(item);
			}
		}
		if (list.Count <= 0)
		{
			GameData.instance.windowGenerator.ShowError(Language.GetString("ui_blank_exchange"));
			return;
		}
		MerchantDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(MerchantDALC.ITEM_EXCHANGE), OnItemExchange);
		MerchantDALC.instance.doItemExchange(list);
	}

	private void OnItemExchange(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		MerchantDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(MerchantDALC.ITEM_EXCHANGE), OnItemExchange);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		List<ItemData> list = ItemData.listFromSFSObject(sfsob.GetSFSObject("ite4"));
		List<ItemData> list2 = ItemData.listFromSFSObject(sfsob.GetSFSObject("ite5"));
		GameData.instance.PROJECT.character.addItems(list);
		GameData.instance.PROJECT.character.removeItems(list2);
		KongregateAnalytics.checkEconomyTransaction("Craft Exchange", list2, list, sfsob, "Craft", 2);
		GameData.instance.windowGenerator.ShowItems(list, compare: true, added: true);
		OnClose();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		minBtn.interactable = true;
		minusBtn.interactable = true;
		plusBtn.interactable = true;
		maxBtn.interactable = true;
		exchangeBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		minBtn.interactable = true;
		minusBtn.interactable = true;
		plusBtn.interactable = true;
		maxBtn.interactable = true;
		exchangeBtn.interactable = true;
	}
}
