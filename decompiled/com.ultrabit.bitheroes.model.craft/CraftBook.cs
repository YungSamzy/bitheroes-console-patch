using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.consumable;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.craft;

public class CraftBook
{
	private static List<CraftUpgradeRef> _upgrades;

	private static List<CraftReforgeRef> _reforges;

	private static List<CraftRerollRef> _rerolls;

	private static List<CraftTradeRef> _trades;

	public static int sizeTrades => _trades.Count;

	public static List<CraftUpgradeRef> upgrades => _upgrades;

	public static List<CraftReforgeRef> reforges => _reforges;

	public static List<CraftRerollRef> rerolls => _rerolls;

	public static List<CraftTradeRef> trades => _trades;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_upgrades = new List<CraftUpgradeRef>();
		_reforges = new List<CraftReforgeRef>();
		_rerolls = new List<CraftRerollRef>();
		_trades = new List<CraftTradeRef>();
		int index = 0;
		foreach (CraftBookData.Upgrade item in XMLBook.instance.craftBook.upgrades.lstUpgrade)
		{
			_upgrades.Add(new CraftUpgradeRef(index, item));
			index++;
			if (XMLBook.instance.UpdateProcessingCount())
			{
				yield return null;
			}
		}
		index = 0;
		foreach (CraftBookData.Reforge item2 in XMLBook.instance.craftBook.reforges.lstReforge)
		{
			_reforges.Add(new CraftReforgeRef(index, item2));
			index++;
			if (XMLBook.instance.UpdateProcessingCount())
			{
				yield return null;
			}
		}
		index = 0;
		foreach (CraftBookData.Reroll item3 in XMLBook.instance.craftBook.rerolls.lstReroll)
		{
			_rerolls.Add(new CraftRerollRef(index, item3));
			index++;
			if (XMLBook.instance.UpdateProcessingCount())
			{
				yield return null;
			}
		}
		index = 0;
		foreach (CraftBookData.Trade item4 in XMLBook.instance.craftBook.trades.lstTrade)
		{
			CraftTradeRef craftTradeRef = new CraftTradeRef(index, item4);
			_trades.Add(craftTradeRef);
			foreach (ItemData requiredItem in craftTradeRef.tradeRef.requiredItems)
			{
				requiredItem.itemRef.addTrade(craftTradeRef);
			}
			index++;
			if (XMLBook.instance.UpdateProcessingCount())
			{
				yield return null;
			}
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static CraftTradeRef LookupTrade(int id)
	{
		if (id < 0 || id >= sizeTrades)
		{
			return null;
		}
		return _trades[id];
	}

	public static CraftUpgradeRef LookupUpgradeLink(string link)
	{
		foreach (CraftUpgradeRef upgrade in _upgrades)
		{
			if (upgrade.link.ToLower().Equals(link.ToLower()))
			{
				return upgrade;
			}
		}
		return null;
	}

	public static CraftReforgeRef LookupReforge(int id)
	{
		foreach (CraftReforgeRef reforge in _reforges)
		{
			if (reforge.id == id)
			{
				return reforge;
			}
		}
		return null;
	}

	public static CraftReforgeRef LookupReforgeLink(string link)
	{
		if (_reforges == null)
		{
			return null;
		}
		if (link == null)
		{
			D.LogWarning("david", "CraftBook::LookupReforgeLink link is null");
			return null;
		}
		foreach (CraftReforgeRef reforge in _reforges)
		{
			if (reforge.link != null && reforge.link.ToLower().Equals(link.ToLower()))
			{
				return reforge;
			}
		}
		return null;
	}

	public static List<CraftTradeRef> GetTradeRefsByType(int type)
	{
		List<CraftTradeRef> list = new List<CraftTradeRef>();
		int num = sizeTrades;
		for (int i = 0; i < num; i++)
		{
			CraftTradeRef craftTradeRef = LookupTrade(i);
			if (craftTradeRef == null || (craftTradeRef.gameRequirement != null && !craftTradeRef.gameRequirement.RequirementsMet()))
			{
				continue;
			}
			bool flag = true;
			foreach (ItemRef craftingRequiredItem in craftTradeRef.craftingRequiredItems)
			{
				if (!GameData.instance.PROJECT.character.inventory.hasOwnedItem(craftingRequiredItem))
				{
					flag = false;
					break;
				}
			}
			if (flag && craftTradeRef.type == type)
			{
				list.Add(craftTradeRef);
			}
		}
		return list;
	}

	public static List<CraftTradeRef> GetItemTradeRefs(ItemRef itemRef)
	{
		List<CraftTradeRef> list = new List<CraftTradeRef>();
		int num = sizeTrades;
		for (int i = 0; i < num; i++)
		{
			CraftTradeRef craftTradeRef = LookupTrade(i);
			if (craftTradeRef == null || (craftTradeRef.gameRequirement != null && !craftTradeRef.gameRequirement.RequirementsMet()))
			{
				continue;
			}
			bool flag = true;
			foreach (ItemRef craftingRequiredItem in craftTradeRef.craftingRequiredItems)
			{
				if (!GameData.instance.PROJECT.character.inventory.hasOwnedItem(craftingRequiredItem))
				{
					flag = false;
					break;
				}
			}
			if (!flag)
			{
				continue;
			}
			foreach (ItemData requiredItem in craftTradeRef.tradeRef.requiredItems)
			{
				if (requiredItem.itemRef == itemRef)
				{
					list.Add(craftTradeRef);
					break;
				}
			}
		}
		return list;
	}

	public static CraftRerollRef GetItemRerollRef(ItemRef itemRef)
	{
		foreach (CraftRerollRef reroll in _rerolls)
		{
			if (reroll.itemType == itemRef.itemType && (reroll.itemSubtype < 0 || reroll.itemSubtype == itemRef.subtype) && reroll.rarityRef.Equals(itemRef.rarityRef))
			{
				return reroll;
			}
		}
		foreach (CraftRerollRef reroll2 in _rerolls)
		{
			if (reroll2.rarityRef.Equals(itemRef.rarityRef))
			{
				return reroll2;
			}
		}
		return null;
	}

	public static CraftReforgeRef getItemReforgeRef(ItemRef itemRef, ItemRef targetRef)
	{
		if (itemRef.reforges != null)
		{
			foreach (ItemReforgeRef reforge in itemRef.reforges)
			{
				if (reforge.itemRef.Equals(targetRef))
				{
					return reforge.reforgeRef;
				}
			}
		}
		foreach (CraftReforgeRef reforge2 in _reforges)
		{
			if ((reforge2.rank < 0 || reforge2.rank == itemRef.rank) && reforge2.rarityRef.id == itemRef.rarityRef.id)
			{
				return reforge2;
			}
		}
		return null;
	}

	public static List<CraftTradeRef> GetItemsByCrafter(string crafter)
	{
		List<CraftTradeRef> list = new List<CraftTradeRef>();
		for (int i = 0; i < sizeTrades; i++)
		{
			CraftTradeRef craftTradeRef = LookupTrade(i);
			if (craftTradeRef != null && (craftTradeRef.gameRequirement == null || craftTradeRef.gameRequirement.RequirementsMet()) && !(craftTradeRef.crafter != crafter))
			{
				list.Add(craftTradeRef);
			}
		}
		return list;
	}

	public static List<CraftTradeRef> GetItemsRevealedByCrafter(string crafter)
	{
		List<CraftTradeRef> list = new List<CraftTradeRef>();
		for (int i = 0; i < sizeTrades; i++)
		{
			CraftTradeRef craftTradeRef = LookupTrade(i);
			if (craftTradeRef == null || (craftTradeRef.gameRequirement != null && !craftTradeRef.gameRequirement.RequirementsMet()) || craftTradeRef.crafter != crafter)
			{
				continue;
			}
			bool flag = true;
			foreach (ItemRef craftingRequiredItem in craftTradeRef.craftingRequiredItems)
			{
				if (!GameData.instance.PROJECT.character.inventory.hasOwnedItem(craftingRequiredItem))
				{
					flag = false;
				}
			}
			if (flag)
			{
				list.Add(craftTradeRef);
			}
		}
		return list;
	}

	public static List<CraftTradeRef> getItemsByResultType(int itemType)
	{
		List<CraftTradeRef> list = new List<CraftTradeRef>();
		foreach (CraftTradeRef trade in _trades)
		{
			if (trade != null && trade.tradeRef != null && (trade.gameRequirement == null || trade.gameRequirement.RequirementsMet()) && (trade.tradeRef.resultItem.itemRef.itemType == itemType || (trade.tradeRef.resultItem.itemRef.itemType == 4 && (trade.tradeRef.resultItem.itemRef.itemType != 4 || (trade.tradeRef.resultItem.itemRef as ConsumableRef).consumableItemType == itemType))))
			{
				list.Add(trade);
			}
		}
		return list;
	}
}
