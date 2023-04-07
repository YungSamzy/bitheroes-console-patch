using UnityEngine;
using UnityEngine.UI;

public class EntitiesCount : MonoBehaviour
{
	public Transform holder;

	private Text text;

	private void Update()
	{
		if (text == null)
		{
			text = GetComponent<Text>();
		}
		text.text = holder.childCount.ToString();
	}
}
