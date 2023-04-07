using UnityEngine;

namespace com.ultrabit.bitheroes.ui.assets;

public class NPCAsset : MonoBehaviour
{
	private SWFAsset _swfasset;

	public void SetSWFAsset(SWFAsset swfa)
	{
		_swfasset = swfa;
	}

	private void OnTriggerAttack()
	{
		_swfasset?.OnTriggerAttack();
	}
}
