using System.Collections.Generic;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.segmented;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.events;

public class EventSegmentedPoolRef
{
	private string _link;

	private int _qty;

	private List<ItemData> _items;

	public string link => _link;

	public int qty => _qty;

	public List<ItemData> items => _items;

	public EventSegmentedPoolRef(EventRewardBookData.ZonePool data)
	{
		_link = data.link;
		_qty = ((data.qty == null) ? 1 : int.Parse(data.qty));
		_items = new List<ItemData>();
		SegmentedPoolRef poolByLink = EventRewardBook.GetPoolByLink(_link);
		if (poolByLink == null)
		{
			return;
		}
		foreach (ItemRef item in poolByLink.items)
		{
			_items.Add(new ItemData(item, _qty));
		}
	}
}
