using System;
using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.item.action;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.game;
using com.ultrabit.bitheroes.ui.promo;
using com.ultrabit.bitheroes.ui.shop;
using com.ultrabit.bitheroes.ui.utility;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.shoppromolist;

public class ShopPromoList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	private ShopWindow _shopWindow;

	private ShopPromoTile _shopPromoTile;

	private ItemActionBase action;

	private Action OnListScrollEnd;

	private Action OnListScrollStart;

	public int currentItemIndex;

	public SimpleDataHelper<ShopPromoItemModel> Data { get; private set; }

	public void InitList(ShopWindow shopWindow, ShopPromoTile shopPromoTile)
	{
		_shopWindow = shopWindow;
		_shopPromoTile = shopPromoTile;
		if (Data == null)
		{
			Data = new SimpleDataHelper<ShopPromoItemModel>(this);
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
		_shopPromoTile.ClearTimer();
		ScrollByAbstractDelta(base.Parameters.DefaultItemSize * (float)direction);
	}

	public void ScrollChangePage(int index)
	{
		_shopPromoTile.ClearTimer();
		ScrollToPage(index);
	}

	public void ScrollToPage(int index, bool direct = false)
	{
		if (currentItemIndex < _Params.obj[0].transform.childCount)
		{
			_Params.obj[0].transform.GetChild(currentItemIndex).GetComponent<Image>().color = Color.white;
		}
		if (!direct)
		{
			currentItemIndex += index;
		}
		else
		{
			currentItemIndex = Mathf.Clamp(index, 0, _Params.obj[0].transform.childCount);
		}
		if (Data.Count <= currentItemIndex)
		{
			SmoothScrollTo(0, 0.2f, 0.5f, 0.5f, null, OnListScrollEnd, overrideCurrentScrollingAnimation: true);
			currentItemIndex = 0;
		}
		else if (currentItemIndex < 0)
		{
			SmoothScrollTo(Data.Count - 1, 0.2f, 0.5f, 0.5f, null, OnListScrollEnd, overrideCurrentScrollingAnimation: true);
			currentItemIndex = Data.Count - 1;
		}
		else
		{
			SmoothScrollTo(currentItemIndex, 0.2f, 0.5f, 0.5f, null, OnListScrollEnd, overrideCurrentScrollingAnimation: true);
		}
		_Params.obj[0].transform.GetChild(currentItemIndex).GetComponent<Image>().color = Color.green;
	}

	protected override MyListItemViewsHolder CreateViewsHolder(int itemIndex)
	{
		MyListItemViewsHolder myListItemViewsHolder = new MyListItemViewsHolder();
		myListItemViewsHolder.Init(_Params.ItemPrefab, _Params.Content, itemIndex);
		return myListItemViewsHolder;
	}

	public void RefreshOne(MyListItemViewsHolder newOrRecycled)
	{
		UpdateViewsHolder(newOrRecycled);
	}

	protected override void UpdateViewsHolder(MyListItemViewsHolder newOrRecycled)
	{
		ShopPromoItemModel shopPromoItemModel = Data[newOrRecycled.ItemIndex];
		int sortingOrder = _shopWindow.sortingLayer + 1;
		if (newOrRecycled.placeholderPromo.childCount > 0)
		{
			ClearView(newOrRecycled);
		}
		Transform obj = UnityEngine.Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<Transform>("ui/promo/" + typeof(PromoObject).Name));
		obj.SetParent(newOrRecycled.placeholderPromo.transform, worldPositionStays: false);
		PromoObject component = obj.GetComponent<PromoObject>();
		component.LoadDetails(shopPromoItemModel.shopPromoRef, newOrRecycled.placeholderPromo.transform, PromoObject.TYPE.SHOP);
		component.CreateAssets(sortingOrder);
		Util.ChangeChildrenParticleSystemMaskInteraction(obj.gameObject, SpriteMaskInteraction.VisibleInsideMask);
		foreach (GameTextRef text in shopPromoItemModel.shopPromoRef.texts)
		{
			Transform obj2 = UnityEngine.Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<Transform>("ui/game/" + typeof(GameText).Name));
			obj2.SetParent(newOrRecycled.placeholderPromo.transform, worldPositionStays: false);
			obj2.GetComponent<GameText>().LoadDetails(text, (!Util.asianLangManager.isAsian) ? 4 : 0, _shopWindow.debugDate);
			Util.ChangeLayer(obj2, "UI");
			SortingGroup sortingGroup = obj2.gameObject.AddComponent<SortingGroup>();
			if (sortingGroup != null && sortingGroup.enabled)
			{
				sortingGroup.sortingLayerName = "UI";
				sortingGroup.sortingOrder = sortingOrder;
			}
			WindowsMain.CheckAsianFont(obj2.gameObject);
		}
		ItemRef itemRef = shopPromoItemModel.shopPromoRef.itemRef;
		newOrRecycled.root.GetComponent<Button>().enabled = false;
		if (itemRef != null)
		{
			ItemData itemData = new ItemData(itemRef, 1);
			if (itemRef.hasContents())
			{
				action = ItemActionFactory.Create(itemData, 7);
			}
			else
			{
				action = ItemActionFactory.Create(itemData, 4);
			}
			newOrRecycled.root.GetComponent<Button>().enabled = true;
			Navigation navigation = default(Navigation);
			navigation.mode = Navigation.Mode.None;
			newOrRecycled.root.GetComponent<Button>().navigation = navigation;
			newOrRecycled.root.GetComponent<Button>().onClick.AddListener(delegate
			{
				action.Execute();
			});
		}
		bool flag = false;
		for (int i = 0; i < _Params.obj[0].transform.childCount - Data.Count; i++)
		{
			if (!flag)
			{
				ScrollToPage(Mathf.Clamp(currentItemIndex, 0, Mathf.Max(0, Data.Count - 1)), direct: true);
				flag = true;
			}
			UnityEngine.Object.Destroy(_Params.obj[0].transform.GetChild(Data.Count + i).gameObject);
		}
		for (int j = 0; j < Data.Count; j++)
		{
			if (Data.Count == _Params.obj[0].transform.childCount)
			{
				_Params.obj[0].transform.GetChild(j).GetComponent<ShopPromoDotClick>().SetID(j);
			}
			else if (Data.Count > _Params.obj[0].transform.childCount)
			{
				UnityEngine.Object.Instantiate(_Params.obj[1], _Params.obj[0].transform).GetComponent<Image>().color = Color.white;
			}
		}
		_Params.obj[0].transform.GetChild(currentItemIndex).GetComponent<Image>().color = Color.green;
		HoverImages hoverImages = newOrRecycled.root.GetComponent<HoverImages>();
		if (hoverImages == null)
		{
			hoverImages = newOrRecycled.root.gameObject.AddComponent<HoverImages>();
		}
		hoverImages.ForceStart();
		hoverImages.GetOwnTexts();
	}

	private int GetQty(ShopPromoItemModel model)
	{
		if (model.shopPromoRef.itemRef.allowQty && _shopWindow != null)
		{
			return _shopWindow.GetQty();
		}
		return 1;
	}

	protected override void OnBeforeRecycleOrDisableViewsHolder(MyListItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		if (inRecycleBinOrVisible != null && inRecycleBinOrVisible.root != null && inRecycleBinOrVisible.root.GetComponent<HoverImages>() != null)
		{
			inRecycleBinOrVisible.root.GetComponent<HoverImages>().OnExit();
		}
		base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
		ClearView(inRecycleBinOrVisible);
	}

	private void ClearView(MyListItemViewsHolder inRecycleBinOrVisible)
	{
		inRecycleBinOrVisible.root.GetComponent<Button>().onClick.RemoveAllListeners();
		if (inRecycleBinOrVisible.placeholderPromo.childCount > 0)
		{
			for (int i = 0; i < inRecycleBinOrVisible.placeholderPromo.childCount; i++)
			{
				UnityEngine.Object.Destroy(inRecycleBinOrVisible.placeholderPromo.GetChild(i).gameObject);
			}
		}
	}

	public void AddItemsAt(int index, IList<ShopPromoItemModel> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<ShopPromoItemModel> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		ShopPromoItemModel[] newItems = new ShopPromoItemModel[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		ShopPromoItemModel[] newItems = new ShopPromoItemModel[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(ShopPromoItemModel[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
