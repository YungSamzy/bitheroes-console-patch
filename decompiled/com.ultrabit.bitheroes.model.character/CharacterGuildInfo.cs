using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.character;

public class CharacterGuildInfo
{
	private int _id;

	private string _name;

	private string _initials;

	public int id => _id;

	public string name => _name;

	public string initials => _initials;

	public CharacterGuildInfo(int id, string name, string initials)
	{
		_id = id;
		_name = name;
		_initials = initials;
	}

	public static CharacterGuildInfo fromSFSObject(ISFSObject sfsob)
	{
		if (!sfsob.ContainsKey("gui0"))
		{
			return null;
		}
		int @int = sfsob.GetInt("gui0");
		string utfString = sfsob.GetUtfString("gui2");
		string utfString2 = sfsob.GetUtfString("gui3");
		return new CharacterGuildInfo(@int, utfString, utfString2);
	}
}
