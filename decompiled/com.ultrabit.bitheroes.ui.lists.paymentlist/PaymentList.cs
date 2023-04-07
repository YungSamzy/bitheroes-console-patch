using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.payment;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.utility;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui.lists.paymentlist;

public class PaymentList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	private UnityAction<PaymentData> onClickDelegate;

	public SimpleDataHelper<PaymentItem> Data { get; private set; }

	protected override void Start()
	{
		InitList(null);
	}

	public void InitList(UnityAction<PaymentData> onClickDelegate)
	{
		if (onClickDelegate != null)
		{
			this.onClickDelegate = onClickDelegate;
		}
		if (Data == null)
		{
			Data = new SimpleDataHelper<PaymentItem>(this);
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
		PaymentItem model = Data[newOrRecycled.ItemIndex];
		newOrRecycled.root.GetComponent<HoverImages>().ForceStart();
		if (newOrRecycled.nameTxt.gameObject.GetComponent<TextBrightness>() == null)
		{
			newOrRecycled.nameTxt.gameObject.AddComponent<TextBrightness>();
		}
		if (newOrRecycled.descTxt.gameObject.GetComponent<TextBrightness>() == null)
		{
			newOrRecycled.descTxt.gameObject.AddComponent<TextBrightness>();
		}
		if (newOrRecycled.costTxt.gameObject.GetComponent<TextBrightness>() == null)
		{
			newOrRecycled.costTxt.gameObject.AddComponent<TextBrightness>();
		}
		newOrRecycled.nameTxt.gameObject.GetComponent<TextBrightness>().ForceStart();
		newOrRecycled.descTxt.gameObject.GetComponent<TextBrightness>().ForceStart();
		newOrRecycled.costTxt.gameObject.GetComponent<TextBrightness>().ForceStart();
		newOrRecycled.root.GetComponent<HoverImages>().GetOwnTexts();
		if (model.paymentData.paymentRef.type == 5)
		{
			newOrRecycled.nameTxt.text = Language.GetString("ui_offerwall_select_title");
		}
		else
		{
			newOrRecycled.nameTxt.text = model.paymentData.paymentRef.name;
		}
		newOrRecycled.descTxt.text = Util.ParseString(model.paymentData.paymentRef.desc);
		newOrRecycled.costTxt.text = model.paymentData.localizedPrice;
		newOrRecycled.asset.overrideSprite = model.paymentData.paymentRef.GetSpriteIcon();
		newOrRecycled.btn.onClick.AddListener(delegate
		{
			GameData.instance.audioManager.PlaySoundLink("buttonclick");
			if (onClickDelegate != null)
			{
				onClickDelegate(model.paymentData);
			}
		});
	}

	protected override void OnBeforeRecycleOrDisableViewsHolder(MyListItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		inRecycleBinOrVisible.root.GetComponent<HoverImages>().OnExit();
		inRecycleBinOrVisible.btn.onClick.RemoveAllListeners();
		base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<PaymentItem> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<PaymentItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		PaymentItem[] newItems = new PaymentItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		PaymentItem[] newItems = new PaymentItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(PaymentItem[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
