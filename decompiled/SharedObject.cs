using System.Collections.Generic;

public class SharedObject
{
	public List<SOValue> values;

	public SOValue GetVal()
	{
		return default(SOValue);
	}

	public SharedObject()
	{
		values = new List<SOValue>();
	}

	public SOValue Get(string keyword)
	{
		for (int i = 0; i < values.Count; i++)
		{
			if (values[i].key == keyword)
			{
				return values[i];
			}
		}
		return default(SOValue);
	}
}
