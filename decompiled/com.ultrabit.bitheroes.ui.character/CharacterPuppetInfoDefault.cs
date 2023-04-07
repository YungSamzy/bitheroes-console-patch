using System.Collections.Generic;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.mount;

namespace com.ultrabit.bitheroes.ui.character;

public class CharacterPuppetInfoDefault : CharacterPuppetInfo
{
	private int _hairID;

	private int _hairColorID;

	private int _skinColorID;

	public int hairID => _hairID;

	public int hairColorID => _hairColorID;

	public int skinColorID => _skinColorID;

	public CharacterPuppetInfoDefault(CharacterPuppet.Gender genre = CharacterPuppet.Gender.MALE, int hairID = 17, int hairColorID = 11, int skinColorID = 6, float scale = 1f, float headScale = 1f, Equipment equipment = null, Mounts mounts = null, bool showHelm = true, bool showMount = false, bool showBody = true, bool showAccessory = true, List<object> equipmentOverride = null, bool enableLoading = true)
	{
		_gender = genre;
		_hairID = hairID;
		_hairColorID = hairColorID;
		_skinColorID = skinColorID;
		_scale = scale;
		_headScale = headScale;
		_equipment = equipment;
		_mounts = mounts;
		_showHelm = showHelm;
		_showMount = showMount;
		_showBody = showBody;
		_showAccessory = showAccessory;
		_equipmentOverride = equipmentOverride;
		_enableLoading = enableLoading;
	}

	public override object Clone()
	{
		return new CharacterPuppetInfoDefault(_gender, _hairID, _hairColorID, _skinColorID, _scale, _headScale, _equipment, _mounts, _showHelm, _showMount, _showBody, _showAccessory, _equipmentOverride, _enableLoading);
	}
}
