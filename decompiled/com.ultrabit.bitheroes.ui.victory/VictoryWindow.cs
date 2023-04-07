using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.battle;
using com.ultrabit.bitheroes.model.currency;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.sound;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.utility;
using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui.victory;

public class VictoryWindow : WindowsMain
{
	private int _type;

	private long _exp;

	private int _gold;

	private int _prevPower;

	private int _prevStamina;

	private int _prevAgility;

	private List<ItemData> _items;

	private bool _isRerunnable;

	private bool _animate = true;

	protected bool _isAutoClosable = true;

	public VictoryResults _results;

	public VictoryAnimation _animation;

	public CharacterEquipmentPanel _equipmentPanel;

	protected bool _shouldCheckTutorials = true;

	public UnityCustomEvent ON_CLOSE = new UnityCustomEvent();

	public UnityEvent ON_RERUN = new UnityEvent();

	public override void Start()
	{
		base.Start();
	}

	public virtual void LoadDetails(int type = 0, long exp = 0L, int gold = 0, List<ItemData> items = null, List<BattleStat> battleStats = null, bool shouldPlayMusic = true, string customShieldText = null, string customLootText = null, string customCloseText = null, bool isVictorious = true, bool isCloseRed = true, bool isRerunnable = false)
	{
		Disable();
		_type = type;
		_exp = exp;
		_gold = gold;
		_items = items;
		_isAutoClosable = customShieldText == null;
		_isRerunnable = isRerunnable;
		_animate = GameData.instance.SAVE_STATE.animations && GameData.instance.SAVE_STATE.GetAnimationsType(GameData.instance.PROJECT.character.id, 1, GameData.instance.SAVE_STATE.GetAnimationsTypes(GameData.instance.PROJECT.character.id));
		CreateWindow(closeWord: true, "", scroll: false, stayUp: true, 0.55f);
		_animation.ShieldAnimation(base.gameObject, _animate, customShieldText, isVictorious);
		int exp2 = 0;
		int gold2 = 0;
		if (exp > 0 || gold > 0)
		{
			CheckForGoldOrExp(items, out exp2, out gold2);
		}
		if (exp2 <= 0 && exp > 0)
		{
			items.Add(new ItemData(CurrencyBook.Lookup(3), (int)exp));
		}
		if (gold2 <= 0 && gold > 0)
		{
			items.Add(new ItemData(CurrencyBook.Lookup(1), gold));
		}
		_results.LoadDetails(_type, this, _exp, _gold, _items, animate: true, battleStats, base.gameObject, customLootText, isCloseRed, customCloseText, isRerunnable);
		_equipmentPanel.LoadDetails(GameData.instance.PROJECT.character.toCharacterData(), editable: true);
		_equipmentPanel.UpdateLayer();
		GameData.instance.PROJECT.character.AddListener("STATS_CHANGE", OnStatChange);
		GameData.instance.PROJECT.character.equipment.OnChange.AddListener(OnEquipmentChange);
		GameData.instance.PROJECT.character.equipment.BeforeChange.AddListener(OnEquipmentBeforeChange);
		UpdateStats();
		if (shouldPlayMusic)
		{
			GameData.instance.audioManager.StopMusic();
			SoundRef soundRef = SoundBook.Lookup("victory");
			if (soundRef != null)
			{
				GameData.instance.audioManager.PlaySound(soundRef);
				StartCoroutine(DoMusic(soundRef.duration));
			}
		}
		ListenForBack(OnClose);
		if (_isRerunnable)
		{
			ListenForForward(_results.OnRerunBtn);
		}
		else
		{
			ListenForForward(OnClose);
		}
	}

	public void UpdateStats()
	{
		_equipmentPanel.SetStats(GameData.instance.PROJECT.character.getTotalPower(), GameData.instance.PROJECT.character.getTotalStamina(), GameData.instance.PROJECT.character.getTotalAgility());
	}

	private void CheckForGoldOrExp(List<ItemData> items, out int exp, out int gold)
	{
		exp = 0;
		gold = 0;
		foreach (ItemData item in items)
		{
			if (item.type != 3)
			{
				continue;
			}
			if (item.id == 1 && gold <= 0)
			{
				gold = item.qty;
				if (exp > 0)
				{
					break;
				}
			}
			if (item.id == 3 && exp <= 0)
			{
				exp = item.qty;
				if (gold > 0)
				{
					break;
				}
			}
		}
	}

	private void OnStatChange()
	{
		UpdateStats();
	}

	private IEnumerator DoMusic(float delay)
	{
		yield return new WaitForSeconds(delay);
		if (!base.scrollingOut)
		{
			GameData.instance.audioManager.PlayMusicLink("victory");
		}
	}

	private void OnEquipmentBeforeChange()
	{
		_prevPower = GameData.instance.PROJECT.character.getTotalPower();
		_prevStamina = GameData.instance.PROJECT.character.getTotalStamina();
		_prevAgility = GameData.instance.PROJECT.character.getTotalAgility();
	}

	private void OnEquipmentChange()
	{
		_equipmentPanel.SetCharacterData(GameData.instance.PROJECT.character.toCharacterData());
		_equipmentPanel.SetNewStats(0, _prevPower);
		_equipmentPanel.SetNewStats(1, _prevStamina);
		_equipmentPanel.SetNewStats(2, _prevAgility);
	}

	private void OnResultsComplete()
	{
		int type = _type;
		if (type != 2 && type != 9 && GameData.instance.PROJECT.character.autoPilot && !GameData.instance.tutorialManager.hasPopup && _isAutoClosable)
		{
			OnClose();
		}
	}

	public void OnAnimationComplete()
	{
		ForceScrollDown();
		_results.Animate(_animate);
		if (_isAutoClosable && !_animate)
		{
			SCROLL_IN_COMPLETE.AddListener(OnScrollInComplete);
		}
	}

	private void OnScrollInComplete(object e)
	{
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		if (_shouldCheckTutorials)
		{
			_results.CheckTutorial();
		}
	}

	public override void UpdateSortingLayers(int layer)
	{
		base.UpdateSortingLayers(layer);
		_equipmentPanel.UpdateLayer();
	}

	public override void OnClose()
	{
		if (_results.closeable)
		{
			_animation.ForceTweenUp();
			if (!_isRerunnable)
			{
				ON_CLOSE.Invoke(this);
			}
			base.OnClose();
			GameData.instance.tutorialManager.ClearTutorial();
		}
	}

	public void OnCloseReRun()
	{
		if (_results.closeable)
		{
			_animation.ForceTweenUp();
			base.OnClose();
			GameData.instance.tutorialManager.ClearTutorial();
		}
	}

	public override void Enable()
	{
		base.Enable();
		ActivateCloseBtn();
		_results.DoEnable();
	}

	public override void Disable()
	{
		base.Disable();
		DeactivateCloseBtn();
		_results.DoDisable();
	}

	public override void DoDestroy()
	{
		GameData.instance.PROJECT.character.RemoveListener("STATS_CHANGE", OnStatChange);
		GameData.instance.PROJECT.character.equipment.OnChange.RemoveListener(OnEquipmentChange);
		GameData.instance.PROJECT.character.equipment.BeforeChange.RemoveListener(OnEquipmentBeforeChange);
		ON_CLOSE.RemoveAllListeners();
		ON_RERUN.RemoveAllListeners();
		SCROLL_IN_COMPLETE.RemoveListener(OnScrollInComplete);
		base.DoDestroy();
	}
}
