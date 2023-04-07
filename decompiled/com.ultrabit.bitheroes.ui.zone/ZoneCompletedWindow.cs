using System.Collections;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.zone;
using com.ultrabit.bitheroes.ui.victory;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.zone;

public class ZoneCompletedWindow : WindowsMain
{
	private const float COMPLETED_DELAY = 3f;

	private const float ANIMATION_SKIP_TIME = 0.9f;

	private const string ANIMATION_NAME = "ZoneCompletedWindowOpen";

	public ZoneCompletedMap map;

	public VictoryAnimation victory;

	public TextMeshProUGUI zoneNameTxt;

	public TextMeshProUGUI nextZoneNameTxt;

	public TextMeshProUGUI nextZoneTitleTxt;

	public Image zoneBg;

	private ZoneRef _zoneRef;

	private bool _closing;

	private CanvasGroup _canvasGroup;

	private Animator _mapAnimator;

	public void LoadDetails(ZoneRef zoneRef)
	{
		_zoneRef = zoneRef;
		float delay = 0.1f;
		float duration = 0.5f;
		AddBG(0.9f);
		ListenForBack(OnClose);
		ListenForForward(OnClose);
		zoneNameTxt.text = _zoneRef.name;
		nextZoneTitleTxt.text = Language.GetString("ui_zone_completed_next_title");
		CreateAssets();
		map.ON_VICTORY_SHOW.AddListener(OnShieldAnimationShow);
		victory.ON_SHIELD_ANIMATION_END.AddListener(delegate
		{
			victory.ToggleAnimation(enabled: false);
		});
		map.ON_FIRST_ZONE_SHOW.AddListener(OnFirstZoneShow);
		map.ON_ZONE_CHANGE.AddListener(OnZoneChange);
		map.ON_SECOND_ZONE_SHOW.AddListener(OnSecondZoneShow);
		map.ON_ANIMATION_END.AddListener(OnAnimationEnd);
		_mapAnimator = map.GetComponent<Animator>();
		_canvasGroup = GetComponent<CanvasGroup>();
		_canvasGroup.interactable = true;
		_canvasGroup.blocksRaycasts = true;
		_canvasGroup.alpha = 0f;
		StartCoroutine(StartAnimation(delay, duration));
		CreateWindow(closeWord: true, Language.GetString("ui_done"), scroll: false);
	}

	private void CreateAssets()
	{
		zoneBg.GetComponent<Image>().overrideSprite = GameData.instance.main.assetLoader.GetSpriteAsset(AssetURL.ZONE, _zoneRef.asset);
	}

	private IEnumerator StartAnimation(float delay, float duration)
	{
		yield return new WaitForSeconds(delay);
		DoAlpha(1f, duration);
	}

	public void DoAlpha(float finalAlpha, float duration)
	{
		float currentAlpha = _canvasGroup.alpha;
		DOTween.To(() => currentAlpha, delegate(float x)
		{
			currentAlpha = x;
		}, finalAlpha, duration).SetEase(Ease.Linear).OnUpdate(delegate
		{
			_canvasGroup.alpha = currentAlpha;
		})
			.OnComplete(delegate
			{
				if (finalAlpha == 0f)
				{
					DoDestroy();
				}
			});
	}

	private void ToggleAnimation(bool enabled)
	{
		_mapAnimator.enabled = enabled;
	}

	private void OnShieldAnimationShow()
	{
		victory.ShieldAnimation(base.gameObject, animate: true, Language.GetString("ui_zone_completed"));
	}

	private void OnFirstZoneShow()
	{
		ToggleAnimation(enabled: false);
		StartCoroutine(WaitAndContinueAnimation(3f));
	}

	private IEnumerator WaitAndContinueAnimation(float time)
	{
		yield return new WaitForSeconds(time);
		ToggleAnimation(enabled: true);
	}

	private void OnZoneChange()
	{
		ZoneRef zoneRef = ZoneBook.Lookup(_zoneRef.id + 1);
		if (zoneRef != null)
		{
			zoneBg.GetComponent<Image>().overrideSprite = GameData.instance.main.assetLoader.GetSpriteAsset(AssetURL.ZONE, zoneRef.asset);
			nextZoneNameTxt.text = zoneRef.name;
		}
		else
		{
			ToggleAnimation(enabled: false);
			_mapAnimator.Play("ZoneCompletedWindowOpen", -1, 0.9f);
			ToggleAnimation(enabled: true);
		}
	}

	private void OnSecondZoneShow()
	{
		ToggleAnimation(enabled: false);
		Enable();
	}

	public override void OnClose()
	{
		if (!_closing)
		{
			_closing = true;
			ToggleAnimation(enabled: true);
		}
	}

	public void OnAnimationEnd()
	{
		base.OnClose();
		DoAlpha(0f, 0.5f);
	}

	public override void DoDestroy()
	{
		map.ON_VICTORY_SHOW.RemoveListener(OnShieldAnimationShow);
		victory.ON_SHIELD_ANIMATION_END.RemoveListener(delegate
		{
			victory.ToggleAnimation(enabled: false);
		});
		map.ON_FIRST_ZONE_SHOW.RemoveListener(OnFirstZoneShow);
		map.ON_ZONE_CHANGE.RemoveListener(OnZoneChange);
		map.ON_SECOND_ZONE_SHOW.RemoveListener(OnSecondZoneShow);
		map.ON_ANIMATION_END.RemoveListener(OnAnimationEnd);
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
	}
}
