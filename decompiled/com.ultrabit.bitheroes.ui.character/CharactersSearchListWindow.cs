using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.ui.lists.characterssearchlist;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.character;

public class CharactersSearchListWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI descTxt;

	public Image loadingIcon;

	public CharactersSearchList charactersSearchList;

	private List<CharacterHeroTagData> _list;

	private int _total;

	private bool _showSelect;

	private string _nameSelected = "";

	private UnityAction<string> _selectCallback;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(List<CharacterHeroTagData> list, int total, bool showSelect = false, UnityAction<string> selectCallback = null)
	{
		ShowLoading();
		_list = list;
		_total = total;
		_showSelect = showSelect;
		_selectCallback = selectCallback;
		descTxt.text = Language.GetString("posibles_users_search");
		topperTxt.text = Language.GetString("ui_search");
		charactersSearchList.InitList(OnTileSelect);
		CreateTiles();
		HideLoading();
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void CreateTiles()
	{
		charactersSearchList.ClearList();
		foreach (CharacterHeroTagData item in _list)
		{
			charactersSearchList.Data.InsertOneAtEnd(new CharacterSearchItem
			{
				character = item,
				posId = item.charID,
				showSelect = _showSelect
			});
		}
	}

	private void OnTileSelect(string name)
	{
		_nameSelected = name;
		if (_selectCallback != null)
		{
			_selectCallback(name);
		}
		OnClose();
	}

	public string GetSelectedName()
	{
		return _nameSelected;
	}

	private void ShowLoading()
	{
		GameData.instance.main.ShowLoading();
	}

	private void HideLoading()
	{
		loadingIcon.gameObject.SetActive(value: false);
		GameData.instance.main.HideLoading();
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
