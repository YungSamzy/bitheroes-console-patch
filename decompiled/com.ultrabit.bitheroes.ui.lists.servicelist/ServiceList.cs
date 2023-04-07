using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.ui.utility;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui.lists.servicelist;

public class ServiceList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	private UnityAction<int> onClickDelegate;

	public SimpleDataHelper<ServiceListItemModel> Data { get; private set; }

	public void InitList(UnityAction<int> onClickDelegate)
	{
		if (onClickDelegate != null)
		{
			this.onClickDelegate = onClickDelegate;
		}
		if (Data == null)
		{
			Data = new SimpleDataHelper<ServiceListItemModel>(this);
			base.Start();
		}
	}

	public void ClearDelegate()
	{
		onClickDelegate = null;
	}

	public void ClearList()
	{
		if (Data != null && Data.Count > 0)
		{
			Data.RemoveItems(0, Data.Count);
		}
	}

	protected override void Start()
	{
		InitList(null);
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
		ServiceListItemModel model = Data[newOrRecycled.ItemIndex];
		if (newOrRecycled.hoverImages == null)
		{
			newOrRecycled.hoverImages = newOrRecycled.root.gameObject.AddComponent<HoverImages>();
		}
		if (newOrRecycled.itemNameBrightness == null)
		{
			newOrRecycled.itemNameBrightness = newOrRecycled.root.gameObject.AddComponent<TextBrightness>();
		}
		if (newOrRecycled.descriptionBrightness == null)
		{
			newOrRecycled.descriptionBrightness = newOrRecycled.root.gameObject.AddComponent<TextBrightness>();
		}
		if (newOrRecycled.costBrightness == null)
		{
			newOrRecycled.costBrightness = newOrRecycled.root.gameObject.AddComponent<TextBrightness>();
		}
		newOrRecycled.hoverImages.ForceStart();
		newOrRecycled.hoverImages.canGlow = model.canGlow;
		newOrRecycled.itemNameBrightness.ForceStart();
		newOrRecycled.descriptionBrightness.ForceStart();
		newOrRecycled.costBrightness.ForceStart();
		List<TextMeshProUGUI> list = new List<TextMeshProUGUI>();
		list.Add(newOrRecycled.itemName);
		list.Add(newOrRecycled.description);
		list.Add(newOrRecycled.cost);
		newOrRecycled.hoverImages.SetTexts(list);
		newOrRecycled.btn.onClick.RemoveAllListeners();
		newOrRecycled.btn.onClick.AddListener(delegate
		{
			ClickAction(model.id);
		});
		newOrRecycled.itemName.text = model.name;
		newOrRecycled.description.text = model.description;
		newOrRecycled.cost.text = model.cost.ToString();
		newOrRecycled.image.overrideSprite = model.icon;
		if (model.highlighted && model.canGlow && newOrRecycled.hoverImages != null)
		{
			newOrRecycled.hoverImages.StartGlow();
		}
		newOrRecycled.creditsIcon.gameObject.SetActive(model.currencyID == 2);
		newOrRecycled.goldIcon.gameObject.SetActive(model.currencyID == 1);
		void ClickAction(int id)
		{
			GameData.instance.audioManager.PlaySoundLink("buttonclick");
			if (onClickDelegate != null)
			{
				onClickDelegate(id);
			}
		}
	}

	protected override void OnBeforeRecycleOrDisableViewsHolder(MyListItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		inRecycleBinOrVisible.root.GetComponent<HoverImages>().StopGlowing();
		inRecycleBinOrVisible.root.GetComponent<HoverImages>().OnExit();
		Data[inRecycleBinOrVisible.ItemIndex].canGlow = inRecycleBinOrVisible.root.GetComponent<HoverImages>().canGlow;
		base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<ServiceListItemModel> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<ServiceListItemModel> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		ServiceListItemModel[] newItems = new ServiceListItemModel[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		ServiceListItemModel[] newItems = new ServiceListItemModel[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(ServiceListItemModel[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
