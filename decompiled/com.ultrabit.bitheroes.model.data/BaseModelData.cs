using com.ultrabit.bitheroes.model.item;

namespace com.ultrabit.bitheroes.model.data;

public abstract class BaseModelData
{
	public abstract ItemRef itemRef { get; }

	public abstract int power { get; }

	public abstract int stamina { get; }

	public abstract int agility { get; }

	public abstract object data { get; }

	public abstract int qty { get; set; }

	public abstract int type { get; }
}
