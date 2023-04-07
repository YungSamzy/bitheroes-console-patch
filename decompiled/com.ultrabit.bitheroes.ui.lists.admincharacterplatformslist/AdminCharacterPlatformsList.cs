using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.admincharacterplatformslist;

public class AdminCharacterPlatformsList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	private UnityAction<CharacterPlatformData> _selectedcallBack;

	public SimpleDataHelper<AdminPlatformItem> Data { get; private set; }

	public void InitList(UnityAction<CharacterPlatformData> selectedcallBack)
	{
		_selectedcallBack = selectedcallBack;
		if (Data == null)
		{
			Data = new SimpleDataHelper<AdminPlatformItem>(this);
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
		AdminPlatformItem model = Data[newOrRecycled.ItemIndex];
		newOrRecycled.nameTxt.text = AppInfo.GetPlatformName(model.platformData.platform) + ":";
		newOrRecycled.userIDTxt.text = model.platformData.userID;
		newOrRecycled.enableBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_enable");
		newOrRecycled.disableBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_disable");
		newOrRecycled.enableBtn.GetComponent<Button>().onClick.RemoveAllListeners();
		newOrRecycled.disableBtn.GetComponent<Button>().onClick.RemoveAllListeners();
		if (model.platformData.active)
		{
			Util.SetImageAlpha(newOrRecycled.bg, alpha: false);
			newOrRecycled.enableBtn.gameObject.SetActive(value: false);
			newOrRecycled.disableBtn.gameObject.SetActive(value: true);
			newOrRecycled.disableBtn.GetComponent<Button>().onClick.AddListener(delegate
			{
				_selectedcallBack(model.platformData);
			});
		}
		else
		{
			Util.SetImageAlpha(newOrRecycled.bg, alpha: true);
			newOrRecycled.enableBtn.gameObject.SetActive(value: true);
			newOrRecycled.disableBtn.gameObject.SetActive(value: false);
			newOrRecycled.enableBtn.GetComponent<Button>().onClick.AddListener(delegate
			{
				_selectedcallBack(model.platformData);
			});
		}
	}

	public void AddItemsAt(int index, IList<AdminPlatformItem> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<AdminPlatformItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		AdminPlatformItem[] newItems = new AdminPlatformItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		AdminPlatformItem[] newItems = new AdminPlatformItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(AdminPlatformItem[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
