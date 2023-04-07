using System.Collections.Generic;
using com.ultrabit.bitheroes.ui.grid;
using UnityEngine;

public class PathGrid
{
	private GridMap grid;

	private Tile start;

	private Tile target;

	private Tile current;

	private Tile preselection;

	private List<Tile> ListaAbierta;

	private List<Tile> ListaCerrada;

	public PathGrid(GridMap _grid)
	{
		grid = _grid;
		ListaAbierta = new List<Tile>();
		ListaCerrada = new List<Tile>();
	}

	public void GetPath(Tile selectTile)
	{
		if (start == null)
		{
			start = (current = selectTile);
			start.sprite.color = Color.green;
		}
		else
		{
			target = selectTile;
			target.sprite.color = Color.red;
			SelectNeighbors(start);
		}
	}

	private void SelectNeighbors(Tile tile)
	{
		if (target == preselection)
		{
			foreach (Tile listaCerradum in ListaCerrada)
			{
				listaCerradum.sprite.color = Color.green;
			}
			return;
		}
		current = tile;
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if (i == 0 && j == 0)
				{
					if (!ListaCerrada.Contains(grid.tiles[tile.xPos + i, tile.yPos + j]))
					{
						ListaCerrada.Add(grid.tiles[tile.xPos + i, tile.yPos + j]);
					}
					continue;
				}
				if ((i == 0 && j != 0) || (i != 0 && j == 0))
				{
					grid.tiles[tile.xPos + i, tile.yPos + j].g = 10;
				}
				else
				{
					grid.tiles[tile.xPos + i, tile.yPos + j].g = 15;
				}
				grid.tiles[tile.xPos + i, tile.yPos + j].h = Mathf.Abs(grid.tiles[tile.xPos + i, tile.yPos + j].xPos - target.xPos) + Mathf.Abs(grid.tiles[tile.xPos + i, tile.yPos + j].yPos - target.yPos);
				grid.tiles[tile.xPos + i, tile.yPos + j].f = grid.tiles[tile.xPos + i, tile.yPos + j].h + grid.tiles[tile.xPos + i, tile.yPos + j].g;
				grid.tiles[tile.xPos + i, tile.yPos + j].parentTile = grid.tiles[tile.xPos, tile.yPos];
				if (grid.tiles[tile.xPos + i, tile.yPos + j].walkable)
				{
					grid.tiles[tile.xPos + i, tile.yPos + j].sprite.color = Color.blue;
					ListaAbierta.Add(grid.tiles[tile.xPos + i, tile.yPos + j]);
				}
			}
		}
		preselection = null;
		foreach (Tile listaAbiertum in ListaAbierta)
		{
			if (preselection == null || preselection.h > listaAbiertum.h)
			{
				preselection = listaAbiertum;
			}
		}
		ListaCerrada.Add(preselection);
		ListaAbierta.Remove(preselection);
		start = (current = preselection);
	}
}
