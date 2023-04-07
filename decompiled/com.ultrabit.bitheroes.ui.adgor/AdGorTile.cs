using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.adgor;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.transaction;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.utility;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.adgor;

public class AdGorTile : MonoBehaviour
{
	public Image timeBar;

	public Image placeholderImage;

	public Image noFull;

	public Image adIcon;

	public Image vitGorIcon;

	public ItemRankTile[] ranks = new ItemRankTile[5];

	private Coroutine _timer;

	private AdGor _adgor;

	private Button tileButton;

	private HoverImages hover;

	public void LoadDetails(bool clickable = true)
	{
		DoReset();
		_adgor = new AdGor();
		vitGorIcon.gameObject.SetActive(value: false);
		tileButton = GetComponent<Button>();
		tileButton.onClick.AddListener(OnTileClick);
		hover = tileButton.gameObject.AddComponent<HoverImages>();
		hover.ForceStart();
		_adgor.AddListener("ADGOR_TIMER_INIT", OnTimerInit);
		_adgor.AddListener("ADGOR_STEP_CHANGE", OnStepChange);
		_adgor.AddListener("ADGOR_TIMER_FINISH", OnTimerFinish);
		_adgor.AddListener("ADGOR_UPDATE", OnAdGorUpdate);
		if (_adgor.step > 0)
		{
			OnTimerInit();
			OnStepChange();
		}
		else
		{
			hover.canGlow = true;
			hover.StartGlow(noStopOnHover: false, restartGlowonExit: true);
		}
		if (_adgor.vipgor)
		{
			setAdgorBtnVipgor();
		}
		else
		{
			_adgor.CheckCoolDown();
		}
	}

	private void OnAdGorUpdate()
	{
		if (_adgor.vipgor)
		{
			setAdgorBtnVipgor();
		}
	}

	private void OnStepChange()
	{
		for (int i = 1; i <= _adgor.step; i++)
		{
			ranks[i - 1].LoadDetails(enabled: true, 3);
		}
		if (_adgor.step == 5)
		{
			noFull.gameObject.SetActive(value: false);
			hover.StopGlowing();
		}
		else
		{
			noFull.gameObject.SetActive(value: true);
			hover.StartGlow(noStopOnHover: true);
		}
	}

	private void OnTimerInit()
	{
		if (_adgor.GetMillisecondsRemaining() > 0)
		{
			ClearTimer();
			_timer = GameData.instance.main.coroutineTimer.AddTimer(base.gameObject, 1000f, CoroutineTimer.TYPE.MILLISECONDS, 0, null, OnTimer);
			timeBar.gameObject.SetActive(value: true);
		}
	}

	private void OnTimerFinish()
	{
		SetTime(0.0, 1.0);
		ClearTimer();
		DoReset();
		TransactionManager.instance.ForceUpdateConsumableModifier(force: true);
	}

	private void OnTimer()
	{
		DoUpdate();
	}

	private void ClearTimer()
	{
		if (_timer != null)
		{
			GameData.instance.main.coroutineTimer.StopTimer(ref _timer);
			timeBar.gameObject.SetActive(value: false);
			for (int i = 1; i <= 5; i++)
			{
				ranks[i - 1].LoadDetails(enabled: true, 1);
			}
		}
	}

	public void DoUpdate()
	{
		double num = _adgor.GetMillisecondsRemaining();
		if (num <= 0.0)
		{
			ClearTimer();
			DoReset();
		}
		else
		{
			SetTime(num, _adgor.MillisecondsTotal());
		}
	}

	public void SetTime(double currentMilliseconds, double totalMilliseconds)
	{
		timeBar.GetComponent<RegularBarFill>().UpdateBar(currentMilliseconds, totalMilliseconds);
		float num = (float)currentMilliseconds / (float)totalMilliseconds;
		if (num < 0f)
		{
			num = 0f;
		}
		if (num > 1f)
		{
			num = 1f;
		}
		if (timeBar.gameObject.activeSelf)
		{
			timeBar.GetComponent<Animator>().Play("HealthBar", 0, Mathf.Round(100f - num * 100f) * 0.01f);
			timeBar.GetComponent<Animator>().speed = 0f;
			timeBar.gameObject.SetActive(num > 0f);
		}
	}

	private void DoReset()
	{
		timeBar.gameObject.SetActive(value: false);
		for (int i = 1; i <= 5; i++)
		{
			int num = i - 1;
			if (ranks.Length > num && ranks[num] != null)
			{
				ranks[num].LoadDetails(enabled: true, 1);
			}
		}
	}

	private void OnTileClick()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if ((AppInfo.allowAds && AppInfo.IsMobile()) || _adgor.step > 0)
		{
			GameData.instance.windowGenerator.NewAdGorWindow(_adgor);
		}
		else
		{
			GameData.instance.windowGenerator.NewVipGorWindow();
		}
	}

	private void OnDestroy()
	{
		if (tileButton != null)
		{
			tileButton.onClick.RemoveListener(OnTileClick);
		}
	}

	private void setAdgorBtnVipgor()
	{
		adIcon.gameObject.SetActive(value: false);
		vitGorIcon.gameObject.SetActive(value: true);
	}
}
