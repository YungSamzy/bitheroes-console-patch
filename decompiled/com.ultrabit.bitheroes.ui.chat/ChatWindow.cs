using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.chat;

public class ChatWindow : WindowsMain
{
	public const int TAB_GLOBAL = 0;

	public const int TAB_GUILD = 1;

	private static string[] TAB_NAMES = new string[2] { "ui_world", "ui_guild" };

	private static string[] TAB_COLORS = new string[2] { "#00FF00", "#F1D96D" };

	public TextMeshProUGUI topperTxt;

	public Button moderatorBtn;

	public GameObject bg;

	public List<ChatTab> _tabs = new List<ChatTab>();

	public List<ChatBox> _panels = new List<ChatBox>();

	private int _currentTab = -1;

	public UnityEvent CHANGE = new UnityEvent();

	public bool loaded;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails()
	{
		topperTxt.text = Language.GetString("ui_chat");
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(7), OnGuildMessage);
		ChatDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(1), OnGlobalMessage);
		GameDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(3), OnNotification);
		moderatorBtn.gameObject.SetActive(GameData.instance.PROJECT.character.moderator);
		moderatorBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_m");
		for (int i = 0; i < _panels.Count; i++)
		{
			_panels[i].Create(this);
			_panels[i].gameObject.SetActive(value: false);
		}
		SetTabs();
		ChatBox chatBox = GetPanel(0);
		SetTab();
		if (chatBox != null)
		{
			chatBox.shown = false;
		}
		ChatBox chatBox2 = GetPanel(1);
		if (chatBox2 != null)
		{
			chatBox2.shown = false;
		}
		SCROLL_IN_START.AddListener(delegate
		{
			DoUpdate();
		});
		ListenForBack(OnClose);
		CreateWindow(closeWord: false, "", scroll: false, stayUp: true);
	}

	public override void DoDestroy()
	{
		bg.gameObject.SetActive(value: false);
		ChatBox chatBox = GetPanel(0);
		if (chatBox != null)
		{
			chatBox.shown = false;
		}
		ChatBox chatBox2 = GetPanel(1);
		if (chatBox2 != null)
		{
			chatBox2.shown = false;
		}
		InvokeDestroyedWithoutDestroying();
	}

	public void OnModeratorBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewChatMuteLogWindow(0, base.gameObject, base.layer);
	}

	private void OnGuildMessage(BaseEvent e)
	{
		DALCEvent dALCEvent = e as DALCEvent;
		if (GameData.instance.PROJECT.character != null && GameData.instance.PROJECT.character.guildData == null)
		{
			SetTab();
			return;
		}
		ChatBox chatBox = GetPanel(1);
		if (!(chatBox == null))
		{
			chatBox.ParseMessage(dALCEvent.sfsob);
			if (GameData.instance.audioManager != null)
			{
				GameData.instance.audioManager.PlaySoundLink("messageguild");
			}
			UpdatePending();
		}
	}

	private void OnGlobalMessage(BaseEvent e)
	{
		DALCEvent dALCEvent = e as DALCEvent;
		if (GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null && GameData.instance.PROJECT.character.chatEnabled)
		{
			ChatBox chatBox = GetPanel(0);
			if (!(chatBox == null))
			{
				chatBox.ParseMessage(dALCEvent.sfsob);
				GameData.instance.audioManager.PlaySoundLink("message");
				UpdatePending();
			}
		}
	}

	private void OnNotification(BaseEvent e)
	{
		SFSObject sfsob = (e as DALCEvent).sfsob;
		ChatBox chatBox = GetPanel(0);
		if (!(chatBox == null))
		{
			sfsob.GetInt("cha1");
			string utfString = sfsob.GetUtfString("cha2");
			int @int = sfsob.GetInt("not0");
			string utfString2 = sfsob.GetUtfString("not1");
			if (@int == 0)
			{
				chatBox.ShowError(Language.GetString(utfString2, new string[1] { utfString }), VariableBook.chatTextColorGlobalMessage);
			}
		}
	}

	private void SetTabs()
	{
		SetTabButton(0);
		SetTabButton(1);
	}

	private void SetTabButton(int tab)
	{
		_tabs[tab].LoadDetails(tab);
	}

	private void OnTabButtonClicked(ChatTab tab)
	{
		if (tab.tabIndex == 1 && GameData.instance.PROJECT.character.guildData == null)
		{
			GameData.instance.windowGenerator.ShowErrorCode(53);
			return;
		}
		for (int i = 0; i < _tabs.Count; i++)
		{
			if (tab == _tabs[i])
			{
				SetTab(i);
				break;
			}
		}
	}

	private ChatTab GetTab(int id)
	{
		if (id < 0 || id >= _tabs.Count)
		{
			return null;
		}
		return _tabs[id];
	}

	private ChatBox GetPanel(int id)
	{
		if (id < 0 || id >= _panels.Count)
		{
			return null;
		}
		return _panels[id];
	}

	private void SetTab(int id = 0)
	{
		for (int i = 0; i < _tabs.Count; i++)
		{
			Util.SetTab(_tabs[i].tabBtn, i == id);
		}
		ChatBox chatBox = GetPanel(_currentTab);
		if (chatBox != null)
		{
			chatBox.gameObject.SetActive(value: false);
			chatBox.shown = false;
		}
		_currentTab = id;
		ChatBox chatBox2 = GetPanel(_currentTab);
		if (!(chatBox2 == null))
		{
			if (chatBox2 != null)
			{
				chatBox2.gameObject.SetActive(value: true);
				chatBox2.shown = true;
				chatBox2.UpdateText();
			}
			UpdatePending();
		}
	}

	public int GetPendingCount(int tab)
	{
		ChatBox chatBox = GetPanel(tab);
		if (chatBox != null)
		{
			return chatBox.pendingCount;
		}
		return 0;
	}

	public static string GetTabName(int tab)
	{
		if (tab < 0 || tab >= TAB_NAMES.Length)
		{
			return null;
		}
		return Language.GetString(TAB_NAMES[tab]);
	}

	public static string GetTabColor(int tab)
	{
		if (tab < 0 || tab >= TAB_COLORS.Length)
		{
			return "#ffffff";
		}
		return TAB_COLORS[tab];
	}

	public void DoUpdate()
	{
		ChatTab tab = GetTab(1);
		ChatBox chatBox = GetPanel(1);
		if (tab != null && chatBox != null)
		{
			chatBox.DoUpdate();
		}
		UpdatePending();
	}

	private void UpdatePending()
	{
		ChatTab tab = GetTab(0);
		ChatBox chatBox = GetPanel(0);
		ChatTab tab2 = GetTab(1);
		ChatBox chatBox2 = GetPanel(1);
		if (tab != null && chatBox != null)
		{
			tab.SetCount(chatBox.pendingCount);
		}
		if (tab2 != null && chatBox2 != null)
		{
			int count = ((GameData.instance.PROJECT.character.guildData != null) ? chatBox2.pendingCount : 0);
			tab2.SetCount(count);
		}
		CHANGE.Invoke();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		ChatBox chatBox = GetPanel(_currentTab);
		if (chatBox != null)
		{
			if (!AppInfo.IsMobile())
			{
				chatBox.inputTxt.Select();
			}
			SetTab(_currentTab);
		}
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
	}

	public override void OnClose()
	{
		GetPanel(_currentTab).inputTxt.enabled = false;
		SCROLL_IN_START.RemoveListener(delegate
		{
			DoUpdate();
		});
		base.OnClose();
	}
}
