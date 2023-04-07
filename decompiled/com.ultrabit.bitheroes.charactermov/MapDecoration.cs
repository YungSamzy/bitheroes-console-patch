using System.Collections.Generic;
using com.ultrabit.bitheroes.gamecontroller;
using UnityEngine;

namespace com.ultrabit.bitheroes.charactermov;

public class MapDecoration : MonoBehaviour
{
	private LayerAdjustment layerAdjust;

	private List<SpriteRenderer> decorationRender = new List<SpriteRenderer>();

	public bool onlyImage;

	private void Start()
	{
	}

	public void SetLayer(LayerAdjustment layer)
	{
		layerAdjust = layer;
		if (onlyImage)
		{
			decorationRender.Add(GetComponent<SpriteRenderer>());
		}
		else
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				if (base.transform.GetChild(i).GetComponent<SpriteRenderer>() != null)
				{
					decorationRender.Add(base.transform.GetChild(i).GetComponent<SpriteRenderer>());
				}
			}
		}
		layerAdjust.SmartChangeLayer(decorationRender, base.gameObject.transform);
	}
}
