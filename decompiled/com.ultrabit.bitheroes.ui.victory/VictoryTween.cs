using System.Collections;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.victory;

public class VictoryTween : MonoBehaviour
{
	private const int SWORD_OFFSET = 300;

	private const int TWEEN_OFFSET = 177;

	public TextMeshProUGUI text;

	private Image ribbon;

	public Image lBlueRibbon;

	public Image redRibbon;

	public Image shield;

	public Image swordLeft;

	public Image swordRight;

	public Image shine;

	private GameObject _parent;

	private RectTransform _shineTransform;

	private RectTransform _shieldTransform;

	private RectTransform _swordLeftTransform;

	private RectTransform _swordRightTransform;

	private RectTransform _ribbonTransform;

	private RectTransform _textTransform;

	public void ShieldAnimation(GameObject parent, bool animate = true, string customShieldText = null, bool isRed = false)
	{
		_parent = parent;
		text.text = ((customShieldText != null) ? customShieldText : Language.GetString("battle_victory_results"));
		redRibbon.gameObject.SetActive(isRed);
		lBlueRibbon.gameObject.SetActive(!isRed);
		ribbon = (isRed ? redRibbon : lBlueRibbon);
		if (animate)
		{
			float num = 1f;
			float num2 = 0f;
			float num3 = 0.35f;
			float num4 = 0f;
			float num5 = 0.4f;
			float num6 = 0.2f;
			float num7 = 0.4f;
			float num8 = 0.3f;
			float num9 = 0.35f;
			float num10 = 0.7f;
			float num11 = 0.35f;
			float num12 = 0.9f;
			float num13 = 1.7f;
			if (AppInfo.TESTING)
			{
				num /= 3f;
				num2 /= 3f;
				num3 /= 3f;
				num4 /= 3f;
				num5 /= 3f;
				num6 /= 3f;
				num7 /= 3f;
				num8 /= 3f;
				num9 /= 3f;
				num10 /= 3f;
				num11 /= 3f;
				num12 /= 3f;
				num13 /= 3f;
			}
			_shineTransform = shine.GetComponent<RectTransform>();
			Color color = shine.color;
			Vector2 sizeDelta = _shineTransform.sizeDelta;
			shine.color = Util.WHITE_ALPHA_0;
			_shineTransform.sizeDelta = new Vector2(0f, 0f);
			StartCoroutine(ShineAnimation(animateScale: true, _shineTransform.sizeDelta, sizeDelta, animateColor: true, shine.color, color, num, easeIn: false, num2));
			_shieldTransform = shield.GetComponent<RectTransform>();
			Vector2 sizeDelta2 = _shieldTransform.sizeDelta;
			_shieldTransform.sizeDelta *= new Vector2(6f, 6f);
			shield.color = Util.WHITE_ALPHA_0;
			StartCoroutine(ShieldAnimation(animateScale: true, _shieldTransform.sizeDelta, sizeDelta2, animateColor: true, shield.color, Color.white, num3, easeIn: true, num4));
			_swordLeftTransform = swordLeft.GetComponent<RectTransform>();
			Vector2 anchoredPosition = _swordLeftTransform.anchoredPosition;
			_swordLeftTransform.anchoredPosition = new Vector2(_swordLeftTransform.anchoredPosition.x - 300f, _swordLeftTransform.anchoredPosition.y + 300f);
			swordLeft.color = Util.WHITE_ALPHA_0;
			StartCoroutine(SwordLeftAnimation(animatePos: true, _swordLeftTransform.anchoredPosition, anchoredPosition, animateColor: true, swordLeft.color, Color.white, num5, easeIn: true, num6));
			_swordRightTransform = swordRight.GetComponent<RectTransform>();
			Vector2 anchoredPosition2 = _swordRightTransform.anchoredPosition;
			_swordRightTransform.anchoredPosition = new Vector2(_swordRightTransform.anchoredPosition.x + 300f, _swordRightTransform.anchoredPosition.y + 300f);
			swordLeft.color = Util.WHITE_ALPHA_0;
			StartCoroutine(SwordRightAnimation(animatePos: true, _swordRightTransform.anchoredPosition, anchoredPosition2, animateColor: true, swordRight.color, Color.white, num7, easeIn: true, num8));
			_ribbonTransform = ribbon.GetComponent<RectTransform>();
			Vector2 sizeDelta3 = _ribbonTransform.sizeDelta;
			_ribbonTransform.sizeDelta = new Vector2(_ribbonTransform.sizeDelta.x * 0.1f, 0.1f);
			ribbon.color = Util.WHITE_ALPHA_0;
			StartCoroutine(RibbonAnimation(animateScale: true, _ribbonTransform.sizeDelta, sizeDelta3, animateColor: true, ribbon.color, Color.white, num9, easeIn: true, num10));
			_textTransform = text.GetComponent<RectTransform>();
			Vector2 sizeDelta4 = _textTransform.sizeDelta;
			_textTransform.sizeDelta = new Vector2(0.1f, 0.1f);
			text.color = Util.WHITE_ALPHA_0;
			StartCoroutine(TextAnimation(animateScale: true, _textTransform.sizeDelta, sizeDelta4, animateColor: true, text.color, Color.white, num11, easeIn: true, num12));
			StartCoroutine(AnimationEnd(num13));
		}
	}

	private IEnumerator ShineAnimation(bool animateScale, Vector2 initialScale, Vector2 finalScale, bool animateColor, Color initialColor, Color finalColor, float duration, bool easeIn, float delay = 0f)
	{
		yield return new WaitForSeconds(delay);
		DoShineAnimation(animateScale, initialScale, finalScale, animateColor, initialColor, finalColor, duration, easeIn, delay);
	}

	private void DoShineAnimation(bool animateScale, Vector2 initialScale, Vector2 finalScale, bool animateColor, Color initialColor, Color finalColor, float duration, bool easeIn, float delay = 0f)
	{
		if (animateColor)
		{
			shine.color = initialColor;
			shine.DOColor(finalColor, duration).SetEase(Ease.OutCubic);
		}
		if (animateScale)
		{
			_shineTransform.sizeDelta = initialScale;
			_shineTransform.DOSizeDelta(finalScale, duration).SetEase(Ease.OutCubic);
		}
	}

	private IEnumerator ShieldAnimation(bool animateScale, Vector2 initialScale, Vector2 finalScale, bool animateColor, Color initialColor, Color finalColor, float duration, bool easeIn, float delay = 0f)
	{
		yield return new WaitForSeconds(delay);
		DoShieldAnimation(animateScale, initialScale, finalScale, animateColor, initialColor, finalColor, duration, easeIn, delay);
	}

	private void DoShieldAnimation(bool animateScale, Vector2 initialScale, Vector2 finalScale, bool animateColor, Color initialColor, Color finalColor, float duration, bool easeIn, float delay = 0f)
	{
		GameData.instance.audioManager.PlaySoundLink("victoryshield");
		if (animateColor)
		{
			shield.color = initialColor;
			shield.DOColor(finalColor, duration).SetEase(Ease.OutCubic);
		}
		if (animateScale)
		{
			_shieldTransform.sizeDelta = initialScale;
			_shieldTransform.DOSizeDelta(finalScale, duration).SetEase(Ease.OutCubic);
		}
	}

	private IEnumerator SwordLeftAnimation(bool animatePos, Vector2 initialPos, Vector2 finalPos, bool animateColor, Color initialColor, Color finalColor, float duration, bool easeIn, float delay = 0f)
	{
		yield return new WaitForSeconds(delay);
		DoSwordLeftAnimation(animatePos, initialPos, finalPos, animateColor, initialColor, finalColor, duration, easeIn, delay);
	}

	private void DoSwordLeftAnimation(bool animatePos, Vector2 initialPos, Vector2 finalPos, bool animateColor, Color initialColor, Color finalColor, float duration, bool easeIn, float delay = 0f)
	{
		GameData.instance.audioManager.PlaySoundLink("victorysword");
		if (animateColor)
		{
			swordLeft.color = initialColor;
			swordLeft.DOColor(finalColor, duration).SetEase(Ease.OutCubic);
		}
		if (animatePos)
		{
			_swordLeftTransform.anchoredPosition = initialPos;
			_swordLeftTransform.DOAnchorPos(finalPos, duration).SetEase(easeIn ? Ease.Linear : Ease.OutCubic);
		}
	}

	private IEnumerator SwordRightAnimation(bool animatePos, Vector2 initialPos, Vector2 finalPos, bool animateColor, Color initialColor, Color finalColor, float duration, bool easeIn, float delay = 0f)
	{
		yield return new WaitForSeconds(delay);
		DoSwordRightAnimation(animatePos, initialPos, finalPos, animateColor, initialColor, finalColor, duration, easeIn, delay);
	}

	private void DoSwordRightAnimation(bool animatePos, Vector2 initialPos, Vector2 finalPos, bool animateColor, Color initialColor, Color finalColor, float duration, bool easeIn, float delay = 0f)
	{
		GameData.instance.audioManager.PlaySoundLink("victorysword");
		if (animateColor)
		{
			swordRight.color = initialColor;
			swordRight.DOColor(finalColor, duration).SetEase(Ease.OutCubic);
		}
		if (animatePos)
		{
			_swordRightTransform.anchoredPosition = initialPos;
			_swordRightTransform.DOAnchorPos(finalPos, duration).SetEase(easeIn ? Ease.Linear : Ease.OutCubic);
		}
	}

	private IEnumerator RibbonAnimation(bool animateScale, Vector2 initialScale, Vector2 finalScale, bool animateColor, Color initialColor, Color finalColor, float duration, bool easeIn, float delay = 0f)
	{
		yield return new WaitForSeconds(delay);
		DoRibbonAnimation(animateScale, initialScale, finalScale, animateColor, initialColor, finalColor, duration, easeIn, delay);
	}

	private void DoRibbonAnimation(bool animateScale, Vector2 initialScale, Vector2 finalScale, bool animateColor, Color initialColor, Color finalColor, float duration, bool easeIn, float delay = 0f)
	{
		Vector2 vector = finalScale;
		vector *= new Vector2(1.1f, 1.1f);
		if (animateColor)
		{
			ribbon.color = initialColor;
			ribbon.DOColor(finalColor, duration).SetEase(easeIn ? Ease.InCubic : Ease.OutCubic);
		}
		if (animateScale)
		{
			Sequence s = DOTween.Sequence();
			_ribbonTransform.sizeDelta = initialScale;
			s.Insert(0f, _ribbonTransform.DOScale(vector, duration * 0.8f).SetEase(easeIn ? Ease.Linear : Ease.OutCubic));
			s.Insert(duration * 0.8f, _ribbonTransform.DOScale(finalScale, duration * 0.2f).SetEase(easeIn ? Ease.Linear : Ease.OutCubic));
		}
	}

	private IEnumerator TextAnimation(bool animateScale, Vector2 initialScale, Vector2 finalScale, bool animateColor, Color initialColor, Color finalColor, float duration, bool easeIn, float delay = 0f)
	{
		yield return new WaitForSeconds(delay);
		DoTextAnimation(animateScale, initialScale, finalScale, animateColor, initialColor, finalColor, duration, easeIn, delay);
	}

	private void DoTextAnimation(bool animateScale, Vector2 initialScale, Vector2 finalScale, bool animateColor, Color initialColor, Color finalColor, float duration, bool easeIn, float delay = 0f)
	{
		Vector2 vector = finalScale;
		vector *= new Vector2(1.1f, 1.1f);
		if (animateColor)
		{
			text.color = initialColor;
			text.DOColor(finalColor, duration).SetEase(easeIn ? Ease.InCubic : Ease.OutCubic);
		}
		if (animateScale)
		{
			Sequence s = DOTween.Sequence();
			_textTransform.sizeDelta = vector;
			s.Insert(0f, _textTransform.DOSizeDelta(finalScale, duration * 0.2f).SetEase(easeIn ? Ease.Linear : Ease.OutCubic));
			s.Insert(duration * 0.8f, _textTransform.DOSizeDelta(vector, duration * 0.8f).SetEase(easeIn ? Ease.Linear : Ease.OutCubic));
		}
	}

	private IEnumerator AnimationEnd(float delay)
	{
		yield return new WaitForSeconds(delay);
		_parent.BroadcastMessage("OnTweenComplete");
	}

	public void DoTweenOffset(float duration, bool fade)
	{
		if (fade)
		{
			DoShineAnimation(animateScale: false, Vector2.down, Vector2.down, animateColor: true, Util.WHITE_ALPHA_0, Color.white, duration, easeIn: true);
			DoShieldAnimation(animateScale: false, Vector2.down, Vector2.down, animateColor: true, Util.WHITE_ALPHA_0, Color.white, duration, easeIn: true);
			DoSwordLeftAnimation(animatePos: false, Vector2.down, Vector2.down, animateColor: true, Util.WHITE_ALPHA_0, Color.white, duration, easeIn: true);
			DoSwordRightAnimation(animatePos: false, Vector2.down, Vector2.down, animateColor: true, Util.WHITE_ALPHA_0, Color.white, duration, easeIn: true);
			DoRibbonAnimation(animateScale: false, Vector2.down, Vector2.down, animateColor: true, Util.WHITE_ALPHA_0, Color.white, duration, easeIn: true);
			DoTextAnimation(animateScale: false, Vector2.down, Vector2.down, animateColor: true, Util.WHITE_ALPHA_0, Color.white, duration, easeIn: true);
		}
		Vector2 vector = new Vector2(base.transform.localPosition.x, base.transform.localPosition.y + 177f);
		Vector2 vector2 = new Vector2(base.transform.localPosition.x, base.transform.localPosition.y + 177f + 50f);
		Sequence s = DOTween.Sequence();
		base.transform.localPosition = new Vector2(base.transform.localPosition.x, base.transform.localPosition.y);
		s.Insert(0f, base.transform.DOLocalMove(vector2, duration * 0.6f).SetEase(Ease.InOutQuart));
		s.Insert(duration * 0.6f, base.transform.DOLocalMove(vector, duration * 0.4f).SetEase(Ease.InOutQuart));
	}

	public void ForceTweenUp()
	{
		Vector3 position = base.transform.position;
		base.transform.position += new Vector3(0f, Screen.height, 0f);
		Vector3 position2 = base.transform.position;
		Sequence s = DOTween.Sequence();
		base.transform.position = position;
		s.Insert(0f, base.transform.DOMove(position -= new Vector3(0f, (float)Screen.height * 0.1f, 0f), 0.25f).SetEase(Ease.OutQuad));
		s.Insert(0.25f, base.transform.DOMove(position2, 0.25f).SetEase(Ease.InOutQuad));
	}

	private void OnDestroy()
	{
	}
}
