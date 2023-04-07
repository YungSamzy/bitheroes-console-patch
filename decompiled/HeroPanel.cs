using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.ui.heroselector;
using UnityEngine;
using UnityEngine.UI;

public class HeroPanel : MonoBehaviour
{
	public HorizontalLayoutGroup heroContent;

	private ScrollRect scrollRect;

	[SerializeField]
	private HeroTileRenderTexture heroTile;

	[SerializeField]
	private HeroStats heroStats;

	private List<CharacterData> _heroList;

	private CharacterData _heroSelected;

	private HeroTileRenderTexture _heroTileSelected;

	private HeroSelectWindow _heroSelectWindow;

	public CharacterData heroSelected => _heroSelected;

	public CharacterData heroTileSelectedData => _heroTileSelected.characterData;

	private void Awake()
	{
		scrollRect = GetComponentInChildren<ScrollRect>();
	}

	public void LoadDetails(HeroSelectWindow heroSelectWindow, List<CharacterData> heroList = null, CharacterData heroSelected = null)
	{
		_heroSelectWindow = heroSelectWindow;
		_heroList = heroList;
		_heroSelected = heroSelected;
		CreateTiles();
	}

	private void CreateTiles()
	{
		ClearTiles();
		if (_heroList == null || _heroList.Count == 0)
		{
			CreateSingleTile(null);
		}
		else
		{
			foreach (CharacterData hero in _heroList)
			{
				CreateSingleTile(hero);
			}
		}
		CheckSelection();
		UpdateScrollView();
	}

	private void CreateSingleTile(CharacterData characterData)
	{
		HeroTileRenderTexture heroTileRenderTexture = UnityEngine.Object.Instantiate(heroTile, heroContent.transform);
		heroTileRenderTexture.OnSelect = (Action<HeroTileRenderTexture>)Delegate.Combine(heroTileRenderTexture.OnSelect, new Action<HeroTileRenderTexture>(OnHeroSelected));
		heroTileRenderTexture.LoadDetails(characterData, characterData?.Equals(heroSelected) ?? false);
	}

	private void ClearTiles()
	{
		foreach (Transform item in heroContent.transform)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
		HeroTileRenderTexture.HERO_TILES.Clear();
	}

	private void CheckSelection()
	{
		if (HeroTileRenderTexture.HERO_TILES == null || HeroTileRenderTexture.HERO_TILES.Count == 0)
		{
			return;
		}
		int num = 0;
		foreach (HeroTileRenderTexture hERO_TILE in HeroTileRenderTexture.HERO_TILES)
		{
			if (hERO_TILE.isSelected)
			{
				num++;
			}
		}
		if (num != 1)
		{
			HeroTileRenderTexture.HERO_TILES[0].isSelected = true;
		}
	}

	private void UpdateScrollView()
	{
		heroContent.gameObject.SetActive(value: true);
		if (_heroList == null || _heroList.Count <= 2)
		{
			scrollRect.movementType = ScrollRect.MovementType.Clamped;
			heroContent.childAlignment = TextAnchor.UpperCenter;
			heroContent.padding.top = 4;
		}
		else
		{
			scrollRect.movementType = ScrollRect.MovementType.Elastic;
			heroContent.childAlignment = TextAnchor.UpperLeft;
			heroContent.padding.top = 0;
		}
	}

	private void OnHeroSelected(HeroTileRenderTexture hTile)
	{
		_heroTileSelected = hTile;
		_heroSelectWindow.SetConfirmButton(_heroTileSelected.hasData);
		heroStats.SetStats(_heroTileSelected.characterData);
	}
}
