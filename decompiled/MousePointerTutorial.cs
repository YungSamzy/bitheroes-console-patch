using UnityEngine;

public class MousePointerTutorial : MonoBehaviour
{
	private float delay = 0.7f;

	private void Start()
	{
		GetComponent<Animator>().SetTrigger("Click");
	}
}
