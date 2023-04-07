using com.ultrabit.bitheroes.core;

namespace com.ultrabit.bitheroes.ui.item;

public class ItemAugmentFilterWindow : ItemFilterWindow
{
	public static readonly int[] AUGMENT_FILTERS = new int[1];

	public static readonly int[] AUGMENT_SUBTYPE_FILTERS = new int[4] { 0, 1, 2, 3 };

	private static readonly int COLUMNS = 2;

	protected override void Awake()
	{
		base.Awake();
		availableFilters = AUGMENT_FILTERS;
		typeForSubtypes = 15;
		availableSubtypeFilters = AUGMENT_SUBTYPE_FILTERS;
		columns = COLUMNS;
		onAdvancedFilterBtn = delegate
		{
			GameData.instance.windowGenerator.NewItemAugmentAdvancedFilterWindow(filter, advancedFilterSettings, base.OnAdvancedFilterWindowClose, typeForSubtypes, base.gameObject);
		};
	}

	protected override void LoadFilter()
	{
		filter = GameData.instance.SAVE_STATE.GetAugmentFilter(GameData.instance.PROJECT.character.id);
		advancedFilterSettings = GameData.instance.SAVE_STATE.GetAugmentAdvancedFilter(GameData.instance.PROJECT.character.id);
	}

	protected override void SaveFilter()
	{
		GameData.instance.SAVE_STATE.SetAugmentFilter(GameData.instance.PROJECT.character.id, filter);
		GameData.instance.SAVE_STATE.SetAugmentAdvancedFilter(GameData.instance.PROJECT.character.id, advancedFilterSettings);
	}
}
