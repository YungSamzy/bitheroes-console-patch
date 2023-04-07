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

namespace com.ultrabit.bitheroes.ui.lists.herolist;

public class HeroList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	private UnityAction<int> onClickDelegate;

	public SimpleDataHelper<HeroListItemModel> Data { get; private set; }

	public void InitList(UnityAction<int> onClickDelegate)
	{
		if (onClickDelegate != null)
		{
			this.onClickDelegate = onClickDelegate;
		}
		if (Data == null)
		{
			Data = new SimpleDataHelper<HeroListItemModel>(this);
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
		HeroListItemModel model = Data[newOrRecycled.ItemIndex];
		newOrRecycled.root.GetComponent<HoverImages>().ForceStart();
		newOrRecycled.root.GetComponent<HoverImages>().canGlow = model.canGlow;
		if (newOrRecycled.itemName.gameObject.GetComponent<TextBrightness>() == null)
		{
			newOrRecycled.itemName.gameObject.AddComponent<TextBrightness>();
		}
		if (newOrRecycled.description.gameObject.GetComponent<TextBrightness>() == null)
		{
			newOrRecycled.description.gameObject.AddComponent<TextBrightness>();
		}
		if (newOrRecycled.cost.gameObject.GetComponent<TextBrightness>() == null)
		{
			newOrRecycled.cost.gameObject.AddComponent<TextBrightness>();
		}
		newOrRecycled.itemName.gameObject.GetComponent<TextBrightness>().ForceStart();
		newOrRecycled.description.gameObject.GetComponent<TextBrightness>().ForceStart();
		newOrRecycled.cost.gameObject.GetComponent<TextBrightness>().ForceStart();
		List<TextMeshProUGUI> list = new List<TextMeshProUGUI>();
		list.Add(newOrRecycled.itemName);
		list.Add(newOrRecycled.description);
		list.Add(newOrRecycled.cost);
		newOrRecycled.root.GetComponent<HoverImages>().SetTexts(list);
		newOrRecycled.btn.onClick.RemoveAllListeners();
		newOrRecycled.btn.onClick.AddListener(delegate
		{
			ClickAction(model.id);
		});
		newOrRecycled.itemName.text = model.name;
		newOrRecycled.description.text = model.description;
		newOrRecycled.cost.text = model.cost.ToString();
		newOrRecycled.image.overrideSprite = model.icon;
		if (model.highlighted && model.canGlow)
		{
			newOrRecycled.root.GetComponent<HoverImages>().StartGlow();
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

	public void AddItemsAt(int index, IList<HeroListItemModel> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<HeroListItemModel> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		HeroListItemModel[] newItems = new HeroListItemModel[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		HeroListItemModel[] newItems = new HeroListItemModel[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(HeroListItemModel[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
