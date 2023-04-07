namespace com.ultrabit.bitheroes.model.character;

public class TutorialData
{
	private int _id;

	private int _step;

	private string _name;

	private string _desc;

	private bool _finalized;

	public int id => _id;

	public int step => _step;

	public string name => _name;

	public string desc => _desc;

	public bool finalized => _finalized;

	public TutorialData(int id, int step, string name, string desc, bool finalized)
	{
		_id = id;
		_step = step;
		_name = name;
		_desc = desc;
		_finalized = finalized;
	}
}
