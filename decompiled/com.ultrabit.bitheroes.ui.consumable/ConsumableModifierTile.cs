using System.Collections;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.consumable;
using com.ultrabit.bitheroes.ui.game;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.consumable;

public class ConsumableModifierTile : GameTimeTile
{
	private ConsumableModifierData _consumableData;

	private IEnumerator _timer;

	public override void LoadDetails(ConsumableModifierData consumableData, bool clickable = true)
	{
		base.LoadDetails();
		_consumableData = consumableData;
		SetAsset(consumableData.consumableRef.GetSpriteIcon(out var isPrefab));
		if (isPrefab)
		{
			SetAsset(consumableData.consumableRef.GetPrefab());
		}
		if (base.gameObject.activeInHierarchy)
		{
			StartCoroutine(Timer());
		}
	}

	public override void OnTileClick()
	{
		base.OnTileClick();
		long.TryParse(_consumableData.consumableRef.value, out var result);
		GameData.instance.windowGenerator.NewGameModifierTimeWindow(_consumableData.consumableRef.coloredName, _consumableData.getMillisecondsRemaining(), result, _consumableData.consumableRef.modifiers);
	}

	private void ClearTimer()
	{
		if (_timer != null)
		{
			StopCoroutine(_timer);
			_timer = null;
		}
	}

	private IEnumerator Timer()
	{
		if (_consumableData != null)
		{
			long milliseconds;
			do
			{
				milliseconds = _consumableData.getMillisecondsRemaining();
				SetTime(milliseconds, long.Parse(_consumableData.consumableRef.value));
				yield return new WaitForSeconds(1f);
			}
			while (milliseconds > 0);
			CHANGE.Invoke();
		}
	}
}
