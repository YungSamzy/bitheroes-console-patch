public class UnityKeyValuePair<K, V>
{
	public virtual K Key { get; set; }

	public virtual V Value { get; set; }

	public UnityKeyValuePair()
	{
		Key = default(K);
		Value = default(V);
	}

	public UnityKeyValuePair(K key, V value)
	{
		Key = key;
		Value = value;
	}
}
