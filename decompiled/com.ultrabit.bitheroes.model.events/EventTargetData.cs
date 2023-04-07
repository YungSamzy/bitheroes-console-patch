using System.Collections.Generic;
using com.ultrabit.bitheroes.model.character;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.events;

public class EventTargetData
{
	private CharacterData _characterData;

	private int _pointsWin;

	private int _pointsLose;

	public CharacterData characterData => _characterData;

	public int stats => _characterData.getTotalStats();

	public int pointsWin => _pointsWin;

	public int pointsLose => _pointsLose;

	public EventTargetData(CharacterData characterData, int pointsWin, int pointsLose)
	{
		_characterData = characterData;
		_pointsWin = pointsWin;
		_pointsLose = pointsLose;
	}

	public static EventTargetData fromSFSObject(ISFSObject sfsob)
	{
		CharacterData obj = CharacterData.fromSFSObject(sfsob);
		int @int = sfsob.GetInt("eve5");
		int int2 = sfsob.GetInt("eve6");
		return new EventTargetData(obj, @int, int2);
	}

	public static List<EventTargetData> listFromSFSObject(ISFSObject sfsob)
	{
		ISFSArray sFSArray = sfsob.GetSFSArray("eve7");
		List<EventTargetData> list = new List<EventTargetData>();
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			ISFSObject sFSObject = sFSArray.GetSFSObject(i);
			list.Add(fromSFSObject(sFSObject));
		}
		return list;
	}
}
