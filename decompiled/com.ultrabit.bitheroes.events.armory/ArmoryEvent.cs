using com.ultrabit.bitheroes.model.events;

namespace com.ultrabit.bitheroes.events.armory;

public class ArmoryEvent : CustomSFSXEvent
{
	public const string ARMORY_NAME_CHANGE = "armoryNameChange";

	public const string ARMORY_EQUIPMENT_CHANGE = "armoryEquipmentChange";

	public const string ARMORY_MOUNT_CHANGE = "armoryMountChange";

	public const string ARMORY_RUNE_CHANGE = "armoryRuneChange";

	public const string ARMORY_TEAMMATE_SELECT = "armoryTeammateSelect";

	private string _eventType;

	private object _parameters;

	private int _armoryID;

	public object parameters => _parameters;

	public int armoryID
	{
		get
		{
			return _armoryID;
		}
		set
		{
			_armoryID = value;
		}
	}

	public ArmoryEvent(string type, object parameters = null)
		: base(type)
	{
		_eventType = type;
		_parameters = parameters;
	}

	public override CustomSFSXEvent clone()
	{
		return new ArmoryEvent(eventType, parameters);
	}
}
