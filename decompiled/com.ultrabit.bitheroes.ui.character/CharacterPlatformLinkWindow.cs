using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.extensions;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.heroselector;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace com.ultrabit.bitheroes.ui.character;

public class CharacterPlatformLinkWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI descTxt;

	public Transform placeholderCurrent;

	public Transform placeholderLinked;

	public Transform tilePrefab;

	private CharacterData _currentData;

	private CharacterData _linkedData;

	private int _platform;

	private CharacterPlatformLinkTile _currentTile;

	private CharacterPlatformLinkTile _linkedTile;

	private int _maxSortingOrder;

	public override void Start()
	{
		base.Start();
		Disable();
		Canvas componentInParent = GetComponentInParent<Canvas>();
		if (componentInParent != null)
		{
			_maxSortingOrder = componentInParent.sortingOrder + 10;
			placeholderCurrent.GetComponent<SortingGroup>().sortingOrder = _maxSortingOrder;
			placeholderLinked.GetComponent<SortingGroup>().sortingOrder = _maxSortingOrder;
		}
	}

	public void LoadDetails(CharacterData linkedData, int platform)
	{
		_currentData = GameData.instance.PROJECT.character?.toCharacterData();
		_linkedData = linkedData;
		_platform = platform;
		if (_currentData == null)
		{
			DoConfirmLinkage(_linkedData.playerID, _linkedData.charID, -1, -1);
			DoDestroy();
			return;
		}
		topperTxt.text = Language.GetString("ui_select");
		descTxt.text = Language.GetString("ui_select_account_desc");
		Transform transform = Object.Instantiate(tilePrefab);
		transform.SetParent(placeholderCurrent, worldPositionStays: false);
		_currentTile = transform.GetComponent<CharacterPlatformLinkTile>();
		_currentTile.LoadDetails(_currentData, this);
		_currentTile.SELECT.AddListener(OnTileSelect);
		_currentTile.DoDisable();
		transform = null;
		transform = Object.Instantiate(tilePrefab);
		transform.SetParent(placeholderLinked, worldPositionStays: false);
		_linkedTile = transform.GetComponent<CharacterPlatformLinkTile>();
		_linkedTile.LoadDetails(_linkedData, this);
		_linkedTile.SELECT.AddListener(OnTileSelect);
		_linkedTile.DoDisable();
		CreateWindow();
		bool status = _currentData.playerID == _linkedData.playerID;
		HideOrShowCloseButton(status);
		CheckAsianFont(overrideResized: true);
	}

	private void HideOrShowCloseButton(bool status)
	{
		if (status)
		{
			ActivateCloseBtn();
		}
		else
		{
			DeactivateCloseBtn();
		}
		closeBtn.gameObject.SetActive(status);
	}

	public void OnTileSelect(object e)
	{
		CharacterPlatformLinkTile characterPlatformLinkTile = e as CharacterPlatformLinkTile;
		DoConfirmSelect(characterPlatformLinkTile.characterData);
	}

	private void DoConfirmSelect(CharacterData selectedCharacter)
	{
		string platformName = AppInfo.GetPlatformName(_platform);
		CharacterData ignoredCharacter = ((selectedCharacter == _currentData) ? _linkedData : _currentData);
		if (selectedCharacter.name.Length <= 0 || ignoredCharacter.name.Length <= 0)
		{
			DoConfirmLinkage(selectedCharacter.playerID, selectedCharacter.charID, ignoredCharacter.playerID, ignoredCharacter.charID);
			return;
		}
		string text = ((selectedCharacter.charID != _linkedData.charID) ? Language.GetString("ui_select_account_current_confirm", new string[3] { selectedCharacter.name, ignoredCharacter.name, platformName }, color: true) : Language.GetString("ui_select_account_other_confirm", new string[2] { selectedCharacter.name, ignoredCharacter.name }, color: true));
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("notification_are_you_sure"), Util.ParseString(text) + " " + Language.GetString("notification_lost_hero"), null, null, delegate
		{
			DoConfirmLinkage(selectedCharacter.playerID, selectedCharacter.charID, ignoredCharacter.playerID, ignoredCharacter.charID);
		}).SetSortingOrder(_maxSortingOrder);
	}

	private void DoConfirmLinkage(int playerIDSelected, int charIDSelected, int playerIDIgnored, int charIDIgnored)
	{
		GameData.instance.main.ShowLoading(Language.GetString("ui_loading_get_character_list"));
		PlayerDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(8), OnConfirmLinkage);
		PlayerDALC.instance.doConfirmLinkage(playerIDSelected, charIDSelected, playerIDIgnored, charIDIgnored);
	}

	private void OnConfirmLinkage(BaseEvent baseEvent)
	{
		GameData.instance.main.HideLoading();
		PlayerDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(8), OnConfirmLinkage);
		SFSObject sfsob = (baseEvent as DALCEvent).sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
		else if (sfsob.ContainsKey("pla13") && sfsob.GetBool("pla13"))
		{
			Dispose();
		}
		else
		{
			ServerExtension.instance.Disconnect(null, null, relog: true);
		}
	}

	private CharacterData GetSelectedDataFromID(int charID)
	{
		if (charID == _currentData.charID)
		{
			return _currentData;
		}
		if (charID == _linkedData.charID)
		{
			return _linkedData;
		}
		return null;
	}

	private CharacterData GetOtherDataFromID(int charID)
	{
		if (charID == _currentData.charID)
		{
			return _linkedData;
		}
		if (charID == _linkedData.charID)
		{
			return _currentData;
		}
		return null;
	}

	public override void OnClose()
	{
		GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("ui_confirm"), Language.GetString("ui_cancel_confirm"), null, null, delegate
		{
			base.OnClose();
			HeroSelectWindow.RefreshIfWindowExists();
		}).SetSortingOrder(_maxSortingOrder);
	}

	public void Dispose()
	{
		base.OnClose();
		HeroSelectWindow.RefreshIfWindowExists();
	}

	public override void DoDestroy()
	{
		if (_currentTile != null)
		{
			_currentTile.SELECT.RemoveListener(OnTileSelect);
		}
		if (_linkedTile != null)
		{
			_linkedTile.SELECT.RemoveListener(OnTileSelect);
		}
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		if (_currentTile != null)
		{
			_currentTile.DoEnable();
		}
		if (_linkedTile != null)
		{
			_linkedTile.DoEnable();
		}
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		if (_currentTile != null)
		{
			_currentTile.DoDisable();
		}
		if (_linkedTile != null)
		{
			_linkedTile.DoDisable();
		}
	}
}
