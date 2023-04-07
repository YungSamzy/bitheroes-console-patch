using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml.common;

public class BattleTriggerData : BaseBookItem
{
	[XmlAttribute("perc")]
	public float perc;

	[XmlAttribute("abilities")]
	public string ability;
}
