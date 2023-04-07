using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.lists.craftlist;
using com.ultrabit.bitheroes.ui.menu;
using com.ultrabit.bitheroes.ui.tutorial;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.craft;

public class CraftWindow : WindowsMain
{
	public const int TAB_EXCHANGE = 0;

	public const int TAB_TRADE = 1;

	public const int TAB_UPGRADE = 2;

	public const int TAB_REFORGE = 3;

	public TextMeshProUGUI topperTxt;

	public Button lockBtn;

	public Button exchangeBtn;

	public Button tradeBtn;

	public Button upgradeBtn;

	public Button reforgeBtn;

	public Image tradeNotification;

	public CraftExchangePanel craftExchangePanel;

	public CraftTradePanel craftTradePanel;

	public CraftUpgradePanel craftUpgradePanel;

	public CraftReforgePanel craftReforgePanel;

	public MenuInterfaceCraftTile craftTile;

	private Color alpha = new Color(1f, 1f, 1f, 0.5f);

	private int _currentTab = -1;

	public int currentTab => _currentTab;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = Language.GetString("ui_smelter");
		exchangeBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_exchange");
		tradeBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_craft");
		upgradeBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_upgrade");
		reforgeBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_reforge");
		craftExchangePanel.LoadDetails(this);
		craftTradePanel.LoadDetails(this);
		craftUpgradePanel.LoadDetails(this);
		craftReforgePanel.LoadDetails(this);
		GameData.instance.PROJECT.character.equipment.OnChange.AddListener(OnEquipmentChange);
		GameData.instance.PROJECT.character.AddListener("INVENTORY_CHANGE", OnInventoryChange);
		GameData.instance.PROJECT.character.AddListener("LOCKED_ITEMS_CHANGE", OnLockedItemsChange);
		GameData.instance.PROJECT.character.AddListener("armoryEquipmentChange", OnArmoryChange);
		SetTab(2);
		SCROLL_IN_COMPLETE.AddListener(OnScrollInComplete);
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void OnScrollInComplete(object e)
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		CheckTutorial();
	}

	public void CheckTutorial()
	{
		if (GameData.instance.tutorialManager == null || GameData.instance.tutorialManager.hasPopup || GameData.instance.tutorialManager.canvas == null)
		{
			return;
		}
		if (_currentTab == 2)
		{
			craftUpgradePanel.CheckTutorial();
		}
		if (GameData.instance.tutorialManager.hasPopup)
		{
			return;
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(27))
		{
			if (GameData.instance.PROJECT.character.zoneCompleted > 3)
			{
				int[] array = new int[6] { 100, 24, 27, 115, 116, 117 };
				foreach (int id in array)
				{
					GameData.instance.PROJECT.character.tutorial.SetState(id);
					GameData.instance.PROJECT.CheckTutorialChanges();
				}
			}
			else
			{
				GameData.instance.PROJECT.character.tutorial.SetState(27);
				GameData.instance.tutorialManager.ShowTutorialForButton(exchangeBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(27), 3, exchangeBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, delegate
				{
					CheckTutorial();
				}, shadow: true, tween: true);
			}
			return;
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(115) && GameData.instance.PROJECT.character.tutorial.GetState(27) && _currentTab == 0)
		{
			if (GameData.instance.PROJECT.character.zoneCompleted > 3)
			{
				int[] array = new int[6] { 100, 24, 27, 115, 116, 117 };
				foreach (int id2 in array)
				{
					GameData.instance.PROJECT.character.tutorial.SetState(id2);
					GameData.instance.PROJECT.CheckTutorialChanges();
				}
				return;
			}
			List<ItemIcon> lowestRarityItems = ItemIcon.GetLowestRarityItems(GetVisibleTiles());
			ItemIcon itemIcon = ((lowestRarityItems != null && lowestRarityItems.Count > 0) ? lowestRarityItems[Random.Range(0, lowestRarityItems.Count)] : null);
			if (itemIcon != null)
			{
				GameData.instance.PROJECT.character.tutorial.SetState(115);
				GameData.instance.tutorialManager.ShowTutorialForButton(itemIcon.gameObject, new TutorialPopUpSettings(Tutorial.GetText(115), 4, itemIcon.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
				return;
			}
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(117) && ((AppInfo.IsMobile() && GameData.instance.PROJECT.character.tutorial.GetState(116)) || (!AppInfo.IsMobile() && GameData.instance.PROJECT.character.tutorial.GetState(115))) && _currentTab == 0)
		{
			if (GameData.instance.PROJECT.character.zoneCompleted > 3)
			{
				int[] array = new int[6] { 100, 24, 27, 115, 116, 117 };
				foreach (int id3 in array)
				{
					GameData.instance.PROJECT.character.tutorial.SetState(id3);
					GameData.instance.PROJECT.CheckTutorialChanges();
				}
			}
			else
			{
				if (!GameData.instance.PROJECT.character.tutorial.GetState(116))
				{
					GameData.instance.PROJECT.character.tutorial.SetState(116);
				}
				GameData.instance.PROJECT.character.tutorial.SetState(117);
				GameData.instance.tutorialManager.ShowTutorialForButton(craftExchangePanel.exchangeBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(117), 4, craftExchangePanel.exchangeBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
				GameData.instance.PROJECT.CheckTutorialChanges();
			}
		}
		else if (GameData.instance.PROJECT.character.tutorial.GetState(86) && !GameData.instance.PROJECT.character.tutorial.GetState(87))
		{
			SetTab(1);
			craftTradePanel.CheckTutorialRune();
		}
		else if (GameData.instance.PROJECT.character.tutorial.GetState(69) && !GameData.instance.PROJECT.character.tutorial.GetState(70))
		{
			SetTab(1);
			craftTradePanel.CheckTutorialAugment();
		}
		else if (GameData.instance.PROJECT.character.tutorial.GetState(122) && !GameData.instance.PROJECT.character.tutorial.GetState(123))
		{
			GameData.instance.PROJECT.character.tutorial.SetState(123);
			GameData.instance.tutorialManager.ShowTutorialForButton(reforgeBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(123), 3, reforgeBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, delegate
			{
				CheckTutorial();
			}, shadow: true, tween: true);
		}
		else if (GameData.instance.PROJECT.character.tutorial.GetState(123) && !GameData.instance.PROJECT.character.tutorial.GetState(124))
		{
			craftReforgePanel.CheckTutorial();
		}
	}

	private List<ItemIcon> GetVisibleTiles()
	{
		List<ItemIcon> list = new List<ItemIcon>();
		for (int i = 0; i < craftExchangePanel.itemList.Data.Count; i++)
		{
			MyGridItemViewsHolder cellViewsHolderIfVisible = craftExchangePanel.itemList.GetCellViewsHolderIfVisible(i);
			if (cellViewsHolderIfVisible != null && cellViewsHolderIfVisible.root.GetComponent<ItemIcon>() != null)
			{
				list.Add(cellViewsHolderIfVisible.root.GetComponent<ItemIcon>());
			}
		}
		return list;
	}

	public void OnExchangeBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SetTab(0);
	}

	public void OnTradeBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SetTab(1);
	}

	public void OnUpgradeBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SetTab(2);
	}

	public void OnReforgeBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SetTab(3);
	}

	public void OnLockBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoLockSelect();
	}

	private void SetTab(int tab)
	{
		switch (tab)
		{
		case 0:
			_currentTab = 0;
			AlphaTabs();
			exchangeBtn.image.color = Color.white;
			exchangeBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
			exchangeBtn.enabled = false;
			craftExchangePanel.Show();
			craftTradePanel.Hide();
			craftUpgradePanel.Hide();
			craftReforgePanel.Hide();
			if (craftExchangePanel.updatePending)
			{
				craftExchangePanel.updatePending = false;
				craftExchangePanel.DoUpdate();
			}
			break;
		case 1:
			_currentTab = 1;
			AlphaTabs();
			tradeBtn.image.color = Color.white;
			tradeBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
			tradeBtn.enabled = false;
			craftExchangePanel.Hide();
			craftTradePanel.Show();
			craftUpgradePanel.Hide();
			craftReforgePanel.Hide();
			if (craftTradePanel.updatePending)
			{
				craftTradePanel.updatePending = false;
				craftTradePanel.DoUpdate();
			}
			break;
		case 2:
			_currentTab = 2;
			AlphaTabs();
			upgradeBtn.image.color = Color.white;
			upgradeBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
			upgradeBtn.enabled = false;
			craftUpgradePanel.Show();
			craftExchangePanel.Hide();
			craftTradePanel.Hide();
			craftReforgePanel.Hide();
			if (craftUpgradePanel.updatePending)
			{
				craftUpgradePanel.updatePending = false;
				craftUpgradePanel.DoUpdate();
			}
			break;
		case 3:
			_currentTab = 3;
			AlphaTabs();
			reforgeBtn.image.color = Color.white;
			reforgeBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
			reforgeBtn.enabled = false;
			craftUpgradePanel.Hide();
			craftExchangePanel.Hide();
			craftTradePanel.Hide();
			craftReforgePanel.Show();
			if (craftReforgePanel.updatePending)
			{
				craftReforgePanel.updatePending = false;
				craftReforgePanel.DoUpdate();
			}
			break;
		}
		UpdateTradeNotifications();
	}

	private void UpdateTradeNotifications()
	{
		int num = CraftTradePanel.availableCraftRecipes.Count - GameData.instance.SAVE_STATE.GetSeenRecipes(GameData.instance.PROJECT.character.id).Count;
		bool active = num > 0 && GameData.instance.SAVE_STATE.notificationsCraft && !GameData.instance.SAVE_STATE.notificationsDisabled;
		tradeNotification.gameObject.SetActive(active);
		tradeNotification.GetComponentInChildren<TextMeshProUGUI>().text = num.ToString();
	}

	private void AlphaTabs()
	{
		exchangeBtn.image.color = alpha;
		exchangeBtn.GetComponentInChildren<TextMeshProUGUI>().color = alpha;
		exchangeBtn.enabled = true;
		tradeBtn.image.color = alpha;
		tradeBtn.GetComponentInChildren<TextMeshProUGUI>().color = alpha;
		tradeBtn.enabled = true;
		upgradeBtn.image.color = alpha;
		upgradeBtn.GetComponentInChildren<TextMeshProUGUI>().color = alpha;
		upgradeBtn.enabled = true;
		reforgeBtn.image.color = alpha;
		reforgeBtn.GetComponentInChildren<TextMeshProUGUI>().color = alpha;
		reforgeBtn.enabled = true;
	}

	private void OnEquipmentChange()
	{
		DoUpdate();
	}

	private void OnArmoryChange()
	{
		DoUpdate();
	}

	private void OnInventoryChange()
	{
		DoUpdate();
	}

	private void OnLockedItemsChange()
	{
		DoUpdate();
	}

	private void DoUpdate()
	{
		craftExchangePanel.DoUpdate();
		craftTradePanel.DoUpdate();
		craftUpgradePanel.DoUpdate();
		craftReforgePanel.DoUpdate();
	}

	private void DoLockSelect()
	{
		List<ItemData> items = GameData.instance.PROJECT.character.inventory.items;
		List<ItemData> list = new List<ItemData>();
		foreach (ItemData item in items)
		{
			if (item.qty > 0 && item.itemRef.exchangeable)
			{
				list.Add(item);
			}
		}
		GameData.instance.windowGenerator.NewItemSearchWindow(list, adminWindow: false, null, showQty: true, closeOnSelect: false, GameData.instance.PROJECT.character.lockedItems, showLock: true, tooltipSuggested: false, base.gameObject).DESTROYED.AddListener(OnLockSelect);
	}

	private void OnLockSelect(object e)
	{
		ItemSearchWindow obj = e as ItemSearchWindow;
		obj.DESTROYED.RemoveListener(OnLockSelect);
		List<ItemRef> selectedItems = obj.GetSelectedItems();
		if (obj.changed)
		{
			GameData.instance.PROJECT.character.lockedItems = selectedItems;
			CharacterDALC.instance.doSaveConfig(GameData.instance.PROJECT.character);
		}
	}

	public override void OnClose()
	{
		GameData.instance.SAVE_STATE.SetSeenRecipes(GameData.instance.PROJECT.character.id, craftTradePanel.tradeList.recipesSeen);
		UpdateCraftNotificationTile();
		base.OnClose();
	}

	public void UpdateCraftNotificationTile()
	{
		if (craftTile != null)
		{
			craftTile.UpdateText();
		}
	}

	public override void DoDestroy()
	{
		GameData.instance.PROJECT.character.equipment.OnChange.RemoveListener(OnEquipmentChange);
		GameData.instance.PROJECT.character.RemoveListener("INVENTORY_CHANGE", OnInventoryChange);
		GameData.instance.PROJECT.character.RemoveListener("LOCKED_ITEMS_CHANGE", OnLockedItemsChange);
		GameData.instance.PROJECT.character.RemoveListener("armoryEquipmentChange", OnArmoryChange);
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		lockBtn.interactable = true;
		exchangeBtn.interactable = true;
		tradeBtn.interactable = true;
		upgradeBtn.interactable = true;
		reforgeBtn.interactable = true;
		craftExchangePanel.DoEnable();
		craftTradePanel.DoEnable();
		craftUpgradePanel.DoEnable();
		craftReforgePanel.DoEnable();
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		lockBtn.interactable = false;
		exchangeBtn.interactable = false;
		tradeBtn.interactable = false;
		upgradeBtn.interactable = false;
		reforgeBtn.interactable = false;
		craftExchangePanel.DoDisable();
		craftTradePanel.DoDisable();
		craftUpgradePanel.DoDisable();
		craftReforgePanel.DoDisable();
	}

	private void SetButtonsState(bool state)
	{
		if (lockBtn != null && lockBtn.gameObject != null)
		{
			lockBtn.interactable = state;
		}
		if (upgradeBtn != null && upgradeBtn.gameObject != null)
		{
			upgradeBtn.interactable = state;
		}
		if (exchangeBtn != null && exchangeBtn.gameObject != null)
		{
			exchangeBtn.interactable = state;
		}
		if (tradeBtn != null && tradeBtn.gameObject != null)
		{
			tradeBtn.interactable = state;
		}
		if (reforgeBtn != null && reforgeBtn.gameObject != null)
		{
			reforgeBtn.interactable = state;
		}
		if (craftUpgradePanel != null && craftUpgradePanel.gameObject != null)
		{
			if (state)
			{
				craftUpgradePanel.DoEnable();
			}
			else
			{
				craftUpgradePanel.DoDisable();
			}
		}
		if (craftExchangePanel != null && craftExchangePanel.gameObject != null)
		{
			if (state)
			{
				craftExchangePanel.DoEnable();
			}
			else
			{
				craftExchangePanel.DoDisable();
			}
		}
		if (craftTradePanel != null && craftTradePanel.gameObject != null)
		{
			if (state)
			{
				craftTradePanel.DoEnable();
			}
			else
			{
				craftTradePanel.DoDisable();
			}
		}
		if (craftReforgePanel != null && craftReforgePanel.gameObject != null)
		{
			if (state)
			{
				craftReforgePanel.DoEnable();
			}
			else
			{
				craftReforgePanel.DoDisable();
			}
		}
	}
}
