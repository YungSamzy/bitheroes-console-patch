using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.instance;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.grid;
using com.ultrabit.bitheroes.ui.utility;
using DG.Tweening;
using Sfs2X.Entities.Data;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.instance;

public class InstancePlayer : InstanceObject
{
	[HideInInspector]
	public UnityCustomEvent BOBBER_COMPLETE = new UnityCustomEvent();

	public Transform bobberPrefab;

	private const int MOUNT_NAME_OFFSET = -50;

	public TextMeshPro nameTxt;

	private Vector2? _namePosition;

	private bool _isMe;

	private bool _flipped;

	private CharacterData _characterData;

	private InstanceFishingData _fishingData;

	private InstanceFishingBobber _bobber;

	public CharacterData characterData => _characterData;

	public bool isMe => _isMe;

	public bool flipped => _flipped;

	public InstanceFishingData fishingData => _fishingData;

	public InstanceFishingBobber bobber => _bobber;

	public void CreateInstancePlayer(Instance instance, Tile tile, CharacterData characterData)
	{
		_isMe = characterData.charID == GameData.instance.PROJECT.character.id;
		SetCharacterData(characterData, load: false);
		CreateInstanceObject(instance, 1, tile, 250f, !isMe, characterData);
		LoadCharacterData();
		UpdateMovementSpeedMult();
		UpdateAnimation(tween: false);
	}

	public void SetCharacterData(CharacterData characterData, bool load = true)
	{
		_characterData = characterData;
		SetData(characterData);
		if (load)
		{
			LoadCharacterData();
		}
	}

	public void SetFishingData(InstanceFishingData fishingData, bool load = true)
	{
		_fishingData = fishingData;
		if (load)
		{
			LoadAssets();
			LoadCharacterData();
		}
	}

	public void SetFlipped(bool flipped)
	{
		_flipped = flipped;
		UpdateRotation();
	}

	public int GetFishingDistanceMin()
	{
		return VariableBook.fishingDistanceMin;
	}

	public int GetFishingDistanceMax()
	{
		int fishingDistanceMax = VariableBook.fishingDistanceMax;
		if (_fishingData == null)
		{
			return fishingDistanceMax;
		}
		float num = 1f + GameModifier.getTypeTotal(_fishingData.getModifiers(), 53);
		return Mathf.RoundToInt((float)fishingDistanceMax * num);
	}

	public void UpdateScale()
	{
		SetScale((!_flipped) ? 1 : (-1));
	}

	public void UpdateRotation()
	{
		SetYRotation(_flipped ? 180 : 0);
	}

	public void UpdateAnimation(bool tween = true)
	{
		if (_fishingData == null)
		{
			ClearBobber();
			return;
		}
		switch (_fishingData.state)
		{
		case 1:
			ClearBobber();
			break;
		case 2:
			ClearBobber();
			if (tween)
			{
				PlayAnimation(CharacterPuppet.AnimationSequence.fishingCastingStart.ToString(), loop: true, CharacterPuppet.AnimationSequence.fishingCastingIdle.ToString());
			}
			else
			{
				PlayAnimation(CharacterPuppet.AnimationSequence.fishingCastingIdle.ToString());
			}
			break;
		case 3:
			if (tween)
			{
				OnBobberLoad();
				PlayAnimation(CharacterPuppet.AnimationSequence.fishingCast.ToString(), loop: true, CharacterPuppet.AnimationSequence.idle.ToString());
			}
			else
			{
				LoadBobber(tween: false, 0.5f);
				PlayAnimation(CharacterPuppet.AnimationSequence.idle.ToString());
			}
			break;
		case 4:
			if (tween)
			{
				PlayAnimation(CharacterPuppet.AnimationSequence.fishingCatchingStart.ToString(), loop: true, CharacterPuppet.AnimationSequence.fishingCatchingIdle.ToString());
			}
			else
			{
				PlayAnimation(CharacterPuppet.AnimationSequence.fishingCatchingIdle.ToString());
			}
			LoadBobber(tween, 1f, 4f);
			break;
		}
	}

	private void OnBobberLoad()
	{
		LoadBobber(tween: true, 0.5f);
	}

	public void ClearBobber()
	{
		if (_bobber != null)
		{
			Object.Destroy(_bobber.gameObject);
			_bobber = null;
		}
	}

	private void LoadBobber(bool tween = true, float slope = 1f, float slopeSpeed = 1f)
	{
		if (_bobber != null)
		{
			_bobber.SetSlopeTarget(slope, slopeSpeed, tween);
			return;
		}
		Vector2 vector = new Vector2(_fishingData.distance * 10, 0f);
		if (_flipped)
		{
			vector.x *= -1f;
		}
		Vector2 rodPosition = GetRodPosition();
		if (!tween)
		{
			if (_bobber == null)
			{
				Transform transform = Object.Instantiate(bobberPrefab);
				transform.SetParent(base.transform, worldPositionStays: false);
				_bobber = transform.GetComponent<InstanceFishingBobber>();
			}
			_bobber.CreateInstanceFishingBobber(base.grid as Instance, this);
			_bobber.localX = vector.x;
			_bobber.localY = vector.y;
		}
		else
		{
			Vector3 position = new Vector3(vector.x / 1.5f, rodPosition.y * 1.25f, 0f);
			List<Vector3> list = BezierLineRenderer.CalculateCurve(base.transform.TransformPoint(rodPosition), base.transform.TransformPoint(position), base.transform.TransformPoint(position), base.transform.TransformPoint(vector));
			if (_bobber == null)
			{
				Transform transform2 = Object.Instantiate(bobberPrefab);
				transform2.SetParent(base.transform, worldPositionStays: false);
				_bobber = transform2.GetComponent<InstanceFishingBobber>();
			}
			_bobber.CreateInstanceFishingBobber(base.grid as Instance, this, list);
			_bobber.localX = rodPosition.x;
			_bobber.localY = rodPosition.y;
			if (isMe)
			{
				base.grid.SetFocus(_bobber);
			}
		}
		_bobber.SetSlopeTarget(slope, slopeSpeed, tween);
	}

	public Vector2 GetRodPosition()
	{
		CharacterDisplay component = base.asset.GetComponent<CharacterDisplay>();
		Transform mainhandAsset = component.mainhandAsset;
		EquipmentRef equipmentRef = ((_fishingData != null) ? _fishingData.rodRef : null);
		if (mainhandAsset != null && equipmentRef != null)
		{
			Vector2 vector = new Vector2(equipmentRef.projectileOffset.x, 0f - equipmentRef.projectileOffset.y);
			Transform transform = null;
			if (equipmentRef.projectileCenter)
			{
				if (equipmentRef.assets[0].url.IndexOf(".swf") > -1)
				{
					transform = Util.RecursiveFindChild(mainhandAsset, "staff");
				}
				if (transform == null)
				{
					if (!component.hasMountEquipped())
					{
						vector.x += mainhandAsset.GetComponentInChildren<SpriteRenderer>().bounds.size.x * 100f - 1000f;
						vector.y -= mainhandAsset.GetComponentInChildren<SpriteRenderer>().bounds.size.y * 100f - 6000f;
					}
					else
					{
						vector.x += mainhandAsset.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 100f + 12f;
						vector.y -= mainhandAsset.GetComponentInChildren<SpriteRenderer>().bounds.size.y / 100f + 8f;
					}
				}
				else if (!component.hasMountEquipped())
				{
					vector.x += transform.GetComponentInChildren<SpriteRenderer>().bounds.size.x - 20f;
					vector.y += transform.GetComponentInChildren<SpriteRenderer>().bounds.size.y - 60f;
				}
				else
				{
					vector.x = vector.x / 100f + 0.2f;
					vector.y = vector.y / 100f - 0.2f;
				}
			}
			GameObject gameObject = new GameObject();
			gameObject.transform.SetParent(mainhandAsset, worldPositionStays: false);
			Vector3 localScale = component.characterPuppet.transform.localScale;
			gameObject.transform.localPosition = new Vector3(vector.x / localScale.x, vector.y / localScale.y, 0f);
			Vector3 vector2 = base.transform.InverseTransformPoint(gameObject.transform.position);
			Object.Destroy(gameObject);
			return vector2;
		}
		return new Vector2(0f, 0f);
	}

	public void ShowItemObtained(ItemRef itemRef)
	{
		if (!(itemRef == null))
		{
			GameObject obj = new GameObject();
			obj.transform.SetParent(base.transform, worldPositionStays: false);
			obj.transform.localPosition = Vector3.zero;
			obj.AddComponent<SpriteRenderer>().sprite = GameData.instance.main.assetLoader.GetSpriteAsset(AssetURL.GetAssetType(itemRef, icon: true), itemRef.icon);
		}
	}

	private void OnItemObtainedLoaded(GameObject asset)
	{
		TweenItemObtainedAsset(asset);
	}

	private void TweenItemObtainedAsset(GameObject asset)
	{
		float num = 3f;
		float distance = 30f;
		float fadeOut = num / 3f;
		if (characterData.mounts.getMountEquipped() != null)
		{
			_ = characterData.showMount;
		}
		else
			_ = 0;
		((RectTransform)asset.transform).anchoredPosition = new Vector2(asset.transform.position.x, GetNameYPosition() + 15f);
		asset.transform.localScale = Vector3.Scale(asset.transform.localScale, new Vector3(6f, 6f, 1f));
		ObtainedItemAnimation(asset, num, distance, fadeOut);
	}

	private IEnumerator AlphaTween(GameObject asset, float duration, float fadeOut)
	{
		yield return new WaitForSeconds(duration - fadeOut);
		SpriteRenderer component = asset.GetComponent<SpriteRenderer>();
		Color endValue = new Color(component.color.r, component.color.g, component.color.b, 0f);
		component.DOColor(endValue, fadeOut).SetEase(Ease.OutBack);
	}

	private void ObtainedItemAnimation(GameObject asset, float duration, float distance, float fadeOut)
	{
		StartCoroutine(AlphaTween(asset, duration, fadeOut));
		asset.transform.DOScale(new Vector3(Instance.OBJECT_SCALE, Instance.OBJECT_SCALE, 1f), 0.5f).SetEase(Ease.OutBack);
		asset.transform.DOMove(new Vector3(asset.transform.position.x, asset.transform.position.y + distance, asset.transform.position.z), duration).SetEase(Ease.Linear).OnComplete(delegate
		{
			RemoveItemObtainedAsset(asset);
		});
	}

	private void RemoveItemObtainedAsset(GameObject asset)
	{
		if (asset != null)
		{
			Object.Destroy(asset);
		}
	}

	public void LoadCharacterData()
	{
		if (_characterData != null)
		{
			nameTxt.gameObject.SetActive(value: true);
			if (!_namePosition.HasValue)
			{
				_namePosition = new Vector2(nameTxt.rectTransform.anchoredPosition.x, nameTxt.rectTransform.anchoredPosition.y);
			}
			if (!isMe)
			{
				if (characterData.mounts.getMountEquipped() != null)
				{
					_ = characterData.showMount;
				}
				else
					_ = 0;
				nameTxt.rectTransform.anchoredPosition = new Vector2(nameTxt.rectTransform.anchoredPosition.x, GetNameYPosition());
				nameTxt.text = characterData.nameplateWithGuild;
			}
			else
			{
				nameTxt.gameObject.SetActive(value: false);
			}
		}
	}

	private float GetNameYPosition()
	{
		if (characterData.mounts.getMountEquipped() != null && characterData.showMount)
		{
			return _namePosition.Value.y - -50f;
		}
		return _namePosition.Value.y;
	}

	public override void SetPath(List<Tile> path)
	{
		UpdateMovementSpeedMult();
		base.SetPath(path);
	}

	public void UpdateMovementSpeedMult()
	{
		if (_characterData.charID == GameData.instance.PROJECT.character.id)
		{
			float num = 1f + GameData.instance.PROJECT.character.getGameModifierValueTotal(9);
			SetSpeedMult(num);
		}
	}

	public override void OnDestroy()
	{
		RemoveItemObtainedAsset(null);
		base.OnDestroy();
	}

	private void Start()
	{
		if (_characterData.charID == GameData.instance.PROJECT.character.id)
		{
			BoxCollider2D[] componentsInChildren = GetComponentsInChildren<BoxCollider2D>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
		}
	}

	public static InstancePlayer FromSFSObject(ISFSObject sfsob, Instance instance)
	{
		CharacterData characterData = CharacterData.fromSFSObject(sfsob);
		InstanceFishingData instanceFishingData = InstanceFishingData.fromSFSObject(sfsob);
		int @int = sfsob.GetInt("ins2");
		bool @bool = sfsob.GetBool("ins5");
		InstancePlayer instancePlayer = Object.Instantiate(Instance.instance.instancePlayerPrefab);
		instancePlayer.CreateInstancePlayer(instance, instance.getTileByID(@int), characterData);
		instancePlayer.SetFlipped(@bool);
		instancePlayer.SetFishingData(instanceFishingData);
		instancePlayer.UpdateAnimation(tween: false);
		return instancePlayer;
	}
}
