using System;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.adgor;

public class AdgorItemRef : BaseBook
{
	private int _id;

	private string _type;

	public int id => _id;

	public string type => _type;

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		throw new NotImplementedException();
	}

	public AdgorItemRef(VariableBookData.Item itemData)
	{
		_id = itemData.id;
		_type = itemData.type;
	}
}
