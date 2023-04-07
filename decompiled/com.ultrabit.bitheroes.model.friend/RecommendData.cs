using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.character;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.friend;

public class RecommendData
{
	private CharacterData _characterData;

	private bool _online;

	public CharacterData characterData => _characterData;

	public bool online
	{
		get
		{
			if (_characterData.charID != GameData.instance.PROJECT.character.id)
			{
				return _online;
			}
			return true;
		}
	}

	public bool offline => !online;

	public RecommendData(CharacterData characterData, bool online)
	{
		_characterData = characterData;
		_online = online;
	}

	public static RecommendData fromSFSObject(ISFSObject sfsob)
	{
		CharacterData obj = CharacterData.fromSFSObject(sfsob);
		bool @bool = sfsob.GetBool("use5");
		return new RecommendData(obj, @bool);
	}

	public long loginMilliseconds()
	{
		return _characterData.loginMilliseconds;
	}
}
