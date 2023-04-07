using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.familiar;

public class FamiliarEncounterItem
{
	private int _type;

	private int _zoneID;

	private int _nodeID;

	private string _link;

	public int type => _type;

	public int zoneID => _zoneID;

	public int nodeID => _nodeID;

	public string link => _link;

	public FamiliarEncounterItem(int type, int zoneID, int nodeID, string link)
	{
		_type = type;
		_zoneID = zoneID;
		_nodeID = nodeID;
		_link = link;
	}

	public static FamiliarEncounterItem fromSFSObject(ISFSObject sfsob)
	{
		int @int = sfsob.GetInt("bat1");
		int num = 0;
		int num2 = 0;
		string text = "";
		if (sfsob.ContainsKey("zon0"))
		{
			num = sfsob.GetInt("zon0");
		}
		if (sfsob.ContainsKey("zon1"))
		{
			num2 = sfsob.GetInt("zon1");
		}
		if (sfsob.ContainsKey("roo2"))
		{
			text = sfsob.GetUtfString("roo2");
		}
		return new FamiliarEncounterItem(@int, num, num2, text);
	}
}
