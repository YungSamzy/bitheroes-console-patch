namespace com.ultrabit.bitheroes.ui.item;

public class ItemInventoryAdvancedFilterWindow : ItemAdvancedFilterWindow
{
	public static readonly int[] RARITY_FILTERS = new int[8] { 1, 2, 3, 4, 5, 6, 7, 8 };

	public static readonly int[] EQUIPMENT_FILTERS = new int[8] { 1, 2, 3, 4, 5, 6, 7, 8 };

	public static readonly int[] TYPE_FILTERS = ItemInventoryFilterWindow.INVENTORY_FILTERS;

	private void Awake()
	{
		availableRarityFilters = RARITY_FILTERS;
		availableEquipmentFilters = EQUIPMENT_FILTERS;
		availableTypeFilters = TYPE_FILTERS;
	}
}
