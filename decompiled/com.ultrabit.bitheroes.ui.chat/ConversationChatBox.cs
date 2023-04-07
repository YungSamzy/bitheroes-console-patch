using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.model.chat;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using UnityEngine;
using UnityEngine.EventSystems;

namespace com.ultrabit.bitheroes.ui.chat;

public class ConversationChatBox : ChatBox
{
	private Conversation _conversation;

	private Vector2 beginDragPosition = Vector2.zero;

	public override void Create(Conversation conversation)
	{
		base.LoadDetails(VariableBook.worldChatMessageLimit, VariableBook.worldChatInputLength);
		_conversation = conversation;
		LoadMessages();
	}

	private void LoadMessages()
	{
		foreach (ChatData message in _conversation.messages)
		{
			ParseData(message, update: false);
		}
		UpdateText();
	}

	private void OnContentBeginDrag(PointerEventData eventData)
	{
		D.Log("OnContentBeginDrag " + eventData.position.ToString());
		beginDragPosition = eventData.position;
	}

	public new void OnContentDrag(PointerEventData eventData)
	{
		float num = Vector2.Distance(beginDragPosition, eventData.position);
		D.Log($"OnContentDrag Distance {num} - {eventData.useDragThreshold}");
		num *= ((beginDragPosition.y > eventData.position.y) ? 1f : (-1f));
		eventData.scrollDelta = new Vector2(0f, num);
		contentTxt.OnScroll(eventData);
		beginDragPosition = eventData.position;
	}

	public override void DoMessage(string text)
	{
		base.DoMessage(text);
		ChatDALC.instance.doPrivateMessage(text, _conversation.charID);
	}
}
