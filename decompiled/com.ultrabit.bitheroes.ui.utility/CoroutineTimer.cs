using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.utility;

public class CoroutineTimer : MonoBehaviour
{
	public enum TYPE
	{
		MILLISECONDS,
		SECONDS
	}

	public delegate void OnResponse();

	public class CoroutineObject
	{
		public OnResponse onComplete;

		public OnResponse onTimer;

		public OnResponse onCancel;

		public float delay;

		public int repeat;

		public int currentRepetitions;

		public TYPE type;
	}

	public class CoroutineGameObject : CoroutineObject
	{
		public bool sourceAdded;

		public GameObject source;
	}

	private Dictionary<Coroutine, CoroutineGameObject> timers;

	private void Awake()
	{
		timers = new Dictionary<Coroutine, CoroutineGameObject>();
	}

	public Coroutine AddTimer(GameObject source, float delay, int repeat, OnResponse onComplete)
	{
		return AddTimer(source, delay, TYPE.MILLISECONDS, repeat, onComplete, null, null);
	}

	public Coroutine AddTimer(GameObject source, float delay, TYPE type, int repeat, OnResponse onComplete)
	{
		return AddTimer(source, delay, type, repeat, onComplete, null, null);
	}

	public Coroutine AddTimer(GameObject source, float delay, OnResponse onComplete)
	{
		return AddTimer(source, delay, TYPE.MILLISECONDS, 1, onComplete, null, null);
	}

	public Coroutine AddTimer(GameObject source, float delay, TYPE type, OnResponse onComplete)
	{
		return AddTimer(source, delay, type, 1, onComplete, null, null);
	}

	public Coroutine AddTimer(GameObject source, float delay, int repeat, OnResponse onComplete, OnResponse onStep)
	{
		return AddTimer(source, delay, TYPE.MILLISECONDS, repeat, onComplete, onStep);
	}

	public Coroutine AddTimer(GameObject source, float delay, TYPE type = TYPE.MILLISECONDS, int repeat = 1, OnResponse onComplete = null, OnResponse onTimer = null, OnResponse onCancel = null)
	{
		CoroutineGameObject coroutineGameObject = new CoroutineGameObject();
		if (source != null)
		{
			coroutineGameObject.source = source;
			coroutineGameObject.sourceAdded = true;
		}
		coroutineGameObject.onComplete = onComplete;
		coroutineGameObject.onTimer = onTimer;
		coroutineGameObject.onCancel = onCancel;
		coroutineGameObject.type = type;
		if (type == TYPE.MILLISECONDS)
		{
			delay /= 1000f;
		}
		coroutineGameObject.delay = delay;
		coroutineGameObject.repeat = repeat;
		coroutineGameObject.currentRepetitions = 0;
		if (base.gameObject.activeInHierarchy)
		{
			Coroutine coroutine = StartCoroutine(Timer(coroutineGameObject));
			timers.Add(coroutine, coroutineGameObject);
			return coroutine;
		}
		return null;
	}

	private bool IsSourceActive(CoroutineGameObject coroutineObject)
	{
		if (coroutineObject.sourceAdded)
		{
			if (coroutineObject.sourceAdded)
			{
				return coroutineObject.source != null;
			}
			return false;
		}
		return true;
	}

	private IEnumerator Timer(CoroutineGameObject coroutineObject)
	{
		while (coroutineObject.currentRepetitions < coroutineObject.repeat || coroutineObject.repeat == 0)
		{
			coroutineObject.currentRepetitions++;
			yield return new WaitForSecondsRealtime(coroutineObject.delay);
			if (IsSourceActive(coroutineObject) && coroutineObject.onTimer != null && coroutineObject.onTimer.Target != null && !coroutineObject.onTimer.Equals(null))
			{
				coroutineObject.onTimer();
			}
		}
		CompleteTimer(coroutineObject, invokeCallBack: true, cancel: false);
	}

	public void CompleteTimer(CoroutineGameObject coroutineObject, bool invokeCallBack, bool cancel = true)
	{
		if (IsSourceActive(coroutineObject) && coroutineObject.onComplete != null && coroutineObject.onComplete.Target != null && !coroutineObject.onComplete.Equals(null) && invokeCallBack)
		{
			coroutineObject.onComplete();
		}
		Coroutine coroutine = null;
		foreach (KeyValuePair<Coroutine, CoroutineGameObject> timer in timers)
		{
			if (timer.Value.Equals(coroutineObject))
			{
				coroutine = timer.Key;
				break;
			}
		}
		StopTimer(ref coroutine, cancelCallback: false);
	}

	public void RestartTimer(Coroutine coroutine)
	{
		foreach (KeyValuePair<Coroutine, CoroutineGameObject> timer in timers)
		{
			if (timer.Key.Equals(coroutine))
			{
				AddTimer(timer.Value.source, timer.Value.delay, timer.Value.type, timer.Value.repeat, timer.Value.onComplete, timer.Value.onTimer, timer.Value.onCancel);
				break;
			}
		}
	}

	public void StopTimer(ref Coroutine coroutine, bool cancelCallback = true)
	{
		if (this == null || !base.enabled || !(base.gameObject != null) || !base.gameObject.activeInHierarchy || coroutine == null || !timers.ContainsKey(coroutine))
		{
			return;
		}
		CoroutineGameObject coroutineGameObject = timers[coroutine];
		if (coroutineGameObject != null && IsSourceActive(coroutineGameObject) && coroutine != null)
		{
			if (cancelCallback && coroutineGameObject.onCancel != null && coroutineGameObject.onCancel.Target != null && !coroutineGameObject.onCancel.Target.Equals(null))
			{
				coroutineGameObject.onCancel();
			}
			StopCoroutine(coroutine);
			timers.Remove(coroutine);
			coroutine = null;
		}
	}

	private void OnDestroy()
	{
		StopAllCoroutines();
	}
}
