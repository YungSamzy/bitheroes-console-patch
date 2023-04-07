using System.Collections;
using com.ultrabit.bitheroes.core;
using UnityEngine;
using UnityEngine.Rendering;

namespace com.ultrabit.bitheroes.ui.battle;

public class BattleEntityOverlay : MonoBehaviour
{
	private const float SMALLEST_PERCEIVABLE_SCALE = 0.2f;

	public Transform meterOverlay;

	private Vector3 meterOverlayLocalScale;

	public Transform overMeterOverlay;

	private Vector3 overMeterOverlayLocalScale;

	public Transform shieldBar;

	private Vector3 shieldBarLocalScale;

	public Transform healthBar;

	private Vector3 healthBarLocalScale;

	public Transform colddownBar;

	private Vector3 colddownBarLocalScale;

	private Animator healthBarAnimator;

	private void Awake()
	{
		meterOverlayLocalScale = meterOverlay.localScale;
		colddownBarLocalScale = colddownBar.localScale;
		overMeterOverlayLocalScale = overMeterOverlay.localScale;
		overMeterOverlay.localScale = Vector3.zero;
		shieldBarLocalScale = shieldBar.localScale;
		shieldBar.localScale = Vector3.zero;
		healthBarLocalScale = healthBar.localScale;
		healthBar.localScale = Vector3.one;
	}

	public void SetMeterPerc(float perc)
	{
		float perc2 = ((perc <= 1f) ? perc : 1f);
		float perc3 = ((perc > 1f) ? (perc - 1f) : 0f);
		SetScale(perc2, meterOverlay, meterOverlayLocalScale);
		SetScale(perc3, overMeterOverlay, overMeterOverlayLocalScale);
	}

	public void SetHealthPerc(float perc)
	{
		SetScale(perc, healthBar, healthBarLocalScale);
		AnimateHealthBar();
	}

	public void SetShieldPerc(float perc, float maxPerc = 0.5f)
	{
		SetScale(perc, shieldBar, shieldBarLocalScale, maxPerc);
	}

	public void SetCooldownPerc(float perc)
	{
		SetScale(perc, colddownBar, colddownBarLocalScale);
	}

	private void SetScale(float perc, Transform obj, Vector3 overlay, float maxPerc = 1f)
	{
		perc = Mathf.Clamp01(perc);
		Vector3 localScale = overlay;
		float num = localScale.x * perc * maxPerc;
		bool flag = 0f == num || num > 0.2f;
		localScale.x = (flag ? num : 0.2f);
		obj.localScale = localScale;
	}

	public void AnimateHealthBar()
	{
		if (!(healthBarAnimator == null))
		{
			healthBarAnimator.speed = 0f;
			healthBarAnimator.Play("HealthBarSprite", 0, 1f - healthBar.localScale.x / healthBarLocalScale.x);
		}
	}

	public void Add()
	{
		SortingGroup sortingGroup = GetComponent<SortingGroup>();
		if (sortingGroup == null)
		{
			sortingGroup = base.gameObject.AddComponent<SortingGroup>();
		}
		if (GameData.instance.SAVE_STATE.battleBarOverlay)
		{
			sortingGroup.sortingLayerID = SortingLayer.NameToID("Overall");
		}
		else
		{
			sortingGroup.sortingLayerID = SortingLayer.NameToID("Default");
		}
		SortingGroup componentInParent = base.transform.parent.GetComponentInParent<SortingGroup>();
		if (componentInParent != null && componentInParent.enabled)
		{
			sortingGroup.sortingOrder = componentInParent.sortingOrder;
		}
	}

	private IEnumerator WaitToFixHealth(float perc)
	{
		yield return new WaitForSeconds(0.1f);
		healthBar.GetComponent<Animator>().Play("HealthBarSprite", 0, Mathf.Round(100f - perc * 100f) * 0.01f);
	}

	private void OnEnable()
	{
		healthBarAnimator = healthBar.GetComponent<Animator>();
		AnimateHealthBar();
	}
}
