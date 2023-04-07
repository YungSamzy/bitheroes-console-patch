using DG.Tweening;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.utility;

public class TextBrightness : MonoBehaviour
{
	private bool isOn;

	private TextMeshProUGUI text;

	private float highSat;

	private float regHue;

	private bool glowing;

	private float originalSat;

	private float originalHue;

	private float originalValue;

	private float originalAlpha;

	private float maxSat;

	private float minSat;

	private float changedSat;

	private void Awake()
	{
		text = GetComponent<TextMeshProUGUI>();
	}

	public void ForceStart()
	{
		text = GetComponent<TextMeshProUGUI>();
	}

	public void AddBrightness()
	{
		if (!isOn && text != null)
		{
			isOn = true;
			float a = text.color.a;
			Color.RGBToHSV(text.color, out var H, out var S, out var V);
			text.outlineColor = new Color(0.15f, 0.15f, 0.15f);
			regHue = H;
			highSat = S - 0.3f;
			text.color = Color.HSVToRGB(H, highSat, V);
			text.color = new Color(text.color.r, text.color.g, text.color.b, a);
			text.UpdateFontAsset();
		}
	}

	public void ClearBrightness()
	{
		if (isOn && text != null)
		{
			isOn = false;
			float a = text.color.a;
			Color.RGBToHSV(text.color, out var _, out var _, out var V);
			text.outlineColor = Color.black;
			text.color = Color.HSVToRGB(regHue, highSat + 0.3f, V);
			text.color = new Color(text.color.r, text.color.g, text.color.b, a);
			text.UpdateFontAsset();
		}
	}

	public void StartGlow()
	{
		if (!glowing && text != null)
		{
			glowing = true;
			Color.RGBToHSV(text.color, out var H, out var S, out var V);
			originalHue = H;
			originalSat = S;
			originalValue = V;
			originalAlpha = text.color.a;
			maxSat = originalSat - 0.3f;
			TweenGlowIncreased();
		}
	}

	public void StopGlow()
	{
		if (glowing && text != null)
		{
			glowing = false;
			text.outlineColor = Color.black;
			text.color = Color.HSVToRGB(originalHue, originalSat, originalValue);
			text.color = new Color(text.color.r, text.color.g, text.color.b, originalAlpha);
			text.UpdateFontAsset();
		}
	}

	public void TweenGlowIncreased()
	{
		float initValue = originalSat;
		DOTween.To(() => initValue, delegate(float x)
		{
			initValue = x;
		}, maxSat, 1f).SetEase(Ease.OutCubic).OnUpdate(delegate
		{
			changedSat = initValue;
			text.outlineColor = new Color(0.15f * changedSat, 0.15f * changedSat, 0.15f * changedSat);
			text.color = Color.HSVToRGB(originalHue, changedSat, originalValue);
			text.color = new Color(text.color.r, text.color.g, text.color.b, originalAlpha);
			text.UpdateFontAsset();
		})
			.OnComplete(delegate
			{
				TweenGlowDecreased();
			});
	}

	public void TweenGlowDecreased()
	{
		float initValue = maxSat;
		DOTween.To(() => initValue, delegate(float x)
		{
			initValue = x;
		}, originalSat, 1f).SetEase(Ease.OutCubic).OnUpdate(delegate
		{
			changedSat = initValue;
			text.outlineColor = new Color(0.15f * (1f - changedSat), 0.15f * (1f - changedSat), 0.15f * (1f - changedSat));
			text.color = Color.HSVToRGB(originalHue, changedSat, originalValue);
			text.color = new Color(text.color.r, text.color.g, text.color.b, originalAlpha);
			text.UpdateFontAsset();
		})
			.OnComplete(delegate
			{
				TweenGlowIncreased();
			});
	}
}
