namespace com.ultrabit.bitheroes.model.xml;

public abstract class BaseBook
{
	public abstract BaseBookItem GetByIdentifier(string identifier);

	public abstract BaseBookItem GetByIdentifier(int identifier);

	public virtual BaseBookItem GetByIdentifier(string identifier, out int id)
	{
		id = -1;
		return null;
	}
}
