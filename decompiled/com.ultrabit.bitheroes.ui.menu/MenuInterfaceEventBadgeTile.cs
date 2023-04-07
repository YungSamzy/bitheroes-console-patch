using System.Collections;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.gvg;
using com.ultrabit.bitheroes.model.invasion;
using com.ultrabit.bitheroes.model.variable;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.menu;

public class MenuInterfaceEventBadgeTile : MainUIButton
{
	public Sprite gvgIcon;

	public Sprite invasionIcon;

	public Sprite gveIcon;

	private IEnumerator _timer;

	private EventRef _eventRef;

	public EventRef eventRef => _eventRef;

	public override void Create()
	{
		UpdateEvent();
	}

	private void SetEvent(EventRef eventRef = null)
	{
		_eventRef = eventRef;
		if (_eventRef == null)
		{
			base.gameObject.SetActive(value: false);
			StartTimer(GetNextEventMilliseconds());
			return;
		}
		base.gameObject.SetActive(value: true);
		Sprite sprite = null;
		switch (_eventRef.eventType)
		{
		case 4:
			sprite = gvgIcon;
			LoadDetails(EventRef.getEventTypeNameShort(_eventRef.eventType), VariableBook.GetGameRequirement(30));
			break;
		case 5:
			sprite = invasionIcon;
			LoadDetails(EventRef.getEventTypeNameShort(_eventRef.eventType), VariableBook.GetGameRequirement(33));
			break;
		case 7:
			sprite = gveIcon;
			LoadDetails(EventRef.getEventTypeNameShort(_eventRef.eventType), VariableBook.GetGameRequirement(35));
			break;
		default:
			LoadDetails();
			break;
		}
		GetComponent<Image>().sprite = sprite;
		if (base.gameObject.activeSelf)
		{
			StartTimer(_eventRef.GetDateRef().getMillisecondsUntilEnd());
		}
	}

	private void UpdateEvent()
	{
		EventRef currentEventRef = GvEEventBook.GetCurrentEventRef();
		if (currentEventRef != null)
		{
			SetEvent(currentEventRef);
			return;
		}
		EventRef currentEventRef2 = InvasionEventBook.GetCurrentEventRef();
		if (currentEventRef2 != null)
		{
			SetEvent(currentEventRef2);
			return;
		}
		EventRef currentEventRef3 = GvGEventBook.GetCurrentEventRef();
		if (currentEventRef3 != null)
		{
			SetEvent(currentEventRef3);
		}
		else
		{
			SetEvent();
		}
	}

	public long GetNextEventMilliseconds()
	{
		long num = 0L;
		EventRef currentEventRef = GvEEventBook.GetCurrentEventRef();
		if (currentEventRef != null)
		{
			long millisecondsUntilStart = currentEventRef.GetDateRef().getMillisecondsUntilStart();
			if (num <= 0 || millisecondsUntilStart < num)
			{
				num = millisecondsUntilStart;
			}
		}
		EventRef currentEventRef2 = InvasionEventBook.GetCurrentEventRef();
		if (currentEventRef2 != null)
		{
			long millisecondsUntilStart2 = currentEventRef2.GetDateRef().getMillisecondsUntilStart();
			if (num <= 0 || millisecondsUntilStart2 < num)
			{
				num = millisecondsUntilStart2;
			}
		}
		EventRef currentEventRef3 = GvGEventBook.GetCurrentEventRef();
		if (currentEventRef3 != null)
		{
			long millisecondsUntilStart3 = currentEventRef3.GetDateRef().getMillisecondsUntilStart();
			if (num <= 0 || millisecondsUntilStart3 < num)
			{
				num = millisecondsUntilStart3;
			}
		}
		return num;
	}

	public override void DoClick()
	{
		base.DoClick();
		if (_eventRef != null)
		{
			switch (_eventRef.eventType)
			{
			case 4:
				Debug.Log("CLICKED GVG");
				GameData.instance.PROJECT.ShowGvGWindow();
				break;
			case 5:
				Debug.Log("CLICKED INVASION");
				GameData.instance.PROJECT.ShowInvasionWindow();
				break;
			case 7:
				Debug.Log("CLICKED GVE");
				GameData.instance.PROJECT.ShowGvEWindow();
				break;
			case 6:
				break;
			}
		}
	}

	private void ClearTimer()
	{
		if (_timer != null)
		{
			StopCoroutine(_timer);
			_timer = null;
		}
	}

	private void StartTimer(long milliseconds = 0L)
	{
		ClearTimer();
		if (milliseconds > 0)
		{
			_timer = Timer(milliseconds / 1000);
			StartCoroutine(_timer);
		}
	}

	private IEnumerator Timer(long seconds = 0L)
	{
		yield return new WaitForSeconds(seconds);
		ClearTimer();
		UpdateEvent();
	}
}
