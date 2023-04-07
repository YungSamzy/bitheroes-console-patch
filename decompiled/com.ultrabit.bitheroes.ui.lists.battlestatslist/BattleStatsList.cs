using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.team;
using com.ultrabit.bitheroes.model.user;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.battle;
using com.ultrabit.bitheroes.ui.character;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using UnityEngine;
using UnityEngine.Rendering;

namespace com.ultrabit.bitheroes.ui.lists.battlestatslist;

public class BattleStatsList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	private float maxW;

	private BattleStatsWindow _battleStatsWindow;

	private Dictionary<int, DataCallbackStruct> dataStruct;

	public SimpleDataHelper<BattleStatsItem> Data { get; private set; }

	public void InitList(BattleStatsWindow battleStatsWindow)
	{
		_battleStatsWindow = battleStatsWindow;
		if (Data == null)
		{
			Data = new SimpleDataHelper<BattleStatsItem>(this);
			dataStruct = new Dictionary<int, DataCallbackStruct>();
			maxW = 0f;
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
		BattleStatsItem model = Data[newOrRecycled.ItemIndex];
		newOrRecycled.redBar.gameObject.SetActive(!model.data.attacker);
		newOrRecycled.greenBar.gameObject.SetActive(!newOrRecycled.redBar.gameObject.activeSelf);
		TeammateData teammateData = new TeammateData(model.data.id, model.data.type, -1L);
		if (teammateData != null && teammateData.data != null)
		{
			if (newOrRecycled.assetPlaceholder.childCount > 0)
			{
				Object.Destroy(newOrRecycled.assetPlaceholder.GetChild(0).gameObject);
			}
			bool flag = teammateData.type == 1;
			CharacterData characterData = (flag ? (teammateData.data as CharacterData) : null);
			newOrRecycled.frameImage.color = ((flag && characterData.isIMXG0) ? characterData.nftRarityColor : Color.white);
			newOrRecycled.avatarGenerationBanner.gameObject.SetActive(flag && characterData.isIMXG0);
			newOrRecycled.avatarBackground.gameObject.SetActive(flag && characterData.isIMXG0);
			if (flag && characterData.isIMXG0)
			{
				newOrRecycled.avatarBackground.LoadDetails(characterData.nftBackground, characterData.nftFrameSimple, characterData.nftFrameSeparator);
				newOrRecycled.avatarGenerationBanner.LoadDetails(characterData.nftGeneration, characterData.nftRarity);
			}
			Transform asset = teammateData.getAsset(2f / 3f);
			asset.transform.SetParent(newOrRecycled.assetPlaceholder.transform, worldPositionStays: false);
			asset.transform.localPosition = Vector3.zero;
			CharacterDisplay componentInChildren = asset.GetComponentInChildren<CharacterDisplay>();
			if (componentInChildren != null)
			{
				Vector3 localPosition = newOrRecycled.assetPlaceholder.localPosition;
				localPosition.y = 0f;
				newOrRecycled.assetPlaceholder.localPosition = localPosition;
				componentInChildren.SetLocalPosition(new Vector3(0f, -63f, 0f));
				componentInChildren.HideMaskedElements();
			}
			else
			{
				Vector3 localPosition2 = newOrRecycled.assetPlaceholder.localPosition;
				localPosition2.y = -14f;
				newOrRecycled.assetPlaceholder.localPosition = localPosition2;
				float x = asset.transform.localScale.x * teammateData.selectScale;
				float y = asset.transform.localScale.y * teammateData.selectScale;
				asset.transform.localScale = new Vector3(x, y, asset.transform.localScale.z);
				float x2 = teammateData.selectOffset.x * asset.localScale.x;
				float y2 = 0f - teammateData.selectOffset.y * asset.localScale.y;
				asset.transform.localPosition = new Vector3(x2, y2, 0f);
				asset.transform.localRotation = Quaternion.identity;
			}
			Util.ChangeLayer(asset.transform, "UI");
			SpriteRenderer[] componentsInChildren = asset.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
			}
			int num = 2 + newOrRecycled.root.transform.GetSiblingIndex() + _battleStatsWindow.sortingLayer;
			if (flag && characterData.isIMXG0)
			{
				newOrRecycled.avatarGenerationBanner.SetSpriteMaskRange(num, num - 1);
			}
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
			SortingGroup sortingGroup = asset.gameObject.AddComponent<SortingGroup>();
			if (sortingGroup != null && sortingGroup.enabled)
			{
				sortingGroup.sortingLayerName = "UI";
				sortingGroup.sortingOrder = num;
			}
			newOrRecycled.nameTxt.text = teammateData.name;
		}
		else
		{
			newOrRecycled.nameTxt.text = "";
			if (!dataStruct.ContainsKey(model.data.id))
			{
				dataStruct.Add(model.data.id, new DataCallbackStruct
				{
					model = model,
					newOrRecycled = newOrRecycled
				});
			}
			CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(9), OnGetProfile);
			CharacterDALC.instance.doGetProfile(model.data.id);
		}
		newOrRecycled.teamTxt.text = (model.data.attacker ? Language.GetString("ui_battle_stats_myteam") : Language.GetString("ui_battle_stats_enemy"));
		newOrRecycled.statTxt.text = model.data.value.ToString();
		if (maxW <= 0f)
		{
			maxW = newOrRecycled.redBar.rectTransform.sizeDelta.x;
		}
		float x3 = (float)model.data.value * maxW / (float)model.data.max;
		if (model.data.max == 0)
		{
			x3 = 0f;
		}
		newOrRecycled.redBar.rectTransform.sizeDelta = new Vector2(x3, newOrRecycled.redBar.rectTransform.sizeDelta.y);
		newOrRecycled.greenBar.rectTransform.sizeDelta = new Vector2(x3, newOrRecycled.greenBar.rectTransform.sizeDelta.y);
		newOrRecycled.frame.onClick.AddListener(delegate
		{
			OnClick(model);
		});
	}

	private void OnGetProfile(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(9), OnGetProfile);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		UserData userData = UserData.fromSFSObject(sfsob);
		if (userData == null)
		{
			return;
		}
		CharacterData characterData = userData.characterData;
		if (characterData == null || !dataStruct.ContainsKey(characterData.charID))
		{
			return;
		}
		BattleStatsItem model = dataStruct[characterData.charID].model;
		MyListItemViewsHolder newOrRecycled = dataStruct[characterData.charID].newOrRecycled;
		dataStruct.Remove(characterData.charID);
		if (characterData == null || model == null || newOrRecycled == null || characterData.charID != model.data.id)
		{
			return;
		}
		if (newOrRecycled.nameTxt != null)
		{
			newOrRecycled.nameTxt.text = characterData.parsedName;
		}
		if (newOrRecycled.assetPlaceholder != null && newOrRecycled.assetPlaceholder.childCount > 0)
		{
			Object.Destroy(newOrRecycled.assetPlaceholder.GetChild(0).gameObject);
		}
		CharacterDisplay characterDisplay = characterData.toCharacterDisplay(2f / 3f);
		if (characterDisplay != null)
		{
			characterDisplay.transform.SetParent(newOrRecycled.assetPlaceholder.transform, worldPositionStays: false);
			characterDisplay.transform.localPosition = Vector3.zero;
			characterDisplay.SetLocalPosition(new Vector3(0f, -63f, 0f));
			Util.ChangeLayer(characterDisplay.transform, "UI");
			SpriteRenderer[] componentsInChildren = characterDisplay.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
			}
		}
		int num = 1 + newOrRecycled.root.transform.GetSiblingIndex() + _battleStatsWindow.sortingLayer;
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
		if (characterDisplay != null)
		{
			SortingGroup sortingGroup = characterDisplay.gameObject.AddComponent<SortingGroup>();
			if (sortingGroup != null && sortingGroup.enabled)
			{
				sortingGroup.sortingLayerName = "UI";
				sortingGroup.sortingOrder = num;
			}
		}
	}

	private void OnClick(BattleStatsItem model)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		switch (model.data.type)
		{
		case 1:
			GameData.instance.windowGenerator.ShowPlayer(model.data.id);
			break;
		case 2:
			GameData.instance.windowGenerator.ShowFamiliar(model.data.id);
			break;
		}
	}

	protected override void OnBeforeRecycleOrDisableViewsHolder(MyListItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		inRecycleBinOrVisible.frame.onClick.RemoveAllListeners();
		inRecycleBinOrVisible.frameHover.OnExit();
		base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<BattleStatsItem> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<BattleStatsItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		BattleStatsItem[] newItems = new BattleStatsItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		BattleStatsItem[] newItems = new BattleStatsItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(BattleStatsItem[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
