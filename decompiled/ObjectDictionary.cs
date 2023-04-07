using System;
using System.Collections.Generic;

[Serializable]
public class ObjectDictionary : UnityDictionary<string>
{
	public List<ObjectKvp> values;

	protected override List<UnityKeyValuePair<string, string>> KeyValuePairs
	{
		get
		{
			if (values == null)
			{
				values = new List<ObjectKvp>();
			}
			List<UnityKeyValuePair<string, string>> list = new List<UnityKeyValuePair<string, string>>();
			foreach (ObjectKvp value in values)
			{
				list.Add(ConvertOkvp(value));
			}
			return list;
		}
		set
		{
			if (value == null)
			{
				values = new List<ObjectKvp>();
				return;
			}
			foreach (UnityKeyValuePair<string, string> item in value)
			{
				values.Add(ConvertUkvp(item));
			}
		}
	}

	public new ObjectKvp ConvertUkvp(UnityKeyValuePair<string, string> ukvp)
	{
		return new ObjectKvp(ukvp.Key, ukvp.Value);
	}

	public UnityKeyValuePair<string, string> ConvertOkvp(ObjectKvp okvp)
	{
		return new UnityKeyValuePair<string, string>(okvp.Key, okvp.Value);
	}

	protected override void SetKeyValuePair(string k, string v)
	{
		int num = values.FindIndex((ObjectKvp x) => x.Key == k);
		if (num != -1)
		{
			if (v == null)
			{
				values.RemoveAt(num);
			}
			else
			{
				values[num] = new ObjectKvp(k, v);
			}
		}
		else
		{
			values.Add(new ObjectKvp(k, v));
		}
	}
}
