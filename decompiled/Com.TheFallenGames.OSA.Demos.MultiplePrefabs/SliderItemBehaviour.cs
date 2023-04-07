using System;
using UnityEngine;
using UnityEngine.UI;

namespace Com.TheFallenGames.OSA.Demos.MultiplePrefabs;

[ExecuteInEditMode]
public class SliderItemBehaviour : MonoBehaviour
{
	private Text _Value;

	private Slider _Slider;

	public float Value
	{
		get
		{
			return _Slider.value;
		}
		set
		{
			_Slider.value = value;
		}
	}

	public event Action<float> ValueChanged;

	private void Awake()
	{
		_Value = base.transform.Find("ValueText").GetComponentInChildren<Text>();
		_Slider = GetComponentInChildren<Slider>();
		if (Application.isPlaying)
		{
			_Slider.onValueChanged.AddListener(OnValueChanged);
			OnValueChanged(_Slider.value);
		}
	}

	private void OnValueChanged(float value)
	{
		_Value.text = value.ToString("0.00");
		if (this.ValueChanged != null)
		{
			this.ValueChanged(value);
		}
	}
}
