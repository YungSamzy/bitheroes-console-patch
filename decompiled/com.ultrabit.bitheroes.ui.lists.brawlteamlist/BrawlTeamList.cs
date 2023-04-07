using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.brawl;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.brawl;
using com.ultrabit.bitheroes.ui.character;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.brawlteamlist;

public class BrawlTeamList : OSA<MyParams, MyViewsHolder>
{
	private Canvas _Canvas;

	private RectTransform _CanvasRT;

	private UnityAction onAddButtonClicked;

	private UnityAction<BrawlPlayer> onRemoveButtonClicked;

	private BrawlRoomWindow brawlRoomWindow;

	public SimpleDataHelper<BrawlTeamListItemData> Data { get; private set; }

	public void InitList(UnityAction<BrawlPlayer> onRemoveButtonClicked, UnityAction onAddButtonClicked, BrawlRoomWindow brawlRoomWindow)
	{
		this.onAddButtonClicked = onAddButtonClicked;
		this.onRemoveButtonClicked = onRemoveButtonClicked;
		this.brawlRoomWindow = brawlRoomWindow;
		if (Data == null)
		{
			Data = new SimpleDataHelper<BrawlTeamListItemData>(this);
			_Canvas = GetComponentInParent<Canvas>();
			_CanvasRT = _Canvas.transform as RectTransform;
			base.Start();
		}
	}

	public List<int> GetPlayerOrder()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < Data.Count; i++)
		{
			if (Data[i].player == null)
			{
				list.Add(0);
			}
			else
			{
				list.Add(Data[i].player.characterData.charID);
			}
		}
		return list;
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

	private void SwapUp(RectTransform item)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		MyViewsHolder itemViewsHolderIfVisible = GetItemViewsHolderIfVisible(item);
		if (itemViewsHolderIfVisible == null)
		{
			return;
		}
		if (itemViewsHolderIfVisible.ItemIndex > 0)
		{
			Data.Swap(itemViewsHolderIfVisible.ItemIndex, itemViewsHolderIfVisible.ItemIndex - 1);
			for (int i = 0; i < _VisibleItemsCount; i++)
			{
				UpdateViewsHolder(_VisibleItems[i]);
			}
		}
		brawlRoomWindow.DoOrder(GetPlayerOrder());
	}

	private void SwapDown(RectTransform item)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		MyViewsHolder itemViewsHolderIfVisible = GetItemViewsHolderIfVisible(item);
		if (itemViewsHolderIfVisible == null)
		{
			return;
		}
		if (itemViewsHolderIfVisible.ItemIndex < Data.Count - 1)
		{
			Data.Swap(itemViewsHolderIfVisible.ItemIndex, itemViewsHolderIfVisible.ItemIndex + 1);
			for (int i = 0; i < _VisibleItemsCount; i++)
			{
				UpdateViewsHolder(_VisibleItems[i]);
			}
		}
		brawlRoomWindow.DoOrder(GetPlayerOrder());
	}

	private void RetrieveDataAndUpdate(int count)
	{
	}

	private void OnDataRetrieved(BrawlTeamListItemData[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}

	protected override void CollectItemsSizes(ItemCountChangeMode changeMode, int count, int indexIfInsertingOrRemoving, ItemsDescriptor itemsDesc)
	{
		base.CollectItemsSizes(changeMode, count, indexIfInsertingOrRemoving, itemsDesc);
		if (changeMode == ItemCountChangeMode.RESET && count != 0)
		{
			int num = 0;
			int num2 = num + count;
			itemsDesc.BeginChangingItemsSizes(num);
			for (int i = num; i < num2; i++)
			{
				itemsDesc[i] = Random.Range(_Params.DefaultItemSize / 3f, _Params.DefaultItemSize * 3f);
			}
			itemsDesc.EndChangingItemsSizes();
		}
	}

	protected override MyViewsHolder CreateViewsHolder(int itemIndex)
	{
		MyViewsHolder myViewsHolder = new MyViewsHolder();
		myViewsHolder.Init(_Params.ItemPrefab, _Params.Content, itemIndex);
		return myViewsHolder;
	}

	public void UpdateListItem(MyViewsHolder item)
	{
		UpdateViewsHolder(item);
	}

	protected override void UpdateViewsHolder(MyViewsHolder newOrRecycled)
	{
		BrawlTeamListItemData brawlTeamListItemData = Data[newOrRecycled.ItemIndex];
		newOrRecycled.btnTxt.text = Language.GetString("ui_invite");
		newOrRecycled.removeBtnTxt.text = Language.GetString("ui_x");
		bool empty = brawlTeamListItemData == null || brawlTeamListItemData.player == null;
		SetSlot(brawlTeamListItemData, newOrRecycled, empty);
	}

	private void SetSlot(BrawlTeamListItemData model, MyViewsHolder newOrRecycled, bool empty)
	{
		if (newOrRecycled.addBtn != null)
		{
			newOrRecycled.addBtn.gameObject.SetActive(empty);
		}
		if (newOrRecycled.numberTxt != null)
		{
			newOrRecycled.numberTxt.gameObject.SetActive(empty);
		}
		if (newOrRecycled.frame != null)
		{
			newOrRecycled.frame.gameObject.SetActive(value: true);
			newOrRecycled.frame.color = Color.white;
		}
		if (newOrRecycled.teamObject != null)
		{
			newOrRecycled.teamObject.gameObject.SetActive(!empty);
		}
		if (newOrRecycled.frameGreen != null)
		{
			newOrRecycled.frameGreen.gameObject.SetActive(!empty);
		}
		if (newOrRecycled.avatarFrameGreen != null)
		{
			newOrRecycled.avatarFrameGreen.gameObject.SetActive(!empty);
		}
		if (newOrRecycled.avatarBackground != null)
		{
			newOrRecycled.frameGreen.gameObject.SetActive(!empty);
		}
		if (newOrRecycled.avatarGenerationBanner != null)
		{
			newOrRecycled.frameGreen.gameObject.SetActive(!empty);
		}
		if (newOrRecycled.upBtn != null)
		{
			newOrRecycled.upBtn.gameObject.SetActive(!empty);
		}
		if (newOrRecycled.downBtn != null)
		{
			newOrRecycled.downBtn.gameObject.SetActive(!empty);
		}
		if (newOrRecycled.removeBtn != null)
		{
			newOrRecycled.removeBtn.gameObject.SetActive(!empty);
		}
		if (newOrRecycled.addBtn != null && newOrRecycled.addBtn.gameObject != null)
		{
			newOrRecycled.addBtn.GetComponent<Button>().onClick.RemoveAllListeners();
		}
		if (newOrRecycled.root != null && newOrRecycled.root.gameObject != null)
		{
			newOrRecycled.root.GetComponent<Button>().onClick.RemoveAllListeners();
		}
		if (empty)
		{
			if (newOrRecycled.numberTxt != null)
			{
				newOrRecycled.numberTxt.text = (newOrRecycled.ItemIndex + 1).ToString();
			}
			if (model.room.leader == GameData.instance.PROJECT.character.id)
			{
				if (newOrRecycled.addBtn != null && newOrRecycled.addBtn.gameObject != null)
				{
					Util.SetButton(newOrRecycled.addBtn.GetComponent<Button>());
					newOrRecycled.addBtn.GetComponent<Button>().onClick.AddListener(delegate
					{
						GameData.instance.audioManager.PlaySoundLink("buttonclick");
						onAddButtonClicked();
					});
				}
				if (newOrRecycled.root != null && newOrRecycled.root.gameObject != null)
				{
					newOrRecycled.root.GetComponent<Button>().onClick.AddListener(delegate
					{
						GameData.instance.audioManager.PlaySoundLink("buttonclick");
						onAddButtonClicked();
					});
				}
			}
			else if (newOrRecycled.addBtn != null && newOrRecycled.addBtn.gameObject != null)
			{
				Util.SetButton(newOrRecycled.addBtn.GetComponent<Button>(), enabled: false);
			}
			if (newOrRecycled.frame != null && newOrRecycled.frame.gameObject != null)
			{
				Util.SetImageAlpha(newOrRecycled.frame, alpha: true);
			}
			if (newOrRecycled.root != null && newOrRecycled.root.gameObject != null)
			{
				newOrRecycled.root.GetComponent<Image>().raycastTarget = false;
			}
			return;
		}
		Util.SetImageAlpha(newOrRecycled.frame, alpha: false);
		if (model.room.leader != GameData.instance.PROJECT.character.id)
		{
			Util.SetButton(newOrRecycled.upBtn.GetComponent<Button>(), enabled: false, useInteractable: true);
			Util.SetButton(newOrRecycled.downBtn.GetComponent<Button>(), enabled: false, useInteractable: true);
			Util.SetButton(newOrRecycled.removeBtn.GetComponent<Button>(), enabled: false, useInteractable: true);
			if (newOrRecycled.root != null && newOrRecycled.root.gameObject != null)
			{
				newOrRecycled.root.GetComponent<Image>().raycastTarget = false;
			}
		}
		else
		{
			if (newOrRecycled.root != null && newOrRecycled.root.gameObject != null)
			{
				newOrRecycled.root.GetComponent<Image>().raycastTarget = true;
			}
			Util.SetButton(newOrRecycled.upBtn.GetComponent<Button>(), newOrRecycled.ItemIndex != 0, useInteractable: true);
			Util.SetButton(newOrRecycled.downBtn.GetComponent<Button>(), newOrRecycled.ItemIndex != brawlRoomWindow.room.slots.Count - 1, useInteractable: true);
			Util.SetButton(newOrRecycled.removeBtn.GetComponent<Button>(), enabled: true, useInteractable: true);
			if (model.player.characterData.charID == GameData.instance.PROJECT.character.id)
			{
				Util.SetButton(newOrRecycled.removeBtn.GetComponent<Button>(), enabled: false, useInteractable: true);
			}
			else if (newOrRecycled.removeBtn != null && newOrRecycled.removeBtn.gameObject != null)
			{
				Util.SetButton(newOrRecycled.removeBtn.GetComponent<Button>(), enabled: true, useInteractable: true);
				newOrRecycled.removeBtn.GetComponent<Button>().onClick.RemoveAllListeners();
				newOrRecycled.removeBtn.GetComponent<Button>().onClick.AddListener(delegate
				{
					GameData.instance.audioManager.PlaySoundLink("buttonclick");
					onRemoveButtonClicked(Data[newOrRecycled.ItemIndex].player);
				});
			}
		}
		if (newOrRecycled.nameTxt != null)
		{
			newOrRecycled.nameTxt.text = model.player.characterData.name;
		}
		if (newOrRecycled.statsTxt != null)
		{
			newOrRecycled.statsTxt.text = Util.NumberFormat(model.player.characterData.getTotalStats());
		}
		int highestStat = model.player.characterData.getHighestStat();
		if (newOrRecycled.powerIcon != null)
		{
			newOrRecycled.powerIcon.gameObject.SetActive(highestStat == 0);
		}
		if (newOrRecycled.staminaIcon != null)
		{
			newOrRecycled.staminaIcon.gameObject.SetActive(highestStat == 1);
		}
		if (newOrRecycled.agilityIcon != null)
		{
			newOrRecycled.agilityIcon.gameObject.SetActive(highestStat == 2);
		}
		if (model.room.leader == model.player.characterData.charID)
		{
			if (newOrRecycled.leaderIcon != null)
			{
				newOrRecycled.leaderIcon.gameObject.SetActive(value: true);
			}
			if (newOrRecycled.readyIcon != null)
			{
				newOrRecycled.readyIcon.gameObject.SetActive(value: false);
			}
			if (newOrRecycled.unreadyIcon != null)
			{
				newOrRecycled.unreadyIcon.gameObject.SetActive(value: false);
			}
			if (newOrRecycled.frameGreen != null)
			{
				newOrRecycled.frameGreen.gameObject.SetActive(value: false);
			}
			if (newOrRecycled.avatarFrameGreen != null)
			{
				newOrRecycled.avatarFrameGreen.gameObject.SetActive(value: false);
			}
			if (newOrRecycled.frame != null)
			{
				newOrRecycled.frame.gameObject.SetActive(value: true);
			}
		}
		else if (model.player.ready)
		{
			if (newOrRecycled.leaderIcon != null)
			{
				newOrRecycled.leaderIcon.gameObject.SetActive(value: false);
			}
			if (newOrRecycled.readyIcon != null)
			{
				newOrRecycled.readyIcon.gameObject.SetActive(value: true);
			}
			if (newOrRecycled.unreadyIcon != null)
			{
				newOrRecycled.unreadyIcon.gameObject.SetActive(value: false);
			}
			if (newOrRecycled.frameGreen != null)
			{
				if (model.player.characterData.isIMXG0)
				{
					newOrRecycled.frameGreen.gameObject.SetActive(value: false);
				}
				else
				{
					newOrRecycled.frameGreen.gameObject.SetActive(value: true);
				}
			}
			if (newOrRecycled.avatarFrameGreen != null)
			{
				if (model.player.characterData.isIMXG0)
				{
					newOrRecycled.avatarFrameGreen.gameObject.SetActive(value: true);
				}
				else
				{
					newOrRecycled.avatarFrameGreen.gameObject.SetActive(value: false);
				}
			}
			if (newOrRecycled.frame != null)
			{
				newOrRecycled.frame.gameObject.SetActive(value: false);
			}
		}
		else
		{
			if (newOrRecycled.leaderIcon != null)
			{
				newOrRecycled.leaderIcon.gameObject.SetActive(value: false);
			}
			if (newOrRecycled.readyIcon != null)
			{
				newOrRecycled.readyIcon.gameObject.SetActive(value: false);
			}
			if (newOrRecycled.unreadyIcon != null)
			{
				newOrRecycled.unreadyIcon.gameObject.SetActive(value: true);
			}
			if (newOrRecycled.frameGreen != null)
			{
				newOrRecycled.frameGreen.gameObject.SetActive(value: false);
			}
			if (newOrRecycled.avatarFrameGreen != null)
			{
				newOrRecycled.avatarFrameGreen.gameObject.SetActive(value: false);
			}
			if (newOrRecycled.frame != null)
			{
				newOrRecycled.frame.gameObject.SetActive(value: true);
			}
		}
		if (newOrRecycled.placeholderAsset != null && newOrRecycled.placeholderAsset.childCount > 0)
		{
			for (int i = 0; i < newOrRecycled.placeholderAsset.childCount; i++)
			{
				Object.Destroy(newOrRecycled.placeholderAsset.GetChild(i).gameObject);
			}
		}
		newOrRecycled.frame.color = (model.player.characterData.isIMXG0 ? model.player.characterData.nftRarityColor : Color.white);
		newOrRecycled.avatarGenerationBanner.gameObject.SetActive(model.player.characterData.isIMXG0);
		newOrRecycled.avatarBackground.gameObject.SetActive(model.player.characterData.isIMXG0);
		if (model.player.characterData.isIMXG0)
		{
			newOrRecycled.avatarBackground.LoadDetails(model.player.characterData.nftBackground, model.player.characterData.nftFrameSimple, model.player.characterData.nftFrameSeparator);
			newOrRecycled.avatarGenerationBanner.LoadDetails(model.player.characterData.nftGeneration, model.player.characterData.nftRarity);
		}
		CharacterDisplay characterDisplay = model.player.characterData.toCharacterDisplay(2f / brawlRoomWindow.panel.transform.localScale.x, displayMount: false, null, enableLoading: false);
		if (characterDisplay != null)
		{
			characterDisplay.transform.SetParent(newOrRecycled.placeholderAsset.transform, worldPositionStays: false);
			characterDisplay.transform.localPosition = Vector3.zero;
			CharacterDisplay component = characterDisplay.GetComponent<CharacterDisplay>();
			if (component != null)
			{
				component.SetLocalPosition(new Vector3(0f, -63f, 0f));
				component.HideMaskedElements();
			}
			Util.ChangeLayer(characterDisplay.transform, "UI");
			SpriteRenderer[] componentsInChildren = characterDisplay.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				componentsInChildren[j].maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
			}
		}
		if (newOrRecycled.root == null)
		{
			return;
		}
		int num = 2 + newOrRecycled.root.transform.GetSiblingIndex() + brawlRoomWindow.sortingLayer;
		newOrRecycled.statsObject.sortingOrder = num + 2;
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
		if (newOrRecycled.assetMask4 != null)
		{
			newOrRecycled.assetMask4.frontSortingLayerID = SortingLayer.NameToID("UI");
			newOrRecycled.assetMask4.frontSortingOrder = num;
			newOrRecycled.assetMask4.backSortingLayerID = SortingLayer.NameToID("UI");
			newOrRecycled.assetMask4.backSortingOrder = num - 1;
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
		newOrRecycled.root.GetComponent<Button>().onClick.AddListener(delegate
		{
			GameData.instance.audioManager.PlaySoundLink("buttonclick");
			GameData.instance.windowGenerator.ShowPlayer(model.player.characterData.charID);
		});
	}

	public void RemoveModel(int index)
	{
		Data.List[index] = null;
		for (int i = 0; i < _VisibleItemsCount; i++)
		{
			UpdateViewsHolder(_VisibleItems[i]);
		}
	}

	private void UpdateScaleOfVisibleItems(MyViewsHolder vhToRotate)
	{
		foreach (MyViewsHolder visibleItem in _VisibleItems)
		{
			visibleItem.scalableViews.localScale = Vector3.one * ((visibleItem == vhToRotate) ? 0.98f : 1f);
		}
	}
}
