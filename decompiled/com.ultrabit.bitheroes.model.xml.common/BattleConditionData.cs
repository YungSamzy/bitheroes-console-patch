using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml.common;

public class BattleConditionData : BaseBookItem
{
	[XmlElement("modifiers")]
	public GameModifiersData modifiers;

	[XmlElement("modifier")]
	public List<GameModifierData> lstModifier;

	[XmlElement("perc")]
	public string perc;

	[XmlAttribute("abilities")]
	public string abilities;

	[XmlAttribute("value")]
	public string value;
}
