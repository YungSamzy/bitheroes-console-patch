using com.ultrabit.bitheroes.model.character;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.guild;

public class GuildData
{
	private int _level;

	private long _exp;

	private int _points;

	private string _message;

	private bool _open;

	private Inventory _inventory;

	private GuildHallData _hallData;

	public int level => _level;

	public long exp => _exp;

	public int points => _points;

	public string message => _message;

	public bool open => _open;

	public Inventory inventory => _inventory;

	public GuildHallData hallData => _hallData;

	public GuildData(int level, long exp, int points, string message, bool open, Inventory inventory, GuildHallData hallData)
	{
		_level = level;
		_exp = exp;
		_points = points;
		_message = message;
		_open = open;
		_inventory = inventory;
		_hallData = hallData;
	}

	public void updateData(GuildData data)
	{
		_level = data.level;
		_exp = data.exp;
		_points = data.points;
		_message = data.message;
		_open = data.open;
		if (data.inventory != null)
		{
			_inventory = new Inventory(data.inventory.items);
		}
		if (data.hallData != null)
		{
			_hallData = new GuildHallData(data.hallData.guildID, data.hallData.cosmetics, data.hallData.throne, data.hallData.leftRoom, data.hallData.rightRoom);
		}
	}

	public void setOpen(bool open)
	{
		_open = open;
	}

	public static GuildData fromSFSObject(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("gui7");
		long @long = sfsob.GetLong("gui8");
		int int2 = sfsob.GetInt("gui14");
		string utfString = sfsob.GetUtfString("gui19");
		bool @bool = sfsob.GetBool("gui20");
		Inventory inventory = Inventory.fromSFSObject(sfsob);
		GuildHallData guildHallData = GuildHallData.fromSFSObject(sfsob);
		return new GuildData(@int, @long, int2, utfString, @bool, inventory, guildHallData);
	}
}
