using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using TMPro;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.admin;

public class AdminUploadWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public Button addBtn;

	public Button uploadBtn;

	public override void Start()
	{
		base.Start();
		Disable();
		topperTxt.text = Language.GetString("ui_xmls");
		uploadBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_upload");
		addBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_add");
		ListenForBack(OnClose);
		CreateWindow();
	}

	public void OnAddBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
	}

	public void OnUpdateBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		addBtn.interactable = true;
		uploadBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		addBtn.interactable = false;
		uploadBtn.interactable = false;
	}
}
