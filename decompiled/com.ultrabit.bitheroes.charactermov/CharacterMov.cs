using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.eventdispatcher;
using com.ultrabit.bitheroes.gamecontroller;
using com.ultrabit.bitheroes.model.utility;
using UnityEngine;
using UnityEngine.AI;

namespace com.ultrabit.bitheroes.charactermov;

public class CharacterMov : MonoBehaviour
{
	private float playerKeyMove = 0.05f;

	private bool left;

	public bool click;

	public float clickX;

	public float clickY;

	private List<SpriteRenderer> characterRender = new List<SpriteRenderer>();

	private LayerAdjustment layerAdjust;

	private float previousY;

	public bool reachedDestination;

	private NavMeshAgent playerAgent;

	private PlayerVisual playerVisual;

	private Target target;

	private bool targetChanged;

	private bool goToinstance;

	private GameObject previousCaller;

	private void Awake()
	{
		GameData.instance.characterMov = this;
	}

	private void Start()
	{
		layerAdjust = GameData.instance.layerAdjustment;
		playerAgent = base.gameObject.GetComponentInChildren<NavMeshAgent>();
		playerVisual = base.gameObject.GetComponentInChildren<PlayerVisual>();
		target = base.gameObject.GetComponentInChildren<Target>();
		if (playerAgent == null)
		{
			D.LogError("<color=red><b> " + base.gameObject.transform.parent.name + " does not have an NPCAgent. The NPC will be destroyed in playmode.</b></color>");
		}
		if (playerVisual == null)
		{
			D.LogError("<color=red><b> " + base.gameObject.transform.parent.name + " NPCVisual does not have an NPCVisual script or an NPCVisual GameObject. The NPC will be destroyed in playmode.</b></color>");
		}
		if (target == null)
		{
			D.LogError("<color=red><b> " + base.gameObject.transform.parent.name + " Target does not have an Target script or an Target GameObject. The NPC will be destroyed in playmode.</b></color>");
		}
		for (int i = 0; i < playerVisual.gameObject.transform.childCount; i++)
		{
			if (playerVisual.transform.GetChild(i).GetComponent<SpriteRenderer>() != null)
			{
				characterRender.Add(playerVisual.transform.GetChild(i).GetComponent<SpriteRenderer>());
			}
		}
		layerAdjust.SmartChangeLayer(characterRender, playerVisual.transform);
		previousY = playerVisual.transform.localPosition.y;
	}

	private void FixedUpdate()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.UpArrow))
		{
			playerAgent.SetDestination(playerAgent.transform.position += new Vector3(0f, 0f, playerKeyMove));
			click = false;
		}
		if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKey(KeyCode.DownArrow))
		{
			playerAgent.SetDestination(playerAgent.transform.position += new Vector3(0f, 0f, 0f - playerKeyMove));
			click = false;
		}
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
		{
			playerAgent.SetDestination(playerAgent.transform.position += new Vector3(0f - playerKeyMove, 0f, 0f));
			if (!left)
			{
				left = true;
				playerVisual.transform.localScale = new Vector3(playerVisual.transform.localScale.x * -1f, playerVisual.transform.localScale.y, playerVisual.transform.localScale.z);
			}
			click = false;
		}
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.RightArrow))
		{
			playerAgent.SetDestination(playerAgent.transform.position += new Vector3(playerKeyMove, 0f, 0f));
			if (left)
			{
				left = false;
				playerVisual.transform.localScale = new Vector3(playerVisual.transform.localScale.x * -1f, playerVisual.transform.localScale.y, playerVisual.transform.localScale.z);
			}
			click = false;
		}
	}

	private void LateUpdate()
	{
		if (playerAgent.pathEndPosition != playerAgent.transform.position)
		{
			playerVisual.transform.localPosition = new Vector3(playerAgent.transform.localPosition.x, playerAgent.transform.localPosition.y, playerVisual.transform.localPosition.z);
			if (playerVisual.transform.localPosition.y != previousY)
			{
				layerAdjust.SmartChangeLayer(characterRender, playerAgent.transform);
				previousY = playerVisual.transform.localPosition.y;
			}
		}
		if ((double)playerAgent.remainingDistance < 1E-05 && goToinstance && targetChanged)
		{
			reachedDestination = true;
			EventUtil.DispatchEvent("REACHED_INSTANCE");
			targetChanged = false;
			goToinstance = false;
		}
		if (!click)
		{
			return;
		}
		target.transform.position = new Vector3(clickX, target.transform.position.y, clickY);
		if (target.transform.localPosition.x < playerAgent.transform.localPosition.x)
		{
			if (!left)
			{
				left = true;
				playerVisual.transform.localScale = new Vector3(playerVisual.transform.localScale.x * -1f, playerVisual.transform.localScale.y, playerVisual.transform.localScale.z);
			}
		}
		else if (target.transform.localPosition.x > playerAgent.transform.localPosition.x && left)
		{
			left = false;
			playerVisual.transform.localScale = new Vector3(playerVisual.transform.localScale.x * -1f, playerVisual.transform.localScale.y, playerVisual.transform.localScale.z);
		}
		playerAgent.SetDestination(new Vector3(target.transform.position.x, playerAgent.transform.position.y, target.transform.position.z));
		targetChanged = true;
	}

	public void MoveTo(float X, float Y, bool fromInstance, GameObject caller)
	{
		click = true;
		clickX = X;
		clickY = Y;
		reachedDestination = false;
		targetChanged = false;
		goToinstance = fromInstance;
		EventUtil.DispatchEvent("ON_MAP_CLICK", caller);
	}
}
