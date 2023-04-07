using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.ad;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.adgor;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.consumable;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.tutorial;
using com.ultrabit.bitheroes.ui.utility;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.adgor;

public class AdGorWindow : WindowsMain
{
	private const int ADGOR_TOTAL_STEPS = 5;

	private string AD_HUD_PLACEMENT = "Hud";

	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI descTxt;

	public Button adGorBtn;

	public Button shopBtn;

	public Button vipGorBtn;

	public AdGorBar adgorBar;

	public TextMeshProUGUI timeNameTxt;

	public TextMeshProUGUI[] completedTxt = new TextMeshProUGUI[5];

	public TextMeshProUGUI watchUpTxt;

	public TimeBarColor timeBar;

	private bool _watching;

	private AdGor _adgor;

	private bool _simulate;

	private Coroutine _timerSimulate;

	private Coroutine _timerCooldown;

	private TextMeshProUGUI _vipGorBtnTxt;

	private TextMeshProUGUI _adGorBtnTxt;

	private bool adsAllowed
	{
		get
		{
			if (_simulate)
			{
				return true;
			}
			if (AppInfo.allowAds)
			{
				return AppInfo.IsMobile();
			}
			return false;
		}
	}

	private void Awake()
	{
		_vipGorBtnTxt = vipGorBtn.GetComponentInChildren<TextMeshProUGUI>();
		_adGorBtnTxt = adGorBtn.GetComponentInChildren<TextMeshProUGUI>();
	}

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(AdGor adGor)
	{
		_simulate = false;
		Util.SetButton(adGorBtn, enabled: false);
		_adgor = adGor;
		DoReset();
		shopBtn.gameObject.SetActive(value: false);
		_adgor.AddListener("ADGOR_TIMER_INIT", OnTimerInit);
		_adgor.AddListener("ADGOR_STEP_CHANGE", OnStepChange);
		_adgor.AddListener("ADGOR_TIMER_FINISH", OnTimerFinish);
		_adgor.AddListener("ADGOR_UPDATE", OnAdGorUpdate);
		_adgor.AddListener("ADGOR_COOLDOWN_UPDATE", OnAdGorCooldownUpdate);
		topperTxt.text = Language.GetString("adgor_instance_name");
		timeNameTxt.text = Language.GetString("ui_time_available") + ":";
		_vipGorBtnTxt.text = Language.GetString("ui_vipgor");
		SetAdgorBtn();
		SetSteps();
		SetTimer();
		adgorBar.GoToAndStopBgBar(1);
		adgorBar.GoToAndStop(_adgor.step + 1);
		if (_adgor.vipgor)
		{
			setAdgorWindowVipgor();
		}
		else
		{
			_adgor.CheckCoolDown();
		}
		ListenForBack(OnClose);
		SCROLL_IN_COMPLETE.AddListener(delegate
		{
			CheckTutorial();
		});
		CreateWindow();
	}

	private void CheckTutorial()
	{
		TutorialRef tutorialRef = VariableBook.LookUpTutorial("adgor_check");
		if (tutorialRef != null && !GameData.instance.PROJECT.character.tutorial.GetState(102) && GameData.instance.PROJECT.character.tutorial.GetState(101) && tutorialRef.areConditionsMet && adGorBtn != null)
		{
			if (AppInfo.allowAds)
			{
				GameData.instance.PROJECT.character.tutorial.SetState(102);
				GameData.instance.tutorialManager.ShowTutorialForButton(adGorBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(102), 4, adGorBtn.gameObject, 0f, indicator: false, button: false, glow: true, 250, new Vector2(AppInfo.GetLeftOffset(), 0f)), stageTrigger: true, null, funcSameAsTargetFunc: false, null, shadow: false);
			}
			GameData.instance.PROJECT.CheckTutorialChanges();
		}
	}

	private void SetAdgorBtn()
	{
		if (GameData.instance.PROJECT.character.toCharacterData().nftIsAdFree)
		{
			_adGorBtnTxt.text = Language.GetString("ui_fill");
		}
		else if (_adgor.step < 1)
		{
			_adGorBtnTxt.text = Language.GetString("ui_watch");
		}
		else
		{
			_adGorBtnTxt.text = Language.GetString("ui_boost");
		}
		if (_simulate)
		{
			Util.SetButton(adGorBtn);
		}
		if (!adsAllowed)
		{
			Util.SetButton(adGorBtn, enabled: false);
			return;
		}
		bool active = _adgor.step != 5 || _adgor.GetCooldownRemaining() > 0;
		Util.SetButton(adGorBtn, active);
		adGorBtn.gameObject.SetActive(active);
		if (_adgor.GetCooldownRemaining() > 0)
		{
			Util.SetButton(adGorBtn, enabled: false);
		}
	}

	private void OnStepChange()
	{
		SetAdgorBtn();
		SetSteps();
	}

	private void OnTimerInit()
	{
		SetTimer();
	}

	private void OnTimerFinish()
	{
		ClearTimer();
		SetSteps();
		SetAdgorBtn();
		DoReset();
		_adgor.CheckCoolDown();
	}

	private void OnAdGorUpdate()
	{
		if (_adgor.vipgor)
		{
			setAdgorWindowVipgor();
		}
	}

	private void DoReset()
	{
		timeBar.transform.parent.gameObject.SetActive(value: false);
		UpdateCooldownText(_adgor.GetCooldownRemaining());
		SetTextDesc(Language.GetString("adgor_instance_desc"));
	}

	private void SetTimer()
	{
		if (_adgor.step > 0)
		{
			timeBar.transform.parent.gameObject.SetActive(value: true);
			ClearTimer();
			timeBar.SetMaxValueMilliseconds(_adgor.MillisecondsTotal());
			timeBar.SetCurrentValueMilliseconds(_adgor.GetMillisecondsRemaining());
			timeBar.ForceStart();
		}
	}

	private void OnAdGorCooldownUpdate()
	{
		if (_adgor.GetCooldownRemaining() > 0)
		{
			Util.SetButton(adGorBtn, enabled: false);
		}
		else
		{
			SetAdgorBtn();
		}
		SetTimerCooldown();
	}

	private void SetTimerCooldown()
	{
		if (_adgor.GetCooldownRemaining() > 0 && _timerCooldown == null)
		{
			ClearTimerCooldown();
			_timerCooldown = GameData.instance.main.coroutineTimer.AddTimer(base.gameObject, 1000f, CoroutineTimer.TYPE.MILLISECONDS, 0, OnTimerCooldown, OnTimerCooldown);
		}
	}

	private void OnTimerCooldown()
	{
		long cooldownRemaining = _adgor.GetCooldownRemaining();
		UpdateCooldownText(cooldownRemaining);
		if (cooldownRemaining <= 0)
		{
			ClearTimerCooldown();
			DoReset();
			if (adsAllowed)
			{
				adGorBtn.gameObject.SetActive(value: true);
				Util.SetButton(adGorBtn);
			}
		}
	}

	private void UpdateCooldownText(long miliseconds)
	{
		bool flag = (double)miliseconds < 3600000.0;
		watchUpTxt.text = "";
		if (miliseconds <= 0)
		{
			if (!adsAllowed)
			{
				watchUpTxt.text = Language.GetString("adgor_instance_no_watch");
			}
			else
			{
				watchUpTxt.text = Language.GetString("adgor_instance_watch", new string[1] { (5 - _adgor.step).ToString() }, color: true);
			}
		}
		else if (flag)
		{
			watchUpTxt.text = Language.GetString("adgor_instance_watch_cooldown", new string[1] { Util.TimeFormatClean(miliseconds / 1000) }, color: true);
		}
		else
		{
			watchUpTxt.text = Language.GetString("adgor_instance_watch_cooldown", new string[1] { Util.TimeFormat((int)(miliseconds / 1000), isLong: true) }, color: true);
		}
		watchUpTxt.text = "\u00a0" + watchUpTxt.text;
	}

	private void SetTextDesc(string html)
	{
		descTxt.text = html;
	}

	private void SetSteps()
	{
		if (_adgor.step == 5)
		{
			SetTextDesc(Language.GetString("adgor_instancemax_desc"));
		}
		for (int i = 0; i < AdGor.adgorStepsConsumables.Length; i++)
		{
			ConsumableRef consumableRef = AdGor.adgorStepsConsumables[i];
			float num = ((consumableRef.modifiers.Count > 0) ? (consumableRef.modifiers[0].value * 100f) : 0f);
			string text = $"+{num}%";
			if (_adgor.step - 1 == i)
			{
				text = (_adgor.vipgor ? AdGor.GetNameplateColorString(text) : Util.ParseString("^" + text + "^"));
			}
			completedTxt[i].text = text;
		}
		adgorBar.GoToAndStopBgBar(1);
		adgorBar.GoToAndStop(_adgor.step + 1);
		UpdateCooldownText(_adgor.GetCooldownRemaining());
	}

	private void ClearTimer()
	{
		timeBar.CancelInvoke("UpdateSeconds");
	}

	private void ClearTimerCooldown()
	{
		GameData.instance.main.coroutineTimer.StopTimer(ref _timerCooldown);
	}

	public override void DoDestroy()
	{
		ClearTimer();
		ClearTimerCooldown();
		ClearTimerSimulate();
		_adgor.RemoveListener("ADGOR_TIMER_INIT", OnTimerInit);
		_adgor.RemoveListener("ADGOR_STEP_CHANGE", OnStepChange);
		_adgor.RemoveListener("ADGOR_TIMER_FINISH", OnTimerFinish);
		_adgor.RemoveListener("ADGOR_UPDATE", OnAdGorUpdate);
		_adgor.RemoveListener("ADGOR_COOLDOWN_UPDATE", OnAdGorCooldownUpdate);
		base.DoDestroy();
	}

	private void ClearTimerSimulate()
	{
		GameData.instance.main.coroutineTimer.StopTimer(ref _timerSimulate);
	}

	private void DoAdWatchSimulate(string placement)
	{
		if (!_watching)
		{
			_watching = true;
			GameData.instance.windowGenerator.NewConfirmMessageWindow("AD SIMULATION", "An ad is shown.", "CLOSE AD", OnTimerSimulate);
		}
	}

	private void OnTimerSimulate()
	{
		ClearTimerSimulate();
		_adgor.Watch();
		_watching = false;
	}

	private void DoAdWatch(string placement)
	{
		if (!_watching)
		{
			if (GameData.instance.PROJECT.character.toCharacterData().nftIsAdFree)
			{
				_adgor.Watch();
				return;
			}
			_watching = true;
			AppInfo.adEventDispatcher.AddListener(OnAdWatchComplete);
			AppInfo.ShowAd("AdGor");
			KongregateAnalytics.trackAdStart("AdGor", "Rewarded Video");
		}
	}

	private void OnAdWatchComplete(AdEvent adEvent)
	{
		AppInfo.adEventDispatcher.RemoveListener(OnAdWatchComplete);
		_watching = false;
		if (adEvent.success)
		{
			_adgor.Watch();
			KongregateAnalytics.trackAdEnd(null, "step: " + _adgor.step, "Rewarded Video");
		}
	}

	public void OnAdGorBtn()
	{
		if (!_watching)
		{
			GameData.instance.audioManager.PlaySoundLink("buttonclick");
			if (!AppInfo.adsAvailable && !_simulate)
			{
				GameData.instance.windowGenerator.ShowDialogMessage(Language.GetString("adgor_instance_name"), Language.GetString("no_ads_available_desc"));
				return;
			}
			Util.SetButton(adGorBtn, enabled: false);
			CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(63), OnCheckAdgorCooldown);
			CharacterDALC.instance.doCheckAdGorCooldown();
		}
	}

	public void OnVipGorBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewVipGorWindow();
	}

	private void OnCheckAdgorCooldown(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		if (adGorBtn != null)
		{
			adGorBtn.gameObject.SetActive(value: true);
			Util.SetButton(adGorBtn);
		}
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(63), OnCheckAdgorCooldown);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			D.LogError("onCheckAdgorCooldown: " + sfsob.GetInt("err0"));
		}
		else if (sfsob.ContainsKey("adgor02"))
		{
			_adgor.cooldown = sfsob.GetLong("adgor02");
		}
		if (_adgor.cooldown <= 0)
		{
			if (!_simulate)
			{
				DoAdWatch(AD_HUD_PLACEMENT);
			}
			else
			{
				DoAdWatchSimulate(AD_HUD_PLACEMENT);
			}
		}
		else
		{
			bool isLong = (double)_adgor.cooldown < 3600000.0;
			string @string = Language.GetString("adgor_instance_watch_cooldown", new string[1] { Util.TimeFormat((int)(_adgor.cooldown / 1000), isLong) }, color: true);
			GameData.instance.windowGenerator.ShowDialogMessage(Language.GetString("adgor_instance_name"), @string);
		}
	}

	public void OnShopBtn()
	{
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		adGorBtn.interactable = true;
		shopBtn.interactable = true;
		if (!_adgor.vipgor)
		{
			vipGorBtn.interactable = true;
		}
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		adGorBtn.interactable = false;
		shopBtn.interactable = false;
		vipGorBtn.interactable = false;
	}

	private void setAdgorWindowVipgor()
	{
		hideWatchUpTxt();
		adGorBtn.gameObject.SetActive(value: false);
		SetSteps();
		Util.SetButton(vipGorBtn, enabled: false);
		adgorBar.GoToAndStopBgBar(3);
		topperTxt.text = AdGor.GetNameplateColorString(Language.GetString("ui_vipgor"));
	}

	private void hideWatchUpTxt()
	{
		watchUpTxt.text = "";
		watchUpTxt.gameObject.SetActive(value: false);
	}
}
