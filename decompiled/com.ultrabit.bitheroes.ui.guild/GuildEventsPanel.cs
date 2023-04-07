using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.gvg;
using com.ultrabit.bitheroes.model.language;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.guild;

public class GuildEventsPanel : MonoBehaviour
{
	public TextMeshProUGUI nameTxt;

	public TextMeshProUGUI eventTxt;

	public Button enterBtn;

	public TimeBarColor timeBar;

	private EventRef _eventRef;

	private GuildWindow _guildWindow;

	public void LoadDetails(GuildWindow guildWindow)
	{
		_guildWindow = guildWindow;
		nameTxt.text = EventRef.getEventTypeName(4);
		enterBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_enter");
		UpdateEvent();
	}

	public void UpdateEvent()
	{
		_eventRef = GvGEventBook.GetCurrentEventRef();
		timeBar.CancelInvoke("UpdateSeconds");
		timeBar.SetMaxValueSeconds(0f);
		timeBar.SetCurrentValueMilliseconds(0L);
		timeBar.COMPLETE.RemoveListener(OnEventTimerComplete);
		string @string = Language.GetString("event_blank");
		long num = 0L;
		if (_eventRef == null)
		{
			EventRef nextEventRef = GvGEventBook.GetNextEventRef();
			if (nextEventRef != null)
			{
				num = nextEventRef.GetDateRef().getMillisecondsUntilStart();
				@string = Language.GetString("event_start", new string[1] { nextEventRef.name });
			}
		}
		else
		{
			num = _eventRef.GetDateRef().getMillisecondsUntilEnd();
			@string = Language.GetString("event_end", new string[1] { _eventRef.name });
		}
		eventTxt.text = @string;
		if (num > 0)
		{
			if (_eventRef != null)
			{
				timeBar.ForceStart();
				timeBar.SetMaxValueMilliseconds(_eventRef.GetDateRef().getMillisecondsDuration());
				timeBar.SetCurrentValueMilliseconds(_eventRef.GetDateRef().getMillisecondsUntilEnd());
				timeBar.COMPLETE.AddListener(OnEventTimerComplete);
			}
			else
			{
				timeBar.ForceStart(invokeSeconds: true, showColorBar: false);
				timeBar.SetMaxValueMilliseconds(num);
				timeBar.SetCurrentValueMilliseconds(num);
				timeBar.COMPLETE.AddListener(OnEventTimerComplete);
			}
		}
	}

	public void OnEventTimerComplete()
	{
		UpdateEvent();
	}

	public void OnEnterBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		if (GameData.instance.PROJECT.CheckGameRequirement(30))
		{
			GameData.instance.windowGenerator.NewGvGEventWindow(_guildWindow.gameObject);
		}
	}

	private void OnDestroy()
	{
		timeBar.COMPLETE.RemoveListener(OnEventTimerComplete);
	}

	public void Show()
	{
		nameTxt.gameObject.SetActive(value: true);
		eventTxt.gameObject.SetActive(value: true);
		timeBar.transform.parent.gameObject.SetActive(value: true);
		enterBtn.gameObject.SetActive(value: true);
	}

	public void Hide()
	{
		nameTxt.gameObject.SetActive(value: false);
		eventTxt.gameObject.SetActive(value: false);
		timeBar.transform.parent.gameObject.SetActive(value: false);
		enterBtn.gameObject.SetActive(value: false);
	}

	public void DoEnable()
	{
		enterBtn.interactable = true;
	}

	public void DoDisable()
	{
		enterBtn.interactable = false;
	}

	private void Update()
	{
	}
}
