using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.dungeon;
using com.ultrabit.bitheroes.model.encounter;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.npc;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.assets;
using com.ultrabit.bitheroes.ui.grid;
using DG.Tweening;
using Sfs2X.Entities.Data;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.dungeon;

public class DungeonObject : GridObject
{
	public const int TYPE_ENEMY = 0;

	public const int TYPE_TREASURE = 1;

	public const int TYPE_BOSS = 2;

	public const int TYPE_SHRINE = 3;

	public const int TYPE_LOOTABLE = 4;

	public const int TYPE_MERCHANT = 5;

	public const int TYPE_AD = 6;

	private static Dictionary<int, string> TYPE_NAMES = new Dictionary<int, string>
	{
		[0] = "dungeon_enemy",
		[1] = "dungeon_treasure_name",
		[2] = "ui_brawl_short",
		[3] = "dungeon_shrine",
		[4] = "ui_loot",
		[5] = "dungeon_merchant_name",
		[6] = "dungeon_ad"
	};

	public GameObject loadingIcon;

	public DungeonExclamationPoint exclamationIcon;

	private int _id;

	private int _type;

	private DungeonObjectRef _objectRef;

	private Dungeon _dungeon;

	private DungeonNode _node;

	private IEnumerator _timer;

	private List<ItemData> _items;

	private List<NPCRef> _npcs;

	private int _pathDistance;

	private bool _disabled;

	private bool _ableToStartPathing;

	public bool ignorePath
	{
		get
		{
			if (_type == 3 && GameData.instance.SAVE_STATE.ignoreShrines)
			{
				return true;
			}
			if (_type == 2 && GameData.instance.SAVE_STATE.ignoreBoss)
			{
				return true;
			}
			return false;
		}
	}

	public bool ignoreClear
	{
		get
		{
			if (_type == 3 && GameData.instance.SAVE_STATE.ignoreShrines)
			{
				return true;
			}
			return false;
		}
	}

	public int id => _id;

	public int type => _type;

	public DungeonNode node => _node;

	public bool collision
	{
		get
		{
			if (_objectRef == null)
			{
				return false;
			}
			return _objectRef.collision;
		}
	}

	public bool instant
	{
		get
		{
			if (_objectRef == null)
			{
				return false;
			}
			return _objectRef.instant;
		}
	}

	public bool clickable
	{
		get
		{
			if (_objectRef == null)
			{
				return true;
			}
			return _objectRef.clickable;
		}
	}

	public int distance
	{
		get
		{
			if (_objectRef == null)
			{
				return 0;
			}
			return _objectRef.distance;
		}
	}

	public DungeonObjectRef objectRef => _objectRef;

	public List<ItemData> items => _items;

	public int pathDistance => _pathDistance;

	public bool disabled => _disabled;

	public void Create(int id, int type, List<ItemData> items, List<NPCRef> npcs)
	{
		switch (type)
		{
		case 1:
			_objectRef = DungeonBook.LookupTreasure(id);
			if (_objectRef == null)
			{
				D.LogError("david", "DungeonObject::Create Treasure produce null ObjectRef for ID:" + id);
			}
			break;
		case 3:
			_objectRef = DungeonBook.LookupShrine(id);
			if (_objectRef == null)
			{
				D.LogError("david", "DungeonObject::Create Shrine produce null ObjectRef for ID:" + id);
			}
			break;
		case 4:
			_objectRef = DungeonBook.LookupLootable(id);
			if (_objectRef == null)
			{
				D.LogError("david", "DungeonObject::Create Lootable produce null ObjectRef for ID:" + id);
			}
			break;
		case 5:
			_pathDistance = 1;
			_objectRef = DungeonBook.LookupMerchant(id);
			if (_objectRef == null)
			{
				D.LogError("david", "DungeonObject::Create Merchant produce null ObjectRef for ID:" + id);
			}
			break;
		case 6:
			_objectRef = DungeonBook.LookupAd(id);
			if (_objectRef == null)
			{
				D.LogError("david", "DungeonObject::Create Ad produce null ObjectRef for ID:" + id);
			}
			break;
		}
		LoadDetails(null, null, 125f, clickable);
		_id = id;
		_type = type;
		_items = items;
		_npcs = npcs;
		SetExclamation();
	}

	private Transform GetTransformAsset(string url)
	{
		if (url == null)
		{
			D.LogError("DungeonObject::GetTransformAsset can't load asset of null url");
			Object.Destroy(base.gameObject);
		}
		Transform transformAsset = GameData.instance.main.assetLoader.GetTransformAsset(url);
		if (transformAsset != null)
		{
			transformAsset.SetParent(base.transform);
			transformAsset.localPosition = Vector3.zero;
		}
		return transformAsset;
	}

	private Transform GetTransformAsset(int type, string url)
	{
		if (url == null)
		{
			D.LogError("DungeonObject::GetTransformAsset can't load asset of type: " + type + " with null url");
			Object.Destroy(base.gameObject);
		}
		return GetTransformAsset(AssetURL.GetPath(type, url));
	}

	public void OnAddedToStage()
	{
		DungeonRef dungeonRef = Dungeon.instance.dungeonRef;
		if (dungeonRef == null)
		{
			return;
		}
		if (objectRef != null && objectRef.displayRef != null)
		{
			Transform transform = objectRef.displayRef.getAsset();
			if (transform != null && transform.gameObject != null)
			{
				transform.SetParent(base.transform);
				transform.localPosition = Vector3.zero;
				transform.transform.localScale = new Vector3(2f, 2f, 1f);
				if (loadingIcon != null)
				{
					loadingIcon.SetActive(value: false);
				}
				transform.gameObject.AddComponent<BoxCollider2D>();
				SetAsset(transform.gameObject);
			}
		}
		GameObject gameObject = null;
		switch (_type)
		{
		case 0:
		{
			DungeonEnemyRef enemy = dungeonRef.getEnemy(_id);
			if (enemy != null)
			{
				gameObject = GetNPCAsset(enemy.encounter);
				if (gameObject != null && gameObject.gameObject != null)
				{
					gameObject.AddComponent<SWFAsset>();
				}
				SetAsset(gameObject);
			}
			break;
		}
		case 2:
		{
			DungeonBossRef boss = dungeonRef.boss;
			if (boss != null)
			{
				gameObject = GetNPCAsset(boss.encounter);
				if (gameObject != null && gameObject.gameObject != null)
				{
					gameObject.AddComponent<SWFAsset>();
				}
				SetAsset(gameObject);
			}
			break;
		}
		}
		if (gameObject != null)
		{
			OnAssetLoaded();
		}
	}

	private void Update()
	{
		ENTER_FRAME.Invoke(this);
	}

	public override void SetAsset(Asset asset, Vector2? offset = null)
	{
		base.SetAsset(asset, offset);
	}

	public void SetDungeon(Dungeon dungeon, DungeonNode node)
	{
		_dungeon = dungeon;
		_node = node;
		Tile tile = _node.GetTile();
		if (CanPath())
		{
			tile = _node.GetRandomCenterTile();
		}
		else if (_objectRef != null)
		{
			if (_objectRef.spread == 1)
			{
				tile = _node.GetRandomCenterTile();
			}
			else if (_objectRef.spread > 1)
			{
				tile = _node.GetRandomWalkableTile();
			}
		}
		SetGrid(dungeon);
		SetTile(tile, tween: false);
		_ableToStartPathing = true;
		if (base.gameObject.activeSelf)
		{
			StartPathing();
		}
	}

	public override void SetExclamation(bool enabled = false)
	{
		base.SetExclamation(enabled);
		if (enabled)
		{
			exclamationIcon.gameObject.SetActive(value: true);
			exclamationIcon.DoFlash();
			exclamationIcon.ZoomIn(HideExclamation);
		}
		else
		{
			RemoveExclamation();
		}
	}

	private void HideExclamation()
	{
		float num = 1f;
		float num2 = 1f;
		if (AppInfo.TESTING)
		{
			num /= 3f;
			num2 /= 3f;
		}
		exclamationIcon.ChangeAlpha(null, 0f, num2, Ease.Unset, num, RemoveExclamation);
	}

	private void RemoveExclamation()
	{
		exclamationIcon.gameObject.SetActive(value: false);
	}

	public bool CanPath()
	{
		switch (_type)
		{
		case 1:
		case 3:
		case 4:
		case 6:
			return false;
		default:
			return true;
		}
	}

	private void StartPathing()
	{
		if (CanPath())
		{
			StartTimer();
		}
	}

	public void StopPathing()
	{
		ClearTimer();
		ClearPath(finish: false);
	}

	private void StartTimer()
	{
		ClearTimer();
		_timer = OnTimer(Util.randomInt(1, 4));
		StartCoroutine(_timer);
	}

	private void ClearTimer()
	{
		if (_timer != null)
		{
			StopCoroutine(_timer);
			_timer = null;
		}
	}

	private IEnumerator OnTimer(float delay)
	{
		yield return new WaitForSeconds(delay);
		if (!_dungeon.sceneDestroyed)
		{
			Tile randomCenterTile = _node.GetRandomCenterTile(base.tile);
			SetPath(_dungeon.GeneratePath(this, randomCenterTile));
			StartTimer();
		}
	}

	private GameObject GetNPCAsset(EncounterRef encounterRef)
	{
		if (_npcs != null && _npcs.Count > 0)
		{
			List<NPCRef> bossesFromList = NPCRef.getBossesFromList(_npcs);
			if (bossesFromList != null && bossesFromList.Count > 0)
			{
				NPCRef nPCRef = bossesFromList[Util.randomInt(0, bossesFromList.Count - 1)];
				Transform transformAsset = GetTransformAsset(nPCRef.displayRef.assetURL);
				if (transformAsset != null)
				{
					transformAsset.localScale = new Vector3(2f * nPCRef.scale, 2f * nPCRef.scale, 1f);
					return transformAsset.gameObject;
				}
			}
			NPCRef nPCRef2 = _npcs[Util.randomInt(0, _npcs.Count - 1)];
			Transform transformAsset2 = GetTransformAsset(nPCRef2.displayRef.assetURL);
			if (transformAsset2 != null)
			{
				transformAsset2.localScale = new Vector3(2f * nPCRef2.scale, 2f * nPCRef2.scale, 1f);
				return transformAsset2.gameObject;
			}
		}
		return null;
	}

	private void OnAssetLoaded()
	{
		loadingIcon.SetActive(value: false);
		loadingIcon = null;
		PlayAnimation("idle");
	}

	public ItemData GetFirstItem()
	{
		if (_items == null || _items.Count <= 0)
		{
			return null;
		}
		return _items[0];
	}

	public void DisableObject()
	{
		_disabled = true;
		ChangeColliders(enabled: false);
		if (type == 3)
		{
			GetComponentInChildren<Animator>().Play("Disabled");
		}
	}

	public void DoDestroy()
	{
		if (!(_dungeon == null))
		{
			if (base.tile != null)
			{
				base.tile.SetWalkable(walkable: true);
			}
			_dungeon.RemoveObject(this);
			_dungeon.RemoveDungeonObject(this);
			ClearTimer();
			Object.Destroy(base.gameObject);
		}
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		ClearTimer();
	}

	public static DungeonObject FromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("dun13");
		int int2 = sfsob.GetInt("dun14");
		List<ItemData> list = ItemData.listFromSFSObject(sfsob);
		List<NPCRef> npcs = NPCRef.listFromSFSObject(sfsob);
		DungeonObject dungeonObject = Object.Instantiate(Dungeon.instance.dungeonObjectPrefab);
		dungeonObject.name = "DO" + @int + "-" + int2;
		dungeonObject.Create(@int, int2, list, npcs);
		return dungeonObject;
	}

	public static string GetTypeName(int type)
	{
		return Language.GetString(TYPE_NAMES[type]);
	}

	public void OnEnable()
	{
		if (type == 3)
		{
			Animator componentInChildren = GetComponentInChildren<Animator>();
			if (componentInChildren != null)
			{
				if (_disabled)
				{
					componentInChildren.Play("Disabled");
				}
				else
				{
					componentInChildren.Play("Enabled");
				}
			}
		}
		if (_ableToStartPathing)
		{
			StartPathing();
		}
	}
}
