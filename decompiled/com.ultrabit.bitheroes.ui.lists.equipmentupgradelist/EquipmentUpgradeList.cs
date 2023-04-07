using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.item;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui.lists.equipmentupgradelist;

public class EquipmentUpgradeList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	public Transform itemCraftTilePrefab;

	private EquipmentRef upgradedEquipmentRef;

	[HideInInspector]
	public UnityEvent UPGRADE = new UnityEvent();

	public SimpleDataHelper<UpgradeItemModel> Data { get; private set; }

	public void StartList()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<UpgradeItemModel>(this);
			base.Start();
		}
	}

	public void ClearList()
	{
		if (Data != null && Data.Count > 0)
		{
			Data.RemoveItems(0, Data.Count);
		}
	}

	private void OnScroll(int direction)
	{
		ScrollByAbstractDelta(base.Parameters.DefaultItemSize * (float)direction);
	}

	protected override MyListItemViewsHolder CreateViewsHolder(int itemIndex)
	{
		MyListItemViewsHolder myListItemViewsHolder = new MyListItemViewsHolder();
		myListItemViewsHolder.Init(_Params.ItemPrefab, _Params.Content, itemIndex);
		return myListItemViewsHolder;
	}

	protected override void UpdateViewsHolder(MyListItemViewsHolder newOrRecycled)
	{
		UpgradeItemModel model = Data[newOrRecycled.ItemIndex];
		newOrRecycled.craftBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_upgrade");
		newOrRecycled.craftBtn.onClick.AddListener(delegate
		{
			DoItemUpgrade(model);
		});
		ItemIcon itemIcon = newOrRecycled.result.gameObject.GetComponent<ItemIcon>();
		if (itemIcon == null)
		{
			itemIcon = newOrRecycled.result.gameObject.AddComponent<ItemIcon>();
		}
		itemIcon.SetItemData(new ItemData(model.upgradeRef.getUpgradeItemRef(), -1));
		itemIcon.SetItemActionType(0);
		itemIcon.SetCompare(compare: true);
		itemIcon.PlayComparison("+");
		foreach (ItemData requiredItem in model.upgradeRef.getUpgradeRef().requiredItems)
		{
			if (requiredItem != null)
			{
				Transform obj = Object.Instantiate(itemCraftTilePrefab);
				obj.SetParent(newOrRecycled.requiredPlaceholder, worldPositionStays: false);
				obj.GetComponent<ItemCraftTile>().LoadDetails(requiredItem);
			}
		}
		bool flag = model.upgradeRef.getUpgradeRef().RequirementsMet();
		newOrRecycled.backgroundImage.color = (flag ? Color.white : new Color(0.7f, 0.7f, 0.7f));
		if (flag)
		{
			Util.SetButton(newOrRecycled.craftBtn);
		}
		else
		{
			Util.SetButton(newOrRecycled.craftBtn, enabled: false);
		}
		EquipmentRef equipmentRef = model.upgradeRef.getUpgradeItemRef() as EquipmentRef;
		EquipmentRef equipmentRef2 = model.equipmentRef;
		ChangeText(newOrRecycled.powerTxt, equipmentRef2.power, equipmentRef.power - equipmentRef2.power);
		ChangeText(newOrRecycled.staminaTxt, equipmentRef2.stamina, equipmentRef.stamina - equipmentRef2.stamina);
		ChangeText(newOrRecycled.agilityTxt, equipmentRef2.agility, equipmentRef.agility - equipmentRef2.agility);
	}

	public void ChangeText(TextMeshProUGUI txtChange, int stat, int change)
	{
		Color colorFromHex = Util.GetColorFromHex(Util.GetNumberColor(stat, stat - change, stat - change));
		txtChange.color = colorFromHex;
		txtChange.text = ((change >= 0) ? ("+" + Util.NumberFormat(change)) : Util.NumberFormat(change));
	}

	private void DoItemUpgrade(UpgradeItemModel model)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_upgrade_confirm", new string[1] { model.equipmentRef.coloredName }), null, null, delegate
		{
			OnConfirm(model);
		});
	}

	private void OnConfirm(UpgradeItemModel model)
	{
		GameData.instance.main.ShowLoading();
		MerchantDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(MerchantDALC.ITEM_UPGRADE), OnItemUpgrade);
		MerchantDALC.instance.doItemUpgrade(model.equipmentRef.id, model.equipmentRef.itemType, model.upgradeRef.id);
		upgradedEquipmentRef = model.equipmentRef;
	}

	private void OnItemUpgrade(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		MerchantDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(MerchantDALC.ITEM_UPGRADE), OnItemUpgrade);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		Equipment equipment = Equipment.fromSFSObject(sfsob);
		if (equipment != null)
		{
			GameData.instance.PROJECT.character.equipment.setEquipmentSlots(equipment.equipmentSlots);
		}
		List<ItemData> list = ItemData.listFromSFSObject(sfsob.GetSFSObject("ite4"));
		List<ItemData> list2 = ItemData.listFromSFSObject(sfsob.GetSFSObject("ite5"));
		GameData.instance.PROJECT.character.addItems(list);
		GameData.instance.PROJECT.character.removeItems(list2);
		GameData.instance.PROJECT.character.armory.updateArmorySlot(list, upgradedEquipmentRef);
		KongregateAnalytics.checkEconomyTransaction("Craft Upgrade", list2, list, sfsob, "Craft", 1);
		GameData.instance.windowGenerator.ShowItems(list, compare: true, added: true).SCROLL_OUT_START.AddListener(OnItemWindowClosed);
		GameData.instance.audioManager.PlaySoundLink("upgradeitem");
	}

	private void OnItemWindowClosed(object e)
	{
		ItemListWindow itemListWindow = e as ItemListWindow;
		if (itemListWindow != null)
		{
			itemListWindow.GetComponent<ItemListWindow>().SCROLL_OUT_START.RemoveListener(OnItemWindowClosed);
		}
		UPGRADE.Invoke();
	}

	protected override void OnBeforeRecycleOrDisableViewsHolder(MyListItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		for (int i = 0; i < inRecycleBinOrVisible.requiredPlaceholder.childCount; i++)
		{
			Object.Destroy(inRecycleBinOrVisible.requiredPlaceholder.GetChild(i).gameObject);
		}
		inRecycleBinOrVisible.craftBtn.onClick.RemoveAllListeners();
		base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<UpgradeItemModel> items)
	{
		Data.List.InsertRange(index, items);
		Data.NotifyListChangedExternally();
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.List.RemoveRange(index, count);
		Data.NotifyListChangedExternally();
	}

	public void SetItems(IList<UpgradeItemModel> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		UpgradeItemModel[] newItems = new UpgradeItemModel[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		UpgradeItemModel[] newItems = new UpgradeItemModel[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(UpgradeItemModel[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
