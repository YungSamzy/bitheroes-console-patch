using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.dungeon;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.dungeon;

public class DungeonOverlay : MonoBehaviour
{
	private Dungeon _dungeon;

	private DungeonOverlayRef _overlayRef;

	public SpriteRenderer _renderer;

	private bool doScroll;

	private float speed = 20f;

	private Vector3 initialPosition;

	private Vector3 endPosition;

	public void LoadDetails(Dungeon dungeon, DungeonOverlayRef overlayRef)
	{
		_dungeon = dungeon;
		_overlayRef = overlayRef;
		Sprite spriteAsset = GameData.instance.main.assetLoader.GetSpriteAsset(AssetURL.DUNGEON_OVERLAY, overlayRef.asset);
		Render(spriteAsset);
	}

	private void Render(Sprite overlayAsset)
	{
		if (!(overlayAsset == null))
		{
			_ = GameData.instance.main.mainCamera;
			float num = (float)((double)_dungeon.height * 2.0);
			float num2 = _dungeon.width * 2f;
			base.transform.parent = _dungeon.transform;
			_renderer.sprite = overlayAsset;
			Vector2 size = _renderer.size;
			size.x = num2;
			size.y = num;
			_renderer.size = size;
			_renderer.drawMode = SpriteDrawMode.Tiled;
			_renderer.sortingLayerName = "Background";
			_renderer.sortingOrder = 5000;
			base.transform.localScale = Vector3.one;
			_renderer.transform.localScale = new Vector3(2f, 2f, 2f);
			_renderer.transform.localPosition = new Vector3(num2 * -1.5f, (0f - num) * -1.5f, 0f);
			initialPosition = _renderer.transform.localPosition;
			endPosition = new Vector3((0f - num2) / 2f, num / 2f, 0f);
			doScroll = true;
		}
	}

	private void Update()
	{
		Scroll();
	}

	public void Scroll()
	{
		if (doScroll)
		{
			Vector3 localPosition = _renderer.transform.localPosition;
			localPosition.x += Time.deltaTime * _overlayRef.scroll.x * speed;
			localPosition.y += Time.deltaTime * _overlayRef.scroll.y * (0f - speed);
			if (localPosition.x > endPosition.x || localPosition.y < endPosition.y)
			{
				localPosition = initialPosition;
			}
			_renderer.transform.localPosition = localPosition;
		}
	}
}
