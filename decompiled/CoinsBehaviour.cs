using UnityEngine;

public class CoinsBehaviour : MonoBehaviour
{
	public ParticleSystem Coins;

	public void PlayCoinsAnimation()
	{
		Coins.Play();
	}

	public void StopCoinsAnimation()
	{
		Coins.Stop();
	}
}
