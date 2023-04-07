using UnityEngine;

public class WorldToScreenTester : MonoBehaviour
{
	public bool testing;

	private void Start()
	{
	}

	private void Update()
	{
		if (testing)
		{
			Debug.Log(base.transform.TransformVector(base.transform.position));
		}
	}
}
