namespace com.ultrabit.bitheroes.model.playervoting;

public class CandidateRef
{
	private int _id;

	private string _name;

	private string _guild;

	public string name => _name;

	public int id => _id;

	public string guild
	{
		get
		{
			return _guild;
		}
		set
		{
			_guild = value;
		}
	}

	public CandidateRef(int id, string name, string guild)
	{
		_guild = guild;
		_name = name;
		_id = id;
	}
}
