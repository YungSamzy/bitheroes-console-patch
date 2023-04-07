using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.brawl;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.ui.language;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui.lists.dropdownlist;

public class DropdownList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	private UnityAction<MyDropdownItemModel> onClickDelegate;

	private GameObject p;

	private int currentSeleted;

	private AsianLanguageFontManager asianLangManager;

	public SimpleDataHelper<MyDropdownItemModel> Data { get; private set; }

	public void StartList(GameObject _parent, int _id, UnityAction<MyDropdownItemModel> onClickDelegate = null)
	{
		this.onClickDelegate = onClickDelegate;
		p = _parent;
		currentSeleted = _id;
		if (Data == null)
		{
			Data = new SimpleDataHelper<MyDropdownItemModel>(this);
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

	protected override void Start()
	{
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
		MyDropdownItemModel model = Data[newOrRecycled.ItemIndex];
		newOrRecycled.btn.onClick.AddListener(delegate
		{
			Broad(model);
		});
		newOrRecycled.titleText.text = model.title;
		if (model.desc != null)
		{
			newOrRecycled.descText.text = model.desc;
			newOrRecycled.descText.gameObject.SetActive(value: true);
		}
		else
		{
			newOrRecycled.descText.gameObject.SetActive(value: false);
			newOrRecycled.titleText.transform.localPosition = Vector3.zero;
		}
		newOrRecycled.btnHelp.gameObject.SetActive(model.btnHelp);
		if (model.btnHelp && model.data != null)
		{
			newOrRecycled.btnHelp.onClick.AddListener(delegate
			{
				GameData.instance.audioManager.PlaySoundLink("buttonclick");
				List<GameModifier> list = null;
				if (model.data is CurrencyBonusRef)
				{
					list = (model.data as CurrencyBonusRef).modifiers;
				}
				if (model.data is BrawlTierDifficultyRef)
				{
					list = (model.data as BrawlTierDifficultyRef).difficultyRef.modifiers;
				}
				if (list != null)
				{
					GameData.instance.windowGenerator.NewGameModifierListWindow(list);
				}
			});
		}
		if (currentSeleted == model.id || model.locked)
		{
			newOrRecycled.btn.interactable = false;
		}
		else
		{
			newOrRecycled.btn.interactable = true;
		}
		asianLangManager = base.gameObject.GetComponent<AsianLanguageFontManager>();
		if (asianLangManager == null)
		{
			asianLangManager = base.gameObject.AddComponent<AsianLanguageFontManager>();
		}
		if (asianLangManager != null)
		{
			asianLangManager.SetAsianFontsIfNeeded();
		}
	}

	private void Broad(MyDropdownItemModel model)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (onClickDelegate != null)
		{
			onClickDelegate(model);
		}
		else
		{
			p.BroadcastMessage("ReloadList", model.id, SendMessageOptions.DontRequireReceiver);
		}
		currentSeleted = model.id;
	}

	protected override void OnBeforeRecycleOrDisableViewsHolder(MyListItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		inRecycleBinOrVisible.btn.onClick.RemoveAllListeners();
		base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<MyDropdownItemModel> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<MyDropdownItemModel> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		MyDropdownItemModel[] newItems = new MyDropdownItemModel[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		MyDropdownItemModel[] newItems = new MyDropdownItemModel[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(MyDropdownItemModel[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
