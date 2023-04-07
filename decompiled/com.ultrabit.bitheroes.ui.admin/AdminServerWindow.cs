using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.utility;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.admin;

public class AdminServerWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI xmlsTitleTxt;

	public TextMeshProUGUI battlesTitleTxt;

	public TextMeshProUGUI dungeonsTitleTxt;

	public TextMeshProUGUI threadsTxt;

	public TextMeshProUGUI statisticsTxt;

	public TextMeshProUGUI shutdownTxt;

	public TextMeshProUGUI battlesTxt;

	public TextMeshProUGUI dungeonsTxt;

	public TextMeshProUGUI shutdownCountTxt;

	public Button threadPoolBtn;

	public Button disableStatisticsBtn;

	public Button enableStatisticsBtn;

	public Button uploadBtn;

	public Button clearBtn;

	public Button disableBattlesBtn;

	public Button enableBattlesBtn;

	public Button disableDungeonsBtn;

	public Button enableDungeonsBtn;

	public Button checkInfoBtn;

	public Button shutdownStartBtn;

	public Button shutdownCancelBtn;

	public Button refreshBtn;

	public TMP_InputField threadPoolTxt;

	public Toggle instanceCheckbox;

	private long _shutdownMilliseconds = -1L;

	private Coroutine _shutdownTimer;

	private long _shutdownTime;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = "Tools";
		xmlsTitleTxt.text = Language.GetString("ui_xmls");
		battlesTitleTxt.text = Language.GetString("ui_battles");
		dungeonsTitleTxt.text = Language.GetString("ui_dungeons");
		threadsTxt.text = Language.GetString("ui_threads");
		statisticsTxt.text = Language.GetString("ui_statistics");
		shutdownTxt.text = "Shutdown";
		battlesTxt.text = "";
		dungeonsTxt.text = "";
		shutdownCountTxt.text = "";
		threadPoolTxt.text = "";
		threadPoolBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_update");
		disableStatisticsBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_disable");
		enableStatisticsBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_enable");
		uploadBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_upload");
		clearBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_clear");
		disableBattlesBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_disable");
		enableBattlesBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_enable");
		disableDungeonsBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_disable");
		enableDungeonsBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_enable");
		checkInfoBtn.GetComponentInChildren<TextMeshProUGUI>().text = "i";
		shutdownStartBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_start");
		shutdownCancelBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_cancel");
		instanceCheckbox.GetComponentInChildren<TextMeshProUGUI>().text = "Global";
		instanceCheckbox.isOn = true;
		Debug.LogWarning("Check InputText Submit on mobile");
		threadPoolTxt.onSubmit.AddListener(DoUpdateThreadPool);
		ListenForBack(OnClose);
		DoUpdateInfo();
		CreateWindow();
	}

	private void DoUpdateInfo()
	{
		_shutdownMilliseconds = -1L;
		UpdateObjects();
		GameData.instance.main.ShowLoading();
		AdminDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(12), OnUpdateInfo);
		AdminDALC.instance.doInfoServer();
	}

	private void UpdateObjects()
	{
		if (_shutdownMilliseconds > 0)
		{
			StartShutdownTimer();
		}
		else
		{
			ClearShutdownTimer();
		}
	}

	private void StartShutdownTimer()
	{
		ClearShutdownTimer();
		_shutdownTime = (long)(Time.realtimeSinceStartup * 1000f);
		_shutdownTimer = GameData.instance.main.coroutineTimer.AddTimer(base.gameObject, 1000f, CoroutineTimer.TYPE.MILLISECONDS, 0, null, OnShutdownTimer);
		UpdateShutdown();
	}

	private void OnShutdownTimer()
	{
		long num = (long)(Time.realtimeSinceStartup * 1000f);
		long num2 = num - _shutdownTime;
		_shutdownTime = num;
		_shutdownMilliseconds -= num2;
		UpdateShutdown();
	}

	private void ClearShutdownTimer()
	{
		if (_shutdownTimer != null)
		{
			_shutdownTime = 0L;
			GameData.instance.main.coroutineTimer.StopTimer(ref _shutdownTimer);
			UpdateShutdown();
		}
	}

	private void UpdateShutdown()
	{
		if (_shutdownMilliseconds <= 0)
		{
			shutdownCountTxt.text = "-";
			shutdownCancelBtn.gameObject.SetActive(value: false);
			shutdownStartBtn.gameObject.SetActive(value: true);
		}
		else
		{
			shutdownCountTxt.text = Util.TimeFormat((int)(_shutdownMilliseconds / 1000));
			shutdownCancelBtn.gameObject.SetActive(value: true);
			shutdownStartBtn.gameObject.SetActive(value: false);
		}
	}

	private void OnUpdateInfo(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		AdminDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(12), OnUpdateInfo);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		bool @bool = sfsob.GetBool("bat26");
		bool bool2 = sfsob.GetBool("dun29");
		int @int = sfsob.GetInt("serv3");
		int int2 = sfsob.GetInt("serv4");
		bool bool3 = sfsob.GetBool("sta0");
		int int3 = sfsob.GetInt("serv2");
		long @long = sfsob.GetLong("serv10");
		enableStatisticsBtn.gameObject.SetActive(bool3);
		disableStatisticsBtn.gameObject.SetActive(!enableStatisticsBtn.gameObject.activeSelf);
		enableBattlesBtn.gameObject.SetActive(@bool);
		disableBattlesBtn.gameObject.SetActive(!enableBattlesBtn.gameObject.activeSelf);
		enableDungeonsBtn.gameObject.SetActive(bool2);
		disableDungeonsBtn.gameObject.SetActive(!enableDungeonsBtn.gameObject.activeSelf);
		_shutdownMilliseconds = @long;
		dungeonsTxt.text = int2.ToString();
		battlesTxt.text = @int.ToString();
		threadPoolTxt.text = int3.ToString();
		UpdateObjects();
	}

	public void OnDisableStatisticsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoDisableStatistics(disabled: true);
	}

	public void OnEnableStatisticsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoDisableStatistics(disabled: false);
	}

	public void OnThreadPoolBtn(string args)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoUpdateThreadPool();
	}

	public void OnUploadBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewAdminUploadWindow();
	}

	public void OnClearBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewClosablePromptMessageWindow("Clear XMLs", "Are you sure you want to clear all uploaded XMLs?", null, null, delegate
		{
			OnClearXMLConfirm();
		});
	}

	public void OnDisableBattlesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoDisableBattles(disabled: true);
	}

	public void OnEnableBattlesBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoDisableBattles(disabled: false);
	}

	public void OnDisableDungeonsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoDisableDungeons(disabled: true);
	}

	public void OnEnableDungeonsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoDisableDungeons(disabled: false);
	}

	public void OnRefreshBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoUpdateInfo();
	}

	public void OnCheckInfoBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		AdminDALC.instance.doCheckInfo();
	}

	public void OnShutdownStartBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoShutdown(start: true);
	}

	public void OnShutdownCancelBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		DoShutdown(start: false);
	}

	private void DoShutdown(bool start)
	{
		GameData.instance.main.ShowLoading();
		AdminDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(30), OnShutdown);
		AdminDALC.instance.doShutdown(start, GetInstance());
	}

	private void OnShutdown(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		AdminDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(30), OnShutdown);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
		else
		{
			DoUpdateInfo();
		}
	}

	private void DoUpdateThreadPool(string args = null)
	{
		int num = int.Parse(threadPoolTxt.text);
		if (num > 0)
		{
			GameData.instance.main.ShowLoading();
			AdminDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(13), OnUpdateThreadPool);
			AdminDALC.instance.doUpdateThreadPool(num);
		}
	}

	private void OnUpdateThreadPool(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		AdminDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(13), OnUpdateThreadPool);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
		else
		{
			DoUpdateInfo();
		}
	}

	private void DoDisableStatistics(bool disabled)
	{
		GameData.instance.main.ShowLoading();
		AdminDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(10), OnDisableStatistics);
		AdminDALC.instance.doDisableStatistics(disabled, GetInstance());
	}

	private void OnDisableStatistics(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		AdminDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(10), OnDisableStatistics);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
		else
		{
			DoUpdateInfo();
		}
	}

	private void OnClearXMLConfirm()
	{
		DoClearXMLs();
	}

	private void DoClearXMLs()
	{
		GameData.instance.main.ShowLoading();
		AdminDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(3), OnClearXMLs);
		AdminDALC.instance.doClearXMLs();
	}

	private void OnClearXMLs(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		AdminDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(3), OnClearXMLs);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
	}

	private void DoDisableBattles(bool disabled)
	{
		GameData.instance.main.ShowLoading();
		AdminDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(5), OnDisableBattles);
		AdminDALC.instance.doDisableBattles(disabled, GetInstance());
	}

	private void OnDisableBattles(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		AdminDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(5), OnDisableBattles);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
		else
		{
			DoUpdateInfo();
		}
	}

	private void DoDisableDungeons(bool disabled)
	{
		GameData.instance.main.ShowLoading();
		AdminDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(6), OnDisableDungeons);
		AdminDALC.instance.doDisableDungeons(disabled, GetInstance());
	}

	private void OnDisableDungeons(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		AdminDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(6), OnDisableDungeons);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
		else
		{
			DoUpdateInfo();
		}
	}

	public override void DoDestroy()
	{
		threadPoolTxt.onSubmit.RemoveListener(DoUpdateThreadPool);
		ClearShutdownTimer();
		base.DoDestroy();
	}

	private bool GetInstance()
	{
		return !instanceCheckbox.isOn;
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		threadPoolBtn.interactable = true;
		disableStatisticsBtn.interactable = true;
		enableStatisticsBtn.interactable = true;
		uploadBtn.interactable = true;
		clearBtn.interactable = true;
		disableBattlesBtn.interactable = true;
		enableBattlesBtn.interactable = true;
		disableDungeonsBtn.interactable = true;
		enableDungeonsBtn.interactable = true;
		checkInfoBtn.interactable = true;
		shutdownStartBtn.interactable = true;
		shutdownCancelBtn.interactable = true;
		refreshBtn.interactable = true;
		threadPoolTxt.interactable = true;
		instanceCheckbox.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		threadPoolBtn.interactable = false;
		disableStatisticsBtn.interactable = false;
		enableStatisticsBtn.interactable = false;
		uploadBtn.interactable = false;
		clearBtn.interactable = false;
		disableBattlesBtn.interactable = false;
		enableBattlesBtn.interactable = false;
		disableDungeonsBtn.interactable = false;
		enableDungeonsBtn.interactable = false;
		checkInfoBtn.interactable = false;
		shutdownStartBtn.interactable = false;
		shutdownCancelBtn.interactable = false;
		refreshBtn.interactable = false;
		threadPoolTxt.interactable = false;
		instanceCheckbox.interactable = false;
	}
}
