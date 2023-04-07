using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.chat;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.parsing.model.utility;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.chat;

public class ChatGuildPanel : ChatBox
{
	public TextMeshProUGUI initialsTxt;

	public Image loadingIcon;

	private ChatWindow _chatWindow;

	public override void Create(ChatWindow chatWindow)
	{
		_chatWindow = chatWindow;
		base.LoadDetails(VariableBook.guildChatMessageLimit, VariableBook.guildChatInputLength, showInitials: false);
		DoUpdate();
	}

	public override void DoUpdate()
	{
		base.DoUpdate();
		if (GameData.instance.PROJECT.character.guildData == null)
		{
			initialsTxt.gameObject.SetActive(value: false);
			ClearChat();
			ClearLoading();
			return;
		}
		initialsTxt.gameObject.SetActive(value: true);
		initialsTxt.text = Util.ParseGuildInitials(GameData.instance.PROJECT.character.guildData.initials);
		if (GameData.instance.PROJECT.character.guildData.messages == null)
		{
			DoChatLoad();
		}
	}

	private void AddLoading()
	{
		if (loadingIcon != null && loadingIcon.gameObject != null)
		{
			loadingIcon.gameObject.SetActive(value: true);
		}
	}

	private void ClearLoading()
	{
		if (loadingIcon != null && loadingIcon.gameObject != null)
		{
			loadingIcon.gameObject.SetActive(value: false);
		}
	}

	public override void DoMessage(string text)
	{
		if (GameData.instance.PROJECT.character.guildData == null)
		{
			ShowError(ErrorCode.getErrorMessage(53));
			return;
		}
		base.DoMessage(text);
		GuildDALC.instance.doChatMessage(text);
	}

	private void DoChatLoad()
	{
		ClearChat();
		AddLoading();
		DoDisable();
		GuildDALC.instance.doChatLoad();
		GuildDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(6), onChatLoad);
	}

	private void onChatLoad(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		DoEnable();
		ClearChat();
		GuildDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(6), onChatLoad);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			ClearLoading();
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		List<ChatData> messages = ChatData.listFromSFSObject(sfsob);
		if (GameData.instance.PROJECT.character.guildData != null)
		{
			GameData.instance.PROJECT.character.guildData.setMessages(messages);
		}
		LoadMessages();
	}

	private void LoadMessages()
	{
		ClearLoading();
		if (GameData.instance.PROJECT.character.guildData == null || GameData.instance.PROJECT.character.guildData.messages == null)
		{
			return;
		}
		foreach (ChatData message in GameData.instance.PROJECT.character.guildData.messages)
		{
			ParseData(message, update: false);
		}
		UpdateText();
	}
}
