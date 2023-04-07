using com.ultrabit.bitheroes.model.utility;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.team;

public class TeamSortBtn : MonoBehaviour
{
	private Image[] image;

	private Material[] mat;

	private void Start()
	{
		image = GetComponentsInChildren<Image>();
		mat = new Material[image.Length];
		for (int i = 0; i < image.Length; i++)
		{
			mat[i] = image[i].material;
			image[i].material = new Material(mat[i]);
		}
	}

	public void Clicked()
	{
		for (int i = 0; i < image.Length; i++)
		{
			Util.adjustBrightness(image[i], 1.4f);
		}
	}

	public void Unclicked()
	{
		for (int i = 0; i < image.Length; i++)
		{
			Util.adjustBrightness(image[i]);
		}
	}

	private void OnDestroy()
	{
		for (int i = 0; i < image.Length; i++)
		{
			if (image[i] != null)
			{
				Object.Destroy(image[i].material);
			}
		}
	}
}
