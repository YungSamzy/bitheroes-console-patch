using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.augment;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.augment;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.familiar;

public class FamiliarAugmentsPanel : FamiliarPanel
{
	public RectTransform placeholderSlotA;

	public RectTransform placeholderSlotB;

	public RectTransform placeholderSlotC;

	public RectTransform placeholderSlotD;

	public RectTransform placeholderSlotE;

	public RectTransform placeholderSlotF;

	public Transform augmentTilePrefab;

	private FamiliarWindow _familiarWindow;

	private FamiliarRef _familiarRef;

	private Augments _augments;

	private List<AugmentTile> _tiles;

	public void LoadDetails(FamiliarWindow familiarWindow, FamiliarRef familiarRef, Augments augments)
	{
		_familiarWindow = familiarWindow;
		_familiarRef = familiarRef;
		_augments = augments;
		_tiles = new List<AugmentTile>();
		CreateTile(0, placeholderSlotA);
		CreateTile(1, placeholderSlotB);
		CreateTile(2, placeholderSlotC);
		CreateTile(3, placeholderSlotD);
		CreateTile(4, placeholderSlotE);
		CreateTile(5, placeholderSlotF);
	}

	private void CreateTile(int slot, RectTransform placeholder)
	{
		bool changeable = ((!(GameData.instance.PROJECT.battle != null)) ? true : false);
		Transform obj = Object.Instantiate(augmentTilePrefab);
		obj.SetParent(placeholder, worldPositionStays: false);
		AugmentTile component = obj.GetComponent<AugmentTile>();
		component.LoadDetails(slot, _augments, _augments.getFamiliarAugmentSlot(_familiarRef, slot), _familiarRef, changeable, selectable: false, borders: true);
		_tiles.Add(component);
		if (!_familiarRef.allowAugmentSlot(slot))
		{
			Util.SetButton(component.GetComponent<Button>(), enabled: false);
		}
	}

	public override void DoUpdate()
	{
		if (_familiarWindow != null && _familiarWindow.currentTab == 1)
		{
			UpdateTiles();
		}
	}

	private void UpdateTiles()
	{
		_augments = GameData.instance.PROJECT.character.augments;
		foreach (AugmentTile tile in _tiles)
		{
			AugmentData familiarAugmentSlot = _augments.getFamiliarAugmentSlot(_familiarRef, tile.slot);
			tile.SetAugments(_augments);
			tile.SetAugment(familiarAugmentSlot);
		}
	}

	public void AnimateTiles(List<AugmentTile> tiles)
	{
		foreach (AugmentTile tile in tiles)
		{
			_ = tile;
		}
	}

	public AugmentTile GetAugmentSlot(int slot)
	{
		foreach (AugmentTile tile in _tiles)
		{
			if (tile.slot == slot)
			{
				return tile;
			}
		}
		return null;
	}

	public override void DoShow()
	{
		base.DoShow();
		base.gameObject.SetActive(value: true);
		base.transform.SetAsLastSibling();
	}

	public override void DoHide()
	{
		base.DoHide();
		base.gameObject.SetActive(value: false);
	}
}
