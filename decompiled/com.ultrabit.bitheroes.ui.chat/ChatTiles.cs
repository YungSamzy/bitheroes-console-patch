using System;
using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.ui.language;
using com.ultrabit.bitheroes.ui.menu;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.chat;

public class ChatTiles : MonoBehaviour
{
	public Transform menuInterfaceChatTilePrefab;

	private List<GameObject> _chatTiles;

	private AsianLanguageFontManager asianLangManager;

	public List<GameObject> chatTiles => _chatTiles;

	public void LoadDetails()
	{
		_chatTiles = new List<GameObject>();
		Transform obj = UnityEngine.Object.Instantiate(menuInterfaceChatTilePrefab);
		obj.SetParent(base.transform, worldPositionStays: false);
		MenuInterfaceChatTile component = obj.GetComponent<MenuInterfaceChatTile>();
		component.Create();
		_chatTiles.Add(component.gameObject);
		asianLangManager = base.gameObject.GetComponent<AsianLanguageFontManager>();
		if (asianLangManager == null)
		{
			asianLangManager = base.gameObject.AddComponent<AsianLanguageFontManager>();
		}
		if (asianLangManager != null)
		{
			asianLangManager.SetAsianFontsIfNeeded();
		}
		StartCoroutine(WaitAndChangeLayer());
	}

	private IEnumerator WaitAndChangeLayer()
	{
		yield return new WaitForSeconds(1f);
		Vector2 vector = base.transform.position;
		Main.CONTAINER.AddToLayer(base.gameObject, 6, front: false, center: false, resize: false);
		base.transform.position = vector;
	}

	public GameObject GetTileByClass(Type type)
	{
		foreach (GameObject chatTile in _chatTiles)
		{
			if (chatTile.GetComponent(type) != null)
			{
				return chatTile;
			}
		}
		return null;
	}
}
