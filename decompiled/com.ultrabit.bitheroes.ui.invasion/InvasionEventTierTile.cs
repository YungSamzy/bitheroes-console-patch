using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.invasion;
using com.ultrabit.bitheroes.model.utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.invasion;

public class InvasionEventTierTile : MonoBehaviour
{
	public Image notch;

	public TextMeshProUGUI tierTxt;

	private EventRef _eventRef;

	private InvasionEventTierRef _tierRef;

	private InvasionEventLevelRef _levelRef;

	public void LoadDetails(EventRef eventRef, InvasionEventTierRef tierRef, InvasionEventLevelRef levelRef, bool current)
	{
		_eventRef = eventRef;
		_tierRef = tierRef;
		if (!eventRef.hasSegmentedRewards)
		{
			InvasionEventTierRef firstTierRef = ((InvasionEventRef)eventRef).GetFirstTierRef();
			InvasionEventTierRef lastTierRef = ((InvasionEventRef)eventRef).GetLastTierRef();
			if (_tierRef == firstTierRef || _tierRef == lastTierRef)
			{
				notch.gameObject.SetActive(value: false);
			}
		}
		else
		{
			InvasionEventLevelRef firstLevelRef = ((InvasionEventRef)eventRef).GetFirstLevelRef();
			InvasionEventLevelRef lastLevelRef = ((InvasionEventRef)eventRef).GetLastLevelRef();
			if (_levelRef == firstLevelRef || _levelRef == lastLevelRef)
			{
				notch.gameObject.SetActive(value: false);
			}
		}
		Color color;
		if (current)
		{
			ColorUtility.TryParseHtmlString("#00FF00", out color);
		}
		else
		{
			ColorUtility.TryParseHtmlString("#999999", out color);
		}
		tierTxt.color = color;
		tierTxt.text = Util.NumberFormat(tierRef?.id ?? (levelRef.id - 1));
	}
}
