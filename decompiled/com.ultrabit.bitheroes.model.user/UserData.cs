using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.character;
using Sfs2X.Entities.Data;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.user;

public class UserData
{
	private CharacterData _characterData;

	private bool _online;

	public UnityEvent OnChange = new UnityEvent();

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
		set
		{
			if (value != _online)
			{
				_characterData.loginMilliseconds = 1000L;
			}
			_online = value;
			Broadcast();
		}
	}

	public CharacterData characterData
	{
		get
		{
			return _characterData;
		}
		set
		{
			_characterData = value;
			Broadcast();
		}
	}

	public bool offline => !online;

	public int level => _characterData.level;

	public int stats => _characterData.getTotalStats();

	public float loginMilliseconds => _characterData.loginMilliseconds;

	public UserData(CharacterData characterData, bool online)
	{
		_characterData = characterData;
		_online = online;
	}

	public static UserData fromSFSObject(ISFSObject sfsob)
	{
		CharacterData obj = CharacterData.fromSFSObject(sfsob);
		bool @bool = sfsob.GetBool("use5");
		return new UserData(obj, @bool);
	}

	public void Broadcast()
	{
		OnChange?.Invoke();
	}

	public string getLoginText()
	{
		return _characterData.getLoginText(online);
	}
}
