using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.utility;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.game;

public class GameSettingsSupportPanel : MonoBehaviour
{
	public GameObject gameSettingsSupportContent;

	private void Start()
	{
	}

	public void OnPrivacyBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		Util.OpenURL("https://www.kongregate.com/privacy");
	}

	public void OnTermsOfUseBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		Util.OpenURL("https://www.kongregate.com/user-agreement");
	}

	public void OnDeleteAccountBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		Util.OpenURL("https://privacyportal-de.onetrust.com/webform/4a6c7306-a5a2-45b8-95fc-64a4ce3a6384/80f079fc-1996-4bff-b7e2-dd9ac1d32e80");
	}

	public void OnCustomerSupportBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		Util.OpenURL("https://bitheroes.zendesk.com/hc/en-us");
	}

	public void Show()
	{
		base.gameObject.SetActive(value: true);
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}
}
