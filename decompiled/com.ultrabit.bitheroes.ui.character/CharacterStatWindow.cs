using System.Collections;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.tutorial;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.character;

public class CharacterStatWindow : WindowsMain
{
	private const int REPEAT_DELAY_MAX = 150;

	private const int REPEAT_DELAY_MIN = 5;

	private const int REPEAT_DELAY_CHANGE = 5;

	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI pointsTxt;

	public TextMeshProUGUI pointsNameTxt;

	public TextMeshProUGUI powerTxt;

	public TextMeshProUGUI powerBaseTxt;

	public TextMeshProUGUI staminaTxt;

	public TextMeshProUGUI staminaBaseTxt;

	public TextMeshProUGUI agilityTxt;

	public TextMeshProUGUI agilityBaseTxt;

	public Image powerIcon;

	public Image staminaIcon;

	public Image agilityIcon;

	public Button powerAddBtn;

	public Button powerRemoveBtn;

	public Button staminaAddBtn;

	public Button staminaRemoveBtn;

	public Button agilityAddBtn;

	public Button agilityRemoveBtn;

	public Button resetBtn;

	public Button saveBtn;

	private int _power;

	private int _powerBonus;

	private int _stamina;

	private int _staminaBonus;

	private int _agility;

	private int _agilityBonus;

	private int _points;

	private int _pointsTotal;

	private Coroutine _repeatTimer;

	private bool _repeatIncreased;

	private bool _repeatAdded;

	private int _repeatStat;

	private int _repeatDelay = 150;

	private Color alpha = new Color(1f, 1f, 1f, 0.5f);

	public GameObject mouseLock;

	private GameObject pressed;

	private GameObject _parent;

	private string _callBack;

	public override void Start()
	{
		base.Start();
		Disable();
		_pointsTotal = GameData.instance.PROJECT.character.points;
		topperTxt.text = Language.GetString("ui_stats");
		pointsNameTxt.text = Language.GetString("ui_points");
		powerAddBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_plus");
		powerRemoveBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_minus");
		staminaAddBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_plus");
		staminaRemoveBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_minus");
		agilityAddBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_plus");
		agilityRemoveBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_minus");
		saveBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_save");
		mouseLock.transform.SetAsFirstSibling();
		GameData.instance.PROJECT.character.AddListener("STATS_CHANGE", OnStatsChange);
		OnStatsChange();
		SCROLL_IN_COMPLETE.AddListener(OnScrollInComplete);
		ListenForBack(OnClose);
		CreateWindow();
	}

	private void OnScrollInComplete(object e)
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		CheckTutorial();
	}

	private void CheckTutorial(object e = null)
	{
		if (!(GameData.instance.tutorialManager == null) && !GameData.instance.tutorialManager.hasPopup && !(GameData.instance.tutorialManager.canvas == null))
		{
			if (!GameData.instance.PROJECT.character.tutorial.GetState(15))
			{
				GameData.instance.PROJECT.character.tutorial.SetState(15);
				GameData.instance.tutorialManager.ShowTutorialForEventTrigger(powerIcon.gameObject, new TutorialPopUpSettings(Tutorial.GetText(15), 3, powerIcon.gameObject, 0f, indicator: false, button: true, glow: false), null, stageTrigger: false, null, funcSameAsTargetFunc: false, CheckTutorial, shadow: true, tween: true);
			}
			else if (!GameData.instance.PROJECT.character.tutorial.GetState(16))
			{
				GameData.instance.PROJECT.character.tutorial.SetState(16);
				GameData.instance.tutorialManager.ShowTutorialForEventTrigger(staminaIcon.gameObject, new TutorialPopUpSettings(Tutorial.GetText(16), 3, staminaIcon.gameObject, 0f, indicator: false, button: true, glow: false), null, stageTrigger: false, null, funcSameAsTargetFunc: false, CheckTutorial, shadow: true, tween: true);
			}
			else if (!GameData.instance.PROJECT.character.tutorial.GetState(17))
			{
				GameData.instance.PROJECT.character.tutorial.SetState(17);
				GameData.instance.tutorialManager.ShowTutorialForEventTrigger(agilityIcon.gameObject, new TutorialPopUpSettings(Tutorial.GetText(17), 3, agilityIcon.gameObject, 0f, indicator: false, button: true, glow: false), null, stageTrigger: false, null, funcSameAsTargetFunc: false, CheckTutorial, shadow: true, tween: true);
			}
			else
			{
				GameData.instance.PROJECT.CheckTutorialChanges();
			}
		}
	}

	public void SetParentCall(GameObject parent, string callBack)
	{
		_parent = parent;
		_callBack = callBack;
	}

	public void OnButtonUp()
	{
		ClearRepeatTimer();
		mouseLock.transform.SetAsFirstSibling();
	}

	public void OnPowerAddBtn()
	{
		mouseLock.transform.SetAsLastSibling();
		powerAddBtn.transform.SetAsLastSibling();
		pressed = powerAddBtn.gameObject;
		StartRepeatTimer(CheckIncreased(), added: true, 0);
	}

	public void OnPowerRemoveBtn()
	{
		mouseLock.transform.SetAsLastSibling();
		powerRemoveBtn.transform.SetAsLastSibling();
		pressed = powerRemoveBtn.gameObject;
		StartRepeatTimer(CheckIncreased(), added: false, 0);
	}

	public void OnStaminaAddBtn()
	{
		mouseLock.transform.SetAsLastSibling();
		staminaAddBtn.transform.SetAsLastSibling();
		pressed = staminaAddBtn.gameObject;
		StartRepeatTimer(CheckIncreased(), added: true, 1);
	}

	public void OnStaminaRemoveBtn()
	{
		mouseLock.transform.SetAsLastSibling();
		staminaRemoveBtn.transform.SetAsLastSibling();
		pressed = staminaRemoveBtn.gameObject;
		StartRepeatTimer(CheckIncreased(), added: false, 1);
	}

	public void OnAgilityAddBtn()
	{
		mouseLock.transform.SetAsLastSibling();
		agilityAddBtn.transform.SetAsLastSibling();
		pressed = agilityAddBtn.gameObject;
		StartRepeatTimer(CheckIncreased(), added: true, 2);
	}

	public void OnAgilityRemoveBtn()
	{
		mouseLock.transform.SetAsLastSibling();
		agilityRemoveBtn.transform.SetAsLastSibling();
		pressed = agilityRemoveBtn.gameObject;
		StartRepeatTimer(CheckIncreased(), added: false, 2);
	}

	public void OnResetBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		base.OnClose();
		GameData.instance.windowGenerator.NewServiceWindow(4, ServiceBook.GetFirstServiceByType(4));
	}

	public void OnSaveBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		Disable();
		DoSaveStats();
	}

	public bool CheckIncreased()
	{
		if (Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl))
		{
			return true;
		}
		if (Input.GetKeyDown(KeyCode.RightCommand) || Input.GetKeyDown(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand) || Input.GetKey(KeyCode.LeftCommand))
		{
			return true;
		}
		return false;
	}

	private void OnStatsChange()
	{
		_power = GameData.instance.PROJECT.character.power;
		_powerBonus = GameData.instance.PROJECT.character.getTotalPower() - _power;
		_stamina = GameData.instance.PROJECT.character.stamina;
		_staminaBonus = GameData.instance.PROJECT.character.getTotalStamina() - _stamina;
		_agility = GameData.instance.PROJECT.character.agility;
		_agilityBonus = GameData.instance.PROJECT.character.getTotalAgility() - _agility;
		_points = GameData.instance.PROJECT.character.points;
		DoUpdate();
	}

	public void DoUpdate()
	{
		int num = _power + _powerBonus;
		int num2 = _stamina + _staminaBonus;
		int num3 = _agility + _agilityBonus;
		pointsTxt.text = Util.NumberFormat(_points);
		Color color2;
		if (_points > 0)
		{
			if (ColorUtility.TryParseHtmlString("#00FF00", out var color))
			{
				pointsTxt.color = color;
			}
		}
		else if (ColorUtility.TryParseHtmlString("#FFFFFF", out color2))
		{
			pointsTxt.color = color2;
		}
		powerTxt.text = Util.NumberFormat(num);
		powerBaseTxt.text = "+" + Util.NumberFormat(_power);
		Color color4;
		if (_power > 0)
		{
			if (ColorUtility.TryParseHtmlString("#00FF00", out var color3))
			{
				powerBaseTxt.color = color3;
			}
		}
		else if (ColorUtility.TryParseHtmlString("#FFFFFF", out color4))
		{
			powerBaseTxt.color = color4;
		}
		staminaTxt.text = Util.NumberFormat(num2);
		staminaBaseTxt.text = "+" + Util.NumberFormat(_stamina);
		Color color6;
		if (_stamina > 0)
		{
			if (ColorUtility.TryParseHtmlString("#00FF00", out var color5))
			{
				staminaBaseTxt.color = color5;
			}
		}
		else if (ColorUtility.TryParseHtmlString("#FFFFFF", out color6))
		{
			staminaBaseTxt.color = color6;
		}
		agilityTxt.text = Util.NumberFormat(num3);
		agilityBaseTxt.text = "+" + Util.NumberFormat(_agility);
		Color color8;
		if (_agility > 0)
		{
			if (ColorUtility.TryParseHtmlString("#00FF00", out var color7))
			{
				agilityBaseTxt.color = color7;
			}
		}
		else if (ColorUtility.TryParseHtmlString("#FFFFFF", out color8))
		{
			agilityBaseTxt.color = color8;
		}
		if (_points > 0)
		{
			AlphaBtn(powerAddBtn, active: true);
			AlphaBtn(staminaAddBtn, active: true);
			AlphaBtn(agilityAddBtn, active: true);
		}
		else
		{
			AlphaBtn(powerAddBtn, active: false);
			AlphaBtn(staminaAddBtn, active: false);
			AlphaBtn(agilityAddBtn, active: false);
		}
		if (_power > GameData.instance.PROJECT.character.power)
		{
			AlphaBtn(powerRemoveBtn, active: true);
		}
		else
		{
			AlphaBtn(powerRemoveBtn, active: false);
		}
		if (_stamina > GameData.instance.PROJECT.character.stamina)
		{
			AlphaBtn(staminaRemoveBtn, active: true);
		}
		else
		{
			AlphaBtn(staminaRemoveBtn, active: false);
		}
		if (_agility > GameData.instance.PROJECT.character.agility)
		{
			AlphaBtn(agilityRemoveBtn, active: true);
		}
		else
		{
			AlphaBtn(agilityRemoveBtn, active: false);
		}
		if (StatsHaveChanged())
		{
			AlphaBtn(saveBtn, active: true);
		}
		else
		{
			AlphaBtn(saveBtn, active: false);
		}
	}

	private void DoSaveStats()
	{
		GameData.instance.main.UpdateLoading(Language.GetString("ui_saving"));
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(3), OnSaveStats);
		CharacterDALC.instance.doSaveStats(_power, _stamina, _agility, _points);
	}

	private void OnSaveStats(BaseEvent baseEvent)
	{
		if (!(baseEvent is DALCEvent dALCEvent))
		{
			return;
		}
		if (GameData.instance.main != null)
		{
			GameData.instance.main.UpdateLoading();
			GameData.instance.main.HideLoading();
		}
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(3), OnSaveStats);
		SFSObject sfsob = dALCEvent.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
		}
		else if (GameData.instance.PROJECT.character != null)
		{
			GameData.instance.PROJECT.character.points = sfsob.GetInt("cha19");
			GameData.instance.PROJECT.character.power = sfsob.GetInt("cha6");
			GameData.instance.PROJECT.character.stamina = sfsob.GetInt("cha7");
			GameData.instance.PROJECT.character.agility = sfsob.GetInt("cha8");
			if (GameData.instance.PROJECT.character.points <= 0)
			{
				StopAllCoroutines();
				base.OnClose();
			}
		}
		Enable();
	}

	private bool StatsHaveChanged()
	{
		if (GameData.instance.PROJECT.character.power >= _power && GameData.instance.PROJECT.character.stamina >= _stamina && GameData.instance.PROJECT.character.agility >= _agility)
		{
			return GameData.instance.PROJECT.character.points > _points;
		}
		return true;
	}

	private int GetAllowedPoints(bool increased, bool added, int stat)
	{
		int num = 1;
		if (increased)
		{
			num = 10;
		}
		if (added)
		{
			if (_points < num)
			{
				num = _points;
			}
		}
		else
		{
			int num2 = 0;
			switch (stat)
			{
			case 0:
				num2 = _power - GameData.instance.PROJECT.character.power;
				break;
			case 1:
				num2 = _stamina - GameData.instance.PROJECT.character.stamina;
				break;
			case 2:
				num2 = _agility - GameData.instance.PROJECT.character.agility;
				break;
			}
			if (num > num2)
			{
				num = num2;
			}
		}
		if (num <= 0)
		{
			num = 0;
		}
		return num;
	}

	private void DoStatChange(bool increased, bool added, int stat)
	{
		int num = GetAllowedPoints(increased, added, stat);
		if (num > 0)
		{
			if (!added)
			{
				num *= -1;
			}
			switch (stat)
			{
			case 0:
				_power += num;
				break;
			case 1:
				_stamina += num;
				break;
			case 2:
				_agility += num;
				break;
			}
			_points -= num;
			DoUpdate();
			if (added)
			{
				GameData.instance.audioManager.PlaySoundLink("upgradestat");
			}
			else
			{
				GameData.instance.audioManager.PlaySoundLink("buttonclick");
			}
		}
	}

	private void DoRepeatStatChange()
	{
		DoStatChange(_repeatIncreased, _repeatAdded, _repeatStat);
	}

	private void StartRepeatTimer(bool increased, bool added, int stat)
	{
		_repeatIncreased = increased;
		_repeatAdded = added;
		_repeatStat = stat;
		_repeatDelay = 150;
		DoRepeatStatChange();
		CreateRepeatTimer();
	}

	private void CreateRepeatTimer()
	{
		ClearRepeatTimer();
		_repeatTimer = StartCoroutine(OnRepeatTimer(_repeatDelay));
	}

	private void ClearRepeatTimer()
	{
		if (_repeatTimer != null)
		{
			StopCoroutine(_repeatTimer);
			_repeatTimer = null;
		}
	}

	private IEnumerator OnRepeatTimer(int delay)
	{
		yield return new WaitForSeconds((float)delay * 0.001f);
		_repeatDelay -= 5;
		if (_repeatDelay < 5)
		{
			_repeatDelay = 5;
		}
		StopCoroutine(_repeatTimer);
		DoRepeatStatChange();
		if (_repeatTimer != null)
		{
			CreateRepeatTimer();
		}
	}

	public void AlphaBtn(Button button, bool active)
	{
		if (active)
		{
			button.image.color = Color.white;
			button.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
			button.enabled = true;
			if (button.GetComponent<EventTrigger>() != null)
			{
				button.GetComponent<EventTrigger>().enabled = true;
			}
			return;
		}
		button.image.color = alpha;
		button.GetComponentInChildren<TextMeshProUGUI>().color = alpha;
		button.enabled = false;
		if (button.GetComponent<EventTrigger>() != null)
		{
			if (button.gameObject == pressed)
			{
				PointerEventData eventData = new PointerEventData(EventSystem.current);
				ExecuteEvents.Execute(button.gameObject, eventData, ExecuteEvents.pointerUpHandler);
				pressed = null;
			}
			button.GetComponent<EventTrigger>().enabled = false;
		}
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		powerAddBtn.interactable = true;
		powerRemoveBtn.interactable = true;
		staminaAddBtn.interactable = true;
		staminaRemoveBtn.interactable = true;
		agilityAddBtn.interactable = true;
		agilityRemoveBtn.interactable = true;
		resetBtn.interactable = true;
		saveBtn.interactable = true;
		powerAddBtn.GetComponent<EventTrigger>().enabled = true;
		powerRemoveBtn.GetComponent<EventTrigger>().enabled = true;
		staminaAddBtn.GetComponent<EventTrigger>().enabled = true;
		staminaRemoveBtn.GetComponent<EventTrigger>().enabled = true;
		agilityAddBtn.GetComponent<EventTrigger>().enabled = true;
		agilityRemoveBtn.GetComponent<EventTrigger>().enabled = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		powerAddBtn.interactable = false;
		powerRemoveBtn.interactable = false;
		staminaAddBtn.interactable = false;
		staminaRemoveBtn.interactable = false;
		agilityAddBtn.interactable = false;
		agilityRemoveBtn.interactable = false;
		resetBtn.interactable = false;
		saveBtn.interactable = false;
		powerAddBtn.GetComponent<EventTrigger>().enabled = false;
		powerRemoveBtn.GetComponent<EventTrigger>().enabled = false;
		staminaAddBtn.GetComponent<EventTrigger>().enabled = false;
		staminaRemoveBtn.GetComponent<EventTrigger>().enabled = false;
		agilityAddBtn.GetComponent<EventTrigger>().enabled = false;
		agilityRemoveBtn.GetComponent<EventTrigger>().enabled = false;
	}

	public override void OnClose()
	{
		if (StatsHaveChanged())
		{
			GameData.instance.windowGenerator.NewMessageWindow(DialogWindow.TYPE.TYPE_YES_NO, Language.GetString("ui_confirm"), Language.GetString("ui_stats_exit_confirm"), base.gameObject, "StatsExit");
			return;
		}
		StopAllCoroutines();
		base.OnClose();
	}

	public override void DoDestroy()
	{
		if (_parent != null)
		{
			_parent.BroadcastMessage(_callBack, SendMessageOptions.DontRequireReceiver);
		}
		GameData.instance.PROJECT.character.RemoveListener("STATS_CHANGE", OnStatsChange);
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		base.DoDestroy();
	}

	private void DialogYesRecieverStatsExit()
	{
		StopAllCoroutines();
		base.OnClose();
	}
}
