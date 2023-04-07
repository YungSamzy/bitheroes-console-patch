using TMPro;

namespace com.ultrabit.bitheroes.ui.utility;

public class TextWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI descTxt;

	public override void Start()
	{
		base.Start();
		base.SetNextScene = "";
	}

	public void LoadMessage(string title, string description)
	{
		Disable();
		topperTxt.text = title;
		descTxt.text = description;
		ListenForBack(OnClose);
		forceAnimation = true;
		CreateWindow();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
	}
}
