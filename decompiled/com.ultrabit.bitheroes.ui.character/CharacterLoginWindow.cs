using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.character;

public class CharacterLoginWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI guestNameTxt;

	public TextMeshProUGUI kongregateNameTxt;

	public TextMeshProUGUI emailNameTxt;

	public Button guestBtn;

	public Button kongregateBtn;

	public Button emailBtn;

	public Button googleBtn;

	public Button facebookBtn;

	public override void Start()
	{
		base.Start();
		Disable();
		string @string = Language.GetString("ui_login");
		topperTxt.text = @string;
		guestNameTxt.text = Language.GetString("ui_guest");
		kongregateNameTxt.text = Language.GetString("ui_kongregate");
		emailNameTxt.text = Language.GetString("ui_email");
		if (guestBtn != null)
		{
			guestBtn.GetComponentInChildren<TextMeshProUGUI>().text = @string;
			guestBtn.onClick.AddListener(OnGuestBtn);
			if (!AppInfo.IsMobile())
			{
				Util.SetButton(guestBtn, enabled: false);
			}
		}
		if (googleBtn != null)
		{
			googleBtn.GetComponentInChildren<TextMeshProUGUI>().text = @string;
			googleBtn.onClick.AddListener(OnGoogleBtn);
			Util.SetButton(googleBtn, enabled: false);
		}
		if (kongregateBtn != null)
		{
			kongregateBtn.GetComponentInChildren<TextMeshProUGUI>().text = @string;
			kongregateBtn.onClick.AddListener(OnKongregateBtn);
			Util.SetButton(kongregateBtn, enabled: false);
		}
		if (facebookBtn != null)
		{
			facebookBtn.GetComponentInChildren<TextMeshProUGUI>().text = @string;
			facebookBtn.onClick.AddListener(OnFacebookBtn);
			Util.SetButton(facebookBtn, enabled: false);
		}
		if (emailBtn != null)
		{
			emailBtn.GetComponentInChildren<TextMeshProUGUI>().text = @string;
			emailBtn.onClick.AddListener(OnEmailBtn);
		}
	}

	public void OnGuestBtn()
	{
	}

	public void OnGoogleBtn()
	{
	}

	public void OnKongregateBtn()
	{
	}

	public void OnFacebookBtn()
	{
	}

	public void OnEmailBtn()
	{
	}

	public override void DoDestroy()
	{
		if (guestBtn != null)
		{
			guestBtn.onClick.RemoveListener(OnGuestBtn);
		}
		if (googleBtn != null)
		{
			googleBtn.onClick.RemoveListener(OnGoogleBtn);
		}
		if (kongregateBtn != null)
		{
			kongregateBtn.onClick.RemoveListener(OnKongregateBtn);
		}
		if (facebookBtn != null)
		{
			facebookBtn.onClick.RemoveListener(OnFacebookBtn);
		}
		if (emailBtn != null)
		{
			emailBtn.onClick.RemoveListener(OnEmailBtn);
		}
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		if (guestBtn != null)
		{
			guestBtn.interactable = true;
		}
		if (googleBtn != null)
		{
			googleBtn.interactable = true;
		}
		if (kongregateBtn != null)
		{
			kongregateBtn.interactable = true;
		}
		if (facebookBtn != null)
		{
			facebookBtn.interactable = true;
		}
		if (emailBtn != null)
		{
			emailBtn.interactable = true;
		}
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		if (guestBtn != null)
		{
			guestBtn.interactable = false;
		}
		if (googleBtn != null)
		{
			googleBtn.interactable = false;
		}
		if (kongregateBtn != null)
		{
			kongregateBtn.interactable = false;
		}
		if (facebookBtn != null)
		{
			facebookBtn.interactable = false;
		}
		if (emailBtn != null)
		{
			emailBtn.interactable = false;
		}
	}
}
