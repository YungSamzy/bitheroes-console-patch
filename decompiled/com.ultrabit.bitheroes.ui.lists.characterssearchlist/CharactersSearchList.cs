using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.ui.utility;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui.lists.characterssearchlist;

public class CharactersSearchList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	private UnityAction<string> _selectCallback;

	public SimpleDataHelper<CharacterSearchItem> Data { get; private set; }

	public void InitList(UnityAction<string> selectCallback)
	{
		_selectCallback = selectCallback;
		if (Data == null)
		{
			Data = new SimpleDataHelper<CharacterSearchItem>(this);
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
		CharacterSearchItem model = Data[newOrRecycled.ItemIndex];
		newOrRecycled.nameTxt.text = model.character.parsedName + "<color=#9FA9B5> #" + model.character.herotag + " (" + model.character.level + ")</color>";
		newOrRecycled.selectBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_select");
		newOrRecycled.selectBtn.gameObject.SetActive(model.showSelect);
		newOrRecycled.selectBtn.onClick.RemoveAllListeners();
		newOrRecycled.borderPlain.onClick.RemoveAllListeners();
		newOrRecycled.selectBtn.onClick.AddListener(delegate
		{
			OnClickSelect(model);
		});
		newOrRecycled.borderPlain.onClick.AddListener(delegate
		{
			OnClickBorder(model);
		});
		if (newOrRecycled.borderPlain.GetComponent<HoverImages>() == null)
		{
			newOrRecycled.borderPlain.gameObject.AddComponent<HoverImages>().ForceStart();
		}
	}

	private void OnClickSelect(CharacterSearchItem model)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_selectCallback(model.character.name + "#" + model.character.herotag);
	}

	private void OnClickBorder(CharacterSearchItem model)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowPlayer(model.character.charID);
	}

	protected override void OnBeforeRecycleOrDisableViewsHolder(MyListItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		inRecycleBinOrVisible.borderPlain.GetComponent<HoverImages>().OnExit();
		base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<CharacterSearchItem> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<CharacterSearchItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		CharacterSearchItem[] newItems = new CharacterSearchItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		CharacterSearchItem[] newItems = new CharacterSearchItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(CharacterSearchItem[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
