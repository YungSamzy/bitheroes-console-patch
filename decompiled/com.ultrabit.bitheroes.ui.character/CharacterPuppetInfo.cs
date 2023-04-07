using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.mount;

namespace com.ultrabit.bitheroes.ui.character;

public abstract class CharacterPuppetInfo : ICloneable
{
	protected CharacterPuppet.Gender _gender;

	protected float _scale = 1f;

	protected float _headScale = 1f;

	protected Equipment _equipment;

	protected List<object> _equipmentOverride;

	protected bool _showHelm = true;

	protected bool _showBody = true;

	protected bool _showAccessory = true;

	protected Mounts _mounts;

	protected bool _showMount;

	protected bool _enableLoading = true;

	public CharacterPuppet.Gender gender => _gender;

	public float scale
	{
		get
		{
			return _scale;
		}
		set
		{
			_scale = value;
		}
	}

	public float headScale => _headScale;

	public Equipment equipment
	{
		get
		{
			return _equipment;
		}
		set
		{
			_equipment = value;
		}
	}

	public List<object> equipmentOverride
	{
		get
		{
			return _equipmentOverride;
		}
		set
		{
			_equipmentOverride = value;
		}
	}

	public bool showHelm
	{
		get
		{
			return _showHelm;
		}
		set
		{
			_showHelm = value;
		}
	}

	public bool showBody
	{
		get
		{
			return _showBody;
		}
		set
		{
			_showBody = value;
		}
	}

	public bool showAccessory
	{
		get
		{
			return _showAccessory;
		}
		set
		{
			_showAccessory = value;
		}
	}

	public Mounts mounts
	{
		get
		{
			return _mounts;
		}
		set
		{
			_mounts = value;
		}
	}

	public bool showMount
	{
		get
		{
			return _showMount;
		}
		set
		{
			_showMount = value;
		}
	}

	public bool enableLoading
	{
		get
		{
			return _enableLoading;
		}
		set
		{
			_enableLoading = value;
		}
	}

	public abstract object Clone();
}
