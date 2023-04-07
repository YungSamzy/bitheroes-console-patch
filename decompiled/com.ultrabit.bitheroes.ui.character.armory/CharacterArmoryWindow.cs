using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.armory;
using com.ultrabit.bitheroes.model.enchant;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.character.armory;

public class CharacterArmoryWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public Button abilitiesBtn;

	public Button infoBtn;

	public Button runesBtn;

	public Button nameBtn;

	public Button clearGearBtn;

	public Button clearRunesBtn;

	public Button clearEnchantsBtn;

	public Toggle privateCheck;

	public Button equipBtn;

	public Button enchantsBtn;

	public Image armoryDrop;

	public Image armoryBattleTypeDrop;

	public RectTransform placeholderEquipment;

	public Transform characterArmoryEquipmentPanelPrefab;

	private CharacterArmoryEquipmentPanel _equipmentPanel;

	private CharacterArmoryNameChange armoryNameChange;

	private uint _selectedArmor;

	private uint MAX_ARMOR = 10u;

	private List<MyDropdownItemModel> _armoryVector;

	private List<MyDropdownItemModel> _battleTypes;

	private DialogWindow dialogWindow;

	private uint _armorySelectedIndex;

	private Transform _armoryDropWindow;

	private Transform _armoryBattleTypeDropWindow;

	private int equipPanelSortingLayer = -1;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(uint idx = 0u)
	{
		_selectedArmor = idx;
		GameData.instance.PROJECT.character.armory.SetCurrentArmoryEquipmentSlotByIdx(_selectedArmor);
		SetArmoryList();
		topperTxt.text = Language.GetString("ui_armory_title");
		abilitiesBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString(Language.GetString("ui_ability_short"));
		infoBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_info_short");
		runesBtn.GetComponentInChildren<TextMeshProUGUI>().text = ItemRef.GetItemNamePlural(9);
		enchantsBtn.GetComponentInChildren<TextMeshProUGUI>().text = ItemRef.GetItemNamePlural(11);
		nameBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_armory_name_button");
		equipBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_equip");
		clearGearBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_armory_clear_gear");
		clearRunesBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_armory_clear_runes");
		clearEnchantsBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_armory_clear_enchants");
		privateCheck.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_armory_private").ToUpperInvariant();
		equipPanelSortingLayer = 5101 + (GameData.instance.windowGenerator.dialogCount - 1) * 10;
		ReloadArmoryEquipmentPanel();
		GameData.instance.PROJECT.character.AddListener("APPEARANCE_CHANGE", OnAppearanceChange);
		GameData.instance.PROJECT.character.equipment.OnChange.AddListener(OnEquipmentChange);
		GameData.instance.PROJECT.character.AddListener("ENCHANTS_CHANGE", OnEnchantChange);
		GameData.instance.PROJECT.character.enchants.OnChange.AddListener(OnEnchantChange);
		GameData.instance.PROJECT.character.AddListener("MOUNTS_CHANGE", OnMountChange);
		GameData.instance.PROJECT.character.mounts.OnChange.AddListener(OnMountChange);
		GameData.instance.PROJECT.character.AddListener("armoryEquipmentChange", OnArmoryEquipmentChange);
		if (EnchantBook.size <= 0)
		{
			enchantsBtn.gameObject.SetActive(value: false);
		}
		SCROLL_IN_COMPLETE.AddListener(OnScrollInComplete);
		ListenForBack(OnClose);
		CreateWindow();
	}

	private CharacterArmoryEquipmentPanel AddEquipmentPanel()
	{
		Transform obj = Object.Instantiate(characterArmoryEquipmentPanelPrefab);
		obj.SetParent(panel.transform, worldPositionStays: false);
		obj.GetComponent<RectTransform>().anchoredPosition = placeholderEquipment.anchoredPosition;
		return obj.GetComponent<CharacterArmoryEquipmentPanel>();
	}

	private void OnArmoryEquipmentChange()
	{
		ReloadArmoryEquipmentPanel();
	}

	public void OnPrivateCheckboxChange()
	{
		ArmoryEquipment currentArmoryEquipmentSlot = GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot;
		currentArmoryEquipmentSlot.pprivate = privateCheck.isOn;
		CharacterDALC.instance.doSaveArmory(currentArmoryEquipmentSlot, dispatch: false);
	}

	private void SetArmoryList()
	{
		List<ArmoryEquipment> armoryEquipmentSlots = GameData.instance.PROJECT.character.armory.armoryEquipmentSlots;
		_armoryVector = new List<MyDropdownItemModel>();
		for (int i = 0; i < armoryEquipmentSlots.Count; i++)
		{
			ArmoryEquipment armoryEquipment = armoryEquipmentSlots[i];
			if (armoryEquipment.requiredLevel <= GameData.instance.PROJECT.character.level)
			{
				if (armoryEquipment.unlocked)
				{
					_armoryVector.Add(new MyDropdownItemModel
					{
						id = i,
						title = armoryEquipment.name,
						data = armoryEquipment,
						locked = false
					});
				}
				else
				{
					bool flag = false;
					if (i > 0 && armoryEquipmentSlots[i - 1].unlocked)
					{
						flag = true;
					}
					_armoryVector.Add(new MyDropdownItemModel
					{
						id = i,
						title = armoryEquipment.value + " " + ArmoryEquipment.GetArmorySlotCurrency(armoryEquipment.currency),
						data = armoryEquipment,
						locked = !flag
					});
				}
			}
			else
			{
				_armoryVector.Add(new MyDropdownItemModel
				{
					id = i,
					title = Language.GetString("ui_level") + " " + armoryEquipment.requiredLevel,
					data = null,
					locked = true
				});
			}
			if (i == _selectedArmor)
			{
				armoryDrop.GetComponentInChildren<TextMeshProUGUI>().text = _armoryVector[i].title;
			}
		}
		Armory armory = GameData.instance.PROJECT.character.armory;
		_battleTypes = new List<MyDropdownItemModel>();
		_battleTypes.Add(new MyDropdownItemModel
		{
			id = 0,
			title = Language.GetString("ui_none"),
			data = null
		});
		for (int j = 0; j < armory.battleTypes.Count; j++)
		{
			_battleTypes.Add(new MyDropdownItemModel
			{
				id = j + 1,
				title = armory.battleTypes[j].name,
				data = armory.battleTypes[j]
			});
		}
		armoryBattleTypeDrop.GetComponentInChildren<TextMeshProUGUI>().text = _battleTypes[(int)armory.currentArmoryEquipmentSlot.battleType].title;
		privateCheck.SetIsOnWithoutNotify(armory.currentArmoryEquipmentSlot.pprivate);
	}

	public void OnArmoryBattleTypeDrop()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_armoryBattleTypeDropWindow = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_armory_list"));
		DropdownList componentInChildren = _armoryBattleTypeDropWindow.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, _battleTypes[(int)GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.battleType].id, OnArmoryBattleTypeDropdownChange);
		componentInChildren.Data.InsertItemsAtStart(_battleTypes);
	}

	private void OnArmoryBattleTypeDropdownChange(MyDropdownItemModel model)
	{
		ArmoryEquipment currentArmoryEquipmentSlot = GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot;
		if (model.id > 0)
		{
			uint index = (uint)(model.id - 1);
			currentArmoryEquipmentSlot.battleType = (uint)GameData.instance.PROJECT.character.armory.battleTypes[(int)index].id;
		}
		else
		{
			currentArmoryEquipmentSlot.battleType = 0u;
		}
		armoryBattleTypeDrop.GetComponentInChildren<TextMeshProUGUI>().text = _battleTypes[model.id].title;
		CharacterDALC.instance.doSaveArmory(currentArmoryEquipmentSlot);
		if (_armoryBattleTypeDropWindow != null)
		{
			_armoryBattleTypeDropWindow.GetComponent<DropdownWindow>().OnClose();
		}
	}

	private void OnArmoryNameChangeClosed(object e)
	{
		armoryNameChange.ARMORY_NAME_CHANGE.RemoveListener(OnArmoryNameChange);
		armoryNameChange.DESTROYED.RemoveListener(OnArmoryNameChangeClosed);
	}

	private void OnArmoryNameChange(object e)
	{
		ArmoryEquipment currentArmoryEquipmentSlot = GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot;
		currentArmoryEquipmentSlot.name = armoryNameChange.currentName;
		CharacterDALC.instance.doSaveArmory(currentArmoryEquipmentSlot);
		armoryNameChange.ARMORY_NAME_CHANGE.RemoveListener(OnArmoryNameChange);
		armoryNameChange.OnClose();
		SetArmoryList();
	}

	public void OnArmoryDrop()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_armoryDropWindow = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_armory_list"));
		DropdownList componentInChildren = _armoryDropWindow.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, (int)_selectedArmor, OnArmoryDropdownChange);
		componentInChildren.Data.InsertItemsAtStart(_armoryVector);
	}

	private void OnArmoryDropdownChange(MyDropdownItemModel model)
	{
		_armorySelectedIndex = (uint)model.id;
		ArmoryEquipment armoryEquipment = GameData.instance.PROJECT.character.armory.armoryEquipmentSlots[model.id];
		if (armoryEquipment.unlocked)
		{
			_selectedArmor = (uint)model.id;
			GameData.instance.PROJECT.character.armory.SetCurrentArmoryEquipmentSlotByIdx(_selectedArmor);
			ReloadArmoryEquipmentPanel();
			SetArmoryList();
		}
		else
		{
			dialogWindow = GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_armory_unlock_armor", new string[2]
			{
				armoryEquipment.value.ToString(),
				ArmoryEquipment.GetArmorySlotCurrency(armoryEquipment.currency)
			}), null, null, delegate
			{
				OnConfirmAddNewArmory();
			}, delegate
			{
				OnRejectAddNewArmory();
			});
		}
		if (_armoryDropWindow != null)
		{
			_armoryDropWindow.GetComponent<DropdownWindow>().OnClose();
		}
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
		_equipmentPanel.characterArmoryWindow = this;
		int num = equipPanelSortingLayer;
		if (GameData.instance.PROJECT.dungeon != null)
		{
			num += 100;
		}
		_equipmentPanel.LoadDetails(_armoryVector[(int)_selectedArmor].data as ArmoryEquipment, GameData.instance.PROJECT.character, editable: true, num);
	}

	private void OnRejectAddNewArmory()
	{
		SetArmoryList();
	}

	private void OnConfirmAddNewArmory()
	{
		GameData.instance.main.ShowLoading();
		string text = Language.GetString("ui_armory_generic_name") + (_armorySelectedIndex + 1);
		CharacterDALC.instance.doCreateArmorySlot(_armorySelectedIndex + 1, text);
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(54), OnArmoryCreated);
	}

	private void OnArmoryCreated(BaseEvent e)
	{
		DALCEvent dALCEvent = e as DALCEvent;
		SFSObject sfsob = dALCEvent.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.main.HideLoading();
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			SetArmoryList();
			return;
		}
		_ = GameData.instance.PROJECT.character.armory.armoryEquipmentSlots[(int)_armorySelectedIndex];
		bool flag = false;
		if (sfsob.ContainsKey("cha9"))
		{
			int @int = sfsob.GetInt("cha9");
			if (GameData.instance.PROJECT.character.gold != @int)
			{
				GameData.instance.PROJECT.character.gold = @int;
				flag = true;
			}
		}
		if (sfsob.ContainsKey("cha10"))
		{
			int int2 = sfsob.GetInt("cha10");
			if (GameData.instance.PROJECT.character.credits != int2)
			{
				GameData.instance.PROJECT.character.credits = int2;
				flag = true;
			}
		}
		if (flag)
		{
			KongregateAnalytics.updateCommonFields();
		}
		GameData.instance.main.HideLoading();
		ArmoryEquipment armoryEquipment = ArmoryEquipment.FromSFSObject(dALCEvent.sfsob);
		armoryEquipment.unlocked = true;
		GameData.instance.PROJECT.character.armory.SetArmoryEquipmentByIndex(_armorySelectedIndex, armoryEquipment);
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(54), OnArmoryCreated);
		_selectedArmor = _armorySelectedIndex;
		GameData.instance.PROJECT.character.armory.SetCurrentArmoryEquipmentSlotByIdx(_selectedArmor);
		ReloadArmoryEquipmentPanel();
		SetArmoryList();
	}

	private void OnScrollInComplete(object e)
	{
	}

	public void CheckTutorial()
	{
	}

	private void UpdateDisplay()
	{
	}

	private void OnAppearanceChange()
	{
	}

	private void OnEquipmentChange()
	{
		UpdateDisplay();
		_equipmentPanel.UpdateSlots();
	}

	private void OnEnchantChange()
	{
	}

	private void OnMountChange()
	{
		UpdateDisplay();
		_equipmentPanel.UpdateSlots();
	}

	public void OnAbilitiesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewAbilityListWindow(GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.GetAbilities(), GameData.instance.PROJECT.character.getTotalPower(), GameModifier.getTypeTotal(GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.GetModifiers(), 17));
	}

	public void OnInfoBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewCharacterArmoryInfoWindow(GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot, GameData.instance.PROJECT.character);
	}

	public void OnRunesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ArmoryNewRunesWindow(GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.runes, changeable: true, base.gameObject);
	}

	public void OnNameBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		armoryNameChange = GameData.instance.windowGenerator.NewCharacterArmoryNameChange(GameData.instance.PROJECT.character.armory.armoryEquipmentSlots[(int)_selectedArmor].name, base.gameObject);
		armoryNameChange.ARMORY_NAME_CHANGE.AddListener(OnArmoryNameChange);
		armoryNameChange.DESTROYED.AddListener(OnArmoryNameChangeClosed);
	}

	public void OnEnchantsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewArmoryEnchantsWindow(GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.enchants, changeable: true, base.gameObject);
	}

	public void OnEquipBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.PROJECT.character.equipCurrentArmorySlot();
		OnClose();
	}

	public void OnClearGearBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		ClearGearFromArmoryEquipment();
	}

	public void OnClearRunesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		ClearRunesFromArmoryEquipment();
		GameData.instance.windowGenerator.ArmoryNewRunesWindow(GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.runes, changeable: true, base.gameObject);
	}

	public void OnClearEnchantsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		ClearEnchantsFromArmoryEquipment();
		GameData.instance.windowGenerator.NewArmoryEnchantsWindow(GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.enchants, changeable: true, base.gameObject);
	}

	private void ClearEnchantsFromArmoryEquipment()
	{
		ArmoryEquipment currentArmoryEquipmentSlot = GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot;
		currentArmoryEquipmentSlot.enchants.clearEnchantSlots();
		CharacterDALC.instance.updateArmoryEnchants(currentArmoryEquipmentSlot);
	}

	private void ClearRunesFromArmoryEquipment()
	{
		GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.runes.clearSlots();
		CharacterDALC.instance.updateArmoryRunes(GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot);
	}

	private void ClearGearFromArmoryEquipment()
	{
		ArmoryEquipment currentArmoryEquipmentSlot = GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot;
		for (int i = 0; i < 8; i++)
		{
			currentArmoryEquipmentSlot.SetArmoryEquipmentSlot(null, i);
			currentArmoryEquipmentSlot.SetCosmeticSlot(null, i);
		}
		currentArmoryEquipmentSlot.mount = 0L;
		currentArmoryEquipmentSlot.mountCosmetic = 0L;
		ReloadArmoryEquipmentPanel();
		CharacterDALC.instance.doSaveArmory(currentArmoryEquipmentSlot, dispatch: false);
	}

	public override void DoDestroy()
	{
		GameData.instance.PROJECT.character.RemoveListener("APPEARANCE_CHANGE", OnAppearanceChange);
		GameData.instance.PROJECT.character.equipment.OnChange.RemoveListener(OnEquipmentChange);
		GameData.instance.PROJECT.character.RemoveListener("ENCHANTS_CHANGE", OnEnchantChange);
		GameData.instance.PROJECT.character.enchants.OnChange.RemoveListener(OnEnchantChange);
		GameData.instance.PROJECT.character.RemoveListener("MOUNTS_CHANGE", OnMountChange);
		GameData.instance.PROJECT.character.mounts.OnChange.RemoveListener(OnMountChange);
		_equipmentPanel = null;
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		abilitiesBtn.interactable = true;
		infoBtn.interactable = true;
		runesBtn.interactable = true;
		nameBtn.interactable = true;
		clearGearBtn.interactable = true;
		clearRunesBtn.interactable = true;
		clearEnchantsBtn.interactable = true;
		privateCheck.interactable = true;
		equipBtn.interactable = true;
		enchantsBtn.interactable = true;
		armoryDrop.GetComponent<EventTrigger>().enabled = true;
		armoryBattleTypeDrop.GetComponent<EventTrigger>().enabled = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		abilitiesBtn.interactable = false;
		infoBtn.interactable = false;
		runesBtn.interactable = false;
		nameBtn.interactable = false;
		clearGearBtn.interactable = false;
		clearRunesBtn.interactable = false;
		clearEnchantsBtn.interactable = false;
		privateCheck.interactable = false;
		equipBtn.interactable = false;
		enchantsBtn.interactable = false;
		armoryDrop.GetComponent<EventTrigger>().enabled = false;
		armoryBattleTypeDrop.GetComponent<EventTrigger>().enabled = false;
	}
}
