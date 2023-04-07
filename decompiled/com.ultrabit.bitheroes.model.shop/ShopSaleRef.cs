using System;
using com.ultrabit.bitheroes.model.date;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.shop;

public class ShopSaleRef
{
	private ItemRef _itemRef;

	private DateRef _dateRef;

	private ShopBookData.Item itemData;

	private int _id;

	public int id => _id;

	public float mult => Util.GetFloatFromStringProperty(itemData.mult);

	public ItemRef itemRef => _itemRef;

	public DateRef dateRef => _dateRef;

	public ShopSaleRef(int id, ShopBookData.Item itemData)
	{
		_id = id;
		this.itemData = itemData;
		_itemRef = ItemBook.Lookup(itemData.id, itemData.type);
		if (itemData.startDate != null && itemData.endDate != null)
		{
			_dateRef = new DateRef(itemData.startDate, itemData.endDate);
		}
	}

	public bool getActive(DateTime? date = null)
	{
		if (_dateRef != null)
		{
			return _dateRef.getActive(date);
		}
		return true;
	}
}
