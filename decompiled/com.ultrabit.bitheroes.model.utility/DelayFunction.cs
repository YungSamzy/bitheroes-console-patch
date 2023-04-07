using com.ultrabit.bitheroes.core;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.utility;

public class DelayFunction
{
	private Coroutine timer;

	private UnityAction callback;

	private object parameters;

	private UnityAction<object> callbackWithParams;

	private bool done;

	public void Delay(float seconds, UnityAction callback)
	{
		this.callback = callback;
		timer = GameData.instance.main.coroutineTimer.AddTimer(null, seconds * 1000f, OnTimerCompleted);
	}

	public void Delay(float seconds, UnityAction<object> callback, object parameters = null)
	{
		callbackWithParams = callback;
		this.parameters = parameters;
		timer = GameData.instance.main.coroutineTimer.AddTimer(null, seconds * 1000f, OnTimerCompleted);
	}

	private void OnTimerCompleted()
	{
		if (!done)
		{
			done = true;
			if (callback != null)
			{
				callback();
			}
			if (callbackWithParams != null)
			{
				callbackWithParams(parameters);
			}
			if (timer != null)
			{
				GameData.instance.main.coroutineTimer.StopTimer(ref timer);
			}
		}
	}
}
