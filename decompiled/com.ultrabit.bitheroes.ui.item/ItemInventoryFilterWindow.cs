using com.ultrabit.bitheroes.core;

namespace com.ultrabit.bitheroes.ui.item;

public class ItemInventoryFilterWindow : ItemFilterWindow
{
	public static readonly int[] INVENTORY_FILTERS = new int[5] { 0, 1, 2, 4, 12 };

	private static readonly int COLUMNS = 2;

	protected override void Awake()
	{
		base.Awake();
		availableFilters = INVENTORY_FILTERS;
		columns = COLUMNS;
		onAdvancedFilterBtn = delegate
		{
			GameData.instance.windowGenerator.NewItemInventoryAdvancedFilterWindow(filter, advancedFilterSettings, base.OnAdvancedFilterWindowClose, -1, base.gameObject);
		};
	}

	protected override void LoadFilter()
	{
		filter = GameData.instance.SAVE_STATE.GetInventoryFilter(GameData.instance.PROJECT.character.id);
		advancedFilterSettings = GameData.instance.SAVE_STATE.GetInventoryAdvancedFilter(GameData.instance.PROJECT.character.id);
	}

	protected override void SaveFilter()
	{
		GameData.instance.SAVE_STATE.SetInventoryFilter(GameData.instance.PROJECT.character.id, filter);
		GameData.instance.SAVE_STATE.SetInventoryAdvancedFilter(GameData.instance.PROJECT.character.id, advancedFilterSettings);
	}
}
