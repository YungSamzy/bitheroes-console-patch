using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.armory.rune;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.rune;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.rune;

public class ArmoryRunesWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public RectTransform placeholderMajorA;

	public RectTransform placeholderMajorB;

	public RectTransform placeholderMajorC;

	public RectTransform placeholderMajorD;

	public RectTransform placeholderMinorA;

	public RectTransform placeholderMinorB;

	public RectTransform placeholderMeta;

	public RectTransform placeholderRelic;

	public RectTransform placeholderArtifact;

	public Button viewBtn;

	private ArmoryRunes _runes;

	private bool _changeable;

	private List<ArmoryRuneTile> _tiles;

	public Transform runeTilePrefab;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(ArmoryRunes runes, bool changeable = false)
	{
		_runes = runes;
		_changeable = changeable;
		topperTxt.text = ItemRef.GetItemNamePlural(9);
		if (_changeable)
		{
			GameData.instance.PROJECT.character.AddListener("armoryRuneChange", OnRunesChange);
		}
		viewBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_browse");
		viewBtn.gameObject.SetActive(_changeable);
		CreateTiles();
		ListenForBack(OnClose);
		forceAnimation = true;
		CreateWindow();
	}

	private void CreateTiles()
	{
		_tiles = new List<ArmoryRuneTile>();
		CreateTile(0, placeholderMajorA, 1);
		CreateTile(1, placeholderMajorB, 1);
		CreateTile(2, placeholderMajorC, 1);
		CreateTile(3, placeholderMajorD, 1);
		CreateTile(4, placeholderMinorA, 2);
		CreateTile(5, placeholderMinorB, 2);
		CreateTile(6, placeholderMeta, 3);
		CreateTile(7, placeholderRelic, 4);
		CreateTile(8, placeholderArtifact, 5);
	}

	private void CreateTile(int slot, RectTransform placeholder, int runeType)
	{
		Transform obj = Object.Instantiate(runeTilePrefab);
		obj.SetParent(placeholder, worldPositionStays: false);
		ArmoryRuneTile component = obj.GetComponent<ArmoryRuneTile>();
		component.LoadDetails(this, slot, _runes, _runes.getRuneSlot(slot), _changeable, runeType);
		_tiles.Add(component);
	}

	private void UpdateTiles()
	{
		_runes = GameData.instance.PROJECT.character.armory.currentArmoryEquipmentSlot.runes;
		foreach (ArmoryRuneTile tile in _tiles)
		{
			RuneRef runeSlot = _runes.getRuneSlot(tile.slot);
			tile.SetRunes(_runes);
			tile.SetRune(runeSlot);
		}
	}

	public List<ArmoryRuneTile> GetRuneDifferenceTiles(ArmoryRunes runes)
	{
		List<ArmoryRuneTile> list = new List<ArmoryRuneTile>();
		foreach (ArmoryRuneTile tile in _tiles)
		{
			if (_runes.getRuneSlot(tile.slot) != runes.getRuneSlot(tile.slot))
			{
				list.Add(tile);
			}
		}
		return list;
	}

	public void AnimateTiles(List<ArmoryRuneTile> tiles)
	{
		foreach (ArmoryRuneTile tile in tiles)
		{
			tile.Animate();
		}
	}

	public ArmoryRuneTile GetRuneSlot(int slot)
	{
		foreach (ArmoryRuneTile tile in _tiles)
		{
			if (tile.slot == slot)
			{
				return tile;
			}
		}
		return null;
	}

	private void OnRunesChange()
	{
		UpdateTiles();
	}

	public void OnViewBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewRuneSelectWindow(base.gameObject);
	}

	public override void DoDestroy()
	{
		if (_changeable)
		{
			GameData.instance.PROJECT.character.RemoveListener("armoryRuneChange", OnRunesChange);
		}
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		if (_tiles != null)
		{
			for (int i = 0; i < _tiles.Count; i++)
			{
				if (_tiles[i] != null)
				{
					_tiles[i].interactable = true;
				}
			}
		}
		viewBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		if (_tiles != null)
		{
			for (int i = 0; i < _tiles.Count; i++)
			{
				if (_tiles[i] != null)
				{
					_tiles[i].interactable = false;
				}
			}
		}
		viewBtn.interactable = false;
	}
}
