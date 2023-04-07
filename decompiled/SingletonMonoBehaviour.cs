using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour, ISingletonMonoBehaviour where T : MonoBehaviour
{
	private static T m_instance;

	public static T instance
	{
		get
		{
			bool flag = false;
			if (EqualityComparer<T>.Default.Equals(m_instance, null))
			{
				m_instance = Object.FindObjectOfType(typeof(T)) as T;
				flag = true;
			}
			if (EqualityComparer<T>.Default.Equals(m_instance, null))
			{
				m_instance = new GameObject().AddComponent(typeof(T)) as T;
				m_instance.name = typeof(T).Name;
				flag = true;
			}
			if (flag)
			{
				SingletonMonoBehaviours.AddInstance(m_instance);
			}
			return m_instance;
		}
	}

	public static bool exists => (Object)m_instance != (Object)null;
}
