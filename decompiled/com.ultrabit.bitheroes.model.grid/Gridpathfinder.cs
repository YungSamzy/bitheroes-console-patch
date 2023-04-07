using System.Collections.Generic;
using System.Linq;
using com.ultrabit.bitheroes.ui.grid;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.grid;

public class Gridpathfinder
{
	private static int MAX_ITERATIONS = 1000;

	private GridMap _grid;

	private Tile _currentPathTile;

	private Tile _targetPathTile;

	private Tile _originPathTile;

	private List<Tile> _closedList = new List<Tile>();

	private List<Tile> _openList = new List<Tile>();

	public Gridpathfinder(GridMap grid)
	{
		_grid = grid;
	}

	public List<Tile> getPath(Tile start, Tile target)
	{
		_currentPathTile = start;
		_targetPathTile = target;
		reset();
		bool flag = false;
		int num = 0;
		flag = stepPathfinder();
		while (!flag)
		{
			flag = stepPathfinder();
			if (num++ > MAX_ITERATIONS)
			{
				return null;
			}
		}
		List<Tile> list = new List<Tile>();
		int num2 = 0;
		for (Tile tile = _closedList[_closedList.Count - 1]; tile != _originPathTile; tile = tile.parentTile)
		{
			if (num2++ > MAX_ITERATIONS)
			{
				return null;
			}
			list.Add(tile);
		}
		list.Reverse();
		list.RemoveAt(0);
		return list;
	}

	private bool stepPathfinder()
	{
		if (_currentPathTile == _targetPathTile)
		{
			_closedList.Add(_targetPathTile);
			return true;
		}
		_ = _currentPathTile;
		_openList.Add(_currentPathTile);
		List<Tile> adjacentCell = new List<Tile>();
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if ((i != 0 || j != 0) && _currentPathTile.xPos + i >= 0 && _currentPathTile.yPos + j >= 0 && _currentPathTile.xPos + i < _grid.xCount && _currentPathTile.yPos + j < _grid.yCount && _grid.tiles[_currentPathTile.xPos + i, _currentPathTile.yPos + j] != null)
				{
					Tile tile = _grid.tiles[_currentPathTile.xPos + i, _currentPathTile.yPos + j];
					if (tile.walkable && _closedList.IndexOf(tile) == -1)
					{
						adjacentCell.Add(tile);
					}
				}
			}
		}
		int ii;
		for (ii = 0; ii < adjacentCell.Count; ii++)
		{
			int num = _currentPathTile.g + 1;
			int num2 = Mathf.Abs(adjacentCell[ii].x - _targetPathTile.x) + Mathf.Abs(adjacentCell[ii].y - _targetPathTile.y);
			if (_openList.Find((Tile x) => x.id == adjacentCell[ii].id) == null)
			{
				adjacentCell[ii].f = num + num2;
				adjacentCell[ii].parentTile = _currentPathTile;
				adjacentCell[ii].g = num;
				_openList.Add(adjacentCell[ii]);
			}
			else if (adjacentCell[ii].g < _currentPathTile.parentTile.g)
			{
				_currentPathTile.parentTile = adjacentCell[ii];
				_currentPathTile.g = adjacentCell[ii].g + 1;
				_currentPathTile.f = adjacentCell[ii].g + num2;
			}
		}
		int index = _openList.IndexOf(_currentPathTile);
		_closedList.Add(_currentPathTile);
		_openList.RemoveRange(index, 1);
		_openList = _openList.OrderByDescending((Tile x) => x.f).ToList();
		if (_openList.Count == 0)
		{
			return true;
		}
		_currentPathTile = _openList[_openList.Count - 1];
		_openList.RemoveAt(_openList.Count - 1);
		return false;
	}

	private void reset()
	{
		for (int i = 0; i < _grid.xCount; i++)
		{
			for (int j = 0; j < _grid.yCount; j++)
			{
				if (_grid.tiles[i, j] != null)
				{
					Tile tile = _grid.tiles[i, j];
					tile.parentTile = null;
					tile.g = 0;
					tile.f = 0;
				}
			}
		}
		_openList = new List<Tile>();
		_closedList = new List<Tile>();
		_closedList.Add(_originPathTile);
	}
}
