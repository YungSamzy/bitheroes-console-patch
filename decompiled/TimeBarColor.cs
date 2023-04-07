using System;
using System.Collections;
using com.ultrabit.bitheroes.extensions;
using com.ultrabit.bitheroes.model.utility;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimeBarColor : MonoBehaviour
{
	private const string BLANK = "-";

	[SerializeField]
	private TextMeshProUGUI _text;

	[SerializeField]
	private Image imageChild;

	private float currentValue;

	private float totalMaxValue = 60f;

	private float multiply = 1.4f;

	private float currentPercent;

	private float widthTotal;

	private RectTransform rec;

	private Vector2 size = Vector2.zero;

	private Vector4 v4 = Vector4.zero;

	private bool showDescription = true;

	private bool forcedStart;

	private Material newMat;

	private UnityAction _timerUpdate;

	public UnityEvent COMPLETE = new UnityEvent();

	private Coroutine _coroutine;

	private DateTime _endDate;

	[SerializeField]
	private bool _isByRealTime;

	public string text
	{
		get
		{
			if (_text != null)
			{
				return _text.text;
			}
			return null;
		}
		set
		{
			if (_text != null)
			{
				_text.text = value;
			}
		}
	}

	public void ShowText(bool status)
	{
		if (_text != null)
		{
			_text.gameObject.SetActive(status);
		}
	}

	private void Start()
	{
		if (!forcedStart)
		{
			Config();
			ShowColorBar(show: true);
			_coroutine = StartCoroutine(OnContinueCoroutineTimer());
		}
	}

	private void Config()
	{
		rec = GetComponent<RectTransform>();
		newMat = UnityEngine.Object.Instantiate(imageChild.material);
		imageChild.material = newMat;
		widthTotal = rec.rect.width;
	}

	public void ForceStart(bool invokeSeconds = true, bool showColorBar = true, UnityAction timerUpdate = null)
	{
		if (!forcedStart)
		{
			forcedStart = true;
			Config();
			ShowColorBar(showColorBar);
			_timerUpdate = timerUpdate;
			if (invokeSeconds)
			{
				_coroutine = StartCoroutine(OnContinueCoroutineTimer());
			}
		}
	}

	private IEnumerator OnContinueCoroutineTimer()
	{
		while (true)
		{
			if (_isByRealTime)
			{
				UpdateSecondsRealTime();
			}
			else
			{
				UpdateSeconds();
			}
			yield return new WaitForSecondsRealtime(1f);
		}
	}

	public void SetByRealEndTime(DateTime endDate, DateTime startDate)
	{
		_isByRealTime = true;
		_endDate = endDate;
		SetMaxValueMilliseconds((_endDate - startDate).Milliseconds);
	}

	private float GetRealTimeSpanLeft()
	{
		return (float)(_endDate - ServerExtension.instance.GetDate()).TotalSeconds;
	}

	private void CompleteTime()
	{
		size.y = rec.rect.height;
		size.x = 0f;
		rec.sizeDelta = size;
		text = "-";
		if (_coroutine != null)
		{
			StopCoroutine(_coroutine);
		}
		COMPLETE.Invoke();
	}

	private void UpdateSecondsRealTime()
	{
		float realTimeSpanLeft = GetRealTimeSpanLeft();
		if (realTimeSpanLeft <= 0f)
		{
			CompleteTime();
			return;
		}
		if (_timerUpdate != null)
		{
			_timerUpdate();
		}
		if (showDescription)
		{
			text = Util.TimeFormatClean(realTimeSpanLeft);
		}
		else
		{
			text = realTimeSpanLeft.ToString();
		}
		currentPercent = realTimeSpanLeft * 100f / totalMaxValue;
		ShowCurrentPercent();
	}

	private void UpdateSeconds()
	{
		if (currentValue <= 0f)
		{
			CompleteTime();
			return;
		}
		currentValue -= 1f;
		if (_timerUpdate != null)
		{
			_timerUpdate();
		}
		if (showDescription)
		{
			text = Util.TimeFormatClean(currentValue);
		}
		else
		{
			text = currentValue.ToString();
		}
		currentPercent = currentValue * 100f / totalMaxValue;
		ShowCurrentPercent();
	}

	private void ShowCurrentPercent()
	{
		if (currentPercent > 100f)
		{
			currentPercent = 100f;
		}
		size.x = widthTotal * currentPercent / 100f;
		if (rec != null)
		{
			_ = rec.rect;
			size.y = rec.rect.height;
		}
		if (currentPercent >= 0f && currentPercent < 20f)
		{
			v4 = new Vector4(1.61f, 0f, 0.14f, 0f);
			if (imageChild != null && imageChild.material != null)
			{
				imageChild.material.SetVector("_HSLAAdjust", v4);
			}
		}
		if (currentPercent > 20f && currentPercent < 90f)
		{
			v4 = new Vector4(multiply - currentPercent / 150f * -1f, 0f, 0.16f, 0f);
			if (imageChild != null && imageChild.material != null)
			{
				imageChild.material.SetVector("_HSLAAdjust", v4);
			}
		}
		if (currentPercent > 90f && currentPercent <= 100f)
		{
			v4 = new Vector4(2f, 0f, 0f, 0f);
			if (imageChild != null && imageChild.material != null)
			{
				imageChild.material.SetVector("_HSLAAdjust", v4);
			}
		}
		if (rec != null)
		{
			rec.sizeDelta = size;
		}
	}

	public void ShowColorWithoutTimer()
	{
		currentPercent = currentValue * 100f / totalMaxValue;
		if (currentPercent > 100f)
		{
			currentPercent = 100f;
		}
		size.x = widthTotal * currentPercent / 100f;
		size.y = rec.rect.height;
		if (currentPercent >= 0f && currentPercent < 20f)
		{
			v4 = new Vector4(1.61f, 0f, 0.14f, 0f);
			if (imageChild != null)
			{
				imageChild.material.SetVector("_HSLAAdjust", v4);
			}
		}
		if (currentPercent > 20f && currentPercent < 90f)
		{
			v4 = new Vector4(multiply - currentPercent / 150f * -1f, 0f, 0.16f, 0f);
			if (imageChild != null)
			{
				imageChild.material.SetVector("_HSLAAdjust", v4);
			}
		}
		if (currentPercent > 90f && currentPercent <= 100f)
		{
			v4 = new Vector4(2f, 0f, 0f, 0f);
			if (imageChild != null)
			{
				imageChild.material.SetVector("_HSLAAdjust", v4);
			}
		}
		if (rec != null)
		{
			rec.sizeDelta = size;
		}
	}

	public void SetCurrentValueMilliseconds(long milliseconds)
	{
		currentValue = milliseconds / 1000;
	}

	public void SetCurrentValueSeconds(float seconds)
	{
		currentValue = seconds;
	}

	public float GetCurrentValueSeconds()
	{
		return currentValue;
	}

	public float GetCurrentValueMilliseconds()
	{
		return currentValue * 1000f;
	}

	public void SetMaxValueMilliseconds(long milliseconds)
	{
		totalMaxValue = milliseconds / 1000;
	}

	public void SetMaxValueSeconds(float seconds)
	{
		totalMaxValue = seconds;
	}

	public void SetShowDescription(bool show)
	{
		showDescription = show;
	}

	public void ShowColorBar(bool show)
	{
		imageChild.gameObject.SetActive(show);
	}

	private void OnDestroy()
	{
		if (_coroutine != null)
		{
			StopCoroutine(_coroutine);
		}
		UnityEngine.Object.Destroy(newMat);
	}
}
