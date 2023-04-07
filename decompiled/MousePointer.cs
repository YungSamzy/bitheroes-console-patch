using com.ultrabit.bitheroes.core;
using UnityEngine;

public class MousePointer : MonoBehaviour
{
	private float delay = 0.7f;

	private void Start()
	{
		GameData.instance.audioManager.PlaySoundLink("clickindicator");
		GetComponent<Animator>().SetTrigger("Click");
		Object.Destroy(base.gameObject, GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length - delay);
	}
}
