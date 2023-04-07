using com.ultrabit.bitheroes.core;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.utility;

public class RankComponent : MonoBehaviour
{
	private Sprite[] ranks;

	private void Awake()
	{
		ranks = new Sprite[3];
		ranks[0] = GameData.instance.main.assetLoader.GetAsset<GameObject>("Prefabs/Rank/RankEmpty").GetComponent<Image>().sprite;
		ranks[1] = GameData.instance.main.assetLoader.GetAsset<GameObject>("Prefabs/Rank/RankGreen").GetComponent<Image>().sprite;
		ranks[2] = GameData.instance.main.assetLoader.GetAsset<GameObject>("Prefabs/Rank/RankBlue").GetComponent<Image>().sprite;
	}

	public Sprite GetRank(int rank)
	{
		int num = rank - 1;
		if (ranks.Length > num)
		{
			return ranks[num];
		}
		return null;
	}
}
