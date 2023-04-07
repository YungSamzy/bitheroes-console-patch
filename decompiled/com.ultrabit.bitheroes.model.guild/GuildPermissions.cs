using System.Collections.Generic;
using com.ultrabit.bitheroes.model.utility;
using Sfs2X.Entities.Data;

namespace com.ultrabit.bitheroes.model.guild;

public class GuildPermissions
{
	private List<List<bool>> _permissions;

	private bool _changed;

	public bool changed => _changed;

	public GuildPermissions(List<List<bool>> permissions)
	{
		_permissions = permissions;
	}

	public bool getRankPermission(int rank, int permission)
	{
		if (rank < 0 || rank >= _permissions.Count)
		{
			return false;
		}
		List<bool> list = _permissions[rank];
		if (permission < 0 || permission >= list.Count)
		{
			return false;
		}
		return list[permission];
	}

	public void setRankPermission(int rank, int permission, bool flag)
	{
		if (rank >= 0 && rank < _permissions.Count)
		{
			List<bool> list = _permissions[rank];
			if (permission >= 0 && permission < list.Count)
			{
				list[permission] = flag;
				_changed = true;
			}
		}
	}

	public SFSObject toSFSObject(SFSObject sfsob)
	{
		ISFSArray iSFSArray = new SFSArray();
		for (int i = 0; i < _permissions.Count; i++)
		{
			List<bool> vector = _permissions[i];
			SFSObject sFSObject = new SFSObject();
			sFSObject.PutBoolArray("gui11", Util.getArrayFromBooleanVector(vector));
			iSFSArray.AddSFSObject(sFSObject);
		}
		sfsob.PutSFSArray("gui4", iSFSArray);
		return sfsob;
	}

	public static GuildPermissions fromSFSObject(ISFSObject sfsob)
	{
		ISFSArray sFSArray = sfsob.GetSFSArray("gui4");
		List<List<bool>> list = new List<List<bool>>();
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			List<bool> booleanVectorFromArray = Util.getBooleanVectorFromArray(sFSArray.GetSFSObject(i).GetBoolArray("gui11"));
			list.Add(booleanVectorFromArray);
		}
		return new GuildPermissions(list);
	}
}
