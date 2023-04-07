using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.ui.item;

namespace com.ultrabit.bitheroes.ui.lists.craftlist;

public class CraftItemModel
{
	public ItemIcon itemIcon;

	public ItemData itemData;

	public int action;

	public int originalQty;

	public bool visible;

	public CraftItemModel(ItemData itemData, int action, ItemIcon itemIcon = null, int? originalQty = null)
	{
		this.itemData = itemData;
		this.action = action;
		this.itemIcon = itemIcon;
		this.originalQty = originalQty ?? itemData.qty;
	}
}
