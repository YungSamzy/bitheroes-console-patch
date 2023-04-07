using System.Collections;
using System.IO;
using UnityEngine;

public class PNGUploader : MonoBehaviour
{
	private IEnumerator Start()
	{
		yield return UploadPNG();
	}

	private IEnumerator UploadPNG()
	{
		yield return new WaitForEndOfFrame();
		int width = Screen.width;
		int height = Screen.height;
		Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGB24, mipChain: false);
		texture2D.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
		texture2D.Apply();
		byte[] bytes = texture2D.EncodeToPNG();
		Object.Destroy(texture2D);
		File.WriteAllBytes(Application.persistentDataPath + "/Saven3.png", bytes);
		Debug.Log(Application.persistentDataPath);
	}
}
