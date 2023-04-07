using com.ultrabit.bitheroes.model.craft;

namespace com.ultrabit.bitheroes.model.item;

public class ItemReforgeRef
{
	private int _itemID;

	private int _itemType;

	private string _reforgeLink;

	public ItemRef itemRef => ItemBook.Lookup(_itemID, _itemType);

	public CraftReforgeRef reforgeRef => CraftBook.LookupReforgeLink(_reforgeLink);

	public ItemReforgeRef(int itemID, int itemType, string reforgeLink)
	{
		_itemID = itemID;
		_itemType = itemType;
		_reforgeLink = reforgeLink;
	}
}
