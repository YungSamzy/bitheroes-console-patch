using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.shop;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.lists.shoppromolist;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.shop;

public class ShopPromoTile : MonoBehaviour
{
	public Button leftBtn;

	public Button rightBtn;

	public ShopPromoList _shopPromoList;

	private Coroutine _timer;

	private ShopWindow _shopWindow;

	public SpriteMask _spriteMask;

	public void LoadDetails(ShopWindow shopWindow)
	{
		_shopWindow = shopWindow;
		CreateObjects();
		CreateTimer();
		if (_shopPromoList.Data != null && _shopPromoList.Data.Count > 1)
		{
			leftBtn.gameObject.SetActive(value: true);
			rightBtn.gameObject.SetActive(value: true);
		}
		else
		{
			leftBtn.gameObject.SetActive(value: false);
			rightBtn.gameObject.SetActive(value: false);
		}
	}

	public void RefreshMask()
	{
		_spriteMask.frontSortingLayerID = SortingLayer.NameToID("UI");
		_spriteMask.frontSortingOrder = _shopWindow.sortingLayer + 99;
		_spriteMask.backSortingLayerID = SortingLayer.NameToID("UI");
		_spriteMask.backSortingOrder = _shopWindow.sortingLayer - 1;
	}

	public void CreateObjects()
	{
		_shopPromoList.InitList(_shopWindow, this);
		_shopPromoList.ClearList();
		List<ShopPromoItemModel> list = new List<ShopPromoItemModel>();
		for (int i = 0; i < ShopBook.promosSize; i++)
		{
			ShopPromoRef shopPromoRef = ShopBook.LookupPromo(i);
			if (shopPromoRef.getActive(_shopWindow.debugDate) || AppInfo.TESTING)
			{
				list.Add(new ShopPromoItemModel
				{
					shopPromoRef = shopPromoRef
				});
			}
		}
		_shopPromoList.Data.InsertItemsAtStart(list);
	}

	public void ClearTimer()
	{
		GameData.instance.main.coroutineTimer.StopTimer(ref _timer);
	}

	private void CreateTimer()
	{
		if (_shopPromoList.Data == null || _shopPromoList.Data.Count > 1)
		{
			ClearTimer();
			_timer = GameData.instance.main.coroutineTimer.AddTimer(base.gameObject, VariableBook.shopPromoScrollDelay, 0, null, OnTimer);
		}
	}

	private void OnTimer()
	{
		if (base.gameObject.activeSelf)
		{
			_shopPromoList.ScrollToPage(1);
		}
	}

	public void DoUpdate()
	{
		if (_shopPromoList.Data != null)
		{
			_shopPromoList.Refresh();
		}
	}

	public void OnEnable()
	{
		if (_timer != null)
		{
			CreateTimer();
		}
	}

	public void OnDisable()
	{
		ClearTimer();
	}
}
