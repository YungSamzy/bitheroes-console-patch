using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.rarity;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.utility;

public class TrophyHandler : MonoBehaviour
{
	public void ReplaceTrophy(GameObject origin, int rank)
	{
		if (!(origin != null))
		{
			return;
		}
		GameObject trophy = GameData.instance.trophyComponent.GetTrophy(rank);
		origin.GetComponent<Image>().sprite = trophy.GetComponent<Image>().sprite;
		if (origin.GetComponent<Animator>() != null)
		{
			Object.Destroy(origin.GetComponent<Animator>());
		}
		if (trophy.GetComponent<Animator>() != null)
		{
			Animator animator = origin.AddComponent<Animator>();
			animator.runtimeAnimatorController = trophy.GetComponent<Animator>().runtimeAnimatorController;
			Debug.Log($"************************** {animator} rank: {rank}");
			switch (rank)
			{
			case 1:
				animator.SetTrigger("gold");
				break;
			case 2:
				animator.SetTrigger("silver");
				break;
			case 3:
				animator.SetTrigger("bronze");
				break;
			}
		}
	}

	public void ReplaceTrophyByRarity(GameObject origin, RarityRef rarity)
	{
		int rank = 0;
		if (rarity.link == "generic" || rarity.link == "generic")
		{
			rank = 3;
		}
		else if (rarity.link == "rare" || rarity.link == "epic")
		{
			rank = 2;
		}
		else if (rarity.link == "legendary" || rarity.link == "set")
		{
			rank = 2;
		}
		ReplaceTrophy(origin, rank);
	}
}
