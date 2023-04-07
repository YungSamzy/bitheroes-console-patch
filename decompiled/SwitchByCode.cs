using UnityEngine;

public class SwitchByCode : MonoBehaviour
{
	public Texture anotherTexture;

	private MaterialPropertyBlock _material;

	private void OnEnable()
	{
		_material = new MaterialPropertyBlock();
	}

	private void Start()
	{
		GetComponent<Renderer>().GetPropertyBlock(_material);
		_material.SetTexture("_SwitchTex", anotherTexture);
		_material.SetFloat("_Switch", 1f);
		_material.SetFloat("_Hue", 0.5f);
		_material.SetFloat("_Negative", 1f);
		GetComponent<Renderer>().SetPropertyBlock(_material);
	}
}
