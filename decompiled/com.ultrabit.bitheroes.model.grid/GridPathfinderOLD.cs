using System.Collections.Generic;
using System.Linq;
using com.ultrabit.bitheroes.model.utility;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.grid;

public class GridPathfinderOLD
{
	private GridTile _currentPathTile;

	private GridTile _targetPathTile;

	private List<GridTile> vecinos = new List<GridTile>();

	public List<GridTile> Abierta = new List<GridTile>();

	private int counter;

	public void UltraGetPatch(GridTile start, GridTile target)
	{
		_currentPathTile = start;
		_targetPathTile = target;
		Abierta.Clear();
		Abierta.Add(start);
		counter = 0;
		GetVecinos(start);
	}

	public void GetVecinos(GridTile current)
	{
		if (current == _targetPathTile)
		{
			_ = Abierta.Count;
			_ = 1;
			foreach (GridTile vecino in vecinos)
			{
				vecino.g = 0;
				vecino.f = 0;
				current.parent = null;
			}
			foreach (GridTile abiertum in Abierta)
			{
				abiertum.f = 0;
				abiertum.g = 0;
				current.parent = null;
			}
			vecinos.Clear();
			Abierta.Clear();
			return;
		}
		counter++;
		if (counter > 1000)
		{
			D.Log("Superados!");
			counter = 0;
			return;
		}
		foreach (GridTile vecino2 in vecinos)
		{
			int num = current.g + 1;
			int num2 = Mathf.Abs(vecino2.x - _targetPathTile.x) + Mathf.Abs(vecino2.y - _targetPathTile.y);
			if (Abierta.IndexOf(vecino2) == -1)
			{
				vecino2.f = num + num2;
				vecino2.parent = _currentPathTile;
				vecino2.g = num;
			}
			else if (current.parent != null)
			{
				if (vecino2.g < current.parent.g)
				{
					current.parent = vecino2;
					current.g = vecino2.g + 1;
					current.f = vecino2.g + num2;
				}
			}
			else
			{
				current.parent = vecino2;
				current.g = vecino2.g + 1;
				current.f = vecino2.g + num2;
			}
		}
		if (vecinos != null && vecinos.Count > 0)
		{
			GridTile gridTile = vecinos.OrderByDescending((GridTile i) => i.f).Reverse().ToArray()[0];
			vecinos.Remove(gridTile);
			if (Abierta.IndexOf(gridTile) == -1)
			{
				Abierta.Add(gridTile);
			}
			if (current != _targetPathTile)
			{
				GetVecinos(gridTile);
			}
		}
	}
}
