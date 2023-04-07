using UnityEngine;

namespace Kongregate;

public class InitializeTargetPlatform
{
	public static void Initialize()
	{
		TargetPlatform.Current = TargetPlatforms.Standalone;
		Debug.Log("Kongregate target platform is standalone");
	}
}
