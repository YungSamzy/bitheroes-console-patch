using System;
using com.ultrabit.bitheroes.model.fishing;
using com.ultrabit.bitheroes.model.item;

namespace com.ultrabit.bitheroes.model.data;

public class FishingShopItemRefModelData : BaseModelData
{
	private FishingShopItemRef _fishingShopItemRef;

	private int _qty;

	public FishingShopItemRef fishingShopItemRef => _fishingShopItemRef;

	public override ItemRef itemRef => fishingShopItemRef.itemData.itemRef;

	public override int power => 0;

	public override int stamina => 0;

	public override int agility => 0;

	public override object data => null;

	public override int qty
	{
		get
		{
			return _qty;
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public override int type => fishingShopItemRef.itemData.itemRef.itemType;

	public FishingShopItemRefModelData(FishingShopItemRef fishingShopItemRef, int qty)
	{
		_fishingShopItemRef = fishingShopItemRef;
		_qty = qty;
	}
}
