using System.Collections.Generic;
using com.ultrabit.bitheroes.charactermov;
using com.ultrabit.bitheroes.gamecontroller;
using com.ultrabit.bitheroes.login;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.zone;
using com.ultrabit.bitheroes.transaction;
using com.ultrabit.bitheroes.ui;
using com.ultrabit.bitheroes.ui.audio;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.grid;
using com.ultrabit.bitheroes.ui.tutorial;
using com.ultrabit.bitheroes.ui.utility;
using UnityEngine;
using UnityEngine.EventSystems;

namespace com.ultrabit.bitheroes.core;

public class GameData
{
	private static GameData _instance;

	public LayerAdjustment layerAdjustment;

	public WindowGenerator windowGenerator;

	public CharacterMov characterMov;

	public LogInManager logInManager;

	public TutorialManager tutorialManager;

	public AudioManager audioManager;

	public GameObject battleTextPool;

	public EventSystem eventSystem;

	public DotsComponent dotsComponent;

	public TrophyComponent trophyComponent;

	public StarComponent starComponent;

	public AugmentsComponent augmentsComponent;

	public RankComponent rankComponent;

	public TransactionIAPMobile transactionIAPMobile;

	public ZoneNodeRef lastNode;

	public bool zoneWindowTween;

	public GridMap grid;

	public int zoneID;

	public bool DISCONNECTED;

	public bool SERVER_SELECTED;

	public string SERVER_INSTANCE_ID;

	public int DUEL_CHAR_ID;

	public int BRAWL_INDEX;

	public string DISCONNECT_ERROR_MESSAGE;

	public bool RELOG;

	public bool RELOAD_SERVER_XML_FILES;

	public bool DISPLAY_LOGOS;

	public bool enableDLC;

	public Main main;

	public bool active;

	public PersistentPlayerData SAVE_STATE;

	public Project PROJECT;

	public CharacterDisplay sceneCharacterDisplay;

	public Transform windowTooltipContainer;

	public Transform windowTooltipContainerCompare;

	public List<int> friendCharIDsToUpdate;

	public static GameData instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new GameData();
			}
			return _instance;
		}
	}

	public GameData()
	{
		SAVE_STATE = new PersistentPlayerData();
		RELOAD_SERVER_XML_FILES = true;
		DISPLAY_LOGOS = true;
		RELOG = true;
		DISCONNECTED = false;
		SERVER_SELECTED = false;
		SERVER_INSTANCE_ID = null;
		DUEL_CHAR_ID = 0;
		BRAWL_INDEX = 0;
		enableDLC = true;
		friendCharIDsToUpdate = new List<int>();
	}

	public void Clear()
	{
		PROJECT?.clear();
	}

	public void Init()
	{
		Clear();
		PROJECT = new Project();
	}

	public List<KongregateAnalyticsSchema.SettingsStat> GetLoadOut()
	{
		return new List<KongregateAnalyticsSchema.SettingsStat>
		{
			new KongregateAnalyticsSchema.SettingsStat
			{
				name = "autoFemGold",
				value = SAVE_STATE.autopersuadeFamiliarsGold
			},
			new KongregateAnalyticsSchema.SettingsStat
			{
				name = "autoFemGem",
				value = SAVE_STATE.autopersuadeFamiliarsGems
			}
		};
	}
}
