using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.character;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace com.ultrabit.bitheroes.ui.lists.guildapplicantslist;

public class GuildApplicantsList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	public SimpleDataHelper<GuildApplicantItem> Data { get; private set; }

	protected override void Start()
	{
		InitList();
	}

	public void InitList()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<GuildApplicantItem>(this);
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
		GuildApplicantItem model = Data[newOrRecycled.ItemIndex];
		newOrRecycled.UIName.text = model.userData.characterData.name;
		newOrRecycled.UILevel.text = Util.NumberFormat(model.userData.characterData.level);
		newOrRecycled.UILogin.text = model.userData.characterData.getLoginText(model.userData.online);
		if (model.userData.online)
		{
			newOrRecycled.UIOnline.gameObject.SetActive(value: true);
			newOrRecycled.UIOffline.gameObject.SetActive(value: false);
		}
		else
		{
			newOrRecycled.UIOnline.gameObject.SetActive(value: false);
			newOrRecycled.UIOffline.gameObject.SetActive(value: true);
		}
		newOrRecycled.UIAccept.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_accept");
		newOrRecycled.UIDecline.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_decline");
		if (!model.actionDone)
		{
			Util.SetButton(newOrRecycled.UIAccept);
			Util.SetButton(newOrRecycled.UIDecline);
			newOrRecycled.UIAccept.onClick.AddListener(delegate
			{
				DoApplicationAccept(model);
			});
			newOrRecycled.UIDecline.onClick.AddListener(delegate
			{
				DoApplicationDecline(model);
			});
		}
		else
		{
			Util.SetButton(newOrRecycled.UIAccept, enabled: false);
			Util.SetButton(newOrRecycled.UIDecline, enabled: false);
		}
		foreach (Transform item in newOrRecycled.placeholderAsset.transform)
		{
			Object.Destroy(item.gameObject);
		}
		newOrRecycled.UIFrameImage.color = (model.userData.characterData.isIMXG0 ? model.userData.characterData.nftRarityColor : Color.white);
		newOrRecycled.avatarGenerationBanner.gameObject.SetActive(model.userData.characterData.isIMXG0);
		newOrRecycled.avatarBackground.gameObject.SetActive(model.userData.characterData.isIMXG0);
		if (model.userData.characterData.isIMXG0)
		{
			newOrRecycled.avatarBackground.LoadDetails(model.userData.characterData.nftBackground, model.userData.characterData.nftFrameSimple, model.userData.characterData.nftFrameSeparator);
			newOrRecycled.avatarGenerationBanner.LoadDetails(model.userData.characterData.nftGeneration, model.userData.characterData.nftRarity);
		}
		CharacterDisplay characterDisplay = model.userData.characterData.toCharacterDisplay(0.65f, displayMount: false, null, enableLoading: false);
		characterDisplay.transform.SetParent(newOrRecycled.placeholderAsset.transform, worldPositionStays: false);
		characterDisplay.SetLocalPosition(new Vector3(0f, -63f, 0f));
		Util.ChangeLayer(characterDisplay.transform, "UI");
		SpriteRenderer[] componentsInChildren = characterDisplay.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
		foreach (SpriteRenderer spriteRenderer in componentsInChildren)
		{
			if (spriteRenderer.maskInteraction == SpriteMaskInteraction.VisibleInsideMask)
			{
				spriteRenderer.gameObject.SetActive(value: false);
			}
			else
			{
				spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
			}
		}
		characterDisplay.HideMaskedElements();
		int num = 1 + newOrRecycled.ItemIndex + GameData.instance.windowGenerator.GetLastDialog().sortingLayer;
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
			OnTileClick(model);
		});
	}

	public void DoApplicationAccept(GuildApplicantItem model)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.main.ShowLoading();
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(14), delegate(BaseEvent z)
		{
			OnApplicationAccept(z, model);
		});
		GuildDALC.instance.doApplicationAccept(model.userData.characterData.charID);
	}

	private void OnApplicationAccept(BaseEvent e, GuildApplicantItem model)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(14), delegate(BaseEvent z)
		{
			OnApplicationAccept(z, model);
		});
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		model.actionDone = true;
		Refresh();
	}

	public void DoApplicationDecline(GuildApplicantItem model)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.main.ShowLoading();
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(15), delegate(BaseEvent z)
		{
			OnApplicationDecline(z, model);
		});
		GuildDALC.instance.doApplicationDecline(model.userData.characterData.charID);
	}

	private void OnApplicationDecline(BaseEvent e, GuildApplicantItem model)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(15), delegate(BaseEvent z)
		{
			OnApplicationDecline(z, model);
		});
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		model.actionDone = true;
		Refresh();
	}

	public void OnTileClick(GuildApplicantItem model)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowPlayer(model.userData.characterData.charID);
	}

	protected override void OnBeforeRecycleOrDisableViewsHolder(MyListItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		inRecycleBinOrVisible.UIAccept.onClick.RemoveAllListeners();
		inRecycleBinOrVisible.UIDecline.onClick.RemoveAllListeners();
		inRecycleBinOrVisible.UIFrame.onClick.RemoveAllListeners();
		inRecycleBinOrVisible.UIFrameHover.OnExit();
		base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<GuildApplicantItem> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<GuildApplicantItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		GuildApplicantItem[] newItems = new GuildApplicantItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		GuildApplicantItem[] newItems = new GuildApplicantItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(GuildApplicantItem[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
