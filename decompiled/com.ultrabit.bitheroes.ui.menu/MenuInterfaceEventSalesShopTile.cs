using System.Collections;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.eventsales;
using com.ultrabit.bitheroes.model.instance;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.instance;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.menu;

public class MenuInterfaceEventSalesShopTile : MainUIButton
{
	public GameObject notify;

	public GameObject shimmer;

	private IEnumerator _timer;

	private EventSalesShopEventRef _eventRef;

	private bool _visited;

	public EventSalesShopEventRef eventRef => _eventRef;

	public override void Create()
	{
		UpdateEvent();
	}

	private void SetEvent(EventSalesShopEventRef eventRef = null)
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
		if (_eventRef.hudSprite == string.Empty || _eventRef.hudSprite == null)
		{
			base.gameObject.SetActive(value: false);
			LoadDetails();
		}
		else
		{
			sprite = GameData.instance.main.assetLoader.GetSpriteAsset(AssetURL.CUSTOM_UI, _eventRef.hudSprite);
			LoadDetails(Language.GetString(eventRef.hudLabel));
		}
		GetComponent<Image>().sprite = sprite;
		shimmer.GetComponent<Image>().sprite = sprite;
		if (base.gameObject.activeSelf)
		{
			StartTimer(_eventRef.GetDateRef().getMillisecondsUntilEnd());
		}
	}

	private void UpdateEvent()
	{
		UpdateNotification();
		EventSalesShopEventRef currentEvent = EventSalesShopBook.GetCurrentEvent();
		if (currentEvent != null)
		{
			SetEvent(currentEvent);
		}
		else
		{
			SetEvent();
		}
	}

	public void UpdateNotification()
	{
		_visited = DialogBook.Lookup(VariableBook.eventSalesShopDialog)?.seen ?? true;
		shimmer.SetActive(!_visited);
		notify.SetActive(!_visited);
	}

	public long GetNextEventMilliseconds()
	{
		long num = 0L;
		EventSalesShopEventRef currentEvent = EventSalesShopBook.GetCurrentEvent();
		if (currentEvent != null)
		{
			long millisecondsUntilStart = currentEvent.GetDateRef().getMillisecondsUntilStart();
			if (num <= 0 || millisecondsUntilStart < num)
			{
				num = millisecondsUntilStart;
			}
		}
		return num;
	}

	public override void DoClick()
	{
		base.DoClick();
		if (_visited)
		{
			GameData.instance.PROJECT.ShowEventSalesShop("HUD");
			return;
		}
		InstanceObjectRef instanceObjectRef = InstanceBook.GetFirstInstanceByType(1)?.GetFirstObjectByType(39);
		if (instanceObjectRef != null && !instanceObjectRef.getActive())
		{
			GameData.instance.PROJECT.ShowEventSalesShop("HUD");
			UpdateNotification();
		}
		else if (GameData.instance.PROJECT.instance.instanceRef.type == 1)
		{
			WalkToEventShop();
		}
		else
		{
			EnterTownAndWalkToEventShop();
		}
	}

	private void WalkToEventShop()
	{
		if (!(GameData.instance.PROJECT.instance == null) && !GameData.instance.PROJECT.instance.MoveToObjectRefType(39, random: false, execute: false, closest: true, executeIfThere: true))
		{
			GameData.instance.PROJECT.ShowEventSalesShop("Event_Character");
			UpdateNotification();
		}
	}

	private void EnterTownAndWalkToEventShop()
	{
		if (!(GameData.instance.PROJECT.instance == null))
		{
			GameData.instance.PROJECT.DoEnterInstance(InstanceBook.GetFirstInstanceByType(1), transition: false);
			Instance.SetAutoTravelType(39);
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
