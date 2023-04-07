using System;

namespace Com.TheFallenGames.OSA.Demos.MultiplePrefabs.Models;

public abstract class BaseModel
{
	public int id;

	public Type CachedType { get; private set; }

	public BaseModel()
	{
		CachedType = GetType();
	}
}
