using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.playervoting;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.playervoting;

public class PlayerVotingWindow : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public Image loadingIcon;

	public TMP_InputField searchTxt;

	public Image viewDropdown;

	private Dictionary<int, int> _votesCasted = new Dictionary<int, int>();

	public override void Start()
	{
		base.Start();
		Disable();
		ShowLoading();
		if (PlayerVotingBook.activeVoting)
		{
			PlayerVotingDALC.instance.getMyVotes(1);
			PlayerVotingDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(1), OnGetMyVotes);
		}
		topperTxt.text = PlayerVotingBook.name;
		searchTxt.text = "";
		DoGetList();
		CreateWindow();
	}

	public void OnViewDropdown()
	{
		GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_view"));
	}

	public void OnSearchChange()
	{
	}

	private void OnGetMyVotes(BaseEvent e)
	{
		ISFSArray sFSArray = (e as DALCEvent).sfsob.GetSFSArray("vot3");
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			ISFSObject sFSObject = sFSArray.GetSFSObject(i);
			int @int = sFSObject.GetInt("vot1");
			int int2 = sFSObject.GetInt("vot2");
			_votesCasted[@int] = int2;
		}
		DoGetList();
		ListenForBack(OnClose);
	}

	public void DoGetList()
	{
		ShowLoading();
		HideLoading();
	}

	private void ShowLoading()
	{
		searchTxt.gameObject.SetActive(value: false);
		GameData.instance.main.ShowLoading();
	}

	private void HideLoading()
	{
		searchTxt.gameObject.SetActive(value: true);
		loadingIcon.gameObject.SetActive(value: false);
		GameData.instance.main.HideLoading();
	}

	public override void DoDestroy()
	{
		PlayerVotingDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(1), OnGetMyVotes);
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		searchTxt.interactable = true;
		viewDropdown.GetComponent<EventTrigger>().enabled = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		searchTxt.interactable = false;
		viewDropdown.GetComponent<EventTrigger>().enabled = false;
	}
}
