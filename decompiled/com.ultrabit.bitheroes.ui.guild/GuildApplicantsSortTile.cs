using com.ultrabit.bitheroes.model.user;

namespace com.ultrabit.bitheroes.ui.guild;

public class GuildApplicantsSortTile
{
	private UserData _userData;

	public UserData userData => _userData;

	public long loginMilliseconds
	{
		get
		{
			if (!_userData.online)
			{
				return -_userData.characterData.loginMilliseconds;
			}
			return 2147483647L;
		}
	}

	public int level => _userData.characterData.level;

	public int stats => _userData.characterData.getTotalStats();

	public int power => _userData.characterData.power;

	public int stamina => _userData.characterData.stamina;

	public int agility => _userData.characterData.agility;

	public string name => _userData.characterData.name;

	public GuildApplicantsSortTile(UserData userData)
	{
		_userData = userData;
	}
}
