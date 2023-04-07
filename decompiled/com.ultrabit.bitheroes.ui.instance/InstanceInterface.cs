using System;
using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.dialog;
using com.ultrabit.bitheroes.model.guild;
using com.ultrabit.bitheroes.model.instance;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.payment;
using com.ultrabit.bitheroes.model.pvp;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.dialog;
using com.ultrabit.bitheroes.ui.language;
using com.ultrabit.bitheroes.ui.menu;
using com.ultrabit.bitheroes.ui.tutorial;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.instance;

public class InstanceInterface : MonoBehaviour
{
	public Button exitBtn;

	public Button editBtn;

	[SerializeField]
	private RectTransform[] placeholderButtonsLeft;

	[SerializeField]
	private RectTransform[] placeholderButtonsRightBottom;

	[SerializeField]
	private RectTransform[] placeholderButtonsRightTop;

	public Transform menuInterfaceQuestTile;

	public Transform menuInterfacePvPTile;

	public Transform menuInterfaceCraftTile;

	public Transform menuInterfaceBrawlTile;

	public Transform menuInterfaceRaidTile;

	public Transform menuInterfaceFishingTile;

	public Transform menuInterfaceKongregateTile;

	public Transform menuInterfaceEventTokenTile;

	public Transform menuInterfaceEventBadgeTile;

	public Transform menuInterfaceAdTile;

	public Transform menuInterfaceNBPTile;

	public Transform menuInterfaceNBPZTile;

	public Transform menuInterfaceBoostersTile;

	public Transform menuInterfaceEventSalesShopTile;

	private IEnumerator _timer;

	private Instance _instance;

	private List<MainUIButton> _buttonsLeft;

	private List<MainUIButton> _buttonsRightTop;

	private List<MainUIButton> _buttonsRightBottom;

	private AsianLanguageFontManager asianLangManager;

	private GameObject menuInterfaceKongregateInstance;

	public void LoadDetails(Instance instance)
	{
		_instance = instance;
		CreateButtons();
		RepositionButtons();
		UpdateUIButtons();
		GameData.instance.PROJECT.character.AddListener("AD_MILLISECONDS_CHANGE", OnCharacterAdMilliseconds);
		GameData.instance.PROJECT.character.AddListener("AD_READY", OnCharacterAdReady);
		GameData.instance.PROJECT.character.AddListener("NBP_DATE_CHANGE", OnCharacterNBPChange);
		GameData.instance.PROJECT.character.AddListener("INVENTORY_CHANGE", OnCharacterInventoryChange);
		asianLangManager = base.gameObject.GetComponent<AsianLanguageFontManager>();
		if (asianLangManager != null)
		{
			asianLangManager.SetAsianFontsIfNeeded(debug: true);
		}
	}

	public void OnExitBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoEnterTown();
	}

	public void OnEditBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (GameData.instance.PROJECT.character.guildData != null && !GameData.instance.PROJECT.character.guildData.hasPermission(6))
		{
			GameData.instance.windowGenerator.ShowErrorCode(58);
		}
		else
		{
			GameData.instance.PROJECT.ToggleGuildHallEditMode();
		}
	}

	private void CreateButtons()
	{
		if (_buttonsLeft == null && _buttonsRightTop == null && _buttonsRightBottom == null)
		{
			_buttonsLeft = new List<MainUIButton>();
			_buttonsRightTop = new List<MainUIButton>();
			_buttonsRightBottom = new List<MainUIButton>();
			AddButtonLeft(menuInterfaceQuestTile);
			AddButtonLeft(menuInterfacePvPTile);
			if (VariableBook.brawlVisible)
			{
				AddButtonLeft(menuInterfaceBrawlTile);
			}
			if (VariableBook.raidVisible)
			{
				AddButtonLeft(menuInterfaceRaidTile);
			}
			if (AppInfo.kongButtonAvailable)
			{
				menuInterfaceKongregateInstance = AddButtonRightTop(menuInterfaceKongregateTile);
			}
			else
			{
				menuInterfaceKongregateInstance = null;
			}
			AddButtonRightTop(menuInterfaceCraftTile);
			AddButtonRightBottom(menuInterfaceFishingTile);
			AddButtonRightBottom(menuInterfaceEventTokenTile);
			AddButtonRightBottom(menuInterfaceEventBadgeTile);
			if (AppInfo.allowAds)
			{
				AddButtonRightBottom(menuInterfaceAdTile);
			}
			AddButtonRightBottom(menuInterfaceBoostersTile);
			AddButtonRightBottom(menuInterfaceEventSalesShopTile);
			UpdateTiles();
		}
	}

	private void AddButtonLeft(Transform button)
	{
		Transform transform = UnityEngine.Object.Instantiate(button);
		transform.SetParent(base.transform, worldPositionStays: false);
		transform.GetComponent<MainUIButton>().Create();
		_buttonsLeft.Add(transform.GetComponent<MainUIButton>());
	}

	private GameObject AddButtonRightTop(Transform button)
	{
		Transform transform = UnityEngine.Object.Instantiate(button);
		transform.SetParent(base.transform, worldPositionStays: false);
		transform.GetComponent<MainUIButton>().Create();
		_buttonsRightTop.Add(transform.GetComponent<MainUIButton>());
		return transform.gameObject;
	}

	private GameObject AddButtonRightBottom(Transform button)
	{
		Transform transform = UnityEngine.Object.Instantiate(button);
		transform.SetParent(base.transform, worldPositionStays: false);
		transform.GetComponent<MainUIButton>().Create();
		_buttonsRightBottom.Add(transform.GetComponent<MainUIButton>());
		return transform.gameObject;
	}

	public Component GetButton(Type cl)
	{
		foreach (MainUIButton item in _buttonsLeft)
		{
			if (item.GetComponent(cl) != null)
			{
				return item.GetComponent(cl);
			}
		}
		foreach (MainUIButton item2 in _buttonsRightTop)
		{
			if (item2.GetComponent(cl) != null)
			{
				return item2.GetComponent(cl);
			}
		}
		foreach (MainUIButton item3 in _buttonsRightBottom)
		{
			if (item3.GetComponent(cl) != null)
			{
				return item3.GetComponent(cl);
			}
		}
		return null;
	}

	private void OnCharacterAdMilliseconds()
	{
		UpdateAdButton();
	}

	private void OnCharacterAdReady()
	{
		UpdateAdButton();
	}

	private void OnCharacterNBPChange()
	{
		UpdateNBPButton();
	}

	private void OnCharacterInventoryChange()
	{
		UpdateNBPButton();
	}

	public void UpdateUIButtons()
	{
		exitBtn.gameObject.SetActive(value: false);
		if (_instance == null)
		{
			editBtn.gameObject.SetActive(value: false);
		}
		else if (_instance.instanceRef.type == 2)
		{
			GuildHallData guildHallData = _instance.data as GuildHallData;
			editBtn.gameObject.SetActive(GameData.instance.PROJECT.character.guildData != null && guildHallData.guildID == GameData.instance.PROJECT.character.guildData.id);
		}
		else
		{
			editBtn.gameObject.SetActive(value: false);
		}
	}

	public void UpdateTiles()
	{
		UpdateEventTokenButton();
		UpdateEventBadgeButton();
		UpdateEventSalesShopButton();
		UpdateAdButton();
		UpdateNBPButton();
		UpdateTimer();
		RepositionButtons();
	}

	public void UpdateEventTokenButton()
	{
		MenuInterfaceEventTokenTile menuInterfaceEventTokenTile = GetButton(typeof(MenuInterfaceEventTokenTile)) as MenuInterfaceEventTokenTile;
		if (!(menuInterfaceEventTokenTile == null))
		{
			if (menuInterfaceEventTokenTile.eventRef == null)
			{
				menuInterfaceEventTokenTile.gameObject.SetActive(value: false);
			}
			RepositionButtons();
		}
	}

	public void UpdateEventBadgeButton()
	{
		MenuInterfaceEventBadgeTile menuInterfaceEventBadgeTile = GetButton(typeof(MenuInterfaceEventBadgeTile)) as MenuInterfaceEventBadgeTile;
		if (!(menuInterfaceEventBadgeTile == null))
		{
			if (menuInterfaceEventBadgeTile.eventRef == null)
			{
				menuInterfaceEventBadgeTile.gameObject.SetActive(value: false);
			}
			RepositionButtons();
		}
	}

	public void UpdateEventSalesShopButton()
	{
		MenuInterfaceEventSalesShopTile menuInterfaceEventSalesShopTile = GetButton(typeof(MenuInterfaceEventSalesShopTile)) as MenuInterfaceEventSalesShopTile;
		if (!(menuInterfaceEventSalesShopTile == null))
		{
			menuInterfaceEventSalesShopTile.UpdateNotification();
			if (menuInterfaceEventSalesShopTile.eventRef == null)
			{
				menuInterfaceEventSalesShopTile.gameObject.SetActive(value: false);
			}
			RepositionButtons();
		}
	}

	public void UpdateAdButton()
	{
		MenuInterfaceAdTile menuInterfaceAdTile = GetButton(typeof(MenuInterfaceAdTile)) as MenuInterfaceAdTile;
		if (!(menuInterfaceAdTile == null))
		{
			GameData.instance.main.AddBreadcrumb("InstanceInterface:UpdateAdButton");
			if (!AppInfo.adsAvailable || GameData.instance.PROJECT.character.adMilliseconds < 0)
			{
				GameData.instance.main.AddBreadcrumb("InstanceInterface:UpdateAdButton Hide");
				menuInterfaceAdTile.DoHide();
			}
			else
			{
				GameData.instance.main.AddBreadcrumb("InstanceInterface:UpdateAdButton Show");
				menuInterfaceAdTile.DoShow();
			}
			RepositionButtons();
		}
	}

	public void UpdateNBPButton()
	{
		MenuInterfaceNBPTile menuInterfaceNBPTile = GetButton(typeof(MenuInterfaceNBPTile)) as MenuInterfaceNBPTile;
		if (!(menuInterfaceNBPTile == null))
		{
			PaymentRef firstPaymentByType = PaymentBook.GetFirstPaymentByType(3);
			bool flag = firstPaymentByType == null || (firstPaymentByType.itemData.itemRef.unique && GameData.instance.PROJECT.character.inventory.hasOwnedItem(firstPaymentByType.itemData.itemRef)) || !AppInfo.allowNBP || !GameData.instance.PROJECT.character.nbpDate.HasValue || GameData.instance.PROJECT.character.nbpMilliseconds <= 0;
			if (flag)
			{
				menuInterfaceNBPTile.DoHide();
			}
			else
			{
				menuInterfaceNBPTile.DoShow();
			}
			menuInterfaceNBPTile.gameObject.SetActive(!flag);
			RepositionButtons();
		}
	}

	[ContextMenu("RepositionButtons()")]
	public void RepositionButtons()
	{
		try
		{
			RepositionButtonsLeft();
			RepositionButtonsRightTop();
			RepositionButtonsRightBottom();
		}
		catch (Exception e)
		{
			D.LogException("RepositionButtons", e);
		}
	}

	private void RepositionButtonsLeft()
	{
		int num = 0;
		for (int i = 0; i < _buttonsLeft.Count; i++)
		{
			if (_buttonsLeft[i].gameObject.activeSelf && !GameData.instance.tutorialManager.TargetObjectEquals(_buttonsLeft[i].gameObject))
			{
				_buttonsLeft[i].transform.SetParent(placeholderButtonsLeft[num], worldPositionStays: false);
				num++;
			}
		}
	}

	private void RepositionButtonsRightTop()
	{
		int num = 0;
		for (int i = 0; i < _buttonsRightTop.Count; i++)
		{
			if (_buttonsRightTop[i].gameObject.activeSelf && !GameData.instance.tutorialManager.TargetObjectEquals(_buttonsRightTop[i].gameObject))
			{
				_buttonsRightTop[i].transform.SetParent(placeholderButtonsRightTop[num], worldPositionStays: false);
				num++;
			}
		}
	}

	private void RepositionButtonsRightBottom()
	{
		int num = 0;
		for (int i = 0; i < _buttonsRightBottom.Count; i++)
		{
			if (_buttonsRightBottom[i].gameObject.activeInHierarchy && !GameData.instance.tutorialManager.TargetObjectEquals(_buttonsRightBottom[i].gameObject))
			{
				_buttonsRightBottom[i].transform.SetParent(placeholderButtonsRightBottom[num], worldPositionStays: false);
				num++;
			}
		}
	}

	public bool CheckTutorial()
	{
		if (GameData.instance.tutorialManager.hasPopup || GameData.instance.tutorialManager.canvas == null)
		{
			return false;
		}
		if (GameData.instance.PROJECT.dungeon != null || GameData.instance.PROJECT.battle != null)
		{
			return false;
		}
		UpdateRestrictions();
		if (VariableBook.GameRequirementMet(2) && !GameData.instance.PROJECT.character.tutorial.GetState(100) && !GameData.instance.PROJECT.character.tutorial.GetState(24))
		{
			if (GameData.instance.PROJECT.character.zoneCompleted > 3)
			{
				int[] array = new int[6] { 100, 24, 27, 115, 116, 117 };
				foreach (int id in array)
				{
					GameData.instance.PROJECT.character.tutorial.SetState(id);
					GameData.instance.PROJECT.CheckTutorialChanges();
				}
				return false;
			}
			DialogRef dialogRef = DialogBook.Lookup(VariableBook.tutorialDialogCraft);
			if (dialogRef != null)
			{
				GameData.instance.PROJECT.character.tutorial.SetState(100);
				GameData.instance.PROJECT.CheckTutorialChanges();
				GameData.instance.windowGenerator.NewDialogPopup(dialogRef).CLEAR.AddListener(OnCraftDialogClosed);
				return true;
			}
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(24))
		{
			if (GameData.instance.PROJECT.character.zoneCompleted > 3)
			{
				int[] array = new int[6] { 100, 24, 27, 115, 116, 117 };
				foreach (int id2 in array)
				{
					GameData.instance.PROJECT.character.tutorial.SetState(id2);
					GameData.instance.PROJECT.CheckTutorialChanges();
				}
				return false;
			}
			MenuInterfaceCraftTile menuInterfaceCraftTile = GetButton(typeof(MenuInterfaceCraftTile)) as MenuInterfaceCraftTile;
			if (menuInterfaceCraftTile != null && menuInterfaceCraftTile.available)
			{
				GameData.instance.PROJECT.character.tutorial.SetState(24);
				GameData.instance.tutorialManager.ShowTutorialForButton(menuInterfaceCraftTile.gameObject, new TutorialPopUpSettings(Tutorial.GetText(24), 3, menuInterfaceCraftTile.gameObject, 0f, indicator: false, button: false, glow: true, 250, new Vector2(AppInfo.GetRightOffset(), 0f)), stageTrigger: false, null, funcSameAsTargetFunc: true, null, shadow: true, tween: true);
				return true;
			}
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(62))
		{
			MenuInterfaceBoostersTile menuInterfaceBoostersTile = GetButton(typeof(MenuInterfaceBoostersTile)) as MenuInterfaceBoostersTile;
			if ((bool)menuInterfaceBoostersTile && menuInterfaceBoostersTile.available)
			{
				GameData.instance.PROJECT.character.tutorial.SetState(62);
				GameData.instance.tutorialManager.ShowTutorialForButton(menuInterfaceBoostersTile.gameObject, new TutorialPopUpSettings(Tutorial.GetText(62), 4, menuInterfaceBoostersTile.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: true, null, shadow: true, tween: true);
				GameData.instance.PROJECT.CheckTutorialChanges();
				return true;
			}
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(0) && !GameData.instance.PROJECT.character.tutorial.GetState(1))
		{
			GameData.instance.PROJECT.character.tutorial.SetState(0);
			GameData.instance.windowGenerator.NewDialogPopup(DialogBook.Lookup(VariableBook.tutorialDialogIntro), null, showKongButton: true);
			return true;
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(1))
		{
			MenuInterfaceQuestTile menuInterfaceQuestTile = GetButton(typeof(MenuInterfaceQuestTile)) as MenuInterfaceQuestTile;
			if (menuInterfaceQuestTile != null && menuInterfaceQuestTile.available)
			{
				GameData.instance.PROJECT.character.tutorial.SetState(1);
				GameData.instance.tutorialManager.ShowTutorialForButton(menuInterfaceQuestTile.gameObject, new TutorialPopUpSettings(Tutorial.GetText(1), 1, menuInterfaceQuestTile.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: true, null, shadow: true, tween: true);
				return true;
			}
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(21) && PvPEventBook.GetCurrentEventRef() != null)
		{
			MenuInterfacePvPTile menuInterfacePvPTile = GetButton(typeof(MenuInterfacePvPTile)) as MenuInterfacePvPTile;
			if (menuInterfacePvPTile != null && menuInterfacePvPTile.available)
			{
				GameData.instance.PROJECT.character.tutorial.SetState(21);
				GameData.instance.tutorialManager.ShowTutorialForButton(menuInterfacePvPTile.gameObject, new TutorialPopUpSettings(Tutorial.GetText(21), 3, menuInterfacePvPTile.gameObject, 0f, indicator: false, button: false, glow: true, 250, new Vector2(AppInfo.GetLeftOffset(), 0f)), stageTrigger: false, null, funcSameAsTargetFunc: true, null, shadow: true, tween: true);
				GameData.instance.PROJECT.CheckTutorialChanges();
				return true;
			}
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(57))
		{
			MenuInterfaceFishingTile menuInterfaceFishingTile = GetButton(typeof(MenuInterfaceFishingTile)) as MenuInterfaceFishingTile;
			if (menuInterfaceFishingTile != null && menuInterfaceFishingTile.available)
			{
				GameData.instance.PROJECT.character.tutorial.SetState(57);
				GameData.instance.tutorialManager.ShowTutorialForButton(menuInterfaceFishingTile.gameObject, new TutorialPopUpSettings(Tutorial.GetText(57), 4, menuInterfaceFishingTile.gameObject, 0f, indicator: false, button: false, glow: true, 250, new Vector2(AppInfo.GetRightOffset(), 0f)), stageTrigger: false, null, funcSameAsTargetFunc: true, null, shadow: true, tween: true);
				GameData.instance.PROJECT.CheckTutorialChanges();
				return true;
			}
		}
		if (GameData.instance.PROJECT.character.tutorial.GetState(1) && !GameData.instance.PROJECT.character.tutorial.GetState(2))
		{
			MenuInterfaceQuestTile menuInterfaceQuestTile2 = GetButton(typeof(MenuInterfaceQuestTile)) as MenuInterfaceQuestTile;
			if ((bool)menuInterfaceQuestTile2 && menuInterfaceQuestTile2.available)
			{
				GameData.instance.PROJECT.character.tutorial.SetState(2);
				GameData.instance.tutorialManager.ShowTutorialForButton(menuInterfaceQuestTile2.gameObject, new TutorialPopUpSettings(Tutorial.GetText(2), 1, menuInterfaceQuestTile2.gameObject), stageTrigger: true, null, funcSameAsTargetFunc: false, null, shadow: false, tween: true);
				GameData.instance.PROJECT.CheckTutorialChanges();
				return true;
			}
		}
		TutorialRef tutorialRef = VariableBook.LookUpTutorial("augment_craft");
		if (tutorialRef != null && !GameData.instance.PROJECT.character.tutorial.GetState(69) && tutorialRef.areConditionsMet)
		{
			MenuInterfaceCraftTile menuInterfaceCraftTile2 = GetButton(typeof(MenuInterfaceCraftTile)) as MenuInterfaceCraftTile;
			if (menuInterfaceCraftTile2 != null && menuInterfaceCraftTile2.available)
			{
				GameData.instance.PROJECT.character.tutorial.SetState(69);
				GameData.instance.tutorialManager.ShowTutorialForButton(menuInterfaceCraftTile2.gameObject, new TutorialPopUpSettings(Tutorial.GetText(69), 3, menuInterfaceCraftTile2.gameObject, 0f, indicator: false, button: false, glow: true, 250, new Vector2(AppInfo.GetLeftOffset(), 0f)), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
				return true;
			}
		}
		TutorialRef tutorialRef2 = VariableBook.LookUpTutorial("runes_craft");
		if (tutorialRef2 != null && !GameData.instance.PROJECT.character.tutorial.GetState(86) && tutorialRef2.areConditionsMet)
		{
			MenuInterfaceCraftTile menuInterfaceCraftTile3 = GetButton(typeof(MenuInterfaceCraftTile)) as MenuInterfaceCraftTile;
			if (menuInterfaceCraftTile3 != null && menuInterfaceCraftTile3.available)
			{
				GameData.instance.PROJECT.character.tutorial.SetState(86);
				GameData.instance.tutorialManager.ShowTutorialForButton(menuInterfaceCraftTile3.gameObject, new TutorialPopUpSettings(Tutorial.GetText(86), 3, menuInterfaceCraftTile3.gameObject, 0f, indicator: false, button: false, glow: true, 250, new Vector2(AppInfo.GetLeftOffset(), 0f)), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
				return true;
			}
		}
		TutorialRef tutorialRef3 = VariableBook.LookUpTutorial("craft_reforge");
		if (tutorialRef3 != null)
		{
			List<ItemData> reforgableItems = GameData.instance.PROJECT.character.inventory.getReforgableItems();
			if (!GameData.instance.PROJECT.character.tutorial.GetState(122) && tutorialRef3.areConditionsMet && reforgableItems != null && reforgableItems.Count >= 1)
			{
				MenuInterfaceCraftTile menuInterfaceCraftTile4 = GetButton(typeof(MenuInterfaceCraftTile)) as MenuInterfaceCraftTile;
				if (menuInterfaceCraftTile4 != null && menuInterfaceCraftTile4.available)
				{
					GameData.instance.PROJECT.character.tutorial.SetState(122);
					GameData.instance.tutorialManager.ShowTutorialForButton(menuInterfaceCraftTile4.gameObject, new TutorialPopUpSettings(Tutorial.GetText(122), 3, menuInterfaceCraftTile4.gameObject, 0f, indicator: false, button: false, glow: true, 250, new Vector2(AppInfo.GetLeftOffset(), 0f)), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
					return true;
				}
			}
		}
		return false;
	}

	private void OnCraftDialogClosed(object e)
	{
		(e as DialogPopup).CLEAR.RemoveListener(OnCraftDialogClosed);
		CheckTutorial();
	}

	public void UpdateRestrictions()
	{
		foreach (MainUIButton item in _buttonsLeft)
		{
			item.DoUpdate();
		}
		foreach (MainUIButton item2 in _buttonsRightBottom)
		{
			item2.DoUpdate();
		}
	}

	private long GetNextEventMilliseconds()
	{
		long num = 0L;
		MenuInterfaceEventTokenTile menuInterfaceEventTokenTile = GetButton(typeof(MenuInterfaceEventTokenTile)) as MenuInterfaceEventTokenTile;
		MenuInterfaceEventBadgeTile menuInterfaceEventBadgeTile = GetButton(typeof(MenuInterfaceEventBadgeTile)) as MenuInterfaceEventBadgeTile;
		if (menuInterfaceEventTokenTile != null)
		{
			if (menuInterfaceEventTokenTile.eventRef != null)
			{
				long millisecondsUntilEnd = menuInterfaceEventTokenTile.eventRef.GetDateRef().getMillisecondsUntilEnd();
				if (millisecondsUntilEnd > 0 && (num <= 0 || millisecondsUntilEnd < num))
				{
					num = millisecondsUntilEnd;
				}
			}
			else
			{
				long nextEventMilliseconds = menuInterfaceEventTokenTile.GetNextEventMilliseconds();
				if (nextEventMilliseconds > 0 && (num <= 0 || nextEventMilliseconds < num))
				{
					num = nextEventMilliseconds;
				}
			}
		}
		if (menuInterfaceEventBadgeTile != null)
		{
			if (menuInterfaceEventBadgeTile.eventRef != null)
			{
				long millisecondsUntilEnd2 = menuInterfaceEventBadgeTile.eventRef.GetDateRef().getMillisecondsUntilEnd();
				if (millisecondsUntilEnd2 > 0 && (num <= 0 || millisecondsUntilEnd2 < num))
				{
					num = millisecondsUntilEnd2;
				}
			}
			else
			{
				long nextEventMilliseconds2 = menuInterfaceEventBadgeTile.GetNextEventMilliseconds();
				if (nextEventMilliseconds2 > 0 && (num <= 0 || nextEventMilliseconds2 < num))
				{
					num = nextEventMilliseconds2;
				}
			}
		}
		return num;
	}

	private void UpdateTimer()
	{
		long nextEventMilliseconds = GetNextEventMilliseconds();
		if (nextEventMilliseconds > 0)
		{
			StartTimer(nextEventMilliseconds);
		}
	}

	private void ClearTimer()
	{
		if (_timer != null)
		{
			StopCoroutine(_timer);
			_timer = null;
		}
	}

	private void StartTimer(long milliseconds = 0L)
	{
		ClearTimer();
		if (milliseconds > 0)
		{
			_timer = Timer(milliseconds / 1000);
			StartCoroutine(_timer);
		}
	}

	private IEnumerator Timer(long seconds = 0L)
	{
		yield return new WaitForSeconds(seconds);
		ClearTimer();
		UpdateTiles();
	}

	private void DoEnterTown()
	{
		InstanceRef firstInstanceByType = InstanceBook.GetFirstInstanceByType(1);
		if (firstInstanceByType != null)
		{
			GameDALC.instance.doEnterInstance(firstInstanceByType);
		}
	}

	public void ReassignKongregateTile(Transform parent = null)
	{
		if (!(menuInterfaceKongregateInstance == null))
		{
			if (parent == null)
			{
				parent = base.transform;
			}
			menuInterfaceKongregateInstance.transform.SetParent(parent);
			Vector3 position = menuInterfaceKongregateInstance.transform.position;
			position.z = 0f;
			menuInterfaceKongregateInstance.transform.position = position;
		}
	}

	private void OnDestroy()
	{
		if (GameData.instance.PROJECT.character != null)
		{
			GameData.instance.PROJECT.character.RemoveListener("AD_MILLISECONDS_CHANGE", OnCharacterAdMilliseconds);
			GameData.instance.PROJECT.character.RemoveListener("AD_READY", OnCharacterAdReady);
			GameData.instance.PROJECT.character.RemoveListener("NBP_DATE_CHANGE", OnCharacterNBPChange);
			GameData.instance.PROJECT.character.RemoveListener("INVENTORY_CHANGE", OnCharacterInventoryChange);
		}
	}
}
