namespace com.ultrabit.bitheroes.model.particle;

public class ParticleEmitterRef
{
	private ParticleRef _particleRef;

	private float _delayMin;

	private float _delayMax;

	public ParticleRef particleRef => _particleRef;

	public float delayMin => _delayMin;

	public float delayMax => _delayMax;

	public ParticleEmitterRef(ParticleRef particleRef, float delayMin, float delayMax)
	{
		_particleRef = particleRef;
		_delayMin = delayMin;
		_delayMax = delayMax;
	}
}
