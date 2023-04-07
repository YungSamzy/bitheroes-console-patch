using com.ultrabit.bitheroes.fromflash;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.assets;

public class Asset : MonoBehaviour
{
	public int scaleX;

	internal float x;

	internal float y;

	internal bool loaded;

	internal DisplayObject theObject;

	public virtual void Awake()
	{
	}
}
