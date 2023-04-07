using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.promo;
using com.ultrabit.bitheroes.ui.raid;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.raidlist;

public class RaidList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	private RaidWindow _raidWindow;

	public int currentItemIndex;

	public SimpleDataHelper<RaidListModel> Data { get; private set; }

	public void InitList(RaidWindow raidWindow)
	{
		_raidWindow = raidWindow;
		if (Data == null)
		{
			Data = new SimpleDataHelper<RaidListModel>(this);
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

	public void ScrollToPage(int index, bool directo = false, bool noAnim = false)
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
			if (noAnim)
			{
				ScrollTo(0);
			}
			else
			{
				SmoothScrollTo(0, 0.2f, 0.5f, 0.5f, null, null, overrideCurrentScrollingAnimation: true);
			}
			currentItemIndex = 0;
		}
		else if (currentItemIndex < 0)
		{
			if (noAnim)
			{
				ScrollTo(Data.Count - 1);
			}
			else
			{
				SmoothScrollTo(Data.Count - 1, 0.2f, 0.5f, 0.5f, null, null, overrideCurrentScrollingAnimation: true);
			}
			currentItemIndex = Data.Count - 1;
		}
		else if (noAnim)
		{
			ScrollTo(currentItemIndex);
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
		RaidListModel raidListModel = Data[newOrRecycled.ItemIndex];
		newOrRecycled.txtTitle.text = raidListModel.title;
		newOrRecycled.txtDescription.text = raidListModel.description;
		newOrRecycled.summonBtn.id = raidListModel.id;
		newOrRecycled.summonBtn.raidWindow = _raidWindow;
		int sortingOrder = _raidWindow.sortingLayer + 1;
		Transform obj = Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<Transform>("ui/promo/" + typeof(PromoObject).Name));
		obj.SetParent(newOrRecycled.root.transform, worldPositionStays: false);
		PromoObject component = obj.GetComponent<PromoObject>();
		component.LoadDetails(raidListModel.raidRef.promoRef, newOrRecycled.placeholderPromo.transform, PromoObject.TYPE.RAID);
		component.CreateAssets(sortingOrder);
		Util.ChangeChildrenParticleSystemMaskInteraction(obj.gameObject, SpriteMaskInteraction.VisibleInsideMask);
		for (int i = 0; i < Data.Count; i++)
		{
			if (Data.Count == _Params.obj[0].transform.childCount)
			{
				_Params.obj[0].transform.GetChild(i).GetComponent<RaidDotClick>().SetID(i);
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

	public void AddItemsAt(int index, IList<RaidListModel> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<RaidListModel> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		RaidListModel[] newItems = new RaidListModel[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		RaidListModel[] newItems = new RaidListModel[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(RaidListModel[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
