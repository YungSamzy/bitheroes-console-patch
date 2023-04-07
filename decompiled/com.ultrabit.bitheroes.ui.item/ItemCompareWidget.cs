using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.item;

public class ItemCompareWidget : MonoBehaviour
{
	public const string COMPARE_BETTER = "+";

	public const string COMPARE_EQUAL = "=";

	public const string COMPARE_WORSE = "-";

	public const string COMPARE_EQUIPPED = "E";

	public const string COMPARE_COSMETIC = "cosmetic";

	public Transform better;

	public Transform equal;

	public Transform worse;

	public Transform equipped;

	public Transform cosmetic;

	private Image image;

	private GameObject target;

	private string _currentLabel = "";

	private GameObject _icon;

	public string currentLabel => _currentLabel;

	public void Awake()
	{
	}

	public void SetCompare(int difference)
	{
		if (difference == 0)
		{
			SetCompare("=");
		}
		else if (difference < 0)
		{
			SetCompare("-");
		}
		else
		{
			SetCompare("+");
		}
	}

	public void SetCompare(string label)
	{
		switch (label)
		{
		case "+":
			target = better.gameObject;
			break;
		case "=":
			target = null;
			break;
		case "-":
			target = worse.gameObject;
			break;
		case "E":
			target = equipped.gameObject;
			break;
		case "cosmetic":
			target = cosmetic.gameObject;
			break;
		}
		if (_icon != null)
		{
			Object.Destroy(_icon);
		}
		if (target != null)
		{
			_icon = Object.Instantiate(target, base.transform, worldPositionStays: false);
			_icon.GetComponent<RectTransform>().sizeDelta = new Vector2(target.GetComponent<Image>().sprite.bounds.size.x * 200f / 3f, target.GetComponent<Image>().sprite.bounds.size.y * 200f / 3f);
			_icon.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
		}
		_currentLabel = label;
	}
}
