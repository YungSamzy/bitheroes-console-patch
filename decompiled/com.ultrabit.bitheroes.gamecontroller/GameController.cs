using com.ultrabit.bitheroes.model.utility;
using UnityEngine;

namespace com.ultrabit.bitheroes.gamecontroller;

public class GameController : MonoBehaviour
{
	public GameObject mapParent;

	private LayerAdjustment layerAdjustment;

	public GameObject adressables;

	private void Start()
	{
		layerAdjustment = mapParent.GetComponent<LayerAdjustment>();
		int childCount = adressables.transform.childCount;
		_ = mapParent.GetComponent<SpriteRenderer>().bounds.size.z / mapParent.GetComponent<Transform>().localScale.x;
		for (int num = childCount - 1; num >= 0; num--)
		{
			Transform child = adressables.transform.GetChild(num);
			child.parent = mapParent.transform;
			child.localPosition = Vector3.zero;
			Transform transform = child.Find("ObstacleBottom");
			if (transform != null)
			{
				layerAdjustment.AdressableChangeLayer(transform);
			}
			else
			{
				D.LogError("<color=red><b> " + child.name + " does not have an ObstacleBottom or it's name is different from 'ObstacleBottom'. (It will not be added to the game) </b></color>");
				Object.Destroy(child.gameObject);
			}
		}
		mapParent.GetComponent<MapChildSetter>().SetChildren();
	}

	private void Update()
	{
	}
}
