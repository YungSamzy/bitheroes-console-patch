using com.ultrabit.bitheroes.core;

namespace com.ultrabit.bitheroes.ui.item;

public class ItemExchangeFilterWindow : ItemFilterWindow
{
	public static readonly int[] EXCHANGE_FILTERS = new int[5] { 0, 1, 15, 9, 11 };

	private static readonly int COLUMNS = 2;

	protected override void Awake()
	{
		base.Awake();
		availableFilters = EXCHANGE_FILTERS;
		columns = COLUMNS;
		onAdvancedFilterBtn = delegate
		{
			GameData.instance.windowGenerator.NewItemExchangeAdvancedFilterWindow(filter, advancedFilterSettings, base.OnAdvancedFilterWindowClose, -1, base.gameObject);
		};
	}

	protected override void LoadFilter()
	{
		filter = GameData.instance.SAVE_STATE.GetExchangeFilter(GameData.instance.PROJECT.character.id);
		advancedFilterSettings = GameData.instance.SAVE_STATE.GetExchangeAdvancedFilter(GameData.instance.PROJECT.character.id);
	}

	protected override void SaveFilter()
	{
		GameData.instance.SAVE_STATE.SetExchangeFilter(GameData.instance.PROJECT.character.id, filter);
		GameData.instance.SAVE_STATE.SetExchangeAdvancedFilter(GameData.instance.PROJECT.character.id, advancedFilterSettings);
	}
}
