using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.friend;
using com.ultrabit.bitheroes.ui.utility;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.friendrecommendlist;

public class FriendRecommendList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	private FriendRecommendWindow friendRecommendWindow;

	private int uILayerID;

	private int highestLevel = int.MinValue;

	private int highestPower = int.MinValue;

	private int highestStamina = int.MinValue;

	private int highestAgility = int.MinValue;

	private int lowestLevel = int.MaxValue;

	private int lowestPower = int.MaxValue;

	private int lowestStamina = int.MaxValue;

	private int lowestAgility = int.MaxValue;

	private int sortingCount;

	public SimpleDataHelper<FriendRecommendItem> Data { get; private set; }

	public void InitList(FriendRecommendWindow friendRecommendWindow)
	{
		this.friendRecommendWindow = friendRecommendWindow;
		if (Data == null)
		{
			Data = new SimpleDataHelper<FriendRecommendItem>(this);
			base.Start();
		}
		uILayerID = SortingLayer.NameToID("UI");
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
		FriendRecommendItem model = Data[newOrRecycled.ItemIndex];
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
		newOrRecycled.UIOfflineIcon.gameObject.SetActive(!model.online);
		newOrRecycled.UIOfflineSelect.gameObject.SetActive(!model.online);
		newOrRecycled.UIOnlineIcon.gameObject.SetActive(model.online);
		newOrRecycled.UIOnlineSelect.gameObject.SetActive(model.online);
		if (model.online)
		{
			newOrRecycled.UIOnlineSelect.GetComponentInChildren<TextMeshProUGUI>().text = model.selectText;
			newOrRecycled.UIOnlineSelect.onClick.RemoveAllListeners();
			newOrRecycled.UIOnlineSelect.onClick.AddListener(delegate
			{
				OnTileSelected(newOrRecycled.UIOnlineSelect, newOrRecycled.UIOfflineSelect, model);
			});
		}
		else
		{
			newOrRecycled.UIOfflineSelect.GetComponentInChildren<TextMeshProUGUI>().text = model.selectText;
			newOrRecycled.UIOfflineSelect.onClick.RemoveAllListeners();
			newOrRecycled.UIOfflineSelect.onClick.AddListener(delegate
			{
				OnTileSelected(newOrRecycled.UIOnlineSelect, newOrRecycled.UIOfflineSelect, model);
			});
		}
		newOrRecycled.UIFrame.onClick.AddListener(delegate
		{
			OnTileClicked(model);
		});
		Util.SetButton(newOrRecycled.UIOnlineSelect, !model.invited);
		Util.SetButton(newOrRecycled.UIOfflineSelect, !model.invited);
		newOrRecycled.UIFrameImage.color = (model.characterData.isIMXG0 ? model.characterData.nftRarityColor : Color.white);
		newOrRecycled.avatarGenerationBanner.gameObject.SetActive(model.characterData.isIMXG0);
		newOrRecycled.avatarBackground.gameObject.SetActive(model.characterData.isIMXG0);
		if (model.characterData.isIMXG0)
		{
			newOrRecycled.avatarBackground.LoadDetails(model.characterData.nftBackground, model.characterData.nftFrameSimple, model.characterData.nftFrameSeparator);
			newOrRecycled.avatarGenerationBanner.LoadDetails(model.characterData.nftGeneration, model.characterData.nftRarity);
		}
		CharacterDisplay characterDisplay = model.characterData.toCharacterDisplay(0.65f);
		characterDisplay.transform.SetParent(newOrRecycled.placeholderAsset.transform, worldPositionStays: false);
		characterDisplay.SetLocalPosition(new Vector3(0f, -63f, 0f));
		Util.ChangeLayer(characterDisplay.transform, "UI");
		SpriteRenderer[] componentsInChildren = characterDisplay.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
		}
		int num = 2 + friendRecommendWindow.sortingLayer + newOrRecycled.ItemIndex;
		newOrRecycled.avatarGenerationBanner.SetSpriteMaskRange(num, num - 1);
		SpriteMask[] array = new SpriteMask[4] { newOrRecycled.assetMask0, newOrRecycled.assetMask1, newOrRecycled.assetMask2, newOrRecycled.assetMask3 };
		for (int i = 0; i < array.Length; i++)
		{
			SetMaskSortingOrder(array[i], uILayerID, num, uILayerID, num - 1);
		}
		SortingGroup sortingGroup = characterDisplay.gameObject.AddComponent<SortingGroup>();
		if (sortingGroup != null && sortingGroup.enabled)
		{
			sortingGroup.sortingLayerName = "UI";
			sortingGroup.sortingOrder = num;
		}
		static void SetMaskSortingOrder(SpriteMask mask, int frontSortingLayerID, int frontSortingOrder, int backSortingLayerID, int backSortingOrder)
		{
			if (!(mask == null))
			{
				mask.frontSortingLayerID = frontSortingLayerID;
				mask.frontSortingOrder = frontSortingOrder;
				mask.backSortingLayerID = backSortingLayerID;
				mask.backSortingOrder = backSortingOrder;
			}
		}
	}

	private void OnTileSelected(Button UIOnlineSelect, Button UIOfflineSelect, FriendRecommendItem item)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		Util.SetButton(UIOnlineSelect, enabled: false);
		Util.SetButton(UIOfflineSelect, enabled: false);
		item.invited = true;
		GameData.instance.PROJECT.DoSendRequestByID(item.characterData.charID);
	}

	private void OnTileClicked(FriendRecommendItem model)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowPlayer(model.characterData.charID, -1, null, friendRecommendWindow.gameObject);
	}

	protected override void OnBeforeRecycleOrDisableViewsHolder(MyListItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		inRecycleBinOrVisible.UIOfflineSelect.onClick.RemoveAllListeners();
		inRecycleBinOrVisible.UIOnlineSelect.onClick.RemoveAllListeners();
		inRecycleBinOrVisible.UIFrame.onClick.RemoveAllListeners();
		inRecycleBinOrVisible.UIFrame.gameObject.GetComponent<HoverImage>().OnExit();
		CharacterDisplay componentInChildren = inRecycleBinOrVisible.root.GetComponentInChildren<CharacterDisplay>();
		if (componentInChildren != null)
		{
			Object.Destroy(componentInChildren.gameObject);
		}
		base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<FriendRecommendItem> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<FriendRecommendItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		FriendRecommendItem[] newItems = new FriendRecommendItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		FriendRecommendItem[] newItems = new FriendRecommendItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(FriendRecommendItem[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
