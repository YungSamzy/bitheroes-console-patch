using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.armory;
using com.ultrabit.bitheroes.model.armory.enchant;
using com.ultrabit.bitheroes.model.armory.rune;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.enchant;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.character.armory;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using com.ultrabit.bitheroes.ui.tutorial;
using com.ultrabit.bitheroes.ui.utility;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.character;

public class CharacterWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public Button infoBtn;

	public Button abilitiesBtn;

	public Button pointsBtn;

	public Button resetBtn;

	public Button runesBtn;

	public Button enchantsBtn;

	public Button achievementsBtn;

	public Button armoryBtn;

	public Button saveToArmoryBtn;

	public Button heroTagBtn;

	public Button historyNamesBtn;

	public Button heroSelectBtn;

	public Button nameChangeBtn;

	public CharacterInventoryPanel _inventoryPanel;

	public CharacterEquipmentPanel _equipmentPanel;

	private ArmoryEquipment selectedAEquipment;

	private Transform _armoryDropWindow;

	private CharacterArmoryWindow armoryWindow;

	private int _prevPower;

	private int _prevStamina;

	private int _prevAgility;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = GameData.instance.PROJECT.character.name;
		heroTagBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_herotag_btn");
		infoBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_info_short");
		armoryBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_armory_title");
		saveToArmoryBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_save");
		abilitiesBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_ability_short");
		runesBtn.GetComponentInChildren<TextMeshProUGUI>().text = ItemRef.GetItemNamePlural(9);
		enchantsBtn.GetComponentInChildren<TextMeshProUGUI>().text = ItemRef.GetItemNamePlural(11);
		achievementsBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_achievements_legacy");
		historyNamesBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_historyname");
		heroSelectBtn.gameObject.SetActive(GameData.instance.PROJECT.instance != null && GameData.instance.PROJECT.instance.instanceInterface != null);
		nameChangeBtn.gameObject.SetActive(GameData.instance.PROJECT.instance != null && GameData.instance.PROJECT.instance.instanceInterface != null);
		_inventoryPanel.LoadDetails();
		_equipmentPanel.characterWindow = this;
		_equipmentPanel.LoadDetails(GameData.instance.PROJECT.character.toCharacterData(), editable: true);
		GameData.instance.PROJECT.character.AddListener("STATS_CHANGE", OnStatChange);
		GameData.instance.PROJECT.character.AddListener("POINTS_CHANGE", OnPointsChange);
		GameData.instance.PROJECT.character.equipment.OnChange.AddListener(OnEquipmentChange);
		GameData.instance.PROJECT.character.equipment.BeforeChange.AddListener(OnEquipmentBeforeChange);
		GameData.instance.PROJECT.character.AddListener("APPEARANCE_CHANGE", OnAppearanceChange);
		GameData.instance.PROJECT.character.mounts.OnChange.AddListener(OnMountChange);
		GameData.instance.PROJECT.character.mounts.OnCosmeticChange.AddListener(OnMountCosmeticChange);
		GameData.instance.PROJECT.character.AddListener("NAME_CHANGE", OnHerotagChange);
		pointsBtn.GetComponent<GlowNoOver>().ForceStart(forceTextsComponents: true);
		pointsBtn.GetComponent<GlowNoOver>().ON_ENTER.AddListener(OnPointsBtnEnter);
		UpdateStats();
		OnPointsChange();
		historyNamesBtn.gameObject.SetActive(GameData.instance.PROJECT.character.nameHasChanged);
		achievementsBtn.gameObject.SetActive(AppInfo.allowAchievementShowable);
		if (EnchantBook.enchants.Count <= 0)
		{
			enchantsBtn.gameObject.SetActive(value: false);
		}
		GameData.instance.PROJECT.PauseDungeon();
		SCROLL_IN_COMPLETE.AddListener(OnScrollInComplete);
		SCROLL_IN_START.AddListener(CheckAllButtons);
		ListenForBack(OnClose);
		CreateWindow();
	}

	public override void UpdateSortingLayers(int layer)
	{
		base.UpdateSortingLayers(layer);
		_equipmentPanel.UpdateLayer();
	}

	private void OnScrollInComplete(object e)
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		CheckTutorial();
	}

	private void CheckTutorial()
	{
		if (GameData.instance.tutorialManager == null || GameData.instance.tutorialManager.hasPopup || GameData.instance.tutorialManager.canvas == null)
		{
			return;
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(35) && GameData.instance.PROJECT.character.tutorial.GetState(34) && GameData.instance.PROJECT.character.points > 0)
		{
			GameData.instance.PROJECT.character.tutorial.SetState(35);
			GameData.instance.tutorialManager.ShowTutorialForButton(pointsBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(35), 3, pointsBtn.gameObject), stageTrigger: true, null, funcSameAsTargetFunc: false, null, shadow: false, tween: true);
			GameData.instance.PROJECT.CheckTutorialChanges();
			return;
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(95) && GameData.instance.PROJECT.character.tutorial.GetState(94))
		{
			GameData.instance.PROJECT.character.tutorial.SetState(95);
			GameData.instance.tutorialManager.ShowTutorialForButton(enchantsBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(95), 4, enchantsBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
			return;
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(90) && GameData.instance.PROJECT.character.tutorial.GetState(89))
		{
			GameData.instance.PROJECT.character.tutorial.SetState(90);
			GameData.instance.tutorialManager.ShowTutorialForButton(runesBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(90), 4, runesBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
			return;
		}
		TutorialRef tutorialRef = VariableBook.LookUpTutorial("pet_equip");
		if (tutorialRef != null && !GameData.instance.PROJECT.character.tutorial.GetState(77) && GameData.instance.PROJECT.character.tutorial.GetState(76) && tutorialRef.areConditionsMet && _equipmentPanel.placeholderPet != null && _equipmentPanel.placeholderPet.activeSelf)
		{
			GameData.instance.PROJECT.character.tutorial.SetState(77);
			GameData.instance.tutorialManager.ShowTutorialForButton(_equipmentPanel.placeholderPet.GetComponentInChildren<Button>().gameObject, new TutorialPopUpSettings(Tutorial.GetText(77), 2, _equipmentPanel.placeholderPet.GetComponentInChildren<Button>().gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
			return;
		}
		TutorialRef tutorialRef2 = VariableBook.LookUpTutorial("accessory_equip");
		if (tutorialRef2 != null && !GameData.instance.PROJECT.character.tutorial.GetState(81) && GameData.instance.PROJECT.character.tutorial.GetState(80) && tutorialRef2.areConditionsMet && _equipmentPanel.placeholderAccessory != null && _equipmentPanel.placeholderAccessory.activeSelf)
		{
			GameData.instance.PROJECT.character.tutorial.SetState(81);
			GameData.instance.tutorialManager.ShowTutorialForButton(_equipmentPanel.placeholderAccessory.GetComponentInChildren<Button>().gameObject, new TutorialPopUpSettings(Tutorial.GetText(81), 2, _equipmentPanel.placeholderAccessory.GetComponentInChildren<Button>().gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
			return;
		}
		TutorialRef tutorialRef3 = VariableBook.LookUpTutorial("mount_summon");
		if (tutorialRef3 != null && !GameData.instance.PROJECT.character.tutorial.GetState(85) && GameData.instance.PROJECT.character.tutorial.GetState(84) && tutorialRef3.areConditionsMet && _equipmentPanel.placeholderMount != null && _equipmentPanel.placeholderMount.activeSelf)
		{
			GameData.instance.PROJECT.character.tutorial.SetState(85);
			GameData.instance.tutorialManager.ShowTutorialForButton(_equipmentPanel.placeholderMount.GetComponentInChildren<Button>().gameObject, new TutorialPopUpSettings(Tutorial.GetText(85), 4, _equipmentPanel.placeholderMount.GetComponentInChildren<Button>().gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
		}
		else if (!GameData.instance.PROJECT.character.tutorial.GetState(104) && GameData.instance.PROJECT.character.tutorial.GetState(103))
		{
			GameData.instance.PROJECT.character.tutorial.SetState(104);
			GameData.instance.PROJECT.CheckTutorialChanges();
			ItemIcon upgradableItem = GetUpgradableItem();
			if (upgradableItem != null)
			{
				GameData.instance.tutorialManager.ShowTutorialForButton(upgradableItem.GetComponentInChildren<Button>().gameObject, new TutorialPopUpSettings(Tutorial.GetText(104), 4, upgradableItem.GetComponentInChildren<Button>().gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
			}
			else
			{
				GameData.instance.tutorialManager.ShowTutorialForButton(_equipmentPanel.placeholderMainhand.GetComponentInChildren<Button>().gameObject, new TutorialPopUpSettings(Tutorial.GetText(104), 4, _equipmentPanel.placeholderMainhand.GetComponentInChildren<Button>().gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
			}
		}
		else
		{
			_inventoryPanel.CheckTutorial();
		}
	}

	private void OnEquipmentBeforeChange()
	{
		_prevPower = GameData.instance.PROJECT.character.getTotalPower();
		_prevStamina = GameData.instance.PROJECT.character.getTotalStamina();
		_prevAgility = GameData.instance.PROJECT.character.getTotalAgility();
	}

	private void OnMountChange()
	{
		D.Log("nacho", "OnMountChange");
		OnEquipmentChange();
	}

	private void OnMountCosmeticChange()
	{
		D.Log("nacho", "OnMountChange");
		OnEquipmentBeforeChange();
		OnEquipmentChange();
	}

	private void OnEquipmentChange()
	{
		_equipmentPanel.SetCharacterData(GameData.instance.PROJECT.character.toCharacterData());
		_equipmentPanel.SetNewStats(0, _prevPower);
		_equipmentPanel.SetNewStats(1, _prevStamina);
		_equipmentPanel.SetNewStats(2, _prevAgility);
	}

	private void OnAppearanceChange()
	{
		_equipmentPanel.SetDisplay(GameData.instance.PROJECT.character.toCharacterDisplay(1f, displayMount: true));
	}

	public void OnInfoBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewCharacterInfoWindow();
	}

	public void OnAbilitiesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewAbilityListWindow(GameData.instance.PROJECT.character.getAbilities(), GameData.instance.PROJECT.character.getTotalPower(), GameModifier.getTypeTotal(GameData.instance.PROJECT.character.getModifiers(), 17));
	}

	public void OnPointsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewCharacterStatWindow();
	}

	public void OnResetBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewServiceWindow(4, ServiceBook.GetFirstServiceByType(4), new int[4] { 0, 1, 2, 3 });
	}

	public void OnResetNameBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewServiceWindow(4, ServiceBook.GetFirstServiceByType(12), new int[4] { 0, 1, 2, 3 });
	}

	public void OnRunesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewRunesWindow(GameData.instance.PROJECT.character.runes, changeable: true, base.gameObject);
	}

	public void OnEnchantsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewEnchantsWindow(GameData.instance.PROJECT.character.enchants, changeable: true, base.gameObject);
	}

	public void OnAchievementsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_ = AppInfo.allowAchievements;
	}

	public void OnArmoryBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (GameData.instance.PROJECT.character.armory.armoryEquipmentSlots[0].unlocked)
		{
			armoryWindow = GameData.instance.windowGenerator.NewCharacterArmoryWindow();
			armoryWindow.DESTROYED.AddListener(OnArmoryWindowDestroyed);
		}
		else
		{
			GameData.instance.PROJECT.character.armory.CloneEquipmentForIndex(0u, doCreate: true);
			GameData.instance.PROJECT.character.AddListener("armoryEquipmentChange", OnArmoryEquipmentCreatedAndLoaded);
		}
	}

	private void OnArmoryWindowDestroyed(object e)
	{
		_equipmentPanel.OnUpdateMountIcon();
	}

	private void OnArmoryEquipmentCreatedAndLoaded()
	{
		GameData.instance.PROJECT.character.RemoveListener("armoryEquipmentChange", OnArmoryEquipmentCreatedAndLoaded);
		armoryWindow = GameData.instance.windowGenerator.NewCharacterArmoryWindow();
	}

	public void OnSaveToArmoryBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		ShowArmoryList();
	}

	public void OnHeroTagBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewCharacterHerotagWindow(GameData.instance.PROJECT.character.name, GameData.instance.PROJECT.character.herotag, changeable: true);
	}

	public void OnHistoryNamesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowHistoryNames(GameData.instance.PROJECT.character.id);
	}

	public void UpdateStats()
	{
		_equipmentPanel.SetStats(GameData.instance.PROJECT.character.getTotalPower(), GameData.instance.PROJECT.character.getTotalStamina(), GameData.instance.PROJECT.character.getTotalAgility());
	}

	private void OnStatChange()
	{
		UpdateStats();
	}

	private void OnPointsChange()
	{
		pointsBtn.GetComponentInChildren<TextMeshProUGUI>().text = "+" + GameData.instance.PROJECT.character.points;
		pointsBtn.GetComponent<GlowNoOver>().enabled = true;
		pointsBtn.GetComponent<GlowNoOver>().StopGlowing();
		pointsBtn.gameObject.SetActive(GameData.instance.PROJECT.character.points > 0);
		if (VariableBook.GameRequirementMet(38))
		{
			resetBtn.gameObject.SetActive(!pointsBtn.gameObject.activeSelf);
		}
		else
		{
			resetBtn.gameObject.SetActive(value: false);
		}
		if (pointsBtn.gameObject.activeSelf)
		{
			pointsBtn.GetComponent<GlowNoOver>().StartGlow();
		}
	}

	private void OnPointsBtnEnter()
	{
		pointsBtn.GetComponent<GlowNoOver>().ON_ENTER.RemoveListener(OnPointsBtnEnter);
		pointsBtn.GetComponent<GlowNoOver>().StopGlowing();
		pointsBtn.GetComponent<GlowNoOver>().enabled = false;
	}

	private void DoChangeHerotag()
	{
		topperTxt.text = GameData.instance.PROJECT.character.name;
	}

	private void OnHerotagChange()
	{
		DoChangeHerotag();
	}

	private void ShowArmoryList()
	{
		List<ArmoryEquipment> armoryEquipmentSlots = GameData.instance.PROJECT.character.armory.armoryEquipmentSlots;
		List<MyDropdownItemModel> list = new List<MyDropdownItemModel>();
		for (int i = 0; i < armoryEquipmentSlots.Count; i++)
		{
			ArmoryEquipment armoryEquipment = armoryEquipmentSlots[i];
			if (armoryEquipment.requiredLevel <= GameData.instance.PROJECT.character.level)
			{
				if (armoryEquipment.unlocked)
				{
					list.Add(new MyDropdownItemModel
					{
						id = i,
						title = armoryEquipment.name,
						data = armoryEquipment,
						locked = false
					});
					continue;
				}
				bool flag = false;
				if (i > 0 && armoryEquipmentSlots[i - 1].unlocked)
				{
					flag = true;
				}
				list.Add(new MyDropdownItemModel
				{
					id = i,
					title = armoryEquipment.value + " " + ArmoryEquipment.GetArmorySlotCurrency(armoryEquipment.currency),
					data = armoryEquipment,
					locked = !flag
				});
			}
			else
			{
				list.Add(new MyDropdownItemModel
				{
					id = i,
					title = Language.GetString("ui_level") + " " + armoryEquipment.requiredLevel,
					data = null,
					locked = true
				});
			}
		}
		_armoryDropWindow = GameData.instance.windowGenerator.NewArmoryDropdownWindow(Language.GetString("ui_armory_list"), Language.GetString("ui_armory_save_to_armory_description"));
		DropdownList componentInChildren = _armoryDropWindow.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, -1, OnArmoryDropdownSelected);
		componentInChildren.Data.InsertItemsAtStart(list);
	}

	private void OnArmoryDropdownSelected(MyDropdownItemModel model)
	{
		selectedAEquipment = model.data as ArmoryEquipment;
		if (_armoryDropWindow != null)
		{
			_armoryDropWindow.GetComponent<ArmoryDropdownWindow>().OnClose();
		}
		if (selectedAEquipment.unlocked)
		{
			GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_armory_equipment_to_armory_slot"), null, null, delegate
			{
				OnConfirmSaveEquipmentToArmory();
			});
			return;
		}
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_armory_unlock_armor", new string[2]
		{
			selectedAEquipment.value.ToString(),
			ArmoryEquipment.GetArmorySlotCurrency(selectedAEquipment.currency)
		}), null, null, delegate
		{
			OnConfirmAddNewArmoryFromSaveList();
		});
	}

	private void OnConfirmSaveEquipmentToArmory()
	{
		ArmoryEquipment armoryEquipment = selectedAEquipment;
		Equipment equipment = GameData.instance.PROJECT.character.equipment;
		for (int i = 0; i < 8; i++)
		{
			EquipmentRef equipmentSlot = equipment.getEquipmentSlot(i);
			EquipmentRef cosmeticSlot = equipment.getCosmeticSlot(i);
			armoryEquipment.SetArmoryEquipmentSlot(ArmoryRef.EquipmentRefToArmoryRef(equipmentSlot), i);
			armoryEquipment.SetCosmeticSlot(ArmoryRef.EquipmentRefToArmoryRef(cosmeticSlot), i);
		}
		ArmoryRunes runes = new ArmoryRunes(GameData.instance.PROJECT.character.runes.runeSlots, GameData.instance.PROJECT.character.runes.runeSlotsMemory);
		armoryEquipment.SetRunes(runes);
		ArmoryEnchants enchants = new ArmoryEnchants(GameData.instance.PROJECT.character.enchants.slots, GameData.instance.PROJECT.character.enchants.enchants);
		armoryEquipment.SetEnchants(enchants);
		armoryEquipment.mount = GameData.instance.PROJECT.character.mounts.getMountEquippedUID();
		if (GameData.instance.PROJECT.character.mounts.cosmetic != null)
		{
			armoryEquipment.mountCosmetic = GameData.instance.PROJECT.character.mounts.cosmetic.id;
		}
		uint idx = selectedAEquipment.position - 1;
		GameData.instance.PROJECT.character.armory.SetArmoryEquipmentByIndex(idx, armoryEquipment);
		CharacterDALC.instance.updateArmoryRunes(armoryEquipment);
		CharacterDALC.instance.updateArmoryEnchants(armoryEquipment);
		CharacterDALC.instance.doSaveArmory(armoryEquipment);
		GameData.instance.windowGenerator.NewCharacterArmoryWindow(idx);
	}

	private void OnConfirmAddNewArmoryFromSaveList()
	{
		CharacterDALC.instance.doCreateArmorySlot(selectedAEquipment.position, Language.GetString("ui_armory_generic_name"));
		GameData.instance.main.ShowLoading();
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(54), OnArmoryCreatedToSaveEquipment);
	}

	private void OnArmoryCreatedToSaveEquipment(BaseEvent e)
	{
		SFSObject sfsob = (e as DALCEvent).sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.main.HideLoading();
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
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
		ArmoryEquipment armoryEquipment = ArmoryEquipment.FromSFSObject(sfsob);
		Equipment equipment = GameData.instance.PROJECT.character.equipment;
		for (int i = 0; i < 8; i++)
		{
			EquipmentRef equipmentSlot = equipment.getEquipmentSlot(i);
			EquipmentRef cosmeticSlot = equipment.getCosmeticSlot(i);
			armoryEquipment.SetArmoryEquipmentSlot(ArmoryRef.EquipmentRefToArmoryRef(equipmentSlot), i);
			armoryEquipment.SetCosmeticSlot(ArmoryRef.EquipmentRefToArmoryRef(cosmeticSlot), i);
		}
		ArmoryRunes runes = new ArmoryRunes(GameData.instance.PROJECT.character.runes.runeSlots, GameData.instance.PROJECT.character.runes.runeSlotsMemory);
		armoryEquipment.SetRunes(runes);
		ArmoryEnchants enchants = new ArmoryEnchants(GameData.instance.PROJECT.character.enchants.slots, GameData.instance.PROJECT.character.enchants.enchants);
		armoryEquipment.SetEnchants(enchants);
		armoryEquipment.mount = GameData.instance.PROJECT.character.mounts.getMountEquippedUID();
		if (GameData.instance.PROJECT.character.mounts.cosmetic != null)
		{
			armoryEquipment.mountCosmetic = GameData.instance.PROJECT.character.mounts.cosmetic.id;
		}
		uint idx = selectedAEquipment.position - 1;
		GameData.instance.PROJECT.character.armory.SetArmoryEquipmentByIndex(idx, armoryEquipment);
		CharacterDALC.instance.updateArmoryRunes(armoryEquipment);
		CharacterDALC.instance.updateArmoryEnchants(armoryEquipment);
		CharacterDALC.instance.doSaveArmory(armoryEquipment);
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(54), OnArmoryCreatedToSaveEquipment);
		GameData.instance.windowGenerator.NewCharacterArmoryWindow(idx);
	}

	private ItemIcon GetUpgradableItem()
	{
		Equipment equipment = GameData.instance.PROJECT.character.equipment;
		for (int i = 0; i < 8; i++)
		{
			EquipmentRef equipmentSlot = equipment.getEquipmentSlot(i);
			if (equipmentSlot != null && equipmentSlot.canUpgrade())
			{
				return _equipmentPanel.slots[i];
			}
		}
		return null;
	}

	public override void DoDestroy()
	{
		pointsBtn.GetComponent<GlowNoOver>().ON_ENTER.RemoveListener(OnPointsBtnEnter);
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		GameData.instance.PROJECT.character.RemoveListener("STATS_CHANGE", OnStatChange);
		GameData.instance.PROJECT.character.RemoveListener("POINTS_CHANGE", OnPointsChange);
		GameData.instance.PROJECT.character.equipment.OnChange.RemoveListener(OnEquipmentChange);
		GameData.instance.PROJECT.character.equipment.BeforeChange.RemoveListener(OnEquipmentBeforeChange);
		GameData.instance.PROJECT.character.RemoveListener("APPEARANCE_CHANGE", OnAppearanceChange);
		GameData.instance.PROJECT.character.RemoveListener("NAME_CHANGE", OnHerotagChange);
		GameData.instance.PROJECT.character.mounts.OnChange.RemoveListener(OnMountChange);
		GameData.instance.PROJECT.character.mounts.OnCosmeticChange.RemoveListener(OnMountCosmeticChange);
		GameData.instance.PROJECT.ResumeDungeon();
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		infoBtn.interactable = true;
		abilitiesBtn.interactable = true;
		pointsBtn.interactable = true;
		runesBtn.interactable = true;
		enchantsBtn.interactable = true;
		achievementsBtn.interactable = true;
		_inventoryPanel.DoEnable();
		_equipmentPanel.DoEnable();
		armoryBtn.interactable = true;
		saveToArmoryBtn.interactable = true;
		heroTagBtn.interactable = true;
		historyNamesBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		infoBtn.interactable = false;
		abilitiesBtn.interactable = false;
		pointsBtn.interactable = false;
		runesBtn.interactable = false;
		enchantsBtn.interactable = false;
		achievementsBtn.interactable = false;
		_inventoryPanel.DoDisable();
		_equipmentPanel.DoDisable();
		armoryBtn.interactable = false;
		saveToArmoryBtn.interactable = false;
		heroTagBtn.interactable = false;
		historyNamesBtn.interactable = false;
	}

	private void CheckAllButtons(object e)
	{
		CheckArmoryButtons();
		CheckRunesButton();
		CheckEnchantsButton();
		CheckHeroTagButton();
		GetComponentInChildren<CharacterEquipmentPanel>().CheckAllButtons();
	}

	private void CheckRunesButton()
	{
		runesBtn.gameObject.SetActive(GameData.instance.PROJECT.character.hasRuneOrHad());
	}

	private void CheckEnchantsButton()
	{
		enchantsBtn.gameObject.SetActive(GameData.instance.PROJECT.character.hasEnchantOrHad());
	}

	private void CheckArmoryButtons()
	{
		bool active = GameData.instance.PROJECT.character.armory.armoryEquipmentSlots.Count != 0 && GameData.instance.PROJECT.character.level >= GameData.instance.PROJECT.character.armory.armoryEquipmentSlots[0].requiredLevel;
		armoryBtn.gameObject.SetActive(active);
		saveToArmoryBtn.gameObject.SetActive(active);
	}

	private void CheckHeroTagButton()
	{
		heroTagBtn.gameObject.SetActive(GameData.instance.PROJECT.character.zoneCompleted > 0);
	}
}
