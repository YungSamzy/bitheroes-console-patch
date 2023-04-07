using System.Collections.Generic;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.segmented;

public class SegmentedRewardZoneCompleteRef
{
	private int _min;

	private int _max;

	private List<ItemData> _items;

	public List<ItemData> items => _items;

	public SegmentedRewardZoneCompleteRef(RewardBookData.ZoneCompleted zoneData)
	{
		_min = zoneData.min;
		_max = zoneData.max;
		_items = new List<ItemData>();
		foreach (RewardBookData.ZonePool item in zoneData.lstPool)
		{
			SegmentedPoolRef poolByLink = RewardBook.GetPoolByLink(item.link);
			if (poolByLink == null)
			{
				continue;
			}
			foreach (ItemRef item2 in poolByLink.items)
			{
				_items.Add(new ItemData(item2, item.qty));
			}
		}
	}

	public bool ContainsZone(int zone)
	{
		if (_min <= zone && _max >= zone)
		{
			return true;
		}
		return false;
	}
}
