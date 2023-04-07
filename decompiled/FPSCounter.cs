using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
	[SerializeField]
	private Text _fpsText;

	[SerializeField]
	private float _hudRefreshRate = 0.3f;

	private float _timer;

	private int _min = 99999;

	private int _max = -1;

	private int _waitForMeasureMin = 10;

	private int _currentWait;

	private void Update()
	{
		if (Time.unscaledTime > _timer)
		{
			int num = (int)(1f / Time.unscaledDeltaTime);
			if (_currentWait > _waitForMeasureMin && num < _min)
			{
				_min = num;
			}
			if (num > _max)
			{
				_max = num;
			}
			_currentWait++;
			_fpsText.text = "FPS: " + num + "\nMax: " + _max;
			if (_min != 99999)
			{
				Text fpsText = _fpsText;
				fpsText.text = fpsText.text + " - Min: " + _min;
			}
			_timer = Time.unscaledTime + _hudRefreshRate;
		}
	}

	public void resetFPS()
	{
		Debug.Log("Reset counter");
		_timer = 0f;
		_min = 99999;
		_max = -1;
		_currentWait = 0;
	}
}
