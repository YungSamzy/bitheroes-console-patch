using System.Collections;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.daily;
using com.ultrabit.bitheroes.ui.game;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui.daily;

public class DailyBonusTile : GameTimeTile
{
	private IEnumerator _timer;

	private Coroutine timerCoroutine;

	private DailyBonusRef _currentRef;

	public override void LoadDetails(bool clickable = true)
	{
		base.LoadDetails(clickable);
		_timer = Timer();
		timerCoroutine = StartCoroutine(_timer);
		DoUpdate();
	}

	public override void OnTileClick()
	{
		base.OnTileClick();
		DailyBonusRef currentBonusRef = DailyBonusBook.GetCurrentBonusRef();
		DailyBonusRef nextBonusRef = DailyBonusBook.GetNextBonusRef();
		UnityAction onComplete = null;
		if (currentBonusRef == null)
		{
			return;
		}
		if (nextBonusRef != null)
		{
			onComplete = delegate
			{
				GameData.instance.windowGenerator.NewGameModifierTimeWindow(nextBonusRef.name, 86400000L, 86400000L, nextBonusRef.modifiers);
				DoUpdate();
			};
		}
		GameData.instance.windowGenerator.NewGameModifierTimeWindow(currentBonusRef.name, currentBonusRef.GetMillisecondsRemaining(), 86400000L, currentBonusRef.modifiers, onComplete);
	}

	public override void DoUpdate()
	{
		base.DoUpdate();
		DailyBonusRef currentBonusRef = DailyBonusBook.GetCurrentBonusRef();
		if (currentBonusRef == null)
		{
			base.gameObject.SetActive(value: false);
		}
		else
		{
			base.gameObject.SetActive(value: true);
			long millisecondsRemaining = currentBonusRef.GetMillisecondsRemaining();
			SetTime(millisecondsRemaining, 86400000.0);
		}
		if (_currentRef == null || _currentRef.id != currentBonusRef.id)
		{
			_currentRef = currentBonusRef;
			CHANGE.Invoke();
			SetAsset(currentBonusRef.GetSpriteIcon());
		}
	}

	private IEnumerator Timer()
	{
		while (base.enabled)
		{
			DoUpdate();
			yield return new WaitForSeconds(60f);
		}
	}

	private void OnEnable()
	{
		if (_timer != null && timerCoroutine == null)
		{
			timerCoroutine = StartCoroutine(_timer);
		}
	}

	private void OnDisable()
	{
		StopCoroutine(timerCoroutine);
	}
}
