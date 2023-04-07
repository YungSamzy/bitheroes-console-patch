using System.Collections.Generic;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.ui.menu;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.battle;

public class BattleTiles : MonoBehaviour
{
	public Transform menuInterfaceAutoPilotTilePrefab;

	private List<GameObject> _battleTiles;

	public List<GameObject> battleTiles => _battleTiles;

	public void LoadDetails()
	{
		_battleTiles = new List<GameObject>();
		Transform obj = Object.Instantiate(menuInterfaceAutoPilotTilePrefab);
		obj.SetParent(base.transform, worldPositionStays: false);
		obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
		MenuInterfaceAutoPilotTile component = obj.GetComponent<MenuInterfaceAutoPilotTile>();
		component.Create();
		_battleTiles.Add(component.gameObject);
		RepositionObjects();
	}

	public void RepositionObjects()
	{
		foreach (GameObject battleTile in _battleTiles)
		{
			battleTile.GetComponent<RectTransform>().anchoredPosition -= new Vector2(AppInfo.GetRightOffset(), 0f);
		}
	}
}
