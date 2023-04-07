using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.guild;

public class GuildWindow : WindowsMain
{
	public const int TAB_MAIN = 0;

	public const int TAB_MEMBERS = 1;

	public const int TAB_SHOP = 2;

	public const int TAB_EVENTS = 3;

	public TextMeshProUGUI topperTxt;

	public Button mainBtn;

	public Button membersBtn;

	public Button shopBtn;

	public Button eventsBtn;

	public GuildMainPanel guildMainPanel;

	public GuildMembersPanel guildMembersPanel;

	public GuildShopPanel guildShopPanel;

	public GuildEventsPanel guildEventsPanel;

	[HideInInspector]
	public bool guildMembersRefreshPending;

	private Color alpha = new Color(1f, 1f, 1f, 0.5f);

	private int _currentTab = -1;

	private new int _sortingLayer;

	public int currentTab => _currentTab;

	public new int sortingLayer => _sortingLayer;

	public override void Start()
	{
		base.Start();
	}

	public void LoadDetails()
	{
		topperTxt.text = GameData.instance.PROJECT.character.guildData.name;
		mainBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_main");
		membersBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_members");
		shopBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_shop");
		eventsBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_events");
		guildMainPanel.LoadDetails(this);
		guildMembersPanel.LoadDetails(this);
		guildShopPanel.LoadDetails();
		guildEventsPanel.LoadDetails(this);
		GameData.instance.PROJECT.PauseDungeon();
		SetTab(0);
		ListenForBack(OnClose);
		CreateWindow();
	}

	public override void UpdateSortingLayers(int layer)
	{
		base.UpdateSortingLayers(layer);
		_sortingLayer = layer;
		guildMembersPanel.UpdateLayers();
	}

	public void OnMainBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SetTab(0);
	}

	public void OnMembersBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SetTab(1);
	}

	public void OnShopBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SetTab(2);
	}

	public void OnEventsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		SetTab(3);
	}

	private void SetTab(int tab)
	{
		switch (tab)
		{
		case 0:
			_currentTab = 0;
			AlphaTabs();
			mainBtn.image.color = Color.white;
			mainBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
			mainBtn.enabled = false;
			guildMainPanel.Show();
			guildMembersPanel.Hide();
			guildShopPanel.Hide();
			guildEventsPanel.Hide();
			break;
		case 1:
			_currentTab = 1;
			AlphaTabs();
			membersBtn.image.color = Color.white;
			membersBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
			membersBtn.enabled = false;
			guildMainPanel.Hide();
			guildMembersPanel.Show();
			guildShopPanel.Hide();
			guildEventsPanel.Hide();
			if (guildMembersRefreshPending)
			{
				guildMembersRefreshPending = false;
				guildMembersPanel.CreateList();
			}
			break;
		case 2:
			_currentTab = 2;
			AlphaTabs();
			shopBtn.image.color = Color.white;
			shopBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
			shopBtn.enabled = false;
			guildMainPanel.Hide();
			guildMembersPanel.Hide();
			guildShopPanel.Show();
			guildEventsPanel.Hide();
			break;
		case 3:
			_currentTab = 3;
			AlphaTabs();
			eventsBtn.image.color = Color.white;
			eventsBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
			eventsBtn.enabled = false;
			guildMainPanel.Hide();
			guildMembersPanel.Hide();
			guildShopPanel.Hide();
			guildEventsPanel.Show();
			break;
		}
	}

	private void AlphaTabs()
	{
		mainBtn.image.color = alpha;
		mainBtn.GetComponentInChildren<TextMeshProUGUI>().color = alpha;
		mainBtn.enabled = true;
		membersBtn.image.color = alpha;
		membersBtn.GetComponentInChildren<TextMeshProUGUI>().color = alpha;
		membersBtn.enabled = true;
		shopBtn.image.color = alpha;
		shopBtn.GetComponentInChildren<TextMeshProUGUI>().color = alpha;
		shopBtn.enabled = true;
		eventsBtn.image.color = alpha;
		eventsBtn.GetComponentInChildren<TextMeshProUGUI>().color = alpha;
		eventsBtn.enabled = true;
	}

	public override void DoDestroy()
	{
		GameData.instance.PROJECT.ResumeDungeon();
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		mainBtn.interactable = true;
		membersBtn.interactable = true;
		shopBtn.interactable = true;
		eventsBtn.interactable = true;
		guildMembersPanel.DoEnable();
		guildEventsPanel.DoEnable();
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		mainBtn.interactable = false;
		membersBtn.interactable = false;
		shopBtn.interactable = false;
		eventsBtn.interactable = false;
		guildMainPanel.DoDisable();
		guildMembersPanel.DoDisable();
		guildShopPanel.DoDisable();
		guildEventsPanel.DoDisable();
	}
}
