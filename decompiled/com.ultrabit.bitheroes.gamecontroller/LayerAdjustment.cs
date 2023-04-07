using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.ultrabit.bitheroes.gamecontroller;

public class LayerAdjustment : MonoBehaviour
{
	private float sorting;

	private float sortingLayer;

	private float pixelHeight;

	private float mapHeight;

	public GameObject mapTop;

	public GameObject mapBottom;

	public GameObject map;

	private int minSortingLayer;

	private string sufix;

	public int parts;

	public float mapSizeY;

	private float mapPartY;

	private float mapBottomY;

	public float mapSizeX;

	public Vector2 mapMin;

	public Vector2 mapMax;

	private void Awake()
	{
		GameData.instance.layerAdjustment = this;
		switch (SceneManager.GetActiveScene().name)
		{
		case "Main":
			sufix = "Map";
			break;
		case "Fishing":
			sufix = "Fishing";
			break;
		}
		mapHeight = (Mathf.Abs(mapTop.transform.position.z) - Mathf.Abs(mapBottom.transform.position.z)) / (float)parts;
		mapSizeY = (float)(int)(GetComponent<SpriteRenderer>().bounds.size.z / GetComponent<Transform>().localScale.x * 1000f) * 0.001f;
		mapSizeX = (float)(int)(GetComponent<SpriteRenderer>().bounds.size.x / GetComponent<Transform>().localScale.x * 1000f) * 0.001f;
		mapPartY = mapSizeY / (float)parts;
		mapBottomY = 0f - mapSizeY * 0.5f;
		mapMin = new Vector2(0f - mapSizeX * 0.5f, 0f - mapSizeY * 0.5f);
		mapMax = new Vector2(mapSizeX * 0.5f, mapSizeY * 0.5f);
		if (SortingLayer.NameToID(parts - 1 + sufix) == 0)
		{
			D.LogError("The sort layer for the last part was not found. <color=red><b>Check if the current LayerAdjuster has 'parts' set properly.</b></color>");
		}
		if (SortingLayer.NameToID(parts + sufix) != 0)
		{
			D.LogError("You have more sorting layers for the map than needed. <color=red><b>Check if the current LayerAdjuster has 'parts' set properly.</b></color>");
		}
	}

	public void ChangeLayer(List<SpriteRenderer> spritesToLayer, GameObject parent)
	{
		for (int i = 0; i < parts; i++)
		{
			if (parent.transform.position.z > mapTop.transform.position.z + mapHeight * (float)i)
			{
				for (int j = 0; j < spritesToLayer.Count; j++)
				{
					spritesToLayer[j].sortingLayerName = i + sufix;
				}
				break;
			}
		}
	}

	public void AdressableChangeLayer(Transform parent)
	{
		for (int num = parts - 1; num >= 0; num--)
		{
			if (parent.localPosition.y < mapBottomY + mapPartY * (float)(parts - num))
			{
				for (int i = 0; i < parent.childCount; i++)
				{
					parent.GetChild(i).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = num + sufix;
				}
				break;
			}
		}
	}

	public void SmartChangeLayer(List<SpriteRenderer> spritesToLayer, Transform parent)
	{
		for (int num = parts - 1; num >= 0; num--)
		{
			if (parent.localPosition.y < mapBottomY + mapPartY * (float)(parts - num))
			{
				for (int i = 0; i < spritesToLayer.Count; i++)
				{
					spritesToLayer[i].sortingLayerName = num + sufix;
				}
				break;
			}
		}
	}
}
