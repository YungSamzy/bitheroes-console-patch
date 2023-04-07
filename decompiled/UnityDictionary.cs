using System;
using System.Collections;
using System.Collections.Generic;

public abstract class UnityDictionary<K, V> : IDictionary<K, V>, ICollection<KeyValuePair<K, V>>, IEnumerable<KeyValuePair<K, V>>, IEnumerable
{
	internal sealed class UnityDictionaryEnumerator : IEnumerator<KeyValuePair<K, V>>, IEnumerator, IDisposable
	{
		private KeyValuePair<K, V>[] items;

		private int index = -1;

		object IEnumerator.Current => Current;

		public KeyValuePair<K, V> Current
		{
			get
			{
				ValidateIndex();
				return items[index];
			}
		}

		public KeyValuePair<K, V> Entry => Current;

		public K Key
		{
			get
			{
				ValidateIndex();
				return items[index].Key;
			}
		}

		public V Value
		{
			get
			{
				ValidateIndex();
				return items[index].Value;
			}
		}

		internal UnityDictionaryEnumerator()
		{
		}

		internal UnityDictionaryEnumerator(UnityDictionary<K, V> ud)
		{
			items = new KeyValuePair<K, V>[ud.Count];
			ud.CopyTo(items, 0);
		}

		public void Dispose()
		{
			index = -1;
			items = null;
		}

		public bool MoveNext()
		{
			if (index < items.Length - 1)
			{
				index++;
				return true;
			}
			return false;
		}

		private void ValidateIndex()
		{
			if (index < 0 || index >= items.Length)
			{
				throw new InvalidOperationException("Enumerator is before or after the collection.");
			}
		}

		public void Reset()
		{
			index = -1;
		}
	}

	protected abstract List<UnityKeyValuePair<K, V>> KeyValuePairs { get; set; }

	public virtual V this[K key]
	{
		get
		{
			UnityKeyValuePair<K, V> unityKeyValuePair = KeyValuePairs.Find((UnityKeyValuePair<K, V> x) => x.Key.Equals(key));
			if (unityKeyValuePair == null)
			{
				return default(V);
			}
			return unityKeyValuePair.Value;
		}
		set
		{
			if (key != null)
			{
				SetKeyValuePair(key, value);
			}
		}
	}

	public int Count => KeyValuePairs.Count;

	public ICollection<K> Keys
	{
		get
		{
			ICollection<K> collection = new List<K>();
			foreach (UnityKeyValuePair<K, V> keyValuePair in KeyValuePairs)
			{
				collection.Add(keyValuePair.Key);
			}
			return collection;
		}
	}

	public ICollection<V> Values
	{
		get
		{
			ICollection<V> collection = new List<V>();
			foreach (UnityKeyValuePair<K, V> keyValuePair in KeyValuePairs)
			{
				collection.Add(keyValuePair.Value);
			}
			return collection;
		}
	}

	public ICollection<KeyValuePair<K, V>> Items
	{
		get
		{
			List<KeyValuePair<K, V>> list = new List<KeyValuePair<K, V>>();
			foreach (UnityKeyValuePair<K, V> keyValuePair in KeyValuePairs)
			{
				list.Add(new KeyValuePair<K, V>(keyValuePair.Key, keyValuePair.Value));
			}
			return list;
		}
	}

	public V SyncRoot => default(V);

	public bool IsFixedSize => false;

	public bool IsReadOnly => false;

	public bool IsSynchronized => false;

	protected abstract void SetKeyValuePair(K k, V v);

	public void Add(K key, V value)
	{
		this[key] = value;
	}

	public void Add(KeyValuePair<K, V> kvp)
	{
		this[kvp.Key] = kvp.Value;
	}

	public bool TryGetValue(K key, out V value)
	{
		if (!ContainsKey(key))
		{
			value = default(V);
			return false;
		}
		value = this[key];
		return true;
	}

	public bool Remove(KeyValuePair<K, V> item)
	{
		return Remove(item.Key);
	}

	public bool Remove(K key)
	{
		List<UnityKeyValuePair<K, V>> keyValuePairs = KeyValuePairs;
		int num = keyValuePairs.FindIndex((UnityKeyValuePair<K, V> x) => x.Key.Equals(key));
		if (num == -1)
		{
			return false;
		}
		keyValuePairs.RemoveAt(num);
		KeyValuePairs = keyValuePairs;
		return true;
	}

	public void Clear()
	{
		List<UnityKeyValuePair<K, V>> keyValuePairs = KeyValuePairs;
		keyValuePairs.Clear();
		KeyValuePairs = keyValuePairs;
	}

	public bool ContainsKey(K key)
	{
		return KeyValuePairs.FindIndex((UnityKeyValuePair<K, V> x) => x.Key.Equals(key)) != -1;
	}

	public bool Contains(KeyValuePair<K, V> kvp)
	{
		return this[kvp.Key].Equals(kvp.Value);
	}

	public void CopyTo(KeyValuePair<K, V>[] array, int index)
	{
		List<KeyValuePair<K, V>> list = new List<KeyValuePair<K, V>>();
		for (int i = 0; i < KeyValuePairs.Count; i++)
		{
			list[i] = ConvertUkvp(KeyValuePairs[i]);
		}
		list.CopyTo(array, index);
	}

	public KeyValuePair<K, V> ConvertUkvp(UnityKeyValuePair<K, V> ukvp)
	{
		return new KeyValuePair<K, V>(ukvp.Key, ukvp.Value);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
	{
		return new UnityDictionaryEnumerator(this);
	}
}
public abstract class UnityDictionary<V> : UnityDictionary<string, V>
{
}
