using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.chat;

public class ChatPlayerWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public Button messageBtn;

	public Button viewBtn;

	public Button muteBtn;

	public Button unmuteBtn;

	public Button mutesBtn;

	public Image timeDropdown;

	public Image reasonDropdown;

	private int _charID;

	private string _name;

	private string _log;

	private int _mutes;

	private bool _muted;

	private int _selectedSeconds;

	private int _selectedReason;

	private Transform _timeWindow;

	private Transform _reasonsWindow;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(int charID, string name, string log = "")
	{
		_charID = charID;
		_name = name;
		_log = log;
		topperTxt.text = name;
		muteBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_mute");
		unmuteBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_unmute");
		messageBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_message");
		viewBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_view");
		foreach (int chatMuteSecond in VariableBook.chatMuteSeconds)
		{
			if (GameData.instance.PROJECT.character.admin || chatMuteSecond <= VariableBook.chatMuteSecondsModeratorLimit)
			{
				_selectedSeconds = chatMuteSecond;
				break;
			}
		}
		timeDropdown.GetComponentInChildren<TextMeshProUGUI>().text = Util.TimeFormatShort(_selectedSeconds);
		_selectedReason = 0;
		string text = VariableBook.chatMuteReasons[_selectedReason];
		reasonDropdown.GetComponentInChildren<TextMeshProUGUI>().text = text;
		DoPlayerInfo();
		DoUpdate();
		ListenForBack(OnClose);
		CreateWindow();
	}

	public void OnViewBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowPlayer(_charID);
	}

	public void OnMessageBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.ShowConversation(_charID, _name);
	}

	public void OnMuteBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoMute();
	}

	public void UnmuteBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoUnmute();
	}

	public void MutesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewChatMuteLogWindow(_charID, base.gameObject, base.layer);
	}

	public void OnTimerDropdown()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_timeWindow = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_duration"), null, base.layer);
		DropdownList componentInChildren = _timeWindow.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, _selectedSeconds, OnSecondsDropdownChange);
		for (int i = 0; i < VariableBook.chatMuteSeconds.Count; i++)
		{
			if (GameData.instance.PROJECT.character.admin || VariableBook.chatMuteSeconds[i] <= VariableBook.chatMuteSecondsModeratorLimit)
			{
				componentInChildren.Data.InsertOneAtEnd(new MyDropdownItemModel
				{
					id = i,
					title = Util.TimeFormatShort(VariableBook.chatMuteSeconds[i]),
					btnHelp = false,
					data = VariableBook.chatMuteSeconds[i]
				});
			}
		}
	}

	private void OnSecondsDropdownChange(MyDropdownItemModel model)
	{
		_selectedSeconds = (model.data as int?).Value;
		timeDropdown.GetComponentInChildren<TextMeshProUGUI>().text = model.title;
		if (_timeWindow != null)
		{
			_timeWindow.GetComponent<DropdownWindow>().OnClose();
		}
	}

	public void OnReasonDropdown()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		_reasonsWindow = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_reason"), null, base.layer);
		DropdownList componentInChildren = _reasonsWindow.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, _selectedReason, OnReasonsDropdownChange);
		for (int i = 0; i < VariableBook.chatMuteReasons.Count; i++)
		{
			componentInChildren.Data.InsertOneAtEnd(new MyDropdownItemModel
			{
				id = i,
				title = VariableBook.chatMuteReasons[i],
				btnHelp = false,
				data = i
			});
		}
	}

	private void OnReasonsDropdownChange(MyDropdownItemModel model)
	{
		_selectedReason = (model.data as int?).Value;
		reasonDropdown.GetComponentInChildren<TextMeshProUGUI>().text = model.title;
		if (_reasonsWindow != null)
		{
			_reasonsWindow.GetComponent<DropdownWindow>().OnClose();
		}
	}

	public void DoUpdate()
	{
		if ((bool)mutesBtn)
		{
			mutesBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("chat_previous_mutes", new string[1] { Util.NumberFormat(_mutes) });
		}
		if (_muted)
		{
			Util.SetButton(muteBtn, enabled: false);
			Util.SetButton(unmuteBtn);
		}
		else
		{
			Util.SetButton(muteBtn);
			Util.SetButton(unmuteBtn, enabled: false);
		}
	}

	private void DoMute()
	{
		int selectedSeconds = _selectedSeconds;
		int selectedReason = _selectedReason;
		Disable();
		GameData.instance.main.ShowLoading();
		ChatDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(5), OnMute);
		ChatDALC.instance.doMute(_charID, selectedSeconds, selectedReason, _log);
	}

	private void OnMute(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		Enable();
		GameData.instance.main.HideLoading();
		ChatDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(5), OnMute);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		int selectedSeconds = _selectedSeconds;
		DoUpdate();
		DoPlayerInfo();
		GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("ui_success"), Language.GetString("chat_mute_success", new string[2]
		{
			_name,
			Util.TimeFormatShort(selectedSeconds)
		}), null, null, null, base.layer);
	}

	private void DoUnmute()
	{
		Disable();
		GameData.instance.main.ShowLoading();
		ChatDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(6), OnUnmute);
		ChatDALC.instance.doUnmute(_charID);
	}

	private void OnUnmute(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		Enable();
		GameData.instance.main.HideLoading();
		ChatDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(6), OnUnmute);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		DoUpdate();
		DoPlayerInfo();
		GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("ui_success"), Language.GetString("chat_unmute_success", new string[1] { _name }), null, null, null, base.layer);
	}

	private void DoPlayerInfo()
	{
		ChatDALC.instance.doPlayerInfo(_charID);
		ChatDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(9), OnPlayerInfo);
	}

	private void OnPlayerInfo(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		ChatDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(9), OnPlayerInfo);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		_mutes = sfsob.GetInt("cha49");
		_muted = sfsob.GetBool("cha73");
		DoUpdate();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		messageBtn.interactable = true;
		viewBtn.interactable = true;
		muteBtn.interactable = true;
		unmuteBtn.interactable = true;
		mutesBtn.interactable = true;
		timeDropdown.GetComponent<EventTrigger>().enabled = true;
		reasonDropdown.GetComponent<EventTrigger>().enabled = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		messageBtn.interactable = false;
		viewBtn.interactable = false;
		muteBtn.interactable = false;
		unmuteBtn.interactable = false;
		mutesBtn.interactable = false;
		timeDropdown.GetComponent<EventTrigger>().enabled = false;
		reasonDropdown.GetComponent<EventTrigger>().enabled = false;
	}
}
