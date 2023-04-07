using com.ultrabit.bitheroes.ui.dungeon;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.dungeonentitieslist;

public class DungeonEntityItem
{
	public int id;

	public string title;

	public Color color;

	public DungeonEntity entity;

	public DungeonUI dungeonUI;

	public bool draggable = true;

	public bool enabled = true;
}
