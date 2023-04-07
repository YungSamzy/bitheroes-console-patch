using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.messenger;
using com.ultrabit.bitheroes.model.consumable;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.utility;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.adgor;

public class AdGor : Messenger
{
	public static string VIPGOR_NAMEPLATE_COLOR = "f7b614";

	private int _step;

	private long _millisecondsTotal;

	private long _millisecondsRemaining;

	private Coroutine _timer;

	private long _timerTime;

	private Coroutine _timerCooldown;

	private long _timerTimeCooldown;

	private float _totalMs = 100000f;

	private int _firstAdgorIdprev = 3000;

	private int _firstExtendIdprev = 3005;

	private long _cooldown;

	private bool _watching;

	public static bool devMode = false;

	private static ConsumableRef[] _adgorStepsConsumables;

	public static ConsumableRef[] adgorStepsConsumables
	{
		get
		{
			if (_adgorStepsConsumables == null)
			{
				_adgorStepsConsumables = new ConsumableRef[5];
				for (int i = 0; i < _adgorStepsConsumables.Length; i++)
				{
					_adgorStepsConsumables[i] = ConsumableBook.Lookup(VariableBook.adgorRef.GetFirstAdGor() + i);
				}
			}
			return _adgorStepsConsumables;
		}
	}

	public int step
	{
		get
		{
			return _step;
		}
		set
		{
			_step = value;
			Broadcast("ADGOR_STEP_CHANGE");
		}
	}

	public long cooldown
	{
		get
		{
			return _cooldown;
		}
		set
		{
			_cooldown = value;
		}
	}

	public bool vipgor => hasVipgorActive();

	public AdGor()
	{
		_step = 0;
		_millisecondsTotal = 0L;
		_millisecondsRemaining = 0L;
		ConsumableRef consumableRef = null;
		consumableRef = (vipgor ? getVipgor() : adgorStepsConsumables[0]);
		_totalMs = Util.ParseFloat(consumableRef.value);
		_firstAdgorIdprev = VariableBook.adgorRef.GetFirstAdGor() - 1;
		if (vipgor)
		{
			setVipGor(consumableRef);
		}
		else
		{
			DoGetAdGorState();
		}
		ConsumableManager.instance.OnAdgorUpdate.AddListener(PurchaseComplete);
		CheckCoolDown();
	}

	public void CheckCoolDown()
	{
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(63), OnCheckAdgorCooldown);
		CharacterDALC.instance.doCheckAdGorCooldown();
	}

	private void OnCheckAdgorCooldown(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(63), OnCheckAdgorCooldown);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			D.LogError(string.Format("{0}::onCheckAdgorCooldown::{1}", GetType(), sfsob.GetInt("err0")));
		}
		else if (sfsob.ContainsKey("adgor02"))
		{
			_cooldown = sfsob.GetLong("adgor02");
			SetTimerCooldown();
			Broadcast("ADGOR_COOLDOWN_UPDATE");
		}
	}

	private void PurchaseComplete()
	{
		ConsumableManager.instance.OnAdgorUpdate.RemoveListener(PurchaseComplete);
		if (vipgor)
		{
			CheckCoolDown();
			ConsumableRef consumableRef = getVipgor();
			setVipGor(consumableRef);
			Broadcast("ADGOR_UPDATE");
			if (!GameData.instance.SAVE_STATE.viewVipgorPurchaseSuccess)
			{
				GameData.instance.SAVE_STATE.viewVipgorPurchaseSuccess = true;
				GameData.instance.windowGenerator.NewVipGorSuccessWindow(consumableRef);
			}
		}
	}

	private void DoGetAdGorState()
	{
		List<int> adGorIDs = VariableBook.adgorRef.GetAdGorIDs();
		foreach (ConsumableModifierData consumableModifier in GameData.instance.PROJECT.character.consumableModifiers)
		{
			if (consumableModifier != null && consumableModifier.isActive())
			{
				int id = consumableModifier.consumableRef.id;
				if (adGorIDs.IndexOf(id) > -1)
				{
					_millisecondsTotal = long.Parse(consumableModifier.consumableRef.value);
					_millisecondsRemaining = consumableModifier.getMillisecondsRemaining();
					_step = id - _firstAdgorIdprev;
				}
			}
		}
		StepUpdate();
	}

	private void StepUpdate()
	{
		if (_step > 0)
		{
			step = _step;
			SetTimer();
		}
	}

	public void Watch()
	{
		if (!_watching && _step < 5)
		{
			_watching = true;
			_step++;
			step = _step;
			if (_step == 1 && _timer == null)
			{
				_millisecondsRemaining = (long)_totalMs;
				_millisecondsTotal = (long)_totalMs;
				SetTimer();
			}
			ConsumableRef consumableRef = ConsumableBook.Lookup(_step + _firstAdgorIdprev);
			CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(62), OnUseAdGorConsumableModifier);
			CharacterDALC.instance.doUseAdGorConsumableModifier(consumableRef, _step);
		}
	}

	private void OnUseAdGorConsumableModifier(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(62), OnUseAdGorConsumableModifier);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			D.LogError(string.Format("{0}::OnUseAdGorConsumableModifier::{1}", GetType(), sfsob.GetInt("err0")));
		}
		else
		{
			List<ConsumableModifierData> consumableModifiers = ConsumableModifierData.listFromSFSObject(sfsob);
			GameData.instance.PROJECT.character.consumableModifiers = consumableModifiers;
		}
		_watching = false;
	}

	private void SetTimer()
	{
		if (_step > 0)
		{
			_timerTime = Mathf.RoundToInt(Time.realtimeSinceStartup * 1000f);
			_timer = GameData.instance.main.coroutineTimer.AddTimer(null, _millisecondsRemaining, OnTimer);
			Broadcast("ADGOR_TIMER_INIT");
		}
	}

	private void OnTimer()
	{
		if (GetMillisecondsRemaining() <= 0)
		{
			_step = 0;
			step = _step;
			_millisecondsTotal = 0L;
			_millisecondsRemaining = 0L;
			Broadcast("ADGOR_TIMER_FINISH");
			CharacterDALC.instance.doAdgorTimerFinish();
			ClearTimer();
			CheckCoolDown();
		}
	}

	private void ClearTimer()
	{
		GameData.instance.main.coroutineTimer.StopTimer(ref _timer);
	}

	private void SetTimerCooldown()
	{
		if (_cooldown > 0)
		{
			_timerTimeCooldown = (long)(Time.realtimeSinceStartup * 1000f);
			_timerCooldown = GameData.instance.main.coroutineTimer.AddTimer(null, _cooldown, CoroutineTimer.TYPE.MILLISECONDS, 0, null, OnTimerCooldown);
		}
	}

	private void OnTimerCooldown()
	{
		_cooldown = 0L;
		cooldown = _cooldown;
		ClearTimerCooldown();
		Broadcast("ADGOR_COOLDOWN_UPDATE");
	}

	private void ClearTimerCooldown()
	{
		GameData.instance.main.coroutineTimer.StopTimer(ref _timerCooldown);
	}

	public long MillisecondsTotal()
	{
		return _millisecondsTotal;
	}

	public long GetMillisecondsRemaining()
	{
		return _millisecondsRemaining - (Mathf.RoundToInt(Time.realtimeSinceStartup * 1000f) - _timerTime);
	}

	public long GetCooldownRemaining()
	{
		return _cooldown - (Mathf.RoundToInt(Time.realtimeSinceStartup * 1000f) - _timerTimeCooldown);
	}

	public bool hasVipgorActive()
	{
		List<int> vipgorIDs = VariableBook.adgorRef.getVipgorIDs();
		if (GameData.instance.PROJECT.character == null)
		{
			return false;
		}
		_ = GameData.instance.PROJECT.character.consumableModifiers;
		foreach (ConsumableModifierData consumableModifier in GameData.instance.PROJECT.character.consumableModifiers)
		{
			if (vipgorIDs.IndexOf(consumableModifier.consumableRef.id) >= 0 && consumableModifier.isActive())
			{
				return true;
			}
		}
		return false;
	}

	public ConsumableRef getVipgor()
	{
		List<int> vipgorIDs = VariableBook.adgorRef.getVipgorIDs();
		foreach (ConsumableModifierData consumableModifier in GameData.instance.PROJECT.character.consumableModifiers)
		{
			if (vipgorIDs.IndexOf(consumableModifier.consumableRef.id) >= 0 && consumableModifier.isActive())
			{
				return consumableModifier.consumableRef;
			}
		}
		return null;
	}

	public ConsumableModifierData getVipgorData()
	{
		List<int> vipgorIDs = VariableBook.adgorRef.getVipgorIDs();
		foreach (ConsumableModifierData consumableModifier in GameData.instance.PROJECT.character.consumableModifiers)
		{
			if (vipgorIDs.IndexOf(consumableModifier.consumableRef.id) >= 0 && consumableModifier.isActive())
			{
				return consumableModifier;
			}
		}
		return null;
	}

	private void setVipGor(ConsumableRef consumableRef)
	{
		_step = 5;
		_millisecondsTotal = long.Parse(consumableRef.value);
		ConsumableModifierData vipgorData = getVipgorData();
		_millisecondsRemaining = vipgorData.getMillisecondsRemaining();
		SetTimer();
	}

	public static string GetNameplateColorString(string text)
	{
		return Util.colorString(text, VIPGOR_NAMEPLATE_COLOR);
	}

	public static string ConvertNameplateAS3ToUnity(string text)
	{
		text = text.Replace("<font color='", "<color=");
		text = text.Replace("'>", ">");
		text = text.Replace("</font>", "</color>");
		return text;
	}
}
