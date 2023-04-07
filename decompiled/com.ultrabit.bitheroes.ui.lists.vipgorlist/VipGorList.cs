using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.adgor;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.payment;
using com.ultrabit.bitheroes.model.shop;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.utility;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.vipgorlist;

public class VipGorList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	public SimpleDataHelper<VipgorItem> Data { get; private set; }

	public void InitList()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<VipgorItem>(this);
			base.Start();
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
		VipgorItem model = Data[newOrRecycled.ItemIndex];
		newOrRecycled.ribbonTxt.text = Language.GetString("ui_best").ToUpperInvariant();
		newOrRecycled.nameTxt.text = model.itemRef.coloredName;
		HoverImages hoverImages = newOrRecycled.root.GetComponent<HoverImages>();
		if (hoverImages == null)
		{
			hoverImages = newOrRecycled.root.gameObject.AddComponent<HoverImages>();
		}
		hoverImages.ForceStart();
		hoverImages.GetOwnTexts();
		int num = 1;
		ShopSaleRef shopSaleRef = ShopBook.LookupVipgor(model.itemRef.id);
		PaymentData paymentData = model.itemRef.getPaymentData();
		if (AdGor.devMode && GameData.instance.PROJECT.character.admin)
		{
			paymentData = null;
		}
		bool flag = shopSaleRef != null && paymentData == null && (double)shopSaleRef.mult != 1.0;
		string text = "";
		newOrRecycled.ribbon.gameObject.SetActive(flag);
		int num2 = ((paymentData == null) ? ((model.itemRef.costCreditsRaw <= 0) ? 1 : 2) : 0);
		int num3 = ((paymentData == null) ? ((num2 == 2) ? model.itemRef.costCredits : model.itemRef.costGold) : 0);
		if (paymentData == null)
		{
			if (num2 == 2)
			{
				_ = model.itemRef.costCreditsRaw;
			}
			else
				_ = model.itemRef.costGoldRaw;
		}
		else
			_ = 0;
		num3 *= num;
		newOrRecycled.goldIcon.gameObject.SetActive(num2 == 1);
		newOrRecycled.creditsIcon.gameObject.SetActive(num2 == 2);
		if (paymentData != null)
		{
			text = paymentData.price;
		}
		else
		{
			text = Util.NumberFormat(num3);
			if (flag)
			{
				text = Util.ParseString("^" + text + "^");
			}
		}
		newOrRecycled.costTxt.text = text;
		newOrRecycled.ribbonTxt.gameObject.SetActive(flag);
		if (model.itemRef.box != "")
		{
			newOrRecycled.ribbon.gameObject.SetActive(value: true);
			newOrRecycled.ribbonTxt.gameObject.SetActive(value: true);
		}
		ItemIcon itemIcon = newOrRecycled.itemIcon.GetComponent<ItemIcon>();
		if (itemIcon == null)
		{
			itemIcon = newOrRecycled.itemIcon.gameObject.AddComponent<ItemIcon>();
		}
		itemIcon.SetItemData(new ItemData(model.itemRef, num));
		itemIcon.SetItemActionType(0);
		itemIcon.SetClickable(clickable: false);
		itemIcon.onClick.RemoveAllListeners();
		itemIcon.onClick.AddListener(delegate
		{
			GameData.instance.windowGenerator.ShowCustomPayment(model.itemRef.getPaymentRef());
		});
		newOrRecycled.root.GetComponent<Button>().onClick.RemoveAllListeners();
		newOrRecycled.root.GetComponent<Button>().onClick.AddListener(delegate
		{
			GameData.instance.windowGenerator.ShowCustomPayment(model.itemRef.getPaymentRef());
		});
	}

	protected override void OnBeforeRecycleOrDisableViewsHolder(MyListItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		inRecycleBinOrVisible.root.GetComponent<HoverImages>().OnExit();
		base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<VipgorItem> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<VipgorItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		VipgorItem[] newItems = new VipgorItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		VipgorItem[] newItems = new VipgorItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(VipgorItem[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
