using System.Collections.Generic;
using com.ultrabit.bitheroes.model.character;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.character;

public class CharacterInfoTile : MonoBehaviour
{
	private static string BREAK_LINE = "<br>";

	public CharacterInfoStat characterInfoStatOutlinePrefab;

	public CharacterInfoStat characterInfoStatNoOutlinePrefab;

	public TextMeshProUGUI nameTxt;

	public Transform statsPlaceholder;

	private List<CharacterInfoStat> _statTiles = new List<CharacterInfoStat>();

	public void LoadDetails(CharacterInfoData statData)
	{
		nameTxt.text = statData.name;
		ClearTiles();
		foreach (CharacterInfoValue item in statData.info)
		{
			CharacterInfoStat characterInfoStat = Object.Instantiate(statData.outline ? characterInfoStatOutlinePrefab : characterInfoStatNoOutlinePrefab, statsPlaceholder);
			characterInfoStat.LoadDetails(statData, item);
			_statTiles.Add(characterInfoStat);
		}
	}

	private void ClearTiles()
	{
		foreach (CharacterInfoStat statTile in _statTiles)
		{
			Object.Destroy(statTile.gameObject);
		}
		_statTiles.Clear();
	}
}
