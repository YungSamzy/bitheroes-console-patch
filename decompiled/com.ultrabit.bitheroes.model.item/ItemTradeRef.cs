using System.Collections.Generic;
using com.ultrabit.bitheroes.core;

namespace com.ultrabit.bitheroes.model.item;

public class ItemTradeRef
{
	private List<ItemData> _requiredItems;

	private ItemData _resultItem;

	public List<ItemData> requiredItems => _requiredItems;

	public ItemData resultItem => _resultItem;

	public ItemTradeRef(List<ItemData> requiredItems, ItemData resultItem)
	{
		_requiredItems = requiredItems;
		_resultItem = resultItem;
	}

	public bool requirementsMet()
	{
		foreach (ItemData requiredItem in _requiredItems)
		{
			if (GameData.instance.PROJECT.character.getItemQty(requiredItem.itemRef) < requiredItem.qty)
			{
				return false;
			}
		}
		return true;
	}
}
