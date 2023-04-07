using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.lists.admincharacterplatformslist;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.admin;

public class AdminCharacterPlatformsWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public Button addBtn;

	public AdminCharacterPlatformsList adminCharacterPlatformsList;

	private int _charID;

	private List<CharacterPlatformData> _platforms;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(int charID, List<CharacterPlatformData> platforms)
	{
		_charID = charID;
		topperTxt.text = "Platforms";
		addBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_add");
		adminCharacterPlatformsList.InitList(OnTileSelected);
		SetPlatforms(platforms);
		ListenForBack(OnClose);
		CreateWindow();
	}

	public void SetPlatforms(List<CharacterPlatformData> platforms)
	{
		_platforms = platforms;
		CreateTiles(platforms);
	}

	private void CreateTiles(List<CharacterPlatformData> platforms)
	{
		adminCharacterPlatformsList.ClearList();
		for (int i = 0; i < platforms.Count; i++)
		{
			CharacterPlatformData platformData = platforms[i];
			adminCharacterPlatformsList.Data.InsertOneAtEnd(new AdminPlatformItem
			{
				platformData = platformData
			});
		}
	}

	private void OnTileSelected(CharacterPlatformData platformData)
	{
		if (platformData.active)
		{
			DoPlatformDisable(platformData);
		}
		else
		{
			DoPlatformEnable(platformData);
		}
	}

	private void DoPlatformEnable(CharacterPlatformData platformData)
	{
		GameData.instance.main.ShowLoading();
		AdminDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(20), OnPlatformEnable);
		AdminDALC.instance.doCharacterPlatformEnable(_charID, platformData.id, platformData.platform, platformData.userID);
	}

	private void OnPlatformEnable(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		AdminDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(20), OnPlatformEnable);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			int @int = sfsob.GetInt("err0");
			if (@int == 0)
			{
				int int2 = sfsob.GetInt("cha1");
				GameData.instance.windowGenerator.ShowError(Util.ParseString("This platform and user id is already linked to character id ^" + int2 + "^"));
			}
			else
			{
				GameData.instance.windowGenerator.ShowErrorCode(@int);
			}
			return;
		}
		List<CharacterPlatformData> platforms = CharacterPlatformData.listFromSFSObject(sfsob);
		SetPlatforms(platforms);
		AdminCharacterWindow adminCharacterWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(AdminCharacterWindow)) as AdminCharacterWindow;
		if (adminCharacterWindow != null)
		{
			adminCharacterWindow.DoRefresh();
		}
	}

	private void DoPlatformDisable(CharacterPlatformData platformData)
	{
		GameData.instance.main.ShowLoading();
		AdminDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(21), OnPlatformDisable);
		AdminDALC.instance.doCharacterPlatformDisable(_charID, platformData.id);
	}

	private void OnPlatformDisable(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		AdminDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(21), OnPlatformDisable);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		List<CharacterPlatformData> platforms = CharacterPlatformData.listFromSFSObject(sfsob);
		SetPlatforms(platforms);
		AdminCharacterWindow adminCharacterWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(AdminCharacterWindow)) as AdminCharacterWindow;
		if (adminCharacterWindow != null)
		{
			adminCharacterWindow.DoRefresh();
		}
	}

	public void OnAddBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewAdminCharacterPlatformAddWindow(_charID, base.gameObject);
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		addBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		addBtn.interactable = false;
	}
}
