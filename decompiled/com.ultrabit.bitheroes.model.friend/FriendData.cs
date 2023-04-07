using System.Collections.Generic;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.user;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.friend;

public class FriendData : UserData
{
	public FriendData(CharacterData characterData, bool online)
		: base(characterData, online)
	{
	}

	public new static FriendData fromSFSObject(ISFSObject sfsob)
	{
		CharacterData obj = CharacterData.fromSFSObject(sfsob);
		bool @bool = sfsob.GetBool("use5");
		return new FriendData(obj, @bool);
	}

	public static List<FriendData> listFromSFSObject(ISFSObject sfsob)
	{
		List<FriendData> list = new List<FriendData>();
		ISFSArray sFSArray = sfsob.GetSFSArray("cha13");
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			FriendData item = fromSFSObject(sFSArray.GetSFSObject(i));
			list.Add(item);
		}
		return list;
	}
}
