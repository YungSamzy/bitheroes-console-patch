using com.ultrabit.bitheroes.model.data;

namespace com.ultrabit.bitheroes.model.item.action;

public class ItemActionSelect : ItemActionBase
{
	public ItemActionSelect(BaseModelData itemData, int type)
		: base(itemData, type)
	{
	}

	public override void Execute()
	{
		base.Execute();
		if (onPostExecuteCallback != null)
		{
			onPostExecuteCallback(itemData);
		}
	}
}
