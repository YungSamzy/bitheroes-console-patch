using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.fromflash;
using com.ultrabit.bitheroes.model.ability;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using com.ultrabit.bitheroes.model.xml.common;
using com.ultrabit.bitheroes.ui.assets;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.mount;

[DebuggerDisplay("{name} (MountRef)")]
public class MountRef : ItemRef, IEquatable<MountRef>, IComparable<MountRef>
{
	private MountRarityRef _mountRarityRef;

	private MountStats _stats;

	private List<AbilityRef> _abilities;

	private List<MountModifierRef> _modifiers;

	private List<GameModifier> _baseModifiers;

	private string _asset;

	private bool _hideFoot;

	private bool _rerollable;

	public MountRarityRef mountRarityRef => _mountRarityRef;

	public bool rerollable => _rerollable;

	public bool hideFoot => _hideFoot;

	public MountStats stats => _stats;

	public List<AbilityRef> abilities => _abilities;

	public List<MountModifierRef> modifiers => _modifiers;

	public List<GameModifier> baseModifiers => _baseModifiers;

	public string asset => _asset;

	public MountRef(int id, MountBookData.Mount mountData)
		: base(id, 8)
	{
		_stats = new MountStats(Util.GetFloatFromStringProperty(mountData.power), Util.GetFloatFromStringProperty(mountData.stamina), Util.GetFloatFromStringProperty(mountData.agility));
		_mountRarityRef = MountBook.LookupRarity(mountData.rarity);
		_abilities = AbilityBook.LookupAbilities(mountData.abilities);
		_modifiers = MountBook.GetRarityModifiers(_mountRarityRef.rarityRef.id);
		_baseModifiers = new List<GameModifier>();
		if (mountData.modifier != null)
		{
			_baseModifiers.Add(new GameModifier(mountData.modifier));
		}
		if (mountData.modifiers != null)
		{
			foreach (GameModifierData modifier in mountData.modifiers)
			{
				if (modifier != null)
				{
					_baseModifiers.Add(new GameModifier(modifier));
				}
			}
		}
		_asset = mountData.asset;
		_hideFoot = Util.GetBoolFromStringProperty(mountData.hideFoot, defaultValue: true);
		_rerollable = Util.GetBoolFromStringProperty(mountData.rerollable, defaultValue: true);
		LoadDetails(mountData);
	}

	public List<GameModifier> getGameModifiers()
	{
		List<GameModifier> list = new List<GameModifier>();
		foreach (GameModifier modifier in _mountRarityRef.modifiers)
		{
			list.Add(modifier);
		}
		foreach (GameModifier baseModifier in _baseModifiers)
		{
			list.Add(baseModifier);
		}
		return list;
	}

	public Transform getAsset(bool center = false, float scale = 1f, bool setDefaults = false)
	{
		return GameData.instance.main.assetLoader.GetTransformAsset(AssetURL.MOUNT, _asset);
	}

	public GameObject getGOAsset(bool center = false, float scale = 1f, bool setDefaults = false)
	{
		string text = _asset;
		GameObject gameObjectAsset = GameData.instance.main.assetLoader.GetGameObjectAsset(AssetURL.MOUNT, text, instantiate: false);
		if (gameObjectAsset != null && gameObjectAsset.GetComponent<SWFAsset>() == null)
		{
			gameObjectAsset.AddComponent<SWFAsset>();
		}
		return gameObjectAsset;
	}

	private void onAssetLoaded(CustomSFSXEvent e)
	{
		_ = e.currentTarget;
		throw new Exception("Error --> CONTROL");
	}

	private void onAssetChange(CustomSFSXEvent e)
	{
		Asset asset = e.currentTarget as Asset;
		clearAssetCharacter(asset);
	}

	private void setAssetDefaults(Asset asset)
	{
		if (asset is SWFAsset)
		{
			(asset as SWFAsset).PlayAnimation("idle");
		}
		clearAssetCharacter(asset);
	}

	private void clearAssetCharacter(Asset asset)
	{
		if (asset is SWFAsset)
		{
			DisplayObject childWithName = Util.getChildWithName((asset as SWFAsset).theObject, "character");
			if (childWithName != null)
			{
				childWithName.visible = false;
			}
		}
	}

	public bool Equals(MountRef other)
	{
		if (other == null)
		{
			return false;
		}
		return base.id.Equals(other.id);
	}

	public int CompareTo(MountRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return base.id.CompareTo(other.id);
	}
}
