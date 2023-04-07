using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.book;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.craft;

[DebuggerDisplay("{resultItemName} (CraftTradeRef)")]
public class CraftTradeRef : BaseRef, IEquatable<CraftTradeRef>, IComparable<CraftTradeRef>
{
	public const string CRAFTER_NONE = "NONE";

	public const string CRAFTER_MYTHIC_AUGMENT = "mythic_augment";

	public const string CRAFTER_MYTHIC_ENCHANT = "mythic_enchant";

	public const string CRAFTER_MYTHIC_RUNE = "mythic_rune";

	public const string CRAFTER_NPC_SETS = "npc_sets";

	public const string CRAFTER_ARMORY_FORGE = "armory_forge";

	public const string CRAFTER_EVENT_SHOP_EXCHANGE = "event_shop_exchange";

	public const string VERSION_OLD = "OLD";

	public const int TRADE_TYPE_NONE = 0;

	public const int TRADE_TYPE_MOUNT = 1;

	private static Dictionary<string, int> TRADE_TYPES = new Dictionary<string, int>
	{
		["none"] = 0,
		["mount"] = 1
	};

	private ItemTradeRef _tradeRef;

	private GameRequirement _gameRequirement;

	private List<ItemRef> _craftingRequiredItems;

	private string _crafter;

	private int _type;

	private bool _isOld;

	private string resultItemName => tradeRef.resultItem.itemRef.name;

	public int type => _type;

	public ItemTradeRef tradeRef => _tradeRef;

	public GameRequirement gameRequirement => _gameRequirement;

	public List<ItemRef> craftingRequiredItems => _craftingRequiredItems;

	public string crafter => _crafter;

	public bool isOld => _isOld;

	public CraftTradeRef(int id, CraftBookData.Trade tradeData)
		: base(id)
	{
		if (tradeData.requirement != null && !tradeData.requirement.Equals(""))
		{
			_gameRequirement = VariableBook.GetGameRequirement(GameRequirement.GetType(tradeData.requirement));
		}
		List<ItemData> list = new List<ItemData>();
		foreach (CraftBookData.Item item in tradeData.requirements.lstItem)
		{
			list.Add(new ItemData(ItemBook.Lookup(item.id, ItemRef.getItemType(item.type)), int.Parse(item.qty)));
		}
		ItemData resultItem = new ItemData(ItemBook.Lookup(tradeData.result.item.id, tradeData.result.item.type), tradeData.result.item.qty);
		_tradeRef = new ItemTradeRef(list, resultItem);
		_craftingRequiredItems = new List<ItemRef>();
		if (tradeData.lstReveal.Count > 0)
		{
			foreach (CraftBookData.Reveal item2 in tradeData.lstReveal)
			{
				_craftingRequiredItems.Add(ItemBook.Lookup(item2.id, item2.type));
			}
		}
		_crafter = ((tradeData.crafter != null) ? tradeData.crafter : "NONE");
		_type = GetTradeType(tradeData.type);
		_isOld = tradeData.version == "OLD";
		LoadDetails(tradeData);
	}

	public static int GetTradeType(string type)
	{
		if (type == null)
		{
			return 0;
		}
		if (TRADE_TYPES.ContainsKey(type))
		{
			return TRADE_TYPES[type.ToLowerInvariant()];
		}
		return 0;
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}

	public bool Equals(CraftTradeRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(CraftTradeRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
