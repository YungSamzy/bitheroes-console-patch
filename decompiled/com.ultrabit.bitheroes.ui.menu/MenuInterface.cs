using System;
using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.consumable;
using com.ultrabit.bitheroes.model.dialog;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.adgor;
using com.ultrabit.bitheroes.ui.consumable;
using com.ultrabit.bitheroes.ui.daily;
using com.ultrabit.bitheroes.ui.dialog;
using com.ultrabit.bitheroes.ui.dungeon;
using com.ultrabit.bitheroes.ui.game;
using com.ultrabit.bitheroes.ui.language;
using com.ultrabit.bitheroes.ui.tutorial;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.menu;

public class MenuInterface : MonoBehaviour
{
	public RectTransform[] placeholderButtonsRightBottom;

	public RectTransform placeholderSettings;

	public RectTransform placeholderCharacterTile;

	public RectTransform placeholderDailyBonusTile;

	public RectTransform placeholderDailyQuestTile;

	public RectTransform placeholderAdGorTile;

	[Header("Prefabs")]
	public Transform menuInterfaceSettingsTile;

	public Transform menuInterfaceFriendsTile;

	public Transform menuInterfaceGuildTile;

	public Transform menuInterfaceFamiliarTile;

	public Transform menuInterfaceShopTile;

	public Transform menuInterfaceDailyQuestTile;

	public Transform menuInterfaceCharacterTile;

	public Transform dailyBonusTilePrefab;

	public Transform consumableModifierTile;

	public Transform adGorTilePrefab;

	private MenuInterfaceCharacterTile _characterTile;

	private MenuInterfaceDailyQuestTile _dailyQuestTile;

	private MenuInterfaceSettingsTile _settingsTile;

	private DailyBonusTile _dailyBonusTile;

	private List<MainUIButton> _buttons;

	private List<GameObject> _interfaceTiles = new List<GameObject>();

	private List<GameTimeTile> _modifierTiles = new List<GameTimeTile>();

	private AdGorTile _adGorTile;

	private AsianLanguageFontManager asianLangManager;

	private bool _chatRepositionfirstFlag;

	public void LoadDetails()
	{
		GameData.instance.PROJECT.character.AddListener("CONSUMABLE_MODIFIER_CHANGE", OnModifierChange);
		CreateTiles();
		CreateButtons();
		UpdateModifierTiles();
		StartCoroutine(UpdateSortingLayers(1000));
		asianLangManager = base.gameObject.GetComponent<AsianLanguageFontManager>();
		if (asianLangManager != null)
		{
			asianLangManager.SetAsianFontsIfNeeded(debug: true);
		}
	}

	public void AdjustLayers()
	{
	}

	private void OnDisable()
	{
		GameData.instance.windowGenerator.chatUIPosition = placeholderButtonsRightBottom[0].position;
		GameData.instance.windowGenerator.chatUISize = placeholderButtonsRightBottom[0].sizeDelta;
	}

	private void OnEnable()
	{
		RepositionButtons();
	}

	private IEnumerator UpdateSortingLayers(int layer)
	{
		yield return null;
		Canvas[] componentsInChildren = GetComponentsInChildren<Canvas>();
		if (componentsInChildren != null)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].overrideSorting = true;
				componentsInChildren[i].sortingLayerName = "UI";
				componentsInChildren[i].sortingOrder += layer;
				componentsInChildren[i].gameObject.AddComponent<GraphicRaycaster>();
			}
		}
		CheckTutorial();
	}

	public bool CheckTutorial()
	{
		if (GameData.instance.tutorialManager == null)
		{
			return false;
		}
		if (GameData.instance.windowGenerator.GetDialogCountWithout(typeof(DungeonUI)) > 0 || GameData.instance.tutorialManager.canvas == null)
		{
			return false;
		}
		if (GameData.instance.PROJECT.dungeon != null || GameData.instance.PROJECT.battle != null)
		{
			return false;
		}
		UpdateRestrictions();
		TutorialRef tutorialRef = VariableBook.LookUpTutorial("schematic_fusion");
		if (tutorialRef != null && tutorialRef.areConditionsMet && !GameData.instance.PROJECT.character.tutorial.GetState(99) && !GameData.instance.PROJECT.character.tutorial.GetState(65))
		{
			DialogRef dialogRef = DialogBook.Lookup(VariableBook.tutorialDialogFusion);
			if (dialogRef != null)
			{
				GameData.instance.PROJECT.character.tutorial.SetState(99);
				GameData.instance.PROJECT.CheckTutorialChanges();
				GameData.instance.windowGenerator.NewDialogPopup(dialogRef).CLEAR.AddListener(OnFusionDialogClosed);
				return true;
			}
		}
		TutorialRef tutorialRef2 = VariableBook.LookUpTutorial("schematic_fusion");
		if (tutorialRef2 != null && tutorialRef2.areConditionsMet && !GameData.instance.PROJECT.character.tutorial.GetState(65))
		{
			MenuInterfaceFamiliarTile menuInterfaceFamiliarTile = GetButton(typeof(MenuInterfaceFamiliarTile)) as MenuInterfaceFamiliarTile;
			if (menuInterfaceFamiliarTile != null && menuInterfaceFamiliarTile.available)
			{
				GameData.instance.PROJECT.character.tutorial.SetState(65);
				GameData.instance.tutorialManager.ShowTutorialForButton(menuInterfaceFamiliarTile.gameObject, new TutorialPopUpSettings(Tutorial.GetText(65), 4, menuInterfaceFamiliarTile.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
				return true;
			}
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(28))
		{
			MenuInterfaceShopTile menuInterfaceShopTile = GetButton(typeof(MenuInterfaceShopTile)) as MenuInterfaceShopTile;
			if (menuInterfaceShopTile != null && menuInterfaceShopTile.available)
			{
				GameData.instance.PROJECT.character.tutorial.SetState(28);
				GameData.instance.tutorialManager.ShowTutorialForButton(menuInterfaceShopTile.gameObject, new TutorialPopUpSettings(Tutorial.GetText(28), 4, menuInterfaceShopTile.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: true, null, shadow: true, tween: true);
				return true;
			}
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(32))
		{
			MenuInterfaceGuildTile menuInterfaceGuildTile = GetButton(typeof(MenuInterfaceGuildTile)) as MenuInterfaceGuildTile;
			if (menuInterfaceGuildTile != null && menuInterfaceGuildTile.available)
			{
				GameData.instance.PROJECT.character.tutorial.SetState(32);
				GameData.instance.tutorialManager.ShowTutorialForButton(menuInterfaceGuildTile.gameObject, new TutorialPopUpSettings(Tutorial.GetText(32), 4, menuInterfaceGuildTile.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: true, null, shadow: true, tween: true);
				GameData.instance.PROJECT.CheckTutorialChanges();
				return true;
			}
		}
		new Vector2(_characterTile.transform.localPosition.x, _characterTile.transform.localPosition.y + _characterTile.baseBackground.sizeDelta.y * 1.3f);
		if (!GameData.instance.PROJECT.character.tutorial.GetState(34) && GameData.instance.PROJECT.character.points > 0 && _characterTile != null && _characterTile.available)
		{
			GameData.instance.PROJECT.character.tutorial.SetState(34);
			GameData.instance.tutorialManager.ShowTutorialForButton(_characterTile.gameObject, new TutorialPopUpSettings(Tutorial.GetText(34), 4, _characterTile.gameObject, 0f, indicator: false, button: false, glow: true, 250, new Vector2(AppInfo.GetLeftOffset(), 0f)), stageTrigger: false, null, funcSameAsTargetFunc: true, null, shadow: true, tween: true);
			GameData.instance.PROJECT.CheckTutorialChanges();
			return true;
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(41) && GameData.instance.PROJECT.character.GetUpgradeEquipment() != null && _characterTile != null && _characterTile.available)
		{
			GameData.instance.PROJECT.character.tutorial.SetState(41);
			GameData.instance.tutorialManager.ShowTutorialForButton(_characterTile.gameObject, new TutorialPopUpSettings(Tutorial.GetText(41), 4, _characterTile.gameObject, 0f, indicator: false, button: false, glow: true, 250, new Vector2(AppInfo.GetLeftOffset(), 0f)), stageTrigger: false, null, funcSameAsTargetFunc: true, null, shadow: true, tween: true);
			GameData.instance.PROJECT.CheckTutorialChanges();
			return true;
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(48) && GameData.instance.PROJECT.character.dailyQuests.hasQuestDataLootable())
		{
			MenuInterfaceDailyQuestTile menuInterfaceDailyQuestTile = GetButton(typeof(MenuInterfaceDailyQuestTile)) as MenuInterfaceDailyQuestTile;
			if (menuInterfaceDailyQuestTile != null && menuInterfaceDailyQuestTile.available)
			{
				new Vector2(menuInterfaceDailyQuestTile.transform.position.x, menuInterfaceDailyQuestTile.transform.position.y + 35f);
				GameData.instance.PROJECT.character.tutorial.SetState(48);
				GameData.instance.tutorialManager.ShowTutorialForButton(menuInterfaceDailyQuestTile.gameObject, new TutorialPopUpSettings(Tutorial.GetText(48), 4, menuInterfaceDailyQuestTile.gameObject, 0f, indicator: false, button: false, glow: true, 250, new Vector2(AppInfo.GetLeftOffset(), 0f)), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
				GameData.instance.PROJECT.CheckTutorialChanges();
				return true;
			}
		}
		TutorialRef tutorialRef3 = VariableBook.LookUpTutorial("augment_shop");
		if (tutorialRef3 != null && !GameData.instance.PROJECT.character.tutorial.GetState(68) && tutorialRef3.areConditionsMet)
		{
			MenuInterfaceFamiliarTile menuInterfaceFamiliarTile2 = GetButton(typeof(MenuInterfaceFamiliarTile)) as MenuInterfaceFamiliarTile;
			if (menuInterfaceFamiliarTile2 != null && menuInterfaceFamiliarTile2.available)
			{
				GameData.instance.PROJECT.character.tutorial.SetState(68);
				GameData.instance.tutorialManager.ShowTutorialForButton(menuInterfaceFamiliarTile2.gameObject, new TutorialPopUpSettings(Tutorial.GetText(68), 4, menuInterfaceFamiliarTile2.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
				return true;
			}
		}
		TutorialRef tutorialRef4 = VariableBook.LookUpTutorial("pet_equip");
		if (tutorialRef4 != null && !GameData.instance.PROJECT.character.tutorial.GetState(76) && tutorialRef4.areConditionsMet && _characterTile != null && _characterTile.available)
		{
			GameData.instance.PROJECT.character.tutorial.SetState(76);
			GameData.instance.tutorialManager.ShowTutorialForButton(_characterTile.gameObject, new TutorialPopUpSettings(Tutorial.GetText(76), 4, _characterTile.gameObject, 0f, indicator: false, button: false, glow: true, 250, new Vector2(AppInfo.GetLeftOffset(), 0f)), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
			return true;
		}
		TutorialRef tutorialRef5 = VariableBook.LookUpTutorial("accessory_equip");
		if (tutorialRef5 != null && !GameData.instance.PROJECT.character.tutorial.GetState(80) && tutorialRef5.areConditionsMet && _characterTile != null && _characterTile.available)
		{
			GameData.instance.PROJECT.character.tutorial.SetState(80);
			GameData.instance.tutorialManager.ShowTutorialForButton(_characterTile.gameObject, new TutorialPopUpSettings(Tutorial.GetText(80), 4, _characterTile.gameObject, 0f, indicator: false, button: false, glow: true, 250, new Vector2(AppInfo.GetLeftOffset(), 0f)), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
			return true;
		}
		TutorialRef tutorialRef6 = VariableBook.LookUpTutorial("mount_summon");
		if (tutorialRef6 != null && !GameData.instance.PROJECT.character.tutorial.GetState(84) && tutorialRef6.areConditionsMet && _characterTile != null && _characterTile.available)
		{
			GameData.instance.PROJECT.character.tutorial.SetState(84);
			GameData.instance.tutorialManager.ShowTutorialForButton(_characterTile.gameObject, new TutorialPopUpSettings(Tutorial.GetText(84), 4, _characterTile.gameObject, 0f, indicator: false, button: false, glow: true, 250, new Vector2(AppInfo.GetLeftOffset(), 0f)), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
			return true;
		}
		TutorialRef tutorialRef7 = VariableBook.LookUpTutorial("runes_shop");
		if (tutorialRef7 != null && !GameData.instance.PROJECT.character.tutorial.GetState(89) && tutorialRef7.areConditionsMet && _characterTile != null && _characterTile.available)
		{
			GameData.instance.PROJECT.character.tutorial.SetState(89);
			GameData.instance.tutorialManager.ShowTutorialForButton(_characterTile.gameObject, new TutorialPopUpSettings(Tutorial.GetText(89), 4, _characterTile.gameObject, 0f, indicator: false, button: false, glow: true, 250, new Vector2(AppInfo.GetLeftOffset(), 0f)), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
			return true;
		}
		TutorialRef tutorialRef8 = VariableBook.LookUpTutorial("enchants_identify");
		if (tutorialRef8 != null && !GameData.instance.PROJECT.character.tutorial.GetState(94) && tutorialRef8.areConditionsMet && _characterTile != null && _characterTile.available)
		{
			GameData.instance.PROJECT.character.tutorial.SetState(94);
			GameData.instance.tutorialManager.ShowTutorialForButton(_characterTile.gameObject, new TutorialPopUpSettings(Tutorial.GetText(94), 4, _characterTile.gameObject, 0f, indicator: false, button: false, glow: true, 250, new Vector2(AppInfo.GetLeftOffset(), 0f)), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
			return true;
		}
		TutorialRef tutorialRef9 = VariableBook.LookUpTutorial("craft_upgrade");
		if (tutorialRef9 != null && !GameData.instance.PROJECT.character.tutorial.GetState(103) && tutorialRef9.areConditionsMet)
		{
			DialogRef dialogRef2 = DialogBook.Lookup(VariableBook.tutorialDialogUpgrade);
			if (dialogRef2 != null && !GameData.instance.PROJECT.character.tutorial.GetState(107))
			{
				GameData.instance.PROJECT.character.tutorial.SetState(107);
				GameData.instance.PROJECT.CheckTutorialChanges();
				GameData.instance.windowGenerator.NewDialogPopup(dialogRef2).CLEAR.AddListener(OnFusionDialogClosed);
				return true;
			}
			if (_characterTile != null && _characterTile.available)
			{
				GameData.instance.PROJECT.character.tutorial.SetState(103);
				GameData.instance.tutorialManager.ShowTutorialForButton(_characterTile.gameObject, new TutorialPopUpSettings(Tutorial.GetText(103), 4, _characterTile.gameObject, 0f, indicator: false, button: false, glow: true, 250, new Vector2(AppInfo.GetLeftOffset(), 0f)), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
				GameData.instance.PROJECT.CheckTutorialChanges();
				return true;
			}
		}
		TutorialRef tutorialRef10 = VariableBook.LookUpTutorial("friends_suggest");
		if (tutorialRef10 != null && tutorialRef10.areConditionsMet && !GameData.instance.PROJECT.character.tutorial.GetState(127))
		{
			MenuInterfaceFriendsTile menuInterfaceFriendsTile = GetButton(typeof(MenuInterfaceFriendsTile)) as MenuInterfaceFriendsTile;
			if (menuInterfaceFriendsTile != null && menuInterfaceFriendsTile.available)
			{
				GameData.instance.PROJECT.character.tutorial.SetState(127);
				GameData.instance.tutorialManager.ShowTutorialForButton(menuInterfaceFriendsTile.gameObject, new TutorialPopUpSettings(Tutorial.GetText(127), 4, menuInterfaceFriendsTile.gameObject, 0f, indicator: false, button: false, glow: true, 250, new Vector2(AppInfo.GetLeftOffset(), 0f)), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
				return true;
			}
		}
		TutorialRef tutorialRef11 = VariableBook.LookUpTutorial("adgor_check");
		if (tutorialRef11 != null && !GameData.instance.PROJECT.character.tutorial.GetState(101) && tutorialRef11.areConditionsMet && _adGorTile != null)
		{
			if (AppInfo.allowAds)
			{
				GameData.instance.PROJECT.character.tutorial.SetState(101);
				GameData.instance.tutorialManager.ShowTutorialForButton(_adGorTile.gameObject, new TutorialPopUpSettings(Tutorial.GetText(101), 4, _adGorTile.gameObject, 0f, indicator: false, button: false, glow: true, 250, new Vector2(AppInfo.GetLeftOffset(), 0f)), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
				return true;
			}
			return false;
		}
		return false;
	}

	private void OnFusionDialogClosed(object e)
	{
		(e as DialogPopup).CLEAR.RemoveListener(OnFusionDialogClosed);
		CheckTutorial();
	}

	private void CreateButtons()
	{
		if (_buttons == null)
		{
			_buttons = new List<MainUIButton>();
			AddButton(menuInterfaceShopTile);
			AddSpace();
			AddButton(menuInterfaceFriendsTile);
			AddButton(menuInterfaceGuildTile);
			AddButton(menuInterfaceFamiliarTile);
			CreateExtraButtons();
			RepositionButtons();
		}
	}

	private void CreateExtraButtons()
	{
		if (_dailyQuestTile == null && (GameData.instance.PROJECT.character.tutorial.GetState(48) || (!GameData.instance.PROJECT.character.tutorial.GetState(48) && GameData.instance.PROJECT.character.dailyQuests.hasQuestDataLootable() && GameData.instance.PROJECT.dungeon == null)))
		{
			Transform transform = UnityEngine.Object.Instantiate(menuInterfaceDailyQuestTile);
			transform.SetParent(base.transform, worldPositionStays: false);
			((RectTransform)transform).anchoredPosition = new Vector2(0f, 0f);
			transform.GetComponent<MainUIButton>().Create();
			_buttons.Add(transform.GetComponent<MainUIButton>());
			_dailyQuestTile = transform.GetComponent<MenuInterfaceDailyQuestTile>();
		}
		if (_settingsTile == null)
		{
			Transform transform = UnityEngine.Object.Instantiate(menuInterfaceSettingsTile);
			transform.SetParent(placeholderSettings, worldPositionStays: false);
			transform.GetComponent<MainUIButton>().Create();
			_buttons.Add(transform.GetComponent<MainUIButton>());
			_settingsTile = transform.GetComponent<MenuInterfaceSettingsTile>();
		}
	}

	private void AddButton(Transform button)
	{
		Transform transform = UnityEngine.Object.Instantiate(button);
		transform.SetParent(base.transform, worldPositionStays: false);
		transform.GetComponent<MainUIButton>().Create();
		_buttons.Add(transform.GetComponent<MainUIButton>());
	}

	private void AddSpace()
	{
		new GameObject("placeholderChatTile").AddComponent<RectTransform>().SetParent(base.transform, worldPositionStays: false);
		_buttons.Add(null);
	}

	public Component GetButton(Type cl)
	{
		foreach (MainUIButton button in _buttons)
		{
			if (button != null && button.GetComponent(cl) != null)
			{
				return button.GetComponent(cl);
			}
		}
		return null;
	}

	private void CreateTiles()
	{
		Transform transform = null;
		if (_characterTile == null)
		{
			transform = UnityEngine.Object.Instantiate(menuInterfaceCharacterTile);
			transform.SetParent(base.transform, worldPositionStays: false);
			transform.GetComponent<MainUIButton>().Create();
			_characterTile = transform.GetComponent<MenuInterfaceCharacterTile>();
			transform = null;
		}
		if (_dailyBonusTile == null && VariableBook.GetGameRequirement(11).RequirementsMet())
		{
			transform = UnityEngine.Object.Instantiate(dailyBonusTilePrefab);
			transform.SetParent(base.transform, worldPositionStays: false);
			_dailyBonusTile = transform.GetComponent<DailyBonusTile>();
			_dailyBonusTile.LoadDetails();
			_dailyBonusTile.CHANGE.AddListener(OnModifierChange);
			transform = null;
		}
		TutorialRef tutorialRef = VariableBook.LookUpTutorial("adgor_check");
		Project pROJECT = GameData.instance.PROJECT;
		Dungeon dungeon = pROJECT.dungeon;
		Tutorial tutorial = pROJECT.character.tutorial;
		if (_adGorTile == null && ((!tutorialRef.isMaxFlagConditionMet && tutorialRef.isMinFlagConditionMet) || tutorial.GetState(101) || (!tutorial.GetState(101) && tutorialRef.areConditionsMet && dungeon == null)))
		{
			transform = UnityEngine.Object.Instantiate(adGorTilePrefab);
			transform.SetParent(base.transform, worldPositionStays: false);
			_adGorTile = transform.GetComponent<AdGorTile>();
			_adGorTile.LoadDetails();
		}
		RepositionObjects();
	}

	private void OnScreenUpdate()
	{
		RepositionObjects();
	}

	private void RepositionObjects()
	{
		RepositionTiles();
		RepositionModifierTiles();
		RepositionButtons();
	}

	[ContextMenu("RepositionButtons()")]
	private void RepositionButtons()
	{
		SetDailyQuestButton();
		SetOtherButtons();
		void SetDailyQuestButton()
		{
			if (!(_dailyQuestTile == null))
			{
				(_dailyQuestTile.transform as RectTransform).SetParent(placeholderDailyQuestTile, worldPositionStays: false);
				RectTransform rectTransform = null;
				if (_adGorTile != null)
				{
					rectTransform = (RectTransform)_adGorTile.transform.parent;
				}
				else if (_dailyBonusTile != null)
				{
					rectTransform = (RectTransform)_dailyBonusTile.transform.parent;
				}
				else if (_modifierTiles != null && _modifierTiles.Count > 0)
				{
					rectTransform = (RectTransform)_modifierTiles[0].transform;
				}
				if (rectTransform == null)
				{
					placeholderDailyQuestTile.anchoredPosition *= Vector2.right;
				}
				else
				{
					placeholderDailyQuestTile.anchoredPosition = new Vector2(placeholderDailyQuestTile.anchoredPosition.x, rectTransform.rect.height + (rectTransform.anchoredPosition.y - rectTransform.rect.height * rectTransform.pivot.y));
				}
			}
		}
		void SetOtherButtons()
		{
			if (_buttons == null)
			{
				return;
			}
			int num = 0;
			foreach (MainUIButton button in _buttons)
			{
				if (button == null)
				{
					if (VariableBook.GetGameRequirement(4).RequirementsMet())
					{
						if (!_chatRepositionfirstFlag)
						{
							GameData.instance.windowGenerator.SetChatUIDefaultParent(placeholderButtonsRightBottom[num]);
							_chatRepositionfirstFlag = true;
						}
						else
						{
							GameData.instance.windowGenerator.chatUIPosition = placeholderButtonsRightBottom[num].position;
							GameData.instance.windowGenerator.chatUISize = placeholderButtonsRightBottom[num].sizeDelta;
						}
						num++;
					}
				}
				else if (!GameData.instance.tutorialManager.TargetObjectEquals(button.gameObject) && !(button.GetComponent<MenuInterfaceDailyQuestTile>() != null) && !(button.GetComponent<MenuInterfaceSettingsTile>() != null) && button.gameObject.activeSelf)
				{
					button.transform.SetParent(placeholderButtonsRightBottom[num], worldPositionStays: false);
					num++;
				}
			}
		}
	}

	private void RepositionTiles()
	{
		if (_characterTile != null)
		{
			_characterTile.transform.SetParent(placeholderCharacterTile, worldPositionStays: false);
			_characterTile.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
		}
		if (_dailyBonusTile != null)
		{
			_dailyBonusTile.transform.SetParent(placeholderDailyBonusTile, worldPositionStays: false);
			_dailyBonusTile.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
			if (_adGorTile == null)
			{
				placeholderDailyBonusTile.anchoredPosition = placeholderAdGorTile.anchoredPosition;
			}
			else
			{
				placeholderDailyBonusTile.anchoredPosition = new Vector2(placeholderAdGorTile.anchoredPosition.x + placeholderAdGorTile.sizeDelta.x * 0.5f + placeholderDailyBonusTile.sizeDelta.x * 0.5f, placeholderAdGorTile.anchoredPosition.y);
			}
		}
		if (_adGorTile != null)
		{
			_adGorTile.transform.SetParent(placeholderAdGorTile, worldPositionStays: false);
			_adGorTile.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
		}
	}

	private void OnModifierChange()
	{
		UpdateModifierTiles();
	}

	private void UpdateModifierTiles()
	{
		foreach (GameTimeTile modifierTile in _modifierTiles)
		{
			if (!(modifierTile == null) && !(modifierTile.GetComponent<DailyBonusTile>() != null))
			{
				modifierTile.CHANGE.RemoveListener(OnModifierChange);
				UnityEngine.Object.Destroy(modifierTile.gameObject);
			}
		}
		_modifierTiles.Clear();
		if (_dailyBonusTile != null)
		{
			_modifierTiles.Add(_dailyBonusTile);
			foreach (ConsumableModifierData consumableModifier in GameData.instance.PROJECT.character.consumableModifiers)
			{
				if (consumableModifier != null && consumableModifier.isActive() && !consumableModifier.consumableRef.hidden)
				{
					Transform obj = UnityEngine.Object.Instantiate(consumableModifierTile);
					obj.SetParent(base.transform, worldPositionStays: false);
					ConsumableModifierTile component = obj.GetComponent<ConsumableModifierTile>();
					component.LoadDetails(consumableModifier);
					component.CHANGE.AddListener(OnModifierChange);
					_modifierTiles.Add(component);
				}
			}
			for (int i = 0; i < _modifierTiles.Count; i++)
			{
				_modifierTiles[i].transform.SetSiblingIndex(_dailyBonusTile.transform.GetSiblingIndex() + i);
			}
		}
		RepositionObjects();
	}

	private void RepositionModifierTiles()
	{
		if (_modifierTiles == null)
		{
			return;
		}
		for (int i = 0; i < _modifierTiles.Count; i++)
		{
			GameTimeTile gameTimeTile = _modifierTiles[i];
			if (gameTimeTile.gameObject.activeSelf && gameTimeTile.GetComponent<DailyBonusTile>() == null)
			{
				gameTimeTile.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
				gameTimeTile.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 0f);
				gameTimeTile.GetComponent<RectTransform>().anchoredPosition = new Vector2(placeholderDailyBonusTile.anchoredPosition.x + placeholderDailyBonusTile.sizeDelta.x * (float)i, placeholderDailyBonusTile.anchoredPosition.y);
			}
		}
	}

	public void UpdateRestrictions()
	{
		CreateExtraButtons();
		CreateTiles();
		foreach (MainUIButton button in _buttons)
		{
			if (button != null)
			{
				button.DoUpdate();
			}
		}
		RepositionButtons();
		RepositionTiles();
		UpdateModifierTiles();
	}

	public void Clear()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void OnDestroy()
	{
		if (GameData.instance.PROJECT.character != null)
		{
			GameData.instance.PROJECT.character.RemoveListener("CONSUMABLE_MODIFIER_CHANGE", OnModifierChange);
		}
		if (_dailyBonusTile != null)
		{
			_dailyBonusTile.CHANGE.RemoveListener(OnModifierChange);
		}
	}
}
