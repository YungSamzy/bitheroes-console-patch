using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.model.armory;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.mount;
using com.ultrabit.bitheroes.model.rarity;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.cosmeticslist;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.cosmetic;

public class ArmoryCosmeticsWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI countTxt;

	public Button clearBtn;

	public TMP_InputField searchTxt;

	public Image rarityDropdown;

	public CosmeticsList cosmeticsList;

	private ArmoryRef _equipmentRef;

	private MountRef _mountRef;

	private ItemRef _cosmeticRef;

	private List<ItemRef> _items;

	private RarityRef selectedRarity;

	private int currentRarity = -1;

	private Transform dropdownWindow;

	private ArmoryEquipment armoryEquip;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(ArmoryRef equipmentRef = null, MountRef mountRef = null)
	{
		_equipmentRef = equipmentRef;
		_mountRef = mountRef;
		armoryEquip = GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot;
		_cosmeticRef = ((_equipmentRef != null) ? ((ItemRef)armoryEquip.GetCosmeticSlot(Equipment.getAvailableSlot(_equipmentRef.armoryType))) : ((ItemRef)armoryEquip.GetCosmeticMountEquipped()));
		topperTxt.text = Language.GetString("ui_cosmetic");
		searchTxt.text = "";
		clearBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_clear");
		rarityDropdown.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_all");
		UpdateItems();
		cosmeticsList.InitList(OnSelect);
		CreateList();
		ListenForBack(OnClose);
		CreateWindow();
	}

	public void OnValueChanged()
	{
		CancelInvoke("DoSearch");
		Invoke("DoSearch", Util.SEARCHBOX_ACTION_DELAY);
	}

	private void DoSearch()
	{
		CreateList();
	}

	public void OnClearBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SaveCosmetic();
	}

	public void OnRarityDropdown()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		dropdownWindow = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_rarity"));
		DropdownList componentInChildren = dropdownWindow.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, currentRarity, OnRarityDropdownChange);
		componentInChildren.Data.InsertOneAtEnd(new MyDropdownItemModel
		{
			id = -1,
			title = Language.GetString("ui_all"),
			btnHelp = false,
			data = null
		});
		for (int i = 0; i < RarityBook.size; i++)
		{
			RarityRef rarityRef = RarityBook.LookupID(i);
			if (rarityRef != null && GetRarityCount(rarityRef) > 0)
			{
				componentInChildren.Data.InsertOneAtEnd(new MyDropdownItemModel
				{
					id = i,
					title = rarityRef.coloredName,
					btnHelp = false,
					data = rarityRef
				});
			}
		}
	}

	private void OnRarityDropdownChange(MyDropdownItemModel model)
	{
		currentRarity = model.id;
		selectedRarity = model.data as RarityRef;
		rarityDropdown.GetComponentInChildren<TextMeshProUGUI>().text = model.title;
		CreateList();
		if (dropdownWindow != null)
		{
			dropdownWindow.GetComponent<DropdownWindow>().OnClose();
		}
	}

	private int GetRarityCount(RarityRef rarityRef)
	{
		int num = 0;
		foreach (ItemRef item in _items)
		{
			if (item.rarityRef == rarityRef)
			{
				num++;
			}
		}
		return num;
	}

	private void UpdateItems()
	{
		_items = new List<ItemRef>();
		if (_equipmentRef != null)
		{
			List<EquipmentRef> fullEquipmentList = EquipmentBook.GetFullEquipmentList();
			for (int i = 0; i < fullEquipmentList.Count; i++)
			{
				EquipmentRef equipmentRef = fullEquipmentList[i];
				if (!(equipmentRef == null) && equipmentRef.cosmetic && equipmentRef.rank <= 0 && _equipmentRef.equipmentType == equipmentRef.equipmentType && _equipmentRef.subtypesMatch(equipmentRef))
				{
					_items.Add(equipmentRef);
				}
			}
		}
		if (!(_mountRef != null))
		{
			return;
		}
		for (int j = 0; j <= MountBook.size; j++)
		{
			MountRef mountRef = MountBook.Lookup(j);
			if (!(mountRef == null) && mountRef.cosmetic)
			{
				_items.Add(mountRef);
			}
		}
	}

	private void CreateList()
	{
		cosmeticsList.ClearList();
		cosmeticsList.cosmeticRef = _cosmeticRef;
		RarityRef rarityRef = selectedRarity;
		List<ItemRef> list = new List<ItemRef>();
		foreach (ItemRef item in _items)
		{
			if (rarityRef == null || item.rarityRef == rarityRef)
			{
				list.Add(item);
			}
		}
		string text = searchTxt.text;
		int num = 0;
		int num2 = 0;
		List<ItemRef> list2 = Util.SortVector(list, new string[2] { "rarity", "id" }, Util.ARRAY_ASCENDING);
		List<CosmeticItem> list3 = new List<CosmeticItem>();
		foreach (ItemRef item2 in list2)
		{
			bool flag = true;
			if (flag && text.Length > 0 && item2.name.ToLowerInvariant().IndexOf(text.ToLowerInvariant()) < 0)
			{
				flag = false;
			}
			if (flag)
			{
				if (GameData.instance.PROJECT.character.inventory.hasOwnedItem(item2))
				{
					num2++;
				}
				num++;
				list3.Add(new CosmeticItem
				{
					itemRef = item2
				});
			}
		}
		cosmeticsList.Data.InsertItems(0, list3);
		countTxt.text = Util.NumberFormat(num2) + "/" + Util.NumberFormat(num);
	}

	private void OnSelect(BaseModelData baseModelData)
	{
		SaveCosmetic(baseModelData.itemRef);
	}

	private void SaveCosmetic(ItemRef itemRef = null)
	{
		if (_equipmentRef != null)
		{
			ArmoryRef equipRef = ArmoryRef.EquipmentRefToArmoryRef(itemRef as EquipmentRef);
			GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.SetCosmeticSlot(equipRef, Equipment.getAvailableSlot(_equipmentRef.armoryType));
			CharacterDALC.instance.doSaveArmory(GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot);
		}
		if (_mountRef != null)
		{
			if (itemRef != null)
			{
				int id = (itemRef as MountRef).id;
				GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.mountCosmetic = id;
			}
			else
			{
				GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.mountCosmetic = 0L;
			}
			CharacterDALC.instance.doSaveArmory(GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot);
		}
		OnClose();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		clearBtn.interactable = true;
		searchTxt.interactable = true;
		rarityDropdown.GetComponent<EventTrigger>().enabled = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		clearBtn.interactable = false;
		searchTxt.interactable = false;
		rarityDropdown.GetComponent<EventTrigger>().enabled = false;
	}
}
