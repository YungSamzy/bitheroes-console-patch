using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.utility;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.battle;

public class BattleText : MonoBehaviour
{
	public const float SCALEX = 0.9f;

	public const float SCALEY = 1.13f;

	public static string COLOR_WHITE = "#FFFFFF";

	public static string COLOR_BLACK = "#000000";

	public static string COLOR_RED = "#FF0000";

	public static string COLOR_ORANGE = "#FF9900";

	public static string COLOR_BLUE = "#7979FF";

	public static string COLOR_CYAN = "#00CCFF";

	public static string COLOR_GREEN = "#00FF00";

	public static string COLOR_YELLOW = "#FFFF00";

	public static string COLOR_PURPLE = "#FF77FF";

	public static string COLOR_PINK = "#FF2FAA";

	public static string COLOR_TEAL = "#00FFFF";

	public const string COLOR_FIRE = "#FD5400";

	public const string COLOR_WATER = "#1342EF";

	public const string COLOR_ELECTRIC = "#E7DA83";

	public const string COLOR_EARTH = "#5FC608";

	public const string COLOR_AIR = "#B5D4F0";

	public TextMeshPro displayTxt;

	private string _text;

	private string _color;

	private float _duration;

	private float _spread;

	private float _scale;

	private float _distance;

	private Coroutine _delay;

	private Coroutine _flashTimer;

	private Color _previousOutlineColor;

	private bool _readyForUse = true;

	public bool readyForUse => _readyForUse;

	public float x
	{
		get
		{
			return base.transform.position.x;
		}
		set
		{
			base.transform.position = new Vector3(value, base.transform.position.y, base.transform.position.z);
		}
	}

	public float y
	{
		get
		{
			return base.transform.position.y;
		}
		set
		{
			base.transform.position = new Vector3(base.transform.position.x, value, base.transform.position.z);
		}
	}

	public void LoadDetails(string text, string color, float duration = 3f, float spread = 0f, float xPos = 0f, float yPos = 0f, float scale = 1f, float distance = -30f, float delay = 0f)
	{
		_text = text;
		_color = color;
		_duration = duration;
		_scale = scale;
		_spread = spread;
		_distance = distance;
		_readyForUse = false;
		base.transform.position = new Vector3(xPos, yPos, base.transform.position.z);
		displayTxt.text = "<color=" + _color + ">" + _text + "</color>";
		displayTxt.color = new Color(displayTxt.color.r, displayTxt.color.g, displayTxt.color.b, 1f);
		if (displayTxt.enabled)
		{
			displayTxt.sortingLayerID = SortingLayer.NameToID("Battle");
			displayTxt.sortingOrder = 150;
		}
		GetComponent<MeshRenderer>().enabled = false;
		if (delay == 0f)
		{
			Display();
			return;
		}
		if (_delay != null)
		{
			GameData.instance.main.coroutineTimer.StopTimer(ref _delay);
		}
		_delay = GameData.instance.main.coroutineTimer.AddTimer(base.gameObject, delay, CoroutineTimer.TYPE.SECONDS, Display);
	}

	public void Display()
	{
		GetComponent<MeshRenderer>().enabled = true;
		base.transform.localScale = new Vector3(_scale * 5f * 0.9f, _scale * 5f * 1.13f, 1f);
		base.transform.position = new Vector3(Util.RandomNumber(x - _spread / 2f, x + _spread / 2f), Util.RandomNumber(y - _spread / 2f, y + _spread / 2f), base.transform.position.z);
		float num = _duration / 3f;
		if (!GameData.instance.SAVE_STATE.reducedEffects)
		{
			DoFlash();
		}
		Sequence sequence = DOTween.Sequence();
		sequence.Insert(0f, ShortcutExtensions.DOScale(endValue: new Vector3(_scale * 1.5f * 0.9f, _scale * 1.5f * 1.13f, 1f), target: base.transform, duration: 0.5f).SetEase(Ease.OutBack));
		sequence.Insert(0f, ShortcutExtensions.DOMove(endValue: new Vector3(base.transform.position.x, base.transform.position.y - _distance, base.transform.position.z), target: base.transform, duration: _duration).SetEase(Ease.OutBack));
		Color endValue3 = new Color(displayTxt.color.r, displayTxt.color.g, displayTxt.color.b, 0f);
		sequence.Insert(_duration - num, displayTxt.DOColor(endValue3, num).SetEase(Ease.OutBack));
		sequence.OnComplete(OnCompletePositionTween);
	}

	private void DoFlash()
	{
		_previousOutlineColor = displayTxt.outlineColor;
		displayTxt.outlineColor = Color.white;
		displayTxt.text = "<color=" + COLOR_WHITE + ">" + _text + "</color>";
		if (_flashTimer != null)
		{
			GameData.instance.main.coroutineTimer.StopTimer(ref _flashTimer);
		}
		GameData.instance.main.coroutineTimer.AddTimer(base.gameObject, 0.05f, CoroutineTimer.TYPE.SECONDS, OnFlashEnd);
	}

	private void OnFlashEnd()
	{
		displayTxt.outlineColor = _previousOutlineColor;
		displayTxt.text = "<color=" + _color + ">" + _text + "</color>";
	}

	private void OnCompletePositionTween()
	{
		PrepareForRecycle();
	}

	public void PrepareForRecycle()
	{
		GetComponent<MeshRenderer>().enabled = false;
		_readyForUse = true;
	}
}
