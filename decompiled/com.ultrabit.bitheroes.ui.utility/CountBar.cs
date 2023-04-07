using com.ultrabit.bitheroes.model.utility;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.utility;

public class CountBar : MonoBehaviour
{
	public TextMeshProUGUI countTxt;

	private int _current;

	private int _total;

	public void LoadDetails(int current, int total)
	{
		SetCurrent(current, draw: false);
		SetTotal(total, draw: false);
		DoUpdate();
	}

	public void SetCurrent(int current, bool draw = true)
	{
		_current = current;
		if (draw)
		{
			DoUpdate();
		}
	}

	public void SetTotal(int total, bool draw = true)
	{
		_total = total;
		if (draw)
		{
			DoUpdate();
		}
	}

	public void DoUpdate()
	{
		if (_total > 1)
		{
			countTxt.text = Util.NumberFormat(_current) + " / " + Util.NumberFormat(_total);
		}
		else
		{
			countTxt.text = Util.NumberFormat(_current);
		}
	}
}
