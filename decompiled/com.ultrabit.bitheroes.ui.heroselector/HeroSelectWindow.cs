using System;
using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.extensions;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.login;
using com.ultrabit.bitheroes.ui.character;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.heroselector;

public class HeroSelectWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public HeroPanel _heroPanel;

	private List<CharacterData> _heroList;

	private CharacterData _heroSelected;

	[SerializeField]
	private Button refreshBtn;

	[SerializeField]
	private TextMeshProUGUI refreshTxt;

	[SerializeField]
	private Button confirmBtn;

	[SerializeField]
	private TextMeshProUGUI confirmTxt;

	[SerializeField]
	private Button kongBtn;

	[SerializeField]
	private TextMeshProUGUI kongTxt;

	[SerializeField]
	private SpriteMask leftMask;

	[SerializeField]
	private SpriteMask rightMask;

	private string _confirmLang;

	private string _createLang;

	private bool _kongLink;

	private bool _forceRelog;

	private static HeroSelectWindow _instance;

	private void Awake()
	{
		if (_instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		_instance = this;
	}

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(CharacterData heroSelected = null, bool showCloseBtn = true, bool forceRelog = false)
	{
		refreshBtn.gameObject.SetActive(value: false);
		confirmBtn.gameObject.SetActive(value: false);
		kongBtn.gameObject.SetActive(value: false);
		_heroSelected = heroSelected;
		_kongLink = !string.IsNullOrEmpty(AppInfo.kongID);
		_forceRelog = forceRelog;
		topperTxt.text = Language.GetString("ui_your_heroes");
		refreshTxt.text = Language.GetString("ui_refresh");
		kongTxt.text = Language.GetString("ui_link");
		_confirmLang = Language.GetString("ui_confirm");
		_createLang = Language.GetString("ui_create");
		leftMask.frontSortingOrder = base.sortingLayer + 5;
		leftMask.backSortingOrder = base.sortingLayer;
		rightMask.frontSortingOrder = base.sortingLayer + 5;
		rightMask.backSortingOrder = base.sortingLayer;
		CreateWindow(closeWord: false, "", scroll: false);
		kongBtn.onClick.AddListener(DoKongLink);
		refreshBtn.onClick.AddListener(DoGetCharacterList);
		closeBtn.gameObject.SetActive(showCloseBtn);
		if (showCloseBtn)
		{
			ListenForBack(OnClose);
		}
		DoGetCharacterList();
	}

	public void RefreshButtons()
	{
		kongBtn.gameObject.SetActive(!AppInfo.IsKongLogged());
		refreshBtn.gameObject.SetActive(AppInfo.IsKongLogged());
	}

	public void SetConfirmButton(bool confirmText)
	{
		confirmBtn.gameObject.SetActive(value: true);
		confirmTxt.text = (confirmText ? _confirmLang : _createLang);
		confirmBtn.onClick.RemoveAllListeners();
		if (confirmText)
		{
			confirmBtn.onClick.AddListener(DoConfirmCharacter);
		}
		else
		{
			confirmBtn.onClick.AddListener(OpenCharacterCustomizeWindow);
		}
	}

	private void OpenCharacterCustomizeWindow()
	{
		CharacterCustomizeWindow characterCustomizeWindow = GameData.instance.windowGenerator.NewCharacterCustomizeWindow(base.gameObject);
		characterCustomizeWindow.CREATED_CHARACTER = (Action<string, bool, int, int, int>)Delegate.Combine(characterCustomizeWindow.CREATED_CHARACTER, new Action<string, bool, int, int, int>(DoCreateCharacter));
	}

	public void DoCreateCharacter(string name, bool genderMale, int hairID, int hairColorID, int skinColorID)
	{
		GameData.instance.main.ShowLoading(Language.GetString("ui_loading_create_character"));
		PlayerDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(5), OnCreateCharacter);
		PlayerDALC.instance.doCreateCharacter(name, genderMale, hairID, hairColorID, skinColorID);
	}

	public void OnCreateCharacter(BaseEvent baseEvent)
	{
		GameData.instance.main.HideLoading();
		PlayerDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(5), OnCreateCharacter);
		SFSObject sfsob = (baseEvent as DALCEvent).sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		Character character = Character.fromSFSObject(sfsob);
		if (character == null)
		{
			ServerExtension.instance.Disconnect(null, null, relog: true);
		}
		GameData.instance.PROJECT.SetCharacter(character);
		GameData.instance.main.ContinueToTown();
		OnClose();
	}

	public void DoKongLink()
	{
		KongregateLogin.Link();
	}

	public void DoConfirmCharacter()
	{
		if (_heroPanel.heroTileSelectedData == null)
		{
			return;
		}
		if (GameData.instance.PROJECT.character != null && !_forceRelog && _heroPanel.heroTileSelectedData.charID == GameData.instance.PROJECT.character.id)
		{
			OnClose();
			return;
		}
		if (_heroPanel.heroTileSelectedData.nftState == Character.NFTState.bitverseAvatar)
		{
			GameData.instance.windowGenerator.NewCharacterNFTNameChangeWindow().NAME_CHANGE.AddListener(delegate(object e)
			{
				DoNFTNameChange((string)e);
			});
			return;
		}
		GameData.instance.main.ShowLoading(Language.GetString("ui_loading_confirm_character"));
		PlayerDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(6), OnConfirmCharacter);
		PlayerDALC.instance.doConfirmCharacter(_heroPanel.heroTileSelectedData.charID, _heroPanel.heroTileSelectedData.playerID);
		if (GameData.instance.PROJECT.character != null && _heroPanel.heroTileSelectedData.charID != GameData.instance.PROJECT.character.id)
		{
			KongregateAnalytics.TrackSwitchHeroes(GameData.instance.PROJECT.character.id, GameData.instance.PROJECT.character.nameAndHeroTag, GameData.instance.PROJECT.character.heroType, _heroPanel.heroTileSelectedData.charID, _heroPanel.heroTileSelectedData.nameAndHeroTag, _heroPanel.heroTileSelectedData.heroType);
		}
	}

	public void OnConfirmCharacter(BaseEvent baseEvent)
	{
		GameData.instance.main.HideLoading();
		PlayerDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(6), OnConfirmCharacter);
		SFSObject sfsob = (baseEvent as DALCEvent).sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
		else
		{
			ServerExtension.instance.Disconnect(null, null, relog: true);
		}
	}

	private void DoNFTNameChange(string name)
	{
		GameData.instance.main.ShowLoading(Language.GetString("ui_loading_nft_name_change"));
		PlayerDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(7), OnNFTNameChange);
		PlayerDALC.instance.doNFTNameChange(_heroPanel.heroTileSelectedData.nftToken, name);
	}

	public void OnNFTNameChange(BaseEvent baseEvent)
	{
		GameData.instance.main.HideLoading();
		PlayerDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(7), OnNFTNameChange);
		SFSObject sfsob = (baseEvent as DALCEvent).sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
		else
		{
			ServerExtension.instance.Disconnect(null, null, relog: true);
		}
	}

	public void DoGetCharacterList()
	{
		GameData.instance.main.ShowLoading(Language.GetString("ui_loading_get_character_list"));
		PlayerDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(4), OnGetCharacterList);
		PlayerDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(4), OnGetCharacterList);
		PlayerDALC.instance.doGetCharacterList();
	}

	public void OnGetCharacterList(BaseEvent baseEvent)
	{
		GameData.instance.main.HideLoading();
		PlayerDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(4), OnGetCharacterList);
		SFSObject sfsob = (baseEvent as DALCEvent).sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			DoDestroy();
			return;
		}
		if (sfsob.ContainsKey("lnk5") && sfsob.GetBool("lnk5"))
		{
			CharacterData characterData = CharacterData.fromSFSObject(sfsob);
			if (characterData == null)
			{
				AppInfo.LogoutNotification();
				DoDestroy();
			}
			else
			{
				int @int = sfsob.GetInt("use2");
				GameData.instance.windowGenerator.NewCharacterPlatformLinkWindow(characterData, @int);
				DoDestroy();
			}
			return;
		}
		SFSArray sFSArray = (sfsob.ContainsKey("pla14") ? ((SFSArray)sfsob.GetSFSArray("pla14")) : null);
		List<CharacterData> list = null;
		if (sFSArray != null && sFSArray.Size() > 0)
		{
			list = new List<CharacterData>();
			foreach (SFSObject item in (IEnumerable)sFSArray)
			{
				list.Add(CharacterData.fromSFSObject(item));
			}
		}
		_heroList = list;
		_heroPanel.LoadDetails(this, _heroList, _heroSelected);
		RefreshButtons();
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

	private void OnDestroy()
	{
		_instance = null;
	}

	public static void OpenWindow()
	{
		if (!AppInfo.CheckIfNFTLogged())
		{
			if (_instance == null)
			{
				GameData.instance.windowGenerator.NewHeroSelectWindow(GameData.instance.PROJECT.character.toCharacterData());
			}
			else
			{
				RefreshIfWindowExists();
			}
		}
	}

	public static void RefreshIfWindowExists()
	{
		if (_instance != null)
		{
			_instance.DoGetCharacterList();
		}
	}

	public static void ForceClose()
	{
		if (_instance != null)
		{
			_instance.OnClose();
		}
	}
}
