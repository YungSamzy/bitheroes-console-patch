using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.rune;
using com.ultrabit.bitheroes.ui.item;

namespace com.ultrabit.bitheroes.model.item.action;

public class ItemActionFactory
{
	public static ItemActionBase Create(BaseModelData itemData, int type = -1, string altName = null, ItemIcon itemIcon = null, bool forceConsume = false)
	{
		if (itemData == null && type == -1)
		{
			return null;
		}
		if (itemData != null && type == -1)
		{
			type = itemData.itemRef.getItemTooltipType(GameData.instance.PROJECT.character);
		}
		switch (type)
		{
		case 1:
		case 2:
			return new ItemActionToggleEquip(itemData, type, itemIcon);
		case 6:
			return new ItemActionUse(itemData, forceConsume);
		case 3:
			if (altName != null)
			{
				return new ItemActionChange(itemData, altName);
			}
			return new ItemActionChange(itemData);
		case 4:
			return new ItemActionBuy(itemData);
		case 7:
			return new ItemActionView(itemData);
		case 14:
			return new ItemActionIdentify(itemData);
		case 8:
			return new ItemActionExchange(itemData);
		case 9:
			return new ItemActionUpgrade(itemData);
		case 12:
			return new ItemActionReforge(itemData);
		case 15:
			return new ItemActionTrade(itemData);
		case 16:
			return new ItemActionSummon(itemData);
		case 10:
			return new ItemActionSelect(itemData, 10);
		case 13:
			return new ItemActionSelect(itemData, 13);
		case 11:
			return new ItemActionSelect(itemData, 11);
		case 17:
		case 18:
		case 19:
			return new ItemActionRune(itemData, type, (itemData.itemRef as RuneRef).runeTile);
		case 20:
			return new ItemActionRune(itemData, type, (itemData.itemRef as RuneRef).amoryRuneTile);
		default:
			return new ItemActionNone(itemData);
		}
	}
}
