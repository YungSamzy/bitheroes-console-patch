using UnityEngine;

namespace com.ultrabit.bitheroes.ui.utility;

public class TrophyComponent : MonoBehaviour
{
	public GameObject[] trophies;

	public GameObject GetTrophy(int rank)
	{
		int num = rank - 1;
		if (num < 0 || trophies.Length <= num)
		{
			num = trophies.Length - 1;
		}
		return trophies[num];
	}
}
