using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.augment;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.ui.game;
using com.ultrabit.bitheroes.ui.item;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.augment;

public class AugmentDataTile : MonoBehaviour
{
	public ItemIcon itemIcon;

	public RectTransform placeholderModifier;

	public Transform gameModifierBtnPrefab;

	private AugmentData _augmentData;

	private List<GameModifierBtn> _tiles = new List<GameModifierBtn>();

	public void LoadDetails(AugmentData augmentData)
	{
		_augmentData = augmentData;
		itemIcon.SetItemData(augmentData);
		itemIcon.SetItemActionType(0);
		UpdateItemIconComparission(augmentData.equipped);
		for (int i = 0; i < _tiles.Count; i++)
		{
			Object.Destroy(_tiles[i].gameObject);
		}
		_tiles.Clear();
		List<GameModifier> gameModifiers = augmentData.getGameModifiers(augmentData.getRank(GameData.instance.PROJECT.character.familiarStable));
		Debug.Log(gameModifiers.Count + "Augment modifiers");
		foreach (GameModifier item in gameModifiers)
		{
			Transform transform = Object.Instantiate(gameModifierBtnPrefab);
			transform.SetParent(placeholderModifier, worldPositionStays: false);
			transform.GetComponent<GameModifierBtn>().SetText(item.GetTileDesc(_augmentData));
			_tiles.Add(transform.GetComponent<GameModifierBtn>());
		}
	}

	public void UpdateItemIconComparission(bool value)
	{
		if (value)
		{
			itemIcon.PlayComparison("E");
		}
		else
		{
			itemIcon.HideComparison();
		}
	}

	public void DoEnable()
	{
		foreach (GameModifierBtn tile in _tiles)
		{
			tile.GetComponent<Button>().interactable = true;
		}
	}

	public void DoDisable()
	{
		foreach (GameModifierBtn tile in _tiles)
		{
			tile.GetComponent<Button>().interactable = false;
		}
	}
}
