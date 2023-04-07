using UnityEngine;

public class SpritePlusSample : MonoBehaviour
{
	private MaterialPropertyBlock _material;

	private void OnEnable()
	{
		_material = new MaterialPropertyBlock();
	}

	private void Start()
	{
		GetComponent<Renderer>().GetPropertyBlock(_material);
		_material.SetFloat("_Hue", 0.5f);
		_material.SetFloat("_Negative", 1f);
		GetComponent<Renderer>().SetPropertyBlock(_material);
	}
}
