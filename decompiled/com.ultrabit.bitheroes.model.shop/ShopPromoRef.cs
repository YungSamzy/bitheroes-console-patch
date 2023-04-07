using System;
using System.Diagnostics;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.promo;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.shop;

[DebuggerDisplay("{name} (ShopPromoRef)")]
public class ShopPromoRef : PromoRef, IEquatable<ShopPromoRef>, IComparable<ShopPromoRef>
{
	private ItemRef _itemRef;

	private int _serviceTab;

	private string name => itemRef.name;

	public ItemRef itemRef => _itemRef;

	public int serviceTab => _serviceTab;

	public ShopPromoRef(int id, ShopBookData.Promo promoData)
		: base(id, promoData)
	{
		_itemRef = ItemBook.Lookup(promoData.id, promoData.type);
		_serviceTab = Util.GetIntFromStringProperty(promoData.promoServiceTab, -1);
	}

	public bool Equals(ShopPromoRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(ShopPromoRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
