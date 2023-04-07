using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml.common;

public class GameModifiersData
{
	[XmlElement("modifier")]
	public List<GameModifierData> lstModifier { get; set; }

	[XmlElement("modifiers")]
	public GameModifiersData modifiers { get; set; }
}
