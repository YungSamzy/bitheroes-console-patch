using System.Collections;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.gauntlet;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.menu;

public class MenuInterfaceEventTokenTile : MainUIButton
{
	public Sprite riftIcon;

	public Sprite gauntletIcon;

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
		switch (_eventRef.eventType)
		{
		case 2:
			GetComponent<Image>().sprite = riftIcon;
			LoadDetails(EventRef.getEventTypeNameShort(_eventRef.eventType), VariableBook.GetGameRequirement(21));
			break;
		case 3:
			GetComponent<Image>().sprite = gauntletIcon;
			LoadDetails(EventRef.getEventTypeNameShort(_eventRef.eventType), VariableBook.GetGameRequirement(23));
			break;
		default:
			LoadDetails();
			break;
		}
		if (base.gameObject.activeSelf)
		{
			StartTimer(_eventRef.GetDateRef().getMillisecondsUntilEnd());
		}
	}

	public void UpdateEvent()
	{
		EventRef currentEventRef = GauntletEventBook.GetCurrentEventRef();
		if (currentEventRef != null)
		{
			SetEvent(currentEventRef);
			return;
		}
		EventRef currentEventRef2 = RiftEventBook.GetCurrentEventRef();
		if (currentEventRef2 != null)
		{
			SetEvent(currentEventRef2);
		}
		else
		{
			SetEvent();
		}
	}

	public long GetNextEventMilliseconds()
	{
		long num = 0L;
		EventRef currentEventRef = GauntletEventBook.GetCurrentEventRef();
		if (currentEventRef != null)
		{
			long millisecondsUntilStart = currentEventRef.GetDateRef().getMillisecondsUntilStart();
			if (num <= 0 || millisecondsUntilStart < num)
			{
				num = millisecondsUntilStart;
			}
		}
		EventRef currentEventRef2 = RiftEventBook.GetCurrentEventRef();
		if (currentEventRef2 != null)
		{
			long millisecondsUntilStart2 = currentEventRef2.GetDateRef().getMillisecondsUntilStart();
			if (num <= 0 || millisecondsUntilStart2 < num)
			{
				num = millisecondsUntilStart2;
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
			case 2:
				GameData.instance.PROJECT.ShowRiftWindow();
				break;
			case 3:
				GameData.instance.PROJECT.ShowGauntletWindow();
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
