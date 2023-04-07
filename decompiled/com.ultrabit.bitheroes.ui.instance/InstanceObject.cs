using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.guild;
using com.ultrabit.bitheroes.model.instance;
using com.ultrabit.bitheroes.model.leaderboard;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.armory;
using com.ultrabit.bitheroes.ui.assets;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.grid;
using com.ultrabit.bitheroes.ui.utility;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.instance;

public class InstanceObject : GridObject
{
	public const int TYPE_CHARACTER = 1;

	public const int TYPE_OTHER = 2;

	public const int TIMER_DELAY = 10000;

	private Instance _instance;

	private int _type;

	private InstanceObjectRef _objectRef;

	private GameObject _sprite;

	private int _actionIndex;

	private IEnumerator _timer;

	private IEnumerator _actionDelay;

	private float count;

	private float total;

	private float sum = 0.01f;

	private int finalPos;

	public int type => _type;

	public InstanceObjectRef objectRef => _objectRef;

	public string identifier => objectRef.tileID + "_" + objectRef.displayRef.asset;

	public void CreateInstanceObject(Instance instance, int type, Tile tile, float speed = 250f, bool clickable = true, object data = null)
	{
		if (data is InstanceObjectRef)
		{
			_objectRef = data as InstanceObjectRef;
		}
		LoadDetails(instance, tile, speed, clickable, data, (_objectRef != null) ? _objectRef.order : 0);
		_instance = instance;
		_type = type;
		SetData(data);
	}

	public void SetGlobalData(Instance instance, int type, Tile tile, float speed = 250f, bool clickable = true, object data = null)
	{
		if (data is InstanceObjectRef)
		{
			_objectRef = data as InstanceObjectRef;
		}
		LoadDetails(instance, tile, speed, clickable, data, (_objectRef != null) ? _objectRef.order : 0);
		_instance = instance;
		_type = type;
		base.SetData(data);
		CheckActions();
		SetAsset(base.transform.gameObject);
		SetTile(tile);
		_instance.AddObject(this);
	}

	public void SetInitialSortingOrder(int order)
	{
		setPosition(order);
	}

	public override void SetData(object data)
	{
		base.SetData(data);
		LoadAssets();
		CheckActions();
	}

	public override void CheckActions()
	{
		base.CheckActions();
		if (!base.gameObject.activeSelf || base.data == null || !(base.data is InstanceObjectRef))
		{
			return;
		}
		InstanceObjectRef instanceObjectRef = base.data as InstanceObjectRef;
		InstanceActionRef action = instanceObjectRef.getAction(_actionIndex);
		if (action == null)
		{
			_actionIndex = 0;
			action = instanceObjectRef.getAction(_actionIndex);
		}
		if (action == null)
		{
			return;
		}
		switch (action.type)
		{
		case 0:
		{
			if (_actionDelay != null)
			{
				StopCoroutine(_actionDelay);
				_actionDelay = null;
			}
			float seconds = ((action.values.Count > 1) ? Util.RandomNumber(Util.ParseFloat(action.getValue()), Util.ParseFloat(action.getValue(1))) : Util.ParseFloat(action.getValue()));
			_actionDelay = Delay(seconds);
			StartCoroutine(_actionDelay);
			break;
		}
		case 1:
			SetPath(_instance.GeneratePath(this, _instance.getTileByID(int.Parse(action.getValue(Util.randomInt(0, action.values.Count - 1))))));
			break;
		case 2:
			PlayAnimation(action.getValue(Util.randomInt(0, action.values.Count - 1)), loop: false);
			break;
		}
		_actionIndex++;
	}

	private IEnumerator Delay(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		CheckActions();
	}

	public void LoadAssets()
	{
		_ = Instance.OBJECT_SCALE;
		_ = Instance.OBJECT_SCALE;
		bool flag = false;
		if (base.asset != null)
		{
			_ = base.asset.transform.localScale;
			_ = base.asset.transform.localScale;
			SetAsset((GameObject)null, (Vector2?)null);
		}
		if (_sprite != null)
		{
			Object.Destroy(_sprite);
			_sprite = null;
		}
		switch (_type)
		{
		case 1:
		{
			CharacterData obj2 = base.data as CharacterData;
			InstancePlayer instancePlayer = this as InstancePlayer;
			List<object> overrideEquipment = null;
			if (instancePlayer.fishingData != null)
			{
				overrideEquipment = new List<object> { instancePlayer.fishingData.rodRef };
			}
			CharacterDisplay characterDisplay = obj2.toCharacterDisplay(Instance.OBJECT_SCALE, displayMount: true, overrideEquipment);
			if (!characterDisplay.hasMountEquipped())
			{
				characterDisplay.characterPuppet.transform.position = new Vector3(0f, 63f, 0f);
			}
			characterDisplay.transform.SetParent(base.transform);
			characterDisplay.SetLocalPosition(Vector3.zero);
			characterDisplay.gameObject.AddComponent<SWFAsset>();
			flag = instancePlayer.flipped;
			SetAsset(characterDisplay.gameObject);
			break;
		}
		case 2:
		{
			InstanceObjectRef instanceObjectRef = base.data as InstanceObjectRef;
			_ = instanceObjectRef.offset;
			SetOffset(instanceObjectRef.offset);
			switch (instanceObjectRef.type)
			{
			case 29:
			{
				ArmoryManekin armoryManekin2 = _instance.AddArmoryManekinObject(this, instanceObjectRef);
				if (armoryManekin2 != null)
				{
					_sprite = armoryManekin2.gameObject;
				}
				break;
			}
			case 30:
			{
				ArmoryManekin armoryManekin = _instance.AddArmoryManekinObject(this, instanceObjectRef);
				if (armoryManekin != null)
				{
					_sprite = armoryManekin.gameObject;
				}
				break;
			}
			case 4:
				if (_instance.previousPvPEventData != null)
				{
					_sprite = _instance.AddEventStatusObject(this, _instance.previousPvPEventData.leaders, _instance.previousPvPEventData.eventRef).gameObject;
					_sprite.gameObject.name = "PVPEventStatue";
				}
				break;
			case 23:
				if (_instance.previousFishingEventData == null)
				{
					break;
				}
				foreach (LeaderboardData leader in _instance.previousFishingEventData.leaders)
				{
					D.Log($"LeaderboardData {leader.id} - {leader.parsedName}");
				}
				_sprite = _instance.AddEventStatusObject(this, _instance.previousFishingEventData.leaders, _instance.previousFishingEventData.eventRef).gameObject;
				_sprite.gameObject.name = "FishingEventStatue";
				break;
			case 7:
			{
				Transform transform3 = instanceObjectRef.displayRef.getAsset(center: true, Instance.OBJECT_SCALE, base.transform);
				GameObject gameObject = null;
				if (transform3 != null)
				{
					gameObject = transform3.gameObject;
				}
				SetAsset(gameObject);
				StartTimer();
				break;
			}
			case 16:
			{
				GuildHallData obj = _instance.data as GuildHallData;
				GuildHallCosmeticTypeRef guildHallCosmeticTypeRef = GuildHallBook.LookupCosmeticType(instanceObjectRef.value);
				GuildHallCosmeticRef guildHallCosmeticRef = GuildHallBook.LookupCosmetic(obj.getCosmetic(guildHallCosmeticTypeRef.id), guildHallCosmeticTypeRef.id);
				Transform transform4 = instanceObjectRef.displayRef.getAsset(center: true, Instance.OBJECT_SCALE, base.transform);
				if (transform4 != null)
				{
					FrameNavigator[] componentsInChildren = transform4.GetComponentsInChildren<FrameNavigator>();
					foreach (FrameNavigator frameNavigator in componentsInChildren)
					{
						frameNavigator.GoToAndStop(guildHallCosmeticRef.frame);
						if (instanceObjectRef.definition != "object" && instanceObjectRef.definition != "asset" && frameNavigator.gameObject.name != instanceObjectRef.definition)
						{
							frameNavigator.gameObject.SetActive(value: false);
						}
					}
					transform4.gameObject.SetActive(value: true);
					transform4.gameObject.isStatic = true;
				}
				foreach (GameObject item in Util.GetChildsWithTag(transform4, "guildhall"))
				{
					item.SetActive(value: false);
				}
				foreach (string @object in guildHallCosmeticRef.objects)
				{
					_instance.CheckObjectLinkWithParent(@object, transform4.gameObject);
				}
				flag = instanceObjectRef.flipped;
				RepositionGuildObject(instanceObjectRef.order);
				break;
			}
			default:
			{
				Transform transform = instanceObjectRef.displayRef.getAsset(center: true, Instance.OBJECT_SCALE, base.transform);
				if (transform != null)
				{
					transform.gameObject.SetActive(value: true);
					transform.gameObject.isStatic = true;
					SetAsset(transform.gameObject);
					if (instanceObjectRef.definition != null && instanceObjectRef.definition != "")
					{
						Transform transform2 = transform.transform;
						for (int i = 0; i < transform2.childCount; i++)
						{
							if (transform2.GetChild(i).name != instanceObjectRef.definition)
							{
								transform2.GetChild(i).gameObject.SetActive(value: false);
							}
						}
					}
				}
				flag = instanceObjectRef.flipped;
				break;
			}
			}
			break;
		}
		}
		if (base.asset != null)
		{
			OnAssetLoaded();
			base.asset.transform.rotation = Quaternion.Euler(base.asset.transform.rotation.eulerAngles.x, flag ? 180 : 0, base.asset.transform.rotation.eulerAngles.z);
		}
		if (_sprite != null)
		{
			_sprite.transform.rotation = Quaternion.Euler(_sprite.transform.rotation.eulerAngles.x, flag ? 180 : 0, _sprite.transform.rotation.eulerAngles.z);
		}
	}

	public void RepositionGuildObject(int pos)
	{
		finalPos = pos;
		setPosition(finalPos);
		_instance.guildCount += sum;
		total = _instance.guildCount;
		InvokeRepeating("RepositionGuildObjectTimer", _instance.guildCount, sum);
	}

	private void RepositionGuildObjectTimer()
	{
		count += sum;
		setPosition(finalPos);
		if (count >= total)
		{
			setPosition(finalPos);
			CancelInvoke("RepositionGuildObjectTimer");
		}
	}

	private void OnAssetLoaded()
	{
		DoUpdate();
	}

	private void OnAnimationEnd()
	{
		CheckActions();
	}

	private IEnumerator OnTimer(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		DoUpdate();
	}

	public void DoUpdate()
	{
		if (base.data != null && base.data is InstanceObjectRef)
		{
			InstanceObjectRef instanceObjectRef = base.data as InstanceObjectRef;
			if (instanceObjectRef.type == 16)
			{
				UpdateObjectLinks(add: true);
			}
			if (!(base.asset == null) && instanceObjectRef.type == 29)
			{
				D.LogError("ArmoryManekin not implemented");
			}
		}
	}

	public void UpdateObjectLinks(bool add)
	{
		if (base.data == null || !(base.data is InstanceObjectRef))
		{
			return;
		}
		InstanceObjectRef instanceObjectRef = base.data as InstanceObjectRef;
		if (instanceObjectRef.type != 16)
		{
			return;
		}
		GuildHallData obj = _instance.data as GuildHallData;
		GuildHallCosmeticTypeRef guildHallCosmeticTypeRef = GuildHallBook.LookupCosmeticType(instanceObjectRef.value);
		GuildHallCosmeticRef guildHallCosmeticRef = GuildHallBook.LookupCosmetic(obj.getCosmetic(guildHallCosmeticTypeRef.id), guildHallCosmeticTypeRef.id);
		Transform transform = ((base.transform.childCount != 0) ? base.transform.GetChild(0) : instanceObjectRef.displayRef.getAsset(center: true, Instance.OBJECT_SCALE, base.transform));
		if (transform != null)
		{
			FrameNavigator[] componentsInChildren = transform.GetComponentsInChildren<FrameNavigator>();
			foreach (FrameNavigator frameNavigator in componentsInChildren)
			{
				frameNavigator.GoToAndStop(guildHallCosmeticRef.frame);
				if (instanceObjectRef.definition != "object" && instanceObjectRef.definition != "asset" && frameNavigator.gameObject.name != instanceObjectRef.definition)
				{
					frameNavigator.gameObject.SetActive(value: false);
				}
			}
			transform.gameObject.SetActive(value: true);
			transform.gameObject.isStatic = true;
		}
		foreach (GameObject item in Util.GetChildsWithTag(transform, "guildhall"))
		{
			item.SetActive(value: false);
		}
		foreach (string @object in guildHallCosmeticRef.objects)
		{
			_instance.CheckObjectLinkWithParent(@object, transform.gameObject);
		}
		RepositionGuildObject(instanceObjectRef.order);
	}

	private void StartTimer()
	{
		ClearTimer();
		_timer = OnTimer(10000f);
		DoUpdate();
	}

	private void ClearTimer()
	{
		if (_timer != null)
		{
			StopCoroutine(_timer);
			_timer = null;
		}
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		ClearTimer();
		if (base.asset != null && base.asset.GetComponent<HoverAndGlowSprites>() != null)
		{
			Object.Destroy(base.asset.GetComponent<HoverAndGlowSprites>());
		}
	}
}
