using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.utility;

public class CutoutMaskTextMeshProUGUI : TextMeshProUGUI
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
