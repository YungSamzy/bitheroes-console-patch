using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.brawl;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.utility;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace com.ultrabit.bitheroes.ui.lists.brawlinvitelist;

public class BrawlInviteList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	private BrawlRoomInviteWindow brawlRoomInviteWindow;

	public TextMeshProUGUI emptyText;

	private int highestLevel = int.MinValue;

	private int highestPower = int.MinValue;

	private int highestStamina = int.MinValue;

	private int highestAgility = int.MinValue;

	private int lowestLevel = int.MaxValue;

	private int lowestPower = int.MaxValue;

	private int lowestStamina = int.MaxValue;

	private int lowestAgility = int.MaxValue;

	public SimpleDataHelper<BrawlInviteItem> Data { get; private set; }

	public void InitList(BrawlRoomInviteWindow brawlRoomInviteWindow)
	{
		this.brawlRoomInviteWindow = brawlRoomInviteWindow;
		if (Data == null)
		{
			Data = new SimpleDataHelper<BrawlInviteItem>(this);
			base.Start();
		}
		emptyText.text = Language.GetString("ui_brawl_invite_list_empty");
	}

	public void ClearList()
	{
		if (Data != null && Data.Count > 0)
		{
			Data.RemoveItems(0, Data.Count);
		}
		emptyText.gameObject.SetActive(Data.Count == 0);
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

	public void UpdateListItem(MyListItemViewsHolder item)
	{
		UpdateViewsHolder(item);
	}

	protected override void UpdateViewsHolder(MyListItemViewsHolder newOrRecycled)
	{
		BrawlInviteItem model = Data[newOrRecycled.ItemIndex];
		newOrRecycled.UIName.text = model.characterData.parsedName;
		newOrRecycled.UILogin.text = model.characterData.getLoginText(model.online);
		int level = model.characterData.level;
		int totalPower = model.characterData.getTotalPower();
		int totalStamina = model.characterData.getTotalStamina();
		int totalAgility = model.characterData.getTotalAgility();
		if (level > highestLevel)
		{
			highestLevel = level;
		}
		if (level < lowestLevel)
		{
			lowestLevel = level;
		}
		if (totalPower > highestPower)
		{
			highestPower = totalPower;
		}
		if (totalPower < lowestPower)
		{
			lowestPower = totalPower;
		}
		if (totalStamina > highestStamina)
		{
			highestStamina = totalStamina;
		}
		if (totalStamina < lowestStamina)
		{
			lowestStamina = totalStamina;
		}
		if (totalAgility > highestAgility)
		{
			highestAgility = totalAgility;
		}
		if (totalAgility < lowestAgility)
		{
			lowestAgility = totalAgility;
		}
		newOrRecycled.UILevel.text = Util.colorString(Util.NumberFormat(level), Util.getCurrentColor(highestLevel, level, lowestLevel));
		newOrRecycled.UIPower.text = Util.colorString(Util.NumberFormat(totalPower), Util.getCurrentColor(highestPower, totalPower, lowestPower));
		newOrRecycled.UIStamina.text = Util.colorString(Util.NumberFormat(totalStamina), Util.getCurrentColor(highestStamina, totalStamina, lowestStamina));
		newOrRecycled.UIAgility.text = Util.colorString(Util.NumberFormat(totalAgility), Util.getCurrentColor(highestAgility, totalAgility, lowestAgility));
		newOrRecycled.UIOfflineIcon.gameObject.SetActive(value: false);
		newOrRecycled.UIOfflineSelect.gameObject.SetActive(value: false);
		newOrRecycled.UIOnlineSelect.GetComponentInChildren<TextMeshProUGUI>().text = model.selectText;
		newOrRecycled.UIOnlineSelect.onClick.RemoveAllListeners();
		newOrRecycled.UIOnlineSelect.onClick.AddListener(delegate
		{
			OnTileSelected(model);
		});
		newOrRecycled.UIFrame.onClick.RemoveAllListeners();
		newOrRecycled.UIFrame.onClick.AddListener(delegate
		{
			OnTileClicked(model);
		});
		if (newOrRecycled.placeholderAsset.transform.childCount > 0)
		{
			foreach (Transform item in newOrRecycled.placeholderAsset.transform)
			{
				Object.Destroy(item.gameObject);
			}
		}
		newOrRecycled.UIFrameImage.color = (model.characterData.isIMXG0 ? model.characterData.nftRarityColor : Color.white);
		newOrRecycled.avatarGenerationBanner.gameObject.SetActive(model.characterData.isIMXG0);
		newOrRecycled.avatarBackground.gameObject.SetActive(model.characterData.isIMXG0);
		if (model.characterData.isIMXG0)
		{
			newOrRecycled.avatarBackground.LoadDetails(model.characterData.nftBackground, model.characterData.nftFrameSimple, model.characterData.nftFrameSeparator);
			newOrRecycled.avatarGenerationBanner.LoadDetails(model.characterData.nftGeneration, model.characterData.nftRarity);
		}
		CharacterDisplay characterDisplay = model.characterData.toCharacterDisplay(2f / brawlRoomInviteWindow.panel.transform.localScale.x);
		characterDisplay.transform.SetParent(newOrRecycled.placeholderAsset.transform, worldPositionStays: false);
		characterDisplay.transform.localPosition = Vector3.zero;
		characterDisplay.SetLocalPosition(new Vector3(0f, -63f, 0f));
		Util.ChangeLayer(characterDisplay.transform, "UI");
		SpriteRenderer[] componentsInChildren = characterDisplay.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
		}
		int num = 2 + newOrRecycled.root.transform.GetSiblingIndex() + brawlRoomInviteWindow.sortingLayer;
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
		emptyText.gameObject.SetActive(Data.Count == 0);
	}

	private void OnTileSelected(BrawlInviteItem model)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		model.inviteWindow.DoInvite(model.characterData.charID, model.characterData.name);
	}

	private void OnTileClicked(BrawlInviteItem model)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowPlayer(model.characterData.charID);
	}

	protected override void OnBeforeRecycleOrDisableViewsHolder(MyListItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		inRecycleBinOrVisible.UIOfflineSelect.onClick.RemoveAllListeners();
		inRecycleBinOrVisible.UIOnlineSelect.onClick.RemoveAllListeners();
		inRecycleBinOrVisible.UIFrame.onClick.RemoveAllListeners();
		inRecycleBinOrVisible.UIFrame.gameObject.GetComponent<HoverImage>().OnExit();
		base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<BrawlInviteItem> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<BrawlInviteItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		BrawlInviteItem[] newItems = new BrawlInviteItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		BrawlInviteItem[] newItems = new BrawlInviteItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(BrawlInviteItem[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
