using System.Collections.Generic;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml.common;

namespace com.ultrabit.bitheroes.model.instance;

public class InstanceActionRef
{
	public const int TYPE_WAIT = 0;

	public const int TYPE_MOVE = 1;

	public const int TYPE_ANIMATE = 2;

	private static Dictionary<string, int> TYPES = new Dictionary<string, int>
	{
		["wait"] = 0,
		["move"] = 1,
		["animate"] = 2
	};

	private int _index;

	private int _type;

	private List<string> _values;

	public int index => _index;

	public int type => _type;

	public List<string> values => _values;

	public InstanceActionRef(int index, AssetDisplayData.Action actionData)
	{
		_index = index;
		_type = getType(actionData.type);
		_values = ((actionData.values != null) ? Util.GetStringListFromStringProperty(actionData.values) : new List<string>());
	}

	public string getValue(int id = 0)
	{
		if (id < 0 || id >= _values.Count)
		{
			return null;
		}
		return _values[id];
	}

	public static int getType(string type)
	{
		return TYPES[type.ToLowerInvariant()];
	}
}
