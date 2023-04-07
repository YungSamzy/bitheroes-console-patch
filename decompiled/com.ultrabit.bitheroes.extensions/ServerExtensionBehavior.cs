using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.utility;
using UnityEngine;

namespace com.ultrabit.bitheroes.extensions;

public class ServerExtensionBehavior : MonoBehaviour
{
	public virtual void Update()
	{
		ServerExtension.instance.Update();
	}

	private void OnApplicationQuit()
	{
		D.Log("ServerExtensionBehaviour::OnApplicationQuit");
		ServerExtension.instance.ClearSmartFox();
	}

	protected virtual void OnDestroy()
	{
		if (GameData.instance != null && GameData.instance.PROJECT != null && GameData.instance.PROJECT.character != null)
		{
			GameData.instance.PROJECT.character.ClearTimers();
		}
	}
}
