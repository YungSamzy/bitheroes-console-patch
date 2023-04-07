using UnityEngine;

public class BaseManager<T> : SingletonMonoBehaviour<T>, IManager where T : BaseManager<T>
{
	private bool m_dontDestroyOnLoad;

	public void SetDirty()
	{
	}

	public void SetDontDestroyOnLoad()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		m_dontDestroyOnLoad = true;
	}

	public bool GetDontDestroyOnLoad()
	{
		return m_dontDestroyOnLoad;
	}
}
