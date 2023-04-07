using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.augment;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.ui.utility;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.augment;

public class AugmentSlotSelectWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI descTxt;

	public Transform placeholderSlots;

	public Transform augmentTilePrefab;

	private AugmentData _augmentData;

	private FamiliarRef _familiarRef;

	private List<AugmentTile> _tiles;

	private int _selectedSlot = -1;

	[HideInInspector]
	public UnityCustomEvent SELECT = new UnityCustomEvent();

	public int selectedSlot => _selectedSlot;

	public FamiliarRef familiarRef => _familiarRef;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(AugmentData augmentData, FamiliarRef familiarRef)
	{
		_augmentData = augmentData;
		_familiarRef = familiarRef;
		topperTxt.text = Language.GetString("ui_equip");
		descTxt.text = Language.GetString("ui_enchant_slot_desc", new string[1] { ItemRef.GetItemName(15) });
		CreateTiles();
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void CreateTiles()
	{
		if (_tiles != null)
		{
			return;
		}
		_tiles = new List<AugmentTile>();
		foreach (AugmentSlotRef typeSlot in AugmentBook.GetTypeSlots(_augmentData.augmentRef.typeRef.id))
		{
			Augments augments = GameData.instance.PROJECT.character.augments;
			AugmentData familiarAugmentSlot = augments.getFamiliarAugmentSlot(_familiarRef, typeSlot.id);
			Transform obj = Object.Instantiate(augmentTilePrefab);
			obj.SetParent(placeholderSlots, worldPositionStays: false);
			AugmentTile component = obj.GetComponent<AugmentTile>();
			bool borders = false;
			if (familiarAugmentSlot != null)
			{
				borders = true;
			}
			component.LoadDetails(typeSlot.id, augments, familiarAugmentSlot, _familiarRef, changeable: false, selectable: true, borders, this);
			component.OverrideAugmentBG(1);
			component.SELECT.AddListener(OnTileSelect);
			_tiles.Add(component);
		}
	}

	private void OnTileSelect(object e)
	{
		AugmentTile augmentTile = e as AugmentTile;
		if (!augmentTile.locked)
		{
			_selectedSlot = augmentTile.slot;
			SELECT.Invoke(this);
		}
	}

	public override void DoDestroy()
	{
		foreach (AugmentTile tile in _tiles)
		{
			if (tile != null)
			{
				tile.SELECT.RemoveListener(OnTileSelect);
			}
		}
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
	}
}
