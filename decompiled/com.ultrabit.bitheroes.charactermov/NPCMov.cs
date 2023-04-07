using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.gamecontroller;
using UnityEngine;
using UnityEngine.AI;

namespace com.ultrabit.bitheroes.charactermov;

public class NPCMov : MonoBehaviour
{
	public enum NPCNAME
	{
		NPC_PVP,
		NPC_MOUNTS,
		NPC_FUSION,
		NPC_RIFTS,
		NPC_GAUNTLET,
		NPC_MYSTIC,
		NPC_STORE,
		NPC_STABLE,
		NPC_TRIALS,
		NPC_FISHING,
		NPC_LEADERBOARDS,
		NPC_CRAFT,
		NPC_AUGMENTS,
		NPC_ENCHANTS,
		NPC_QUEST
	}

	public new NPCNAME name;

	private NavMeshAgent navAgent;

	public Vector2 minPos;

	public Vector2 maxPos;

	public bool L;

	public bool LReflex;

	public float LVerticalBreak;

	public float LHorizontalBreak;

	private int LPlace;

	public Vector2 minPosL;

	public Vector2 maxPosL;

	private float waitTime;

	public float minWaitTime = 2f;

	public float maxWaitTime = 5f;

	private float destinationX;

	private float destinationZ;

	private bool left;

	private List<SpriteRenderer> characterRender = new List<SpriteRenderer>();

	public LayerAdjustment layerAdjust;

	private float previousZ;

	private void Start()
	{
		navAgent = GetComponent<NavMeshAgent>();
		for (int i = 0; i < base.transform.childCount; i++)
		{
			characterRender.Add(base.transform.GetChild(i).GetComponent<SpriteRenderer>());
		}
		destinationX = base.transform.position.x;
		destinationZ = base.transform.position.z;
		waitTime = Random.Range(minWaitTime, maxWaitTime);
		StartCoroutine(MoveRandom());
		previousZ = base.transform.position.z;
	}

	private void FixedUpdate()
	{
		if (base.transform.position.z != previousZ)
		{
			previousZ = base.transform.position.z;
		}
	}

	private IEnumerator MoveRandom()
	{
		yield return new WaitForSeconds(waitTime);
		if (L)
		{
			if (base.transform.position.z <= LVerticalBreak)
			{
				if (base.transform.position.x >= LHorizontalBreak)
				{
					destinationX = Random.Range(minPosL.x, maxPosL.x);
					destinationZ = Random.Range(minPosL.y, maxPosL.y);
				}
				else
				{
					switch (Random.Range(0, 2))
					{
					case 0:
						destinationX = Random.Range(minPosL.x, maxPosL.x);
						destinationZ = Random.Range(minPosL.y, maxPosL.y);
						break;
					case 1:
						destinationX = Random.Range(minPos.x, maxPos.x);
						destinationZ = Random.Range(minPos.y, maxPos.y);
						break;
					}
				}
			}
			else
			{
				destinationX = Random.Range(minPos.x, maxPos.x);
				destinationZ = Random.Range(minPos.y, maxPos.y);
			}
		}
		if (LReflex)
		{
			if (base.transform.position.z <= LVerticalBreak)
			{
				if (base.transform.position.x < LHorizontalBreak)
				{
					destinationX = Random.Range(minPosL.x, maxPosL.x);
					destinationZ = Random.Range(minPosL.y, maxPosL.y);
				}
				else
				{
					switch (Random.Range(0, 2))
					{
					case 0:
						destinationX = Random.Range(minPosL.x, maxPosL.x);
						destinationZ = Random.Range(minPosL.y, maxPosL.y);
						break;
					case 1:
						destinationX = Random.Range(minPos.x, maxPos.x);
						destinationZ = Random.Range(minPos.y, maxPos.y);
						break;
					}
				}
			}
			else
			{
				destinationX = Random.Range(minPos.x, maxPos.x);
				destinationZ = Random.Range(minPos.y, maxPos.y);
			}
		}
		if (!L && !LReflex)
		{
			destinationX = Random.Range(minPos.x, maxPos.x);
			destinationZ = Random.Range(minPos.y, maxPos.y);
		}
		if (destinationX < base.transform.position.x)
		{
			if (!left)
			{
				left = true;
				base.transform.localScale = new Vector3(base.transform.localScale.x * -1f, base.transform.localScale.y, base.transform.localScale.z);
			}
		}
		else if (left)
		{
			left = false;
			base.transform.localScale = new Vector3(base.transform.localScale.x * -1f, base.transform.localScale.y, base.transform.localScale.z);
		}
		navAgent.SetDestination(new Vector3(destinationX, base.transform.position.y, destinationZ));
		waitTime = Random.Range(minWaitTime, maxWaitTime);
		StartCoroutine(MoveRandom());
	}

	public void AdjustBrightness(float amount)
	{
	}

	public void NPCDialog()
	{
	}
}
