using System;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.Demos.MultiplePrefabs.Models;

namespace Com.TheFallenGames.OSA.Demos.MultiplePrefabs.ViewsHolders;

public abstract class BaseVH : BaseItemViewsHolder
{
	public override void CollectViews()
	{
		base.CollectViews();
	}

	public abstract bool CanPresentModelType(Type modelType);

	public virtual void UpdateViews(BaseModel model)
	{
	}
}
