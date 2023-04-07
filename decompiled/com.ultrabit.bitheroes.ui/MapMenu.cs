using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.eventdispatcher;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui;

public class MapMenu : MonoBehaviour
{
	private WindowGenerator windowGenerator;

	private bool clicked;

	private InstanceMouse[] clickables;

	private bool isDestination;

	private SpriteRenderer renderImage;

	private Color lightColor = new Color(1.4f, 1.4f, 1.4f);

	private Color normalColor = new Color(1f, 1f, 1f);

	private void Start()
	{
		clickables = base.gameObject.GetComponentsInChildren<InstanceMouse>();
		InstanceMouse[] array = clickables;
		foreach (InstanceMouse instanceMouse in array)
		{
			if (instanceMouse == null)
			{
				Debug.LogError("<color=red><b> " + instanceMouse.gameObject.name + " does not have an NPCMouse Script. The NPC will be destroyed in playmode.</b></color>");
				Object.Destroy(base.gameObject);
			}
			else
			{
				instanceMouse.SetInstanceParent = this;
			}
		}
		renderImage = GetComponentInChildren<InstanceMouse>().gameObject.GetComponent<SpriteRenderer>();
		if (renderImage == null)
		{
			Debug.LogError("<color=red><b> " + base.gameObject.name + " does not have an image. The Map Menu will be destroyed in playmode.</b></color>");
			Object.Destroy(base.gameObject);
		}
	}

	public void SetWindow(WindowGenerator window)
	{
		windowGenerator = window;
	}

	public void HighlightedColor()
	{
		renderImage = GetComponentInChildren<InstanceMouse>().gameObject.GetComponent<SpriteRenderer>();
		renderImage.material.color = lightColor;
	}

	public void NotHighlightedColor()
	{
		renderImage = GetComponentInChildren<InstanceMouse>().gameObject.GetComponent<SpriteRenderer>();
		renderImage.material.color = normalColor;
	}

	public void InstanceWindow(Vector2 pointerPos)
	{
		Debug.Log(EventUtil.HandlerHasListener("ON_MAP_CLICK", IsNotDestination) + "-" + base.transform.parent.name);
		if (!clicked && Physics.Raycast(GameData.instance.main.mainCamera.ScreenPointToRay(pointerPos), out var hitInfo))
		{
			clicked = true;
			GameData.instance.characterMov.MoveTo(hitInfo.point.x, hitInfo.point.z, fromInstance: true, base.gameObject);
			if (!EventUtil.HandlerHasListener("ON_MAP_CLICK", IsNotDestination))
			{
				EventUtil.AddListener("ON_MAP_CLICK", IsNotDestination);
				Debug.Log("Added destination listener");
			}
			EventUtil.AddListener("REACHED_INSTANCE", PlayerNearInstance);
			Debug.Log("Added PlayerNearInstance listener");
			isDestination = true;
		}
	}

	private void PlayerNearInstance(EventArgs evt)
	{
		Debug.Log("new Window NPC");
		GameData.instance.windowGenerator.NewWindow();
		clicked = false;
		isDestination = false;
		Debug.Log("Removed destination listener from PlayerNearIstance()");
		EventUtil.RemoveListener("ON_MAP_CLICK", IsNotDestination);
		EventUtil.RemoveListener("REACHED_INSTANCE", PlayerNearInstance);
	}

	private void IsNotDestination(EventArgs evt)
	{
		Debug.Log("is not destination");
		GameObject gameObject = evt.args[0] as GameObject;
		if (gameObject == null)
		{
			Debug.Log("caller null");
			isDestination = false;
			clicked = false;
			EventUtil.RemoveListener("ON_MAP_CLICK", IsNotDestination);
			EventUtil.RemoveListener("REACHED_INSTANCE", PlayerNearInstance);
			Debug.Log("Removed destination listener from null caller");
		}
		else if (gameObject.GetInstanceID() != base.gameObject.GetInstanceID())
		{
			isDestination = false;
			clicked = false;
			EventUtil.RemoveListener("ON_MAP_CLICK", IsNotDestination);
			EventUtil.RemoveListener("REACHED_INSTANCE", PlayerNearInstance);
			Debug.Log("Removed destination listener from different ID");
		}
	}

	private void OnDestroy()
	{
		EventUtil.RemoveListener("ON_MAP_CLICK", IsNotDestination);
		EventUtil.RemoveListener("REACHED_INSTANCE", PlayerNearInstance);
		Debug.Log("Removed destination listener destroy");
	}
}
