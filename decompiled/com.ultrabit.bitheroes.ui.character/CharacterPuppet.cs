using System;
using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.ui.assets;
using UnityEngine;
using UnityEngine.Rendering;

namespace com.ultrabit.bitheroes.ui.character;

public abstract class CharacterPuppet : MonoBehaviour
{
	public class CharacterPuppetEquipmentRef
	{
		public Vector3 offset;

		public float offsetDivider;

		public float scale;

		private SortingGroup _sortingGroup;

		private SpriteRenderer _spriteRenderer;

		private Transform _transform;

		private int _overrideSortingLayer = -1;

		public string assetName;

		public SortingGroup sortingGroup => _sortingGroup;

		public SpriteRenderer spriteRenderer => _spriteRenderer;

		public Transform transform
		{
			get
			{
				if (_transform != null)
				{
					return _transform;
				}
				if (_sortingGroup != null)
				{
					return _sortingGroup.transform;
				}
				if (_spriteRenderer != null)
				{
					return _spriteRenderer.transform;
				}
				return null;
			}
			set
			{
				_transform = value;
			}
		}

		public int sortingLayer
		{
			get
			{
				if (_overrideSortingLayer >= 0)
				{
					return _overrideSortingLayer;
				}
				if (_sortingGroup != null)
				{
					return _sortingGroup.sortingOrder;
				}
				if (_spriteRenderer != null)
				{
					return _spriteRenderer.sortingOrder;
				}
				return 0;
			}
			set
			{
				_overrideSortingLayer = value;
			}
		}

		public CharacterPuppetEquipmentRef(Transform transform, Vector3 offset, float offsetDivider = 0.01f, float scale = 0.01f)
		{
			_transform = transform;
			assetName = "asset";
			this.offset = offset;
			this.offsetDivider = offsetDivider;
			this.scale = scale;
		}

		public CharacterPuppetEquipmentRef(SortingGroup sortingGroup, Vector3 offset, float offsetDivider = 0.01f, float scale = 0.01f)
		{
			_sortingGroup = sortingGroup;
			assetName = "asset";
			this.offset = offset;
			this.offsetDivider = offsetDivider;
			this.scale = scale;
		}

		public CharacterPuppetEquipmentRef(SpriteRenderer spriteRenderer, Vector3 offset, float offsetDivider = 0.01f, float scale = 0.01f)
		{
			_spriteRenderer = spriteRenderer;
			assetName = "asset";
			this.offset = offset;
			this.offsetDivider = offsetDivider;
			this.scale = scale;
		}
	}

	public enum AnimationSequence
	{
		idle,
		walk,
		attack1,
		attack2,
		attackAOE,
		attack4,
		attackStaff1,
		attackStaff2,
		attackStaff3,
		attackBow1,
		attackBow2,
		attackBow3,
		attackCrossbow1,
		attackCrossbow2,
		attackGun1,
		hit,
		fishingCastingStart,
		fishingCastingIdle,
		fishingCast,
		fishingCatchingStart,
		fishingCatchingIdle,
		trigger
	}

	public enum Gender
	{
		MALE,
		FEMALE,
		NEUTRAL
	}

	public const string SUBPART_UNDERSKIN = "underSkin";

	public const string SUBPART_SKIN = "skin";

	public const string SUBPART_EQUIPMENT = "equipment";

	public const string SUBPART_HAIR = "hair";

	public const string SUBPART_OVERALL = "overall";

	public const string BP_SHADOW = "shadow";

	public const string BP_EFFECTS = "effects";

	public const string BP_EFFECTS2 = "effects2";

	public const string BP_BACK = "back";

	public static string BP_BACK_EQUIPMENT = "back_equipment";

	public const string BP_RIGHT_HAND = "rightHand";

	public static string BP_RIGHT_HAND_UNDERSKIN = "rightHand_underSkin";

	public static string BP_RIGHT_HAND_SKIN = "rightHand_skin";

	public static string BP_RIGHT_HAND_EQUIPMENT = "rightHand_equipment";

	public static string BP_RIGHT_HAND_OVERALL = "rightHand_overall";

	public const string BP_RIGHT_OBJECT = "rightObject";

	public const string BP_RIGHT_FOOT = "rightFoot";

	public static string BP_RIGHT_FOOT_UNDERSKIN = "rightFoot_underSkin";

	public static string BP_RIGHT_FOOT_SKIN = "rightFoot_skin";

	public static string BP_RIGHT_FOOT_EQUIPMENT = "rightFoot_equipment";

	public static string BP_RIGHT_FOOT_OVERALL = "rightFoot_overall";

	public const string BP_LEFT_FOOT = "leftFoot";

	public static string BP_LEFT_FOOT_UNDERSKIN = "leftFoot_underSkin";

	public static string BP_LEFT_FOOT_SKIN = "leftFoot_skin";

	public static string BP_LEFT_FOOT_EQUIPMENT = "leftFoot_equipment";

	public static string BP_LEFT_FOOT_OVERALL = "leftFoot_overall";

	public const string BP_TORSO = "torso";

	public static string BP_TORSO_UNDERSKIN = "torso_underSkin";

	public static string BP_TORSO_SKIN = "torso_skin";

	public static string BP_TORSO_EQUIPMENT = "torso_equipment";

	public static string BP_TORSO_OVERALL = "torso_overall";

	public const string BP_HEAD = "head";

	public static string BP_HEAD_UNDERSKIN = "head_underSkin";

	public static string BP_HEAD_SKIN = "head_skin";

	public static string BP_HEAD_EQUIPMENT = "head_equipment";

	public static string BP_HEAD_HAIR = "head_hair";

	public static string BP_HEAD_OVERALL = "head_overall";

	public const string BP_LEFT_HAND = "leftHand";

	public static string BP_LEFT_HAND_UNDERSKIN = "leftHand_underSkin";

	public static string BP_LEFT_HAND_SKIN = "leftHand_skin";

	public static string BP_LEFT_HAND_EQUIPMENT = "leftHand_equipment";

	public static string BP_LEFT_HAND_OVERALL = "leftHand_overall";

	public const string BP_WEAPON_FRONT = "weaponFront";

	public const string BP_PET = "pet";

	public const string BP_LEFT_OBJECT = "leftObject";

	public const string BP_COVER = "cover";

	public static Vector3 OFFSET_BACK = new Vector3(0f, 0.02f, 0f);

	public static Vector3 OFFSET_RIGHT_HAND = new Vector3(-0.005f, 0.04f, 0f);

	public static Vector3 OFFSET_RIGHT_OBJECT = Vector3.zero;

	public static Vector3 OFFSET_RIGHT_FOOT = new Vector3(-0.005f, 0.03f, 0f);

	public static Vector3 OFFSET_LEFT_FOOT = new Vector3(0.005f, 0.03f, 0f);

	public static Vector3 OFFSET_TORSO = new Vector3(0f, 0.01f, 0f);

	public static Vector3 OFFSET_HEAD = new Vector3(0.02f, -0.07f, 0f);

	public static Vector3 OFFSET_LEFT_HAND = new Vector3(0.005f, 0.045f, 0f);

	public static Vector3 OFFSET_PET = Vector3.zero;

	public static Vector3 OFFSET_LEFT_OBJECT = Vector3.zero;

	public static Vector3 OFFSET_COVER = Vector3.zero;

	[Header("Character Puppet References")]
	[SerializeField]
	protected SortingGroup shadow;

	[Space(15f)]
	[SerializeField]
	protected SortingGroup back;

	[SerializeField]
	protected SortingGroup backEquipment;

	[Space(15f)]
	[SerializeField]
	protected SortingGroup rightHand;

	[SerializeField]
	protected SortingGroup rightHandUnderSkin;

	[SerializeField]
	protected Component rightHandSkin;

	[SerializeField]
	protected SortingGroup rightHandEquipment;

	[SerializeField]
	protected SortingGroup rightHandOverall;

	[Space(15f)]
	[SerializeField]
	protected SortingGroup rightObject;

	[Space(15f)]
	[SerializeField]
	protected SortingGroup rightFoot;

	[SerializeField]
	protected SortingGroup rightFootUnderSkin;

	[SerializeField]
	protected Component rightFootSkin;

	[SerializeField]
	protected SortingGroup rightFootEquipment;

	[SerializeField]
	protected SortingGroup rightFootOverall;

	[Space(15f)]
	[SerializeField]
	protected SortingGroup leftFoot;

	[SerializeField]
	protected SortingGroup leftFootUnderSkin;

	[SerializeField]
	protected Component leftFootSkin;

	[SerializeField]
	protected SortingGroup leftFootEquipment;

	[SerializeField]
	protected SortingGroup leftFootOverall;

	[Space(15f)]
	[SerializeField]
	protected SortingGroup torso;

	[SerializeField]
	protected SortingGroup torsoUnderSkin;

	[SerializeField]
	protected Component torsoSkin;

	[SerializeField]
	protected SortingGroup torsoEquipment;

	[SerializeField]
	protected SortingGroup torsoOverall;

	[Space(15f)]
	[SerializeField]
	protected SortingGroup head;

	[SerializeField]
	protected SortingGroup headUnderSkin;

	[SerializeField]
	protected Component headSkin;

	[SerializeField]
	protected SortingGroup headEquipment;

	[SerializeField]
	protected SortingGroup headHair;

	[SerializeField]
	protected SortingGroup headOverall;

	[Space(15f)]
	[SerializeField]
	protected SortingGroup leftHand;

	[SerializeField]
	protected SortingGroup leftHandUnderSkin;

	[SerializeField]
	protected Component leftHandSkin;

	[SerializeField]
	protected SortingGroup leftHandEquipment;

	[SerializeField]
	protected SortingGroup leftHandOverall;

	[Space(15f)]
	[SerializeField]
	protected SortingGroup pet;

	[SerializeField]
	protected SortingGroup leftObject;

	[SerializeField]
	protected SortingGroup cover;

	[Space(15f)]
	[SerializeField]
	protected GameObject loading;

	[SerializeField]
	protected Animator animator;

	protected Transform _mainhandAsset;

	protected float _scale;

	protected float _headScale;

	protected bool _showHelm;

	protected bool _showMount;

	protected bool _showBody;

	protected bool _showAccessory;

	private Gender _genre;

	protected Equipment _equipment;

	protected List<object> _equipmentOverride;

	protected bool _showLeg = true;

	protected SpriteRenderer[] _childRenderers;

	protected Dictionary<string, CharacterPuppetEquipmentRef> characterPuppetEquipmentReference;

	protected Dictionary<AnimationSequence, string> animationTriggers = new Dictionary<AnimationSequence, string>
	{
		[AnimationSequence.attack1] = "trigger1",
		[AnimationSequence.attack2] = "trigger2",
		[AnimationSequence.attackAOE] = "trigger3",
		[AnimationSequence.attack4] = "trigger4",
		[AnimationSequence.attackStaff1] = "triggerStaff1",
		[AnimationSequence.attackStaff2] = "triggerStaff2",
		[AnimationSequence.attackStaff3] = "triggerStaff3",
		[AnimationSequence.attackBow1] = "triggerBow1",
		[AnimationSequence.attackBow2] = "triggerBow2",
		[AnimationSequence.attackBow3] = "triggerBow3",
		[AnimationSequence.attackCrossbow1] = "triggerCrossbow1",
		[AnimationSequence.attackCrossbow2] = "triggerCrossbow2",
		[AnimationSequence.attackGun1] = "triggerGun1",
		[AnimationSequence.fishingCast] = "triggerCast"
	};

	protected AnimationSequence currentSequence;

	protected string sequenceName = "idle";

	protected string mainURL = "Equipment/";

	protected bool _enableLoading;

	public Action ON_ENABLED;

	protected Dictionary<string, int> equippedItemsPerType = new Dictionary<string, int>();

	public Transform petAsset => pet.transform;

	public Transform mainHandAsset => _mainhandAsset;

	public virtual Gender gender
	{
		get
		{
			return _genre;
		}
		set
		{
			_genre = value;
		}
	}

	public string label => currentSequence.ToString();

	protected virtual void Awake()
	{
		characterPuppetEquipmentReference = new Dictionary<string, CharacterPuppetEquipmentRef>
		{
			["back"] = new CharacterPuppetEquipmentRef(back, OFFSET_BACK),
			[BP_BACK_EQUIPMENT] = new CharacterPuppetEquipmentRef(backEquipment, OFFSET_BACK),
			["rightHand"] = new CharacterPuppetEquipmentRef(rightHand, OFFSET_RIGHT_HAND),
			[BP_RIGHT_HAND_UNDERSKIN] = new CharacterPuppetEquipmentRef(rightHandUnderSkin, OFFSET_RIGHT_HAND),
			[BP_RIGHT_HAND_EQUIPMENT] = new CharacterPuppetEquipmentRef(rightHandEquipment, OFFSET_RIGHT_HAND),
			[BP_RIGHT_HAND_OVERALL] = new CharacterPuppetEquipmentRef(rightHandOverall, OFFSET_RIGHT_HAND),
			["rightObject"] = new CharacterPuppetEquipmentRef(rightObject, OFFSET_RIGHT_OBJECT),
			["rightFoot"] = new CharacterPuppetEquipmentRef(rightFoot, OFFSET_RIGHT_FOOT),
			[BP_RIGHT_FOOT_UNDERSKIN] = new CharacterPuppetEquipmentRef(rightFootUnderSkin, OFFSET_RIGHT_FOOT),
			[BP_RIGHT_FOOT_EQUIPMENT] = new CharacterPuppetEquipmentRef(rightFootEquipment, OFFSET_RIGHT_FOOT),
			[BP_RIGHT_FOOT_OVERALL] = new CharacterPuppetEquipmentRef(rightFootOverall, OFFSET_RIGHT_FOOT),
			["leftFoot"] = new CharacterPuppetEquipmentRef(leftFoot, OFFSET_LEFT_FOOT),
			[BP_LEFT_FOOT_UNDERSKIN] = new CharacterPuppetEquipmentRef(leftFootUnderSkin, OFFSET_LEFT_FOOT),
			[BP_LEFT_FOOT_EQUIPMENT] = new CharacterPuppetEquipmentRef(leftFootEquipment, OFFSET_LEFT_FOOT),
			[BP_LEFT_FOOT_OVERALL] = new CharacterPuppetEquipmentRef(leftFootOverall, OFFSET_LEFT_FOOT),
			["torso"] = new CharacterPuppetEquipmentRef(torso, OFFSET_TORSO),
			[BP_TORSO_UNDERSKIN] = new CharacterPuppetEquipmentRef(torsoUnderSkin, OFFSET_TORSO),
			[BP_TORSO_EQUIPMENT] = new CharacterPuppetEquipmentRef(torsoEquipment, OFFSET_TORSO),
			[BP_TORSO_OVERALL] = new CharacterPuppetEquipmentRef(torsoOverall, OFFSET_TORSO),
			["head"] = new CharacterPuppetEquipmentRef(head, OFFSET_HEAD),
			[BP_HEAD_UNDERSKIN] = new CharacterPuppetEquipmentRef(headUnderSkin, OFFSET_HEAD),
			[BP_HEAD_EQUIPMENT] = new CharacterPuppetEquipmentRef(headEquipment, OFFSET_HEAD),
			[BP_HEAD_HAIR] = new CharacterPuppetEquipmentRef(headHair, OFFSET_HEAD),
			[BP_HEAD_OVERALL] = new CharacterPuppetEquipmentRef(headOverall, OFFSET_HEAD),
			["leftHand"] = new CharacterPuppetEquipmentRef(leftHand, OFFSET_LEFT_HAND),
			[BP_LEFT_HAND_UNDERSKIN] = new CharacterPuppetEquipmentRef(leftHandUnderSkin, OFFSET_LEFT_HAND),
			[BP_LEFT_HAND_EQUIPMENT] = new CharacterPuppetEquipmentRef(leftHandEquipment, OFFSET_LEFT_HAND),
			[BP_LEFT_HAND_OVERALL] = new CharacterPuppetEquipmentRef(leftHandOverall, OFFSET_LEFT_HAND),
			["pet"] = new CharacterPuppetEquipmentRef(pet, OFFSET_PET),
			["leftObject"] = new CharacterPuppetEquipmentRef(leftObject, OFFSET_LEFT_OBJECT),
			["cover"] = new CharacterPuppetEquipmentRef(cover, OFFSET_COVER)
		};
	}

	protected virtual void Start()
	{
		loading.SetActive(value: false);
	}

	public abstract void SetCharacterPuppet(CharacterPuppetInfo puppetInfo);

	public void ShowLeg(bool show)
	{
		_showLeg = show;
		rightFoot.gameObject.SetActive(show);
	}

	public abstract void ShowHair(bool showHair);

	public abstract void ShowHeadSkin(bool showHeadSkin);

	public void ShowShadow(bool show)
	{
		shadow.gameObject.SetActive(show);
	}

	public void ChangeSortingLayer(int newLayer)
	{
		ChangeSortingLayerRecursively(base.transform, newLayer);
	}

	public void ChangeSortingLayerRecursively(Transform tr, int newLayer)
	{
		if (tr.TryGetComponent<SpriteRenderer>(out var component) && component.enabled)
		{
			component.sortingOrder = component.sortingOrder % 100 + newLayer;
		}
		for (int i = 0; i < tr.childCount; i++)
		{
			ChangeSortingLayerRecursively(tr.GetChild(i), newLayer);
		}
	}

	public void ChangeSortingLayerForEquipment(int newLayer)
	{
		foreach (EquipmentRef displayItem in _equipment.getDisplayItems())
		{
			if (displayItem == null || displayItem.assets == null)
			{
				continue;
			}
			foreach (EquipmentAssetRef asset in displayItem.assets)
			{
				CharacterPuppetEquipmentRef characterPuppetEquipmentRef = null;
				string bodyPart = asset.bodyPart;
				if (characterPuppetEquipmentReference.ContainsKey(bodyPart))
				{
					characterPuppetEquipmentRef = characterPuppetEquipmentReference[bodyPart];
				}
				if (characterPuppetEquipmentRef == null)
				{
					continue;
				}
				Transform transform = characterPuppetEquipmentRef.transform;
				string assetKey = GetAssetKey(displayItem.equipmentType, asset.url);
				Transform transform2 = transform.Find(assetKey);
				if (transform2 == null)
				{
					continue;
				}
				GameObject gameObject = transform2.gameObject;
				if (!transform.TryGetComponent<SpriteRenderer>(out var component))
				{
					continue;
				}
				component.sortingOrder += newLayer + characterPuppetEquipmentRef.sortingLayer;
				SpriteRenderer componentInChildren = gameObject.GetComponentInChildren<SpriteRenderer>();
				if (componentInChildren != null)
				{
					if (component != null)
					{
						componentInChildren.sortingOrder = component.sortingOrder + 1;
					}
					Transform transform3 = componentInChildren.transform.parent.Find("remove");
					if (transform3 != null)
					{
						componentInChildren.sortingOrder = transform3.GetComponent<SpriteRenderer>().sortingOrder;
					}
					else
					{
						transform3 = componentInChildren.transform.parent.parent.Find("remove");
						if (transform3 != null)
						{
							componentInChildren.sortingOrder = transform3.GetComponent<SpriteRenderer>().sortingOrder;
						}
					}
				}
				SortingGroup component2 = gameObject.GetComponent<SortingGroup>();
				if (component2 != null && base.isActiveAndEnabled && component2.enabled)
				{
					component2.sortingOrder = componentInChildren.sortingOrder;
				}
			}
		}
	}

	public void OnTrigger(string tName)
	{
		SWFAsset componentInParent = GetComponentInParent<SWFAsset>();
		componentInParent.ANIMATION_TRIGGER.Invoke(componentInParent);
	}

	public void PlayAnimation(AnimationSequence seq)
	{
		sequenceName = seq.ToString();
		currentSequence = seq;
		if (!animator.enabled)
		{
			ToggleRecursivelyAnimatorStatus(enable: true);
		}
		animator.Play(sequenceName);
	}

	public void PlayAnimation(string seqStr)
	{
		sequenceName = seqStr;
		currentSequence = GetSequenceByName(seqStr);
		if (!animator.enabled)
		{
			ToggleRecursivelyAnimatorStatus(enable: true);
		}
		animator.Play(sequenceName);
	}

	public void StopAllAnimations()
	{
		ToggleRecursivelyAnimatorStatus(enable: false);
	}

	public void HideMaskedElements()
	{
		SpriteMask[] componentsInChildren = base.transform.GetComponentsInChildren<SpriteMask>(includeInactive: true);
		SpriteRenderer[] childRenderers = _childRenderers;
		foreach (SpriteRenderer spriteRenderer in childRenderers)
		{
			if (spriteRenderer.maskInteraction == SpriteMaskInteraction.VisibleInsideMask)
			{
				spriteRenderer.gameObject.SetActive(value: false);
			}
		}
		SpriteMask[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(value: false);
		}
	}

	private void ToggleRecursivelyAnimatorStatus(bool enable)
	{
		animator.enabled = enable;
		Animator[] components = GetComponents<Animator>();
		for (int i = 0; i < components.Length; i++)
		{
			components[i].enabled = enable;
		}
		Animator[] componentsInChildren = GetComponentsInChildren<Animator>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = enable;
		}
		ParticleSystem[] componentsInChildren2 = GetComponentsInChildren<ParticleSystem>();
		if (componentsInChildren2 != null)
		{
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				ParticleSystem.EmissionModule emission = componentsInChildren2[i].emission;
				emission.enabled = enable;
			}
		}
	}

	protected void SetEquipment(Equipment equipment, List<object> equipmentOverride = null)
	{
		if (_equipment != null)
		{
			_equipment.OnChange.RemoveListener(OnEquipmentChange);
			_equipment = null;
		}
		if (equipment != null)
		{
			_equipmentOverride = equipmentOverride;
			_equipment = equipment;
			_equipment.OnChange.AddListener(OnEquipmentChange);
			updateEquipment();
		}
	}

	private void updateEquipment()
	{
		setItems(_equipment.getDisplayItems());
	}

	protected string GetKeyForAsset(EquipmentAssetRef assetRef, int equipmentType)
	{
		string text = assetRef.bodyPart;
		switch (text)
		{
		case "back":
			text += "_equipment";
			break;
		case "rightHand":
		case "rightFoot":
		case "leftFoot":
		case "torso":
		case "head":
		case "leftHand":
			text = ((!assetRef.underSkin) ? ((equipmentType != 7) ? (text + "_equipment") : (text + "_overall")) : (text + "_underSkin"));
			break;
		}
		return text;
	}

	private void setItems(List<EquipmentRef> itemList)
	{
		if (_equipmentOverride != null)
		{
			foreach (EquipmentRef item in _equipmentOverride)
			{
				if (item == null)
				{
					continue;
				}
				bool flag = false;
				for (int i = 0; i < itemList.Count; i++)
				{
					EquipmentRef equipmentRef2 = itemList[i];
					if (!(equipmentRef2 == null) && item.equipmentType == equipmentRef2.equipmentType)
					{
						itemList[i] = item;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					itemList.Add(item);
				}
			}
		}
		_mainhandAsset = null;
		bool showHair = true;
		bool showHeadSkin = true;
		if (itemList == null || itemList.Count == 0)
		{
			return;
		}
		foreach (EquipmentRef item2 in itemList)
		{
			if (item2 == null || item2.assets == null || (!_showHelm && item2.equipmentType == 3) || (!_showBody && item2.equipmentType == 4) || (!_showAccessory && item2.equipmentType == 7))
			{
				continue;
			}
			float num = 1f;
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, int> item3 in equippedItemsPerType)
			{
				if (item3.Value == item2.equipmentType)
				{
					list.Add(item3.Key);
				}
			}
			foreach (string item4 in list)
			{
				UnityEngine.Object.Destroy(base.transform.Find(item4));
				equippedItemsPerType.Remove(item4);
			}
			list.Clear();
			foreach (EquipmentAssetRef asset in item2.assets)
			{
				string keyForAsset = GetKeyForAsset(asset, item2.equipmentType);
				if (!characterPuppetEquipmentReference.TryGetValue(keyForAsset, out var value))
				{
					continue;
				}
				if (characterPuppetEquipmentReference.TryGetValue(asset.bodyPart + "_skin", out var value2))
				{
					value2.transform.gameObject.SetActive(asset.showSkin);
				}
				Transform parent = value.transform;
				if ((asset.bodyPart.Equals("head") || item2.equipmentType == 3) && item2.equipmentType != 7)
				{
					showHair = asset.showHair;
					showHeadSkin = asset.showSkin;
				}
				GameObject gameObjectAsset = GameData.instance.main.assetLoader.GetGameObjectAsset(AssetURL.EQUIPMENT, asset.url, instantiate: false);
				if (gameObjectAsset == null)
				{
					continue;
				}
				string assetKey = GetAssetKey(item2.equipmentType, asset.url);
				if (!equippedItemsPerType.ContainsKey(assetKey))
				{
					equippedItemsPerType.Add(assetKey, item2.equipmentType);
				}
				GameObject gameObject = UnityEngine.Object.Instantiate(gameObjectAsset);
				gameObject.name = assetKey;
				gameObject.transform.parent = parent;
				if (item2.equipmentType == 1)
				{
					_mainhandAsset = gameObject.transform;
				}
				if (!AssetURL.IsPrefab(asset.url))
				{
					float x = asset.offset.x * value.offsetDivider + value.offset.x;
					float y = asset.offset.y * value.offsetDivider * -1f + value.offset.y;
					float z = value.offset.z;
					gameObject.transform.localPosition = new Vector3(x, y, z);
					gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
					gameObject.transform.localScale = Vector3.one * value.scale;
				}
				else
				{
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localRotation = Quaternion.identity;
				}
				num = base.transform.localScale.x;
				if (item2.equipmentType == 8)
				{
					ParticleSystem[] componentsInChildren = pet.GetComponentsInChildren<ParticleSystem>();
					foreach (ParticleSystem obj in componentsInChildren)
					{
						ParticleSystem.MainModule main = obj.main;
						main.startSize = new ParticleSystem.MinMaxCurve(main.startSize.constantMax * num, main.startSize.constantMax * num);
						main.gravityModifier = new ParticleSystem.MinMaxCurve(main.gravityModifier.constantMax * num, main.gravityModifier.constantMax * num);
						main.startSpeed = new ParticleSystem.MinMaxCurve(main.startSpeed.constantMax * num, main.startSpeed.constantMax * num);
						ParticleSystem.ShapeModule shape = obj.shape;
						Vector3 position = shape.position;
						position.x *= num;
						position.y *= num;
						position.z *= num;
						shape.position = position;
						Vector3 scale = shape.scale;
						scale.x *= num;
						scale.y *= num;
						scale.z *= num;
						shape.scale = scale;
						shape.radius *= num / 100f;
					}
				}
			}
		}
		ShowHair(showHair);
		ShowHeadSkin(showHeadSkin);
		if (_enableLoading)
		{
			SetAllActiveGameObjects(active: false);
			loading.SetActive(value: true);
			SortingGroup[] componentsInChildren2 = base.transform.GetComponentsInChildren<SortingGroup>(includeInactive: true);
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				componentsInChildren2[j].enabled = false;
			}
		}
		GameData.instance.main.coroutineTimer.AddTimer(base.gameObject, 400f, delegate
		{
			if (base.transform != null)
			{
				SortingGroup[] componentsInChildren3 = base.transform.GetComponentsInChildren<SortingGroup>(includeInactive: true);
				for (int k = 0; k < componentsInChildren3.Length; k++)
				{
					componentsInChildren3[k].enabled = true;
				}
				if (base.gameObject.activeSelf && base.isActiveAndEnabled)
				{
					StartCoroutine(DoEnableGameObjects());
				}
				else
				{
					SetAllActiveGameObjects(active: true);
					if (_enableLoading)
					{
						loading.SetActive(value: false);
					}
				}
			}
		});
	}

	private IEnumerator DoEnableGameObjects()
	{
		yield return new WaitForSeconds(0.1f);
		SetAllActiveGameObjects(active: true);
		if (_enableLoading)
		{
			loading.SetActive(value: false);
		}
		ON_ENABLED?.Invoke();
	}

	private string GetAssetKey(int type, string url)
	{
		return $"asset_{type}_{url}";
	}

	private void OnEquipmentChange()
	{
	}

	private void SetAllActiveGameObjects(bool active)
	{
		rightObject.gameObject.SetActive(active);
		leftObject.gameObject.SetActive(active);
		pet.gameObject.SetActive(active);
		cover.gameObject.SetActive(active);
		back.gameObject.SetActive(active);
		rightHand.gameObject.SetActive(active);
		leftHand.gameObject.SetActive(active);
		torso.gameObject.SetActive(active);
		head.gameObject.SetActive(active);
		leftFoot.gameObject.SetActive(active);
		if (!active || _showLeg)
		{
			rightFoot.gameObject.SetActive(active);
		}
	}

	public void ShowCharacter(bool show)
	{
		ShowCharacterRecursive(base.transform, show);
	}

	private void ShowCharacterRecursive(Transform tr, bool show)
	{
		Renderer component = tr.GetComponent<Renderer>();
		if (component != null)
		{
			component.enabled = show;
		}
		for (int i = 0; i < tr.childCount; i++)
		{
			ShowCharacterRecursive(tr.GetChild(i), show);
		}
	}

	private void ChangeLayer(string lName)
	{
		ChangeLayerRecursive(base.transform, lName);
	}

	private void ChangeLayerRecursive(Transform tr, string lName)
	{
		SpriteRenderer component = tr.GetComponent<SpriteRenderer>();
		if ((bool)component)
		{
			component.sortingLayerName = lName;
		}
		for (int i = 0; i < tr.childCount; i++)
		{
			ChangeLayerRecursive(tr.GetChild(i), lName);
		}
	}

	private AnimationSequence GetSequenceByName(string pname)
	{
		if (!Enum.TryParse<AnimationSequence>(pname, out var result))
		{
			return AnimationSequence.trigger;
		}
		return result;
	}

	public void ShowLoading(bool show)
	{
		loading.SetActive(show);
	}

	public static Gender ParseGenderFromString(string text)
	{
		return text.ToUpperInvariant() switch
		{
			"F" => Gender.FEMALE, 
			"N" => Gender.NEUTRAL, 
			_ => Gender.MALE, 
		};
	}
}
