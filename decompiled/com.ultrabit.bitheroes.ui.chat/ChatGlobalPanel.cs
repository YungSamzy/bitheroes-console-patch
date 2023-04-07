using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.model.chat;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.chat;

public class ChatGlobalPanel : ChatBox
{
	public Image channelDropdown;

	private Transform dropdownWindow;

	private ChatWindow _chatWindow;

	public override void Create(ChatWindow chatWindow)
	{
		_chatWindow = chatWindow;
		base.LoadDetails(VariableBook.worldChatMessageLimit, VariableBook.worldChatInputLength);
		if (VariableBook.worldChatChannels.Count > 1)
		{
			channelDropdown.gameObject.SetActive(value: true);
			{
				foreach (ChatChannel worldChatChannel in VariableBook.worldChatChannels)
				{
					if (worldChatChannel.id == GameData.instance.PROJECT.character.chatChannel)
					{
						channelDropdown.GetComponentInChildren<TextMeshProUGUI>().text = worldChatChannel.name;
					}
				}
				return;
			}
		}
		channelDropdown.gameObject.SetActive(value: false);
	}

	public void OnChannelDropdown()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		dropdownWindow = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_channel"), null, GetComponentInParent<WindowsMain>().layer);
		DropdownList componentInChildren = dropdownWindow.GetComponentInChildren<DropdownList>();
		componentInChildren.StartList(base.gameObject, GameData.instance.PROJECT.character.chatChannel, OnChannelDropdownChange);
		foreach (ChatChannel worldChatChannel in VariableBook.worldChatChannels)
		{
			componentInChildren.Data.InsertOneAtEnd(new MyDropdownItemModel
			{
				id = worldChatChannel.id,
				title = worldChatChannel.name,
				btnHelp = false
			});
		}
	}

	private void OnChannelDropdownChange(MyDropdownItemModel model)
	{
		ShowError(Language.GetString("ui_channel_change", new string[1] { model.title }));
		GameData.instance.PROJECT.character.chatChannel = model.id;
		channelDropdown.GetComponentInChildren<TextMeshProUGUI>().text = model.title;
		CharacterDALC.instance.doSaveConfig(GameData.instance.PROJECT.character);
		if (dropdownWindow != null)
		{
			dropdownWindow.GetComponent<DropdownWindow>().OnClose();
		}
	}

	public override void DoMessage(string text)
	{
		base.DoMessage(text);
		if (!GameData.instance.PROJECT.character.chatEnabled)
		{
			ShowError(Language.GetString("chat_error_disabled"));
		}
		else
		{
			ChatDALC.instance.doChatMessage(text);
		}
	}

	public override void DoEnable()
	{
		base.DoEnable();
		channelDropdown.GetComponent<EventTrigger>().enabled = true;
	}

	public override void DoDisable()
	{
		base.DoDisable();
		channelDropdown.GetComponent<EventTrigger>().enabled = false;
	}
}
