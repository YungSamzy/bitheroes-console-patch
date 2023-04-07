using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using com.ultrabit.bitheroes.ui.assets;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.equipment;

public class EquipmentAssetRef
{
	private EquipmentBookData.Asset _asset;

	public string bodyPart => _asset.bodyPart;

	public int order => Util.GetIntFromStringProperty(_asset.order);

	public bool underSkin => Util.GetBoolFromStringProperty(_asset.underSkin);

	public bool showHair => Util.GetBoolFromStringProperty(_asset.showHair, defaultValue: true);

	public bool showSkin => Util.GetBoolFromStringProperty(_asset.showSkin, defaultValue: true);

	public string url => _asset.url;

	public Vector2 offset => Util.GetVector2FromStringProperty(_asset.offset);

	public EquipmentAssetRef(EquipmentBookData.Asset asset)
	{
		_asset = asset;
	}

	public bool allowGender(string gender)
	{
		if (_asset.genders == null)
		{
			return true;
		}
		string[] stringArrayFromStringProperty = Util.GetStringArrayFromStringProperty(_asset.genders);
		for (int i = 0; i < stringArrayFromStringProperty.Length; i++)
		{
			if (stringArrayFromStringProperty[i].ToLowerInvariant() == gender.ToLowerInvariant())
			{
				return true;
			}
		}
		return false;
	}

	public Asset getAsset(bool center = false, float scale = 1f, bool offset = true)
	{
		return null;
	}
}
