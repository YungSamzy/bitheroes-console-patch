using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml.common;

public class ProcRef : BaseBookItem
{
	[XmlElement("modifier")]
	public List<GameModifierData> lstModifier;

	[XmlElement("modifiers")]
	public GameModifiersData modifier;
}
