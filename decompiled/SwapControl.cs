using Com.TheFallenGames.OSA.Util.ItemDragging;
using UnityEngine;

public class SwapControl : MonoBehaviour
{
	private DraggableItem draggable;

	private RectTransform rt;

	private Transform messageHandler;

	public void ClickButtonSwap(bool toUp)
	{
		string methodName = (toUp ? "SwapUp" : "SwapDown");
		GetMessageHandler().BroadcastMessage(methodName, GetRectTransform());
	}

	private Transform GetMessageHandler()
	{
		if (messageHandler == null)
		{
			messageHandler = base.transform.parent.parent.parent;
		}
		return messageHandler;
	}

	private RectTransform GetRectTransform()
	{
		if (rt == null)
		{
			rt = GetComponent<RectTransform>();
		}
		return rt;
	}
}
