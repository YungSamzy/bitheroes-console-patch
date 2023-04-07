using System.Collections.Generic;
using com.ultrabit.bitheroes.model.service;
using TMPro;

namespace com.ultrabit.bitheroes.ui.service;

public class ServiceListWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public ServicePanel _panel;

	private List<ServiceRef> _services;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(string name, List<ServiceRef> services)
	{
		_services = services;
		topperTxt.text = name;
		_panel.LoadDetails(services);
		_panel.ShowServicePanel();
		ListenForBack(OnClose);
		CreateWindow();
	}

	public void OnConsumablesBtn()
	{
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		_panel.DoEnable();
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		_panel.DoDisable();
	}
}
