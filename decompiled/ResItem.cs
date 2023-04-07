using System;

[Serializable]
public class ResItem
{
	public string name;

	public string path;

	public ResItem(string _name, string _path)
	{
		name = _name;
		path = _path;
	}
}
