using com.ultrabit.bitheroes.core;
using UnityEngine;

public class TestWindowCreation : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F2))
		{
			GameData.instance.windowGenerator.NewRiftEventWindow();
		}
		Input.GetKeyDown(KeyCode.F3);
	}
}
