using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.craft;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.rune;
using com.ultrabit.bitheroes.ui.shop;
using com.ultrabit.bitheroes.ui.tutorial;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.rune;

public class RunesWindow : WindowsMain
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

	private Runes _runes;

	private bool _changeable;

	private List<RuneTile> _tiles;

	public Transform runeTilePrefab;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(Runes runes, bool changeable = false)
	{
		_runes = runes;
		_changeable = changeable;
		topperTxt.text = ItemRef.GetItemNamePlural(9);
		if (_changeable)
		{
			GameData.instance.PROJECT.character.AddListener("RUNES_CHANGE", OnRunesChange);
		}
		viewBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_browse");
		viewBtn.gameObject.SetActive(_changeable);
		CreateTiles();
		ListenForBack(OnClose);
		SCROLL_IN_COMPLETE.AddListener(OnScrollInComplete);
		CreateWindow();
	}

	private void OnScrollInComplete(object e)
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		CheckTutorial();
	}

	private void CheckTutorial()
	{
		if (GameData.instance.tutorialManager == null || GameData.instance.tutorialManager.hasPopup || GameData.instance.tutorialManager.canvas == null || !GameData.instance.PROJECT.character.tutorial.GetState(90) || GameData.instance.PROJECT.character.tutorial.GetState(91))
		{
			return;
		}
		GameData.instance.PROJECT.character.tutorial.SetState(91);
		GameData.instance.PROJECT.CheckTutorialChanges();
		List<ItemData> itemsByType = GameData.instance.PROJECT.character.inventory.GetItemsByType(9);
		if (itemsByType.Count <= 0)
		{
			return;
		}
		foreach (RuneTile tile in _tiles)
		{
			if (tile.runeType == (itemsByType[0].itemRef as RuneRef).runeType)
			{
				GameData.instance.tutorialManager.ShowTutorialForEventTrigger(tile.gameObject, new TutorialPopUpSettings(Tutorial.GetText(90), 4, tile.gameObject), EventTriggerType.PointerClick, stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
				break;
			}
		}
	}

	private void CreateTiles()
	{
		_tiles = new List<RuneTile>();
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
		RuneTile component = obj.GetComponent<RuneTile>();
		component.LoadDetails(this, slot, _runes, _runes.getRuneSlot(slot), _changeable, runeType);
		_tiles.Add(component);
	}

	private void UpdateTiles()
	{
		_runes = GameData.instance.PROJECT.character.runes;
		foreach (RuneTile tile in _tiles)
		{
			RuneRef runeSlot = _runes.getRuneSlot(tile.slot);
			tile.SetRunes(_runes);
			tile.SetRune(runeSlot);
		}
	}

	public List<RuneTile> GetRuneDifferenceTiles(Runes runes)
	{
		List<RuneTile> list = new List<RuneTile>();
		foreach (RuneTile tile in _tiles)
		{
			if (_runes.getRuneSlot(tile.slot) != runes.getRuneSlot(tile.slot))
			{
				list.Add(tile);
			}
		}
		return list;
	}

	public void AnimateTiles(List<RuneTile> tiles)
	{
		foreach (RuneTile tile in tiles)
		{
			tile.Animate();
		}
	}

	public RuneTile GetRuneSlot(int slot)
	{
		foreach (RuneTile tile in _tiles)
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
		if (GameData.instance.PROJECT.character.inventory.GetItemsByType(9).Count > 0)
		{
			GameData.instance.windowGenerator.NewRuneSelectWindow(base.gameObject);
		}
		else
		{
			GameData.instance.windowGenerator.NewClosablePromptMessageWindow(Language.GetString("error_name"), Language.GetString("error_no_runes"), Language.GetString("ui_craft"), Language.GetString("ui_shop"), OnRunesErrorCraft, OnRunesErrorShop);
		}
	}

	private void OnRunesErrorCraft()
	{
		GameData.instance.windowGenerator.NewCraftTradeWindow(CraftBook.getItemsByResultType(9));
	}

	private void OnRunesErrorShop()
	{
		GameData.instance.windowGenerator.NewShopWindow(new int[4] { 0, 1, 2, 3 }, ShopWindow.TAB_FEATURED);
	}

	public override void DoDestroy()
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		if (_changeable)
		{
			GameData.instance.PROJECT.character.RemoveListener("RUNES_CHANGE", OnRunesChange);
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
