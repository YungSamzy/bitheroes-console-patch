using TMPro;
using UnityEngine;

public class TextOnStage : MonoBehaviour
{
	private TextMeshPro tmp;

	private TextMeshProUGUI tmpUI;

	private string _text;

	private Animation anim;

	public void Set(string text, string colorText)
	{
		_text = "<color=" + colorText + ">" + text + "</color>";
		if ((bool)GetComponent<TextMeshPro>())
		{
			GetComponent<TextMeshPro>().text = _text;
		}
		if ((bool)GetComponent<TextMeshProUGUI>())
		{
			GetComponent<TextMeshProUGUI>().text = _text;
		}
		anim = GetComponent<Animation>();
		base.transform.localPosition = default(Vector3);
		AnimationClip animationClip = new AnimationClip();
		animationClip.legacy = true;
		AnimationCurve curve = new AnimationCurve(new Keyframe(0f, base.transform.localPosition.y), new Keyframe(0.1f, base.transform.localPosition.y), new Keyframe(0.25f, base.transform.localPosition.y + 1f), new Keyframe(2f, base.transform.localPosition.y + 8f));
		animationClip.SetCurve("", typeof(Transform), "localPosition.y", curve);
		AnimationCurve curve2 = new AnimationCurve(new Keyframe(0f, base.transform.localPosition.x), new Keyframe(2f, base.transform.localPosition.x));
		animationClip.SetCurve("", typeof(Transform), "localPosition.x", curve2);
		AnimationCurve curve3 = new AnimationCurve(new Keyframe(0f, -30f), new Keyframe(2f, -25f));
		animationClip.SetCurve("", typeof(Transform), "localPosition.z", curve3);
		anim.AddClip(animationClip, animationClip.name);
		anim.Play(animationClip.name);
	}

	public void Destroy()
	{
		base.gameObject.SetActive(value: false);
	}
}
