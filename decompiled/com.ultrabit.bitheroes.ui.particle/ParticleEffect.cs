using com.ultrabit.bitheroes.model.utility;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.particle;

public class ParticleEffect : MonoBehaviour
{
	public string startColor = "0xFFFFCC";

	public string endColor = "0xFF0000";

	public int minParticles = 10;

	public int maxParticles = 15;

	public float minSize = 3f;

	public float maxSize = 7f;

	public float minDuration = 1000f;

	public float maxDuration = 2000f;

	public float minAlpha = 0.6f;

	public float maxAlpha = 0.9f;

	public float minVelocityX = -5f;

	public float maxVelocityX = 5f;

	public float minVelocityY = -5f;

	public float maxVelocityY = -20f;

	public float gravityX;

	public float gravityY = 1f;

	public float spread = 10f;

	public float dampening;

	private float _speed;

	private ParticleSystem system;

	private void Start()
	{
	}

	public void CreateParticles(float speed)
	{
		_speed = speed;
		system = GetComponent<ParticleSystem>();
		Util.RandomNumber(minSize, maxSize);
		float duration = Util.RandomNumber(minDuration, maxDuration);
		Util.RandomNumber(0f - spread, spread);
		Util.RandomNumber(0f - spread, spread);
		ColorUtility.TryParseHtmlString(startColor, out var color);
		color.a = minAlpha;
		Color max = new Color(color.r, color.g, color.b, maxAlpha);
		ColorUtility.TryParseHtmlString(endColor, out var color2);
		color2.a = minAlpha;
		ParticleSystem.MainModule main = system.main;
		main.startSize = new ParticleSystem.MinMaxCurve(minSize, maxSize);
		main.duration = duration;
		main.startColor = new ParticleSystem.MinMaxGradient(color, max);
		main.startSpeed = new ParticleSystem.MinMaxCurve(Util.RandomNumber(minVelocityX, maxVelocityX), Util.RandomNumber(minVelocityY, maxVelocityY));
		main.gravityModifier = new ParticleSystem.MinMaxCurve(gravityX, gravityY);
		system.Play();
	}
}
