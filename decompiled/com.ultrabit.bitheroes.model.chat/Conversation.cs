using System.Collections.Generic;
using com.ultrabit.bitheroes.messenger;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.variable;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.chat;

public class Conversation : Messenger
{
	private Vector2 _point;

	private int _charID;

	private string _name;

	private List<ChatData> _messages = new List<ChatData>();

	public Vector2 point => _point;

	public int charID => _charID;

	public string name => _name;

	public List<ChatData> messages => _messages;

	public Conversation(Vector2 point, int charID, string name)
	{
		_point = point;
		_charID = charID;
		_name = name;
	}

	public void addMessage(ChatData chatData)
	{
		if (_messages != null)
		{
			_messages.Add(chatData);
			while (_messages.Count > VariableBook.worldChatMessageLimit)
			{
				_messages.RemoveAt(0);
			}
			Broadcast(CustomSFSXEvent.CHANGE);
		}
	}

	public ChatData getLastMessage()
	{
		if (_messages == null || _messages.Count <= 0)
		{
			return null;
		}
		return _messages[_messages.Count - 1];
	}
}
