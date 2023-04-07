using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.charactermov;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.login;
using com.ultrabit.bitheroes.model.ability;
using com.ultrabit.bitheroes.model.adgor;
using com.ultrabit.bitheroes.model.admin;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.armory;
using com.ultrabit.bitheroes.model.armory.enchant;
using com.ultrabit.bitheroes.model.armory.rune;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.augment;
using com.ultrabit.bitheroes.model.battle;
using com.ultrabit.bitheroes.model.booster;
using com.ultrabit.bitheroes.model.brawl;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.chat;
using com.ultrabit.bitheroes.model.consumable;
using com.ultrabit.bitheroes.model.craft;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.dialog;
using com.ultrabit.bitheroes.model.enchant;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.fishing;
using com.ultrabit.bitheroes.model.friend;
using com.ultrabit.bitheroes.model.fusion;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.guild;
using com.ultrabit.bitheroes.model.gve;
using com.ultrabit.bitheroes.model.gvg;
using com.ultrabit.bitheroes.model.invasion;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.mount;
using com.ultrabit.bitheroes.model.payment;
using com.ultrabit.bitheroes.model.rune;
using com.ultrabit.bitheroes.model.service;
using com.ultrabit.bitheroes.model.shop;
using com.ultrabit.bitheroes.model.team;
using com.ultrabit.bitheroes.model.user;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.model.zone;
using com.ultrabit.bitheroes.parsing.model.utility;
using com.ultrabit.bitheroes.server;
using com.ultrabit.bitheroes.ui.ability;
using com.ultrabit.bitheroes.ui.ad;
using com.ultrabit.bitheroes.ui.adgor;
using com.ultrabit.bitheroes.ui.admin;
using com.ultrabit.bitheroes.ui.augment;
using com.ultrabit.bitheroes.ui.battle;
using com.ultrabit.bitheroes.ui.brawl;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.character.armory;
using com.ultrabit.bitheroes.ui.chat;
using com.ultrabit.bitheroes.ui.cosmetic;
using com.ultrabit.bitheroes.ui.craft;
using com.ultrabit.bitheroes.ui.daily;
using com.ultrabit.bitheroes.ui.dialog;
using com.ultrabit.bitheroes.ui.dropdown;
using com.ultrabit.bitheroes.ui.dungeon;
using com.ultrabit.bitheroes.ui.enchant;
using com.ultrabit.bitheroes.ui.equipment;
using com.ultrabit.bitheroes.ui.events;
using com.ultrabit.bitheroes.ui.eventsales;
using com.ultrabit.bitheroes.ui.familiar;
using com.ultrabit.bitheroes.ui.fishing;
using com.ultrabit.bitheroes.ui.friend;
using com.ultrabit.bitheroes.ui.fusion;
using com.ultrabit.bitheroes.ui.game;
using com.ultrabit.bitheroes.ui.gauntlet;
using com.ultrabit.bitheroes.ui.guild;
using com.ultrabit.bitheroes.ui.gve;
using com.ultrabit.bitheroes.ui.gvg;
using com.ultrabit.bitheroes.ui.heroselector;
using com.ultrabit.bitheroes.ui.instance;
using com.ultrabit.bitheroes.ui.instance.fishing;
using com.ultrabit.bitheroes.ui.invasion;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.kongregate;
using com.ultrabit.bitheroes.ui.leaderboard;
using com.ultrabit.bitheroes.ui.lists.tradelist;
using com.ultrabit.bitheroes.ui.menu;
using com.ultrabit.bitheroes.ui.mount;
using com.ultrabit.bitheroes.ui.news;
using com.ultrabit.bitheroes.ui.payment.custom;
using com.ultrabit.bitheroes.ui.payment.nbp;
using com.ultrabit.bitheroes.ui.playervoting;
using com.ultrabit.bitheroes.ui.pvp;
using com.ultrabit.bitheroes.ui.raid;
using com.ultrabit.bitheroes.ui.rift;
using com.ultrabit.bitheroes.ui.rune;
using com.ultrabit.bitheroes.ui.service;
using com.ultrabit.bitheroes.ui.shop;
using com.ultrabit.bitheroes.ui.team;
using com.ultrabit.bitheroes.ui.utility;
using com.ultrabit.bitheroes.ui.victory;
using com.ultrabit.bitheroes.ui.vipgor;
using com.ultrabit.bitheroes.ui.zone;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui;

public class WindowGenerator : MonoBehaviour
{
	public enum Window
	{
		None,
		Enchants,
		Character
	}

	[HideInInspector]
	public UnityEvent CHANGE = new UnityEvent();

	public const string UI = "ui/";

	public const string ABILITY = "ui/ability/";

	public const string AD = "ui/ad/";

	public const string ADGOR = "ui/adgor/";

	public const string ADMIN = "ui/admin/";

	public const string AUDIO = "ui/audio/";

	public const string AUGMENT = "ui/augment/";

	public const string BATTLE = "ui/battle/";

	public const string BRAWL = "ui/brawl/";

	public const string CHARACTER = "ui/character/";

	public const string HERO = "ui/hero/";

	public const string CHARACTER_ARMORY = "ui/character/armory/";

	public const string CHAT = "ui/chat/";

	public const string COSMETIC = "ui/cosmetic/";

	public const string CRAFT = "ui/craft/";

	public const string DAILY = "ui/daily/";

	public const string CHARACTER_ACHIEVEMENTS = "ui/achievements/";

	public const string DIALOG = "ui/dialog/";

	public const string DROPDOWN = "ui/dropdown/";

	public const string DUNGEON = "ui/dungeon/";

	public const string ENCHANT = "ui/enchant/";

	public const string EQUIPMENT = "ui/equipment/";

	public const string EVENTS = "ui/events/";

	public const string FAMILIAR = "ui/familiar/";

	public const string FISHING = "ui/fishing/";

	public const string EVENT_SALES = "ui/eventsales/";

	public const string FRIEND = "ui/friend/";

	public const string FUSION = "ui/fusion/";

	public const string GAME = "ui/game/";

	public const string GAUNTLET = "ui/gauntlet/";

	public const string GUILD = "ui/guild/";

	public const string GVE = "ui/gve/";

	public const string GVG = "ui/gvg/";

	public const string INSTANCE = "ui/instance/";

	public const string INSTANCE_FISHING = "ui/instance/fishing/";

	public const string INVASION = "ui/invasion/";

	public const string ITEM = "ui/item/";

	public const string KONGREGATE = "ui/kongregate/";

	public const string LEADERBOARD = "ui/leaderboard/";

	public const string MENU = "ui/menu/";

	public const string MOUNT = "ui/mount/";

	public const string NEWS = "ui/news/";

	public const string PAYMENT = "ui/payment/";

	public const string PAYMENT_CUSTOM = "ui/payment/custom/";

	public const string PAYMENT_NBP = "ui/payment/nbp/";

	public const string PLAYERVOTING = "ui/playervoting/";

	public const string PROMO = "ui/promo/";

	public const string PVP = "ui/pvp/";

	public const string RAID = "ui/raid/";

	public const string RIFT = "ui/rift/";

	public const string RUNE = "ui/rune/";

	public const string SERVICE = "ui/service/";

	public const string SHOP = "ui/shop/";

	public const string SHOP_WINDOW = "ui/shop/window/";

	public const string TEAM = "ui/team/";

	public const string UTILITY = "ui/utility/";

	public const string VICTORY = "ui/victory/";

	public const string VIPGOR = "ui/vipgor/";

	public const string ZONE = "ui/zone/";

	public const string ZONE_ZONES = "ui/zone/zones/";

	public const string EULA = "ui/eula/";

	public const int CURRENCY_CREDITS_TILE = 0;

	public const int CURRENCY_GOLD_TILE = 1;

	public const int CURRENCY_ENERGY_TILE = 2;

	public const int CURRENCY_TICKETS_TILE = 3;

	[HideInInspector]
	public bool chatVisible;

	[HideInInspector]
	public GameObject canvas;

	[HideInInspector]
	public CharacterMov player;

	private GameObject _currencyCreditsTile;

	private GameObject _currencyGoldTile;

	private GameObject _currencyEnergyTile;

	private GameObject _currencyTicketsTile;

	private List<GameObject> _dialogs = new List<GameObject>();

	private List<int[]> _classes = new List<int[]>();

	private List<GameObject> _chatObjects = new List<GameObject>();

	private ChatTiles _chatTiles;

	private Vector2 _chatTilePosition;

	private Vector2 _chatTileSize;

	private List<GameObject> _battleObjects = new List<GameObject>();

	private BattleTiles _battleTiles;

	private GameObject _notifications;

	private string _nametmp;

	private MainUIButton[] UIbuttons;

	private List<ConversationWindow> conversationsWindow;

	private EnchantsWindow _enchantsWindow;

	private Vector2 _chatUIPosition;

	private Vector2 _chatUISize;

	private Transform _chatTileParent;

	private Dictionary<Window, WindowsMain> _windows = new Dictionary<Window, WindowsMain>();

	private int _showPlayerLayer = -1;

	public int dialogCount
	{
		get
		{
			CheckNullDialogs();
			return _dialogs.Count;
		}
	}

	public bool hasPopup
	{
		get
		{
			CheckNullDialogs();
			return _dialogs.Count > 0;
		}
	}

	public Dictionary<Window, WindowsMain> Windows => _windows;

	public EnchantsWindow enchantsWindow
	{
		get
		{
			if (!Windows.ContainsKey(Window.Enchants))
			{
				return null;
			}
			return Windows[Window.Enchants] as EnchantsWindow;
		}
	}

	public CharacterWindow CharacterWindow
	{
		get
		{
			if (!Windows.ContainsKey(Window.Character))
			{
				return null;
			}
			return Windows[Window.Character] as CharacterWindow;
		}
	}

	public Vector2 chatUIPosition
	{
		get
		{
			return _chatUIPosition;
		}
		set
		{
			_chatUIPosition = value;
			SetChatUIPosition(_chatUIPosition);
		}
	}

	public Vector2 chatUISize
	{
		get
		{
			return _chatUISize;
		}
		set
		{
			_chatUISize = value;
			SetChatUISize(_chatUISize);
		}
	}

	private void CheckNullDialogs()
	{
		for (int num = _dialogs.Count - 1; num >= 0; num--)
		{
			if (_dialogs[num] == null)
			{
				_dialogs.RemoveAt(num);
				_classes.RemoveAt(num);
			}
		}
	}

	private void Awake()
	{
		GameData.instance.windowGenerator = this;
		canvas = GameObject.Find("Canvas");
		if (GameData.instance.PROJECT != null)
		{
			GameData.instance.PROJECT.SetSpecialListeners();
		}
	}

	public int GetDialogCountWithout(Type exception)
	{
		int num = 0;
		for (int i = 0; i < _dialogs.Count; i++)
		{
			if (_dialogs[i] != null && _dialogs[i].GetComponent(exception) == null)
			{
				num++;
			}
		}
		return num;
	}

	public int GetDialogCountWithout(Type exception1, Type exception2)
	{
		int num = 0;
		for (int i = 0; i < _dialogs.Count; i++)
		{
			if (_dialogs[i] != null && _dialogs[i].GetComponent(exception1) == null && _dialogs[i].GetComponent(exception2) == null)
			{
				num++;
			}
		}
		return num;
	}

	public WindowsMain GetLastDialog()
	{
		CheckNullDialogs();
		if (_dialogs.Count <= 0)
		{
			return null;
		}
		return _dialogs[_dialogs.Count - 1].GetComponent(typeof(WindowsMain)) as WindowsMain;
	}

	public WindowsMain GetFirstDialog()
	{
		CheckNullDialogs();
		if (_dialogs.Count <= 0)
		{
			return null;
		}
		return _dialogs[0].GetComponent(typeof(WindowsMain)) as WindowsMain;
	}

	public bool HasDialogOffset()
	{
		CheckNullDialogs();
		return _dialogs.Count > 0;
	}

	public object GetDialogByClass(Type type)
	{
		for (int i = 0; i < _dialogs.Count; i++)
		{
			if (_dialogs[i] != null && _dialogs[i].GetComponent(type) != null)
			{
				return _dialogs[i].GetComponent(type);
			}
		}
		return null;
	}

	public object[] GetDialogsByClass(Type type)
	{
		List<object> list = new List<object>();
		for (int i = 0; i < _dialogs.Count; i++)
		{
			if (_dialogs[i] != null && _dialogs[i].GetComponent(type) != null)
			{
				list.Add(_dialogs[i].GetComponent(type));
			}
		}
		return list.ToArray();
	}

	public bool HasDialogByClass(Type type)
	{
		foreach (GameObject dialog in _dialogs)
		{
			if (!(dialog == null) && dialog.GetComponent(type) != null)
			{
				return true;
			}
		}
		return false;
	}

	public void OnWindowScrollOut(object e)
	{
		WindowsMain windowsMain = e as WindowsMain;
		for (int i = 0; i < _dialogs.Count; i++)
		{
			if (_dialogs[i].GetInstanceID() == windowsMain.gameObject.GetInstanceID() && windowsMain.dialogParent != null)
			{
				WindowsMain component = windowsMain.dialogParent.GetComponent<WindowsMain>();
				if (component != null)
				{
					component.DoShow();
				}
			}
		}
	}

	public void OnWindowDestroyed(object e)
	{
		WindowsMain windowsMain = e as WindowsMain;
		for (int i = 0; i < _dialogs.Count; i++)
		{
			if (_dialogs[i].GetInstanceID() == windowsMain.gameObject.GetInstanceID())
			{
				windowsMain.SCROLL_OUT_START.RemoveListener(OnWindowScrollOut);
				windowsMain.DESTROYED.RemoveListener(OnWindowDestroyed);
				_dialogs.RemoveAt(i);
				_classes.RemoveAt(i);
				UpdateDialogUI();
				break;
			}
		}
	}

	public void UpdateDialogUI()
	{
		CheckNullDialogs();
		CHANGE.Invoke();
		if (_currencyCreditsTile != null)
		{
			Main.CONTAINER.AddToLayer(_currencyCreditsTile, 0, front: true, center: false, resize: false);
		}
		if (_currencyGoldTile != null)
		{
			Main.CONTAINER.AddToLayer(_currencyGoldTile, 0, front: true, center: false, resize: false);
		}
		if (_currencyEnergyTile != null)
		{
			Main.CONTAINER.AddToLayer(_currencyEnergyTile, 0, front: true, center: false, resize: false);
		}
		if (_currencyTicketsTile != null)
		{
			Main.CONTAINER.AddToLayer(_currencyTicketsTile, 0, front: true, center: false, resize: false);
		}
		if (_dialogs.Count <= 0)
		{
			return;
		}
		_ = _dialogs.Count;
		for (int i = 0; i < _dialogs.Count; i++)
		{
			GameObject gameObject = _dialogs[i];
			if (gameObject == null)
			{
				continue;
			}
			WindowsMain component = gameObject.GetComponent<WindowsMain>();
			if (component == null)
			{
				break;
			}
			if (component.layer < 0)
			{
				if ((bool)gameObject.GetComponent<TransitionScreen>())
				{
					component.layer = 7;
				}
				else
				{
					component.layer = 5;
				}
			}
			Main.CONTAINER.AddToLayer(gameObject, component.layer, front: true, !component.layer.Equals(8));
			int num = component.layer * 1000 + i * 200;
			component.UpdateSortingLayers(num);
			if (_classes == null || _classes[i] == null)
			{
				continue;
			}
			for (int j = 0; j < _classes[i].Length; j++)
			{
				switch (_classes[i][j])
				{
				case 0:
					if (_currencyCreditsTile != null)
					{
						Main.CONTAINER.AddToLayer(_currencyCreditsTile, component.layer, front: true, center: false, resize: false);
						_currencyCreditsTile.GetComponent<Canvas>().sortingOrder = 1 + num;
					}
					break;
				case 1:
					if (_currencyGoldTile != null)
					{
						Main.CONTAINER.AddToLayer(_currencyGoldTile, component.layer, front: true, center: false, resize: false);
						_currencyGoldTile.GetComponent<Canvas>().sortingOrder = 1 + num;
					}
					break;
				case 2:
					if (_currencyEnergyTile != null)
					{
						Main.CONTAINER.AddToLayer(_currencyEnergyTile, component.layer, front: true, center: false, resize: false);
						_currencyEnergyTile.GetComponent<Canvas>().sortingOrder = 1 + num;
					}
					break;
				case 3:
					if (_currencyTicketsTile != null)
					{
						Main.CONTAINER.AddToLayer(_currencyTicketsTile, component.layer, front: true, center: false, resize: false);
						_currencyTicketsTile.GetComponent<Canvas>().sortingOrder = 1 + num;
					}
					break;
				}
			}
		}
	}

	public void MoveCurrencyTileToFront(GameObject tile)
	{
		tile.transform.SetAsLastSibling();
	}

	public void MoveCurrencyTileToBack(GameObject tile)
	{
		tile.transform.SetAsFirstSibling();
	}

	public void SetCurrencyTiles(GameObject currencyCreditsTile, GameObject currencyGoldTile, GameObject currencyEnergyTile, GameObject currencyTicketsTile)
	{
		_currencyCreditsTile = currencyCreditsTile;
		_currencyGoldTile = currencyGoldTile;
		_currencyEnergyTile = currencyEnergyTile;
		_currencyTicketsTile = currencyTicketsTile;
	}

	public void ClearAllWindows(Type avoid = null, bool removeChat = true)
	{
		foreach (GameObject item in new List<GameObject>(_dialogs))
		{
			if (item != null)
			{
				WindowsMain windowsMain = item.GetComponent(typeof(WindowsMain)) as WindowsMain;
				if (windowsMain != null && (avoid == null || windowsMain.GetComponent(avoid) == null) && (removeChat || windowsMain.layer != 8))
				{
					windowsMain.CloseWithoutConfirmation(avoid: false);
				}
			}
		}
	}

	public void ShowCurrencies(bool show)
	{
		bool flag = false;
		bool flag2 = false;
		if (show)
		{
			flag = VariableBook.GameRequirementMet(39);
			flag2 = VariableBook.GameRequirementMet(1);
		}
		_currencyCreditsTile.SetActive(flag);
		_currencyGoldTile.SetActive(flag);
		_currencyEnergyTile.SetActive(flag);
		_currencyTicketsTile.SetActive(flag2 && flag);
		if (show)
		{
			GameObject[] array = new GameObject[4] { _currencyCreditsTile, _currencyGoldTile, _currencyEnergyTile, _currencyTicketsTile };
			for (int i = 0; i < array.Length; i++)
			{
				array[i].GetComponentInChildren<CurrencyBarFill>().Init();
			}
			RepositionCurrencyTiles();
		}
	}

	private void RepositionCurrencyTiles()
	{
		RectTransform rectTransform = _currencyGoldTile.transform as RectTransform;
		RectTransform rectTransform2 = _currencyCreditsTile.transform as RectTransform;
		RectTransform rectTransform3 = _currencyEnergyTile.transform as RectTransform;
		float num = 2f;
		if (_currencyTicketsTile.activeSelf)
		{
			rectTransform.anchoredPosition = new Vector2(0f - rectTransform.sizeDelta.x * rectTransform.localScale.x * 0.5f - num * 0.5f, rectTransform.anchoredPosition.y);
			rectTransform2.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x - rectTransform.sizeDelta.x * rectTransform.localScale.x * 0.5f - rectTransform2.sizeDelta.x * rectTransform2.localScale.x * 0.5f - num, rectTransform2.anchoredPosition.y);
			rectTransform3.anchoredPosition = new Vector2(rectTransform3.sizeDelta.x * rectTransform3.localScale.x * 0.5f + num * 0.5f, rectTransform3.anchoredPosition.y);
		}
		else
		{
			rectTransform.anchoredPosition = new Vector2(0f, rectTransform.anchoredPosition.y);
			rectTransform2.anchoredPosition = new Vector2(0f - rectTransform.sizeDelta.x * rectTransform.localScale.x * 0.5f - rectTransform2.sizeDelta.x * rectTransform2.localScale.x * 0.5f - num, rectTransform2.anchoredPosition.y);
			rectTransform3.anchoredPosition = new Vector2(rectTransform.sizeDelta.x * rectTransform.localScale.x * 0.5f + rectTransform2.sizeDelta.x * rectTransform2.localScale.x * 0.5f + num, rectTransform2.anchoredPosition.y);
		}
	}

	public void ShowChatUI()
	{
		if (_chatObjects.Count > 0)
		{
			return;
		}
		Transform fromResources = GetFromResources("ui/chat/" + typeof(ChatTiles).Name);
		fromResources.transform.SetParent(_chatTileParent);
		fromResources.transform.localPosition = Vector3.zero;
		fromResources.transform.localScale = Vector3.one * 0.9f;
		_chatTiles = fromResources.GetComponent<ChatTiles>();
		_chatTiles.LoadDetails();
		foreach (GameObject chatTile in _chatTiles.chatTiles)
		{
			_chatObjects.Add(chatTile);
			UpdateDialogUI();
		}
	}

	public void SetChatUIDefaultParent(Transform newParent)
	{
		_chatTileParent = newParent;
	}

	public void SetChatUIPosition(Vector2 position)
	{
		if (_chatTiles != null)
		{
			_chatTiles.transform.position = position;
		}
	}

	public void SetChatUISize(Vector2 sizeDelta)
	{
		if (_chatTiles != null)
		{
			((RectTransform)_chatTiles.transform).sizeDelta = sizeDelta;
		}
	}

	public void SetChatUIVisibility(bool visible)
	{
		if (_chatObjects.Count <= 0)
		{
			return;
		}
		foreach (GameObject chatObject in _chatObjects)
		{
			chatObject.SetActive(visible);
		}
	}

	public void HideChatUI()
	{
		if (_chatObjects.Count > 0)
		{
			foreach (GameObject chatObject in _chatObjects)
			{
				UnityEngine.Object.Destroy(chatObject);
			}
			_chatObjects.Clear();
		}
		UnityEngine.Object.Destroy(_chatTiles.gameObject);
		_chatTiles = null;
	}

	public void ShowBattleUI()
	{
		if (_battleObjects.Count > 0)
		{
			return;
		}
		Transform fromResources = GetFromResources("ui/battle/" + typeof(BattleTiles).Name);
		Main.CONTAINER.AddToLayer(fromResources.gameObject, 6, front: false, center: false, resize: false);
		_battleTiles = fromResources.GetComponent<BattleTiles>();
		_battleTiles.LoadDetails();
		foreach (GameObject battleTile in _battleTiles.battleTiles)
		{
			_battleObjects.Add(battleTile);
			UpdateDialogUI();
		}
	}

	public Component GetBattleUI(Type cl)
	{
		if (_battleObjects == null)
		{
			return null;
		}
		foreach (GameObject battleObject in _battleObjects)
		{
			if (battleObject.GetComponent(cl) != null)
			{
				return battleObject.GetComponent(cl);
			}
		}
		return null;
	}

	public void HideBattleUI()
	{
		if (_battleObjects != null && _battleObjects.Count > 0)
		{
			foreach (GameObject battleObject in _battleObjects)
			{
				if (battleObject != null)
				{
					UnityEngine.Object.Destroy(battleObject);
				}
			}
			_battleObjects.Clear();
		}
		if (_battleTiles != null && _battleTiles.gameObject != null)
		{
			UnityEngine.Object.Destroy(_battleTiles.gameObject);
		}
		if (_battleTiles != null)
		{
			UnityEngine.Object.Destroy(_battleTiles.gameObject);
			_battleTiles = null;
		}
	}

	public void UpdateRestrictions()
	{
		foreach (GameObject chatObject in _chatObjects)
		{
			if (chatObject.GetComponent<MenuInterfaceChatTile>() != null && GameData.instance.PROJECT.dungeon == null)
			{
				chatObject.GetComponent<MenuInterfaceChatTile>().DoUpdate();
			}
		}
		foreach (GameObject battleObject in _battleObjects)
		{
			if (battleObject.GetComponent<MenuInterfaceAutoPilotTile>() != null)
			{
				battleObject.GetComponent<MenuInterfaceAutoPilotTile>().DoUpdate();
			}
		}
	}

	public void ShowPlatform()
	{
		switch (AppInfo.platform)
		{
		case 1:
			Util.OpenURL("https://play.google.com/store/apps/details?id=com.kongregate.mobile.bitheroes.google");
			break;
		case 2:
			Util.OpenURL("https://itunes.apple.com/us/app/bit-heroes/id1176312930?ls=1&mt=8");
			break;
		}
	}

	public void ShowForums()
	{
		Util.OpenURL(VariableBook.gameForumsURL);
	}

	public bool ShowNBP()
	{
		PaymentRef firstPaymentByType = PaymentBook.GetFirstPaymentByType(3);
		if (firstPaymentByType == null)
		{
			return false;
		}
		if (HasDialogByClass(typeof(PaymentNBPWindow)))
		{
			return false;
		}
		_ = AppInfo.platform;
		NewPaymentNBPWindow(AppInfo.platform, firstPaymentByType);
		return true;
	}

	public void ShowChat(ChatWindow chat, int layer = -1)
	{
		chat.ForceScrollDown(forceBg: true, chat.bg);
		_dialogs.Add(chat.gameObject);
		_classes.Add(null);
		UpdateLayer(chat.transform, layer);
		chat.GetComponent<WindowsMain>().SCROLL_OUT_START.AddListener(OnWindowScrollOut);
		chat.GetComponent<WindowsMain>().DESTROYED.AddListener(OnWindowDestroyed);
	}

	public void NewWindow(int layer = -1)
	{
	}

	public void showAugmentError(string error, bool close = true)
	{
		D.LogError("AugmentError " + error);
		NewClosablePromptMessageWindow(Language.GetString("error_name"), error, Language.GetString("ui_craft"), Language.GetString("ui_shop"), onAugmentErrorCraft, onAugmentErrorShop);
	}

	private void onAugmentErrorCraft()
	{
		NewCraftTradeWindow(CraftBook.getItemsByResultType(15));
	}

	private void onAugmentErrorShop()
	{
		NewShopWindow(new int[4] { 0, 1, 2, 3 }, ShopWindow.TAB_GEAR);
	}

	public void ShowError(string desc, int layer = 10)
	{
		D.LogError($"{GetType()}:ShowError {desc}");
		NewConfirmMessageWindow(Language.GetString("error_name"), desc, null, null, null, layer);
	}

	public string GetErrorName()
	{
		return Language.GetString("error_name");
	}

	public void ShowErrorCode(int errorCode, int layer = 10)
	{
		D.LogError($"{GetType()}:ShowErrorCode {errorCode}");
		string message = ErrorCode.getErrorMessage(errorCode);
		switch (errorCode)
		{
		case 0:
			break;
		case 24:
			GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("ui_update"), message, Language.GetString("ui_update"), ShowPlatform, null, layer);
			break;
		case 25:
			GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("ui_update"), message, Language.GetString("ui_reload"), null, null, layer);
			break;
		case 20:
			NewPromptServiceWindowError(1);
			break;
		case 21:
			NewPromptServiceWindowError(0);
			break;
		case 22:
			NewPromptServiceWindowError(2);
			break;
		case 23:
			NewPromptServiceWindowError(3);
			break;
		case 122:
			NewPromptServiceWindowError(4);
			break;
		case 44:
			NewPromptServiceTypeError(6);
			break;
		case 127:
			NewPromptServiceTypeError(15);
			break;
		case 96:
			NewPromptServiceTypeError(10);
			break;
		case 104:
			NewPromptServiceTypeError(11);
			break;
		default:
			GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString("error_name"), message, null, null, null, layer);
			break;
		}
		void NewPromptServiceTypeError(int type)
		{
			GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("error_name"), message, null, null, delegate
			{
				GameData.instance.windowGenerator.ShowServiceType(type);
			}, null, null, layer);
		}
		void NewPromptServiceWindowError(int tab)
		{
			GameData.instance.windowGenerator.NewPromptMessageWindow(Language.GetString("error_name"), message, null, null, delegate
			{
				GameData.instance.windowGenerator.NewServiceWindow(tab);
			}, null, null, layer);
		}
	}

	public void ShowServices(int tab, ServiceRef highlightedRef = null)
	{
		ServiceWindow serviceWindow = GetDialogByClass(typeof(ServiceWindow)) as ServiceWindow;
		if (serviceWindow != null)
		{
			serviceWindow.SetTab(tab);
			return;
		}
		NewServiceWindow(tab, highlightedRef, new int[4] { 0, 1, 2, 3 });
	}

	public void ShowServiceType(int type)
	{
		if (!(GetDialogByClass(typeof(ServiceListWindow)) as ServiceListWindow != null))
		{
			D.Log(ServiceBook.GetServicesByType(type).Count.ToString());
			NewServiceListWindow(ServiceRef.GetServiceName(type), ServiceBook.GetServicesByType(type), new int[4] { 1, 0, 2, 3 });
		}
	}

	public void UpdateChatWindow(int layer = -1)
	{
		ChatWindow chatWindow = GetDialogByClass(typeof(ChatWindow)) as ChatWindow;
		if (chatWindow != null)
		{
			chatWindow.DoUpdate();
		}
	}

	public Conversation ShowConversation(int charID, string name = "")
	{
		Conversation conversation = GameData.instance.PROJECT.character.getConversation(charID);
		if (conversation == null)
		{
			conversation = new Conversation(new Vector2(0f, 0f), charID, name);
			GameData.instance.PROJECT.character.addConversation(conversation);
		}
		if (VariableBook.GameRequirementMet(4) && !GameData.instance.tutorialManager.hasPopup)
		{
			OpenConversation(conversation);
		}
		return conversation;
	}

	private ConversationWindow OpenConversation(Conversation conversation)
	{
		if (conversationsWindow != null)
		{
			foreach (ConversationWindow item in conversationsWindow)
			{
				if (item != null && item.conversation.charID == conversation.charID)
				{
					return item;
				}
			}
		}
		return NewConversationWindow(conversation, 8);
	}

	public void ShowAdminTools()
	{
		if (GameData.instance.PROJECT.character == null || !GameData.instance.PROJECT.character.admin)
		{
			return;
		}
		if (!GameData.instance.PROJECT.character.adminLoggedIn)
		{
			if (!HasDialogByClass(typeof(AdminLoginWindow)))
			{
				NewAdminLoginWindow();
			}
		}
		else if (!HasDialogByClass(typeof(AdminWindow)))
		{
			NewAdminWindow();
		}
	}

	public CharacterDisplay GetCharacterDisplay(CharacterPuppetInfo puppetInfo, int layer = -1)
	{
		string text = ((puppetInfo is CharacterPuppetInfoIMXG0) ? "IMXG0" : "");
		return UnityEngine.Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<CharacterDisplay>("ui/character/CharacterDisplay" + text));
	}

	public void ShowFamiliar(int familiarID, bool mine = false)
	{
		FamiliarRef familiarRef = FamiliarBook.Lookup(familiarID);
		if (!(familiarRef == null))
		{
			NewFamiliarWindow(familiarRef, null, mine);
		}
	}

	public void ShowItem(ItemData itemData, bool compare = true, bool added = false, bool forceNonEquipment = false, string name = null, int layer = -1)
	{
		List<ItemData> list = new List<ItemData>();
		list.Add(itemData);
		ShowItems(list, compare, added, name, large: false, forceNonEquipment, null, layer);
	}

	public ItemListWindow ShowItems(List<ItemData> items, bool compare = true, bool added = false, string name = null, bool large = false, bool forceNonEquipment = false, string helpText = null, int layer = -1, string closeWord = null, bool forceItemEnabled = false)
	{
		if (name == null)
		{
			name = Language.GetString("ui_items");
		}
		return NewItemListWindow(items, compare, added, name, large, forceNonEquipment, select: false, helpText, null, layer, closeWord, forceItemEnabled);
	}

	public void ShowDialogMessage(string title, string message, bool close = true)
	{
		NewConfirmMessageWindow(title, message, null, null, null, 10);
	}

	public void ShowPlayer(int charID = 0, int layer = -1, string name = null, GameObject dialogParent = null)
	{
		_showPlayerLayer = layer;
		CharacterData characterData = null;
		bool online = false;
		if (charID == GameData.instance.PROJECT.character.id)
		{
			characterData = GameData.instance.PROJECT.character.toCharacterData();
			online = true;
		}
		else
		{
			FriendData friendData = GameData.instance.PROJECT.character.getFriendData(charID);
			GuildMemberData guildMemberData = ((GameData.instance.PROJECT.character.guildData != null) ? GameData.instance.PROJECT.character.guildData.getMember(charID) : null);
			if (friendData != null)
			{
				characterData = friendData.characterData;
				online = friendData.online;
			}
			else if (guildMemberData != null)
			{
				characterData = guildMemberData.characterData;
				online = guildMemberData.online;
			}
			else if (GameData.instance.PROJECT.instance != null)
			{
				InstancePlayer instancePlayer = GameData.instance.PROJECT.instance.GetPlayer(charID);
				if (instancePlayer != null)
				{
					characterData = instancePlayer.characterData;
					online = true;
				}
			}
			if (characterData == null)
			{
				EventTargetWindow eventTargetWindow = GetDialogByClass(typeof(EventTargetWindow)) as EventTargetWindow;
				if (eventTargetWindow != null)
				{
					CharacterData characterData2 = eventTargetWindow.GetCharacterData(charID);
					if (characterData2 != null)
					{
						characterData = characterData2;
					}
				}
			}
		}
		if (characterData != null)
		{
			if (HasDialogByClass(typeof(ChatWindow)))
			{
				GameData.instance.windowGenerator.NewCharacterProfileWindow(characterData, online, 8, dialogParent);
			}
			else
			{
				GameData.instance.windowGenerator.NewCharacterProfileWindow(characterData, online, _showPlayerLayer, dialogParent);
			}
			return;
		}
		GameData.instance.main.ShowLoading();
		if (name != null && name.IndexOf("#") > -1)
		{
			CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(9), OnGetProfile);
		}
		else
		{
			_nametmp = name;
			CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(9), OnGetProfileWhitoutHerotag);
		}
		CharacterDALC.instance.doGetProfile(charID, name);
	}

	public void ShowPlayer(CharacterData characterData, int layer = -1, string name = null, GameObject dialogParent = null)
	{
		bool online = false;
		int charID = characterData.charID;
		if (charID == GameData.instance.PROJECT.character.id)
		{
			online = true;
		}
		else
		{
			FriendData friendData = GameData.instance.PROJECT.character.getFriendData(charID);
			GuildMemberData guildMemberData = ((GameData.instance.PROJECT.character.guildData != null) ? GameData.instance.PROJECT.character.guildData.getMember(charID) : null);
			if (friendData != null)
			{
				online = friendData.online;
			}
			else if (guildMemberData != null)
			{
				online = guildMemberData.online;
			}
			else if (GameData.instance.PROJECT.instance != null && GameData.instance.PROJECT.instance.GetPlayer(charID) != null)
			{
				online = true;
			}
		}
		if (HasDialogByClass(typeof(ChatWindow)))
		{
			GameData.instance.windowGenerator.NewCharacterProfileWindow(characterData, online, 8, dialogParent);
		}
		else
		{
			GameData.instance.windowGenerator.NewCharacterProfileWindow(characterData, online, _showPlayerLayer, dialogParent);
		}
	}

	public void ShowTeammate(TeammateData teammate, int layer = -1, string name = null)
	{
		CharacterData characterData = null;
		bool online = false;
		int id = teammate.id;
		if (id == GameData.instance.PROJECT.character.id)
		{
			characterData = GameData.instance.PROJECT.character.toCharacterData(duplicateMounts: true);
			online = true;
		}
		else
		{
			FriendData friendData = GameData.instance.PROJECT.character.getFriendData(id);
			GuildMemberData guildMemberData = ((GameData.instance.PROJECT.character.guildData != null) ? GameData.instance.PROJECT.character.guildData.getMember(id) : null);
			if (friendData != null)
			{
				characterData = friendData.characterData;
				online = friendData.online;
			}
			else if (guildMemberData != null)
			{
				characterData = guildMemberData.characterData;
				online = guildMemberData.online;
			}
			else if (GameData.instance.PROJECT.instance != null)
			{
				InstancePlayer instancePlayer = GameData.instance.PROJECT.instance.GetPlayer(id);
				if (instancePlayer != null)
				{
					characterData = instancePlayer.characterData;
					online = true;
				}
			}
			if (characterData == null)
			{
				EventTargetWindow eventTargetWindow = GetDialogByClass(typeof(EventTargetWindow)) as EventTargetWindow;
				if (eventTargetWindow != null)
				{
					CharacterData characterData2 = eventTargetWindow.GetCharacterData(id);
					if (characterData2 != null)
					{
						characterData = characterData2;
					}
				}
			}
		}
		if (characterData != null)
		{
			if (teammate.armoryID > 0)
			{
				characterData.armory.SetCurrentArmoryEquipmentSlotByID(teammate.armoryID);
				Equipment equipment = ArmoryEquipment.ArmoryEquipmentToEquipment(characterData.armory.currentArmoryEquipmentSlot);
				Mounts mounts = new Mounts(characterData.armory.currentArmoryEquipmentSlot.mount, characterData.mounts.getCosmeticMount((int)characterData.armory.currentArmoryEquipmentSlot.mountCosmetic), characterData.mounts.mounts);
				characterData.setEquipment(equipment);
				characterData.SetMounts(mounts);
				Runes runes = new Runes(characterData.armory.currentArmoryEquipmentSlot.runes.runeSlots, characterData.armory.currentArmoryEquipmentSlot.runes.runeSlotsMemory);
				characterData.runes = runes;
			}
			CharacterProfileWindow characterProfileWindow = ((!HasDialogByClass(typeof(ChatWindow))) ? GameData.instance.windowGenerator.NewCharacterProfileWindow(characterData, online, _showPlayerLayer) : GameData.instance.windowGenerator.NewCharacterProfileWindow(characterData, online, 8));
			if (id == GameData.instance.PROJECT.character.id && teammate.armoryID > 0)
			{
				characterProfileWindow.OverrideCharacterStats(teammate.power, teammate.stamina, teammate.agility);
			}
		}
		else
		{
			GameData.instance.main.ShowLoading();
			if (name != null && name.IndexOf("#") > -1)
			{
				CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(9), OnGetProfile);
			}
			else
			{
				_nametmp = name;
				CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(9), OnGetProfileWhitoutHerotag);
			}
			CharacterDALC.instance.doGetProfile(id, name);
		}
	}

	public void ShowHistoryNames(int charID)
	{
		GameData.instance.main.ShowLoading();
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(60), OnGetNameHistory);
		CharacterDALC.instance.doGetNameHistory(charID);
	}

	private void OnGetNameHistory(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(60), OnGetNameHistory);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		string utfString = sfsob.GetUtfString("characterName");
		string[] utfStringArray = sfsob.GetUtfStringArray("names");
		string[] utfStringArray2 = sfsob.GetUtfStringArray("dates");
		List<CharacterInfoData> list = new List<CharacterInfoData>();
		CharacterInfoData characterInfoData = new CharacterInfoData(utfString);
		if (utfStringArray != null)
		{
			for (int i = 0; i < utfStringArray.Length; i++)
			{
				string text = utfStringArray[i];
				string value = utfStringArray2[i];
				characterInfoData.addValue(text, value);
			}
		}
		list.Add(characterInfoData);
		if (HasDialogByClass(typeof(ChatWindow)))
		{
			NewCharacterInfoListWindow(list, Language.GetString("ui_info_names_history"), null, 8);
		}
		else
		{
			NewCharacterInfoListWindow(list, Language.GetString("ui_info_names_history"));
		}
	}

	private void OnGetProfile(BaseEvent baseEvent)
	{
		DALCEvent obj = baseEvent as DALCEvent;
		GameData.instance.main.HideLoading();
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(9), OnGetProfile);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			GameData.instance.windowGenerator.ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		UserData userData = UserData.fromSFSObject(sfsob);
		if (HasDialogByClass(typeof(ChatWindow)))
		{
			GameData.instance.windowGenerator.NewCharacterProfileWindow(userData.characterData, userData.online, 8);
		}
		else
		{
			GameData.instance.windowGenerator.NewCharacterProfileWindow(userData.characterData, userData.online, _showPlayerLayer);
		}
	}

	private void OnGetProfileWhitoutHerotag(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(9), OnGetProfileWhitoutHerotag);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			if (sfsob.GetInt("err0") == 9)
			{
				ShowPosiblePlayers();
			}
			else
			{
				ShowErrorCode(sfsob.GetInt("err0"));
			}
			return;
		}
		UserData userData = UserData.fromSFSObject(sfsob);
		if (HasDialogByClass(typeof(ChatWindow)))
		{
			NewCharacterProfileWindow(userData.characterData, userData.online, 8);
		}
		else if (HasDialogByClass(typeof(FriendRecommendWindow)))
		{
			FriendRecommendWindow friendRecommendWindow = GetDialogByClass(typeof(FriendRecommendWindow)) as FriendRecommendWindow;
			if (friendRecommendWindow != null)
			{
				NewCharacterProfileWindow(userData.characterData, userData.online, _showPlayerLayer, friendRecommendWindow.gameObject);
			}
			else
			{
				NewCharacterProfileWindow(userData.characterData, userData.online, _showPlayerLayer);
			}
		}
		else
		{
			NewCharacterProfileWindow(userData.characterData, userData.online, _showPlayerLayer);
		}
	}

	public void ShowPosiblePlayers()
	{
		if (_nametmp != null)
		{
			GameData.instance.main.ShowLoading();
			CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(59), OnGetProfilesPosibles);
			CharacterDALC.instance.doGetProfilesPosibles(_nametmp);
			_nametmp = null;
		}
	}

	private void OnGetProfilesPosibles(BaseEvent e)
	{
		DALCEvent obj = e as DALCEvent;
		GameData.instance.main.HideLoading();
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(59), OnGetProfilesPosibles);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			ShowErrorCode(sfsob.GetInt("err0"));
			return;
		}
		int @int = sfsob.GetInt(ServerConstants.TOTAL_CHARACTERS_NAMES);
		if (@int > 0)
		{
			ISFSArray sFSArray = sfsob.GetSFSArray(ServerConstants.CHARACTERS_DATA);
			List<CharacterHeroTagData> list = new List<CharacterHeroTagData>();
			for (int i = 0; i < sFSArray.Size(); i++)
			{
				ISFSObject sFSObject = sFSArray.GetSFSObject(i);
				list.Add(CharacterHeroTagData.FromSFSObject(sFSObject));
			}
			ChatWindow chatWindow = GetDialogByClass(typeof(ChatWindow)) as ChatWindow;
			NewCharactersSearchListWindow(list, @int, showSelect: false, null, (chatWindow != null) ? chatWindow.gameObject : null, (chatWindow != null) ? 8 : (-1));
		}
		else
		{
			ShowError(Language.GetString("ui_user_not_found"));
		}
	}

	public bool ShowBoosters(int itemIndex = 0)
	{
		List<BoosterRef> activeBoosters = GameData.instance.PROJECT.character.activeBoosters;
		if (activeBoosters.Count < 1)
		{
			return false;
		}
		NewPaymentBoostersWindow(activeBoosters, itemIndex);
		return true;
	}

	public bool ShowNBPZ(int itemIndex = 0)
	{
		List<PaymentRef> allPaymentsByTypeAndZone = PaymentBook.GetAllPaymentsByTypeAndZone(3, GameData.instance.PROJECT.character.zoneCompleted);
		if (allPaymentsByTypeAndZone.Count < 1)
		{
			return false;
		}
		_ = AppInfo.platform;
		NewPaymentNBPWindow(AppInfo.platform, allPaymentsByTypeAndZone[itemIndex], allPaymentsByTypeAndZone, itemIndex);
		return true;
	}

	private void CenterAndAddCanvasParent(Transform currentWindow)
	{
		if (!(currentWindow == null))
		{
			if (canvas != null)
			{
				currentWindow.SetParent(canvas.transform);
			}
			RectTransform component = currentWindow.GetComponent<RectTransform>();
			if (component != null)
			{
				component.anchoredPosition = Vector2.zero;
				component.sizeDelta = Vector2.one;
				component.localScale = Vector3.one;
			}
		}
	}

	private void UpdateLayer(Transform currentWindow, int layer)
	{
		currentWindow.GetComponent<WindowsMain>().layer = layer;
		UpdateDialogUI();
	}

	private void AddScrollOutAndDestroyListeners(Transform currentWindow)
	{
		currentWindow.GetComponent<WindowsMain>().SCROLL_OUT_START.AddListener(OnWindowScrollOut);
		currentWindow.GetComponent<WindowsMain>().DESTROYED.AddListener(OnWindowDestroyed);
	}

	public int GetDialogLayerCount(int layer, Type exception = null)
	{
		int num = 0;
		for (int i = 0; i < _dialogs.Count; i++)
		{
			GameObject gameObject = _dialogs[i];
			if (!(gameObject != null))
			{
				continue;
			}
			WindowsMain component = gameObject.GetComponent<WindowsMain>();
			if (component != null && component.layer == layer)
			{
				if (exception == null)
				{
					num++;
				}
				else if (component.GetComponent(exception) == null)
				{
					num++;
				}
			}
		}
		return num;
	}

	public int GetDialogLayerCount(int layer, Type[] exceptions = null)
	{
		if (exceptions != null && exceptions.Length == 1)
		{
			return GetDialogLayerCount(layer, exceptions[0]);
		}
		int num = 0;
		for (int i = 0; i < _dialogs.Count; i++)
		{
			GameObject gameObject = _dialogs[i];
			if (!(gameObject != null))
			{
				continue;
			}
			WindowsMain component = gameObject.GetComponent<WindowsMain>();
			if (!(component != null) || component.layer != layer)
			{
				continue;
			}
			if (exceptions == null)
			{
				num++;
				continue;
			}
			bool flag = false;
			foreach (Type type in exceptions)
			{
				if (component.TryGetComponent(type, out var _))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				num++;
			}
		}
		return num;
	}

	public Transform GetFromResources(string link)
	{
		return UnityEngine.Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<Transform>(link));
	}

	public void NewAbilityListWindow(List<AbilityRef> abilities, int power, float bonus, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/ability/" + typeof(AbilityListWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<AbilityListWindow>().LoadDetails(abilities, power, bonus);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewAdWindow(int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/ad/" + typeof(AdWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewAdGorWindow(AdGor adgor, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/adgor/" + typeof(AdGorWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<AdGorWindow>().LoadDetails(adgor);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewAdminWindow(int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/admin/" + typeof(AdminWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewAdminCharacterWindow(AdminCharacterData characterData, GameObject dialogParent, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/admin/" + typeof(AdminCharacterWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<AdminCharacterWindow>().LoadDetails(characterData);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewAdminServerWindow(GameObject dialogParent, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/admin/" + typeof(AdminServerWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewAdminCharacterItemsWindow(int charID, GameObject dialogParent, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/admin/" + typeof(AdminCharacterItemsWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<AdminCharacterItemsWindow>().LoadDetails(charID);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewAdminRenameWindow(string currentName, int type, string herotag = "", GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/admin/" + typeof(AdminRenameWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<AdminRenameWindow>().LoadDetails(currentName, type, herotag);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewAdminGuildWindow(AdminGuildData guildData, GameObject dialogParent, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/admin/" + typeof(AdminGuildWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<AdminGuildWindow>().LoadDetails(guildData);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewAdminCharacterPurchasesWindow(int charID = 0, GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/admin/" + typeof(AdminCharacterPurchasesWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<AdminCharacterPurchasesWindow>().LoadDetails(charID);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewAdminCharacterPlatformsWindow(int charID, List<CharacterPlatformData> platforms, GameObject dialogParent, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/admin/" + typeof(AdminCharacterPlatformsWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<AdminCharacterPlatformsWindow>().LoadDetails(charID, platforms);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewAdminCharacterPlatformAddWindow(int charID, GameObject dialogParent, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/admin/" + typeof(AdminCharacterPlatformAddWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<AdminCharacterPlatformAddWindow>().LoadDetails(charID);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewAdminLoginWindow(int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/admin/" + typeof(AdminLoginWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewAdminUploadWindow(int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/admin/" + typeof(AdminUploadWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public AugmentSelectWindow NewAugmentSelectWindow(Augments augments, bool changeable = false, FamiliarRef familiarRef = null, int slot = -1, GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/augment/" + typeof(AugmentSelectWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<AugmentSelectWindow>().LoadDetails(augments, changeable, familiarRef, slot);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<AugmentSelectWindow>();
	}

	public void NewAugmentWindow(AugmentData augmentData, GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/augment/" + typeof(AugmentWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<AugmentWindow>().LoadDetails(augmentData);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewAugmentOptionsWindow(FamiliarRef familiarRef, int slot, AugmentData tile, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/augment/" + typeof(AugmentOptionsWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<AugmentOptionsWindow>().LoadDetails(familiarRef, slot, tile);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public AugmentSlotSelectWindow NewAugmentSlotSelectWindow(AugmentData augmentData, FamiliarRef familiarRef, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/augment/" + typeof(AugmentSlotSelectWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<AugmentSlotSelectWindow>().LoadDetails(augmentData, familiarRef);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<AugmentSlotSelectWindow>();
	}

	public void NewBattleTestWindow(int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/battle/" + typeof(BattleTestWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public Transform NewBattleEntitySelectWindow(int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/battle/" + typeof(BattleEntitySelectWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources;
	}

	public void NewBattleCaptureWindow(BattleEntity entity, int[] classes = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/battle/" + typeof(BattleCaptureWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<BattleCaptureWindow>().LoadDetails(entity);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(classes);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public Transform NewDefeatableVictoryWindow(int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/victory/" + typeof(DefeatableVictoryWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources;
	}

	public TransitionScreen NewBattleTransitionScreen(string sceneName, string currentSceneName, UnityAction completeAction = null, UnityAction toggleAction = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/battle/" + typeof(BattleTransitionScreen).Name);
		CenterAndAddCanvasParent(fromResources);
		BattleTransitionScreen component = fromResources.GetComponent<BattleTransitionScreen>();
		component.LoadDetails(sceneName, currentSceneName, completeAction, toggleAction);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return component;
	}

	public BattleUI NewBattleUI(Battle battle, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/battle/" + typeof(BattleUI).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<BattleUI>().LoadDetails(battle);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<BattleUI>();
	}

	public void NewBattleStatsWindow(List<BattleStat> battleStats, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/battle/" + typeof(BattleStatsWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<BattleStatsWindow>().LoadDetails(battleStats);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewBrawlWindow(GameObject dialogParent = null, int[] classes = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/brawl/" + typeof(BrawlWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(classes);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewBrawlFilterWindow(GameObject dialogParent, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/brawl/" + typeof(BrawlFilterWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewBrawlCreateWindow(GameObject dialogParent = null, int[] classes = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/brawl/" + typeof(BrawlCreateWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(classes);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewBrawlCreateDifficultyWindow(BrawlRef brawlRef, GameObject dialogParent = null, int[] classes = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/brawl/" + typeof(BrawlCreateDifficultyWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<BrawlCreateDifficultyWindow>().LoadDetails(brawlRef);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(classes);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewBrawlRoomWindow(BrawlRoom brawlRoom, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/brawl/" + typeof(BrawlRoomWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<BrawlRoomWindow>().LoadDetails(brawlRoom);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewBrawlRoomInviteWindow(BrawlRoom brawlRoom, GameObject dialogParent, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/brawl/" + typeof(BrawlRoomInviteWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<BrawlRoomInviteWindow>().LoadDetails(brawlRoom);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public Transform NewLogInWindow(int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/character/" + typeof(CharacterLoginEmailWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<CharacterLoginEmailWindow>().LoadDetails();
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		return fromResources;
	}

	public Transform NewUserCreationWindow(string email, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/character/" + typeof(CharacterCreateWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<CharacterCreateWindow>().LoadDetails(email);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		return fromResources;
	}

	public Transform NewUserWindow(int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/character/" + typeof(CharacterLoginEmailNewUserWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		return fromResources;
	}

	public void NewCharacterWindow(int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/character/" + typeof(CharacterWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		StoreWindowReference(fromResources.GetComponent<CharacterWindow>(), Window.Character);
	}

	public void NewCharacterStatWindow(GameObject parent = null, string callBack = "", int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/character/" + typeof(CharacterStatWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		if (parent != null)
		{
			fromResources.GetComponent<CharacterStatWindow>().SetParentCall(parent, callBack);
		}
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public CharacterProfileWindow NewCharacterProfileWindow(CharacterData characterData, bool online = false, int layer = -1, GameObject dialogParent = null)
	{
		Transform fromResources = GetFromResources("ui/character/" + typeof(CharacterProfileWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		CharacterProfileWindow component = fromResources.GetComponent<CharacterProfileWindow>();
		component.LoadDetails(characterData, online);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return component;
	}

	public void NewCharacterInfoWindow(int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/character/" + typeof(CharacterInfoWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<CharacterInfoWindow>().LoadDetails();
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public CharacterInfoListWindow NewCharacterInfoListWindow(List<CharacterInfoData> info, string name = null, GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/character/" + typeof(CharacterInfoListWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<CharacterInfoListWindow>().LoadDetails(info, name);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<CharacterInfoListWindow>();
	}

	public HeroSelectWindow NewHeroSelectWindow(CharacterData heroSelected = null, bool showCloseBtn = true, bool forceRelog = false, int layer = -1)
	{
		HeroSelectWindow component = GetFromResources("ui/hero/" + typeof(HeroSelectWindow).Name).GetComponent<HeroSelectWindow>();
		CenterAndAddCanvasParent(component.transform);
		component.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(component.gameObject);
		_classes.Add(null);
		UpdateLayer(component.transform, layer);
		AddScrollOutAndDestroyListeners(component.transform);
		bool forceRelog2 = forceRelog;
		component.LoadDetails(heroSelected, showCloseBtn, forceRelog2);
		return component;
	}

	public void NewCharacterLoginWindow(int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/character/" + typeof(CharacterLoginWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewCharacterPlatformLinkWindow(CharacterData linkedData, int platform, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/character/" + typeof(CharacterPlatformLinkWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<CharacterPlatformLinkWindow>().LoadDetails(linkedData, platform);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public CharacterCustomizeWindow NewCharacterCustomizeWindow(GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/character/" + typeof(CharacterCustomizeWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<CharacterCustomizeWindow>();
	}

	public void NewCharacterHerotagWindow(string name, string herotag, bool changeable = false, GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/character/" + typeof(CharacterHerotagWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<CharacterHerotagWindow>().LoadDetails(name, herotag, changeable);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public CharacterHerotagChangeWindow NewCharacterHerotagChangeWindow(int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/character/" + typeof(CharacterHerotagChangeWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<CharacterHerotagChangeWindow>().LoadDetails();
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<CharacterHerotagChangeWindow>();
	}

	public CharacterNFTNameChangeWindow NewCharacterNFTNameChangeWindow(int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/character/" + typeof(CharacterNFTNameChangeWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<CharacterNFTNameChangeWindow>().LoadDetails();
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<CharacterNFTNameChangeWindow>();
	}

	public CharactersSearchListWindow NewCharactersSearchListWindow(List<CharacterHeroTagData> list, int total, bool showSelect = false, UnityAction<string> selectCallback = null, GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/character/" + typeof(CharactersSearchListWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<CharactersSearchListWindow>().LoadDetails(list, total, showSelect, selectCallback);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<CharactersSearchListWindow>();
	}

	public CharacterArmoryWindow NewCharacterArmoryWindow(uint idx = 0u, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/character/armory/" + typeof(CharacterArmoryWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		CharacterArmoryWindow component = fromResources.GetComponent<CharacterArmoryWindow>();
		component.LoadDetails(idx);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return component;
	}

	public CharacterArmoryNameChange NewCharacterArmoryNameChange(string currentName = null, GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/character/armory/" + typeof(CharacterArmoryNameChange).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<CharacterArmoryNameChange>().LoadDetails(currentName);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<CharacterArmoryNameChange>();
	}

	public void NewCharacterArmoryInfoWindow(ArmoryEquipment aequip, Character character, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/character/armory/" + typeof(CharacterArmoryInfoWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<CharacterArmoryInfoWindow>().LoadDetails(aequip, character);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public TeammateArmoryWindow NewTeammateArmoryWindow(CharacterData charData, int idx = 0, bool showSelect = true, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/character/armory/" + typeof(TeammateArmoryWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<TeammateArmoryWindow>().LoadDetails(charData, idx, showSelect);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<TeammateArmoryWindow>();
	}

	public void NewChatIgnoresWindow(GameObject dialogParent, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/chat/" + typeof(ChatIgnoresWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public ChatWindow NewChatWindow(int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/chat/" + typeof(ChatWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		if (!fromResources.GetComponent<ChatWindow>().loaded)
		{
			fromResources.GetComponent<ChatWindow>().LoadDetails();
		}
		return fromResources.GetComponent<ChatWindow>();
	}

	public void NewChatPlayerWindow(int charID, string name, string log = "", GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/chat/" + typeof(ChatPlayerWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ChatPlayerWindow>().LoadDetails(charID, name, log);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewChatMuteLogWindow(int charID = 0, GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/chat/" + typeof(ChatMuteLogWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ChatMuteLogWindow>().LoadDetails(charID);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public ChatAgreementWindow NewChatAgreementWindow(int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/chat/" + typeof(ChatAgreementWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<ChatAgreementWindow>();
	}

	public ConversationWindow NewConversationWindow(Conversation conversation, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/chat/" + typeof(ConversationWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		ConversationWindow component = fromResources.GetComponent<ConversationWindow>();
		component.OnWindowClose.AddListener(OnConversationWindowClosed);
		component.LoadDetails(conversation);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		if (conversationsWindow == null)
		{
			conversationsWindow = new List<ConversationWindow>();
		}
		conversationsWindow.Add(component);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<ConversationWindow>();
	}

	private void OnConversationWindowClosed(ConversationWindow conversationWindow)
	{
		conversationsWindow.Remove(conversationWindow);
	}

	public void NewCosmeticsWindow(EquipmentRef equipmentRef = null, MountRef mountRef = null, int layer = -1, bool isArmory = false)
	{
		Transform fromResources = GetFromResources("ui/cosmetic/" + typeof(CosmeticsWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<CosmeticsWindow>().LoadDetails(equipmentRef, mountRef, isArmory);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewArmoryCosmeticsWindow(ArmoryRef equipmentRef = null, MountRef mountRef = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/cosmetic/" + typeof(ArmoryCosmeticsWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ArmoryCosmeticsWindow>().LoadDetails(equipmentRef, mountRef);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewCraftWindow(MenuInterfaceCraftTile tile = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/craft/" + typeof(CraftWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<CraftWindow>().craftTile = tile;
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewCraftTradeWindow(List<CraftTradeRef> tradeRefs, GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/craft/" + typeof(CraftTradeWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<CraftTradeWindow>().LoadDetails(tradeRefs);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewDailyQuestsWindow(int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/daily/" + typeof(DailyQuestsWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public DailyRewardWindow NewDailyRewardWindow(UnityAction onDestroyed = null, int layer = -1)
	{
		DailyRewardWindow dailyRewardWindow = (DailyRewardWindow)GetDialogByClass(typeof(DailyRewardWindow));
		if (dailyRewardWindow == null)
		{
			Transform fromResources = GetFromResources("ui/daily/" + typeof(DailyRewardWindow).Name);
			CenterAndAddCanvasParent(fromResources);
			dailyRewardWindow = fromResources.GetComponent<DailyRewardWindow>();
			dailyRewardWindow.LoadDetails();
			if (onDestroyed != null)
			{
				dailyRewardWindow.DESTROYED.RemoveAllListeners();
				dailyRewardWindow.DESTROYED.AddListener(delegate
				{
					onDestroyed();
				});
			}
			fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
			_dialogs.Add(fromResources.gameObject);
			_classes.Add(null);
			UpdateLayer(fromResources, layer);
			AddScrollOutAndDestroyListeners(fromResources);
		}
		return dailyRewardWindow;
	}

	public void NewCharacterAchievementsWindow(int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/achievements/" + typeof(AchievementsWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public DialogWindow NewPromptMessageWindow(string title = "", string description = "", string okBtnLabel = null, string cancelBtnLabel = null, UnityAction onOkCallback = null, UnityAction onCancelCallback = null, GameObject parent = null, int layer = -1, int[] classes = null, ButtonSquareColor okBtnColor = ButtonSquareColor.Default, ButtonSquareColor cancelBtnColor = ButtonSquareColor.Default)
	{
		Transform fromResources = GetFromResources("ui/dialog/" + typeof(DialogWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<DialogWindow>().LoadPromptMessage(title, description, parent, okBtnLabel, cancelBtnLabel, okBtnColor, cancelBtnColor, onOkCallback, onCancelCallback, null, showCloseBtn: false);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(classes);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<DialogWindow>();
	}

	public DialogWindow NewPromptMessageWindowBig(string title = "", string description = "", string okBtnLabel = null, string cancelBtnLabel = null, UnityAction onOkCallback = null, UnityAction onCancelCallback = null, GameObject parent = null, int layer = -1, int[] classes = null, ButtonSquareColor okBtnColor = ButtonSquareColor.Default, ButtonSquareColor cancelBtnColor = ButtonSquareColor.Default)
	{
		Transform fromResources = GetFromResources("ui/dialog/" + typeof(DialogWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<DialogWindow>().LoadPromptMessageBig(title, description, parent, okBtnLabel, cancelBtnLabel, okBtnColor, cancelBtnColor, onOkCallback, onCancelCallback);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(classes);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<DialogWindow>();
	}

	public DialogWindow NewClosablePromptMessageWindow(string title = "", string description = "", string okBtnLabel = null, string cancelBtnLabel = null, UnityAction onOkCallback = null, UnityAction onCancelCallback = null, GameObject parent = null, int layer = -1, ButtonSquareColor okBtnColor = ButtonSquareColor.Default, ButtonSquareColor cancelBtnColor = ButtonSquareColor.Default)
	{
		Transform fromResources = GetFromResources("ui/dialog/" + typeof(DialogWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<DialogWindow>().LoadClosablePromptMessage(title, description, parent, okBtnLabel, cancelBtnLabel, onOkCallback, okBtnColor, cancelBtnColor, onCancelCallback);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<DialogWindow>();
	}

	public DialogWindow NewConfirmMessageWindow(string title = "", string description = "", string buttonLabel = null, UnityAction callback = null, GameObject parent = null, int layer = -1, ButtonSquareColor btnColor = ButtonSquareColor.Default)
	{
		Transform fromResources = GetFromResources("ui/dialog/" + typeof(DialogWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<DialogWindow>().LoadConfirmMessage(title, description, parent, buttonLabel, btnColor, callback);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<DialogWindow>();
	}

	public void NewMessageWindow(DialogWindow.TYPE type, string title, string description, GameObject parent = null, string callerLink = "", int layer = -1, ButtonSquareColor btnColor = ButtonSquareColor.Default, ButtonSquareColor okBtnColor = ButtonSquareColor.Default, ButtonSquareColor cancelBtnColor = ButtonSquareColor.Default)
	{
		Transform fromResources = GetFromResources("ui/dialog/" + typeof(DialogWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<DialogWindow>().LoadMessage(type, title, description, parent, btnColor, okBtnColor, cancelBtnColor, callerLink);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public DialogPopup NewDialogPopup(DialogRef dialogRef, object data = null, bool showKongButton = false, int layer = -1, bool setSeen = true)
	{
		Transform fromResources = GetFromResources("ui/dialog/" + typeof(DialogPopup).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<DialogPopup>().LoadDetails(dialogRef, data, showKongButton, setSeen);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<DialogPopup>();
	}

	public void ShowCreditsPurchased(int credits, string type, string name = null, bool sendAnalytics = true)
	{
		ItemData itemData = new ItemData(CurrencyBook.Lookup(2), credits);
		List<ItemData> list = new List<ItemData>();
		list.Add(itemData);
		ShowItems(list, compare: true, added: false, name);
		if (sendAnalytics)
		{
			KongregateAnalytics.trackEconomyTransaction(type, ItemData.parseSummary(null, list), itemData.qty, 0);
		}
	}

	public Transform NewDropdownWindow(string title = null, GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/dropdown/" + typeof(DropdownWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		if (title != null)
		{
			fromResources.GetComponent<DropdownWindow>().UITitle.text = title;
		}
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null && dialogParent.GetComponent<ChatWindow>() == null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources;
	}

	public Transform NewArmoryDropdownWindow(string title = null, string desc = null, GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/dropdown/" + typeof(ArmoryDropdownWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		if (title != null)
		{
			fromResources.GetComponent<ArmoryDropdownWindow>().topperTxt.text = title;
		}
		if (desc != null)
		{
			fromResources.GetComponent<ArmoryDropdownWindow>().messageTxt.text = desc;
		}
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null && dialogParent.GetComponent<ChatWindow>() == null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources;
	}

	public DungeonUI NewDungeonUI(Dungeon dungeon, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/dungeon/" + typeof(DungeonUI).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<DungeonUI>().LoadDetails(dungeon);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<DungeonUI>();
	}

	public void NewDungeonMerchantWindow(Dungeon dungeon, DungeonPlayer player, DungeonObject theObject, int[] classes, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/dungeon/" + typeof(DungeonMerchantWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<DungeonMerchantWindow>().LoadDetails(dungeon, player, theObject);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(classes);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewDungeonAdWindow(Dungeon dungeon, DungeonPlayer player, DungeonObject theObject, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/dungeon/" + typeof(DungeonAdWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<DungeonAdWindow>().LoadDetails(dungeon, player, theObject);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewDungeonTreasureWindow(Dungeon dungeon, DungeonPlayer player, DungeonObject theObject, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/dungeon/" + typeof(DungeonTreasureWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<DungeonTreasureWindow>().LoadDetails(dungeon, player, theObject);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewEnchantsWindow(Enchants enchants, bool changeable = false, GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/enchant/" + typeof(EnchantsWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		StoreWindowReference(fromResources.GetComponent<EnchantsWindow>(), Window.Enchants);
		enchantsWindow.LoadDetails(enchants, changeable);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewEnchantSelectWindow(Enchants enchants, bool changeable = false, int slot = -1, GameObject dialogParent = null, int layer = -1, bool isArmory = false)
	{
		Transform fromResources = GetFromResources("ui/enchant/" + typeof(EnchantSelectWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<EnchantSelectWindow>().LoadDetails(enchants, changeable, slot, isArmory);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewEnchantSlotSelectWindow(EnchantData enchantData, UnityAction onComplete, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/enchant/" + typeof(EnchantSlotSelectWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<EnchantSlotSelectWindow>().LoadDetails(enchantData, onComplete);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewEnchantOptionsWindow(int slot, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/enchant/" + typeof(EnchantOptionsWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<EnchantOptionsWindow>().LoadDetails(slot);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewEnchantWindow(EnchantData enchantData, GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/enchant/" + typeof(EnchantWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<EnchantWindow>().LoadDetails(enchantData);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewArmoryEnchantsWindow(ArmoryEnchants enchants, bool changeable = false, GameObject dialogParent = null, int layer = -1, bool selectable = true, CharacterData friendCharData = null)
	{
		Transform fromResources = GetFromResources("ui/enchant/" + typeof(EnchantsWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		StoreWindowReference(fromResources.GetComponent<EnchantsWindow>(), Window.Enchants);
		enchantsWindow.LoadDetails(enchants, changeable, isArmory: true, friendCharData);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	private void StoreWindowReference(WindowsMain currentWindow, Window windowType)
	{
		if (!Windows.ContainsKey(windowType))
		{
			Windows.Add(windowType, null);
		}
		Windows[windowType] = currentWindow;
	}

	public void NewEquipmentWindow(int slot, GameObject dialogParent, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/equipment/" + typeof(EquipmentWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<EquipmentWindow>().LoadDetails(slot);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewEquipmentUpgradeWindow(EquipmentRef equipmentRef, GameObject dialogParent, int[] classes, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/equipment/" + typeof(EquipmentUpgradeWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<EquipmentUpgradeWindow>().LoadDetails(equipmentRef);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(classes);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewArmoryEquipmentWindow(int slot, GameObject dialogParent, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/character/armory/" + typeof(ArmoryEquipmentWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ArmoryEquipmentWindow>().LoadDetails(slot);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public Transform NewEventLeaderboardWindow(int eventType, int leaderboardType, bool allowSegmented = true, GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/events/" + typeof(EventLeaderboardWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<EventLeaderboardWindow>().LoadDetails(eventType, leaderboardType, allowSegmented);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources;
	}

	public Transform NewEventRewardsWindow(List<EventRef> events, int rank, int points, int zone = -1, int eventID = -1, bool alternate = false, long communityPoints = 0L, GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/events/" + typeof(EventRewardsWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<EventRewardsWindow>().LoadDetails(events, rank, points, zone, eventID, alternate, communityPoints);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources;
	}

	public Transform NewEventTargetWindow(EventRef eventRef, List<EventTargetData> targets, GameObject dialogParent, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/events/" + typeof(EventTargetWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<EventTargetWindow>().LoadDetails(eventRef, targets);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources;
	}

	public void NewFamiliarsWindow(int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/familiar/" + typeof(FamiliarsWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewFamiliarWindow(FamiliarRef familiarRef, GameObject dialogParent, bool mine = false, CharacterData sourceCharacter = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/familiar/" + typeof(FamiliarWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<FamiliarWindow>().LoadDetails(familiarRef, mine, sourceCharacter);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewFamiliarStableWindow(GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/familiar/" + typeof(FamiliarStableWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewFamiliarStableBoardWindow(int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/familiar/" + typeof(FamiliarStableBoardWindow).Name);
		fromResources.transform.SetParent(canvas.transform);
		fromResources.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
		fromResources.GetComponent<RectTransform>().sizeDelta = new Vector2(1f, 1f);
		fromResources.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public FamiliarCaptureSuccessWindow NewFamiliarCaptureSuccessWindow(FamiliarRef familiarRef, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/familiar/" + typeof(FamiliarCaptureSuccessWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<FamiliarCaptureSuccessWindow>().LoadDetails(familiarRef);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<FamiliarCaptureSuccessWindow>();
	}

	public Transform NewFishingShopWindow(GameObject dialogParent, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/fishing/" + typeof(FishingShopWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources;
	}

	public Transform NewFishingEventWindow(GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/fishing/" + typeof(FishingEventWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources;
	}

	public Transform NewFishingWindow(GameObject dialogParent, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/fishing/" + typeof(FishingWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources;
	}

	public Transform NewFishingCaptureWindow(FishingItemRef itemRef, List<ItemData> items, int weight, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/fishing/" + typeof(FishingCaptureWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<FishingCaptureWindow>().LoadDetails(itemRef, items, weight);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources;
	}

	public void NewFriendWindow(int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/friend/" + typeof(FriendWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewFriendInviteWindow(int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/friend/" + typeof(FriendInviteWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewFriendRecommendWindow(GameObject dialogParent, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/friend/" + typeof(FriendRecommendWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<FriendRecommendWindow>().LoadDetails();
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewFriendRequestWindow(int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/friend/" + typeof(FriendRequestWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewFusionWindow(GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/fusion/" + typeof(FusionWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<FusionWindow>().LoadDetails();
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public FusionCompleteWindow NewFusionCompleteWindow(FusionRef fusionRef, Transform asset, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/fusion/" + typeof(FusionCompleteWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<FusionCompleteWindow>().LoadDetails(fusionRef, asset);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<FusionCompleteWindow>();
	}

	public void NewFusionResultWindow(FusionRef fusionRef, GameObject dialogParent, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/fusion/" + typeof(FusionResultWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<FusionResultWindow>().LoadDetails(fusionRef);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public GameBackground AddGameBackground()
	{
		Transform fromResources = GetFromResources("ui/game/" + typeof(GameBackground).Name);
		fromResources.GetComponent<GameBackground>().LoadDetails();
		return fromResources.GetComponent<GameBackground>();
	}

	public GameLogoTween AddGameLogoTween()
	{
		Transform fromResources = GetFromResources("ui/game/" + typeof(GameLogoTween).Name);
		fromResources.GetComponent<GameLogoTween>().LoadDetails();
		return fromResources.GetComponent<GameLogoTween>();
	}

	public GameLogo2022 AddGameLogo2022()
	{
		Transform fromResources = GetFromResources("ui/game/" + typeof(GameLogo2022).Name);
		fromResources.GetComponent<GameLogo2022>().LoadDetails();
		return fromResources.GetComponent<GameLogo2022>();
	}

	public GameModifierTimeWindow NewGameModifierTimeWindow(string name, long millisecondsRemaining, long millisecondsTotal, List<GameModifier> modifiers = null, UnityAction OnComplete = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/game/" + typeof(GameModifierTimeWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		GameModifierTimeWindow component = fromResources.GetComponent<GameModifierTimeWindow>();
		component.LoadDetails(name, millisecondsRemaining, millisecondsTotal, modifiers, OnComplete);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return component;
	}

	public void NewGameModifierListWindow(List<GameModifier> modifiers, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/game/" + typeof(GameModifierListWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<GameModifierListWindow>().LoadDetails(modifiers);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewGameSettingsWindow(int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/game/" + typeof(GameSettingsWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public GameNotification AddNotificationObject(int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/game/" + typeof(GameNotification).Name);
		fromResources.transform.SetParent(canvas.transform);
		_notifications = fromResources.transform.gameObject;
		return _notifications.GetComponent<GameNotification>();
	}

	public void NewGauntletEventWindow(GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/gauntlet/" + typeof(GauntletEventWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewGuildWindow(int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/guild/" + typeof(GuildWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<GuildWindow>().LoadDetails();
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewGuildlessWindow(int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/guild/" + typeof(GuildlessWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewGuildInvitesWindow(GameObject dialogParent, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/guild/" + typeof(GuildInvitesWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewGuildApplicationWindow(GameObject dialogParent, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/guild/" + typeof(GuildApplicationWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewGuildCreationWindow(GameObject dialogParent, int[] classes, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/guild/" + typeof(GuildCreateWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(classes);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewGuildOptionsWindow(GuildData guildData, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/guild/" + typeof(GuildOptionsWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<GuildOptionsWindow>().LoadDetails(guildData);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewGuildPermissionsWindow(GameObject dialogParent, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/guild/" + typeof(GuildPermissionsWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewGuildApplicantsWindow(GameObject dialogParent, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/guild/" + typeof(GuildApplicantsWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewGuildInventoryWindow(GuildData data, int tab = -1, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/guild/" + typeof(GuildInventoryWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<GuildInventoryWindow>().LoadDetails(data, tab);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewGuildPerksWindow(GuildData data, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/guild/" + typeof(GuildPerksWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<GuildPerksWindow>().LoadDetails(data);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewGuildInviteWindow(int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/guild/" + typeof(GuildInviteWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewGuildMemberWindow(GuildMemberData memberData, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/guild/" + typeof(GuildMemberWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<GuildMemberWindow>().LoadDetails(memberData);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewGvEEventWindow(int nodeID = -1, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/gve/" + typeof(GvEEventWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<GvEEventWindow>().LoadDetails(nodeID);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewGvEEventZoneWindow(GvEEventRef eventRef, int nodeID = -1, GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/gve/" + typeof(GvEEventZoneWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<GvEEventZoneWindow>().LoadDetails(eventRef, nodeID);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewGvEEventZoneNodeWindow(GvEEventRef eventRef, GvEZoneNodeRef nodeRef, GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/gve/" + typeof(GvEEventZoneNodeWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<GvEEventZoneNodeWindow>().LoadDetails(eventRef, nodeRef);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewGvGEventWindow(GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/gvg/" + typeof(GvGEventWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewGvGEventTargetWindow(GameObject dialogParent, GvGEventRef eventRef, List<EventTargetData> targets, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/gvg/" + typeof(GvGEventTargetWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		fromResources.GetComponent<GvGEventTargetWindow>().LoadDetails(eventRef, targets);
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public InstanceGuildHallInterface NewInstanceGuildHallInterface(Instance instance, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/instance/" + typeof(InstanceGuildHallInterface).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<InstanceGuildHallInterface>().LoadDetails(instance);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		UpdateLayer(fromResources, layer);
		return fromResources.GetComponent<InstanceGuildHallInterface>();
	}

	public InstanceFishingInterface NewInstanceFishingInterface(Instance instance, InstanceObject instanceObject, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/instance/fishing/" + typeof(InstanceFishingInterface).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<InstanceFishingInterface>().LoadDetails(instance, instanceObject);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<InstanceFishingInterface>();
	}

	public void NewInvasionEventWindow(GameObject dialogParent, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/invasion/" + typeof(InvasionEventWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewInvasionGameModifierListWindow(List<InvasionEventLevelRef> levels, long points, GameObject dialogParent, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/invasion/" + typeof(InvasionGameModifierListWindow).Name);
		fromResources.GetComponent<InvasionGameModifierListWindow>().LoadDetails(levels, points);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public ItemListWindow NewItemListWindow(List<ItemData> items, bool compare = true, bool added = false, string name = null, bool large = false, bool forceNonEquipment = false, bool select = false, string helpText = null, GameObject dialogParent = null, int layer = -1, string closeWord = null, bool forceItemEnabled = false)
	{
		if (items == null)
		{
			return null;
		}
		bool flag = false;
		foreach (ItemData item in items)
		{
			if (item.type == 1)
			{
				flag = true;
				break;
			}
		}
		Transform transform2;
		if (flag && !forceNonEquipment)
		{
			transform2 = SetWindow(GetFromResources("ui/item/" + typeof(ItemListWindowEquipment).Name));
			ItemListWindowEquipment component = transform2.GetComponent<ItemListWindowEquipment>();
			component.LoadDetails(items, compare, added, name, select, helpText, closeWord, forceItemEnabled);
			return component;
		}
		transform2 = ((items.Count >= 9 || large) ? SetWindow(GetFromResources("ui/item/" + typeof(ItemListWindow).Name + "Big")) : SetWindow(GetFromResources("ui/item/" + typeof(ItemListWindow).Name)));
		ItemListWindow component2 = transform2.GetComponent<ItemListWindow>();
		component2.LoadDetails(items, compare, added, name, select, helpText, closeWord, forceItemEnabled);
		return component2;
		Transform SetWindow(Transform transform)
		{
			CenterAndAddCanvasParent(transform);
			transform.GetComponent<Animator>().SetBool("onDown", value: true);
			transform.GetComponent<WindowsMain>().dialogParent = dialogParent;
			if (dialogParent != null)
			{
				dialogParent.GetComponent<WindowsMain>().DoHide();
			}
			_dialogs.Add(transform.gameObject);
			_classes.Add(null);
			UpdateLayer(transform, layer);
			AddScrollOutAndDestroyListeners(transform);
			return transform;
		}
	}

	public ItemSearchWindow NewItemSearchWindow(List<ItemData> items, bool adminWindow, string name = null, bool showQty = true, bool closeOnSelect = true, List<ItemRef> selectedItems = null, bool showLock = false, bool tooltipSuggested = false, GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/item/" + typeof(ItemSearchWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ItemSearchWindow>().LoadDetails(items, adminWindow, name, showQty, closeOnSelect, selectedItems, showLock, tooltipSuggested);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<ItemSearchWindow>();
	}

	public ItemExchangeFilterWindowOLD NewItemExchangeFilterWindowOLD(ItemData itemData = null, GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/item/" + typeof(ItemExchangeFilterWindowOLD).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<ItemExchangeFilterWindowOLD>();
	}

	public void NewItemExchangeWindow(ItemData itemData, int[] classes = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/item/" + typeof(ItemExchangeWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ItemExchangeWindow>().LoadDetails(itemData);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(classes);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewItemTradeWindow(TradeItem model, int[] classes = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/item/" + typeof(ItemTradeWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ItemTradeWindow>().LoadDetails(model);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(classes);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewItemUseWindow(ItemData itemData, int[] classes = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/item/" + typeof(ItemUseWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ItemUseWindow>().LoadDetails(itemData);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(classes);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewItemContentsWindow(ItemRef itemRef, int qty = 1, EventSalesShopItemRefModelData eventModel = null, object data = null, bool purchaseable = true, int[] classes = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/item/" + typeof(ItemContentsWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ItemContentsWindow>().LoadDetails(itemRef, qty, eventModel, data, purchaseable);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(classes);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewItemReforgeWindow(ItemRef itemRef, GameObject dialogParent, int[] classes = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/item/" + typeof(ItemReforgeWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ItemReforgeWindow>().LoadDetails(itemRef);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public Transform NewItemSelectWindow(int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/item/" + typeof(ItemSelectWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources;
	}

	public ItemInventoryFilterWindow NewItemInventoryFilterWindow(GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/item/" + typeof(ItemInventoryFilterWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ItemInventoryFilterWindow>().LoadDetails();
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<ItemInventoryFilterWindow>();
	}

	public ItemInventoryAdvancedFilterWindow NewItemInventoryAdvancedFilterWindow(int filter, AdvancedFilterSettings advancedFilter, UnityAction<AdvancedFilterSettings, bool> onClose, int typeException = -1, GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/item/" + typeof(ItemInventoryAdvancedFilterWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ItemInventoryAdvancedFilterWindow>().LoadDetails(filter, advancedFilter, onClose, typeException);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<ItemInventoryAdvancedFilterWindow>();
	}

	public ItemExchangeFilterWindow NewItemExchangeFilterWindow(GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/item/" + typeof(ItemExchangeFilterWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ItemExchangeFilterWindow>().LoadDetails();
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<ItemExchangeFilterWindow>();
	}

	public ItemExchangeAdvancedFilterWindow NewItemExchangeAdvancedFilterWindow(int baseFilter, AdvancedFilterSettings advancedFilter, UnityAction<AdvancedFilterSettings, bool> onClose, int typeException = -1, GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/item/" + typeof(ItemExchangeAdvancedFilterWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ItemExchangeAdvancedFilterWindow>().LoadDetails(baseFilter, advancedFilter, onClose, typeException);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<ItemExchangeAdvancedFilterWindow>();
	}

	public ItemTradeFilterWindow NewItemTradeFilterWindow(GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/item/" + typeof(ItemTradeFilterWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ItemTradeFilterWindow>().LoadDetails();
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<ItemTradeFilterWindow>();
	}

	public ItemTradeAdvancedFilterWindow NewItemTradeAdvancedFilterWindow(int baseFilter, AdvancedFilterSettings advancedFilter, UnityAction<AdvancedFilterSettings, bool> onClose, int typeException = -1, GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/item/" + typeof(ItemTradeAdvancedFilterWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ItemTradeAdvancedFilterWindow>().LoadDetails(baseFilter, advancedFilter, onClose, typeException);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<ItemTradeAdvancedFilterWindow>();
	}

	public ItemAugmentFilterWindow NewItemAugmentFilterWindow(GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/item/" + typeof(ItemAugmentFilterWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ItemAugmentFilterWindow>().LoadDetails();
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<ItemAugmentFilterWindow>();
	}

	public ItemAugmentAdvancedFilterWindow NewItemAugmentAdvancedFilterWindow(int baseFilter, AdvancedFilterSettings advancedFilter, UnityAction<AdvancedFilterSettings, bool> onClose, int typeException = -1, GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/item/" + typeof(ItemAugmentAdvancedFilterWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ItemAugmentAdvancedFilterWindow>().LoadDetails(baseFilter, advancedFilter, onClose, typeException);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<ItemAugmentAdvancedFilterWindow>();
	}

	public ItemUpgradeFilterWindow NewItemUpgradeFilterWindow(GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/item/" + typeof(ItemUpgradeFilterWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ItemUpgradeFilterWindow>().LoadDetails();
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<ItemUpgradeFilterWindow>();
	}

	public ItemUpgradeAdvancedFilterWindow NewItemUpgradeAdvancedFilterWindow(int baseFilter, AdvancedFilterSettings advancedFilter, UnityAction<AdvancedFilterSettings, bool> onClose, int typeException = -1, GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/item/" + typeof(ItemUpgradeAdvancedFilterWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ItemUpgradeAdvancedFilterWindow>().LoadDetails(baseFilter, advancedFilter, onClose, typeException);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<ItemUpgradeAdvancedFilterWindow>();
	}

	public ItemReforgeFilterWindow NewItemReforgeFilterWindow(GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/item/" + typeof(ItemReforgeFilterWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ItemReforgeFilterWindow>().LoadDetails();
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<ItemReforgeFilterWindow>();
	}

	public ItemReforgeAdvancedFilterWindow NewItemReforgeAdvancedFilterWindow(int baseFilter, AdvancedFilterSettings advancedFilter, UnityAction<AdvancedFilterSettings, bool> onClose, int typeException = -1, GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/item/" + typeof(ItemReforgeAdvancedFilterWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ItemReforgeAdvancedFilterWindow>().LoadDetails(baseFilter, advancedFilter, onClose, typeException);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<ItemReforgeAdvancedFilterWindow>();
	}

	public Transform NewItemToolTipContainer(ItemIcon itemIcon, CharacterData characterData = null, ArmoryEquipment armoryEquipment = null, int layer = -1)
	{
		Transform fromResources;
		if (GameData.instance.windowTooltipContainer == null)
		{
			fromResources = GetFromResources("ui/item/" + typeof(ItemTooltipContainer).Name);
			Main.CONTAINER.AddToLayer(fromResources.gameObject, 11, front: true, center: false, resize: false);
			fromResources.GetComponent<RectTransform>().sizeDelta = new Vector2(1f, 1f);
			fromResources.GetComponent<RectTransform>().localScale = new Vector3(3f, 3f, 3f);
			fromResources.GetComponent<ItemTooltipContainer>().SetItemData(itemIcon, characterData, armoryEquipment);
			GameData.instance.windowTooltipContainer = fromResources;
			return fromResources;
		}
		fromResources = GameData.instance.windowTooltipContainer;
		fromResources.gameObject.SetActive(value: true);
		fromResources.GetComponent<RectTransform>().localPosition = new Vector2(800000f, 800000f);
		fromResources.GetComponent<ItemTooltipContainer>().SetItemData(itemIcon, characterData, armoryEquipment);
		return fromResources;
	}

	public ItemTooltipContainer NewItemToolTipContainerComparisson(BaseModelData itemData, GameObject parent, int layer = -1)
	{
		Transform fromResources;
		if (GameData.instance.windowTooltipContainerCompare == null)
		{
			fromResources = GetFromResources("ui/item/" + typeof(ItemTooltipContainer).Name);
			Main.CONTAINER.AddToLayer(fromResources.gameObject, 11, front: true, center: false, resize: false);
			fromResources.GetComponent<RectTransform>().sizeDelta = new Vector2(1f, 1f);
			fromResources.GetComponent<RectTransform>().localScale = new Vector3(3f, 3f, 3f);
			fromResources.GetComponent<ItemTooltipContainer>().SetItemDataForComparisson(itemData);
			GameData.instance.windowTooltipContainerCompare = fromResources;
			return fromResources.GetComponent<ItemTooltipContainer>();
		}
		fromResources = GameData.instance.windowTooltipContainerCompare;
		fromResources.gameObject.SetActive(value: true);
		fromResources.SetAsLastSibling();
		fromResources.GetComponent<RectTransform>().localPosition = new Vector2(800000f, 800000f);
		fromResources.GetComponent<ItemTooltipContainer>().SetItemDataForComparisson(itemData);
		return fromResources.GetComponent<ItemTooltipContainer>();
	}

	public void NewItemUseSuccessWindow(ConsumableRef itemRef, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/item/" + typeof(ItemUseSuccessWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ItemUseSuccessWindow>().LoadDetails(itemRef);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public KongregateLogoTween AddKongregateLogo()
	{
		Transform fromResources = GetFromResources("ui/kongregate/KongregateLogo");
		fromResources.SetParent(GameData.instance.windowGenerator.canvas.transform, worldPositionStays: false);
		fromResources.transform.localPosition = new Vector3(0f, -50f, 0f);
		fromResources.GetComponentInChildren<KongregateLogoTween>().LoadDetails(1f, center: false);
		return fromResources.GetComponentInChildren<KongregateLogoTween>();
	}

	public void NewLeaderboardWindow(int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/leaderboard/" + typeof(LeaderboardWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public MenuInterface AddMenuInterface()
	{
		Transform fromResources = GetFromResources("ui/menu/" + typeof(MenuInterface).Name);
		fromResources.GetComponent<MenuInterface>().LoadDetails();
		return fromResources.GetComponent<MenuInterface>();
	}

	public void NewMountWindow(MountData mountData, int tier, GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/mount/" + typeof(MountWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<MountWindow>().LoadDetails(mountData, tier);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewMountUpgradeWindow(MountData mountData, GameObject dialogParent, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/mount/" + typeof(MountUpgradeWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<MountUpgradeWindow>().LoadDetails(mountData);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewMountSelectWindow(Mounts mounts, bool changeable = false, bool equippable = false, GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/mount/" + typeof(MountSelectWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<MountSelectWindow>().LoadDetails(mounts, changeable, equippable);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewArmoryMountWindow(MountData mountData, int tier, GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/mount/" + typeof(ArmoryMountWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ArmoryMountWindow>().LoadDetails(mountData, tier);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewArmoryMountSelectWindow(Mounts mounts, bool changeable = false, bool equippable = false, GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/mount/" + typeof(MountSelectWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<MountSelectWindow>().LoadDetails(mounts, changeable, equippable, isArmory: true);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public NewsWindow NewNewsWindow(GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/news/" + typeof(NewsWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<NewsWindow>();
	}

	public void NewPaymentNBPWindow(int platform, PaymentRef paymentRef, List<PaymentRef> paymentsNBPZ = null, int itemIndex = 0, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/payment/nbp/" + typeof(PaymentNBPWindowDefault).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<PaymentNBPWindowDefault>().LoadPlatformDetails(paymentRef, paymentsNBPZ, itemIndex);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewPaymentBoostersWindow(List<BoosterRef> boosters, int itemIndex = 0, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/payment/nbp/" + typeof(PaymentNBPWindowDefault).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<PaymentNBPWindowDefault>().LoadPlatformDetails(boosters, itemIndex);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public bool ShowCustomPayment(PaymentRef paymentRef, int layer = -1)
	{
		if (paymentRef == null && !AdGor.devMode)
		{
			return false;
		}
		_ = AppInfo.platform;
		Transform fromResources = GetFromResources("ui/payment/custom/" + typeof(PaymentCustomWindowDefault).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<PaymentCustomWindowDefault>().LoadPlatformDetails(paymentRef);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return true;
	}

	public Transform NewPlayerVotingWindow(int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/playervoting/" + typeof(PlayerVotingWindow));
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources;
	}

	public Transform NewPvPEventHistoryWindow(GameObject dialogParent, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/pvp/" + typeof(PvPEventHistoryWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources;
	}

	public Transform NewPvPEventWindow(int[] classes = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/pvp/" + typeof(PvPEventWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<PvPEventWindow>().LoadDetails();
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(classes);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources;
	}

	public void NewRaidWindow(GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/raid/" + typeof(RaidWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewRaidDifficultyWindow(int raidID, GameObject dialogParent, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/raid/" + typeof(RaidDifficultyWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<RaidDifficultyWindow>().raidID = raidID;
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewRiftEventWindow(GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/rift/" + typeof(RiftEventWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewRunesWindow(Runes runes, bool changeable = false, GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/rune/" + typeof(RunesWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<RunesWindow>().LoadDetails(runes, changeable);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewRuneSelectWindow(GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/rune/" + typeof(RuneSelectWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<RuneSelectWindow>().LoadDetails();
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewRuneOptionsWindow(int slot, RuneTile tile, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/rune/" + typeof(RuneOptionsWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<RuneOptionsWindow>().LoadDetails(slot, tile);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void ArmoryNewRunesWindow(ArmoryRunes runes, bool changeable = false, GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/rune/" + typeof(ArmoryRunesWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ArmoryRunesWindow>().LoadDetails(runes, changeable);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public Transform NewServiceListWindow(string name, List<ServiceRef> services, int[] classes = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/service/" + typeof(ServiceListWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ServiceListWindow>().LoadDetails(name, services);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(classes);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources;
	}

	public Transform NewServiceWindow(int tab = 0, ServiceRef highlightedRef = null, int[] classes = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/service/" + typeof(ServiceWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ServiceWindow>().LoadDetails(tab, highlightedRef);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(classes);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources;
	}

	public void NewServiceOfferwallSelectWindow(List<PaymentData> offerwallsData)
	{
		Transform fromResources = GetFromResources("ui/service/" + typeof(ServiceOfferwallSelectWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ServiceOfferwallSelectWindow>().LoadDetails(offerwallsData);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, -1);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewShopWindow(int[] classes, int defaultTab, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/shop/window/" + typeof(ShopWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ShopWindow>().LoadDetails(defaultTab);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(classes);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public TeamWindow NewTeamWindow(int type, TeamRules teamRules, UnityAction<TeamData> onTeamSelected, GameObject dialogParent, int layer = -1, bool showArmoryButton = true)
	{
		Transform fromResources = GetFromResources("ui/team/" + typeof(TeamWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<TeamWindow>().LoadDetails(type, teamRules, onTeamSelected, showArmoryButton);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<TeamWindow>();
	}

	public void NewTeamSelectWindow(List<TeammateData> allowed, List<TeammateData> used, TeamRules rules, UnityAction<TeammateData> onItemSelected, GameObject dialogParent, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/team/" + typeof(TeamSelectWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<TeamSelectWindow>().LoadDetails(allowed, used, rules, onItemSelected);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public TransitionScreen NewTransitionScreen(string sceneName, string currentSceneName, UnityAction completeAction = null, UnityAction toggleAction = null, bool unloadFirst = true, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/utility/" + typeof(TransitionScreen).Name);
		CenterAndAddCanvasParent(fromResources);
		TransitionScreen component = fromResources.GetComponent<TransitionScreen>();
		component.LoadDetails(sceneName, currentSceneName, completeAction, toggleAction, unloadFirst);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return component;
	}

	public void NewResponseWindow(string name, string desc, string yesText = null, string noText = null, bool flipped = false, UnityAction onConfirmCallback = null, UnityAction onCancelCallback = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/utility/" + typeof(ResponseWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ResponseWindow>().LoadDetails(name, desc, yesText, noText, flipped, onConfirmCallback, onCancelCallback);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewTextWindow(string title, string desc, GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/utility/" + typeof(TextWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<TextWindow>().LoadMessage(title, desc);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	[ContextMenu("NewEulaImportantWindowOpen()")]
	public void NewEulaImportantWindowOpen()
	{
		NewEulaImportantWindow();
	}

	public void NewEulaImportantWindow(UnityAction callback = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/eula/" + typeof(EulaImportantWindow).Name);
		fromResources.GetComponent<EulaImportantWindow>().LoadDetails(callback);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public Transform NewVictoryWindow(int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/victory/" + typeof(VictoryWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources;
	}

	public DungeonVictoryWindow NewDungeonVictoryWindow(int layer = -1)
	{
		if (HasDialogByClass(typeof(DungeonVictoryWindow)))
		{
			return null;
		}
		Transform fromResources = GetFromResources("ui/victory/" + typeof(DungeonVictoryWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<DungeonVictoryWindow>();
	}

	public void NewVipGorWindow(ShopSaleRef highlightedRef = null, GameObject dialogParent = null, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/vipgor/" + typeof(VipGorWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<VipGorWindow>().LoadDetails(highlightedRef);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewVipGorSuccessWindow(ConsumableRef itemRef, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/vipgor/" + typeof(VipGorSuccessWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<VipGorSuccessWindow>().LoadDetails(itemRef);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public ZoneWindow NewQuestWindow(int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/zone/" + typeof(ZoneWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ZoneWindow>().LoadDetails();
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<ZoneWindow>();
	}

	public GameObject UpdateWindowZone(Transform theParent, int zoneID)
	{
		Transform transformAsset = GameData.instance.main.assetLoader.GetTransformAsset(AssetURL.GetZonePath(zoneID));
		if (transformAsset == null)
		{
			return null;
		}
		transformAsset.transform.SetParent(theParent, worldPositionStays: true);
		transformAsset.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
		transformAsset.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
		return transformAsset.gameObject;
	}

	public void NewZoneNodeWindow(ZoneNodeRef nodeRef, GameObject dialogParent, int[] classes, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/zone/" + typeof(ZoneNodeWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ZoneNodeWindow>().LoadDetails(nodeRef);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			WindowsMain component = dialogParent.GetComponent<WindowsMain>();
			if (component != null)
			{
				component.DoHide();
			}
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(classes);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewZoneNodeDifficultyWindow(ZoneNodeRef nodeRef, GameObject dialogParent, int[] currencies, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/zone/" + typeof(ZoneNodeDifficultyWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ZoneNodeDifficultyWindow>().LoadDetails(nodeRef);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			WindowsMain component = dialogParent.GetComponent<WindowsMain>();
			if (component != null)
			{
				component.DoHide();
			}
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(currencies);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public void NewZoneNodeSingleWindow(ZoneNodeRef nodeRef, GameObject dialogParent, int[] currencies, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/zone/" + typeof(ZoneNodeSingleWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ZoneNodeSingleWindow>().LoadDetails(nodeRef);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		fromResources.GetComponent<WindowsMain>().dialogParent = dialogParent;
		if (dialogParent != null)
		{
			WindowsMain component = dialogParent.GetComponent<WindowsMain>();
			if (component != null)
			{
				component.DoHide();
			}
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(currencies);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
	}

	public ZoneCompletedWindow NewZoneCompletedWindow(ZoneRef zoneRef, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/zone/" + typeof(ZoneCompletedWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<ZoneCompletedWindow>().LoadDetails(zoneRef);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources.GetComponent<ZoneCompletedWindow>();
	}

	public Transform NewEventSalesShopWindow(string origin, GameObject dialogParent, int layer = -1)
	{
		Transform fromResources = GetFromResources("ui/eventsales/" + typeof(EventSalesShopWindow).Name);
		CenterAndAddCanvasParent(fromResources);
		fromResources.GetComponent<Animator>().SetBool("onDown", value: true);
		EventSalesShopWindow component = fromResources.GetComponent<EventSalesShopWindow>();
		component.LoadDetails(origin);
		component.dialogParent = dialogParent;
		if (dialogParent != null)
		{
			dialogParent.GetComponent<WindowsMain>().DoHide();
		}
		_dialogs.Add(fromResources.gameObject);
		_classes.Add(null);
		UpdateLayer(fromResources, layer);
		AddScrollOutAndDestroyListeners(fromResources);
		return fromResources;
	}
}
