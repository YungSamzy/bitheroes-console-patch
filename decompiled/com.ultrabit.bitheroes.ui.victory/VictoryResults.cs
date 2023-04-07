using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.battle;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.battle;
using com.ultrabit.bitheroes.ui.item;
using com.ultrabit.bitheroes.ui.lists.battleresultslist;
using com.ultrabit.bitheroes.ui.tutorial;
using com.ultrabit.bitheroes.ui.utility;
using DG.Tweening;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.victory;

public class VictoryResults : MonoBehaviour
{
	public const float STATSBTN_ANIM_DURATION = 0.3f;

	public TextMeshProUGUI goldTxt;

	public TextMeshProUGUI goldNameTxt;

	public TextMeshProUGUI expTxt;

	public TextMeshProUGUI expNameTxt;

	public RegularBarFill expBar;

	public Image expBg;

	public TextMeshProUGUI levelTxt;

	public TextMeshProUGUI itemsTxt;

	public TextMeshProUGUI lootNameTxt;

	public Button statsBtn;

	private Button closeBtn;

	public Button redCloseBtn;

	public Button greenCloseBtn;

	public Button regroupBtn;

	public Button rerunBtn;

	public Button battleStatsBtn;

	public GameObject battleResultsListView;

	public BattleResultsList battleResultsList;

	public RectTransform placehoderLevelUp;

	private int _type;

	private long _exp;

	private int _gold;

	private List<ItemData> _items;

	private float _expBarScale;

	private long _currentExp;

	private long _pendingExp;

	private long _tickExp;

	private int _currentGold;

	private int _pendingGold;

	private int _tickGold;

	private int _currentLevel;

	private bool _levelUp;

	private List<ItemIcon> _tiles;

	private bool _closeable;

	private bool update;

	private bool firstTime = true;

	private GameObject _parent;

	private VictoryWindow _victoryWindow;

	private bool statsBtnAnimation;

	private bool _refreshPending;

	private bool _showingTutorial;

	private bool _animate;

	private bool _isTutorializable;

	[HideInInspector]
	public List<Button> extraButtons = new List<Button>();

	private List<BattleStat> _battleStats = new List<BattleStat>();

	private GameObject _copyForTutorial;

	private Coroutine waitForStatsButton;

	public bool closeable => _closeable;

	public void LoadDetails(int type, VictoryWindow victoryWindow, long exp = 0L, int gold = 0, List<ItemData> items = null, bool animate = true, List<BattleStat> battleStats = null, GameObject parent = null, string customLootText = null, bool isCloseRed = true, string customCloseText = null, bool isRerunnable = false)
	{
		_type = type;
		_exp = exp;
		_gold = gold;
		_items = items;
		_parent = parent;
		_victoryWindow = victoryWindow;
		_isTutorializable = customLootText == null;
		if (battleStats == null)
		{
			battleStats = new List<BattleStat>();
		}
		_battleStats = battleStats;
		_currentExp = GameData.instance.PROJECT.character.exp - _exp;
		_pendingExp = _exp;
		_tickExp = (long)Mathf.Round((float)_exp / (1f / Time.smoothDeltaTime * 1.25f));
		_currentGold = GameData.instance.PROJECT.character.gold - _gold;
		_pendingGold = _gold;
		_tickGold = (int)Mathf.Round((float)_gold / (1f / Time.smoothDeltaTime * 1.25f));
		_currentLevel = Character.getExpLevel(_currentExp);
		_levelUp = _currentLevel != GameData.instance.PROJECT.character.level;
		if (!animate || AppInfo.TESTING)
		{
			_tickExp = _pendingExp;
			_tickGold = _pendingGold;
		}
		if (_tickExp < 1)
		{
			_tickExp = 1L;
		}
		if (_tickGold < 1)
		{
			_tickGold = 1;
		}
		if (_battleStats.Count > 0)
		{
			battleStatsBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_stats");
			battleStatsBtn.gameObject.SetActive(value: true);
		}
		else
		{
			battleStatsBtn.gameObject.SetActive(value: false);
		}
		statsBtn.GetComponent<GlowNoOver>().ForceStart();
		statsBtn.GetComponent<GlowNoOver>().ON_ENTER.AddListener(OnStatsBtnEnter);
		statsBtn.gameObject.SetActive(value: false);
		itemsTxt.text = Language.GetString("battle_no_items_found");
		lootNameTxt.text = ((customLootText != null) ? customLootText : Language.GetString("ui_loot_found"));
		statsBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_plus");
		rerunBtn.GetComponentInChildren<TextMeshProUGUI>().SetText(Language.GetString("ui_rerun"));
		rerunBtn.gameObject.SetActive(isRerunnable);
		if (isRerunnable)
		{
			extraButtons.Add(rerunBtn);
		}
		redCloseBtn.gameObject.SetActive(isCloseRed);
		greenCloseBtn.gameObject.SetActive(!isCloseRed);
		closeBtn = (isCloseRed ? redCloseBtn : greenCloseBtn);
		closeBtn.GetComponentInChildren<TextMeshProUGUI>().text = ((customCloseText != null) ? customCloseText : Language.GetString("ui_close"));
		GameData.instance.PROJECT.character.equipment.OnChange.AddListener(OnEquipmentChange);
		GameData.instance.PROJECT.character.AddListener("STATS_CHANGE", OnStatsChange);
		GameData.instance.PROJECT.character.AddListener("POINTS_CHANGE", OnStatsChange);
		if (_type == 9)
		{
			regroupBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_regroup");
		}
		else
		{
			regroupBtn.gameObject.SetActive(value: false);
		}
		battleResultsList.InitList();
		CreateItems();
		DoUpdate();
		SetClose(enabled: false);
	}

	private void OnStatsBtnEnter()
	{
		statsBtn.GetComponent<GlowNoOver>().ON_ENTER.RemoveListener(OnStatsBtnEnter);
		Object.Destroy(statsBtn.GetComponent<GlowNoOver>());
	}

	public void OnStatsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewCharacterStatWindow(base.gameObject, "OnStatWindowClosed");
	}

	public void OnCloseBtn()
	{
		_parent.BroadcastMessage("OnClose");
	}

	public void OnRerunBtn()
	{
		_victoryWindow.ON_RERUN?.Invoke();
		_victoryWindow.OnCloseReRun();
	}

	public void OnRegropBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.PROJECT.SetBrawlRejoin(v: true);
		_parent.BroadcastMessage("OnClose");
	}

	public void OnBattleStatsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.PROJECT.character.autoPilot = false;
		GameData.instance.windowGenerator.NewBattleStatsWindow(_battleStats);
	}

	private void OnStatsChange()
	{
		CreateItems();
		if (statsBtn.gameObject.activeSelf)
		{
			statsBtn.gameObject.SetActive(GameData.instance.PROJECT.character.points > 0);
		}
	}

	private void OnEquipmentChange()
	{
		CreateItems();
	}

	public void CreateItems()
	{
		battleResultsList.ClearList();
		bool flag = _items != null && _items.Count > 0;
		battleResultsListView.SetActive(flag);
		itemsTxt.gameObject.SetActive(!flag);
		if (!flag)
		{
			return;
		}
		foreach (ItemData item in ItemData.SortLoot(_items, new string[4] { "type", "rarity", "total", "id" }))
		{
			if (item != null)
			{
				battleResultsList.Data.InsertOneAtEnd(new BattleResultsItem
				{
					itemData = item,
					battleType = _type
				});
			}
		}
	}

	public void Animate(bool animate = true)
	{
		_animate = animate;
		float num = 1f;
		SetClose(enabled: false);
		if (AppInfo.TESTING)
		{
			num /= 3f;
		}
		if (!_animate)
		{
			num = 0f;
			_tickExp = _exp;
			_tickGold = _gold;
		}
		StartCoroutine(DoAnimate(num));
	}

	private IEnumerator DoAnimate(float delay)
	{
		yield return new WaitForSeconds(delay);
		SetClose(enabled: true);
		update = true;
		if (_animate)
		{
			CheckTutorial();
		}
	}

	public void CheckTutorial()
	{
		if (GameData.instance.tutorialManager == null || GameData.instance.tutorialManager.hasPopup || GameData.instance.tutorialManager.canvas == null)
		{
			return;
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(14) && _levelUp)
		{
			if (!statsBtn.gameObject.activeSelf)
			{
				SetClose(enabled: false);
				return;
			}
			SetClose(enabled: true);
			if (waitForStatsButton == null)
			{
				waitForStatsButton = StartCoroutine(WaitStatsBtn());
			}
			return;
		}
		if (!GameData.instance.PROJECT.character.tutorial.GetState(12))
		{
			List<ItemIcon> visibleTiles = GetVisibleTiles();
			ItemIcon equipmentTile = ItemIcon.GetEquipmentTile(upgrade: true, visibleTiles);
			ItemIcon.GetEquipmentTileIndex(upgrade: true, visibleTiles);
			if (equipmentTile != null)
			{
				GameData.instance.PROJECT.character.tutorial.SetState(12);
				GameData.instance.tutorialManager.ShowTutorialForButton(equipmentTile.gameObject, new TutorialPopUpSettings(Tutorial.GetText(12), 4, equipmentTile.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true, delegate
				{
					DestroyCopy();
				});
				GameData.instance.PROJECT.CheckTutorialChanges();
				return;
			}
		}
		if (GameData.instance.PROJECT.character.tutorial.GetState(45))
		{
			return;
		}
		ItemIcon abilityChangeTile = ItemIcon.GetAbilityChangeTile(GetVisibleTiles());
		if (abilityChangeTile != null)
		{
			GameData.instance.PROJECT.character.tutorial.SetState(45);
			GameData.instance.tutorialManager.ShowTutorialForButton(abilityChangeTile.gameObject, new TutorialPopUpSettings(Tutorial.GetText(45), 4, abilityChangeTile.gameObject), stageTrigger: true, null, funcSameAsTargetFunc: false, null, shadow: false, tween: false, delegate
			{
				DestroyCopy();
			});
			GameData.instance.PROJECT.CheckTutorialChanges();
		}
	}

	private void DestroyCopy()
	{
	}

	private List<ItemIcon> GetVisibleTiles()
	{
		List<ItemIcon> list = new List<ItemIcon>();
		if (!_isTutorializable)
		{
			return list;
		}
		for (int i = 0; i < battleResultsList.Data.Count; i++)
		{
			MyGridItemViewsHolder cellViewsHolderIfVisible = battleResultsList.GetCellViewsHolderIfVisible(i);
			if (cellViewsHolderIfVisible != null && cellViewsHolderIfVisible.root.GetComponent<ItemIcon>() != null)
			{
				list.Add(cellViewsHolderIfVisible.root.GetComponent<ItemIcon>());
			}
		}
		return list;
	}

	private void Update()
	{
		if (!update)
		{
			return;
		}
		if (_pendingExp > 0)
		{
			long pendingExp = _pendingExp;
			_pendingExp -= _tickExp;
			if (_pendingExp < 0)
			{
				_pendingExp = 0L;
			}
			long num = pendingExp - _pendingExp;
			_currentExp += num;
			long levelExp = Character.getLevelExp(_currentLevel + 1);
			if (_currentExp >= levelExp)
			{
				_currentLevel = Character.getExpLevel(_currentExp);
				if (!statsBtn.gameObject.activeSelf)
				{
					statsBtn.gameObject.SetActive(value: true);
					Transform obj = Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<Transform>("ui/battle/BattleTextUI"));
					obj.SetParent(placehoderLevelUp, worldPositionStays: false);
					TextMeshProUGUI component = obj.GetComponent<TextMeshProUGUI>();
					component.fontSizeMax = 8f;
					component.characterSpacing = -10f;
					component.fontStyle = FontStyles.Bold;
					obj.GetComponent<TextOnStage>().Set(Language.GetString("ui_level_up"), BattleText.COLOR_YELLOW);
					StartCoroutine(WaitToGlow());
					GameData.instance.audioManager.PlaySoundLink("levelup");
					RefillEnergy();
					KongregateAnalytics.updateCommonFields();
					GameData.instance.PROJECT.character.updateAchievements();
					ScaleStatsBtn();
					if (_animate)
					{
						Invoke("CheckTutorial", 0.3f);
					}
				}
			}
		}
		if (_pendingGold > 0)
		{
			int pendingGold = _pendingGold;
			_pendingGold -= _tickGold;
			if (_pendingGold < 0)
			{
				_pendingGold = 0;
			}
			int num2 = pendingGold - _pendingGold;
			_currentGold += num2;
		}
		if (_pendingExp <= 0 && _pendingGold <= 0)
		{
			if (_parent != null)
			{
				if (!_animate)
				{
					StartCoroutine(WaitWhenNoAnimation());
				}
				else
				{
					_parent.BroadcastMessage("OnResultsComplete");
				}
			}
			update = false;
		}
		DoUpdate();
	}

	private void RefillEnergy()
	{
		CharacterDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(68), OnUpdateEnergy);
		CharacterDALC.instance.doUpdateEnergy();
		GameData.instance.main.ShowLoading();
	}

	private void OnUpdateEnergy(BaseEvent e)
	{
		GameData.instance.main.HideLoading();
		DALCEvent obj = e as DALCEvent;
		CharacterDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(68), OnUpdateEnergy);
		SFSObject sfsob = obj.sfsob;
		if (sfsob.ContainsKey("err0"))
		{
			D.Log(string.Format("OnUpdateEnergy {0}", sfsob.GetInt("err0")));
			return;
		}
		int num = sfsob.GetInt("char127") - GameData.instance.PROJECT.character.energy;
		if (num > 0)
		{
			GameData.instance.PROJECT.character.addItem(new ItemData(CurrencyBook.Lookup(4), num));
		}
	}

	private IEnumerator WaitWhenNoAnimation()
	{
		yield return new WaitForSeconds(1f);
		_parent.BroadcastMessage("OnResultsComplete");
	}

	private IEnumerator WaitToGlow()
	{
		yield return new WaitForSeconds(0.5f);
		if (statsBtn != null && statsBtn.gameObject != null && statsBtn.GetComponent<GlowNoOver>() != null)
		{
			statsBtn.GetComponent<GlowNoOver>().StartGlow();
		}
	}

	private void DoUpdate()
	{
		string text = ((_pendingGold > 0) ? ("+" + Util.colorString(Util.NumberFormat(_pendingGold), "#FFFF77") + " ") : "");
		goldNameTxt.text = text;
		string text2 = ((_pendingExp > 0) ? ("+" + Util.colorString(Util.NumberFormat(_pendingExp), "#FF77FF") + " ") : "");
		expNameTxt.text = text2 + CurrencyRef.GetCurrencyName(3);
		goldTxt.text = Util.NumberFormat(_currentGold);
		levelTxt.text = Util.NumberFormat(_currentLevel);
		long levelExp = Character.getLevelExp(_currentLevel);
		long num = _currentExp - levelExp;
		long num2 = Character.getLevelExp(_currentLevel + 1) - levelExp;
		expTxt.text = Util.NumberFormat(num) + " / " + Util.NumberFormat(num2);
		expBar.UpdateBar(num, num2);
	}

	private void SetClose(bool enabled)
	{
		_closeable = enabled;
		Util.SetButton(regroupBtn, enabled);
		Util.SetButton(closeBtn, enabled);
		Util.SetButton(battleStatsBtn, enabled);
		foreach (Button extraButton in extraButtons)
		{
			Util.SetButton(extraButton, enabled);
		}
	}

	public void OnStatWindowClosed()
	{
		CheckTutorial();
	}

	private void OnDestroy()
	{
		GameData.instance.PROJECT.character.equipment.OnChange.RemoveListener(OnEquipmentChange);
		GameData.instance.PROJECT.character.RemoveListener("STATS_CHANGE", OnStatsChange);
		GameData.instance.PROJECT.character.RemoveListener("POINTS_CHANGE", OnStatsChange);
		if ((bool)statsBtn.GetComponent<GlowNoOver>())
		{
			statsBtn.GetComponent<GlowNoOver>().ON_ENTER.RemoveListener(OnStatsBtnEnter);
		}
	}

	public void DoEnable()
	{
		if (statsBtn != null)
		{
			statsBtn.interactable = true;
			if (closeBtn != null)
			{
				closeBtn.interactable = true;
			}
			else
			{
				redCloseBtn.interactable = true;
				greenCloseBtn.interactable = true;
			}
			regroupBtn.interactable = true;
			battleStatsBtn.interactable = true;
		}
	}

	public void DoDisable()
	{
		if (statsBtn != null)
		{
			statsBtn.interactable = false;
			if (closeBtn != null)
			{
				closeBtn.interactable = false;
			}
			else
			{
				redCloseBtn.interactable = false;
				greenCloseBtn.interactable = false;
			}
			regroupBtn.interactable = false;
			battleStatsBtn.interactable = false;
		}
	}

	private IEnumerator WaitStatsBtn()
	{
		yield return new WaitForSeconds(0.3f);
		if (_victoryWindow.scrollingIn || _victoryWindow.scrollingOut)
		{
			StopCoroutine(waitForStatsButton);
			yield return null;
		}
		GameData.instance.PROJECT.character.tutorial.SetState(14);
		GameData.instance.tutorialManager.ShowTutorialForButton(statsBtn.gameObject, new TutorialPopUpSettings(Tutorial.GetText(14), 4, statsBtn.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true, delegate
		{
			FixBtn();
		});
		GameData.instance.PROJECT.CheckTutorialChanges();
	}

	private void FixBtn()
	{
		statsBtn.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	private void ScaleStatsBtn()
	{
		if (!statsBtnAnimation)
		{
			statsBtnAnimation = true;
			Vector3 localScale = statsBtn.transform.localScale;
			Vector3 middleScale = new Vector3(0.9f, 0.9f, 1f);
			Vector3 localScale2 = new Vector3(2f, 2f, 1f);
			Sequence s = DOTween.Sequence();
			statsBtn.transform.localScale = localScale2;
			s.Insert(0f, statsBtn.transform.DOScale(localScale, 0.15f).OnComplete(delegate
			{
				statsBtn.transform.localScale = middleScale;
			}));
			s.Insert(0.15f, statsBtn.transform.DOScale(localScale, 0.15f));
		}
	}
}
