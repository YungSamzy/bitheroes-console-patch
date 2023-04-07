using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.eventdispatcher;
using com.ultrabit.bitheroes.gamecontroller;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui;
using UnityEngine;
using UnityEngine.AI;

namespace com.ultrabit.bitheroes.charactermov;

public class NPCBehaviour : MonoBehaviour
{
	public enum NPCNAME
	{
		NPC_PVP,
		NPC_FUSION,
		NPC_GAUNTLET,
		NPC_SHOP,
		NPC_FAMILIARSTABLE,
		NPC_RIFTS,
		NPC_FISHING,
		NPC_LEADERBOARD,
		NPC_CRAFT,
		NPC_AUGMENTS,
		NPC_ENCHANTS,
		NPC_QUEST,
		NPC_BRAWL
	}

	private NavMeshAgent npcAgent;

	private NPCVisual npcVisual;

	public new NPCNAME name;

	public Vector2 minPos;

	public Vector2 maxPos;

	public bool L;

	public bool LReflex;

	private int LPlace;

	public Vector2 minPosL;

	public Vector2 maxPosL;

	private float waitTime;

	public float minWaitTime = 2f;

	public float maxWaitTime = 5f;

	private float destinationX;

	private float destinationY;

	private bool left;

	private List<SpriteRenderer> characterRender = new List<SpriteRenderer>();

	private LayerAdjustment layerAdjust;

	private float previousY;

	private Target target;

	private NPCMouse[] clickables;

	private bool clicked;

	private bool isDestination;

	public const string ON_MAP_CLICK = "ON_MAP_CLICK";

	public const string REACHED_INSTANCE = "REACHED_INSTANCE";

	public int NPCid;

	private Color lightColor = new Color(1.2f, 1.2f, 1.2f);

	private Color normalColor = new Color(1f, 1f, 1f);

	private void Start()
	{
		layerAdjust = GameData.instance.layerAdjustment;
		npcAgent = GetComponentInChildren<NavMeshAgent>();
		npcVisual = GetComponentInChildren<NPCVisual>();
		target = GetComponentInChildren<Target>();
		clickables = GetComponentsInChildren<NPCMouse>();
		NPCMouse[] array = clickables;
		foreach (NPCMouse nPCMouse in array)
		{
			if (nPCMouse == null)
			{
				D.LogError("<color=red><b> " + nPCMouse.gameObject.name + " does not have an NPCMouse Script. The NPC will be destroyed in playmode.</b></color>");
				Object.Destroy(base.gameObject);
			}
			else
			{
				nPCMouse.SetNPCParent = this;
			}
		}
		if (npcAgent == null)
		{
			D.LogError("<color=red><b> " + base.gameObject.transform.parent.name + " does not have an NPCAgent. The NPC will be destroyed in playmode.</b></color>");
			Object.Destroy(base.gameObject);
		}
		if (npcVisual == null)
		{
			D.LogError("<color=red><b> " + base.gameObject.transform.parent.name + " NPCVisual does not have an NPCVisual script or an NPCVisual GameObject. The NPC will be destroyed in playmode.</b></color>");
			Object.Destroy(base.gameObject);
		}
		if (target == null)
		{
			D.LogError("<color=red><b> " + base.gameObject.transform.parent.name + " Target does not have an Target script or an Target GameObject. The NPC will be destroyed in playmode.</b></color>");
			Object.Destroy(base.gameObject);
		}
		if (L && LReflex)
		{
			D.LogWarning("<color=red><b> " + base.gameObject.transform.parent.name + " has L and LReflex checked. Please choose one or the other. Your NPC will be destroyed in playmode.</b></color>");
			Object.Destroy(base.gameObject);
		}
		for (int j = 0; j < npcVisual.gameObject.transform.childCount; j++)
		{
			if (npcVisual.transform.GetChild(j).GetComponent<SpriteRenderer>() != null)
			{
				characterRender.Add(npcVisual.transform.GetChild(j).GetComponent<SpriteRenderer>());
			}
		}
		destinationX = npcAgent.transform.localPosition.x;
		destinationY = npcAgent.transform.localPosition.y;
		waitTime = Random.Range(minWaitTime, maxWaitTime);
		previousY = npcVisual.transform.localPosition.y;
		layerAdjust.SmartChangeLayer(characterRender, npcAgent.transform);
		StartCoroutine(MoveRandom());
	}

	private void FixedUpdate()
	{
		if (npcAgent.remainingDistance > 0f)
		{
			npcVisual.transform.localPosition = new Vector3(npcAgent.transform.localPosition.x, npcAgent.transform.localPosition.y, npcVisual.transform.localPosition.z);
			if (npcVisual.transform.localPosition.y != previousY)
			{
				layerAdjust.SmartChangeLayer(characterRender, npcAgent.transform);
				previousY = npcVisual.transform.localPosition.y;
			}
		}
	}

	private IEnumerator MoveRandom()
	{
		yield return new WaitForSeconds(waitTime);
		if (L)
		{
			if (npcAgent.transform.localPosition.y <= maxPos.y)
			{
				if (npcAgent.transform.localPosition.x >= maxPos.x)
				{
					destinationX = Random.Range(minPosL.x, maxPosL.x);
					destinationY = Random.Range(minPosL.y, maxPosL.y);
				}
				else
				{
					switch (Random.Range(0, 2))
					{
					case 0:
						destinationX = Random.Range(minPosL.x, maxPosL.x);
						destinationY = Random.Range(minPosL.y, maxPosL.y);
						break;
					case 1:
						destinationX = Random.Range(minPos.x, maxPos.x);
						destinationY = Random.Range(minPos.y, maxPos.y);
						break;
					}
				}
			}
			else
			{
				destinationX = Random.Range(minPos.x, maxPos.x);
				destinationY = Random.Range(minPos.y, maxPos.y);
			}
		}
		if (LReflex)
		{
			if (npcAgent.transform.localPosition.y <= maxPos.y)
			{
				if (npcAgent.transform.localPosition.x >= maxPos.x)
				{
					destinationX = Random.Range(minPosL.x, maxPosL.x);
					destinationY = Random.Range(minPosL.y, maxPosL.y);
				}
				else
				{
					switch (Random.Range(0, 2))
					{
					case 0:
						destinationX = Random.Range(minPosL.x, maxPosL.x);
						destinationY = Random.Range(minPosL.y, maxPosL.y);
						break;
					case 1:
						destinationX = Random.Range(minPos.x, maxPos.x);
						destinationY = Random.Range(minPos.y, maxPos.y);
						break;
					}
				}
			}
			else
			{
				destinationX = Random.Range(minPos.x, maxPos.x);
				destinationY = Random.Range(minPos.y, maxPos.y);
			}
		}
		if (!L && !LReflex)
		{
			destinationX = Random.Range(minPos.x, maxPos.x);
			destinationY = Random.Range(minPos.y, maxPos.y);
		}
		if (destinationX < npcAgent.transform.localPosition.x)
		{
			if (!left)
			{
				left = true;
				npcVisual.transform.localScale = new Vector3(npcVisual.transform.localScale.x * -1f, npcVisual.transform.localScale.y, npcVisual.transform.localScale.z);
			}
		}
		else if (left)
		{
			left = false;
			npcVisual.transform.localScale = new Vector3(npcVisual.transform.localScale.x * -1f, npcVisual.transform.localScale.y, npcVisual.transform.localScale.z);
		}
		target.transform.localPosition = new Vector3(destinationX, destinationY, target.transform.localPosition.z);
		npcAgent.SetDestination(new Vector3(target.transform.position.x, npcAgent.transform.position.y, target.transform.position.z));
		waitTime = Random.Range(minWaitTime, maxWaitTime);
		StartCoroutine(MoveRandom());
	}

	public void HighlightedColor()
	{
		foreach (SpriteRenderer item in characterRender)
		{
			item.material.color = lightColor;
		}
	}

	public void NotHighlightedColor()
	{
		foreach (SpriteRenderer item in characterRender)
		{
			item.material.color = normalColor;
		}
	}

	public void NPCDialog(Vector2 pointerPos)
	{
		if (!clicked && Physics.Raycast(GameData.instance.main.mainCamera.ScreenPointToRay(pointerPos), out var hitInfo))
		{
			clicked = true;
			GameData.instance.characterMov.MoveTo(hitInfo.point.x, hitInfo.point.z, fromInstance: true, base.gameObject);
			if (!EventUtil.HandlerHasListener("ON_MAP_CLICK", IsNotDestination))
			{
				EventUtil.AddListener("ON_MAP_CLICK", IsNotDestination);
			}
			EventUtil.AddListener("REACHED_INSTANCE", PlayerNearInstance);
			isDestination = true;
		}
	}

	private void PlayerNearInstance(EventArgs evt)
	{
		GameData.instance.windowGenerator.NewWindow();
		clicked = false;
		isDestination = false;
		EventUtil.RemoveListener("ON_MAP_CLICK", IsNotDestination);
		EventUtil.RemoveListener("REACHED_INSTANCE", PlayerNearInstance);
	}

	private void IsNotDestination(EventArgs evt)
	{
		GameObject gameObject = evt.args[0] as GameObject;
		if (gameObject == null)
		{
			isDestination = false;
			clicked = false;
			EventUtil.RemoveListener("ON_MAP_CLICK", IsNotDestination);
			EventUtil.RemoveListener("REACHED_INSTANCE", PlayerNearInstance);
		}
		else if (gameObject.GetInstanceID() != base.gameObject.GetInstanceID())
		{
			isDestination = false;
			clicked = false;
			EventUtil.RemoveListener("ON_MAP_CLICK", IsNotDestination);
			EventUtil.RemoveListener("REACHED_INSTANCE", PlayerNearInstance);
		}
	}

	private void OnDestroy()
	{
		EventUtil.RemoveListener("ON_MAP_CLICK", IsNotDestination);
		EventUtil.RemoveListener("REACHED_INSTANCE", PlayerNearInstance);
	}
}
