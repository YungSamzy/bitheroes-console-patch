using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.character;

public class CharacterHeroTagData
{
	private int _charID;

	private string _name;

	private int _level;

	private string _herotag;

	private string _guildInitials;

	public string parsedName => Util.ParseName(_name, (_guildInitials != null) ? _guildInitials : null);

	public int charID => _charID;

	public string herotag => _herotag;

	public string name => _name;

	public int level => _level;

	public CharacterHeroTagData(int charID, string name, string herotag, int level, string guildInitials)
	{
		_charID = charID;
		_name = name;
		_herotag = herotag;
		_level = level;
		_guildInitials = guildInitials;
	}

	public static CharacterHeroTagData FromSFSObject(ISFSObject sfsob)
	{
		if (!sfsob.ContainsKey("cha1") || !sfsob.ContainsKey("cha4"))
		{
			return null;
		}
		int @int = sfsob.GetInt("cha1");
		string utfString = sfsob.GetUtfString("cha2");
		string utfString2 = sfsob.GetUtfString("cha109");
		int int2 = sfsob.GetInt("cha4");
		string utfString3 = sfsob.GetUtfString("gui3");
		return new CharacterHeroTagData(@int, utfString, utfString2, int2, utfString3);
	}
}
