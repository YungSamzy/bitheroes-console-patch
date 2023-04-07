using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.game;

public class GameContainer
{
	public const int LAYER_BACKGROUND = 0;

	public const int MENU_LAYER_MENU = 1;

	public const int MENU_LAYER_INSTANCE = 2;

	public const int MENU_LAYER_DUNGEON = 3;

	public const int LAYER_PROJECT = 4;

	public const int LAYER_DIALOG = 5;

	public const int LAYER_UI = 6;

	public const int LAYER_TRANSITION = 7;

	public const int LAYER_CHAT_WINDOWS = 8;

	public const int LAYER_NOTIFICATION = 9;

	public const int LAYER_ERROR = 10;

	public const int LAYER_TOOLTIP = 11;

	public const int LAYER_TUTORIAL = 12;

	public const int LAYER_LOADING = 13;

	public const int LAYER_DEBUGGER = 14;

	public const int LAYER_COUNT = 15;

	private Dictionary<int, GameObject> layers = new Dictionary<int, GameObject>();

	private Dictionary<int, string> layersNames = new Dictionary<int, string>
	{
		[0] = "LAYER_BACKGROUND",
		[1] = "MENU_LAYER_MENU",
		[2] = "MENU_LAYER_INSTANCE",
		[3] = "MENU_LAYER_DUNGEON",
		[4] = "LAYER_PROJECT",
		[5] = "LAYER_DIALOG",
		[6] = "LAYER_UI",
		[7] = "LAYER_TRANSITION",
		[8] = "LAYER_CHAT_WINDOWS",
		[9] = "LAYER_NOTIFICATION",
		[10] = "LAYER_ERROR",
		[11] = "LAYER_TOOLTIP",
		[12] = "LAYER_TUTORIAL",
		[13] = "LAYER_LOADING",
		[14] = "LAYER_DEBUGGER"
	};

	public GameContainer()
	{
		for (int i = 0; i < 15; i++)
		{
			layers[i] = AddNewLayer(layersNames[i]);
		}
	}

	private GameObject AddNewLayer(string name)
	{
		GameObject gameObject = new GameObject();
		gameObject.transform.SetParent(GameData.instance.windowGenerator.canvas.transform, worldPositionStays: false);
		RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
		rectTransform.sizeDelta = Vector2.zero;
		rectTransform.anchorMin = Vector2.zero;
		rectTransform.anchorMax = Vector2.one;
		rectTransform.pivot = new Vector2(0.5f, 0.5f);
		gameObject.name = name;
		return gameObject;
	}

	public void AddToLayer(GameObject obj, int layer, bool front = true, bool center = true, bool resize = true)
	{
		foreach (KeyValuePair<int, GameObject> layer2 in layers)
		{
			if (layer2.Value != null)
			{
				layer2.Value.transform.SetAsLastSibling();
			}
		}
		if (!layers.ContainsKey(layer) || layers[layer] == null)
		{
			return;
		}
		obj.transform.SetParent(layers[layer].transform, worldPositionStays: false);
		if (center)
		{
			obj.GetComponent<RectTransform>().localPosition = new Vector2(0f, 0f);
		}
		if (resize)
		{
			obj.GetComponent<RectTransform>().sizeDelta = new Vector2(1f, 1f);
			obj.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
		}
		Canvas component = obj.GetComponent<Canvas>();
		if (component == null)
		{
			component = obj.AddComponent<Canvas>();
			obj.AddComponent<GraphicRaycaster>();
			component.overrideSorting = true;
			component.sortingLayerName = "UI";
			component.sortingOrder = layer * 1000;
			if (front)
			{
				component.sortingOrder = layer * 1000;
			}
			else
			{
				component.sortingOrder = layer * 1000 + 999;
			}
		}
		else
		{
			component.overrideSorting = true;
			component.sortingLayerName = "UI";
			component.sortingOrder = layer * 1000;
			if (front)
			{
				component.sortingOrder = layer * 1000;
			}
			else
			{
				component.sortingOrder = layer * 1000 + 999;
			}
		}
		if (front)
		{
			obj.transform.SetAsLastSibling();
		}
		else
		{
			obj.transform.SetAsFirstSibling();
		}
	}

	public GameObject GetLayerGO(int layer)
	{
		if (layers.ContainsKey(layer))
		{
			return layers[layer];
		}
		return null;
	}
}
