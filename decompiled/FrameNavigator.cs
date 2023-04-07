using UnityEngine;

public class FrameNavigator : MonoBehaviour
{
	private int _currentFrame = 1;

	private int _totalFrames = 1;

	private Transform t;

	public int currentFrame => _currentFrame;

	public int totalFrames => _totalFrames;

	private void Awake()
	{
		t = base.transform;
		_totalFrames = GetTotalFrames();
	}

	public void GoToAndStop(int frame)
	{
		getCurrentGameObject()?.SetActive(value: false);
		_currentFrame = frame;
		getCurrentGameObject()?.SetActive(value: true);
	}

	private int GetTotalFrames()
	{
		for (int i = 1; i < 10000; i++)
		{
			if (t.Find(i.ToString()) == null)
			{
				return i - 1;
			}
		}
		return 1;
	}

	private GameObject getCurrentGameObject()
	{
		Transform transform = t.Find(_currentFrame.ToString());
		if (transform != null)
		{
			return transform.gameObject;
		}
		return null;
	}
}
