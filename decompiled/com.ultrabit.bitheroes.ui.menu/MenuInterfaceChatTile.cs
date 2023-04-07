using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.chat;
using TMPro;

namespace com.ultrabit.bitheroes.ui.menu;

public class MenuInterfaceChatTile : MainUIButton
{
	public TextMeshProUGUI countTxt;

	private ChatWindow _chatWindow;

	private ChatAgreementWindow _agreement;

	public ChatWindow chatWindow => _chatWindow;

	public override void Create()
	{
		LoadDetails(Language.GetString("ui_chat"), VariableBook.GetGameRequirement(4));
		_chatWindow = GameData.instance.windowGenerator.NewChatWindow();
		_chatWindow.bg.SetActive(value: false);
		_chatWindow.CHANGE.AddListener(OnChatChange);
		UpdateCounts();
	}

	private void OnChatChange()
	{
		UpdateCounts();
	}

	private void UpdateCounts()
	{
		SetCounts(_chatWindow.GetPendingCount(0), _chatWindow.GetPendingCount(1));
	}

	public void SetCounts(int global = 0, int guild = 0)
	{
		if (global > VariableBook.worldChatMessageLimit)
		{
			global = VariableBook.worldChatMessageLimit;
		}
		if (guild > VariableBook.guildChatMessageLimit)
		{
			guild = VariableBook.guildChatMessageLimit;
		}
		int num = global + guild;
		string color = ((guild > 0) ? ChatWindow.GetTabColor(1) : ChatWindow.GetTabColor(0));
		string text = ((num > 0) ? Util.colorString(Util.NumberFormat(num), color) : (Util.asianLangManager.isAsian ? ". . ." : "· · ·"));
		countTxt.text = text;
	}

	public override void DoClick()
	{
		base.DoClick();
		Toggle();
	}

	public void Toggle()
	{
		CheckChat();
	}

	private void CheckChat()
	{
		if (!GameData.instance.SAVE_STATE.chatTosVerified)
		{
			_agreement = GameData.instance.windowGenerator.NewChatAgreementWindow(8);
			_agreement.SCROLL_OUT_START.AddListener(OnAgreementClose);
		}
		else
		{
			GameData.instance.windowGenerator.ShowChat(_chatWindow, 8);
			UpdateCounts();
		}
	}

	private void OnAgreementClose(object e)
	{
		if (GameData.instance.SAVE_STATE.chatTosVerified)
		{
			CheckChat();
		}
	}

	private new void OnDestroy()
	{
		if (_chatWindow != null)
		{
			_chatWindow.CHANGE.RemoveListener(OnChatChange);
		}
		if (_agreement != null)
		{
			_agreement.SCROLL_OUT_START.RemoveListener(OnAgreementClose);
		}
	}
}
