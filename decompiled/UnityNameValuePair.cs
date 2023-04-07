public class UnityNameValuePair<V> : UnityKeyValuePair<string, V>
{
	public string name;

	public override string Key
	{
		get
		{
			return name;
		}
		set
		{
			name = value;
		}
	}

	public UnityNameValuePair()
	{
	}

	public UnityNameValuePair(string key, V value)
		: base(key, value)
	{
	}
}
