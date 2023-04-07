using UnityEngine;

namespace com.ultrabit.bitheroes;

public class ScaleAdjustment : MonoBehaviour
{
	public GameObject objectToScale;

	public Vector2 referenceResolution;

	private float referenceScale;

	private Vector2 originalSize;

	private Vector2 newSize;

	private void Start()
	{
		originalSize = new Vector2(Screen.width, Screen.height);
		referenceScale = objectToScale.transform.localScale.x;
		Debug.Log(objectToScale.transform.localScale.x);
		if (originalSize.x != referenceResolution.x || originalSize.y != referenceResolution.y)
		{
			originalSize = new Vector2(Screen.width, Screen.height);
			float num = originalSize.x * referenceScale / referenceResolution.x;
			float num2 = originalSize.y * referenceScale / referenceResolution.y;
			float num3 = (num + num2) / 2f;
			objectToScale.transform.localScale = new Vector3(num3, num3, 1f);
			Debug.Log(num3);
		}
	}

	private void Update()
	{
		ChangeResolution();
	}

	private void ChangeResolution()
	{
		if (originalSize.x != (float)Screen.width || originalSize.y != (float)Screen.height)
		{
			originalSize = new Vector2(Screen.width, Screen.height);
			float num = originalSize.x * referenceScale / referenceResolution.x;
			float num2 = originalSize.y * referenceScale / referenceResolution.y;
			float num3 = (num + num2) / 2f;
			objectToScale.transform.localScale = new Vector3(num3, num3, 1f);
			Debug.Log(num3);
		}
	}
}
