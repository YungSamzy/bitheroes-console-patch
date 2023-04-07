using System;
using System.Collections;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.utility;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.utility;

[Serializable]
public class CurrencyBarFill : MonoBehaviour
{
	public TextMeshProUGUI currencyText;

	public CurrencyRef.CURRENCY_TYPE currencyType;

	private float totalWidth;

	private float height;

	private float newWidth;

	private bool _initialized;

	private const int TOGGLE_SECONDS = 4;

	private bool showingTime;

	private string secondChangeEventName;

	private string currencyChangeEventName;

	private Coroutine _timer;

	private void PreInit()
	{
		totalWidth = base.gameObject.GetComponent<RectTransform>().sizeDelta.x;
		height = base.gameObject.GetComponent<RectTransform>().sizeDelta.y;
	}

	public void Init()
	{
		if (!_initialized)
		{
			PreInit();
			_initialized = true;
			showingTime = false;
			switch (currencyType)
			{
			case CurrencyRef.CURRENCY_TYPE.CURRENCY_GOLD:
				currencyChangeEventName = "CURRENCY_CHANGE";
				break;
			case CurrencyRef.CURRENCY_TYPE.CURRENCY_CREDITS:
				currencyChangeEventName = "CURRENCY_CHANGE";
				break;
			case CurrencyRef.CURRENCY_TYPE.CURRENCY_ENERGY:
				secondChangeEventName = "ENERGY_SECONDS_CHANGE";
				currencyChangeEventName = "ENERGY_CHANGE";
				break;
			case CurrencyRef.CURRENCY_TYPE.CURRENCY_TICKETS:
				secondChangeEventName = "TICKETS_SECONDS_CHANGE";
				currencyChangeEventName = "TICKETS_CHANGE";
				break;
			case CurrencyRef.CURRENCY_TYPE.CURRENCY_BADGES:
				secondChangeEventName = "BADGES_SECONDS_CHANGE";
				currencyChangeEventName = "BADGES_CHANGE";
				break;
			case CurrencyRef.CURRENCY_TYPE.CURRENCY_TOKENS:
				secondChangeEventName = "TOKENS_SECONDS_CHANGE";
				currencyChangeEventName = "TOKENS_CHANGE";
				break;
			case CurrencyRef.CURRENCY_TYPE.CURRENCY_SHARDS:
				secondChangeEventName = "SHARDS_SECONDS_CHANGE";
				currencyChangeEventName = "SHARDS_CHANGE";
				break;
			case CurrencyRef.CURRENCY_TYPE.CURRENCY_SEALS:
				secondChangeEventName = "SEALS_SECONDS_CHANGE";
				currencyChangeEventName = "SEALS_CHANGE";
				break;
			}
			if (secondChangeEventName != null)
			{
				GameData.instance.PROJECT.character.AddListener(secondChangeEventName, OnSecondsChange);
			}
			if (currencyChangeEventName != null)
			{
				GameData.instance.PROJECT.character.AddListener(currencyChangeEventName, OnCurrencyChange);
			}
			UpdateInformation();
			StartToogleTimer();
		}
	}

	private void StartToogleTimer()
	{
		if (AppInfo.IsMobile() && base.transform.parent.gameObject.activeSelf)
		{
			if (_timer != null)
			{
				StopCoroutine(_timer);
			}
			_timer = StartCoroutine(OnToogleTimer());
		}
	}

	private IEnumerator OnToogleTimer()
	{
		yield return new WaitForSeconds(4f);
		if (showingTime)
		{
			OnLostFocus();
		}
		else
		{
			OnFocus();
		}
		if (_timer != null)
		{
			StopCoroutine(_timer);
		}
		_timer = StartCoroutine(OnToogleTimer());
	}

	public void UpdateBar(float currentValue, float maxValue)
	{
		if (_initialized)
		{
			newWidth = 0f;
			if (currentValue < 0f)
			{
				currentValue = 0f;
			}
			if (currentValue > maxValue)
			{
				currentValue = maxValue;
			}
			newWidth = currentValue * totalWidth / maxValue;
			base.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, height);
		}
	}

	private bool CanUpdate()
	{
		return GetCurrencyCount() < GetCurrencyMax();
	}

	public void OnFocus()
	{
		if (CanUpdate())
		{
			showingTime = true;
			UpdateInformation();
		}
	}

	public void OnLostFocus()
	{
		if (CanUpdate())
		{
			showingTime = false;
			UpdateInformation();
		}
	}

	public void OnClick()
	{
	}

	private void OnSecondsChange()
	{
		if (showingTime)
		{
			UpdateInformation();
		}
	}

	private void OnCurrencyChange()
	{
		UpdateInformation();
	}

	public void UpdateInformation()
	{
		if (!(currencyText == null))
		{
			float num = GetCurrencyCount();
			float num2 = GetCurrencyMax();
			string text = Util.NumberFormat(GetCurrencyCount()) + "/" + Util.NumberFormat(GetCurrencyMax());
			switch (currencyType)
			{
			case CurrencyRef.CURRENCY_TYPE.CURRENCY_GOLD:
			case CurrencyRef.CURRENCY_TYPE.CURRENCY_CREDITS:
				num = 0f;
				num2 = 1000f;
				text = Util.NumberFormat(GetCurrencyCount());
				showingTime = false;
				break;
			case CurrencyRef.CURRENCY_TYPE.CURRENCY_TICKETS:
			case CurrencyRef.CURRENCY_TYPE.CURRENCY_SHARDS:
			case CurrencyRef.CURRENCY_TYPE.CURRENCY_TOKENS:
			case CurrencyRef.CURRENCY_TYPE.CURRENCY_BADGES:
			case CurrencyRef.CURRENCY_TYPE.CURRENCY_SEALS:
				text = Util.NumberFormat(GetCurrencyCount());
				break;
			}
			currencyText.gameObject.SetActive(value: false);
			if (num >= num2)
			{
				showingTime = false;
			}
			if (!showingTime || GetCurrencySeconds() <= 0)
			{
				currencyText.text = text;
			}
			else
			{
				currencyText.text = Util.TimeFormat(GetCurrencySeconds());
			}
			currencyText.gameObject.SetActive(value: true);
			UpdateBar(num, num2);
		}
	}

	private int GetCurrencySeconds()
	{
		return currencyType switch
		{
			CurrencyRef.CURRENCY_TYPE.CURRENCY_ENERGY => GameData.instance.PROJECT.character.energySeconds, 
			CurrencyRef.CURRENCY_TYPE.CURRENCY_TICKETS => GameData.instance.PROJECT.character.ticketsSeconds, 
			CurrencyRef.CURRENCY_TYPE.CURRENCY_BADGES => GameData.instance.PROJECT.character.badgesSeconds, 
			CurrencyRef.CURRENCY_TYPE.CURRENCY_TOKENS => GameData.instance.PROJECT.character.tokensSeconds, 
			CurrencyRef.CURRENCY_TYPE.CURRENCY_SHARDS => GameData.instance.PROJECT.character.shardsSeconds, 
			CurrencyRef.CURRENCY_TYPE.CURRENCY_SEALS => GameData.instance.PROJECT.character.extraInfo.sealsSeconds, 
			_ => 0, 
		};
	}

	private int GetCurrencyCount()
	{
		return currencyType switch
		{
			CurrencyRef.CURRENCY_TYPE.CURRENCY_GOLD => GameData.instance.PROJECT.character.gold, 
			CurrencyRef.CURRENCY_TYPE.CURRENCY_CREDITS => GameData.instance.PROJECT.character.credits, 
			CurrencyRef.CURRENCY_TYPE.CURRENCY_ENERGY => GameData.instance.PROJECT.character.energy, 
			CurrencyRef.CURRENCY_TYPE.CURRENCY_TICKETS => GameData.instance.PROJECT.character.tickets, 
			CurrencyRef.CURRENCY_TYPE.CURRENCY_BADGES => GameData.instance.PROJECT.character.badges, 
			CurrencyRef.CURRENCY_TYPE.CURRENCY_TOKENS => GameData.instance.PROJECT.character.tokens, 
			CurrencyRef.CURRENCY_TYPE.CURRENCY_SHARDS => GameData.instance.PROJECT.character.shards, 
			CurrencyRef.CURRENCY_TYPE.CURRENCY_SEALS => GameData.instance.PROJECT.character.extraInfo.seals, 
			_ => 0, 
		};
	}

	private int GetCurrencyMax()
	{
		return currencyType switch
		{
			CurrencyRef.CURRENCY_TYPE.CURRENCY_ENERGY => GameData.instance.PROJECT.character.energyMax, 
			CurrencyRef.CURRENCY_TYPE.CURRENCY_TICKETS => GameData.instance.PROJECT.character.ticketsMax, 
			CurrencyRef.CURRENCY_TYPE.CURRENCY_BADGES => GameData.instance.PROJECT.character.badgesMax, 
			CurrencyRef.CURRENCY_TYPE.CURRENCY_SHARDS => GameData.instance.PROJECT.character.shardsMax, 
			CurrencyRef.CURRENCY_TYPE.CURRENCY_TOKENS => GameData.instance.PROJECT.character.tokensMax, 
			CurrencyRef.CURRENCY_TYPE.CURRENCY_SEALS => GameData.instance.PROJECT.character.extraInfo.sealsMax, 
			_ => 0, 
		};
	}

	private void OnDestroy()
	{
		StopCoroutine(_timer);
		if (GameData.instance == null || GameData.instance.PROJECT == null || GameData.instance.PROJECT.character != null)
		{
			if (secondChangeEventName != null)
			{
				GameData.instance.PROJECT.character.RemoveListener(secondChangeEventName, OnSecondsChange);
			}
			if (currencyChangeEventName != null)
			{
				GameData.instance.PROJECT.character.RemoveListener(currencyChangeEventName, OnCurrencyChange);
			}
		}
	}

	private void OnEnable()
	{
		if (_timer != null)
		{
			StopCoroutine(_timer);
		}
		_timer = StartCoroutine(OnToogleTimer());
	}

	private void OnDisable()
	{
		StopCoroutine(_timer);
	}
}
