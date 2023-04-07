using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.consumable;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.item;

public class ItemUseWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI itemNameTxt;

	public TextMeshProUGUI useTxt;

	public TextMeshProUGUI qtyTxt;

	public Button minBtn;

	public Button minusBtn;

	public Button plusBtn;

	public Button maxBtn;

	public Button useBtn;

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
		bool flag = false;
		if (itemData.itemRef.itemType == 1)
		{
			EquipmentRef equipmentRef = EquipmentBook.Lookup(_itemData.itemRef.id);
			int availableSlot = Equipment.getAvailableSlot(equipmentRef.equipmentType);
			EquipmentRef equipmentSlot = GameData.instance.PROJECT.character.equipment.getEquipmentSlot(availableSlot);
			flag = equipmentRef == equipmentSlot;
		}
		_qtyMax = itemData.qty;
		topperTxt.text = Language.GetString("ui_use");
		itemNameTxt.text = _itemData.itemRef.coloredName;
		useTxt.text = Language.GetString("ui_use_desc");
		itemIcon.SetItemData(itemData);
		itemIcon.SetItemActionType(0);
		if (flag)
		{
			itemIcon.SetupItemComparision(showCosmetic: false, showComparision: true);
		}
		minBtn.GetComponentInChildren<TextMeshProUGUI>().text = Util.NumberFormat(_qtyMin);
		minusBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_minus");
		plusBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_plus");
		maxBtn.GetComponentInChildren<TextMeshProUGUI>().text = Util.NumberFormat(_qtyMax);
		useBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_use");
		SetQty(_qtyMin);
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

	public void OnUseBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		ConsumableManager.instance.SetupConsumable(new ItemData(_itemData.itemRef, _qty));
		ConsumableManager.instance.DoUseConsumableConfirm();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		minBtn.interactable = true;
		minusBtn.interactable = true;
		plusBtn.interactable = true;
		maxBtn.interactable = true;
		useBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		minBtn.interactable = true;
		minusBtn.interactable = true;
		plusBtn.interactable = true;
		maxBtn.interactable = true;
		useBtn.interactable = true;
	}
}
