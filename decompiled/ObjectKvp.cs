using System;

[Serializable]
public sealed class ObjectKvp : UnityNameValuePair<string>
{
	public string value;

	public override string Value
	{
		get
		{
			return value;
		}
		set
		{
			this.value = value;
		}
	}

	public ObjectKvp(string key, string value)
		: base(key, value)
	{
	}
}
