using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.language;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.character;

public class CharacterInfoListWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public Transform infoListContent;

	public CharacterInfoTile characterInfoItemPrefab;

	private List<CharacterInfoTile> _tiles = new List<CharacterInfoTile>();

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(List<CharacterInfoData> info, string name = null)
	{
		if (name == null)
		{
			name = Language.GetString("ui_info");
		}
		topperTxt.text = name;
		CreateTiles(info);
		StartCoroutine(WaitToFixText());
		ListenForBack(OnClose);
		CreateWindow(closeWord: false, "", scroll: false, stayUp: true);
	}

	private void CreateTiles(List<CharacterInfoData> info)
	{
		for (int i = 0; i < info.Count; i++)
		{
			CharacterInfoData statData = info[i];
			CharacterInfoTile characterInfoTile = Object.Instantiate(characterInfoItemPrefab, infoListContent);
			characterInfoTile.LoadDetails(statData);
			_tiles.Add(characterInfoTile);
		}
	}

	private void ClearTiles()
	{
		foreach (CharacterInfoTile tile in _tiles)
		{
			Object.Destroy(tile.gameObject);
		}
		_tiles.Clear();
	}

	private IEnumerator WaitToFixText()
	{
		yield return new WaitForSeconds(0.1f);
		infoListContent.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.MinSize;
		ForceScrollDown();
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
