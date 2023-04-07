using UnityEngine;
using UnityEngine.UI;

public class LogButton : MonoBehaviour
{
	private Button loginBtn;

	public Text mail;

	public Text password;

	private void Start()
	{
		loginBtn = base.gameObject.GetComponent<Button>();
		loginBtn.onClick.AddListener(onClicked);
	}

	private void Update()
	{
	}

	private void onClicked()
	{
	}
}
