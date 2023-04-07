using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.utility;
using Sfs2X.Entities.Data;
using SimpleJSON;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.character;

public class CharacterExtraInfo
{
	public static int CURRENCY_SEAL = 1;

	public static int NAMEPLATE = 2;

	public static int DAILY_REWARDS_STEP = 3;

	public static int BOOSTERS_INFO = 5;

	public static int TESTING_GROUP = 6;

	private long _sealsMilliseconds;

	private long _sealsCooldown;

	private float _sealsStartTime;

	private Coroutine _sealsTimer;

	private int _sealsSeconds;

	private Character _character;

	private int _charID;

	private List<CharacterExtraInfoData> _extraInfoData;

	public int seals
	{
		get
		{
			return GetData(CURRENCY_SEAL);
		}
		set
		{
			SetData(CURRENCY_SEAL, value);
			_character.Broadcast("SEALS_CHANGE");
		}
	}

	public int sealsMax => (int)((float)VariableBook.sealsMax + Mathf.Round(GameModifier.getTypeTotal(_character.getModifiers(), 110)));

	public int sealsSeconds => _sealsSeconds;

	public long sealsMilliseconds
	{
		get
		{
			return _sealsMilliseconds + Mathf.RoundToInt((Time.realtimeSinceStartup - _sealsStartTime) * 1000f);
		}
		set
		{
			_sealsMilliseconds = value;
			_sealsStartTime = Time.realtimeSinceStartup;
			StartSealsTimer();
			_character.Broadcast("SEALS_MILLISECONDS_CHANGE");
		}
	}

	public long sealsCooldown
	{
		get
		{
			return _sealsCooldown;
		}
		set
		{
			_sealsCooldown = value;
		}
	}

	public string testingGroup => GetDataString(TESTING_GROUP);

	public CharacterExtraInfo(List<CharacterExtraInfoData> extraInfoData, Character character)
	{
		_extraInfoData = extraInfoData;
		_charID = character.id;
		_character = character;
	}

	public static CharacterExtraInfo FromSFSObject(ISFSObject sfsob, Character character)
	{
		List<CharacterExtraInfoData> list = new List<CharacterExtraInfoData>();
		ISFSArray sFSArray = sfsob.GetSFSArray("cha121");
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			CharacterExtraInfoData characterExtraInfoData = CharacterExtraInfoData.FromSFSObject(sFSArray.GetSFSObject(i));
			if (characterExtraInfoData != null)
			{
				list.Add(characterExtraInfoData);
			}
		}
		return new CharacterExtraInfo(list, character);
	}

	private void SetData(int Key, int v)
	{
		int num = -1;
		for (int i = 0; i < _extraInfoData.Count; i++)
		{
			if (_extraInfoData[i].key == Key)
			{
				num = i;
				break;
			}
		}
		if (num < 0)
		{
			_extraInfoData.Add(new CharacterExtraInfoData(_charID, Key, v.ToString(), new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)));
		}
		else
		{
			_extraInfoData[num].data = v.ToString();
		}
	}

	private int GetData(int Key)
	{
		for (int i = 0; i < _extraInfoData.Count; i++)
		{
			if (_extraInfoData[i].key == Key)
			{
				return int.Parse(_extraInfoData[i].data);
			}
		}
		return 0;
	}

	private string GetDataString(int Key)
	{
		for (int i = 0; i < _extraInfoData.Count; i++)
		{
			if (_extraInfoData[i].key == Key)
			{
				return _extraInfoData[i].data;
			}
		}
		return null;
	}

	public void setJsonData(int Key, string v)
	{
		int num = -1;
		for (int i = 0; i < _extraInfoData.Count; i++)
		{
			if (_extraInfoData[i].key == Key)
			{
				num = i;
				break;
			}
		}
		if (num < 0)
		{
			_extraInfoData.Add(new CharacterExtraInfoData(_charID, Key, v, default(DateTime)));
		}
		else
		{
			_extraInfoData[num].data = v;
		}
	}

	public JSONNode getJsonData(int Key)
	{
		for (int i = 0; i < _extraInfoData.Count; i++)
		{
			if (_extraInfoData[i].key == Key)
			{
				return JSON.Parse(_extraInfoData[i].data);
			}
		}
		return null;
	}

	public string getDailyRewardStep()
	{
		return GetDataString(DAILY_REWARDS_STEP);
	}

	public bool showNameplate()
	{
		NamePlateData namePlateData = null;
		for (int i = 0; i < _extraInfoData.Count; i++)
		{
			if (_extraInfoData[i].key != NAMEPLATE)
			{
				continue;
			}
			if (_extraInfoData[i].data != null)
			{
				try
				{
					namePlateData = JsonUtility.FromJson<NamePlateData>(_extraInfoData[i].data);
				}
				catch (Exception ex)
				{
					D.LogException(ex.Message, ex);
				}
			}
			break;
		}
		return namePlateData?.showNameplate ?? true;
	}

	private void StartSealsTimer()
	{
		ClearSealsTimer();
		if (seals < sealsMax)
		{
			_sealsSeconds = (int)(-sealsMilliseconds / 1000);
			if (_sealsSeconds <= 0)
			{
				OnSealsTimerComplete();
			}
			else
			{
				_sealsTimer = GameData.instance.main.coroutineTimer.AddTimer(null, 1000f, CoroutineTimer.TYPE.MILLISECONDS, _sealsSeconds, null, OnSealsTimer);
			}
		}
	}

	private void OnSealsTimer()
	{
		_sealsSeconds--;
		if (_sealsSeconds <= 0)
		{
			OnSealsTimerComplete();
		}
		else
		{
			_character.Broadcast("SEALS_SECONDS_CHANGE");
		}
	}

	private void OnSealsTimerComplete()
	{
		int sealsGain = VariableBook.sealsGain;
		int num = seals + sealsGain;
		if (num > sealsMax)
		{
			num = sealsMax;
		}
		seals = num;
		_sealsMilliseconds -= sealsCooldown;
		StartSealsTimer();
	}

	private void ClearSealsTimer()
	{
		if (GameData.instance.main.coroutineTimer != null && (bool)GameData.instance.main.coroutineTimer.gameObject)
		{
			GameData.instance.main.coroutineTimer.StopTimer(ref _sealsTimer);
		}
	}

	public void Clear()
	{
		ClearSealsTimer();
	}
}
