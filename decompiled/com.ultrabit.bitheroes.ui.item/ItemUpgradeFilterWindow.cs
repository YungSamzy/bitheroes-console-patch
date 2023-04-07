using com.ultrabit.bitheroes.core;

namespace com.ultrabit.bitheroes.ui.item;

public class ItemUpgradeFilterWindow : ItemFilterWindow
{
	public static readonly int[] UPGRADE_FILTERS = new int[1];

	public static readonly int[] EQUIPMENT_SUBTYPE_FILTERS = new int[8] { 1, 2, 3, 4, 5, 6, 7, 8 };

	private static readonly int COLUMNS = 2;

	protected override void Awake()
	{
		base.Awake();
		availableFilters = UPGRADE_FILTERS;
		typeForSubtypes = 1;
		availableSubtypeFilters = EQUIPMENT_SUBTYPE_FILTERS;
		columns = COLUMNS;
		onAdvancedFilterBtn = delegate
		{
			GameData.instance.windowGenerator.NewItemUpgradeAdvancedFilterWindow(filter, advancedFilterSettings, base.OnAdvancedFilterWindowClose, typeForSubtypes, base.gameObject);
		};
	}

	protected override void LoadFilter()
	{
		filter = GameData.instance.SAVE_STATE.GetUpgradeFilter(GameData.instance.PROJECT.character.id);
		advancedFilterSettings = GameData.instance.SAVE_STATE.GetUpgradeAdvancedFilter(GameData.instance.PROJECT.character.id);
	}

	protected override void SaveFilter()
	{
		GameData.instance.SAVE_STATE.SetUpgradeFilter(GameData.instance.PROJECT.character.id, filter);
		GameData.instance.SAVE_STATE.SetUpgradeAdvancedFilter(GameData.instance.PROJECT.character.id, advancedFilterSettings);
	}
}
