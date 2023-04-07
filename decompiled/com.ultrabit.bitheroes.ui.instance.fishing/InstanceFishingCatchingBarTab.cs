using com.ultrabit.bitheroes.model.fishing;
using com.ultrabit.bitheroes.model.utility;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.instance.fishing;

public class InstanceFishingCatchingBarTab : MonoBehaviour
{
	public Image tab;

	public Image overlay;

	private FishingItemRef _itemRef;

	private FishingBarRef _barRef;

	private FishingBarChanceRef _chanceRef;

	private float _w;

	private RectTransform _rectTransform;

	private int _index;

	public FishingItemRef itemRef => _itemRef;

	public FishingBarRef barRef => _barRef;

	public FishingBarChanceRef chanceRef => _chanceRef;

	public RectTransform rectTransform => _rectTransform;

	public int index
	{
		get
		{
			return _index;
		}
		set
		{
			_index = value;
		}
	}

	public void LoadDetails(FishingItemRef itemRef, FishingBarRef barRef, FishingBarChanceRef chanceRef, float w)
	{
		_itemRef = itemRef;
		_barRef = barRef;
		_chanceRef = chanceRef;
		_w = w;
		_rectTransform = GetComponent<RectTransform>();
		_rectTransform.sizeDelta = new Vector2(w, _rectTransform.sizeDelta.y);
		tab.color = Util.GetColorFromHex(chanceRef.color);
	}
}
