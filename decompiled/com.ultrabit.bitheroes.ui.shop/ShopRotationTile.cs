using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.shop;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.shop;

public class ShopRotationTile : ShopItemTile
{
	private readonly string[] TEXT_ROTATIONS = new string[2] { "ui_flash", "ui_sale" };

	public TextMeshProUGUI timeTxt;

	private Coroutine _timer;

	private int _timerIndex;

	private ShopSaleRef _rotationRef;

	public void LoadDetails(ShopWindow shopWindow)
	{
		UpdateSaleRef();
		Init(_rotationRef.itemRef, shopWindow);
		GameData.instance.PROJECT.character.AddListener("SHOP_ROTATION_SECONDS_CHANGE", OnSecondsChange);
		OnSecondsChange();
		UpdateText();
		CreateTimer();
		loadAssets();
	}

	private void OnSecondsChange()
	{
		if (!VariableBook.shopRotationVisible)
		{
			timeTxt.gameObject.SetActive(value: false);
			return;
		}
		timeTxt.gameObject.SetActive(value: true);
		int num = GameData.instance.PROJECT.character.shopRotationSeconds;
		if (_rotationRef.dateRef != null)
		{
			num = Mathf.RoundToInt(_rotationRef.dateRef.getMillisecondsUntilEnd() / 1000);
		}
		timeTxt.text = Util.TimeFormatClean(num);
	}

	private void ClearTimer()
	{
		GameData.instance.main.coroutineTimer.StopTimer(ref _timer);
	}

	private void CreateTimer()
	{
		ClearTimer();
		_timer = GameData.instance.main.coroutineTimer.AddTimer(base.gameObject, 2000f, 0, null, OnTimer);
	}

	private void OnTimer()
	{
		if (base.gameObject.activeSelf)
		{
			_timerIndex++;
			if (_timerIndex >= TEXT_ROTATIONS.Length)
			{
				_timerIndex = 0;
			}
			UpdateText();
		}
	}

	public void UpdateText()
	{
		string text = Language.GetString(TEXT_ROTATIONS[_timerIndex]).ToUpperInvariant();
		ribbonTxt.text = text;
	}

	public void UpdateSaleRef()
	{
		_rotationRef = ShopBook.LookupRotation(GameData.instance.PROJECT.character.shopRotationID);
	}

	public override void DoUpdate()
	{
		UpdateSaleRef();
		setItem(_rotationRef.itemRef);
		loadAssets();
		base.DoUpdate();
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		ClearTimer();
		GameData.instance.PROJECT.character.RemoveListener("SHOP_ROTATION_SECONDS_CHANGE", OnSecondsChange);
	}
}
