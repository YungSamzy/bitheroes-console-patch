using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.utility;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.menu;

public class MenuInterfaceNBPTile : MainUIButton
{
	public TextMeshProUGUI timeTxt;

	private Coroutine _timer;

	public override void Create()
	{
		LoadDetails(Language.GetString("ui_nbp"), VariableBook.GetGameRequirement(26));
		DoHide();
	}

	public void DoHide()
	{
		ClearTimer();
	}

	public void DoShow()
	{
		StartTimer();
	}

	public override void DoUpdate()
	{
		base.DoUpdate();
		long nbpMilliseconds = GameData.instance.PROJECT.character.nbpMilliseconds;
		float num = (float)nbpMilliseconds / 1000f;
		if (nbpMilliseconds <= 0)
		{
			GameData.instance.PROJECT.character.Broadcast("NBP_DATE_CHANGE");
		}
		else
		{
			timeTxt.text = Util.TimeFormat((int)num);
		}
	}

	private void ClearTimer()
	{
		if (_timer != null)
		{
			GameData.instance.main.coroutineTimer.StopTimer(ref _timer);
		}
	}

	private void StartTimer()
	{
		ClearTimer();
		_timer = GameData.instance.main.coroutineTimer.AddTimer(base.gameObject, 1000f, CoroutineTimer.TYPE.MILLISECONDS, 0, null, Timer);
		DoUpdate();
	}

	private void Timer()
	{
		DoUpdate();
	}

	public override void DoClick()
	{
		base.DoClick();
		GameData.instance.windowGenerator.ShowNBP();
	}
}
