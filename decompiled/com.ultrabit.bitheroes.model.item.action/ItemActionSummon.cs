using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.language;

namespace com.ultrabit.bitheroes.model.item.action;

public class ItemActionSummon : ItemActionBase
{
	public ItemActionSummon(BaseModelData itemData)
		: base(itemData, 16)
	{
	}

	public override void Execute()
	{
		base.Execute();
		string @string = Language.GetString("ui_summon_confirm", new string[1] { itemData.itemRef.coloredName });
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_summon"), @string, null, null, delegate
		{
			OnExecuteItemGenericAction();
		});
	}
}
