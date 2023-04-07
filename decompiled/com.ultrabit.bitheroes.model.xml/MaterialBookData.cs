using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml;

[XmlRoot("data")]
public class MaterialBookData : BaseBook
{
	public class Material : BaseBookItem
	{
	}

	[XmlElement("material")]
	public List<Material> lstMaterial;

	public override BaseBookItem GetByIdentifier(string identifier)
	{
		throw new NotImplementedException();
	}

	public override BaseBookItem GetByIdentifier(int identifier)
	{
		return lstMaterial.Find((Material item) => item.id.Equals(identifier));
	}

	public void Clear()
	{
		lstMaterial.Clear();
		lstMaterial = null;
	}
}
