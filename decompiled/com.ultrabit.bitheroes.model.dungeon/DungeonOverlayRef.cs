using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;
using com.ultrabit.bitheroes.ui.assets;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.dungeon;

public class DungeonOverlayRef
{
	private string _asset;

	private Vector2 _scroll;

	private bool _loadLocal;

	public Vector2 scroll => _scroll;

	public string asset => _asset;

	public DungeonOverlayRef(DungeonBookData.Overlay overlayData)
	{
		_asset = ((overlayData.asset != null) ? overlayData.asset : null);
		_scroll = ((overlayData.scrollString != null) ? Util.pointFromString(overlayData.scrollString) : new Vector2(0f, 0f));
	}

	public ImageAsset getAsset(bool center = false, float scale = 1f)
	{
		return null;
	}
}
