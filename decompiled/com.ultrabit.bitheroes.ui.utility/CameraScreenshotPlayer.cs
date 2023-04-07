using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.utility;

public class CameraScreenshotPlayer : MonoBehaviour
{
	[SerializeField]
	private Camera playerCamera;

	[SerializeField]
	private RawImage PlayerCameraView;

	private void Start()
	{
		ChangingViewPlayer();
	}

	public void ChangingViewPlayer()
	{
		StartCoroutine(SaveCameraView());
	}

	public IEnumerator SaveCameraView()
	{
		playerCamera.gameObject.SetActive(value: true);
		yield return new WaitForEndOfFrame();
		PlayerCameraView.texture = (RenderTexture.active = playerCamera.targetTexture);
		playerCamera.Render();
		playerCamera.gameObject.SetActive(value: false);
	}
}
