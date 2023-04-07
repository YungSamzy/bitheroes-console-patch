using System.Collections.Generic;
using System.Linq;
using com.ultrabit.bitheroes.model.fishing;
using com.ultrabit.bitheroes.model.utility;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.instance.fishing;

public class InstanceFishingCatchingBar : MonoBehaviour
{
	private const float SPEED = 5f;

	private FishingItemRef _itemRef;

	private InstancePlayer _player;

	private FishingBarRef _barRef;

	private float _w;

	private int _direction;

	private List<InstanceFishingCatchingBarTab> _tabs;

	private RectTransform _rectTransform;

	private GameObject _focus;

	private int _offset;

	private InstanceFishingCatchingScreen _catchingScreen;

	public Transform fishingBarTabPrefab;

	public List<InstanceFishingCatchingBarTab> tabs => _tabs;

	public void LoadDetails(FishingItemRef itemRef, InstancePlayer player, float w, InstanceFishingCatchingScreen catchingScreen)
	{
		_direction = (Util.randomBoolean() ? 1 : (-1));
		_rectTransform = GetComponent<RectTransform>();
		_itemRef = itemRef;
		_player = player;
		_barRef = itemRef.barRef;
		_w = w;
		_catchingScreen = catchingScreen;
		CreateTabs();
	}

	public void SetFocus(GameObject focus, int offset)
	{
		_focus = focus;
		_offset = offset;
	}

	public void UpdatePosition(object e)
	{
		float[] array = e as float[];
		if ((bool)_focus)
		{
			_rectTransform.anchoredPosition = new Vector2(_focus.GetComponent<RectTransform>().anchoredPosition.x + (float)_offset, _rectTransform.anchoredPosition.y);
			return;
		}
		float num = 5f * array[1];
		num /= _catchingScreen.transform.localScale.x;
		float num2 = _rectTransform.anchoredPosition.x + num * (float)_direction;
		if (num2 < 0f)
		{
			num2 = 0f;
			_direction *= -1;
		}
		if (num2 >= _w)
		{
			num2 = _w;
			_direction *= -1;
		}
		_rectTransform.anchoredPosition = new Vector2(num2, _rectTransform.anchoredPosition.y);
	}

	private void CreateTabs()
	{
		_tabs = new List<InstanceFishingCatchingBarTab>();
		float num = _itemRef.getWidth(_player.fishingData.getModifiers()) / _catchingScreen.transform.localScale.x;
		List<FishingBarChanceRef> list = new List<FishingBarChanceRef>();
		list.AddRange(_barRef.chances);
		list.OrderByDescending((FishingBarChanceRef x) => x.size).ToList();
		for (int i = 0; i < list.Count; i++)
		{
			float num2 = num * list[i].size;
			FishingBarChanceRef nextLongestChance = GetNextLongestChance(list[i]);
			if (nextLongestChance != null)
			{
				float num3 = num * nextLongestChance.size;
				num2 = (num2 - num3) / 2f;
				float num4 = 3f / _catchingScreen.transform.localScale.x;
				float num5 = num3 / 2f + num2 / 2f;
				float xPos = 0f - num5 + num4 / 2f;
				float xPos2 = num5 - num4 / 2f;
				AddTab(list[i], xPos, num2 + num4);
				AddTab(list[i], xPos2, num2 + num4);
			}
			else
			{
				AddTab(list[i], 0f, num2);
			}
		}
	}

	private void AddTab(FishingBarChanceRef chanceRef, float xPos, float w)
	{
		Transform transform = Object.Instantiate(fishingBarTabPrefab);
		transform.SetParent(base.transform, worldPositionStays: false);
		_tabs.Insert(0, transform.GetComponent<InstanceFishingCatchingBarTab>());
		transform.GetComponent<InstanceFishingCatchingBarTab>().LoadDetails(_itemRef, _barRef, chanceRef, w);
		transform.GetComponent<InstanceFishingCatchingBarTab>().rectTransform.anchoredPosition = new Vector2(xPos, transform.GetComponent<InstanceFishingCatchingBarTab>().rectTransform.anchoredPosition.y);
	}

	private FishingBarChanceRef GetNextLongestChance(FishingBarChanceRef currentRef)
	{
		FishingBarChanceRef fishingBarChanceRef = null;
		foreach (FishingBarChanceRef chance in _barRef.chances)
		{
			if (!(chance.size >= currentRef.size) && (fishingBarChanceRef == null || chance.size > fishingBarChanceRef.size))
			{
				fishingBarChanceRef = chance;
			}
		}
		return fishingBarChanceRef;
	}

	public InstanceFishingCatchingBarTab GetTabByPosition(float xPos)
	{
		foreach (InstanceFishingCatchingBarTab tab in _tabs)
		{
			if (xPos >= tab.transform.position.x - tab.rectTransform.sizeDelta.x && xPos <= tab.transform.position.x + tab.rectTransform.sizeDelta.x)
			{
				return tab;
			}
		}
		return null;
	}
}
