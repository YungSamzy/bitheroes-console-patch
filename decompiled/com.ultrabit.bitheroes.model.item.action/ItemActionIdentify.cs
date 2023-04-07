using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.transaction;

namespace com.ultrabit.bitheroes.model.item.action;

public class ItemActionIdentify : ItemActionBase
{
	private TransactionManager transactionManager;

	public ItemActionIdentify(BaseModelData itemData)
		: base(itemData, 14)
	{
	}

	public override void Execute()
	{
		base.Execute();
		string @string = Language.GetString("ui_identify_confirm", new string[1] { itemData.itemRef.coloredName });
		if (!GameData.instance.PROJECT.character.tutorial.GetState(74))
		{
			GameData.instance.PROJECT.character.tutorial.SetState(74);
			OnExecuteItemGenericAction();
		}
		else if (!GameData.instance.PROJECT.character.tutorial.GetState(97))
		{
			GameData.instance.PROJECT.character.tutorial.SetState(97);
			OnExecuteItemGenericAction();
		}
		else
		{
			GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), @string, null, null, delegate
			{
				OnExecuteItemGenericAction();
			});
		}
	}
}
