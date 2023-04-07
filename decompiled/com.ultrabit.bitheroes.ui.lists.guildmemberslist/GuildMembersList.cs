using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.adgor;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.guild;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.guild;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using UnityEngine;
using UnityEngine.Rendering;

namespace com.ultrabit.bitheroes.ui.lists.guildmemberslist;

public class GuildMembersList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	private GuildWindow guildWindow;

	public SimpleDataHelper<GuildMemberItem> Data { get; private set; }

	public void InitList(GuildWindow guildWindow)
	{
		this.guildWindow = guildWindow;
		if (Data == null)
		{
			Data = new SimpleDataHelper<GuildMemberItem>(this);
			base.Start();
		}
	}

	public void ClearList()
	{
		int i;
		for (i = 0; i < Data.Count; i++)
		{
			if (Data[i].memberData.eventAdded)
			{
				Data[i].memberData.OnChange.RemoveListener(delegate
				{
					OnChange(Data[i]);
				});
			}
		}
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
		GuildMemberItem model = Data[newOrRecycled.ItemIndex];
		model.memberData.OnChange.AddListener(delegate
		{
			OnChange(model);
		});
		model.memberData.eventAdded = true;
		newOrRecycled.UIMutinyTxt.text = Language.GetString("ui_mutiny");
		newOrRecycled.UIFrame.onClick.AddListener(delegate
		{
			OnFrameClick(model);
		});
		newOrRecycled.UIMutiny.onClick.AddListener(delegate
		{
			OnMutinyClick(model);
		});
		newOrRecycled.UIFrameImage.color = (model.memberData.characterData.isIMXG0 ? model.memberData.characterData.nftRarityColor : Color.white);
		newOrRecycled.avatarGenerationBanner.gameObject.SetActive(model.memberData.characterData.isIMXG0);
		newOrRecycled.avatarBackground.gameObject.SetActive(model.memberData.characterData.isIMXG0);
		if (model.memberData.characterData.isIMXG0)
		{
			newOrRecycled.avatarBackground.LoadDetails(model.memberData.characterData.nftBackground, model.memberData.characterData.nftFrameSimple, model.memberData.characterData.nftFrameSeparator);
			newOrRecycled.avatarGenerationBanner.LoadDetails(model.memberData.characterData.nftGeneration, model.memberData.characterData.nftRarity);
		}
		CharacterDisplay characterDisplay = model.memberData.characterData.toCharacterDisplay(0.65f, displayMount: false, null, enableLoading: false);
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
		int num = 2 + newOrRecycled.ItemIndex + guildWindow.sortingLayer;
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
		newOrRecycled.UILevel.text = Language.GetString("ui_current_lvl", new string[1] { Util.NumberFormat(model.memberData.characterData.level) });
		newOrRecycled.UIRank.text = Guild.getRankColoredName(model.memberData.rank);
		newOrRecycled.UIName.text = model.memberData.characterData.name;
		if (model.memberData.characterData.hasVipgor)
		{
			newOrRecycled.UIName.text = AdGor.GetNameplateColorString(model.memberData.characterData.name);
		}
		newOrRecycled.UILogin.text = model.memberData.getLoginText();
		newOrRecycled.UIOnline.gameObject.SetActive(model.memberData.online);
		newOrRecycled.UIOffline.gameObject.SetActive(model.memberData.offline);
		float num2 = GameData.instance.PROJECT.character.guildData.getMutinyMilliseconds();
		newOrRecycled.UIMutiny.gameObject.SetActive(model.memberData.rank == 0 && num2 > 0f && model.memberData.loginMilliseconds >= num2);
	}

	public void OnChange(GuildMemberItem model)
	{
		if (model.guildMembersPanel != null)
		{
			model.guildMembersPanel.OnGuildMemberChange();
		}
	}

	public void OnFrameClick(GuildMemberItem model)
	{
		D.Log("___playerID: " + model.memberData.characterData.playerID);
		D.Log("___charID: " + model.memberData.characterData.charID);
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewGuildMemberWindow(model.memberData);
	}

	public void OnMutinyClick(GuildMemberItem model)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_mutiny_confirm", new string[1] { model.memberData.characterData.name }), null, null, delegate
		{
			DoMutiny();
		});
	}

	private void DoMutiny()
	{
		GameData.instance.main.ShowLoading();
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(32), OnMutiny);
		GuildDALC.instance.doMutiny();
	}

	private void OnMutiny(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(32), OnMutiny);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
	}

	protected override void OnBeforeRecycleOrDisableViewsHolder(MyListItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		if (Data.Count > 0 && Data[inRecycleBinOrVisible.ItemIndex] != null)
		{
			Data[inRecycleBinOrVisible.ItemIndex].memberData.OnChange.RemoveListener(delegate
			{
				OnChange(Data[inRecycleBinOrVisible.ItemIndex]);
			});
		}
		inRecycleBinOrVisible.UIFrame.onClick.RemoveAllListeners();
		inRecycleBinOrVisible.UIMutiny.onClick.RemoveAllListeners();
		CharacterDisplay componentInChildren = inRecycleBinOrVisible.root.GetComponentInChildren<CharacterDisplay>();
		if (componentInChildren != null)
		{
			Object.Destroy(componentInChildren.gameObject);
		}
		base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<GuildMemberItem> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<GuildMemberItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		GuildMemberItem[] newItems = new GuildMemberItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		GuildMemberItem[] newItems = new GuildMemberItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(GuildMemberItem[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
