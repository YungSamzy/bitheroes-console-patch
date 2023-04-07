using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.brawl;
using com.ultrabit.bitheroes.ui.promo;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.brawllist;

public class BrawlCreateList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	private BrawlCreateWindow _brawlCreateWindow;

	public int currentItemIndex;

	public SimpleDataHelper<BrawlListModel> Data { get; private set; }

	public void InitList(BrawlCreateWindow brawlCreateWindow)
	{
		_brawlCreateWindow = brawlCreateWindow;
		if (Data == null)
		{
			Data = new SimpleDataHelper<BrawlListModel>(this);
			base.Start();
		}
	}

	private void OnScroll(int direction)
	{
		ScrollByAbstractDelta(base.Parameters.DefaultItemSize * (float)direction);
	}

	public void ScrollChangePage(int index)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		ScrollToPage(index);
	}

	public void ScrollToPage(int index, bool directo = false)
	{
		_Params.obj[0].transform.GetChild(currentItemIndex).GetComponent<Image>().color = Color.white;
		if (!directo)
		{
			currentItemIndex += index;
		}
		else
		{
			currentItemIndex = index;
		}
		if (Data.Count <= currentItemIndex)
		{
			SmoothScrollTo(0, 0.2f, 0.5f, 0.5f, null, null, overrideCurrentScrollingAnimation: true);
			currentItemIndex = 0;
		}
		else if (currentItemIndex < 0)
		{
			SmoothScrollTo(Data.Count - 1, 0.2f, 0.5f, 0.5f, null, null, overrideCurrentScrollingAnimation: true);
			currentItemIndex = Data.Count - 1;
		}
		else
		{
			SmoothScrollTo(currentItemIndex, 0.2f, 0.5f, 0.5f, null, null, overrideCurrentScrollingAnimation: true);
		}
		_Params.obj[0].transform.GetChild(currentItemIndex).GetComponent<Image>().color = Color.green;
	}

	protected override MyListItemViewsHolder CreateViewsHolder(int itemIndex)
	{
		MyListItemViewsHolder myListItemViewsHolder = new MyListItemViewsHolder();
		myListItemViewsHolder.Init(_Params.ItemPrefab, _Params.Content, itemIndex);
		return myListItemViewsHolder;
	}

	protected override void UpdateViewsHolder(MyListItemViewsHolder newOrRecycled)
	{
		BrawlListModel brawlListModel = Data[newOrRecycled.ItemIndex];
		newOrRecycled.txtTitle.text = brawlListModel.brawlRef.coloredName;
		newOrRecycled.txtDescription.text = brawlListModel.brawlRef.desc;
		newOrRecycled.summonBtn.brawlRef = brawlListModel.brawlRef;
		newOrRecycled.summonBtn.brawlCreateWindow = _brawlCreateWindow;
		int sortingOrder = _brawlCreateWindow.sortingLayer + 1;
		Transform obj = Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<Transform>("ui/promo/" + typeof(PromoObject).Name));
		obj.SetParent(newOrRecycled.root.transform, worldPositionStays: false);
		PromoObject component = obj.GetComponent<PromoObject>();
		component.LoadDetails(brawlListModel.brawlRef.promoRef, newOrRecycled.placeholderPromo.transform, PromoObject.TYPE.BRAWL);
		component.CreateAssets(sortingOrder);
		Util.ChangeChildrenParticleSystemMaskInteraction(obj.gameObject, SpriteMaskInteraction.VisibleInsideMask);
		for (int i = 0; i < Data.Count; i++)
		{
			if (Data.Count == _Params.obj[0].transform.childCount)
			{
				_Params.obj[0].transform.GetChild(i).GetComponent<BrawlDotClick>().SetID(i);
			}
			else if (Data.Count > _Params.obj[0].transform.childCount)
			{
				Object.Instantiate(_Params.obj[1], _Params.obj[0].transform);
			}
			else
			{
				Object.Destroy(_Params.obj[0].transform.GetChild(_Params.obj[0].transform.childCount));
			}
		}
		_Params.obj[0].transform.GetChild(currentItemIndex).GetComponent<Image>().color = Color.green;
	}

	protected override void OnBeforeRecycleOrDisableViewsHolder(MyListItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
		if (inRecycleBinOrVisible.root.GetComponentInChildren<PromoObject>() != null)
		{
			Object.Destroy(inRecycleBinOrVisible.root.GetComponentInChildren<PromoObject>().gameObject);
		}
		if (inRecycleBinOrVisible.placeholderPromo.childCount > 0)
		{
			for (int i = 0; i < inRecycleBinOrVisible.placeholderPromo.childCount; i++)
			{
				Object.Destroy(inRecycleBinOrVisible.placeholderPromo.GetChild(i).gameObject);
			}
		}
	}

	public void AddItemsAt(int index, IList<BrawlListModel> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<BrawlListModel> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		BrawlListModel[] newItems = new BrawlListModel[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		BrawlListModel[] newItems = new BrawlListModel[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(BrawlListModel[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
