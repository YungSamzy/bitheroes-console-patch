using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.extensions;
using com.ultrabit.bitheroes.model.service;
using com.ultrabit.bitheroes.ui.battle;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.dalc;

public class BattleDALC : BaseDALC
{
	public const int QUEUE = 1;

	public const int ABILITY = 2;

	public const int CONSUMABLE = 3;

	public const int RESULTS = 4;

	public const int AUTO = 5;

	public const int ORDER = 6;

	public const int QUIT = 7;

	public const int CAPTURE_ACCEPT = 8;

	public const int CAPTURE_DECLINE = 9;

	public const int USE_DAMAGE_GAIN = 10;

	private bool _waiting;

	private bool _doingResult;

	private static BattleDALC _instance;

	public static BattleDALC instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new BattleDALC();
			}
			return _instance;
		}
	}

	public bool waiting => _waiting;

	public bool doingResult
	{
		get
		{
			return _doingResult;
		}
		set
		{
			_doingResult = value;
		}
	}

	public void doAbility(int abilityID, BattleEntity selectEntity = null)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 2);
		sFSObject.PutInt("bat22", abilityID);
		if (selectEntity != null)
		{
			sFSObject.PutInt("bat7", selectEntity.index);
		}
		send(sFSObject);
	}

	public void doConsumable(BattleEntity entity, int itemID)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 3);
		sFSObject.PutInt("bat7", entity.index);
		sFSObject.PutInt("ite0", itemID);
		send(sFSObject);
	}

	public void doResults()
	{
		_doingResult = true;
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 4);
		send(sFSObject);
	}

	public void doAuto(bool useDamageGain = true)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 5);
		sFSObject.PutBool("bat57", useDamageGain);
		send(sFSObject);
	}

	public void doOrder(int[] order)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 6);
		sFSObject.PutIntArray("bat2", order);
		send(sFSObject);
	}

	public void doQuit()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 7);
		send(sFSObject);
	}

	public void doCaptureAccept(BattleEntity entity, ServiceRef serviceRef, int currencyID, int currencyCost)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 8);
		sFSObject.PutInt("bat7", entity.index);
		sFSObject.PutInt("ite0", serviceRef.id);
		sFSObject.PutInt("curr0", currencyID);
		sFSObject.PutInt("curr2", currencyCost);
		send(sFSObject);
	}

	public void doCaptureDecline(BattleEntity entity)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 9);
		sFSObject.PutInt("bat7", entity.index);
		send(sFSObject);
	}

	public void doUseDamageGain()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("act0", 10);
		send(sFSObject);
	}

	public override void parse(SFSObject sfsob)
	{
		_waiting = false;
		GameData.instance.PROJECT.battle.battleUI._loadingOverlay.Hide();
		base.parse(sfsob);
	}

	protected override void send(SFSObject sfsob)
	{
		sfsob.GetInt("act0");
		_waiting = true;
		GameData.instance.PROJECT.battle.ClearWindows();
		GameData.instance.PROJECT.battle.UpdateButtons();
		GameData.instance.PROJECT.battle.battleUI._loadingOverlay.Show();
		sfsob.PutInt("dal0", id);
		ServerExtension.instance.Send(sfsob);
	}
}
