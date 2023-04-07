using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.admin;

public class AdminCharacterPlatformAddWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI platformNameTxt;

	public TextMeshProUGUI userIDNameTxt;

	private int _charID;

	public Button addBtn;

	public TMP_InputField userIDTxt;

	public Image platformDropdown;

	private Transform window;

	private MyDropdownItemModel _currentModel;

	private List<MyDropdownItemModel> _objects = new List<MyDropdownItemModel>();

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(int charID)
	{
		_charID = charID;
		topperTxt.text = Language.GetString("ui_add");
		userIDNameTxt.text = "User ID:";
		platformNameTxt.text = "Platform:";
		userIDTxt.text = "";
		addBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_add");
		CreateDropdown();
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void CreateDropdown()
	{
		for (int i = 1; i < 10; i++)
		{
			if (i != 0 && i != 3 && i != 6)
			{
				_objects.Add(new MyDropdownItemModel
				{
					id = i,
					title = AppInfo.GetPlatformName(i),
					data = i
				});
			}
		}
		_currentModel = _objects[0];
		platformDropdown.GetComponentInChildren<TextMeshProUGUI>().text = _currentModel.title;
	}

	public void OnPlatformDropdown()
	{
		GameData.instance.windowGenerator.NewDropdownWindow("Platform");
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		window = GameData.instance.windowGenerator.NewDropdownWindow("Platform");
		DropdownList componentInChildren = window.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, _currentModel.id, OnPlatformSelected);
		componentInChildren.ClearList();
		componentInChildren.Data.InsertItemsAtEnd(_objects);
	}

	private void OnPlatformSelected(MyDropdownItemModel model)
	{
		_currentModel = model;
		platformDropdown.GetComponentInChildren<TextMeshProUGUI>().text = _currentModel.title;
		if (window != null)
		{
			window.GetComponent<DropdownWindow>().OnClose();
		}
	}

	public void OnAddBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoPlatformAdd((int)_currentModel.data, userIDTxt.text);
	}

	private void DoPlatformAdd(int platform, string userID)
	{
		if (Util.removeExtraWhiteSpace(userID).Length > 0)
		{
			GameData.instance.main.ShowLoading();
			AdminDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(22), OnPlatformAdd);
			AdminDALC.instance.doCharacterPlatformAdd(_charID, platform, userID);
		}
	}

	private void OnPlatformAdd(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		AdminDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(22), OnPlatformAdd);
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
		AdminCharacterPlatformsWindow adminCharacterPlatformsWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(AdminCharacterPlatformsWindow)) as AdminCharacterPlatformsWindow;
		if (adminCharacterPlatformsWindow != null)
		{
			adminCharacterPlatformsWindow.SetPlatforms(platforms);
		}
		AdminCharacterWindow adminCharacterWindow = GameData.instance.windowGenerator.GetDialogByClass(typeof(AdminCharacterWindow)) as AdminCharacterWindow;
		if (adminCharacterWindow != null)
		{
			adminCharacterWindow.DoRefresh();
		}
		OnClose();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		addBtn.interactable = true;
		userIDTxt.interactable = true;
		platformDropdown.GetComponent<EventTrigger>().enabled = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		addBtn.interactable = false;
		userIDTxt.interactable = false;
		platformDropdown.GetComponent<EventTrigger>().enabled = false;
	}
}
