using System.Collections.Generic;
using System.Xml.Serialization;

namespace com.ultrabit.bitheroes.model.xml.common;

public class GameModifierData : BaseBookItem
{
	[XmlAttribute("values")]
	public string values;

	[XmlAttribute("value")]
	public float value;

	[XmlElement("condition")]
	public List<BattleConditionData> lstCondition;

	[XmlAttribute("tooltip")]
	public string tooltip;

	[XmlAttribute("tooltipFull")]
	public string tooltipFull;

	[XmlElement("trigger")]
	public List<BattleTriggerData> lstTrigger;

	[XmlElement("proc")]
	public ProcRef proc;

	[XmlAttribute("additive")]
	public string additive { get; set; }
}
