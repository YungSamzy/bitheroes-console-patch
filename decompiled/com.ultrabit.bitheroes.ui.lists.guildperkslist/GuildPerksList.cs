using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.guild;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.utility;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.guildperkslist;

public class GuildPerksList : OSA<BaseParamsWithPrefab, MyListItemViewsHolder>
{
	private GuildPerkItem currentItem;

	public SimpleDataHelper<GuildPerkItem> Data { get; private set; }

	public void InitList()
	{
		if (Data == null)
		{
			Data = new SimpleDataHelper<GuildPerkItem>(this);
			base.Start();
		}
	}

	protected override void Start()
	{
		InitList();
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
		GuildPerkItem model = Data[newOrRecycled.ItemIndex];
		newOrRecycled.UIUpgrade.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_upgrade");
		newOrRecycled.UIUpgrade.onClick.AddListener(delegate
		{
			DoPerkUpgradeConfirm(model);
		});
		newOrRecycled.UIItemIcon.overrideSprite = model.perkRef.GetSpriteIcon();
		int perkRank = GameData.instance.PROJECT.character.guildData.perks.getPerkRank(model.perkRef.id);
		GuildPerkRankRef perkRank2 = model.perkRef.getPerkRank(perkRank);
		GameModifier gameModifier = perkRank2?.getFirstModifier();
		if (gameModifier != null)
		{
			newOrRecycled.UICurrentModifier.GetComponentInChildren<TextMeshProUGUI>().text = ((gameModifier.desc != null) ? Util.ParseModifierString(gameModifier) : GameModifier.getTypeDescriptionShort(gameModifier.type, gameModifier.value));
			Util.SetButton(newOrRecycled.UICurrentModifier);
			newOrRecycled.UICurrentModifier.GetComponent<HoverImage>().enabled = true;
		}
		else
		{
			newOrRecycled.UICurrentModifier.GetComponentInChildren<TextMeshProUGUI>().text = "-";
			Util.SetButton(newOrRecycled.UICurrentModifier, enabled: false);
			newOrRecycled.UICurrentModifier.GetComponent<HoverImage>().enabled = false;
		}
		int num = perkRank + 1;
		GuildPerkRankRef perkRank3 = model.perkRef.getPerkRank(num);
		GameModifier gameModifier2 = perkRank3?.getFirstModifier();
		if (gameModifier2 != null)
		{
			newOrRecycled.UINextModifier.GetComponentInChildren<TextMeshProUGUI>().text = ((gameModifier2.desc != null) ? Util.ParseModifierString(gameModifier2) : GameModifier.getTypeDescriptionShort(gameModifier2.type, gameModifier2.value));
			Util.SetButton(newOrRecycled.UINextModifier);
			newOrRecycled.UINextModifier.GetComponent<HoverImage>().enabled = true;
		}
		else
		{
			newOrRecycled.UINextModifier.GetComponentInChildren<TextMeshProUGUI>().text = "-";
			Util.SetButton(newOrRecycled.UINextModifier, enabled: false);
			newOrRecycled.UINextModifier.GetComponent<HoverImage>().enabled = false;
		}
		newOrRecycled.UICost.text = ((perkRank3 != null) ? Util.NumberFormat(perkRank3.cost) : "-");
		newOrRecycled.UIRank.text = Util.NumberFormat(perkRank2.id);
		newOrRecycled.UICurrentName.text = Language.GetString("ui_rank_count", new string[1] { Util.NumberFormat(perkRank) });
		newOrRecycled.UINextName.text = ((perkRank3 != null) ? Language.GetString("ui_rank_count", new string[1] { Util.NumberFormat(num) }) : Language.GetString("ui_max"));
		if (perkRank3 != null && model.perkWindow.guildData.points >= perkRank3.cost && GameData.instance.PROJECT.character.guildData.hasPermission(7))
		{
			Util.SetButton(newOrRecycled.UIUpgrade);
		}
		else
		{
			Util.SetButton(newOrRecycled.UIUpgrade, enabled: false);
		}
	}

	public void DoPerkUpgradeConfirm(GuildPerkItem item)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		int perkRank = GameData.instance.PROJECT.character.guildData.perks.getPerkRank(item.perkRef.id);
		if (item.perkRef.getPerkRank(perkRank) == null)
		{
			return;
		}
		int rank = perkRank + 1;
		GuildPerkRankRef perkRank2 = item.perkRef.getPerkRank(rank);
		if (perkRank2 == null)
		{
			return;
		}
		CurrencyRef currencyRef = CurrencyBook.Lookup(11);
		if (!(currencyRef == null))
		{
			GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_upgrade_perk_confirm", new string[2]
			{
				Util.NumberFormat(perkRank2.cost),
				currencyRef.coloredName
			}), null, null, delegate
			{
				DoPerkUpgrade(item);
			});
		}
	}

	private void DoPerkUpgrade(GuildPerkItem item)
	{
		GameData.instance.main.ShowLoading();
		currentItem = item;
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(36), OnPerkUpgrade);
		GuildDALC.instance.doPerkUpgrade(item.perkRef.id);
	}

	private void OnPerkUpgrade(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(36), OnPerkUpgrade);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		GuildData data = GuildData.fromSFSObject(sfsob);
		GuildPerks perks = GuildPerks.fromSFSObject(sfsob);
		GameData.instance.PROJECT.character.guildData.setPerks(perks);
		if (Data != null && Data.Count > 0)
		{
			Data[1].perkWindow.guildData.updateData(data);
			Data[1].perkWindow.DoUpdate();
		}
	}

	protected override void OnBeforeRecycleOrDisableViewsHolder(MyListItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
	{
		inRecycleBinOrVisible.UIUpgrade.onClick.RemoveAllListeners();
		base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
	}

	public void AddItemsAt(int index, IList<GuildPerkItem> items)
	{
		Data.InsertItems(index, items);
	}

	public void RemoveItemsFrom(int index, int count)
	{
		Data.RemoveItems(index, count);
	}

	public void SetItems(IList<GuildPerkItem> items)
	{
		Data.ResetItems(items);
	}

	private void RetrieveDataAndUpdate(int count)
	{
		GuildPerkItem[] newItems = new GuildPerkItem[count];
		OnDataRetrieved(newItems);
	}

	private IEnumerator FetchMoreItemsFromDataSourceAndUpdate(int count)
	{
		yield return new WaitForSeconds(0.5f);
		GuildPerkItem[] newItems = new GuildPerkItem[count];
		OnDataRetrieved(newItems);
	}

	private void OnDataRetrieved(GuildPerkItem[] newItems)
	{
		Data.InsertItemsAtEnd(newItems);
	}
}
