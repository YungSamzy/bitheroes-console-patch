using System;
using com.ultrabit.bitheroes.model.guild;
using com.ultrabit.bitheroes.model.item;

namespace com.ultrabit.bitheroes.model.data;

public class GuildShopRefModelData : BaseModelData
{
	private GuildShopRef _guildShopRef;

	private int _qty;

	public GuildShopRef guildShopRef => _guildShopRef;

	public override ItemRef itemRef => guildShopRef.itemRef;

	public override int power => 0;

	public override int stamina => 0;

	public override int agility => 0;

	public override object data => _guildShopRef;

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

	public override int type => guildShopRef.itemRef.itemType;

	public GuildShopRefModelData(GuildShopRef guildShopRef, int qty)
	{
		_guildShopRef = guildShopRef;
		_qty = qty;
	}
}
