using com.ultrabit.bitheroes.core;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.item;

public class ItemRankTile : MonoBehaviour
{
	public const int FRAME_EMPTY = 1;

	public const int FRAME_GREEN = 2;

	public const int FRAME_BLUE = 3;

	private int _frame;

	private Image image;

	public bool filled
	{
		get
		{
			return _frame != 1;
		}
		set
		{
			if (!value)
			{
				_frame = 1;
			}
			if (image != null)
			{
				image.overrideSprite = GameData.instance.rankComponent.GetRank(_frame);
			}
		}
	}

	private void Awake()
	{
		image = GetComponent<Image>();
	}

	public void LoadDetails(bool enabled = true, int frame = -1)
	{
		_frame = frame;
		if (_frame < 0)
		{
			_frame = 2;
		}
		filled = enabled;
	}
}
