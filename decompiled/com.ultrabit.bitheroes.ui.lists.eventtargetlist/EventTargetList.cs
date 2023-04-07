using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.events;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace com.ultrabit.bitheroes.ui.lists.eventtargetlist;

public class EventTargetList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	private UnityAction<EventTargetData> onSelectOpponent;

	private EventTargetWindow eventTargetWindow;

	public SimpleDataHelper<EventTargetItem> Data { get; private set; }

	public void InitList(UnityAction<EventTargetData> onSelectOpponent, EventTargetWindow eventTargetWindow)
	{
		this.onSelectOpponent = onSelectOpponent;
		this.eventTargetWindow = eventTargetWindow;
		if (Data == null)
		{
			Data = new SimpleDataHelper<EventTargetItem>(this);
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
		if (Data == null)
		{
			Data = new SimpleDataHelper<EventTargetItem>(this);
			base.Start();
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
		EventTargetItem model = Data[newOrRecycled.ItemIndex];
		string text = ((model.eventTargetData.pointsWin > 0) ? "+" : "");
		string text2 = ((model.eventTargetData.pointsLose > 0) ? "+" : "");
		newOrRecycled.NameTxt.text = model.eventTargetData.characterData.parsedName;
		newOrRecycled.StatsTxt.text = Util.NumberFormat(model.eventTargetData.stats);
		newOrRecycled.WinTxt.text = "<color=green>" + text + Util.NumberFormat(model.eventTargetData.pointsWin) + "</color>";
		newOrRecycled.LoseTxt.text = "<color=green>" + text2 + Util.NumberFormat(model.eventTargetData.pointsLose) + "</color>";
		newOrRecycled.FightBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_fight");
		newOrRecycled.FightBtn.onClick.AddListener(delegate
		{
			GameData.instance.audioManager.PlaySoundLink("buttonclick");
			onSelectOpponent(model.eventTargetData);
		});
		newOrRecycled.UIFrameImage.color = (model.eventTargetData.characterData.isIMXG0 ? model.eventTargetData.characterData.nftRarityColor : Color.white);
		newOrRecycled.avatarGenerationBanner.gameObject.SetActive(model.eventTargetData.characterData.isIMXG0);
		newOrRecycled.avatarBackground.gameObject.SetActive(model.eventTargetData.characterData.isIMXG0);
		if (model.eventTargetData.characterData.isIMXG0)
		{
			newOrRecycled.avatarBackground.LoadDetails(model.eventTargetData.characterData.nftBackground, model.eventTargetData.characterData.nftFrameSimple, model.eventTargetData.characterData.nftFrameSeparator);
			newOrRecycled.avatarGenerationBanner.LoadDetails(model.eventTargetData.characterData.nftGeneration, model.eventTargetData.characterData.nftRarity);
		}
		CharacterDisplay characterDisplay = model.eventTargetData.characterData.toCharacterDisplay(0.65f);
		characterDisplay.transform.SetParent(newOrRecycled.placeholderAsset.transform, worldPositionStays: false);
		characterDisplay.SetLocalPosition(new Vector3(0f, -63f, 0f));
		characterDisplay.HideMaskedElements();
		Util.ChangeLayer(characterDisplay.transform, "UI");
		SpriteRenderer[] componentsInChildren = characterDisplay.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
		}
		int num = 2 + newOrRecycled.root.transform.GetSiblingIndex() + eventTargetWindow.sortingLayer;
		newOrRecycled.avatarGenerationBanner.SetSpriteMaskRange(num, num - 1);
		if (newOrRecycled.assetMask0 != null)
		{
			newOrRecycled.assetMask0.frontSortingLayerID = SortingLayer.NameToID("UI");
			newOrRecycled.assetMask0.frontSortingOrder = num;
			newOrRecycled.assetMask0.backSortingLayerID = SortingLayer.NameToID("UI");
			newOrRecycled.assetMask0.backSortingOrder = num - 1;
		}
		if (newOrRecycled.assetMask1 != null)
		{
			newOrRecycled.assetMask1.frontSortingLayerID = SortingLayer.NameToID("UI");
			newOrRecycled.assetMask1.frontSortingOrder = num;
			newOrRecycled.assetMask1.backSortingLayerID = SortingLayer.NameToID("UI");
			newOrRecycled.assetMask1.backSortingOrder = num - 1;
		}
		if (newOrRecycled.assetMask2 != null)
		{
			newOrRecycled.assetMask2.frontSortingLayerID = SortingLayer.NameToID("UI");
			newOrRecycled.assetMask2.frontSortingOrder = num;
			newOrRecycled.assetMask2.backSortingLayerID = SortingLayer.NameToID("UI");
			newOrRecycled.assetMask2.backSortingOrder = num - 1;
		}
		if (newOrRecycled.assetMask3 != null)
		{
			newOrRecycled.assetMask3.frontSortingLayerID = SortingLayer.NameToID("UI");
			newOrRecycled.assetMask3.frontSortingOrder = num;
			newOrRecycled.assetMask3.backSortingLayerID = SortingLayer.NameToID("UI");
			newOrRecycled.assetMask3.backSortingOrder = num - 1;
		}
		SortingGroup sortingGroup = characterDisplay.gameObject.AddComponent<SortingGroup>();
		if (sortingGroup != null && sortingGroup.enabled)
		{
			sortingGroup.sortingLayerName = "UI";
			sortingGroup.sortingOrder = num;
		}
		newOrRecycled.UIFrame.onClick.AddListener(delegate
		{
			GameData.instance.windowGenerator.ShowPlayer(model.eventTargetData.characterData.charID);
		});
	}

	protected override void OnBeforeRecycleOrDisableViewsHolder(MyListItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		CharacterDisplay componentInChildren = inRecycleBinOrVisible.root.GetComponentInChildren<CharacterDisplay>();
		if (componentInChildren != null)
		{
			Object.Destroy(componentInChildren.gameObject);
		}
		base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
		inRecycleBinOrVisible.FightBtn.onClick.RemoveAllListeners();
		inRecycleBinOrVisible.UIFrame.onClick.RemoveAllListeners();
		inRecycleBinOrVisible.UIFrameHover.OnExit();
	}

	public void AddItemsAt(int index, IList<EventTargetItem> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<EventTargetItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		EventTargetItem[] newItems = new EventTargetItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		EventTargetItem[] newItems = new EventTargetItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(EventTargetItem[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
