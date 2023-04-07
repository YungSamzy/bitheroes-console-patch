using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.utility;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.game;

public class GameDispatcher : MonoBehaviour
{
	private const float fpsMeasurePeriod = 0.5f;

	private int m_FpsAccumulator;

	private float m_FpsNextPeriod;

	private int m_CurrentFps;

	private const string display = "{0} FPS";

	private const int SWF_FPS = 30;

	private float _time;

	[HideInInspector]
	public UnityCustomEvent FRAME_UPDATE = new UnityCustomEvent();

	public int currentFPS => m_CurrentFps;

	private void Awake()
	{
		_time = Time.realtimeSinceStartup * 1000f;
	}

	private void Start()
	{
		m_FpsNextPeriod = Time.realtimeSinceStartup + 0.5f;
	}

	private void Update()
	{
		m_FpsAccumulator++;
		if (Time.realtimeSinceStartup > m_FpsNextPeriod)
		{
			m_CurrentFps = (int)((float)m_FpsAccumulator / 0.5f);
			m_FpsAccumulator = 0;
			m_FpsNextPeriod += 0.5f;
		}
		float num = Time.realtimeSinceStartup * 1000f;
		float num2 = 1f;
		float num3 = 1f;
		if (m_CurrentFps != 30)
		{
			float num4 = 1000f / (float)m_CurrentFps;
			num2 = Util.roundToNearest(0.1f, (num - _time) / num4);
			num3 = num2 * (30f / (float)m_CurrentFps);
		}
		_time = num;
		FRAME_UPDATE.Invoke(new float[2] { num2, num3 });
	}
}
