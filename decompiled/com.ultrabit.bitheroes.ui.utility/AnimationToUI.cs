using com.ultrabit.bitheroes.core;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.utility;

public class AnimationToUI : MonoBehaviour
{
	private static readonly Vector3 START_POS = new Vector3(100000f, 100000f, 0f);

	private const int SPACE = 1000;

	private static int qty = 0;

	private const string UI_PREFIX = "UI_";

	private const string UI_FOLDER = "/ui/";

	public static Transform GetImageForAsset(string assetURL)
	{
		int startIndex = assetURL.LastIndexOf('/');
		string text = assetURL.Substring(startIndex);
		string text2 = assetURL.Replace(text, "/ui/" + text.Replace("/", "UI_"));
		Debug.LogError(text2);
		Transform transform = null;
		GameObject gameObjectAsset = GameData.instance.main.assetLoader.GetGameObjectAsset(text2);
		if (gameObjectAsset != null)
		{
			transform = gameObjectAsset.transform;
		}
		if (transform != null)
		{
			transform.position = new Vector3(START_POS.x + (float)(1000 * qty), START_POS.y, START_POS.z);
			qty++;
		}
		return transform;
	}
}
