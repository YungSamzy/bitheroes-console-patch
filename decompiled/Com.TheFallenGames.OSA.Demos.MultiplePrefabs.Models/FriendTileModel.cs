using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.friend;

namespace Com.TheFallenGames.OSA.Demos.MultiplePrefabs.Models;

public class FriendTileModel : BaseModel
{
	public CharacterData characterData;

	public string name;

	public string level;

	public string login;

	public bool online;

	public RequestData requestData;

	public int stamina;

	public int power;

	public int agility;

	public int stats;

	public int levelVal;

	public string nameVal;

	public long loginMilliseconds;
}
