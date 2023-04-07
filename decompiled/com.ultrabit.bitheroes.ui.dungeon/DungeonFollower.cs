using System.Collections.Generic;
using com.ultrabit.bitheroes.ui.grid;

namespace com.ultrabit.bitheroes.ui.dungeon;

public class DungeonFollower : GridObject
{
	private GridObject _target;

	private int _index;

	public void Create(GridMap grid, Tile tile, GridObject target, int index)
	{
		LoadDetails(grid, tile, 250f, clickable: false);
		_target = target;
		_target.MOVEMENT_CHANGE.AddListener(OnMovementChange);
		_index = index;
	}

	private void OnMovementChange(object e)
	{
		Tile tile = e as Tile;
		List<Tile> list = base.grid.GeneratePath(this, tile);
		if (list.Count > 0)
		{
			list.RemoveAt(list.Count - 1);
		}
		if (list.Count > 0)
		{
			tile = list[list.Count - 1];
			if (((base.path.Count > 0) ? base.path[base.path.Count - 1] : null) != tile)
			{
				SetPath(list);
			}
		}
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
	}
}
