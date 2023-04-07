using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.rarity;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.language;
using com.ultrabit.bitheroes.ui.lists.dropdownlist;
using com.ultrabit.bitheroes.ui.menu;
using com.ultrabit.bitheroes.ui.utility;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.game;

public class GameSettingsGeneralPanel : MonoBehaviour
{
	public GameObject gameSettingsPanelContent;

	public Transform checkBoxPrefab;

	public Transform sliderPrefab;

	public Transform dropdownPrefab;

	public Transform textPrefab;

	public Transform container;

	public AudioMixer masterAudioMixer;

	private bool _showHelm = true;

	private CheckBoxTile _showHelmCheckbox;

	private bool _showMount = true;

	private CheckBoxTile _showMountCheckbox;

	private bool _showBody = true;

	private CheckBoxTile _showBodyCheckbox;

	private bool _showAccessory = true;

	private CheckBoxTile _showAccessoryCheckbox;

	private CheckBoxTile _notificationsCheckbox;

	private List<CheckBoxTile> _notificationTypesCheckboxes;

	private CheckBoxTile _notificationsFriendCheckbox;

	private CheckBoxTile _notificationsGuildCheckbox;

	private CheckBoxTile _notificationsFamiliarsCheckbox;

	private CheckBoxTile _notificationsFusionsCheckbox;

	private CheckBoxTile _notificationsCraftCheckbox;

	private CheckBoxTile _notificationsOtherCheckbox;

	private CheckBoxTile _appNotificationsCheckbox;

	private bool _appNotificationsDisabled;

	private bool _friendRequests = true;

	private CheckBoxTile _friendRequestsCheckbox;

	private bool _duelRequests = true;

	private CheckBoxTile _duelRequestsCheckbox;

	private bool _chatEnabled = true;

	private CheckBoxTile _chatEnabledCheckbox;

	private bool _autoPilot;

	private CheckBoxTile _autoPilotCheckbox;

	private CheckBoxTile _autoPilotDeathDisableCheckbox;

	private CheckBoxTile _autoEnrageCheckbox;

	private CheckBoxTile _adsCheckbox;

	private CheckBoxTile _reducedEffects;

	private CheckBoxTile _battleTextCheckbox;

	private CheckBoxTile _battleBarOverlayCheckbox;

	private CheckBoxTile _animationsCheckbox;

	private List<CheckBoxTile> _animationsTypesCheckboxes;

	private CheckBoxTile _declineFamiliarDupesCheckbox;

	private List<CheckBoxTile> _declineFamiliarRaritiesCheckboxes;

	private CheckBoxTile _declineMerchantsCheckbox;

	private List<CheckBoxTile> _declineMerchantsRaritiesCheckboxes;

	private CheckBoxTile _declineTreasuresCheckbox;

	private CheckBoxTile _ignoreShrinesCheckbox;

	private CheckBoxTile _ignoreBossCheckbox;

	private CheckBoxTile _brawlRequestsCheckbox;

	private CheckBoxTile _brawlRequestsFriendCheckbox;

	private CheckBoxTile _brawlRequestsGuildCheckbox;

	private CheckBoxTile _brawlRequestsOtherCheckbox;

	private CheckBoxTile _windowedCheckbox;

	private Slider _musicSlider;

	private Slider _soundSlider;

	private Image _resolutionDropdown;

	private CheckBoxTile _hideLevelChatCheckbox;

	private CheckBoxTile _defeatAdvices;

	private CheckBoxTile _allowEquipOnResults;

	private List<CheckBoxTile> _allowEquipOnResultsBattleTypes;

	private CheckBoxTile _showBattleBGCheckbox;

	private CheckBoxTile _persuaseFamiliarsGoldCheckbox;

	private CheckBoxTile _persuaseFamiliarsGemsCheckbox;

	private List<CheckBoxTile> _persuaseFamiliarsGoldRaritiesCheckboxes;

	private List<CheckBoxTile> _persuaseFamiliarsGemsRaritiesCheckboxes;

	private AsianLanguageFontManager asianLangManager;

	private bool _showNameplate = true;

	private CheckBoxTile _showNameplateCheckbox;

	private Vector3 scaleTo1 = new Vector3(1f, 1f, 1f);

	private Color alpha = new Color(1f, 1f, 1f, 0.5f);

	private float listWidth;

	private List<CheckBoxTile> activeCheckBoxes = new List<CheckBoxTile>();

	private Transform window;

	private int resolutionSelected;

	private void Start()
	{
		_showHelm = GameData.instance.PROJECT.character == null || GameData.instance.PROJECT.character.showHelm;
		_showMount = GameData.instance.PROJECT.character == null || GameData.instance.PROJECT.character.showMount;
		_showBody = GameData.instance.PROJECT.character == null || GameData.instance.PROJECT.character.showBody;
		_showAccessory = GameData.instance.PROJECT.character == null || GameData.instance.PROJECT.character.showAccessory;
		_friendRequests = GameData.instance.PROJECT.character == null || GameData.instance.PROJECT.character.friendRequestsEnabled;
		_duelRequests = GameData.instance.PROJECT.character == null || GameData.instance.PROJECT.character.duelRequestsEnabled;
		_chatEnabled = GameData.instance.PROJECT.character == null || GameData.instance.PROJECT.character.chatEnabled;
		_autoPilot = GameData.instance.PROJECT.character != null && GameData.instance.PROJECT.character.autoPilot;
		_showNameplate = GameData.instance.PROJECT.character == null || GameData.instance.PROJECT.character.extraInfo.showNameplate();
		_appNotificationsDisabled = GameData.instance.SAVE_STATE.appNotificationsDisabled;
		listWidth = GetComponent<RectTransform>().sizeDelta.x;
		CreateSliders();
		CreateCheckboxes();
		Transform obj = Object.Instantiate(textPrefab);
		obj.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		obj.GetComponent<TextMeshProUGUI>().text = UnityCloudBuildManifest.GetInfoText() ?? "";
		RectTransform component = obj.GetComponent<RectTransform>();
		component.sizeDelta = new Vector2(60f, component.sizeDelta.y);
		asianLangManager = base.gameObject.GetComponent<AsianLanguageFontManager>();
		if (asianLangManager == null)
		{
			asianLangManager = base.gameObject.AddComponent<AsianLanguageFontManager>();
		}
		if (asianLangManager != null)
		{
			asianLangManager.SetAsianFontsIfNeeded();
		}
	}

	private void CreateSliders()
	{
		Transform transform = Object.Instantiate(sliderPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<SliderTile>().Create(0f, 100f, Language.GetString("ui_music"), Mathf.RoundToInt(GameData.instance.SAVE_STATE.musicVolume * 100f), flip: false, "Music", base.gameObject);
		_musicSlider = transform.GetComponent<SliderTile>().slider;
		transform = null;
		transform = Object.Instantiate(sliderPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<SliderTile>().Create(0f, 100f, Language.GetString("ui_sound"), Mathf.RoundToInt(GameData.instance.SAVE_STATE.soundVolume * 100f), flip: false, "Sound", base.gameObject);
		_soundSlider = transform.GetComponent<SliderTile>().slider;
	}

	public void OnSliderChangeMusic()
	{
		GameData.instance.SAVE_STATE.musicVolume = _musicSlider.value / 100f;
		GameData.instance.audioManager.UpdateVolume(GameData.instance.SAVE_STATE.musicVolume);
	}

	public void OnSliderChangeSound()
	{
		GameData.instance.SAVE_STATE.soundVolume = _soundSlider.value / 100f;
	}

	public void SetSoundVol(float vol)
	{
		vol = (1f - Mathf.Sqrt(vol)) * -80f;
		masterAudioMixer.SetFloat("soundVol", vol);
	}

	private void CreateCheckboxes()
	{
		int num = 6;
		Transform transform;
		if (AppInfo.resizable)
		{
			transform = null;
			if (AppInfo.IsDesktop())
			{
				transform = Object.Instantiate(checkBoxPrefab);
				transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
				transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_windowed")), !GameData.instance.SAVE_STATE.fullscreen, changable: true, listWidth, "Windowed", base.gameObject);
				_windowedCheckbox = transform.GetComponent<CheckBoxTile>();
				activeCheckBoxes.Add(_windowedCheckbox);
				Vector2 vector = new Vector2(GameData.instance.SAVE_STATE.resolutionX, GameData.instance.SAVE_STATE.resolutionY);
				if (vector.x <= 0f || vector.y <= 0f)
				{
					vector = new Vector2(Screen.width, Screen.height);
				}
				Transform transform2 = Object.Instantiate(container);
				transform2.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
				transform = null;
				transform = Object.Instantiate(dropdownPrefab);
				transform.SetParent(transform2.transform, worldPositionStays: false);
				transform2.GetComponent<RectTransform>().sizeDelta = new Vector2(listWidth, transform.GetComponent<RectTransform>().sizeDelta.y);
				transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(transform2.GetComponent<RectTransform>().sizeDelta.x * -0.46f + transform.GetComponent<RectTransform>().sizeDelta.x * 0.5f, 0f);
				_resolutionDropdown = transform.GetComponent<Image>();
				_resolutionDropdown.GetComponentInChildren<TextMeshProUGUI>().text = vector.x + " x " + vector.y;
				EventTrigger component = _resolutionDropdown.GetComponent<EventTrigger>();
				EventTrigger.Entry entry = new EventTrigger.Entry();
				entry.eventID = EventTriggerType.PointerClick;
				entry.callback.AddListener(delegate(BaseEventData data)
				{
					OnResolutionDropdown((PointerEventData)data);
				});
				component.triggers.Add(entry);
			}
		}
		transform = null;
		transform = Object.Instantiate(checkBoxPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_auto_pilot_death_disable")), GameData.instance.SAVE_STATE.autoPilotDeathDisable, changable: true, listWidth, "AutoPilotDeath", base.gameObject);
		_autoPilotDeathDisableCheckbox = transform.GetComponent<CheckBoxTile>();
		activeCheckBoxes.Add(_autoPilotDeathDisableCheckbox);
		transform = null;
		transform = Object.Instantiate(checkBoxPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_ignore_boss")), GameData.instance.SAVE_STATE.ignoreBoss, changable: true, listWidth, "IgnoreBoss", base.gameObject);
		_ignoreBossCheckbox = transform.GetComponent<CheckBoxTile>();
		activeCheckBoxes.Add(_ignoreBossCheckbox);
		transform = null;
		transform = Object.Instantiate(checkBoxPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_ignore_shrines")), GameData.instance.SAVE_STATE.ignoreShrines, changable: true, listWidth, "IgnoreShrines", base.gameObject);
		_ignoreShrinesCheckbox = transform.GetComponent<CheckBoxTile>();
		activeCheckBoxes.Add(_ignoreShrinesCheckbox);
		transform = null;
		transform = Object.Instantiate(checkBoxPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_shownameplate")), _showNameplate, changable: true, listWidth, "ShowNameplate");
		_showNameplateCheckbox = transform.GetComponent<CheckBoxTile>();
		activeCheckBoxes.Add(_showNameplateCheckbox);
		transform = null;
		transform = Object.Instantiate(checkBoxPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_show_helm")), _showHelm, changable: true, listWidth, "ShowHelm");
		_showHelmCheckbox = transform.GetComponent<CheckBoxTile>();
		activeCheckBoxes.Add(_showHelmCheckbox);
		transform = null;
		transform = Object.Instantiate(checkBoxPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_show_body")), _showBody, changable: true, listWidth, "ShowBody");
		_showBodyCheckbox = transform.GetComponent<CheckBoxTile>();
		activeCheckBoxes.Add(_showBodyCheckbox);
		if (GameData.instance.PROJECT.character.hasAccessoryOrHad())
		{
			transform = null;
			transform = Object.Instantiate(checkBoxPrefab);
			transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
			transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_show_accessory")), _showAccessory, changable: true, listWidth, "ShowAccessory");
			_showAccessoryCheckbox = transform.GetComponent<CheckBoxTile>();
			activeCheckBoxes.Add(_showAccessoryCheckbox);
		}
		if (GameData.instance.PROJECT.character.mounts.mounts.Count > 0)
		{
			transform = null;
			transform = Object.Instantiate(checkBoxPrefab);
			transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
			transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_show_mount")), _showMount, changable: true, listWidth, "ShowMount");
			_showMountCheckbox = transform.GetComponent<CheckBoxTile>();
			activeCheckBoxes.Add(_showMountCheckbox);
		}
		transform = null;
		transform = Object.Instantiate(checkBoxPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_notifications")), !GameData.instance.SAVE_STATE.notificationsDisabled, changable: true, listWidth, "Notifications", base.gameObject);
		_notificationsCheckbox = transform.GetComponent<CheckBoxTile>();
		activeCheckBoxes.Add(_notificationsCheckbox);
		_notificationTypesCheckboxes = new List<CheckBoxTile>();
		transform = null;
		transform = Object.Instantiate(checkBoxPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_friends")), GameData.instance.SAVE_STATE.notificationsFriend, changable: true, listWidth - (float)num, "NotificationsFriend", base.gameObject);
		_notificationsFriendCheckbox = transform.GetComponent<CheckBoxTile>();
		activeCheckBoxes.Add(_notificationsFriendCheckbox);
		_notificationTypesCheckboxes.Add(_notificationsFriendCheckbox);
		transform = null;
		transform = Object.Instantiate(checkBoxPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_guild")), GameData.instance.SAVE_STATE.notificationsGuild, changable: true, listWidth - (float)num, "NotificationsGuild", base.gameObject);
		_notificationsGuildCheckbox = transform.GetComponent<CheckBoxTile>();
		activeCheckBoxes.Add(_notificationsGuildCheckbox);
		_notificationTypesCheckboxes.Add(_notificationsGuildCheckbox);
		transform = null;
		transform = Object.Instantiate(checkBoxPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(ItemRef.GetItemNamePlural(6)), GameData.instance.SAVE_STATE.notificationsFamiliars, changable: true, listWidth - (float)num, "NotificationsFamiliars", base.gameObject);
		_notificationsFamiliarsCheckbox = transform.GetComponent<CheckBoxTile>();
		activeCheckBoxes.Add(_notificationsFamiliarsCheckbox);
		_notificationTypesCheckboxes.Add(_notificationsFamiliarsCheckbox);
		transform = null;
		transform = Object.Instantiate(checkBoxPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(ItemRef.GetItemNamePlural(7)), GameData.instance.SAVE_STATE.notificationsFusions, changable: true, listWidth - (float)num, "NotificationsFusions", base.gameObject);
		_notificationsFusionsCheckbox = transform.GetComponent<CheckBoxTile>();
		activeCheckBoxes.Add(_notificationsFusionsCheckbox);
		_notificationTypesCheckboxes.Add(_notificationsFusionsCheckbox);
		transform = null;
		transform = Object.Instantiate(checkBoxPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_craft")), GameData.instance.SAVE_STATE.notificationsCraft, changable: true, listWidth - (float)num, "NotificationsCraft", base.gameObject);
		_notificationsCraftCheckbox = transform.GetComponent<CheckBoxTile>();
		activeCheckBoxes.Add(_notificationsCraftCheckbox);
		_notificationTypesCheckboxes.Add(_notificationsCraftCheckbox);
		transform = null;
		transform = Object.Instantiate(checkBoxPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_other")), GameData.instance.SAVE_STATE.notificationsOther, changable: true, listWidth - (float)num, "NotificationsOther", base.gameObject);
		_notificationsOtherCheckbox = transform.GetComponent<CheckBoxTile>();
		activeCheckBoxes.Add(_notificationsOtherCheckbox);
		_notificationTypesCheckboxes.Add(_notificationsOtherCheckbox);
		if (AppInfo.allowAppNotifications)
		{
			transform = null;
			transform = Object.Instantiate(checkBoxPrefab);
			transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
			transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_app_notifications")), !_appNotificationsDisabled, changable: true, listWidth, "AppNotifications", base.gameObject);
			_appNotificationsCheckbox = transform.GetComponent<CheckBoxTile>();
			activeCheckBoxes.Add(_appNotificationsCheckbox);
		}
		transform = null;
		transform = Object.Instantiate(checkBoxPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_friend_requests")), _friendRequests, changable: true, listWidth, "FriendRequests");
		_friendRequestsCheckbox = transform.GetComponent<CheckBoxTile>();
		activeCheckBoxes.Add(_friendRequestsCheckbox);
		transform = null;
		transform = Object.Instantiate(checkBoxPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_duel_requests")), _duelRequests, changable: true, listWidth, "DuelRequests");
		_duelRequestsCheckbox = transform.GetComponent<CheckBoxTile>();
		activeCheckBoxes.Add(_duelRequestsCheckbox);
		transform = null;
		transform = Object.Instantiate(checkBoxPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_brawl_requests")), GameData.instance.SAVE_STATE.brawlRequests, changable: true, listWidth, "BrawlRequests", base.gameObject);
		_brawlRequestsCheckbox = transform.GetComponent<CheckBoxTile>();
		activeCheckBoxes.Add(_brawlRequestsCheckbox);
		transform = null;
		transform = Object.Instantiate(checkBoxPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_friends")), GameData.instance.SAVE_STATE.brawlRequestsFriend, changable: true, listWidth - (float)num, "BrawlRequestsFriend", base.gameObject);
		_brawlRequestsFriendCheckbox = transform.GetComponent<CheckBoxTile>();
		activeCheckBoxes.Add(_brawlRequestsFriendCheckbox);
		transform = null;
		transform = Object.Instantiate(checkBoxPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_guild")), GameData.instance.SAVE_STATE.brawlRequestsGuild, changable: true, listWidth - (float)num, "BrawlRequestsGuild", base.gameObject);
		_brawlRequestsGuildCheckbox = transform.GetComponent<CheckBoxTile>();
		activeCheckBoxes.Add(_brawlRequestsGuildCheckbox);
		transform = null;
		transform = Object.Instantiate(checkBoxPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_other")), GameData.instance.SAVE_STATE.brawlRequestsOther, changable: true, listWidth - (float)num, "BrawlRequestsOther", base.gameObject);
		_brawlRequestsOtherCheckbox = transform.GetComponent<CheckBoxTile>();
		activeCheckBoxes.Add(_brawlRequestsOtherCheckbox);
		transform = null;
		transform = Object.Instantiate(checkBoxPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_auto_pilot")), _autoPilot, changable: true, listWidth, "AutoPilot");
		_autoPilotCheckbox = transform.GetComponent<CheckBoxTile>();
		activeCheckBoxes.Add(_autoPilotCheckbox);
		transform = null;
		transform = Object.Instantiate(checkBoxPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_auto_enrage")), GameData.instance.SAVE_STATE.autoEnrage, changable: true, listWidth, "AutoEnrage", base.gameObject);
		_autoEnrageCheckbox = transform.GetComponent<CheckBoxTile>();
		activeCheckBoxes.Add(_autoEnrageCheckbox);
		if (AppInfo.adsAvailable)
		{
			transform = null;
			transform = Object.Instantiate(checkBoxPrefab);
			transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
			transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_video_ads")), !GameData.instance.SAVE_STATE.adsDisabled, changable: true, listWidth, "Ads", base.gameObject);
			_adsCheckbox = transform.GetComponent<CheckBoxTile>();
			activeCheckBoxes.Add(_adsCheckbox);
		}
		transform = null;
		transform = Object.Instantiate(checkBoxPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_reduced_effects")), GameData.instance.SAVE_STATE.reducedEffects, changable: true, listWidth, "ReducedEffects", base.gameObject);
		_reducedEffects = transform.GetComponent<CheckBoxTile>();
		activeCheckBoxes.Add(_reducedEffects);
		transform = null;
		transform = Object.Instantiate(checkBoxPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_battle_text")), GameData.instance.SAVE_STATE.battleText, changable: true, listWidth, "BattleText", base.gameObject);
		_battleTextCheckbox = transform.GetComponent<CheckBoxTile>();
		activeCheckBoxes.Add(_battleTextCheckbox);
		transform = null;
		transform = Object.Instantiate(checkBoxPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_battle_bar_overlay")), GameData.instance.SAVE_STATE.battleBarOverlay, changable: true, listWidth, "BattleBarOverlay", base.gameObject);
		_battleBarOverlayCheckbox = transform.GetComponent<CheckBoxTile>();
		activeCheckBoxes.Add(_battleBarOverlayCheckbox);
		transform = null;
		transform = Object.Instantiate(checkBoxPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_world_chat")), _chatEnabled, changable: true, listWidth, "ChatEnabled", base.gameObject);
		_chatEnabledCheckbox = transform.GetComponent<CheckBoxTile>();
		activeCheckBoxes.Add(_chatEnabledCheckbox);
		transform = null;
		transform = Object.Instantiate(checkBoxPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_animations")), GameData.instance.SAVE_STATE.animations, changable: true, listWidth, "Animations", base.gameObject);
		_animationsCheckbox = transform.GetComponent<CheckBoxTile>();
		activeCheckBoxes.Add(_animationsCheckbox);
		_animationsTypesCheckboxes = new List<CheckBoxTile>();
		Dictionary<int, bool> animationsTypes = GameData.instance.SAVE_STATE.GetAnimationsTypes(GameData.instance.PROJECT.character.id);
		Transform obj = Object.Instantiate(checkBoxPrefab);
		obj.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		CheckBoxTile component2 = obj.GetComponent<CheckBoxTile>();
		component2.Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_fusion")), GameData.instance.SAVE_STATE.GetAnimationsType(GameData.instance.PROJECT.character.id, 0, animationsTypes), changable: true, listWidth - (float)num, "AnimationTypeFusion", base.gameObject);
		component2.data.id = 0;
		_animationsTypesCheckboxes.Add(component2);
		activeCheckBoxes.Add(component2);
		Transform obj2 = Object.Instantiate(checkBoxPrefab);
		obj2.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		CheckBoxTile component3 = obj2.GetComponent<CheckBoxTile>();
		component3.Create(new CheckBoxTile.CheckBoxObject(Language.GetString("battle_victory")), GameData.instance.SAVE_STATE.GetAnimationsType(GameData.instance.PROJECT.character.id, 1, animationsTypes), changable: true, listWidth - (float)num, "AnimationTypeVictory", base.gameObject);
		component3.data.id = 1;
		_animationsTypesCheckboxes.Add(component3);
		activeCheckBoxes.Add(component3);
		Transform obj3 = Object.Instantiate(checkBoxPrefab);
		obj3.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		CheckBoxTile component4 = obj3.GetComponent<CheckBoxTile>();
		component4.Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_battle")), GameData.instance.SAVE_STATE.GetAnimationsType(GameData.instance.PROJECT.character.id, 2, animationsTypes), changable: true, listWidth - (float)num, "AnimationTypeBattle", base.gameObject);
		component4.data.id = 2;
		_animationsTypesCheckboxes.Add(component4);
		activeCheckBoxes.Add(component4);
		transform = null;
		transform = Object.Instantiate(checkBoxPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_autopersuade_gold")), GameData.instance.SAVE_STATE.autopersuadeFamiliarsGold, changable: true, listWidth, "AutoPersuadeFamiliarsGold", base.gameObject);
		_persuaseFamiliarsGoldCheckbox = transform.GetComponent<CheckBoxTile>();
		activeCheckBoxes.Add(_persuaseFamiliarsGoldCheckbox);
		_persuaseFamiliarsGoldRaritiesCheckboxes = new List<CheckBoxTile>();
		Dictionary<int, bool> autopersuadeFamiliarsGoldRarities = GameData.instance.SAVE_STATE.GetAutopersuadeFamiliarsGoldRarities(GameData.instance.PROJECT.character.id);
		for (int i = 0; i < RarityBook.size; i++)
		{
			RarityRef rarityRef = RarityBook.LookupID(i);
			if (rarityRef != null && FamiliarBook.GetRarityCount(rarityRef) > 0)
			{
				Transform transform3 = null;
				transform3 = Object.Instantiate(checkBoxPrefab);
				transform3.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
				transform3.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(rarityRef.coloredName, rarityRef), GameData.instance.SAVE_STATE.GetAutopersuadeGoldFamiliarRarity(GameData.instance.PROJECT.character.id, rarityRef, autopersuadeFamiliarsGoldRarities), changable: true, listWidth - (float)num, "AutopersuadeGoldFamiliarRarities", base.gameObject);
				_persuaseFamiliarsGoldRaritiesCheckboxes.Add(transform3.GetComponent<CheckBoxTile>());
				activeCheckBoxes.Add(transform3.GetComponent<CheckBoxTile>());
			}
		}
		transform = null;
		transform = Object.Instantiate(checkBoxPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_autopersuade_gems")), GameData.instance.SAVE_STATE.autopersuadeFamiliarsGems, changable: true, listWidth, "AutoPersuadeFamiliarsGems", base.gameObject);
		_persuaseFamiliarsGemsCheckbox = transform.GetComponent<CheckBoxTile>();
		activeCheckBoxes.Add(_persuaseFamiliarsGemsCheckbox);
		_persuaseFamiliarsGemsRaritiesCheckboxes = new List<CheckBoxTile>();
		Dictionary<int, bool> autopersuadeFamiliarsGemsRarities = GameData.instance.SAVE_STATE.GetAutopersuadeFamiliarsGemsRarities(GameData.instance.PROJECT.character.id);
		for (int j = 0; j < RarityBook.size; j++)
		{
			RarityRef rarityRef = RarityBook.LookupID(j);
			if (rarityRef != null && FamiliarBook.GetRarityCount(rarityRef) > 0)
			{
				Transform transform4 = null;
				transform4 = Object.Instantiate(checkBoxPrefab);
				transform4.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
				transform4.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(rarityRef.coloredName, rarityRef), GameData.instance.SAVE_STATE.GetAutopersuadeGemsFamiliarRarity(GameData.instance.PROJECT.character.id, rarityRef, autopersuadeFamiliarsGemsRarities), changable: true, listWidth - (float)num, "AutopersuadeGemsFamiliarRarities", base.gameObject);
				_persuaseFamiliarsGemsRaritiesCheckboxes.Add(transform4.GetComponent<CheckBoxTile>());
				activeCheckBoxes.Add(transform4.GetComponent<CheckBoxTile>());
			}
		}
		transform = null;
		transform = Object.Instantiate(checkBoxPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_decline_dupes")), GameData.instance.SAVE_STATE.declineFamiliarDupes, changable: true, listWidth, "DeclineFamiliarDupes", base.gameObject);
		_declineFamiliarDupesCheckbox = transform.GetComponent<CheckBoxTile>();
		activeCheckBoxes.Add(_persuaseFamiliarsGoldCheckbox);
		_declineFamiliarRaritiesCheckboxes = new List<CheckBoxTile>();
		Dictionary<int, bool> declineFamiliarRarities = GameData.instance.SAVE_STATE.GetDeclineFamiliarRarities(GameData.instance.PROJECT.character.id);
		for (int k = 0; k < RarityBook.size; k++)
		{
			RarityRef rarityRef = RarityBook.LookupID(k);
			if (rarityRef != null && FamiliarBook.GetRarityCount(rarityRef) > 0)
			{
				Transform transform5 = null;
				transform5 = Object.Instantiate(checkBoxPrefab);
				transform5.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
				transform5.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(rarityRef.coloredName, rarityRef), GameData.instance.SAVE_STATE.GetDeclineFamiliarRarity(GameData.instance.PROJECT.character.id, rarityRef, declineFamiliarRarities), changable: true, listWidth - (float)num, "DeclineFamiliarRarities", base.gameObject);
				_declineFamiliarRaritiesCheckboxes.Add(transform5.GetComponent<CheckBoxTile>());
				activeCheckBoxes.Add(transform5.GetComponent<CheckBoxTile>());
			}
		}
		transform = null;
		transform = Object.Instantiate(checkBoxPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_decline_merchants")), GameData.instance.SAVE_STATE.declineMerchants, changable: true, listWidth, "DeclineMerchants", base.gameObject);
		_declineMerchantsCheckbox = transform.GetComponent<CheckBoxTile>();
		activeCheckBoxes.Add(_declineMerchantsCheckbox);
		_declineMerchantsRaritiesCheckboxes = new List<CheckBoxTile>();
		Dictionary<int, bool> declineMerchantsRarities = GameData.instance.SAVE_STATE.GetDeclineMerchantsRarities(GameData.instance.PROJECT.character.id);
		for (int l = 2; l < 5; l++)
		{
			RarityRef rarityRef = RarityBook.LookupID(l);
			if (rarityRef != null)
			{
				Transform transform6 = null;
				transform6 = Object.Instantiate(checkBoxPrefab);
				transform6.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
				transform6.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(rarityRef.coloredName, rarityRef), GameData.instance.SAVE_STATE.GetDeclineMerchantsRarity(GameData.instance.PROJECT.character.id, rarityRef, declineMerchantsRarities), changable: true, listWidth - (float)num, "DeclineMerchantsRarities", base.gameObject);
				_declineMerchantsRaritiesCheckboxes.Add(transform6.GetComponent<CheckBoxTile>());
				activeCheckBoxes.Add(transform6.GetComponent<CheckBoxTile>());
			}
		}
		transform = null;
		transform = Object.Instantiate(checkBoxPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_decline_treasures")), GameData.instance.SAVE_STATE.declineTreasures, changable: true, listWidth, "DeclineTreasures", base.gameObject);
		_declineTreasuresCheckbox = transform.GetComponent<CheckBoxTile>();
		activeCheckBoxes.Add(_declineTreasuresCheckbox);
		transform = null;
		transform = Object.Instantiate(checkBoxPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_hide_level_chat")), GameData.instance.SAVE_STATE.hideLevelChat, changable: true, listWidth, "hideLevelChat", base.gameObject);
		_hideLevelChatCheckbox = transform.GetComponent<CheckBoxTile>();
		activeCheckBoxes.Add(_hideLevelChatCheckbox);
		transform = null;
		transform = Object.Instantiate(checkBoxPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_defeat_advices")), GameData.instance.SAVE_STATE.defeatAdvices, changable: true, listWidth, "DefeatAdvices", base.gameObject);
		_defeatAdvices = transform.GetComponent<CheckBoxTile>();
		activeCheckBoxes.Add(_defeatAdvices);
		transform = null;
		transform = Object.Instantiate(checkBoxPrefab);
		transform.SetParent(gameSettingsPanelContent.transform, worldPositionStays: false);
		transform.GetComponent<CheckBoxTile>().Create(new CheckBoxTile.CheckBoxObject(Language.GetString("ui_equip_on_results")), GameData.instance.SAVE_STATE.equipOnResults, changable: true, listWidth, "EquipOnResults", base.gameObject);
		_allowEquipOnResults = transform.GetComponent<CheckBoxTile>();
		activeCheckBoxes.Add(_allowEquipOnResults);
		_allowEquipOnResultsBattleTypes = new List<CheckBoxTile>();
		if (GameData.instance.PROJECT.character == null)
		{
			_showHelmCheckbox.enabled = false;
			_showMountCheckbox.enabled = false;
			_showBodyCheckbox.enabled = false;
			_showAccessoryCheckbox.enabled = false;
			_friendRequestsCheckbox.enabled = false;
			_duelRequestsCheckbox.enabled = false;
			_chatEnabledCheckbox.enabled = false;
			_autoPilotCheckbox.enabled = false;
			_showNameplateCheckbox.enabled = false;
		}
		UpdateAnimationTypesCheckboxes();
		UpdateDeclineFamiliarRarityCheckboxes();
		UpdateAutoPersuadeGoldRarityCheckboxes();
		UpdateAutoPersuadeGemsRarityCheckboxes();
		UpdateDeclineMerchantsRarityCheckboxes();
		UpdateNotificationCheckboxes();
		UpdateBrawlRequestsCheckboxes();
		UpdateResolutionDropdown();
		UpdateEquipOnResultsTypesCheckboxes();
		StartCoroutine(CheckCheckboxStatus());
	}

	private IEnumerator CheckCheckboxStatus()
	{
		yield return new WaitForSeconds(1f);
		UpdateAnimationTypesCheckboxes();
		UpdateDeclineFamiliarRarityCheckboxes();
		UpdateAutoPersuadeGoldRarityCheckboxes();
		UpdateAutoPersuadeGemsRarityCheckboxes();
		UpdateDeclineMerchantsRarityCheckboxes();
		UpdateNotificationCheckboxes();
		UpdateBrawlRequestsCheckboxes();
		UpdateResolutionDropdown();
		UpdateEquipOnResultsTypesCheckboxes();
	}

	private void UpdateAnimationTypesCheckboxes()
	{
		bool isOn = _animationsCheckbox.toggle.isOn;
		foreach (CheckBoxTile animationsTypesCheckbox in _animationsTypesCheckboxes)
		{
			EnableToggle(animationsTypesCheckbox, isOn);
		}
	}

	private void UpdateDeclineFamiliarRarityCheckboxes()
	{
		bool isOn = _declineFamiliarDupesCheckbox.toggle.isOn;
		foreach (CheckBoxTile declineFamiliarRaritiesCheckbox in _declineFamiliarRaritiesCheckboxes)
		{
			EnableToggle(declineFamiliarRaritiesCheckbox, isOn);
		}
	}

	private void UpdateAutoPersuadeGoldRarityCheckboxes()
	{
		bool isOn = _persuaseFamiliarsGoldCheckbox.toggle.isOn;
		foreach (CheckBoxTile persuaseFamiliarsGoldRaritiesCheckbox in _persuaseFamiliarsGoldRaritiesCheckboxes)
		{
			EnableToggle(persuaseFamiliarsGoldRaritiesCheckbox, isOn);
		}
	}

	private void UpdateAutoPersuadeGemsRarityCheckboxes()
	{
		bool isOn = _persuaseFamiliarsGemsCheckbox.toggle.isOn;
		foreach (CheckBoxTile persuaseFamiliarsGemsRaritiesCheckbox in _persuaseFamiliarsGemsRaritiesCheckboxes)
		{
			EnableToggle(persuaseFamiliarsGemsRaritiesCheckbox, isOn);
		}
	}

	private void UpdateDeclineMerchantsRarityCheckboxes()
	{
		bool isOn = _declineMerchantsCheckbox.toggle.isOn;
		foreach (CheckBoxTile declineMerchantsRaritiesCheckbox in _declineMerchantsRaritiesCheckboxes)
		{
			EnableToggle(declineMerchantsRaritiesCheckbox, isOn);
		}
	}

	private void UpdateNotificationCheckboxes()
	{
		bool isOn = _notificationsCheckbox.toggle.isOn;
		foreach (CheckBoxTile notificationTypesCheckbox in _notificationTypesCheckboxes)
		{
			EnableToggle(notificationTypesCheckbox, isOn);
		}
	}

	private void UpdateBrawlRequestsCheckboxes()
	{
		EnableToggle(_brawlRequestsFriendCheckbox, _brawlRequestsCheckbox.toggle.isOn);
		EnableToggle(_brawlRequestsGuildCheckbox, _brawlRequestsCheckbox.toggle.isOn);
		EnableToggle(_brawlRequestsOtherCheckbox, _brawlRequestsCheckbox.toggle.isOn);
	}

	private void UpdateResolutionDropdown()
	{
		if (!(_windowedCheckbox == null) && !(_resolutionDropdown == null))
		{
			EnableDropdown(_resolutionDropdown, _windowedCheckbox.toggle.isOn);
		}
	}

	private void UpdateEquipOnResultsTypesCheckboxes()
	{
		bool isOn = _allowEquipOnResults.toggle.isOn;
		foreach (CheckBoxTile allowEquipOnResultsBattleType in _allowEquipOnResultsBattleTypes)
		{
			EnableToggle(allowEquipOnResultsBattleType, isOn);
		}
	}

	private void OnAnimationTypeCheckbox()
	{
		Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
		foreach (CheckBoxTile animationsTypesCheckbox in _animationsTypesCheckboxes)
		{
			int id = animationsTypesCheckbox.data.id;
			dictionary[id] = animationsTypesCheckbox.toggle.isOn;
		}
		GameData.instance.SAVE_STATE.SetAnimationsTypes(GameData.instance.PROJECT.character.id, dictionary);
	}

	private void OnDeclineFamiliarRarityCheckbox()
	{
		Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
		foreach (CheckBoxTile declineFamiliarRaritiesCheckbox in _declineFamiliarRaritiesCheckboxes)
		{
			RarityRef rarityRef = declineFamiliarRaritiesCheckbox.data.objectRef as RarityRef;
			dictionary[rarityRef.id] = declineFamiliarRaritiesCheckbox.toggle.isOn;
		}
		GameData.instance.SAVE_STATE.SetDeclineFamiliarRarities(GameData.instance.PROJECT.character.id, dictionary);
	}

	private void OnDeclineMerchantsRarityCheckbox()
	{
		Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
		foreach (CheckBoxTile declineMerchantsRaritiesCheckbox in _declineMerchantsRaritiesCheckboxes)
		{
			RarityRef rarityRef = declineMerchantsRaritiesCheckbox.data.objectRef as RarityRef;
			dictionary[rarityRef.id] = declineMerchantsRaritiesCheckbox.toggle.isOn;
		}
		GameData.instance.SAVE_STATE.SetDeclineMerchantsRarities(GameData.instance.PROJECT.character.id, dictionary);
	}

	private void OnAutopersuadeGoldFamiliarRarityCheckbox(List<CheckBoxTile> source = null)
	{
		Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
		int num = 0;
		foreach (CheckBoxTile persuaseFamiliarsGoldRaritiesCheckbox in _persuaseFamiliarsGoldRaritiesCheckboxes)
		{
			RarityRef rarityRef = persuaseFamiliarsGoldRaritiesCheckbox.data.objectRef as RarityRef;
			if (source != null && source[num].toggle.isOn)
			{
				persuaseFamiliarsGoldRaritiesCheckbox.toggle.isOn = false;
			}
			dictionary[rarityRef.id] = persuaseFamiliarsGoldRaritiesCheckbox.toggle.isOn;
			num++;
		}
		GameData.instance.SAVE_STATE.SetAutopersuadeFamiliarsGoldRarities(GameData.instance.PROJECT.character.id, dictionary);
		if (source == null)
		{
			OnAutopersuadeGemsFamiliarRarityCheckbox(_persuaseFamiliarsGoldRaritiesCheckboxes);
		}
	}

	private void OnAutopersuadeGemsFamiliarRarityCheckbox(List<CheckBoxTile> source = null)
	{
		Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
		int num = 0;
		foreach (CheckBoxTile persuaseFamiliarsGemsRaritiesCheckbox in _persuaseFamiliarsGemsRaritiesCheckboxes)
		{
			RarityRef rarityRef = persuaseFamiliarsGemsRaritiesCheckbox.data.objectRef as RarityRef;
			if (source != null && source[num].toggle.isOn)
			{
				persuaseFamiliarsGemsRaritiesCheckbox.toggle.isOn = false;
			}
			dictionary[rarityRef.id] = persuaseFamiliarsGemsRaritiesCheckbox.toggle.isOn;
			num++;
		}
		GameData.instance.SAVE_STATE.SetAutopersuadeFamiliarsGemsRarities(GameData.instance.PROJECT.character.id, dictionary);
		if (source == null)
		{
			OnAutopersuadeGoldFamiliarRarityCheckbox(_persuaseFamiliarsGemsRaritiesCheckboxes);
		}
	}

	private bool CheckChanges()
	{
		if (_showHelmCheckbox != null && _showHelmCheckbox.toggle.isOn != _showHelm)
		{
			return true;
		}
		if (_showMountCheckbox != null && _showMountCheckbox.toggle.isOn != _showMount)
		{
			return true;
		}
		if (_showBodyCheckbox != null && _showBodyCheckbox.toggle.isOn != _showBody)
		{
			return true;
		}
		if (_showAccessoryCheckbox != null && _showAccessoryCheckbox.toggle.isOn != _showAccessory)
		{
			return true;
		}
		if (_friendRequestsCheckbox != null && _friendRequestsCheckbox.toggle.isOn != _friendRequests)
		{
			return true;
		}
		if (_duelRequestsCheckbox != null && _duelRequestsCheckbox.toggle.isOn != _duelRequests)
		{
			return true;
		}
		if (_chatEnabledCheckbox != null && _chatEnabledCheckbox.toggle.isOn != _chatEnabled)
		{
			return true;
		}
		if (_autoPilotCheckbox != null && _autoPilotCheckbox.toggle.isOn != _autoPilot)
		{
			return true;
		}
		if (_showNameplateCheckbox != null && _showNameplateCheckbox.toggle.isOn != _showNameplate)
		{
			return true;
		}
		return false;
	}

	public bool SaveChanges()
	{
		if (GameData.instance.PROJECT.character == null || !CheckChanges())
		{
			return false;
		}
		if (_showHelmCheckbox != null)
		{
			GameData.instance.PROJECT.character.showHelm = _showHelmCheckbox.toggle.isOn;
		}
		if (_showMountCheckbox != null)
		{
			GameData.instance.PROJECT.character.showMount = _showMountCheckbox.toggle.isOn;
		}
		if (_showBodyCheckbox != null)
		{
			GameData.instance.PROJECT.character.showBody = _showBodyCheckbox.toggle.isOn;
		}
		if (_showAccessoryCheckbox != null)
		{
			GameData.instance.PROJECT.character.showAccessory = _showAccessoryCheckbox.toggle.isOn;
		}
		if (_friendRequestsCheckbox != null)
		{
			GameData.instance.PROJECT.character.friendRequestsEnabled = _friendRequestsCheckbox.toggle.isOn;
		}
		if (_duelRequestsCheckbox != null)
		{
			GameData.instance.PROJECT.character.duelRequestsEnabled = _duelRequestsCheckbox.toggle.isOn;
		}
		if (_chatEnabledCheckbox != null)
		{
			GameData.instance.PROJECT.character.chatEnabled = _chatEnabledCheckbox.toggle.isOn;
		}
		if (_autoPilotCheckbox != null)
		{
			GameData.instance.PROJECT.character.autoPilot = _autoPilotCheckbox.toggle.isOn;
		}
		if (_showNameplateCheckbox != null)
		{
			GameData.instance.PROJECT.character.saveShowNameplate(_showNameplateCheckbox.toggle.isOn);
		}
		return true;
	}

	public void OnToggleChangeWindowed()
	{
		GameData.instance.SAVE_STATE.fullscreen = !_windowedCheckbox.toggle.isOn;
		MonoBehaviour.print(GameData.instance.SAVE_STATE.fullscreen);
		Screen.fullScreen = GameData.instance.SAVE_STATE.fullscreen;
		UpdateResolutionDropdown();
	}

	public void OnResolutionDropdown(PointerEventData data)
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		window = GameData.instance.windowGenerator.NewDropdownWindow(Language.GetString("ui_resolution"));
		DropdownList componentInChildren = window.GetComponentInChildren<DropdownList>();
		Vector2 vector = new Vector2(GameData.instance.SAVE_STATE.resolutionX, GameData.instance.SAVE_STATE.resolutionY);
		if (vector.x <= 0f || vector.y <= 0f)
		{
			vector = new Vector2(Screen.width, Screen.height);
		}
		List<Vector2> gameResolutions = VariableBook.GetGameResolutions();
		for (int i = 0; i < gameResolutions.Count; i++)
		{
			if (Mathf.Round(gameResolutions[i].x) == Mathf.Round(vector.x) && Mathf.Round(gameResolutions[i].y) == Mathf.Round(vector.y))
			{
				resolutionSelected = i;
			}
		}
		componentInChildren.StartList(base.gameObject, resolutionSelected, OnResolutionDropdownClicked);
		for (int j = 0; j < gameResolutions.Count; j++)
		{
			componentInChildren.Data.InsertOneAtEnd(new MyDropdownItemModel
			{
				id = j,
				title = gameResolutions[j].x + " x " + gameResolutions[j].y,
				btnHelp = false
			});
		}
	}

	public void OnResolutionDropdownClicked(MyDropdownItemModel model)
	{
		List<Vector2> gameResolutions = VariableBook.GetGameResolutions();
		_resolutionDropdown.GetComponentInChildren<TextMeshProUGUI>().text = gameResolutions[model.id].x + " x " + gameResolutions[model.id].y;
		resolutionSelected = model.id;
		GameData.instance.SAVE_STATE.resolutionX = Mathf.RoundToInt(gameResolutions[model.id].x);
		GameData.instance.SAVE_STATE.resolutionY = Mathf.RoundToInt(gameResolutions[model.id].y);
		SetResolution();
		if (window != null)
		{
			window.GetComponent<DropdownWindow>().OnClose();
		}
	}

	public void SetResolution()
	{
		Screen.SetResolution(GameData.instance.SAVE_STATE.resolutionX, GameData.instance.SAVE_STATE.resolutionY, GameData.instance.SAVE_STATE.fullscreen);
	}

	public void OnToggleChangeAutoPilotDeath()
	{
		GameData.instance.SAVE_STATE.autoPilotDeathDisable = _autoPilotDeathDisableCheckbox.toggle.isOn;
	}

	public void OnToggleChangeIgnoreBoss()
	{
		GameData.instance.SAVE_STATE.ignoreBoss = _ignoreBossCheckbox.toggle.isOn;
	}

	public void OnToggleChangeIgnoreShrines()
	{
		GameData.instance.SAVE_STATE.ignoreShrines = _ignoreShrinesCheckbox.toggle.isOn;
	}

	public void OnToggleChangeNotifications()
	{
		GameData.instance.SAVE_STATE.notificationsDisabled = !_notificationsCheckbox.toggle.isOn;
		UpdateNotificationCheckboxes();
	}

	public void OnToggleChangeNotificationsFriend()
	{
		GameData.instance.SAVE_STATE.notificationsFriend = _notificationsFriendCheckbox.toggle.isOn;
	}

	public void OnToggleChangeNotificationsGuild()
	{
		GameData.instance.SAVE_STATE.notificationsGuild = _notificationsGuildCheckbox.toggle.isOn;
	}

	public void OnToggleChangeNotificationsFamiliars()
	{
		GameData.instance.SAVE_STATE.notificationsFamiliars = _notificationsFamiliarsCheckbox.toggle.isOn;
		UpdateFamiliarTileNotifications();
	}

	public void OnToggleChangeNotificationsFusions()
	{
		GameData.instance.SAVE_STATE.notificationsFusions = _notificationsFusionsCheckbox.toggle.isOn;
		UpdateFamiliarTileNotifications();
	}

	public void OnToggleChangeNotificationsCraft()
	{
		GameData.instance.SAVE_STATE.notificationsCraft = _notificationsCraftCheckbox.toggle.isOn;
		UpdateCraftTileNotifications();
	}

	public void OnToggleChangeNotificationsOther()
	{
		GameData.instance.SAVE_STATE.notificationsOther = _notificationsOtherCheckbox.toggle.isOn;
	}

	private void UpdateCraftTileNotifications()
	{
		MenuInterfaceCraftTile menuInterfaceCraftTile = GameData.instance.PROJECT.instance.instanceInterface?.GetButton(typeof(MenuInterfaceCraftTile)) as MenuInterfaceCraftTile;
		if (menuInterfaceCraftTile != null)
		{
			menuInterfaceCraftTile.UpdateText();
		}
	}

	private void UpdateFamiliarTileNotifications()
	{
		MenuInterfaceFamiliarTile menuInterfaceFamiliarTile = GameData.instance.PROJECT.menuInterface?.GetButton(typeof(MenuInterfaceFamiliarTile)) as MenuInterfaceFamiliarTile;
		if (menuInterfaceFamiliarTile != null)
		{
			menuInterfaceFamiliarTile.UpdateText();
		}
	}

	public void OnToggleChangeAppNotifications()
	{
		GameData.instance.SAVE_STATE.appNotificationsDisabled = !_appNotificationsCheckbox.toggle.isOn;
	}

	public void OnToggleChangeBrawlRequests()
	{
		GameData.instance.SAVE_STATE.brawlRequests = _brawlRequestsCheckbox.toggle.isOn;
		UpdateBrawlRequestsCheckboxes();
	}

	public void OnToggleChangeBrawlRequestsFriend()
	{
		GameData.instance.SAVE_STATE.brawlRequestsFriend = _brawlRequestsFriendCheckbox.toggle.isOn;
	}

	public void OnToggleChangeBrawlRequestsGuild()
	{
		GameData.instance.SAVE_STATE.brawlRequestsGuild = _brawlRequestsGuildCheckbox.toggle.isOn;
	}

	public void OnToggleChangeBrawlRequestsOther()
	{
		GameData.instance.SAVE_STATE.brawlRequestsOther = _brawlRequestsOtherCheckbox.toggle.isOn;
	}

	public void OnToggleChangeAutoEnrage()
	{
		GameData.instance.SAVE_STATE.autoEnrage = _autoEnrageCheckbox.toggle.isOn;
	}

	public void OnToggleChangeAds()
	{
		GameData.instance.SAVE_STATE.adsDisabled = !_adsCheckbox.toggle.isOn;
	}

	public void OnToggleChangeReducedEffects()
	{
		GameData.instance.SAVE_STATE.reducedEffects = _reducedEffects.toggle.isOn;
	}

	public void OnToggleChangeBattleText()
	{
		GameData.instance.SAVE_STATE.battleText = _battleTextCheckbox.toggle.isOn;
	}

	public void OnToggleChangeBattleBarOverlay()
	{
		GameData.instance.SAVE_STATE.battleBarOverlay = _battleBarOverlayCheckbox.toggle.isOn;
	}

	public void OnToggleChangeAnimations()
	{
		GameData.instance.SAVE_STATE.animations = _animationsCheckbox.toggle.isOn;
		UpdateAnimationTypesCheckboxes();
	}

	public void OnToggleChangeAnimationTypeFusion()
	{
		OnAnimationTypeCheckbox();
	}

	public void OnToggleChangeAnimationTypeVictory()
	{
		OnAnimationTypeCheckbox();
	}

	public void OnToggleChangeAnimationTypeBattle()
	{
		OnAnimationTypeCheckbox();
	}

	public void OnToggleChangeDeclineFamiliarDupes()
	{
		GameData.instance.SAVE_STATE.declineFamiliarDupes = _declineFamiliarDupesCheckbox.toggle.isOn;
		UpdateDeclineFamiliarRarityCheckboxes();
	}

	public void OnToggleChangeDeclineFamiliarRarities()
	{
		OnDeclineFamiliarRarityCheckbox();
	}

	public void OnToggleChangeAutoPersuadeFamiliarsGold()
	{
		GameData.instance.SAVE_STATE.autopersuadeFamiliarsGold = _persuaseFamiliarsGoldCheckbox.toggle.isOn;
		UpdateAutoPersuadeGoldRarityCheckboxes();
	}

	public void OnToggleChangeAutopersuadeGoldFamiliarRarities()
	{
		OnAutopersuadeGoldFamiliarRarityCheckbox();
	}

	public void OnToggleChangeAutoPersuadeFamiliarsGems()
	{
		GameData.instance.SAVE_STATE.autopersuadeFamiliarsGems = _persuaseFamiliarsGemsCheckbox.toggle.isOn;
		UpdateAutoPersuadeGemsRarityCheckboxes();
	}

	public void OnToggleChangeAutopersuadeGemsFamiliarRarities()
	{
		OnAutopersuadeGemsFamiliarRarityCheckbox();
	}

	public void OnToggleChangeDeclineMerchants()
	{
		GameData.instance.SAVE_STATE.declineMerchants = _declineMerchantsCheckbox.toggle.isOn;
		UpdateDeclineMerchantsRarityCheckboxes();
	}

	public void OnToggleChangeDeclineMerchantsRarities()
	{
		OnDeclineMerchantsRarityCheckbox();
	}

	public void OnToggleChangeDeclineTreasures()
	{
		GameData.instance.SAVE_STATE.declineTreasures = _declineTreasuresCheckbox.toggle.isOn;
	}

	public void OnToggleChangehideLevelChat()
	{
		GameData.instance.SAVE_STATE.hideLevelChat = _hideLevelChatCheckbox.toggle.isOn;
	}

	public void OnToggleChangeDefeatAdvices()
	{
		GameData.instance.SAVE_STATE.defeatAdvices = _defeatAdvices.toggle.isOn;
	}

	public void OnToggleChangeEquipOnResults()
	{
		GameData.instance.SAVE_STATE.equipOnResults = _allowEquipOnResults.toggle.isOn;
		UpdateEquipOnResultsTypesCheckboxes();
	}

	public void OnToggleChangeEquipOnResultsTypeDungeon()
	{
		OnEquipOnResultsTypeCheckbox();
	}

	public void OnToggleChangeEquipOnResultsTypePvP()
	{
		OnEquipOnResultsTypeCheckbox();
	}

	public void OnToggleChangeEquipOnResultsTypeRaid()
	{
		OnEquipOnResultsTypeCheckbox();
	}

	public void OnToggleChangeEquipOnResultsTypeRift()
	{
		OnEquipOnResultsTypeCheckbox();
	}

	public void OnToggleChangeEquipOnResultsTypeGauntlet()
	{
		OnEquipOnResultsTypeCheckbox();
	}

	public void OnToggleChangeEquipOnResultsTypeGvG()
	{
		OnEquipOnResultsTypeCheckbox();
	}

	public void OnToggleChangeEquipOnResultsTypeInvasion()
	{
		OnEquipOnResultsTypeCheckbox();
	}

	public void OnToggleChangeEquipOnResultsTypeBrawl()
	{
		OnEquipOnResultsTypeCheckbox();
	}

	public void OnToggleChangeEquipOnResultsTypeGvE()
	{
		OnEquipOnResultsTypeCheckbox();
	}

	private void OnEquipOnResultsTypeCheckbox()
	{
		Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
		foreach (CheckBoxTile allowEquipOnResultsBattleType in _allowEquipOnResultsBattleTypes)
		{
			int id = allowEquipOnResultsBattleType.data.id;
			dictionary[id] = allowEquipOnResultsBattleType.toggle.isOn;
		}
		GameData.instance.SAVE_STATE.SetEquipOnResultsTypes(GameData.instance.PROJECT.character.id, dictionary);
	}

	private void EnableToggle(CheckBoxTile toggle, bool enable)
	{
		toggle.toggle.interactable = enable;
		if (enable)
		{
			toggle.labelTxt.color = new Color(toggle.labelTxt.color.r, toggle.labelTxt.color.g, toggle.labelTxt.color.b, 1f);
		}
		else
		{
			toggle.labelTxt.color = new Color(toggle.labelTxt.color.r, toggle.labelTxt.color.g, toggle.labelTxt.color.b, 0.5f);
			toggle.gameObject.GetComponent<HoverImage>().ForceInit();
			toggle.gameObject.GetComponent<HoverImage>().OnExit();
		}
		toggle.gameObject.GetComponent<HoverImage>().enabled = enable;
	}

	private void EnableDropdown(Image dropdown, bool enable)
	{
		if (enable)
		{
			dropdown.color = Color.white;
			dropdown.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
			dropdown.GetComponent<EventTrigger>().enabled = true;
			dropdown.GetComponent<HoverImage>().enabled = true;
		}
		else
		{
			dropdown.color = alpha;
			dropdown.GetComponentInChildren<TextMeshProUGUI>().color = alpha;
			dropdown.GetComponent<EventTrigger>().enabled = false;
			dropdown.GetComponent<HoverImage>().ForceInit();
			dropdown.GetComponent<HoverImage>().OnExit();
			dropdown.GetComponent<HoverImage>().enabled = false;
		}
	}

	public void RemoveListeners()
	{
		if (_resolutionDropdown != null)
		{
			_resolutionDropdown.GetComponent<EventTrigger>();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerClick;
			entry.callback.RemoveListener(delegate(BaseEventData data)
			{
				OnResolutionDropdown((PointerEventData)data);
			});
		}
	}

	public void Show()
	{
		base.gameObject.SetActive(value: true);
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}

	public void DoEnable()
	{
		if (_musicSlider != null)
		{
			_musicSlider.interactable = true;
		}
		if (_soundSlider != null)
		{
			_soundSlider.interactable = true;
		}
		if (_resolutionDropdown != null)
		{
			_resolutionDropdown.GetComponent<EventTrigger>().enabled = true;
		}
		for (int i = 0; i < activeCheckBoxes.Count; i++)
		{
			activeCheckBoxes[i].toggle.interactable = true;
		}
	}

	public void DoDisable()
	{
		if (_musicSlider != null)
		{
			_musicSlider.interactable = false;
		}
		if (_soundSlider != null)
		{
			_soundSlider.interactable = false;
		}
		if (_resolutionDropdown != null)
		{
			_resolutionDropdown.GetComponent<EventTrigger>().enabled = false;
		}
		for (int i = 0; i < activeCheckBoxes.Count; i++)
		{
			activeCheckBoxes[i].toggle.interactable = false;
		}
	}
}
