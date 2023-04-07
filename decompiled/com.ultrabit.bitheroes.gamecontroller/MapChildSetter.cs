using com.ultrabit.bitheroes.charactermov;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.ui;
using UnityEngine;

namespace com.ultrabit.bitheroes.gamecontroller;

public class MapChildSetter : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SetChildren()
	{
		MapMenu[] componentsInChildren = base.gameObject.GetComponentsInChildren<MapMenu>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].SetWindow(GameData.instance.windowGenerator);
		}
		MapDecoration[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<MapDecoration>();
		for (int i = 0; i < componentsInChildren2.Length; i++)
		{
			componentsInChildren2[i].SetLayer(GameData.instance.layerAdjustment);
		}
	}
}
