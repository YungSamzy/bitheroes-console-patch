using System;
using System.Collections.Generic;
using System.Linq;

namespace com.ultrabit.bitheroes.ui.item;

public struct AdvancedFilterSettings : IEquatable<AdvancedFilterSettings>
{
	private Dictionary<int, bool> _rarityFilters;

	private Dictionary<int, bool> _equipmentFilters;

	private Dictionary<int, bool> _augmentFilters;

	private Dictionary<int, bool> _typeFilters;

	private bool _enabled;

	public Dictionary<int, bool> rarityFilters => _rarityFilters;

	public Dictionary<int, bool> equipmentFilters => _equipmentFilters;

	public Dictionary<int, bool> augmentFilters => _augmentFilters;

	public Dictionary<int, bool> typeFilters => _typeFilters;

	public bool enabled
	{
		get
		{
			return _enabled;
		}
		set
		{
			_enabled = value;
		}
	}

	public AdvancedFilterSettings(bool enabled, Dictionary<int, bool> rarity = null, Dictionary<int, bool> equipment = null, Dictionary<int, bool> augment = null, Dictionary<int, bool> type = null)
	{
		_enabled = enabled;
		_rarityFilters = rarity;
		_equipmentFilters = equipment;
		_augmentFilters = augment;
		_typeFilters = type;
	}

	public bool IsRarityFilterOn(int rarityId)
	{
		if (_rarityFilters.ContainsKey(rarityId))
		{
			return _rarityFilters[rarityId];
		}
		return true;
	}

	public bool IsEquipmentFilterOn(int equipmentId)
	{
		if (_equipmentFilters.ContainsKey(equipmentId))
		{
			return _equipmentFilters[equipmentId];
		}
		return true;
	}

	public bool IsAugmentFilterOn(int augmentId)
	{
		if (_augmentFilters.ContainsKey(augmentId))
		{
			return _augmentFilters[augmentId];
		}
		return true;
	}

	public bool IsTypeFilterOn(int typeID)
	{
		if (_typeFilters.ContainsKey(typeID))
		{
			return _typeFilters[typeID];
		}
		return true;
	}

	public bool TryConvertToBaseFilter(out int filterType, out int filterSurtype, int[] availableTypeFilters, int[] availableSubtypeFilters)
	{
		bool flag = true;
		filterType = -1;
		filterSurtype = -1;
		if (!enabled)
		{
			return false;
		}
		foreach (KeyValuePair<int, bool> rarityFilter in _rarityFilters)
		{
			if (!IsRarityFilterOn(rarityFilter.Key))
			{
				return false;
			}
		}
		int num = -1;
		if (_augmentFilters != null)
		{
			filterSurtype = 15;
			foreach (KeyValuePair<int, bool> augmentFilter in _augmentFilters)
			{
				if (availableSubtypeFilters.Contains(augmentFilter.Key))
				{
					if (augmentFilter.Value)
					{
						num = ((num != -1) ? (-2) : augmentFilter.Key);
					}
					else if (flag)
					{
						flag = false;
					}
				}
			}
			if (flag)
			{
				filterType = 0;
				return true;
			}
			switch (num)
			{
			case -2:
				return false;
			default:
				filterType = num + 1;
				return true;
			case -1:
				break;
			}
		}
		int num2 = -1;
		if (_typeFilters != null)
		{
			foreach (KeyValuePair<int, bool> typeFilter in _typeFilters)
			{
				if (availableTypeFilters.Contains(typeFilter.Key) && typeFilter.Key != 0)
				{
					if (typeFilter.Value)
					{
						num2 = ((num2 != -1) ? (-2) : typeFilter.Key);
					}
					else if (flag)
					{
						flag = false;
					}
				}
			}
			if (!flag)
			{
				if (num2 == -2)
				{
					return false;
				}
				if (num2 != -1 && (num2 != 1 || _equipmentFilters == null))
				{
					filterType = num2;
					return true;
				}
			}
		}
		if (_equipmentFilters != null)
		{
			foreach (KeyValuePair<int, bool> equipmentFilter in _equipmentFilters)
			{
				if (_typeFilters == null)
				{
					if (availableSubtypeFilters.Contains(equipmentFilter.Key) && equipmentFilter.Key != 0)
					{
						if (equipmentFilter.Value)
						{
							num2 = ((num2 != -1) ? (-2) : equipmentFilter.Key);
						}
						else if (flag)
						{
							flag = false;
						}
					}
				}
				else if (!equipmentFilter.Value)
				{
					return false;
				}
			}
			if (_typeFilters == null && !flag)
			{
				switch (num2)
				{
				case -2:
					return false;
				default:
					filterType = num2;
					filterSurtype = 1;
					return true;
				case -1:
					break;
				}
			}
			if (_typeFilters != null && !_typeFilters[8])
			{
				return false;
			}
		}
		else if (!flag)
		{
			return false;
		}
		filterType = ((!flag) ? 1 : 0);
		return true;
	}

	public bool Equals(AdvancedFilterSettings other)
	{
		foreach (KeyValuePair<int, bool> rarityFilter in _rarityFilters)
		{
			if (other._rarityFilters[rarityFilter.Key] != rarityFilter.Value)
			{
				return false;
			}
		}
		foreach (KeyValuePair<int, bool> equipmentFilter in _equipmentFilters)
		{
			if (other._equipmentFilters[equipmentFilter.Key] != equipmentFilter.Value)
			{
				return false;
			}
		}
		foreach (KeyValuePair<int, bool> augmentFilter in _augmentFilters)
		{
			if (other._augmentFilters[augmentFilter.Key] != augmentFilter.Value)
			{
				return false;
			}
		}
		foreach (KeyValuePair<int, bool> typeFilter in _typeFilters)
		{
			if (other._typeFilters[typeFilter.Key] != typeFilter.Value)
			{
				return false;
			}
		}
		return true;
	}
}
