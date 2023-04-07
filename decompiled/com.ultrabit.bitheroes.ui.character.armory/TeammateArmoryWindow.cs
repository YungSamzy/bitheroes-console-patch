using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.armory;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.enchant;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using com.ultrabit.bitheroes.ui.utility;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.character.armory;

public class TeammateArmoryWindow : WindowsMain
{
	[HideInInspector]
	public UnityCustomEvent ARMORY_TEAMMATE_SELECT = new UnityCustomEvent();

	public TextMeshProUGUI topperTxt;

	public Button abilitiesBtn;

	public Button infoBtn;

	public Button runesBtn;

	public Button equipBtn;

	public Button enchantsBtn;

	public Image armoryDrop;

	public RectTransform placeholderEquipment;

	public Transform characterArmoryEquipmentPanelPrefab;

	private CharacterArmoryEquipmentPanel _equipmentPanel;

	private CharacterArmoryNameChange armoryNameChange;

	private uint _selectedArmor;

	private uint MAX_ARMOR = 10u;

	private List<MyDropdownItemModel> _armoryVector;

	private DialogWindow dialogWindow;

	private CharacterData _charData;

	private bool _showSelect;

	private Transform _armoryDropWindow;

	private int equipPanelSortingLayer = -1;

	private ArmoryEquipment currentEquippedEquipment;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(CharacterData charData, int idx = 0, bool showSelect = true)
	{
		_selectedArmor = (uint)idx;
		_showSelect = showSelect;
		if (charData.charID == GameData.instance.PROJECT.character.id)
		{
			_charData = GameData.instance.PROJECT.character.toCharacterData(duplicateMounts: true);
		}
		else
		{
			_charData = charData.Duplicate();
		}
		_charData.armory.SetCurrentArmoryEquipmentSlotByIdx(_selectedArmor);
		SetArmoryList();
		topperTxt.text = Language.GetString("ui_armory_title");
		abilitiesBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString(Language.GetString("ui_ability_short"));
		infoBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_info_short");
		runesBtn.GetComponentInChildren<TextMeshProUGUI>().text = ItemRef.GetItemNamePlural(9);
		enchantsBtn.GetComponentInChildren<TextMeshProUGUI>().text = ItemRef.GetItemNamePlural(11);
		equipBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_select");
		if (_charData.charID != GameData.instance.PROJECT.character.id)
		{
			infoBtn.gameObject.SetActive(value: false);
		}
		else
		{
			infoBtn.gameObject.SetActive(value: true);
		}
		if (!_showSelect)
		{
			equipBtn.gameObject.SetActive(value: false);
		}
		equipPanelSortingLayer = 5301 + (GameData.instance.windowGenerator.dialogCount - 1) * 10;
		ReloadArmoryEquipmentPanel();
		if (EnchantBook.size <= 0)
		{
			enchantsBtn.gameObject.SetActive(value: false);
		}
		ListenForBack(OnClose);
		CreateWindow();
	}

	public override void UpdateSortingLayers(int layer)
	{
		base.UpdateSortingLayers(layer);
		_equipmentPanel.UpdateSortingLayer(layer + 1);
	}

	private void ReloadArmoryEquipmentPanel()
	{
		if (_equipmentPanel != null)
		{
			Object.Destroy(_equipmentPanel.gameObject);
			_equipmentPanel = null;
		}
		_equipmentPanel = AddEquipmentPanel();
		uint index = _selectedArmor;
		if (_charData.charID != GameData.instance.PROJECT.character.id)
		{
			index = GetRealSlotBySelecterdArmor();
		}
		ArmoryEquipment armoryEquipment = _armoryVector[(int)index].data as ArmoryEquipment;
		D.Log("nacho", "REALOAD EQUIP " + armoryEquipment.name);
		_equipmentPanel.LoadDetails(armoryEquipment, null, editable: false, equipPanelSortingLayer);
		_equipmentPanel.SetCharacterData(_charData);
	}

	private CharacterArmoryEquipmentPanel AddEquipmentPanel()
	{
		Transform obj = Object.Instantiate(characterArmoryEquipmentPanelPrefab);
		obj.SetParent(panel.transform, worldPositionStays: false);
		obj.GetComponent<RectTransform>().anchoredPosition = placeholderEquipment.anchoredPosition;
		return obj.GetComponent<CharacterArmoryEquipmentPanel>();
	}

	private void SetArmoryList(bool countCS = false)
	{
		List<ArmoryEquipment> list = _charData.armory.armoryEquipmentSlots;
		if (_charData.charID == GameData.instance.PROJECT.character.id)
		{
			list = new List<ArmoryEquipment>();
			foreach (ArmoryEquipment armoryEquipmentSlot in _charData.armory.armoryEquipmentSlots)
			{
				if (armoryEquipmentSlot.unlocked && !armoryEquipmentSlot.pprivate)
				{
					list.Add(armoryEquipmentSlot);
				}
			}
		}
		_armoryVector = new List<MyDropdownItemModel>();
		int num = 0;
		if (_charData.charID == GameData.instance.PROJECT.character.id)
		{
			ArmoryEquipment data = ArmoryEquipment.equipmentToArmoryEquipment(GameData.instance.PROJECT.character.equipment, GameData.instance.PROJECT.character.runes, GameData.instance.PROJECT.character.mounts, GameData.instance.PROJECT.character.enchants);
			_armoryVector.Add(new MyDropdownItemModel
			{
				id = 0,
				title = Language.GetString("ui_armory_current_equipment"),
				data = data
			});
			num++;
			if (countCS)
			{
				_selectedArmor++;
			}
			currentEquippedEquipment = data;
		}
		for (int i = 0; i < list.Count; i++)
		{
			ArmoryEquipment armoryEquipment = list[i];
			if (armoryEquipment.unlocked)
			{
				if (_charData.charID != GameData.instance.PROJECT.character.id)
				{
					if (!armoryEquipment.pprivate)
					{
						_armoryVector.Add(new MyDropdownItemModel
						{
							id = num,
							title = armoryEquipment.name,
							data = armoryEquipment
						});
					}
				}
				else
				{
					_armoryVector.Add(new MyDropdownItemModel
					{
						id = num,
						title = armoryEquipment.name,
						data = armoryEquipment
					});
				}
			}
			if (num == _selectedArmor && _armoryVector.Count > i)
			{
				armoryDrop.GetComponentInChildren<TextMeshProUGUI>().text = _armoryVector[i].title;
			}
			num++;
		}
		uint num2 = _selectedArmor;
		if (_charData.charID != GameData.instance.PROJECT.character.id)
		{
			num2 = GetRealSlotBySelecterdArmor();
		}
		if (_armoryVector.Count > num2)
		{
			armoryDrop.GetComponentInChildren<TextMeshProUGUI>().text = _armoryVector[(int)num2].title;
		}
	}

	public void OnArmoryDrop()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		WindowsMain component = (_armoryDropWindow = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_armory_list"))).GetComponent<WindowsMain>();
		Canvas component2 = GetComponent<Canvas>();
		if (component2 != null && component2.sortingOrder > equipPanelSortingLayer)
		{
			equipPanelSortingLayer = component2.sortingOrder + 100;
		}
		component.UpdateSortingLayers(equipPanelSortingLayer + 10);
		DropdownList componentInChildren = _armoryDropWindow.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, (int)_selectedArmor, OnArmoryDropdownChange);
		componentInChildren.Data.InsertItemsAtStart(_armoryVector);
	}

	private void OnArmoryDropdownChange(MyDropdownItemModel model)
	{
		_selectedArmor = (uint)model.id;
		uint currentArmoryEquipmentSlotByIdx = _selectedArmor;
		if (_charData.charID != GameData.instance.PROJECT.character.id)
		{
			currentArmoryEquipmentSlotByIdx = GetRealSlotBySelecterdArmor();
		}
		_charData.armory.SetCurrentArmoryEquipmentSlotByIdx(currentArmoryEquipmentSlotByIdx);
		ReloadArmoryEquipmentPanel();
		SetArmoryList();
		if (_armoryDropWindow != null)
		{
			_armoryDropWindow.GetComponent<DropdownWindow>().OnClose();
		}
	}

	private uint GetRealSlotBySelecterdArmor()
	{
		uint num = 0u;
		uint num2 = 0u;
		foreach (ArmoryEquipment armoryEquipmentSlot in _charData.armory.armoryEquipmentSlots)
		{
			if (armoryEquipmentSlot.unlocked && !armoryEquipmentSlot.pprivate)
			{
				num++;
			}
			num2++;
			if (num2 == _selectedArmor)
			{
				return num;
			}
		}
		return 0u;
	}

	public void OnAbilitiesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		ArmoryEquipment armoryEquipment = _armoryVector[(int)_selectedArmor].data as ArmoryEquipment;
		GameData.instance.windowGenerator.NewAbilityListWindow(armoryEquipment.GetAbilities(), _charData.getTotalPower(), GameModifier.getTypeTotal(_charData.getModifiers(), 17));
	}

	public void OnInfoBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		ArmoryEquipment aequip = _armoryVector[(int)_selectedArmor].data as ArmoryEquipment;
		GameData.instance.windowGenerator.NewCharacterArmoryInfoWindow(aequip, GameData.instance.PROJECT.character);
	}

	public void OnRunesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		ArmoryEquipment armoryEquipment = _armoryVector[(int)_selectedArmor].data as ArmoryEquipment;
		GameData.instance.windowGenerator.ArmoryNewRunesWindow(armoryEquipment.runes, changeable: false, base.gameObject);
	}

	public void OnEnchantsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		ArmoryEquipment armoryEquipment = _armoryVector[(int)_selectedArmor].data as ArmoryEquipment;
		GameData.instance.windowGenerator.NewArmoryEnchantsWindow(armoryEquipment.enchants, changeable: false, base.gameObject, -1, selectable: false, _charData);
	}

	public void OnEquipBtn()
	{
		uint index = _selectedArmor;
		if (_charData.charID != GameData.instance.PROJECT.character.id)
		{
			index = GetRealSlotBySelecterdArmor();
		}
		ArmoryEquipment armoryEquipment = _armoryVector[(int)index].data as ArmoryEquipment;
		ARMORY_TEAMMATE_SELECT.Invoke(armoryEquipment.id);
		OnClose();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		abilitiesBtn.interactable = true;
		infoBtn.interactable = true;
		runesBtn.interactable = true;
		equipBtn.interactable = true;
		enchantsBtn.interactable = true;
		armoryDrop.GetComponent<EventTrigger>().enabled = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		abilitiesBtn.interactable = false;
		infoBtn.interactable = false;
		runesBtn.interactable = false;
		equipBtn.interactable = false;
		enchantsBtn.interactable = false;
		armoryDrop.GetComponent<EventTrigger>().enabled = false;
	}
}
