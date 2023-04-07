using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.zone;

public class ZoneCompletedMap : MonoBehaviour
{
	public UnityEvent ON_VICTORY_SHOW;

	public UnityEvent ON_FIRST_ZONE_SHOW;

	public UnityEvent ON_ZONE_CHANGE;

	public UnityEvent ON_SECOND_ZONE_SHOW;

	public UnityEvent ON_ANIMATION_END;

	private void OnVictoryShow()
	{
		ON_VICTORY_SHOW?.Invoke();
	}

	private void OnFirstZoneShow()
	{
		ON_FIRST_ZONE_SHOW?.Invoke();
	}

	private void OnZoneChange()
	{
		ON_ZONE_CHANGE?.Invoke();
	}

	private void OnSecondZoneShow()
	{
		ON_SECOND_ZONE_SHOW?.Invoke();
	}

	private void OnAnimationEnd()
	{
		ON_ANIMATION_END?.Invoke();
	}
}
