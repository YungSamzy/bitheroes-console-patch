using com.ultrabit.bitheroes.model.utility;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.utility;

public class SliderTile : MonoBehaviour
{
	public CustomSlider slider;

	public TextMeshProUGUI nameTxt;

	public TextMeshProUGUI valueTxt;

	private float _lowVal;

	private float _highVal;

	private float _startVal;

	private bool _flip;

	private string _callerLink;

	private GameObject _broadcastReciever;

	private bool created;

	private float _percent;

	private float _currentVal;

	private float _range;

	public void Create(float lowVal = 0f, float highVal = 1f, string name = "", float startVal = 0f, bool flip = false, string callerLink = "", GameObject broadcastReciever = null)
	{
		_lowVal = lowVal;
		_highVal = highVal;
		_startVal = startVal;
		_flip = flip;
		_callerLink = callerLink;
		_broadcastReciever = broadcastReciever;
		nameTxt.text = name;
		_range = Mathf.Abs(_lowVal) + _highVal;
		if (_startVal < _lowVal)
		{
			_startVal = _lowVal;
		}
		if (_startVal > _highVal)
		{
			_startVal = _highVal;
		}
		_percent = (_startVal - _lowVal) / (_highVal - _lowVal);
		if (flip)
		{
			_percent = 1f - _percent;
		}
		_currentVal = _startVal;
		slider.minValue = lowVal;
		slider.maxValue = highVal;
		slider.value = _currentVal;
		SetValueText(slider.value.ToString());
		created = true;
	}

	public void SetValueText(string text = "")
	{
		valueTxt.text = text;
	}

	public void OnValueChanged()
	{
		if (created)
		{
			_currentVal = slider.value;
			SetValueText(Util.NumberFormat(Mathf.Round(_currentVal)));
			if (_broadcastReciever != null)
			{
				_broadcastReciever.BroadcastMessage("OnSliderChange" + _callerLink);
			}
		}
	}
}
