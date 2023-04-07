using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.chat;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.lists.chatmuteloglist;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.chat;

public class ChatMuteLogWindow : WindowsMain
{
	private const int DEFAULT_LIMIT = 50;

	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI dateTxt;

	public TextMeshProUGUI playerTxt;

	public TextMeshProUGUI moderatorTxt;

	public TextMeshProUGUI durationTxt;

	public TextMeshProUGUI reasonTxt;

	public Button refreshBtn;

	public TMP_InputField limitTxt;

	public ChatMuteLogList chatMuteLogList;

	public GameObject loadingIcon;

	private int _charID;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(int charID = 0)
	{
		_charID = charID;
		topperTxt.text = Language.GetString("ui_mutes");
		dateTxt.text = Language.GetString("ui_date");
		playerTxt.text = Language.GetString("ui_player");
		moderatorTxt.text = Language.GetString("ui_moderator");
		durationTxt.text = Language.GetString("ui_duration");
		reasonTxt.text = Language.GetString("ui_reason");
		limitTxt.characterLimit = 3;
		limitTxt.contentType = TMP_InputField.ContentType.IntegerNumber;
		limitTxt.text = 50.ToString();
		refreshBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_refresh");
		Debug.LogWarning("Check InputText Submit on mobile");
		limitTxt.onSubmit.AddListener(delegate
		{
			DoMuteLogList();
		});
		chatMuteLogList.InitList(OnTileSelect);
		DoMuteLogList();
		ListenForBack(OnClose);
		CreateWindow();
	}

	public void OnRefreshBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoMuteLogList();
	}

	private void DoMuteLogList()
	{
		chatMuteLogList.ClearList();
		Util.SetButton(refreshBtn, enabled: false);
		loadingIcon.SetActive(value: true);
		ChatDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(10), OnMuteLogList);
		ChatDALC.instance.doMuteLogList(_charID, GetLimit());
	}

	private void OnMuteLogList(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		if (refreshBtn != null && refreshBtn.gameObject != null)
		{
			Util.SetButton(refreshBtn);
		}
		if (loadingIcon != null)
		{
			loadingIcon.SetActive(value: false);
		}
		ChatDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(10), OnMuteLogList);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		List<ChatMuteLogData> mutes = ChatMuteLogData.listFromSFSObject(sfsob);
		CreateTiles(mutes);
	}

	private void CreateTiles(List<ChatMuteLogData> mutes)
	{
		chatMuteLogList.ClearList();
		for (int i = 0; i < mutes.Count; i++)
		{
			chatMuteLogList.Data.InsertOneAtEnd(new ChatMuteLogItem
			{
				muteData = mutes[i]
			});
		}
	}

	private void OnTileSelect(object e)
	{
		ChatMuteLogItem chatMuteLogItem = e as ChatMuteLogItem;
		DoMuteLogText(chatMuteLogItem.muteData.id);
	}

	private void DoMuteLogText(int id)
	{
		Disable();
		GameData.instance.main.ShowLoading();
		ChatDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(11), OnMuteLogText);
		ChatDALC.instance.doMuteLogText(id);
	}

	private void OnMuteLogText(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		Enable();
		GameData.instance.main.HideLoading();
		ChatDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(11), OnMuteLogText);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		string text = null;
		text = ((!AppInfo.IsWeb()) ? Util.UncompressString(sfsob.GetByteArray("chat6")) : Util.Base64Decode(sfsob.GetUtfString("chat6")));
		GameData.instance.windowGenerator.NewTextWindow(Language.GetString("chat_log"), Util.ConvertFlashHTML(text), base.gameObject, base.layer);
	}

	private int GetLimit()
	{
		int.TryParse(limitTxt.text, out var result);
		if (result < 0)
		{
			result = 0;
		}
		if (result > int.MaxValue)
		{
			result = int.MaxValue;
		}
		return result;
	}

	public override void DoDestroy()
	{
		limitTxt.onSubmit.RemoveListener(delegate
		{
			DoMuteLogList();
		});
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		refreshBtn.interactable = true;
		limitTxt.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		refreshBtn.interactable = false;
		limitTxt.interactable = false;
	}
}
