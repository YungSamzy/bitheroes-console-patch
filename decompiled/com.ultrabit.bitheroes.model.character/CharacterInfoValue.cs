namespace com.ultrabit.bitheroes.model.character;

public class CharacterInfoValue
{
	private string _name;

	private string _value;

	private string _color;

	public string name => _name;

	public string value => _value;

	public string color => _color;

	public CharacterInfoValue(string name, string value, string color = null)
	{
		_name = name;
		_value = value;
		_color = color;
	}
}
