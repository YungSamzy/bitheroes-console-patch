using com.ultrabit.bitheroes.core;

namespace com.ultrabit.bitheroes.ui.item;

public class ItemTradeFilterWindow : ItemFilterWindow
{
	public static readonly int[] TRADE_FILTERS = new int[7] { 0, 1, 15, 9, 2, 4, 3 };

	private static readonly int COLUMNS = 2;

	protected override void Awake()
	{
		base.Awake();
		availableFilters = TRADE_FILTERS;
		columns = COLUMNS;
		onAdvancedFilterBtn = delegate
		{
			GameData.instance.windowGenerator.NewItemTradeAdvancedFilterWindow(filter, advancedFilterSettings, base.OnAdvancedFilterWindowClose, -1, base.gameObject);
		};
	}

	protected override void LoadFilter()
	{
		filter = GameData.instance.SAVE_STATE.GetTradeFilter(GameData.instance.PROJECT.character.id);
		advancedFilterSettings = GameData.instance.SAVE_STATE.GetTradeAdvancedFilter(GameData.instance.PROJECT.character.id);
	}

	protected override void SaveFilter()
	{
		GameData.instance.SAVE_STATE.SetTradeFilter(GameData.instance.PROJECT.character.id, filter);
		GameData.instance.SAVE_STATE.SetTradeAdvancedFilter(GameData.instance.PROJECT.character.id, advancedFilterSettings);
	}
}
