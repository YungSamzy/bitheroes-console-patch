using System.Collections;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.language;
using com.ultrabit.bitheroes.ui.tutorial;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.instance.fishing;

public class InstanceFishingCastingScreen : MonoBehaviour
{
	private const float GAIN = 0.25f;

	private const float GROWTH = 10f;

	public Button castBtn;

	public Image overlayBar;

	public Image colorBar;

	public Image barBg;

	public Image minBg;

	public TextMeshProUGUI minTxt;

	public Image maxBg;

	public TextMeshProUGUI maxTxt;

	public TextMeshProUGUI distanceTxt;

	private InstanceFishingInterface _fishingInterface;

	private bool _increase;

	private float _distance;

	private int _distanceMin;

	private int _distanceMax;

	private float _overlayScale;

	private float _colorScale;

	private bool _casting;

	private bool doUpdate;

	private AsianLanguageFontManager asianLangManager;

	public void LoadDetails(InstanceFishingInterface fishingInterface)
	{
		colorBar.GetComponent<Animator>().speed = 0f;
		_fishingInterface = fishingInterface;
		InstancePlayer player = _fishingInterface.GetPlayer();
		_distanceMin = player.GetFishingDistanceMin();
		_distanceMax = player.GetFishingDistanceMax();
		_distance = _distanceMin;
		_increase = _distance < (float)_distanceMax;
		_overlayScale = overlayBar.rectTransform.sizeDelta.x;
		_colorScale = colorBar.rectTransform.sizeDelta.x;
		minTxt.text = Util.NumberFormat(_distanceMin);
		maxTxt.text = Util.NumberFormat(_distanceMax);
		castBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_cast");
		StartUpdate();
		StartCoroutine(DelayTutorial());
		asianLangManager = base.gameObject.GetComponent<AsianLanguageFontManager>();
		if (asianLangManager == null)
		{
			asianLangManager = base.gameObject.AddComponent<AsianLanguageFontManager>();
		}
		if (asianLangManager != null)
		{
			asianLangManager.SetAsianFontsIfNeeded();
		}
	}

	private IEnumerator DelayTutorial()
	{
		yield return new WaitForSeconds(0.1f);
		CheckTutorial();
	}

	public void CheckTutorial()
	{
		if (!(GameData.instance.tutorialManager == null) && !GameData.instance.tutorialManager.hasPopup && !(GameData.instance.tutorialManager.canvas == null) && !_casting && !GameData.instance.PROJECT.character.tutorial.GetState(55) && castBtn.enabled && castBtn.interactable)
		{
			GameData.instance.PROJECT.character.tutorial.SetState(55);
			GameData.instance.tutorialManager.ShowTutorialForEventTrigger(castBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(55), 0, castBtn.gameObject, 0f, indicator: false, button: false, glow: true, 250, new Vector2(0f, 180f)), EventTriggerType.PointerClick, stageTrigger: true, null, funcSameAsTargetFunc: false, OnCastEventTrigger, shadow: false, tween: true);
			GameData.instance.PROJECT.CheckTutorialChanges();
		}
	}

	private void OnCastEventTrigger(object arg0)
	{
		OnCastBtn();
	}

	public void OnForward()
	{
		if (!GameData.instance.tutorialManager.hasPopup && castBtn.interactable && castBtn.enabled)
		{
			DoCast();
		}
	}

	private void UpdateBar()
	{
		float perc = GetPerc();
		overlayBar.rectTransform.sizeDelta = new Vector2(perc * _overlayScale, overlayBar.rectTransform.sizeDelta.y);
		colorBar.rectTransform.sizeDelta = new Vector2(perc * _colorScale, colorBar.rectTransform.sizeDelta.y);
		if (_distance >= (float)_distanceMax)
		{
			overlayBar.rectTransform.sizeDelta = new Vector2(_overlayScale, overlayBar.rectTransform.sizeDelta.y);
			colorBar.rectTransform.sizeDelta = new Vector2(_colorScale, colorBar.rectTransform.sizeDelta.y);
		}
		colorBar.GetComponent<Animator>().Play("FishingBarCastingColors", 0, Mathf.Round(perc * 100f) * 0.01f);
		colorBar.GetComponent<Animator>().speed = 0f;
		distanceTxt.text = Util.NumberFormat(GetDistance());
	}

	private void StartUpdate()
	{
		UpdateBar();
		GameData.instance.main.DISPATCHER.FRAME_UPDATE.AddListener(OnUpdate);
	}

	private float GetMult()
	{
		return (float)_distanceMax / 100f;
	}

	private float GetGain()
	{
		return 0.25f * GetMult();
	}

	private float GetGrowth()
	{
		return 10f * GetMult();
	}

	private void OnUpdate(object e)
	{
		float[] array = e as float[];
		float distance = _distance;
		float perc = GetPerc();
		float num = GetGain() + perc * GetGrowth();
		distance = ((!_increase) ? (distance - num * array[1]) : (distance + num * array[1]));
		if (distance >= (float)_distanceMax)
		{
			distance = _distanceMax;
			_increase = false;
		}
		if (distance <= (float)_distanceMin)
		{
			distance = _distanceMin;
			_increase = true;
		}
		_distance = distance;
		UpdateBar();
	}

	private void SetCasting(bool v)
	{
		_casting = v;
		if (_casting)
		{
			Util.SetButton(castBtn, enabled: false);
		}
		else
		{
			Util.SetButton(castBtn);
		}
	}

	private void DoCast()
	{
		GameData.instance.main.DISPATCHER.FRAME_UPDATE.RemoveListener(OnUpdate);
		SetCasting(v: true);
		_fishingInterface.SetButtons(enabled: false);
		_fishingInterface.instance.extension.DoFishingCast(GetDistance());
	}

	private float GetPerc()
	{
		return (_distance - (float)_distanceMin) / ((float)_distanceMax - (float)_distanceMin);
	}

	private int GetDistance()
	{
		return Mathf.RoundToInt(_distance);
	}

	public void OnCastBtn()
	{
		if (castBtn.enabled)
		{
			GameData.instance.audioManager.PlaySoundLink("buttonclick");
			DoCast();
		}
	}

	private void OnDestroy()
	{
		GameData.instance.main.DISPATCHER.FRAME_UPDATE.RemoveListener(OnUpdate);
	}
}
