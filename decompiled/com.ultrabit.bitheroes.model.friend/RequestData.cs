using System.Collections.Generic;
using com.ultrabit.bitheroes.model.character;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.friend;

public class RequestData
{
	private CharacterData _characterData;

	public CharacterData characterData => _characterData;

	public bool online => false;

	public RequestData(CharacterData characterData)
	{
		_characterData = characterData;
	}

	public static RequestData fromSFSObject(ISFSObject sfsob)
	{
		return new RequestData(CharacterData.fromSFSObject(sfsob));
	}

	public static List<RequestData> listFromSFSObject(ISFSObject sfsob)
	{
		List<RequestData> list = new List<RequestData>();
		ISFSArray sFSArray = sfsob.GetSFSArray("cha14");
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			RequestData item = fromSFSObject(sFSArray.GetSFSObject(i));
			list.Add(item);
		}
		return list;
	}
}
