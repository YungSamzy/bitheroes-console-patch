using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.extensions;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.instance;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.ui.instance;
using com.ultrabit.bitheroes.ui.tutorial;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.fishing;

public class FishingWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI timeTxt;

	public TextMeshProUGUI timeDescTxt;

	public Button playBtn;

	public Button eventBtn;

	public Button shopBtn;

	public Button travelBtn;

	private TimeBarColor timeBar;

	public bool fishing
	{
		get
		{
			if (GameData.instance.PROJECT.instance != null)
			{
				return GameData.instance.PROJECT.instance.instanceRef.type == 3;
			}
			return false;
		}
	}

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = Language.GetString("ui_fishing");
		timeDescTxt.text = Language.GetString("daily_fishing_new_desc");
		playBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_play");
		eventBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_events");
		shopBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_shop");
		travelBtn.GetComponentInChildren<TextMeshProUGUI>().text = (fishing ? Language.GetString("ui_exit") : Language.GetString("ui_enter"));
		timeBar = GetComponentInChildren<TimeBarColor>();
		long millisecondsTillDayEnds = ServerExtension.instance.GetMillisecondsTillDayEnds();
		timeBar.SetMaxValueMilliseconds(86400000L);
		timeBar.SetCurrentValueMilliseconds(millisecondsTillDayEnds);
		timeBar.COMPLETE.AddListener(OnTimer);
		SCROLL_IN_COMPLETE.AddListener(OnScrollInComplete);
		ListenForBack(OnClose);
		ListenForForward(OnPlayBtn);
		CreateWindow();
	}

	private void OnTimer()
	{
		if (!GameData.instance.tutorialManager.hasPopup && !base.scrollingIn && !base.scrollingOut)
		{
			GameData.instance.PROJECT.DoDailyFishingRewardCheck();
		}
	}

	private void OnScrollInComplete(object e)
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		CheckTutorial();
	}

	public bool CheckTutorial(object e = null)
	{
		if (GameData.instance.tutorialManager == null)
		{
			return false;
		}
		if (GameData.instance.tutorialManager.hasPopup || GameData.instance.tutorialManager.canvas == null)
		{
			return false;
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(57))
		{
			GameData.instance.PROJECT.character.tutorial.SetState(57);
			GameData.instance.PROJECT.CheckTutorialChanges();
		}
		if (GameData.instance.PROJECT.DoDailyFishingRewardCheck())
		{
			return true;
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(58))
		{
			GameData.instance.PROJECT.character.tutorial.SetState(58);
			GameData.instance.tutorialManager.ShowTutorialForEventTrigger(timeBar.transform.parent.gameObject, new TutorialPopUpSettings(Tutorial.GetText(58), 3, timeBar.transform.parent.gameObject, 0f, indicator: false, button: true, glow: false), null, stageTrigger: false, null, funcSameAsTargetFunc: false, delegate(object z)
			{
				CheckTutorial(z);
			}, shadow: true, tween: true);
			GameData.instance.PROJECT.CheckTutorialChanges();
			return true;
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(59))
		{
			GameData.instance.PROJECT.character.tutorial.SetState(59);
			GameData.instance.tutorialManager.ShowTutorialForButton(playBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(59), 4, playBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: true, null, shadow: true, tween: true);
			GameData.instance.PROJECT.CheckTutorialChanges();
			return true;
		}
		return false;
	}

	private void CheckInstanceObject()
	{
		if (GameData.instance.PROJECT.instance != null && GameData.instance.PROJECT.instance.MoveToObjectRefType(26, random: false, execute: false, closest: true, executeIfThere: true))
		{
			base.OnClose();
		}
	}

	private void CheckInstance(bool travel = false)
	{
		if (!(GameData.instance.PROJECT.instance != null))
		{
			return;
		}
		if (GameData.instance.PROJECT.instance.instanceRef.type == 3)
		{
			GameData.instance.PROJECT.DoEnterInstance(InstanceBook.GetFirstInstanceByType(1), transition: false);
		}
		else
		{
			GameData.instance.PROJECT.DoEnterInstance(InstanceBook.GetFirstInstanceByType(3), transition: false);
			if (travel)
			{
				Instance.SetAutoTravelType(26);
			}
		}
		base.OnClose();
	}

	public void OnPlayBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (fishing)
		{
			CheckInstanceObject();
		}
		else
		{
			CheckInstance(travel: true);
		}
	}

	public void OnEventBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewFishingEventWindow(base.gameObject);
	}

	public void OnShopBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.PROJECT.ShowFishingShop(base.gameObject);
	}

	public void OnTravelBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		CheckInstance();
	}

	public override void DoDestroy()
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		playBtn.interactable = true;
		eventBtn.interactable = true;
		shopBtn.interactable = true;
		travelBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		playBtn.interactable = false;
		eventBtn.interactable = false;
		shopBtn.interactable = false;
		travelBtn.interactable = false;
	}
}
