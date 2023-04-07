using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.extensions;
using com.ultrabit.bitheroes.model.adgor;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.brawl;
using com.ultrabit.bitheroes.model.chat;
using com.ultrabit.bitheroes.model.filter;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.parsing.model.utility;
using com.ultrabit.bitheroes.ui.utility;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.chat;

public class ChatBox : MonoBehaviour
{
	private const string BREAK_LINE = "<br></br>";

	public TMP_InputField contentTxt;

	public TMP_InputField inputTxt;

	public Button sendBtn;

	private TextMeshProUGUI _sendBtnTxt;

	private int _messageLines;

	private bool _showInitials;

	private int _defaultLines;

	private int _messageCount;

	private int _messageMax = 3;

	private Coroutine _messageTimer;

	private float _messageDelay = 2f;

	private string _previousMessage = "";

	private List<ChatText> _pending = new List<ChatText>();

	private List<string> _contents = new List<string>();

	private Color alpha = new Color(1f, 1f, 1f, 0.5f);

	private int m_startSelectIndex;

	private int m_endSelectIndex;

	private string m_seletected_text = "";

	public Button upBtn;

	public Button downBtn;

	public TextMeshProUGUI chatEmptyTxt;

	private string copiedText;

	private bool _shown;

	private Vector2 beginDragPosition = Vector2.zero;

	public int pendingCount => _pending.Count;

	public bool shown
	{
		get
		{
			return _shown;
		}
		set
		{
			_shown = value;
		}
	}

	public object Distance { get; private set; }

	public bool isEmpty
	{
		get
		{
			if (!(contentTxt.text == string.Empty))
			{
				return _contents.Count == 0;
			}
			return true;
		}
	}

	public virtual void LoadDetails(int messageLines = 100, int inputLength = 100, bool showInitials = true)
	{
		_sendBtnTxt = ((sendBtn != null) ? sendBtn.GetComponentInChildren<TextMeshProUGUI>() : null);
		_messageLines = messageLines;
		_showInitials = showInitials;
		_shown = true;
		_defaultLines = contentTxt.lineLimit;
		contentTxt.text = "";
		EventTrigger component = contentTxt.GetComponent<EventTrigger>();
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerClick;
		entry.callback.AddListener(delegate(BaseEventData data)
		{
			OnContentClick((PointerEventData)data);
		});
		component.triggers.Add(entry);
		contentTxt.onTextSelection.AddListener(OnTextSelection);
		contentTxt.verticalScrollbar.onValueChanged.AddListener(OnScrollBarChanged);
		if (AppInfo.IsMobile())
		{
			EventTrigger.Entry entry2 = new EventTrigger.Entry();
			entry2.eventID = EventTriggerType.Drag;
			EventTrigger.Entry entry3 = new EventTrigger.Entry();
			entry3.eventID = EventTriggerType.BeginDrag;
			entry3.callback.AddListener(delegate(BaseEventData data)
			{
				OnContentBeginDrag((PointerEventData)data);
			});
			entry2.callback.AddListener(delegate(BaseEventData data)
			{
				OnContentDrag((PointerEventData)data);
			});
			component.triggers.Add(entry2);
			component.triggers.Add(entry3);
		}
		if (chatEmptyTxt != null)
		{
			chatEmptyTxt.text = Language.GetString("ui_chat_empty");
		}
		ClearInput();
		inputTxt.characterLimit = inputLength;
		inputTxt.onFocusSelectAll = false;
		inputTxt.textComponent.enableWordWrapping = true;
		sendBtn.onClick.AddListener(OnSendBtn);
		upBtn.onClick.AddListener(OnScrollUp);
		downBtn.onClick.AddListener(OnScrollDown);
		inputTxt.onSubmit.AddListener(OnSubmitInputTxt);
		contentTxt.onSubmit.AddListener(OnSubmitContentTxt);
		_sendBtnTxt.text = Language.GetString("ui_send");
		OnInputChange();
		if (AppInfo.IsMobile())
		{
			contentTxt.scrollSensitivity = 0.05f;
		}
		UpdateText();
	}

	private void OnContentBeginDrag(PointerEventData eventData)
	{
		D.Log("OnContentBeginDrag " + eventData.position.ToString());
		beginDragPosition = eventData.position;
	}

	public void OnContentDrag(PointerEventData eventData)
	{
		float num = Vector2.Distance(beginDragPosition, eventData.position);
		D.Log($"OnContentDrag Distance {num} - {eventData.useDragThreshold}");
		num *= ((beginDragPosition.y > eventData.position.y) ? 1f : (-1f));
		eventData.scrollDelta = new Vector2(0f, num);
		contentTxt.OnScroll(eventData);
		beginDragPosition = eventData.position;
	}

	private void Update()
	{
		if (((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.C)) || Input.GetMouseButtonUp(1))
		{
			copiedText = m_seletected_text;
			copiedText = Regex.Replace(copiedText, "<[^>]*>", string.Empty);
			copiedText = copiedText.Replace("<noparse>", string.Empty);
			copiedText = copiedText.Replace("br>", string.Empty);
			copiedText = copiedText.Replace("</color>", string.Empty);
			StartCoroutine("EraseClipboard");
		}
		if ((((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.V)) || Input.GetMouseButtonUp(2)) && inputTxt.isFocused && GUIUtility.systemCopyBuffer == "")
		{
			inputTxt.text += copiedText;
		}
	}

	private IEnumerator EraseClipboard()
	{
		yield return new WaitForSeconds(0.1f);
		GUIUtility.systemCopyBuffer = "";
	}

	public void OnSubmitInputTxt(string args)
	{
		if (!AppInfo.IsMobile())
		{
			inputTxt.ActivateInputField();
		}
		if (!Input.GetKeyDown(KeyCode.RightControl) && !Input.GetKeyDown(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl) && !Input.GetKey(KeyCode.LeftControl) && !Input.GetKeyDown(KeyCode.RightCommand) && !Input.GetKeyDown(KeyCode.LeftCommand) && !Input.GetKey(KeyCode.RightCommand) && !Input.GetKey(KeyCode.LeftCommand))
		{
			if (Input.GetKey(KeyCode.Escape))
			{
				GameData.instance.main.DoBack();
			}
			else
			{
				OnSendBtn();
			}
		}
	}

	public void OnSubmitContentTxt(string args)
	{
		if (Input.GetKey(KeyCode.Escape))
		{
			GameData.instance.main.DoBack();
		}
	}

	public void OnScrollBarChanged(float arg)
	{
		contentTxt.ReleaseSelection();
	}

	public void OnScrollUp()
	{
		PointerEventData pointerEventData = new PointerEventData(GameData.instance.eventSystem);
		pointerEventData.scrollDelta = Vector2.up;
		contentTxt.OnScroll(pointerEventData);
	}

	public void OnScrollDown()
	{
		PointerEventData pointerEventData = new PointerEventData(GameData.instance.eventSystem);
		pointerEventData.scrollDelta = Vector2.down;
		contentTxt.OnScroll(pointerEventData);
	}

	public void OnContentClick(PointerEventData eventData)
	{
		int num = TMP_TextUtilities.FindIntersectingLink(contentTxt.textComponent, eventData.position, GameData.instance.main.uiCamera);
		if (num == -1 || m_endSelectIndex != m_startSelectIndex)
		{
			return;
		}
		TMP_LinkInfo tMP_LinkInfo = contentTxt.textComponent.textInfo.linkInfo[num];
		string[] array = tMP_LinkInfo.GetLinkID().Split(',');
		int num2 = int.Parse(array[0]);
		if (num2 != GameData.instance.PROJECT.character.id)
		{
			string text = ((array.Length <= 1) ? string.Empty : array[1]);
			GameData.instance.audioManager.PlaySoundLink("buttonclick");
			if (GameData.instance.PROJECT.character.moderator)
			{
				GameData.instance.windowGenerator.NewChatPlayerWindow(num2, text, contentTxt.text, null, GetLayer());
			}
			else
			{
				GameData.instance.windowGenerator.ShowPlayer(num2, GetLayer());
			}
		}
	}

	private void OnTextSelection(string text, int startSelectionIndex, int endSelectionIndex)
	{
		m_startSelectIndex = startSelectionIndex;
		m_endSelectIndex = endSelectionIndex;
		int num = startSelectionIndex - endSelectionIndex;
		if (num < 0)
		{
			num = endSelectionIndex - startSelectionIndex;
		}
		m_seletected_text = text.Substring(endSelectionIndex, num);
	}

	private void OnDestroy()
	{
		contentTxt.GetComponent<EventTrigger>();
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerClick;
		entry.callback.RemoveListener(delegate(BaseEventData data)
		{
			OnContentClick((PointerEventData)data);
		});
		contentTxt.onTextSelection.RemoveListener(OnTextSelection);
		sendBtn.onClick.RemoveListener(OnSendBtn);
		contentTxt.verticalScrollbar.onValueChanged.RemoveListener(OnScrollBarChanged);
		inputTxt.onSubmit.RemoveListener(OnSubmitInputTxt);
		contentTxt.onSubmit.RemoveListener(OnSubmitContentTxt);
		StopMessageTimer();
	}

	public void OnSendBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		string text = inputTxt.text.Replace("\t", " ");
		SendAMessage(text);
	}

	public void OnInputChange()
	{
		bool flag = string.IsNullOrEmpty(inputTxt.text);
		sendBtn.image.color = (flag ? alpha : Color.white);
		_sendBtnTxt.color = (flag ? alpha : Color.white);
		sendBtn.enabled = !flag;
	}

	public void AddText(ChatText chatText, bool update = true)
	{
		_pending.Add(chatText);
		while (_pending.Count > _messageLines)
		{
			_pending.RemoveAt(0);
		}
		if (shown && update)
		{
			UpdateText();
		}
	}

	public virtual void UpdateText()
	{
		float value = contentTxt.verticalScrollbar.value;
		float size = contentTxt.verticalScrollbar.size;
		if (_pending.Count >= _messageLines)
		{
			_pending.RemoveRange(0, _pending.Count - _messageLines);
			_contents.Clear();
		}
		else if (_pending.Count + _contents.Count >= _messageLines)
		{
			_contents.RemoveRange(0, _pending.Count + _contents.Count - _messageLines);
		}
		foreach (ChatText item in _pending)
		{
			_contents.Add(item.html + item.text);
		}
		_pending.Clear();
		StringBuilder stringBuilder = new StringBuilder();
		foreach (string content in _contents)
		{
			stringBuilder.Append(content);
		}
		contentTxt.text = stringBuilder.ToString();
		PointerEventData pointerEventData = new PointerEventData(GameData.instance.eventSystem);
		pointerEventData.scrollDelta = new Vector2(0f, ((value == 0f && size == 1f) || value == 1f) ? (-100000) : 0);
		contentTxt.OnScroll(pointerEventData);
		if (chatEmptyTxt != null)
		{
			chatEmptyTxt.gameObject.SetActive(isEmpty);
		}
	}

	public void ClearChat()
	{
		_pending.Clear();
		_contents.Clear();
		contentTxt.text = string.Empty;
	}

	public void AddBlankLines(int count = 5)
	{
		for (int i = 0; i < count; i++)
		{
			AddText(new ChatText(null, string.Empty));
		}
	}

	public void ShowError(string text, string color = null)
	{
		if (color == null)
		{
			color = VariableBook.chatTextColorError;
		}
		AddText(new ChatText("<color=#" + color + ">" + text + "</color><br>"));
	}

	private void ClearInput()
	{
		inputTxt.text = string.Empty;
	}

	private void SendAMessage(string text)
	{
		ClearInput();
		if (Util.removeExtraWhiteSpace(text).Length <= 0)
		{
			ShowError(Language.GetString("chat_error_blank_message"));
		}
		else if (!GameData.instance.PROJECT.character.chatEnabled && this is ChatGlobalPanel)
		{
			ShowError(Language.GetString("chat_error_disabled"));
		}
		else
		{
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			if (text.Substring(0, 1) == "/")
			{
				DoSlashCommand((text.Length > 1) ? text.Substring(1).Split(' ') : new string[1] { string.Empty }, text);
				return;
			}
			if (_messageCount >= _messageMax)
			{
				ShowError(Language.GetString("chat_error_spam"));
				return;
			}
			if (_messageTimer == null)
			{
				_messageTimer = GameData.instance.main.coroutineTimer.AddTimer(base.gameObject, _messageDelay, CoroutineTimer.TYPE.SECONDS, 0, null, OnMessageTimer);
			}
			_messageCount++;
			_previousMessage = text;
			inputTxt.Rebuild(CanvasUpdate.LatePreRender);
			DoMessage(text);
		}
	}

	public void ParseMessage(SFSObject sfsob)
	{
		if (sfsob.ContainsKey("err0"))
		{
			int @int = sfsob.GetInt("err0");
			string text = ErrorCode.getErrorMessage(@int);
			if (@int == 37)
			{
				long @long = sfsob.GetLong("cha38");
				string chatMuteReason = VariableBook.GetChatMuteReason(sfsob.GetInt("cha39"));
				List<string> list = new List<string>();
				list.Add(Util.TimeFormatClean(@long));
				list.Add(chatMuteReason);
				text = Util.ParseStringValues(text, list, color: false);
			}
			ShowError(text);
		}
		else
		{
			ChatData chatData = ChatData.fromSFSObject(sfsob);
			ParseData(chatData);
		}
	}

	public void ParseData(ChatData chatData, bool update = true)
	{
		if (chatData != null && (chatData.admin || chatData.moderator || GameData.instance.PROJECT.character.getChatIgnore(chatData.charID) == null))
		{
			string text = chatData.message;
			if (!GameData.instance.SAVE_STATE.filterDisabled)
			{
				text = Filter.filter(text);
			}
			string text2 = " " + GetColoredText("(" + chatData.level + ")", "999999");
			if (GameData.instance.PROJECT.character.chatEnabled && this is ChatGlobalPanel && GameData.instance.SAVE_STATE.hideLevelChat)
			{
				text2 = string.Empty;
			}
			string text3 = $"<link=\"{chatData.charID},{chatData.name}\">{GetNameColor(chatData.charID, chatData.name, chatData.guildInitials, chatData.admin, chatData.moderator, chatData.hasVipgor)}{text2}</link>";
			text = "<noparse>" + text + "</noparse><br>";
			AddText(new ChatText(text3 + ": ", text), update);
		}
	}

	public virtual void DoMessage(string text)
	{
	}

	private void StopMessageTimer()
	{
		GameData.instance.main.coroutineTimer.StopTimer(ref _messageTimer);
	}

	private void OnMessageTimer()
	{
		if (_messageCount > 0)
		{
			_messageCount--;
		}
		else
		{
			StopMessageTimer();
		}
	}

	private void DoSlashCommand(string[] commands, string text)
	{
		string text2 = null;
		string text3 = null;
		foreach (string text4 in commands)
		{
			if (text4 != " " && text4.Length > 0)
			{
				if (text2 == null)
				{
					text2 = text4;
				}
				else if (text3 == null)
				{
					text3 = text4;
				}
				else if (text == null)
				{
					text = text4;
				}
			}
		}
		if (text2 == null)
		{
			ShowError(Language.GetString("chat_error_command"));
			return;
		}
		switch (ChatCommands.getCommand(text2.ToLowerInvariant()))
		{
		case 0:
		{
			string text5 = Language.GetString("chat_commands");
			for (int j = 0; j < ChatCommands.commands.Length; j++)
			{
				string[] array = ChatCommands.commands[j];
				text5 = text5 + "<br> - " + array[0] + " - (" + Language.GetString(ChatCommands.desc[j]) + ")";
			}
			ShowError(text5);
			break;
		}
		case 1:
			ClearChat();
			break;
		case 2:
			if ((GameData.instance.PROJECT.dungeon != null && GameData.instance.PROJECT.dungeon.GetEventRef() != null && (GameData.instance.PROJECT.dungeon.GetEventRef().eventType == 7 || GameData.instance.PROJECT.dungeon.GetEventRef().eventType == 5)) || GameData.instance.PROJECT.battle != null)
			{
				ShowError(Language.GetString("chat_command_not_able_in_instance"));
			}
			else
			{
				GameData.instance.windowGenerator.ShowPlayer(0, GetLayer(), text3);
			}
			break;
		case 3:
			ShowError(Util.dateFormat(ServerExtension.instance.GetDate()));
			break;
		default:
			ShowError(Language.GetString("chat_error_command"));
			break;
		}
	}

	private int GetLayer()
	{
		WindowsMain componentInParent = GetComponentInParent<WindowsMain>();
		if (componentInParent != null)
		{
			return componentInParent.layer;
		}
		return -1;
	}

	public string GetNameColor(int charID, string playerName, string guildInitials, bool admin = false, bool moderator = false, bool hasVipgor = false)
	{
		if (!_showInitials)
		{
			guildInitials = null;
		}
		if (charID == GameData.instance.PROJECT.character.id)
		{
			if (admin)
			{
				return GetColoredText(Util.ParseName(playerName, guildInitials, color: false), VariableBook.chatTextColorAdmin);
			}
			if (hasVipgor)
			{
				return GetColoredText(Util.ParseName(playerName, guildInitials, color: false), AdGor.VIPGOR_NAMEPLATE_COLOR);
			}
			return GetColoredText(Util.ParseName(playerName, guildInitials, color: false), VariableBook.chatTextColorSelf);
		}
		if (admin)
		{
			return GetColoredText(Util.ParseName(playerName, guildInitials, color: false), VariableBook.chatTextColorAdmin);
		}
		if (moderator)
		{
			return GetColoredText(Util.ParseName(playerName, guildInitials, color: false), VariableBook.chatTextColorModerator);
		}
		if (hasVipgor)
		{
			return GetColoredText(Util.ParseName(playerName, guildInitials, color: false), AdGor.VIPGOR_NAMEPLATE_COLOR);
		}
		return GetColoredText(Util.ParseName(playerName, guildInitials, color: false), VariableBook.chatTextColorOthers);
	}

	public string GetColoredText(string text, string color)
	{
		return "<color=#" + color + ">" + text + "</color>";
	}

	public string GetContents()
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (string content in _contents)
		{
			stringBuilder.Append(content);
		}
		return stringBuilder.ToString();
	}

	public virtual void DoUpdate()
	{
	}

	public virtual void DoEnable()
	{
		sendBtn.interactable = true;
		inputTxt.interactable = true;
		contentTxt.interactable = true;
	}

	public virtual void DoDisable()
	{
		sendBtn.interactable = true;
		inputTxt.interactable = true;
		contentTxt.interactable = true;
	}

	public virtual void Create()
	{
	}

	public virtual void Create(Conversation conversation)
	{
	}

	public virtual void Create(BrawlRoom room)
	{
	}

	public virtual void Create(ChatWindow chatWindow)
	{
	}

	private void OnEnable()
	{
		inputTxt.enabled = true;
		if (!AppInfo.IsMobile())
		{
			inputTxt.ActivateInputField();
		}
	}
}
