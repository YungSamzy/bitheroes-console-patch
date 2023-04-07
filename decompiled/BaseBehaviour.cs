using UnityEngine;

public abstract class BaseBehaviour : MonoBehaviour
{
	private Transform cashedTransform;

	public Transform CachedTransform => cashedTransform ?? (cashedTransform = base.transform);

	protected virtual void Awake()
	{
	}

	protected virtual void Start()
	{
	}

	protected virtual void OnDestroy()
	{
	}
}
