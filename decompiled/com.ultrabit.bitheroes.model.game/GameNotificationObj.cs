namespace com.ultrabit.bitheroes.model.game;

public class GameNotificationObj
{
	private float _duration;

	private int _charID;

	private string _name;

	private string _text;

	private int _type;

	private object _data;

	public float duration => _duration;

	public int charID => _charID;

	public string name => _name;

	public string text => _text;

	public int type => _type;

	public object data => _data;

	public GameNotificationObj(float duration, int charID, string name, string text, int type, object data = null)
	{
		_duration = duration;
		_charID = charID;
		_name = name;
		_text = text;
		_type = type;
		_data = data;
	}
}
