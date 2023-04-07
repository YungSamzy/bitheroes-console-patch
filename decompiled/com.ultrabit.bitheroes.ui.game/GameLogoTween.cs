using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.ui.utility;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui.game;

public class GameLogoTween : MonoBehaviour
{
	[HideInInspector]
	public UnityEvent COMPLETE = new UnityEvent();

	private const int SWORD_OFFSET = 900;

	public SpriteRenderer bg;

	public SpriteRenderer textBit;

	public SpriteRenderer textHeroes;

	public SpriteRenderer shield;

	public SpriteRenderer swordLeft;

	public SpriteRenderer swordRight;

	public SpriteRenderer shine;

	public GameObject shineMask;

	public SpriteRenderer shieldShine;

	public SpriteRenderer gameLogo;

	private float completeDuration;

	public void LoadDetails()
	{
		gameLogo.gameObject.SetActive(value: false);
		float num = 1.5f;
		float num2 = 0.5f;
		float num3 = 1f;
		float num4 = 2f;
		float num5 = 0.35f;
		float num6 = 2f;
		float num7 = 0.4f;
		float num8 = 2.2f;
		float num9 = 0.4f;
		float num10 = 2.3f;
		float num11 = 0.3f;
		float num12 = 2.6f;
		float num13 = 0.3f;
		float num14 = 2.8f;
		float num15 = 3.5f;
		completeDuration = 1.75f;
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
			num14 /= 3f;
			num15 /= 3f;
			completeDuration /= 3f;
		}
		Sequence sequence = DOTween.Sequence();
		bg.transform.localScale = new Vector3((float)Screen.width / Main.DEFAULT_BOUNDS.width, (float)Screen.height / Main.DEFAULT_BOUNDS.height, 1f);
		Color color = bg.color;
		sequence.Insert(num2, DOTweenModuleSprite.DOColor(endValue: new Color(color.r, color.g, color.b, 0f), target: bg, duration: num).SetEase(Ease.InCubic).OnComplete(delegate
		{
			bg.gameObject.SetActive(value: false);
		}));
		shine.transform.localScale = new Vector3(0f, 0f, 1f);
		shine.color = new Color(1f, 1f, 1f, 0f);
		sequence.Insert(num4, DOTweenModuleSprite.DOColor(endValue: Color.white, target: shine, duration: num3).SetEase(Ease.OutBack));
		sequence.Insert(num4, ShortcutExtensions.DOScale(endValue: new Vector3(50f, 50f, 1f), target: shine.transform, duration: num3).SetEase(Ease.Linear));
		shield.transform.localScale = new Vector3(6f, 6f, 1f);
		shield.color = new Color(1f, 1f, 1f, 0f);
		shineMask.gameObject.SetActive(value: false);
		shieldShine.gameObject.SetActive(value: false);
		sequence.Insert(num6, shield.DOColor(Color.white, num5).SetEase(Ease.InCubic));
		sequence.Insert(num6, shield.transform.DOScale(Vector3.one, num5).SetEase(Ease.Linear).OnComplete(delegate
		{
			shield.GetComponent<SpriteFlash>().DoFlash();
			shineMask.gameObject.SetActive(value: true);
			shieldShine.gameObject.SetActive(value: true);
		}));
		Vector3 position = swordLeft.transform.position;
		position.z = 0f;
		swordLeft.color = new Color(1f, 1f, 1f, 0f);
		swordLeft.transform.localPosition += new Vector3(-900f, 900f, 0f);
		sequence.Insert(num8, swordLeft.DOColor(Color.white, num7).SetEase(Ease.InCubic));
		sequence.Insert(num8, swordLeft.transform.DOLocalMove(position, num7).SetEase(Ease.Linear).OnComplete(delegate
		{
			swordLeft.GetComponent<SpriteFlash>().DoFlash();
		}));
		Vector3 localPosition = swordRight.transform.localPosition;
		localPosition.z = 0f;
		swordRight.color = new Color(1f, 1f, 1f, 0f);
		swordRight.transform.localPosition += new Vector3(900f, 900f, 0f);
		sequence.Insert(num10, swordRight.DOColor(Color.white, num9).SetEase(Ease.InCubic));
		sequence.Insert(num10, swordRight.transform.DOLocalMove(position, num9).SetEase(Ease.Linear).OnComplete(delegate
		{
			swordRight.GetComponent<SpriteFlash>().DoFlash();
		}));
		Vector2 vector = new Vector2(textBit.transform.localScale.x, textBit.transform.localScale.y);
		Vector3 localPosition2 = textBit.transform.localPosition;
		localPosition2.z = 0f;
		textBit.transform.localScale = new Vector3(6f, 6f, 1f);
		textBit.color = new Color(1f, 1f, 1f, 0f);
		textBit.transform.localPosition -= new Vector3(0f, 300f, 0f);
		sequence.Insert(num12, textBit.DOColor(Color.white, num11).SetEase(Ease.OutBack));
		sequence.Insert(num12, textBit.transform.DOScale(vector, num11).SetEase(Ease.Linear));
		sequence.Insert(num12, textBit.transform.DOLocalMove(localPosition2, num11).SetEase(Ease.Linear));
		Vector2 vector2 = new Vector2(textHeroes.transform.localScale.x, textHeroes.transform.localScale.y);
		Vector3 localPosition3 = textHeroes.transform.localPosition;
		localPosition3.z = 0f;
		textHeroes.transform.localScale = new Vector3(6f, 6f, 1f);
		textHeroes.color = new Color(1f, 1f, 1f, 0f);
		textHeroes.transform.localPosition -= new Vector3(0f, 300f, 0f);
		sequence.Insert(num14, textHeroes.DOColor(Color.white, num13).SetEase(Ease.OutBack));
		sequence.Insert(num14, textHeroes.transform.DOScale(vector2, num13).SetEase(Ease.Linear));
		sequence.Insert(num14, textHeroes.transform.DOLocalMove(localPosition3, num13).SetEase(Ease.Linear));
		sequence.OnComplete(StartComplete);
	}

	private void OnCompleteAlpha(float duration)
	{
		Color currentColor = Color.white;
		Color endValue = new Color(1f, 1f, 1f, 0f);
		gameLogo.color = currentColor;
		shine.color = currentColor;
		DOTween.To(() => currentColor, delegate(Color x)
		{
			currentColor = x;
		}, endValue, duration).SetEase(Ease.Linear).OnUpdate(delegate
		{
			gameLogo.color = currentColor;
			shine.color = currentColor;
		})
			.OnComplete(delegate
			{
				CompleteAnimation();
			});
	}

	private void StartComplete()
	{
		shieldShine.gameObject.SetActive(value: false);
		shineMask.gameObject.SetActive(value: false);
		gameLogo.gameObject.SetActive(value: true);
		shield.gameObject.SetActive(value: false);
		swordLeft.gameObject.SetActive(value: false);
		swordRight.gameObject.SetActive(value: false);
		textBit.gameObject.SetActive(value: false);
		textHeroes.gameObject.SetActive(value: false);
		OnCompleteAlpha(completeDuration);
	}

	private void CompleteAnimation()
	{
		GameData.instance.main.coroutineTimer.AddTimer(base.gameObject, 0.5f, CoroutineTimer.TYPE.SECONDS, delegate
		{
			COMPLETE.Invoke();
		});
	}

	private void OnDestroy()
	{
	}
}
