using UnityEngine;

public class PukeController : MonoBehaviour
{
	public ParticleSystem particleSystem;

	private void Play()
	{
		particleSystem.Clear();
		particleSystem.Play();
	}
}
