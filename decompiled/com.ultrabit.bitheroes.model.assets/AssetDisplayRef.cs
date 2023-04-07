using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.mount;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml.common;
using com.ultrabit.bitheroes.ui.assets;
using com.ultrabit.bitheroes.ui.character;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.assets;

[DebuggerDisplay("{link} (AssetDisplayRef)")]
public class AssetDisplayRef : IEquatable<AssetDisplayRef>, IComparable<AssetDisplayRef>
{
	private string _asset;

	private string _definition;

	private bool _loadLocal;

	private float _scale;

	private Vector2 _point;

	private Vector2 _position;

	private bool _flipped;

	private int _hair;

	private int _hairColor;

	private int _skinColor;

	private string _gender;

	private Equipment _equipment;

	private Mounts _mounts;

	private int _itemID;

	private int _itemType;

	private string _link;

	private bool _loaded;

	public string assetURL => _asset;

	public string definition => _definition;

	public Vector2 point => _point;

	public Vector2 position => _position;

	public string link => _link;

	public string asset => _asset;

	public float scale => _scale;

	public int hair => _hair;

	public int hairColor => _hairColor;

	public int skinColor => _skinColor;

	public string gender => _gender;

	public bool flipped => _flipped;

	public Equipment equipment => _equipment;

	public Mounts mounts => _mounts;

	public int itemID => _itemID;

	public int itemType => _itemType;

	public AssetDisplayRef(AssetDisplayData objectData, int assetType = -1)
	{
		_itemID = objectData.itemID;
		_itemType = ((!Util.IsEmptyString(objectData.itemType)) ? ItemRef.getItemType(objectData.itemType) : (-1));
		if (assetType == -1 && !Util.IsEmptyString(objectData.itemType))
		{
			assetType = AssetURL.GetAssetTypeByItemType(_itemType, icon: false);
		}
		if (!Util.IsEmptyString(objectData.asset))
		{
			if (assetType == -1)
			{
				D.LogError("AssetDisplayRef::Constructor cannot get asset for " + objectData.asset + " asset type is not defined");
			}
			_asset = AssetURL.GetPath(assetType, objectData.asset);
		}
		_definition = objectData.definition;
		_scale = Util.GetFloatFromStringProperty(objectData.scale, 1f);
		_point = Util.pointFromString(objectData.point);
		_position = Util.pointFromString(objectData.position);
		_flipped = Util.parseBoolean(objectData.flipped, defaultVal: false);
		_hair = objectData.hair;
		_hairColor = objectData.hairColor;
		_skinColor = objectData.skinColor;
		_gender = objectData.gender;
		foreach (AssetDisplayData.Equipment item in objectData.lstEquipment)
		{
			if (_equipment == null)
			{
				_equipment = new Equipment();
			}
			EquipmentRef equipRef = EquipmentBook.Lookup(item.id);
			_equipment.equipItem(equipRef);
		}
		_mounts = null;
		foreach (AssetDisplayData.Mount item2 in objectData.lstMount)
		{
			MountData mountData = MountData.fromXML(item2.id);
			if (mountData != null)
			{
				List<MountData> list = new List<MountData> { mountData };
				_mounts = new Mounts(mountData.uid, null, list);
			}
		}
	}

	public void loadAssets()
	{
		if (!_loaded)
		{
			_loaded = true;
			getAsset();
		}
	}

	public Transform getAsset(bool center = false, float scale = 1f, Transform parent = null)
	{
		Transform assetObject = getAssetObject(center, scale, parent);
		if (assetObject != null && _flipped)
		{
			assetObject.localScale.Scale(new Vector3(-1f, 1f, 1f));
		}
		return assetObject;
	}

	private Transform getAssetObject(bool center = false, float scale = 1f, Transform parent = null)
	{
		scale *= _scale;
		if (string.IsNullOrEmpty(_asset) && (_equipment != null || _mounts != null))
		{
			Transform transform = null;
			CharacterDisplay characterDisplay = GameData.instance.windowGenerator.GetCharacterDisplay(new CharacterPuppetInfoDefault());
			if (characterDisplay != null)
			{
				CharacterPuppetInfo characterDisplay2 = new CharacterPuppetInfoDefault(CharacterPuppet.ParseGenderFromString(_gender), _hair, _hairColor, _skinColor, scale, 1f, _equipment, _mounts, showHelm: true, showMount: true, showBody: true, showAccessory: true, null, enableLoading: false);
				characterDisplay.SetCharacterDisplay(characterDisplay2);
				transform = characterDisplay.transform;
				if (transform != null && transform.gameObject != null)
				{
					if (parent != null)
					{
						transform.SetParent(parent, worldPositionStays: false);
					}
					if (center)
					{
						characterDisplay.SetLocalPosition(Vector3.zero);
					}
					transform.gameObject.AddComponent<SWFAsset>();
					return transform.transform;
				}
			}
		}
		ItemRef itemRef = getItemRef();
		if (_asset == null && itemRef != null)
		{
			switch (itemRef.itemType)
			{
			case 6:
			{
				FamiliarRef familiarRef = itemRef as FamiliarRef;
				if (familiarRef != null && familiarRef.displayRef != null)
				{
					return familiarRef.displayRef.getAsset(center, scale);
				}
				break;
			}
			case 8:
			{
				MountRef mountRef = itemRef as MountRef;
				if (mountRef != null)
				{
					return mountRef.getAsset(center, scale, setDefaults: true);
				}
				break;
			}
			}
			return GameData.instance.main.assetLoader.GetTransformAsset(AssetURL.GetAssetType(itemRef, icon: true), itemRef.icon);
		}
		if (_asset == null)
		{
			return null;
		}
		Transform transformAsset = GameData.instance.main.assetLoader.GetTransformAsset(_asset);
		if (transformAsset == null)
		{
			return null;
		}
		if (parent != null)
		{
			transformAsset.transform.SetParent(parent, worldPositionStays: false);
		}
		if (center)
		{
			if (parent != null)
			{
				transformAsset.transform.localPosition = Vector3.zero;
			}
			else
			{
				transformAsset.transform.position = Vector3.zero;
			}
		}
		transformAsset.transform.localScale = new Vector3(scale, scale, 1f);
		return transformAsset;
	}

	public ItemRef getItemRef()
	{
		if (_itemID < 0 || _itemType < 0)
		{
			return null;
		}
		return ItemBook.Lookup(_itemID, _itemType);
	}

	public void setLink(string link)
	{
		_link = link;
	}

	public bool Equals(AssetDisplayRef other)
	{
		if (other == null)
		{
			return false;
		}
		return assetURL.Equals(other.assetURL);
	}

	public int CompareTo(AssetDisplayRef other)
	{
		if (other == null)
		{
			return -1;
		}
		return link.CompareTo(other.link);
	}
}
