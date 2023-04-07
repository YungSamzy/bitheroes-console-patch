using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.utility;

public class CutoutMaskImage : Image
{
	public override Material materialForRendering
	{
		get
		{
			Material obj = new Material(base.materialForRendering);
			obj.SetInt("_StencilComp", 6);
			return obj;
		}
	}
}
