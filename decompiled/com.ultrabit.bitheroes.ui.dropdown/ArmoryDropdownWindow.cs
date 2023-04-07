using TMPro;

namespace com.ultrabit.bitheroes.ui.dropdown;

public class ArmoryDropdownWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI messageTxt;

	public override void Start()
	{
		base.Start();
		Disable();
		ListenForBack(OnClose);
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

	public override void OnClose()
	{
		base.OnClose();
	}
}
