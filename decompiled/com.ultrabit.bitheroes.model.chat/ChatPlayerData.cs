using System.Collections.Generic;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.chat;

public class ChatPlayerData
{
	private int _charID;

	private string _name;

	public int charID => _charID;

	public string name => _name;

	public ChatPlayerData(int charID, string name)
	{
		_charID = charID;
		_name = name;
	}

	public static ChatPlayerData fromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("cha1");
		string utfString = sfsob.GetUtfString("cha2");
		return new ChatPlayerData(@int, utfString);
	}

	public static List<ChatPlayerData> listFromSFSObject(ISFSObject sfsob)
	{
		List<ChatPlayerData> list = new List<ChatPlayerData>();
		ISFSArray sFSArray = sfsob.GetSFSArray("cha37");
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			ChatPlayerData item = fromSFSObject(sFSArray.GetSFSObject(i));
			list.Add(item);
		}
		return list;
	}
}
