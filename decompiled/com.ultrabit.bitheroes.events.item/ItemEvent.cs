using UnityEngine;

namespace com.ultrabit.bitheroes.events.item;

public class ItemEvent
{
	public const string ITEM_DRAG_START = "ITEM_DRAG_START";

	public const string ITEM_DRAG_STOP = "ITEM_DRAG_STOP";

	public const string ITEM_SELECT = "ITEM_SELECT";

	public const string ITEM_DESELECT = "ITEM_DESELECT";

	private string _eventType;

	private Object _parameters;

	public string eventType => _eventType;

	public Object parameters => _parameters;

	public ItemEvent(string type, Object parameters = null)
	{
		_eventType = type;
		_parameters = parameters;
	}
}
