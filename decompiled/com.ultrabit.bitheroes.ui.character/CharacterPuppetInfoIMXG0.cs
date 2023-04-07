using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.mount;
using com.ultrabit.bitheroes.model.utility;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.character;

public class CharacterPuppetInfoIMXG0 : CharacterPuppetInfo
{
	private Character.IMXG0Data.Puppet _puppetInfo;

	private Character.IMXG0Data.Card _cardInfo;

	private string _rarity;

	private string _name;

	public Character.IMXG0Data.Puppet puppetInfo => _puppetInfo;

	public Character.IMXG0Data.Card cardInfo => _cardInfo;

	public string rarity => _rarity;

	public string name => _name;

	public CharacterPuppetInfoIMXG0(Character.IMXG0Data.Puppet puppetInfo, Character.IMXG0Data.Card cardInfo, string rarity, string name, CharacterPuppet.Gender gender = CharacterPuppet.Gender.MALE, float scale = 1f, float headScale = 1f, Equipment equipment = null, Mounts mounts = null, bool showHelm = true, bool showMount = false, bool showBody = true, bool showAccessory = true, List<object> equipmentOverride = null, bool enableLoading = true)
	{
		_puppetInfo = puppetInfo;
		_cardInfo = cardInfo;
		_rarity = rarity;
		_name = name;
		_gender = gender;
		_scale = scale;
		_headScale = headScale;
		_showHelm = showHelm;
		_showMount = showMount;
		_showBody = showBody;
		_showAccessory = showAccessory;
		_equipment = equipment;
		_mounts = mounts;
		_equipmentOverride = equipmentOverride;
		_enableLoading = enableLoading;
	}

	public void GetSkinAssets(out GameObject rightHand, out GameObject rightFoot, out GameObject leftFoot, out GameObject torso, out GameObject torsoBra, out GameObject head, out GameObject eyelids, out GameObject leftHand)
	{
		rightHand = (rightFoot = (leftFoot = (torso = (torsoBra = (head = (eyelids = (leftHand = null)))))));
		if (!string.IsNullOrEmpty(puppetInfo.skin))
		{
			rightHand = GameData.instance.main.assetLoader.GetGameObjectPrefabAsset(AssetURL.IMX_SKIN, puppetInfo.skin + "_rightHand", instantiate: false);
			rightFoot = GameData.instance.main.assetLoader.GetGameObjectPrefabAsset(AssetURL.IMX_SKIN, puppetInfo.skin + "_rightFoot", instantiate: false);
			leftFoot = GameData.instance.main.assetLoader.GetGameObjectPrefabAsset(AssetURL.IMX_SKIN, puppetInfo.skin + "_leftFoot", instantiate: false);
			torso = GameData.instance.main.assetLoader.GetGameObjectPrefabAsset(AssetURL.IMX_SKIN, puppetInfo.skin + "_torso", instantiate: false);
			head = GameData.instance.main.assetLoader.GetGameObjectPrefabAsset(AssetURL.IMX_SKIN, puppetInfo.skin + "_head", instantiate: false);
			eyelids = GameData.instance.main.assetLoader.GetGameObjectPrefabAsset(AssetURL.IMX_SKIN, puppetInfo.skin + "_eyelids", instantiate: false);
			leftHand = GameData.instance.main.assetLoader.GetGameObjectPrefabAsset(AssetURL.IMX_SKIN, puppetInfo.skin + "_leftHand", instantiate: false);
			LogErrorIfMissingAsset(rightHand, "rightHand", puppetInfo.skin, "GetSkinAssets");
			LogErrorIfMissingAsset(rightFoot, "rightFoot", puppetInfo.skin, "GetSkinAssets");
			LogErrorIfMissingAsset(leftFoot, "leftFoot", puppetInfo.skin, "GetSkinAssets");
			LogErrorIfMissingAsset(torso, "torso", puppetInfo.skin, "GetSkinAssets");
			LogErrorIfMissingAsset(head, "head", puppetInfo.skin, "GetSkinAssets");
			LogErrorIfMissingAsset(eyelids, "eyelids", puppetInfo.skin, "GetSkinAssets");
			LogErrorIfMissingAsset(leftHand, "leftHand", puppetInfo.skin, "GetSkinAssets");
		}
	}

	public void GetOutfitAssets(out GameObject back, out GameObject rightHand, out GameObject rightFoot, out GameObject leftFoot, out GameObject torso, out GameObject head, out GameObject leftHand)
	{
		if (string.IsNullOrEmpty(puppetInfo.outfit))
		{
			back = (rightHand = (rightFoot = (leftFoot = (torso = (head = (leftHand = null))))));
			return;
		}
		back = GameData.instance.main.assetLoader.GetGameObjectPrefabAsset(AssetURL.IMX_OUTFIT, puppetInfo.outfit + "_back", instantiate: false);
		rightHand = GameData.instance.main.assetLoader.GetGameObjectPrefabAsset(AssetURL.IMX_OUTFIT, puppetInfo.outfit + "_rightHand", instantiate: false);
		rightFoot = GameData.instance.main.assetLoader.GetGameObjectPrefabAsset(AssetURL.IMX_OUTFIT, puppetInfo.outfit + "_rightFoot", instantiate: false);
		leftFoot = GameData.instance.main.assetLoader.GetGameObjectPrefabAsset(AssetURL.IMX_OUTFIT, puppetInfo.outfit + "_leftFoot", instantiate: false);
		torso = GameData.instance.main.assetLoader.GetGameObjectPrefabAsset(AssetURL.IMX_OUTFIT, puppetInfo.outfit + "_torso", instantiate: false);
		head = GameData.instance.main.assetLoader.GetGameObjectPrefabAsset(AssetURL.IMX_OUTFIT, puppetInfo.outfit + "_head", instantiate: false);
		leftHand = GameData.instance.main.assetLoader.GetGameObjectPrefabAsset(AssetURL.IMX_OUTFIT, puppetInfo.outfit + "_leftHand", instantiate: false);
		LogErrorIfMissingAllAssets(new GameObject[7] { back, rightHand, rightFoot, leftFoot, torso, head, leftHand }, "outfit", puppetInfo.outfit, "GetOutfitAssets");
	}

	public void GetEyeAssets(out GameObject eyes, out GameObject eyesFlipped)
	{
		if (string.IsNullOrEmpty(puppetInfo.eyes))
		{
			eyes = (eyesFlipped = null);
			return;
		}
		eyes = GameData.instance.main.assetLoader.GetGameObjectPrefabAsset(AssetURL.IMX_EYES, puppetInfo.eyes, instantiate: false);
		eyesFlipped = GameData.instance.main.assetLoader.GetGameObjectPrefabAsset(AssetURL.IMX_EYES, puppetInfo.eyes + "_flipped", instantiate: false);
		LogErrorIfMissingAsset(eyes, "eyes", puppetInfo.eyes, "GetEyeAssets");
		LogErrorIfMissingAsset(eyesFlipped, "eyesFlipped", puppetInfo.eyes, "GetEyeAssets");
	}

	public void GetMaskAsset(out GameObject mask)
	{
		if (string.IsNullOrEmpty(puppetInfo.mask))
		{
			mask = null;
			return;
		}
		mask = GameData.instance.main.assetLoader.GetGameObjectPrefabAsset(AssetURL.IMX_MASK, puppetInfo.mask, instantiate: false);
		LogErrorIfMissingAsset(mask, "mask", puppetInfo.mask, "GetMaskAsset");
	}

	public void GetHairAssets(out GameObject hairBack, out GameObject hairFront)
	{
		if (string.IsNullOrEmpty(puppetInfo.hair))
		{
			hairBack = (hairFront = null);
			return;
		}
		hairBack = GameData.instance.main.assetLoader.GetGameObjectPrefabAsset(AssetURL.IMX_HAIR, puppetInfo.hair + "_back", instantiate: false);
		hairFront = GameData.instance.main.assetLoader.GetGameObjectPrefabAsset(AssetURL.IMX_HAIR, puppetInfo.hair, instantiate: false);
		LogErrorIfMissingAllAssets(new GameObject[2] { hairBack, hairFront }, "hair", puppetInfo.hair, "GetHairAssets");
	}

	public void GetHatAssets(out GameObject hatBack, out GameObject hatFront)
	{
		if (string.IsNullOrEmpty(puppetInfo.hat))
		{
			hatBack = (hatFront = null);
			return;
		}
		hatBack = GameData.instance.main.assetLoader.GetGameObjectPrefabAsset(AssetURL.IMX_HAT, puppetInfo.hat + "_back", instantiate: false);
		hatFront = GameData.instance.main.assetLoader.GetGameObjectPrefabAsset(AssetURL.IMX_HAT, puppetInfo.hat, instantiate: false);
		LogErrorIfMissingAllAssets(new GameObject[2] { hatBack, hatFront }, "hat", puppetInfo.hat, "GetHatAssets");
	}

	public void GetHaloAsset(out GameObject halo)
	{
		if (string.IsNullOrEmpty(puppetInfo.halo))
		{
			halo = null;
			return;
		}
		halo = GameData.instance.main.assetLoader.GetGameObjectPrefabAsset(AssetURL.IMX_HALO, puppetInfo.halo, instantiate: false);
		LogErrorIfMissingAsset(halo, "halo", puppetInfo.halo, "GetHaloAsset");
	}

	public void GetHornAsset(out GameObject horn)
	{
		if (string.IsNullOrEmpty(puppetInfo.horn))
		{
			horn = null;
			return;
		}
		horn = GameData.instance.main.assetLoader.GetGameObjectPrefabAsset(AssetURL.IMX_HORN, puppetInfo.horn, instantiate: false);
		LogErrorIfMissingAsset(horn, "horn", puppetInfo.horn, "GetHornAsset");
	}

	public void GetBackgroundAsset(out GameObject background)
	{
		if (string.IsNullOrEmpty(cardInfo.background))
		{
			background = null;
			return;
		}
		background = GameData.instance.main.assetLoader.GetGameObjectPrefabAsset(AssetURL.IMX_BACKGROUND, cardInfo.background, instantiate: false);
		LogErrorIfMissingAsset(background, "background", cardInfo.background, "GetBackgroundAsset");
	}

	public void GetFrameAsset(out GameObject frame)
	{
		if (string.IsNullOrEmpty(cardInfo.frame))
		{
			frame = null;
			return;
		}
		frame = GameData.instance.main.assetLoader.GetGameObjectPrefabAsset(AssetURL.IMX_FRAME, cardInfo.frame, instantiate: false);
		LogErrorIfMissingAsset(frame, "frame", cardInfo.frame, "GetFrameAsset");
	}

	public void GetFrameSimpleAsset(out GameObject frame)
	{
		if (string.IsNullOrEmpty(cardInfo.frame))
		{
			frame = null;
			return;
		}
		frame = GameData.instance.main.assetLoader.GetGameObjectPrefabAsset(AssetURL.IMX_FRAME, cardInfo.frame + "_simple", instantiate: false);
		if (frame == null)
		{
			GetFrameAsset(out frame);
		}
		LogErrorIfMissingAsset(frame, "frame", cardInfo.frame, "GetFrameAsset");
	}

	public void GetFrameSeparatorAsset(out GameObject frameSeparator)
	{
		if (string.IsNullOrEmpty(cardInfo.frame))
		{
			frameSeparator = null;
			return;
		}
		frameSeparator = GameData.instance.main.assetLoader.GetGameObjectPrefabAsset(AssetURL.IMX_FRAME, cardInfo.frame + "_separator", instantiate: false);
		LogErrorIfMissingAsset(frameSeparator, "frameSeparator", cardInfo.frame, "GetFrameSeparatorAsset");
	}

	public void GetFrameMenuInterfaceAsset(out GameObject frameMenuInterface)
	{
		if (string.IsNullOrEmpty(cardInfo.frame))
		{
			frameMenuInterface = null;
			return;
		}
		frameMenuInterface = GameData.instance.main.assetLoader.GetGameObjectPrefabAsset(AssetURL.IMX_FRAME, cardInfo.frame + "_menuInterface", instantiate: false);
		LogErrorIfMissingAsset(frameMenuInterface, "frameMenuInterface", cardInfo.frame, "GetFrameMenuInterfaceAsset");
	}

	public void LogErrorIfMissingAsset(GameObject asset, string assetPartName, string assetName, string methodName)
	{
		if (!(asset != null))
		{
			D.LogError($"{GetType()}.{methodName}() :: No asset for {assetPartName} \"{assetName}\" has been found.");
		}
	}

	public void LogErrorIfMissingAllAssets(GameObject[] assets, string assetPartName, string assetName, string methodName)
	{
		for (int i = 0; i < assets.Length; i++)
		{
			if (assets[i] != null)
			{
				return;
			}
		}
		D.LogError($"{GetType()}.{methodName}() :: No assets for {assetPartName} \"{assetName}\" have been found.");
	}

	public override object Clone()
	{
		return new CharacterPuppetInfoIMXG0(_puppetInfo, _cardInfo, _rarity, _name, _gender, _scale, _headScale, _equipment, _mounts, _showHelm, _showMount, _showBody, _showAccessory, _equipmentOverride, _enableLoading);
	}
}
