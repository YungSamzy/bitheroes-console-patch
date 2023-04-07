using com.ultrabit.bitheroes.model.chat;
using com.ultrabit.bitheroes.model.events;
using TMPro;

namespace com.ultrabit.bitheroes.ui.chat;

public class ConversationWindow : WindowsMain
{
	public TextMeshProUGUI nameTxt;

	public ConversationWindowBg bg;

	public ConversationChatBox _chatBox;

	private Conversation _conversation;

	public ConversationWindowEvent OnWindowClose = new ConversationWindowEvent();

	public Conversation conversation => _conversation;

	public override void Start()
	{
		base.Start();
	}

	public void LoadDetails(Conversation conversation)
	{
		Disable();
		_conversation = conversation;
		_conversation.AddListener(CustomSFSXEvent.CHANGE, OnChange);
		nameTxt.text = conversation.name;
		bg.LoadDetails(this);
		_chatBox.Create(conversation);
		ListenForBack(DoDestroy);
		CreateWindow(closeWord: false, "", scroll: false);
		Enable();
	}

	private void UpdateConversation()
	{
		_chatBox.UpdateText();
	}

	private void OnChange()
	{
		ChatData lastMessage = _conversation.getLastMessage();
		if (lastMessage != null)
		{
			_chatBox.ParseData(lastMessage);
		}
	}

	public override void OnClose()
	{
		base.OnClose();
		if (OnWindowClose != null)
		{
			OnWindowClose.Invoke(this);
		}
		DoDestroy();
	}

	public override void DoDestroy()
	{
		bg.BeforeDestroy();
		_conversation.RemoveListener(CustomSFSXEvent.CHANGE, OnChange);
		base.DoDestroy();
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
}
