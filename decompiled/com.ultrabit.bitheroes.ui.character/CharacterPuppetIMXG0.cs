using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace com.ultrabit.bitheroes.ui.character;

public class CharacterPuppetIMXG0 : CharacterPuppet
{
	public const string SUBPART_HAT_BACK = "hatBack";

	public const string SUBPART_HAIR_BACK = "hairBack";

	public const string SUBPART_OUTFIT = "outfit";

	public const string SUBPART_EYES = "eyes";

	public const string SUBPART_EYES_FLIPPED = "eyesFlipped";

	public const string SUBPART_EYELASHES = "eyelashes";

	public const string SUBPART_EYELIDS = "eyelids";

	public const string SUBPART_MASK = "mask";

	public const string SUBPART_HAT = "hat";

	public const string SUBPART_HALO = "halo";

	public const string SUBPART_HORN = "horn";

	public const string SUBPART_BRA = "bra";

	public static string BP_BACK_OUTFIT = "back_outfit";

	public static string BP_RIGHT_HAND_OUTFIT = "rightHand_outfit";

	public static string BP_RIGHT_FOOT_OUTFIT = "rightFoot_outfit";

	public static string BP_LEFT_FOOT_OUTFIT = "leftFoot_outfit";

	public static string BP_TORSO_BRA = "torso_bra";

	public static string BP_TORSO_OUTFIT = "torso_outfit";

	public static string BP_HEAD_HAT_BACK = "head_hatBack";

	public static string BP_HEAD_HAIR_BACK = "head_hairBack";

	public static string BP_HEAD_OUTFIT = "head_outfit";

	public static string BP_HEAD_EYES = "head_eyes";

	public static string BP_HEAD_EYES_FLIPPED = "head_eyesFlipped";

	public static string BP_HEAD_EYELASHES = "head_eyelashes";

	public static string BP_HEAD_EYELIDS = "head_eyelids";

	public static string BP_HEAD_MASK = "head_mask";

	public static string BP_HEAD_HAT = "head_hat";

	public static string BP_HEAD_HALO = "head_halo";

	public static string BP_HEAD_HORN = "head_horn";

	public static string BP_LEFT_HAND_OUTFIT = "leftHand_outfit";

	[Header("Character Puppet IMXG0 References")]
	[Space(15f)]
	[SerializeField]
	private SortingGroup backOutfit;

	[Space(15f)]
	[SerializeField]
	private SortingGroup rightHandOutfit;

	[Space(15f)]
	[SerializeField]
	private SortingGroup rightFootOutfit;

	[Space(15f)]
	[SerializeField]
	private SortingGroup leftFootOutfit;

	[Space(15f)]
	[SerializeField]
	private SortingGroup torsoBra;

	[SerializeField]
	private SortingGroup torsoOutfit;

	[Space(15f)]
	[SerializeField]
	private SortingGroup headHatBack;

	[SerializeField]
	private SortingGroup headHairBack;

	[SerializeField]
	private SpriteRenderer headBlush;

	[SerializeField]
	private SortingGroup headOutfit;

	[SerializeField]
	private SortingGroup headEyes;

	[SerializeField]
	private SortingGroup headEyesFlipped;

	[SerializeField]
	private SortingGroup headEyelids;

	[SerializeField]
	private SortingGroup headMouth;

	[SerializeField]
	private SortingGroup headMask;

	[SerializeField]
	private SortingGroup headHat;

	[SerializeField]
	private SortingGroup headHalo;

	[SerializeField]
	private SortingGroup headHorn;

	[Space(15f)]
	[SerializeField]
	private SortingGroup leftHandOutfit;

	[Space(15f)]
	[SerializeField]
	private Animator headAnimator;

	private GameObject[] hairGroup;

	private GameObject[] headSkinGroup;

	private GameObject[] outfitGroup;

	private CharacterPuppetInfoIMXG0 _puppetInfoIMXG0;

	protected override void Awake()
	{
		base.Awake();
		characterPuppetEquipmentReference.Add(BP_BACK_OUTFIT, new CharacterPuppetEquipmentRef(backOutfit, Vector3.zero));
		characterPuppetEquipmentReference.Add(BP_RIGHT_HAND_OUTFIT, new CharacterPuppetEquipmentRef(rightHandOutfit, Vector3.zero));
		characterPuppetEquipmentReference.Add(BP_RIGHT_FOOT_OUTFIT, new CharacterPuppetEquipmentRef(rightFootOutfit, Vector3.zero));
		characterPuppetEquipmentReference.Add(BP_LEFT_FOOT_OUTFIT, new CharacterPuppetEquipmentRef(leftFootOutfit, Vector3.zero));
		characterPuppetEquipmentReference.Add(BP_TORSO_OUTFIT, new CharacterPuppetEquipmentRef(torsoOutfit, Vector3.zero));
		characterPuppetEquipmentReference.Add(BP_HEAD_HAT_BACK, new CharacterPuppetEquipmentRef(headHatBack, Vector3.zero));
		characterPuppetEquipmentReference.Add(BP_HEAD_HAIR_BACK, new CharacterPuppetEquipmentRef(headHairBack, Vector3.zero));
		characterPuppetEquipmentReference.Add(BP_HEAD_OUTFIT, new CharacterPuppetEquipmentRef(headOutfit, Vector3.zero));
		characterPuppetEquipmentReference.Add(BP_HEAD_EYES, new CharacterPuppetEquipmentRef(headEyes, Vector3.zero));
		characterPuppetEquipmentReference.Add(BP_HEAD_EYES_FLIPPED, new CharacterPuppetEquipmentRef(headEyesFlipped, Vector3.zero));
		characterPuppetEquipmentReference.Add(BP_HEAD_EYELIDS, new CharacterPuppetEquipmentRef(headEyelids, Vector3.zero));
		characterPuppetEquipmentReference.Add(BP_HEAD_MASK, new CharacterPuppetEquipmentRef(headMask, Vector3.zero));
		characterPuppetEquipmentReference.Add(BP_HEAD_HAT, new CharacterPuppetEquipmentRef(headHat, Vector3.zero));
		characterPuppetEquipmentReference.Add(BP_HEAD_HALO, new CharacterPuppetEquipmentRef(headHalo, Vector3.zero));
		characterPuppetEquipmentReference.Add(BP_HEAD_HORN, new CharacterPuppetEquipmentRef(headHorn, Vector3.zero));
		characterPuppetEquipmentReference.Add(BP_LEFT_HAND_OUTFIT, new CharacterPuppetEquipmentRef(leftHandOutfit, Vector3.zero));
		hairGroup = new GameObject[5] { headHatBack.gameObject, headHairBack.gameObject, headHat.gameObject, headHair.gameObject, headHorn.gameObject };
		headSkinGroup = new GameObject[6] { headSkin.gameObject, headOutfit.gameObject, headEyes.gameObject, headEyesFlipped.gameObject, headEyelids.gameObject, headMouth.gameObject };
		outfitGroup = new GameObject[7] { backOutfit.gameObject, rightHandOutfit.gameObject, rightFootOutfit.gameObject, leftFootOutfit.gameObject, torsoOutfit.gameObject, headOutfit.gameObject, leftHandOutfit.gameObject };
	}

	protected override void Start()
	{
		base.Start();
	}

	public override void ShowHair(bool show)
	{
		GameObject[] array = hairGroup;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(show);
		}
	}

	public override void ShowHeadSkin(bool show)
	{
		GameObject[] array = headSkinGroup;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(show);
		}
		headAnimator.enabled = show;
	}

	public void ShowOutfit(bool show)
	{
		GameObject[] array = outfitGroup;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(show);
		}
	}

	public override void SetCharacterPuppet(CharacterPuppetInfo puppetInfo)
	{
		if (!(puppetInfo is CharacterPuppetInfoIMXG0))
		{
			throw new Exception(string.Format("{0}.{1}() :: Trying to set with {2}.", GetType(), "SetCharacterPuppet", puppetInfo.GetType()));
		}
		_puppetInfoIMXG0 = (CharacterPuppetInfoIMXG0)puppetInfo;
		gender = _puppetInfoIMXG0.gender;
		_headScale = _puppetInfoIMXG0.headScale;
		_scale = _puppetInfoIMXG0.scale;
		_showHelm = _puppetInfoIMXG0.showHelm;
		_showMount = _puppetInfoIMXG0.showMount;
		_showBody = _puppetInfoIMXG0.showBody;
		_showAccessory = _puppetInfoIMXG0.showAccessory;
		_enableLoading = _puppetInfoIMXG0.enableLoading;
		InstantiateAssets();
		headBlush.gameObject.SetActive(value: false);
		Vector3 localScale = base.transform.localScale;
		localScale *= _scale;
		base.transform.localScale = localScale;
		SetEquipment(_puppetInfoIMXG0.equipment, _puppetInfoIMXG0.equipmentOverride);
		_childRenderers = base.transform.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
	}

	private void InstantiateAssets()
	{
		_puppetInfoIMXG0.GetSkinAssets(out var gameObject, out var gameObject2, out var gameObject3, out var gameObject4, out var _, out var gameObject6, out var eyelids, out var gameObject7);
		if (gameObject != null)
		{
			UnityEngine.Object.Instantiate(gameObject, rightHandSkin.transform);
		}
		if (gameObject2 != null)
		{
			UnityEngine.Object.Instantiate(gameObject2, rightFootSkin.transform);
		}
		if (gameObject3 != null)
		{
			UnityEngine.Object.Instantiate(gameObject3, leftFootSkin.transform);
		}
		if (gameObject4 != null)
		{
			UnityEngine.Object.Instantiate(gameObject4, torsoSkin.transform);
		}
		if (gameObject6 != null)
		{
			UnityEngine.Object.Instantiate(gameObject6, headSkin.transform);
		}
		if (eyelids != null)
		{
			UnityEngine.Object.Instantiate(eyelids, headEyelids.transform);
		}
		if (gameObject7 != null)
		{
			UnityEngine.Object.Instantiate(gameObject7, leftHandSkin.transform);
		}
		_puppetInfoIMXG0.GetOutfitAssets(out var gameObject8, out var gameObject9, out var gameObject10, out var gameObject11, out var gameObject12, out var gameObject13, out var gameObject14);
		if (gameObject8 != null)
		{
			UnityEngine.Object.Instantiate(gameObject8, backOutfit.transform);
		}
		if (gameObject9 != null)
		{
			UnityEngine.Object.Instantiate(gameObject9, rightHandOutfit.transform);
		}
		if (gameObject10 != null)
		{
			UnityEngine.Object.Instantiate(gameObject10, rightFootOutfit.transform);
		}
		if (gameObject11 != null)
		{
			UnityEngine.Object.Instantiate(gameObject11, leftFootOutfit.transform);
		}
		if (gameObject12 != null)
		{
			UnityEngine.Object.Instantiate(gameObject12, torsoOutfit.transform);
		}
		if (gameObject13 != null)
		{
			UnityEngine.Object.Instantiate(gameObject13, headOutfit.transform);
		}
		if (gameObject14 != null)
		{
			UnityEngine.Object.Instantiate(gameObject14, leftHandOutfit.transform);
		}
		_puppetInfoIMXG0.GetMaskAsset(out var mask);
		if (mask != null)
		{
			UnityEngine.Object.Instantiate(mask, headMask.transform);
		}
		_puppetInfoIMXG0.GetEyeAssets(out var eyes, out var eyesFlipped);
		if (eyes != null)
		{
			UnityEngine.Object.Instantiate(eyes, headEyes.transform);
		}
		if (eyesFlipped != null)
		{
			UnityEngine.Object.Instantiate(eyesFlipped, headEyesFlipped.transform);
		}
		_puppetInfoIMXG0.GetHairAssets(out var hairBack, out var hairFront);
		if (hairBack != null)
		{
			UnityEngine.Object.Instantiate(hairBack, headHairBack.transform);
		}
		if (hairFront != null)
		{
			UnityEngine.Object.Instantiate(hairFront, headHair.transform);
		}
		_puppetInfoIMXG0.GetHatAssets(out var hatBack, out var hatFront);
		if (hatBack != null)
		{
			UnityEngine.Object.Instantiate(hatBack, headHatBack.transform);
		}
		if (hatFront != null)
		{
			UnityEngine.Object.Instantiate(hatFront, headHat.transform);
		}
		_puppetInfoIMXG0.GetHaloAsset(out var halo);
		if (halo != null)
		{
			UnityEngine.Object.Instantiate(halo, headHalo.transform);
		}
		_puppetInfoIMXG0.GetHornAsset(out var horn);
		if (horn != null)
		{
			UnityEngine.Object.Instantiate(horn, headHorn.transform);
		}
	}
}
