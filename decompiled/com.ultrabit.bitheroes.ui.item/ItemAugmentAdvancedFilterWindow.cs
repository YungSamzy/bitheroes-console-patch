namespace com.ultrabit.bitheroes.ui.item;

public class ItemAugmentAdvancedFilterWindow : ItemAdvancedFilterWindow
{
	public static readonly int[] RARITY_FILTERS = new int[5] { 1, 2, 3, 4, 6 };

	public static readonly int[] AUGMENT_FILTERS = new int[4] { 0, 1, 2, 3 };

	public static readonly int[] TYPE_FILTERS = ItemAugmentFilterWindow.AUGMENT_FILTERS;

	private void Awake()
	{
		availableRarityFilters = RARITY_FILTERS;
		availableAugmentFilters = AUGMENT_FILTERS;
		availableTypeFilters = TYPE_FILTERS;
	}
}
