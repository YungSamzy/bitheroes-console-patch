using com.ultrabit.bitheroes.model.battle;
using com.ultrabit.bitheroes.ui.menu;
using com.ultrabit.bitheroes.ui.utility;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.battle;

public class BattleUI : WindowsMain
{
	[Header("UI")]
	public RectTransform placeholderDamageGain;

	public RectTransform placeholderDisplay;

	public RectTransform placeholderAbilities;

	public RectTransform placeholderBattleSpeed;

	public RectTransform placeholderBattleConsumables;

	public RectTransform placeholderBattleFormation;

	public LoadingOverlay _loadingOverlay;

	[Header("Prefabs")]
	public Transform countBarPrefab;

	public Transform damageGainTilePrefab;

	public Transform battleSpeedTilePrefab;

	public Transform battleConsumablesTilePrefab;

	public Transform battleFormationTilePrefab;

	public Transform battleTimeBarPrefab;

	public Transform abilityTilePrefab;

	private Battle _battle;

	private TimeBarColor _timeBar;

	private CountBar _countBar;

	private BattleDamageGainTile _damageGainTile;

	private MenuInterfaceBattleSpeedTile _battleSpeedTile;

	private MenuInterfaceBattleConsumablesTile _battleConsumablesTile;

	private MenuInterfaceBattleFormationTile _battleFormationTile;

	public TimeBarColor timeBar => _timeBar;

	public CountBar countBar => _countBar;

	public BattleDamageGainTile damageGainTile => _damageGainTile;

	public MenuInterfaceBattleSpeedTile battleSpeedTile => _battleSpeedTile;

	public MenuInterfaceBattleConsumablesTile battleConsumablesTile => _battleConsumablesTile;

	public MenuInterfaceBattleFormationTile battleFormationTile => _battleFormationTile;

	public override void Start()
	{
		base.Start();
	}

	public void LoadDetails(Battle battle)
	{
		Disable();
		_battle = battle;
		_battle.battleUI = this;
		CreateWindow(closeWord: false, "", scroll: false);
		Enable();
	}

	public void LoadMenu()
	{
		_damageGainTile = Object.Instantiate(damageGainTilePrefab, placeholderDamageGain).GetComponent<BattleDamageGainTile>();
		_battleSpeedTile = Object.Instantiate(battleSpeedTilePrefab, placeholderBattleSpeed).GetComponent<MenuInterfaceBattleSpeedTile>();
		_battleSpeedTile.Create(_battle);
		_battleConsumablesTile = Object.Instantiate(battleConsumablesTilePrefab, placeholderBattleConsumables).GetComponent<MenuInterfaceBattleConsumablesTile>();
		_battleConsumablesTile.Create(_battle);
		_battleFormationTile = Object.Instantiate(battleFormationTilePrefab, placeholderBattleFormation).GetComponent<MenuInterfaceBattleFormationTile>();
		_battleFormationTile.Create(_battle);
		_loadingOverlay.Hide();
		_damageGainTile.SELECT.AddListener(OnDamageGainSelect);
	}

	private void VerifyCloseButton(bool status = true)
	{
		status = _battle.type != 9 && status;
		closeBtn.gameObject.SetActive(status);
	}

	public void LoadAditionalSettings()
	{
		if (_battle.realtime)
		{
			Transform transform = Object.Instantiate(battleTimeBarPrefab);
			transform.SetParent(placeholderDisplay, worldPositionStays: false);
			_timeBar = transform.GetComponentInChildren<TimeBarColor>();
			_timeBar.SetShowDescription(show: false);
			_timeBar.ForceStart(invokeSeconds: false);
			_timeBar.COMPLETE.AddListener(OnTurnTimer);
		}
		else
		{
			BattleTeamData teamData = _battle.GetTeamData(!_battle.GetAttackerFocus());
			if (teamData != null && (teamData.poolTotal > 1 || _battle.type == 8))
			{
				Transform transform2 = Object.Instantiate(countBarPrefab);
				transform2.SetParent(placeholderDisplay, worldPositionStays: false);
				_countBar = transform2.GetComponent<CountBar>();
				_countBar.LoadDetails(teamData.poolCurrent, teamData.poolTotal);
			}
			else
			{
				placeholderDisplay.gameObject.SetActive(value: false);
			}
		}
		_battle.UpdateButtons();
		VerifyCloseButton();
	}

	public void Show(bool show)
	{
		_battleSpeedTile.gameObject.SetActive(show);
		_battleConsumablesTile.gameObject.SetActive(show);
		_battleFormationTile.gameObject.SetActive(show);
		VerifyCloseButton(show);
	}

	private void OnDamageGainSelect()
	{
		_battle.OnDamageGainSelect();
	}

	private void OnTurnTimer()
	{
		_battle.OnTurnTimer();
	}

	public override void OnClose()
	{
		_battle.DoExitConfirm(clicked: true);
	}

	public override void DoDestroy()
	{
		if (_damageGainTile != null)
		{
			_damageGainTile.SELECT.RemoveListener(OnDamageGainSelect);
		}
		if (_timeBar != null)
		{
			_timeBar.COMPLETE.RemoveListener(OnTurnTimer);
		}
		base.DoDestroy();
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
}
