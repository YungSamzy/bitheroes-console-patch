using System.Collections.Generic;

namespace com.ultrabit.bitheroes.model.character;

public class CharacterInfoData
{
	private string _name;

	private bool _outline;

	private int _offset;

	private string _colorName;

	private string _colorValue;

	private List<CharacterInfoValue> _info = new List<CharacterInfoValue>();

	public string name => _name;

	public bool outline => _outline;

	public int offset => _offset;

	public string colorName => _colorName;

	public string colorValue => _colorValue;

	public List<CharacterInfoValue> info => _info;

	public CharacterInfoData(string name, bool outline = true, int offset = 0, string colorName = null, string colorValue = null)
	{
		_name = name;
		_outline = outline;
		_offset = offset;
		_colorName = colorName;
		_colorValue = colorValue;
	}

	public void addValue(string name, object value, string color = null)
	{
		_info.Add(new CharacterInfoValue(name, value.ToString(), color));
	}
}
