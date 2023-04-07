using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.dialog;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.playervoting;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.game;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.news;
using com.ultrabit.bitheroes.ui.promo;
using com.ultrabit.bitheroes.ui.utility;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.lists.news;

public class NewsList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	private NewsWindow _newsWindow;

	public int currentItemIndex;

	public SimpleDataHelper<NewsItemModel> Data { get; private set; }

	public void InitList(NewsWindow newsWindow)
	{
		_newsWindow = newsWindow;
		if (Data == null)
		{
			Data = new SimpleDataHelper<NewsItemModel>(this);
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

	public void ScrollToPage(int index, bool directo = false)
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
			SmoothScrollTo(0, 0.2f, 0.5f, 0.5f, null, null, overrideCurrentScrollingAnimation: true);
			currentItemIndex = 0;
		}
		else if (currentItemIndex < 0)
		{
			SmoothScrollTo(Data.Count - 1, 0.2f, 0.5f, 0.5f, null, null, overrideCurrentScrollingAnimation: true);
			currentItemIndex = Data.Count - 1;
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
		NewsItemModel model = Data[newOrRecycled.ItemIndex];
		int num = _newsWindow.sortingLayer + 1;
		Transform obj = Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<Transform>("ui/promo/" + typeof(PromoObject).Name));
		obj.SetParent(newOrRecycled.placeholderPromo.transform, worldPositionStays: false);
		PromoObject component = obj.GetComponent<PromoObject>();
		component.LoadDetails(model.newsRef.promoRef, newOrRecycled.placeholderPromo.transform, PromoObject.TYPE.NEWS);
		component.CreateAssets(num);
		Util.ChangeChildrenParticleSystemMaskInteraction(obj.gameObject, SpriteMaskInteraction.VisibleInsideMask);
		foreach (GameTextRef text in model.newsRef.texts)
		{
			Transform obj2 = Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<Transform>("ui/game/" + typeof(GameText).Name));
			obj2.SetParent(newOrRecycled.placeholderPromo.transform, worldPositionStays: false);
			obj2.GetComponent<GameText>().LoadDetails(text, 4);
			Util.ChangeLayer(obj2, "UI");
			SortingGroup sortingGroup = obj2.gameObject.AddComponent<SortingGroup>();
			if (sortingGroup != null && sortingGroup.enabled)
			{
				sortingGroup.sortingLayerName = "UI";
				sortingGroup.sortingOrder = num;
			}
		}
		ItemRef itemRef = null;
		switch (model.newsRef.type)
		{
		case 2:
		{
			string[] array = model.newsRef.value.Split(',');
			if (array.Length == 2)
			{
				int id = int.Parse(array[0]);
				int itemType = ItemRef.getItemType(array[1]);
				itemRef = ItemBook.Lookup(id, itemType);
			}
			break;
		}
		case 3:
			model._requirement = VariableBook.GetGameRequirement(6);
			break;
		case 4:
			model._requirement = VariableBook.GetGameRequirement(1);
			break;
		case 5:
			model._requirement = VariableBook.GetGameRequirement(0);
			break;
		case 6:
			model._requirement = VariableBook.GetGameRequirement(3);
			break;
		case 9:
			model._requirement = VariableBook.GetGameRequirement(23);
			break;
		case 10:
			model._requirement = VariableBook.GetGameRequirement(30);
			break;
		case 11:
			model._requirement = VariableBook.GetGameRequirement(8);
			break;
		case 12:
			model._requirement = VariableBook.GetGameRequirement(31);
			break;
		case 13:
			model._requirement = VariableBook.GetGameRequirement(33);
			break;
		case 14:
			model._requirement = VariableBook.GetGameRequirement(32);
			break;
		case 19:
			model._requirement = VariableBook.GetGameRequirement(36);
			break;
		case 20:
			model._requirement = VariableBook.GetGameRequirement(37);
			break;
		}
		newOrRecycled.root.GetComponent<Button>().enabled = false;
		HoverImages hoverImages = newOrRecycled.root.GetComponent<HoverImages>();
		if (hoverImages == null)
		{
			hoverImages = newOrRecycled.root.gameObject.AddComponent<HoverImages>();
		}
		hoverImages.ForceStart();
		hoverImages.GetOwnTexts();
		hoverImages.active = itemRef != null || model.newsRef.type != 0;
		if (itemRef != null)
		{
			ItemIcon itemIcon = newOrRecycled.root.gameObject.GetComponent<ItemIcon>();
			if (itemIcon == null)
			{
				itemIcon = newOrRecycled.root.gameObject.AddComponent<ItemIcon>();
			}
			itemIcon.SetItemData(new ItemData(itemRef, 1), locked: false, 1);
			itemIcon.SetItemActionType(itemRef.hasContents() ? 7 : 4);
		}
		else if (model.newsRef.type != 0)
		{
			newOrRecycled.root.GetComponent<Button>().enabled = true;
			newOrRecycled.root.GetComponent<Button>().onClick.AddListener(delegate
			{
				OnNewsClick(model);
			});
		}
		for (int i = 0; i < Data.Count; i++)
		{
			if (Data.Count == _Params.obj[0].transform.childCount)
			{
				_Params.obj[0].transform.GetChild(i).GetComponent<NewsDotClick>().SetID(i);
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
		_Params.obj[0].transform.parent.GetComponent<Canvas>().sortingOrder = num + component.assetsCount;
	}

	private void OnNewsClick(NewsItemModel model)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (model._requirement != null)
		{
			string requirementsText = model._requirement.GetRequirementsText();
			if (requirementsText != null)
			{
				DialogRef dialogLocked = model._requirement.GetDialogLocked();
				if (dialogLocked != null)
				{
					GameData.instance.windowGenerator.NewDialogPopup(dialogLocked);
				}
				else
				{
					GameData.instance.windowGenerator.ShowError(requirementsText);
				}
				return;
			}
		}
		switch (model.newsRef.type)
		{
		case 1:
			Util.OpenURL(model.newsRef.value);
			break;
		case 3:
			if (GameData.instance.PROJECT.CheckGameRequirement(6))
			{
				GameData.instance.PROJECT.ShowShopWindow();
			}
			break;
		case 4:
			if (GameData.instance.PROJECT.CheckGameRequirement(1))
			{
				GameData.instance.PROJECT.ShowPvPWindow();
			}
			break;
		case 5:
			GameData.instance.PROJECT.ShowZoneWindow();
			break;
		case 6:
			if (GameData.instance.PROJECT.CheckGameRequirement(3))
			{
				GameData.instance.PROJECT.ShowRaidWindow();
			}
			break;
		case 7:
			if (GameData.instance.PROJECT.CheckGameRequirement(19))
			{
				GameData.instance.PROJECT.ShowFusionWindow();
			}
			break;
		case 8:
			if (GameData.instance.PROJECT.CheckGameRequirement(21))
			{
				GameData.instance.PROJECT.ShowRiftWindow();
			}
			break;
		case 9:
			if (GameData.instance.PROJECT.CheckGameRequirement(23))
			{
				GameData.instance.PROJECT.ShowGauntletWindow();
			}
			break;
		case 10:
			if (GameData.instance.PROJECT.CheckGameRequirement(30))
			{
				GameData.instance.PROJECT.ShowGvGWindow();
			}
			break;
		case 11:
			if (GameData.instance.PROJECT.CheckGameRequirement(8))
			{
				GameData.instance.PROJECT.ShowGuildWindow();
			}
			break;
		case 12:
			if (GameData.instance.PROJECT.CheckGameRequirement(31))
			{
				GameData.instance.PROJECT.ShowFamiliarStableWindow();
			}
			break;
		case 13:
			if (GameData.instance.PROJECT.CheckGameRequirement(33))
			{
				GameData.instance.PROJECT.ShowInvasionWindow();
			}
			break;
		case 14:
			if (GameData.instance.PROJECT.CheckGameRequirement(32))
			{
				GameData.instance.PROJECT.ShowEnchantsWindow();
			}
			break;
		case 15:
			if (GameData.instance.PROJECT.CheckGameRequirement(34))
			{
				GameData.instance.PROJECT.ShowBrawlWindow();
			}
			break;
		case 16:
			GameData.instance.windowGenerator.ShowServices(0);
			break;
		case 17:
			if (GameData.instance.PROJECT.CheckGameRequirement(25))
			{
				GameData.instance.PROJECT.ShowFishingWindow();
			}
			break;
		case 18:
			if (GameData.instance.PROJECT.CheckGameRequirement(35))
			{
				GameData.instance.PROJECT.ShowGvEWindow();
			}
			break;
		case 19:
			if (GameData.instance.PROJECT.CheckGameRequirement(36))
			{
				GameData.instance.PROJECT.ShowAugmentsWindow();
			}
			break;
		case 20:
			if (GameData.instance.PROJECT.CheckGameRequirement(37))
			{
				if (PlayerVotingBook.activeVoting)
				{
					GameData.instance.windowGenerator.NewPlayerVotingWindow();
				}
				else
				{
					GameData.instance.windowGenerator.ShowError(Language.GetString("voting_no_active_voting"));
				}
			}
			break;
		case 2:
			break;
		}
	}

	protected override void OnBeforeRecycleOrDisableViewsHolder(MyListItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		inRecycleBinOrVisible.root.GetComponent<HoverImages>().OnExit();
		base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
		inRecycleBinOrVisible.root.GetComponent<Button>().onClick.RemoveAllListeners();
		if (inRecycleBinOrVisible.placeholderPromo.childCount > 0)
		{
			for (int i = 0; i < inRecycleBinOrVisible.placeholderPromo.childCount; i++)
			{
				Object.Destroy(inRecycleBinOrVisible.placeholderPromo.GetChild(i).gameObject);
			}
		}
	}

	public void AddItemsAt(int index, IList<NewsItemModel> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<NewsItemModel> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		NewsItemModel[] newItems = new NewsItemModel[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		NewsItemModel[] newItems = new NewsItemModel[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(NewsItemModel[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
