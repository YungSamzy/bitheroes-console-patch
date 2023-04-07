using com.ultrabit.bitheroes.ui.dungeon;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.teamlistondungeon;

public class DungeonEntityTileModel
{
	public int id;

	public string title;

	public Color color;

	public DungeonEntity entity;

	public DungeonUI dungeonUI;

	public bool draggable = true;

	public bool enabled = true;

	public bool extraFix;
}
