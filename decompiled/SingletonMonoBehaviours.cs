using UnityEngine;

public class SingletonMonoBehaviours
{
	public delegate void MonoBehaviourDelegate(MonoBehaviour behaviour);

	public static event MonoBehaviourDelegate instanceAddedEvent;

	public static void AddInstance(MonoBehaviour instance)
	{
		if (SingletonMonoBehaviours.instanceAddedEvent != null)
		{
			SingletonMonoBehaviours.instanceAddedEvent(instance);
		}
	}
}
