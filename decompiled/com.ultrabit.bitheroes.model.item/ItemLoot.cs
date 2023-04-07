namespace com.ultrabit.bitheroes.model.item;

public class ItemLoot
{
	private ItemData _itemData;

	private float _perc;

	public ItemData itemData => _itemData;

	public float perc => _perc;

	public ItemLoot(ItemData itemData, float perc)
	{
		_itemData = itemData;
		_perc = perc;
	}
}
