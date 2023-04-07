using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.sound;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.lists.battleresultslist;
using com.ultrabit.bitheroes.ui.utility;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.battle;

public class BattleResultsWindowOld : WindowsMain
{
	public TextMeshProUGUI topperTxt;

	public TextMeshProUGUI goldTxt;

	public TextMeshProUGUI goldNameTxt;

	public TextMeshProUGUI expTxt;

	public TextMeshProUGUI expNameTxt;

	public RegularBarFill expBar;

	public Image expBg;

	public TextMeshProUGUI levelTxt;

	public TextMeshProUGUI itemsTxt;

	public Button statsBtn;

	public RectTransform placehoderLevelUp;

	public GameObject battleResultsListView;

	public GameObject battleResultsListScroll;

	public BattleResultsList battleResultsList;

	private long _exp;

	private int _gold;

	private List<ItemData> _items;

	private long _currentExp;

	private long _pendingExp;

	private long _tickExp;

	private int _currentGold;

	private int _pendingGold;

	private int _tickGold;

	private int _currentLevel;

	private bool _levelUp;

	private bool update;

	public UnityCustomEvent COMPLETE = new UnityCustomEvent();

	private bool statsBtnAnimation;

	public override void Start()
	{
		base.Start();
		Disable();
	}

	public void LoadDetails(bool victory, long exp = 0L, int gold = 0, List<ItemData> items = null)
	{
		_exp = exp;
		_gold = gold;
		_items = items;
		topperTxt.text = (victory ? Language.GetString("battle_victory") : Language.GetString("battle_defeat"));
		_currentExp = GameData.instance.PROJECT.character.exp - _exp;
		_pendingExp = _exp;
		_tickExp = (long)Mathf.Round((float)_exp / (1f / Time.smoothDeltaTime * 1.25f));
		_currentGold = GameData.instance.PROJECT.character.gold - _gold;
		_pendingGold = _gold;
		_tickGold = (int)Mathf.Round((float)_gold / (1f / Time.smoothDeltaTime * 1.25f));
		_currentLevel = Character.getExpLevel(_currentExp);
		_levelUp = _currentLevel != GameData.instance.PROJECT.character.level;
		if (AppInfo.TESTING)
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
		statsBtn.GetComponent<GlowNoOver>().ForceStart();
		statsBtn.GetComponent<GlowNoOver>().ON_ENTER.AddListener(OnStatsBtnEnter);
		statsBtn.gameObject.SetActive(value: false);
		statsBtn.GetComponentInChildren<TextMeshProUGUI>().text = Language.GetString("ui_plus");
		GameData.instance.PROJECT.character.equipment.OnChange.AddListener(OnEquipmentChange);
		GameData.instance.PROJECT.character.AddListener("STATS_CHANGE", OnStatsChange);
		GameData.instance.PROJECT.character.AddListener("POINTS_CHANGE", OnStatsChange);
		if (items != null && items.Count > 0)
		{
			battleResultsList.InitList();
			CreateItems();
			itemsTxt.gameObject.SetActive(value: false);
		}
		else
		{
			itemsTxt.gameObject.SetActive(value: true);
			itemsTxt.text = Language.GetString("battle_no_items_found");
			battleResultsListView.SetActive(value: false);
			battleResultsListScroll.SetActive(value: false);
		}
		if (victory)
		{
			SoundRef soundRef = SoundBook.Lookup("victory");
			if (soundRef != null)
			{
				GameData.instance.audioManager.PlaySound(soundRef);
				StartCoroutine(DoMusic(soundRef.duration));
			}
		}
		DoUpdate();
		ListenForBack(OnClose);
		ListenForForward(OnClose);
		float num = 0.5f;
		SetClose(enabled: false);
		if (AppInfo.TESTING)
		{
			num /= 3f;
		}
		StartCoroutine(DelayAnimation(num));
		CreateWindow(closeWord: true);
	}

	private IEnumerator DoMusic(float delay)
	{
		yield return new WaitForSeconds(delay);
		if (!base.scrollingOut)
		{
			GameData.instance.audioManager.PlayMusicLink("victory");
		}
	}

	private void OnStatsBtnEnter()
	{
		statsBtn.GetComponent<GlowNoOver>().ON_ENTER.RemoveListener(OnStatsBtnEnter);
		Object.Destroy(statsBtn.GetComponent<GlowNoOver>());
	}

	public void OnStatsBtn()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
		GameData.instance.windowGenerator.NewCharacterStatWindow();
	}

	private IEnumerator DelayAnimation(float delay)
	{
		yield return new WaitForSeconds(delay);
		Animate();
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

	private void CreateItems()
	{
		battleResultsList.ClearList();
		foreach (ItemData item in ItemData.SortLoot(_items))
		{
			if (item != null)
			{
				battleResultsList.Data.InsertOneAtEnd(new BattleResultsItem
				{
					itemData = item
				});
			}
		}
	}

	private void Animate()
	{
		SetClose(enabled: true);
		update = true;
	}

	private void SetClose(bool enabled)
	{
		if (enabled)
		{
			Util.SetButton(closeBtn);
		}
		else
		{
			Util.SetButton(closeBtn, enabled: false);
		}
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
					ScaleStatsBtn();
					Transform obj = Object.Instantiate(GameData.instance.main.assetLoader.GetAsset<Transform>("ui/battle/BattleTextUI"));
					obj.SetParent(placehoderLevelUp, worldPositionStays: false);
					obj.GetComponent<TextMeshProUGUI>().fontSize = 8f;
					obj.GetComponent<TextOnStage>().Set(Language.GetString("ui_level_up"), BattleText.COLOR_YELLOW);
					StartCoroutine(WaitToGlow());
					GameData.instance.audioManager.PlaySoundLink("levelup");
					RefillEnergy();
					KongregateAnalytics.updateCommonFields();
					GameData.instance.PROJECT.character.updateAchievements();
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
			update = false;
		}
		DoUpdate();
	}

	private void RefillEnergy()
	{
		int num = GameData.instance.PROJECT.character.energyMax - GameData.instance.PROJECT.character.energy;
		if (num > 0)
		{
			GameData.instance.PROJECT.character.addItem(new ItemData(CurrencyBook.Lookup(4), num));
		}
	}

	private IEnumerator WaitToGlow()
	{
		yield return new WaitForSeconds(0.5f);
		if (statsBtn != null && statsBtn.GetComponent<GlowNoOver>() != null)
		{
			statsBtn.GetComponent<GlowNoOver>().StartGlow();
		}
	}

	public void DoUpdate()
	{
		string text = ((_pendingGold > 0) ? ("+" + Util.colorString(Util.NumberFormat(_pendingGold), "#FFFF77") + " ") : "");
		goldNameTxt.text = text + CurrencyRef.GetCurrencyName(1);
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

	public override void OnClose()
	{
		if (closeBtn.enabled)
		{
			COMPLETE.Invoke(this);
			base.OnClose();
		}
	}

	public override void DoDestroy()
	{
		GameData.instance.PROJECT.character.equipment.OnChange.RemoveListener(OnEquipmentChange);
		GameData.instance.PROJECT.character.RemoveListener("STATS_CHANGE", OnStatsChange);
		GameData.instance.PROJECT.character.RemoveListener("POINTS_CHANGE", OnStatsChange);
		if ((bool)statsBtn.GetComponent<GlowNoOver>())
		{
			statsBtn.GetComponent<GlowNoOver>().ON_ENTER.RemoveListener(OnStatsBtnEnter);
		}
		base.DoDestroy();
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		statsBtn.interactable = true;
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		statsBtn.interactable = false;
	}

	private void ScaleStatsBtn()
	{
		if (!statsBtnAnimation)
		{
			statsBtnAnimation = true;
			Vector3 localScale = statsBtn.transform.localScale;
			Vector3 endValue = new Vector3(0.9f, 0.9f, 1f);
			Vector3 localScale2 = new Vector3(2f, 2f, 1f);
			statsBtn.transform.localScale = localScale2;
			Sequence sequence = DOTween.Sequence();
			sequence.Insert(0f, statsBtn.transform.DOScale(endValue, 0.25f));
			sequence.Insert(0.25f, statsBtn.transform.DOScale(localScale, 0.25f));
			sequence.OnComplete(delegate
			{
				statsBtnAnimation = false;
			});
		}
	}
}
