using System.Diagnostics;
using com.ultrabit.bitheroes.model.item;

namespace com.ultrabit.bitheroes.model.eventsales;

[DebuggerDisplay("{name} (EventSalesShopItemRef)")]
public class EventSalesShopItemRef
{
	private int _id;

	private ItemData _itemData;

	private int _cost;

	private int _purchaseRemainingQty = -1;

	private int _purchaseLimit;

	public int id => _id;

	public ItemData itemData => _itemData;

	private string name => _itemData.itemRef.name;

	public int cost => _cost;

	public int purchaseRemainingQty
	{
		get
		{
			return _purchaseRemainingQty;
		}
		set
		{
			_purchaseRemainingQty = value;
		}
	}

	public int purchaseLimit => _purchaseLimit;

	public EventSalesShopItemRef(int id, ItemData itemData, int cost, int purchaseLimit)
	{
		_id = id;
		_itemData = itemData;
		_cost = cost;
		_purchaseLimit = (itemData.itemRef.unique ? 1 : purchaseLimit);
	}
}
