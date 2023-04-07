using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.messenger;
using com.ultrabit.bitheroes.model.augment;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.kongregate;
using com.ultrabit.bitheroes.ui.assets;
using com.ultrabit.bitheroes.ui.character;
using Sfs2X.Entities.Data;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.dungeon;

public class DungeonEntity : Messenger
{
	private int _index;

	private int _id;

	private int _type;

	private int _power;

	private int _stamina;

	private int _agility;

	private int _healthCurrent;

	private int _healthTotal;

	private int _shieldCurrent;

	private int _shieldTotal;

	private CharacterData _characterData;

	private int _consumables;

	private int _meter;

	public Vector2 selectOffset
	{
		get
		{
			if (_type == 3)
			{
				return FamiliarBook.Lookup(_id).selectOffset;
			}
			return default(Vector2);
		}
	}

	public float selectScale
	{
		get
		{
			if (_type == 3)
			{
				return FamiliarBook.Lookup(_id).selectScale;
			}
			return 1f;
		}
	}

	public string name => _type switch
	{
		1 => _characterData.parsedName, 
		3 => FamiliarBook.Lookup(_id).coloredName, 
		_ => "???", 
	};

	public int index => _index;

	public int id => _id;

	public int type => _type;

	public int healthCurrent => _healthCurrent;

	public int healthTotal => _healthTotal;

	public int shieldCurrent => _shieldCurrent;

	public int shieldTotal => _shieldTotal;

	public CharacterData characterData => _characterData;

	public int consumables => _consumables;

	public int meter => _meter;

	public DungeonEntity(int index, int id, int type, CharacterData characterData)
	{
		_index = index;
		_id = id;
		_type = type;
		SetCharacterData(characterData);
	}

	public void SetPower(int power)
	{
		_power = power;
	}

	public void SetStamina(int stamina)
	{
		_stamina = stamina;
	}

	public void SetAgility(int agility)
	{
		_agility = agility;
	}

	public void SetHealth(int healthCurrent, int healthTotal)
	{
		_healthCurrent = healthCurrent;
		_healthTotal = healthTotal;
		Broadcast("ENTITY_HEALTH");
	}

	public void SetShield(int shieldCurrent, int shieldTotal)
	{
		_shieldCurrent = shieldCurrent;
		_shieldTotal = shieldTotal;
		Broadcast("ENTITY_SHIELD");
	}

	public void SetMeter(int meter)
	{
		_meter = meter;
		Broadcast("ENTITY_METER");
	}

	public void SetConsumables(int consumables)
	{
		_consumables = consumables;
		Broadcast("CONSUMABLE_CHANGE");
	}

	public void SetCharacterData(CharacterData characterData)
	{
		_characterData = characterData;
	}

	public void Clear()
	{
		_characterData = null;
	}

	public static DungeonEntity FromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("dun18");
		int int2 = sfsob.GetInt("dun19");
		int int3 = sfsob.GetInt("dun20");
		int int4 = sfsob.GetInt("dun21");
		int int5 = sfsob.GetInt("dun22");
		int int6 = sfsob.GetInt("dun23");
		int int7 = sfsob.GetInt("dun24");
		int int8 = sfsob.GetInt("dun25");
		int int9 = sfsob.GetInt("dun33");
		int int10 = sfsob.GetInt("dun34");
		int int11 = sfsob.GetInt("dun26");
		int int12 = sfsob.GetInt("dun30");
		CharacterData characterData = CharacterData.fromSFSObject(sfsob);
		DungeonEntity dungeonEntity = new DungeonEntity(@int, int2, int3, characterData);
		dungeonEntity.SetPower(int4);
		dungeonEntity.SetStamina(int5);
		dungeonEntity.SetAgility(int6);
		dungeonEntity.SetHealth(int7, int8);
		dungeonEntity.SetShield(int9, int10);
		dungeonEntity.SetConsumables(int11);
		dungeonEntity.SetMeter(int12);
		return dungeonEntity;
	}

	public GameObject getAsset(float scale, Transform parent = null)
	{
		switch (_type)
		{
		case 1:
		{
			CharacterDisplay characterDisplay = _characterData.toCharacterDisplay(scale, displayMount: true);
			if (parent != null)
			{
				characterDisplay.transform.SetParent(parent, worldPositionStays: false);
				characterDisplay.transform.localPosition = Vector3.zero;
			}
			characterDisplay.gameObject.AddComponent<SWFAsset>();
			return characterDisplay.gameObject;
		}
		case 3:
		{
			Transform asset = FamiliarBook.Lookup(_id).displayRef.getAsset(center: true, scale, parent);
			if (asset != null)
			{
				asset.gameObject.AddComponent<SWFAsset>();
				return asset.gameObject;
			}
			break;
		}
		}
		return null;
	}

	public List<GameModifier> GetModifiers()
	{
		return _type switch
		{
			1 => _characterData.getModifiers(), 
			3 => FamiliarBook.Lookup(_id).modifiers, 
			_ => new List<GameModifier>(), 
		};
	}

	public Asset GetIconAsset(float scale)
	{
		switch (_type)
		{
		case 1:
			return _characterData.toCharacterDisplay().convertToIcon(center: true, scale);
		case 3:
			FamiliarBook.Lookup(_id);
			return null;
		default:
			return null;
		}
	}

	public bool IsDead()
	{
		return _healthCurrent <= 0;
	}

	public bool IsAlive()
	{
		return !IsDead();
	}

	public Dictionary<string, object> statEntityFamiliar()
	{
		FamiliarRef familiarRef = FamiliarBook.Lookup(_id);
		foreach (AugmentData familiarAugmentSlot in GameData.instance.PROJECT.character.augments.getFamiliarAugmentSlots(familiarRef))
		{
			if (familiarAugmentSlot != null && familiarAugmentSlot.familiarID == familiarRef.id)
			{
				return new Dictionary<string, object>
				{
					{ "familiar_name", familiarRef.statName },
					{
						"familiar_aug_itemType",
						familiarAugmentSlot.familiarRef.itemType
					},
					{
						"familiar_aug_rarity",
						familiarAugmentSlot.familiarRef.rarity
					}
				};
			}
		}
		return new Dictionary<string, object>
		{
			{ "familiar_name", familiarRef.statName },
			{ "familiar_itemType", familiarRef.itemType },
			{ "familiar_rarity", familiarRef.rarity }
		};
	}

	public KongregateAnalyticsSchema.StatEntityPlayer statEntityPlayer()
	{
		return new KongregateAnalyticsSchema.StatEntityPlayer
		{
			player_name = _characterData.name,
			player_total_stats = _characterData.getTotalStats()
		};
	}
}
