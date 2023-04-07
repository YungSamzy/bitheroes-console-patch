using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.augment;
using com.ultrabit.bitheroes.model.data;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.mount;
using com.ultrabit.bitheroes.model.variable;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.item;

public class ItemIconRanks : MonoBehaviour
{
	public const int MAX_RANKS = 6;

	public Transform itemRankTilePrefab;

	private ItemRankTile[] _rankTiles;

	public void SetRank(BaseModelData item, bool showRanks, bool isThumbnail)
	{
		if (_rankTiles == null)
		{
			_rankTiles = GetComponentsInChildren<ItemRankTile>();
		}
		ClearRankObjects();
		if (item == null || item.itemRef == null || !showRanks)
		{
			return;
		}
		int num = item.itemRef.ranks;
		int num2 = item.itemRef.rank;
		int frame = 2;
		switch (item.itemRef.itemType)
		{
		case 6:
		{
			FamiliarRef familiarRef = item.itemRef as FamiliarRef;
			num = VariableBook.familiarStableMaxQty;
			num2 = GameData.instance.PROJECT.character.familiarStable.getFamiliarQty(familiarRef);
			frame = 3;
			break;
		}
		case 8:
		case 17:
			num = (item.itemRef as MountRef).mountRarityRef.rankMax;
			if (item is MountData)
			{
				num2 = (item as MountData).rank;
			}
			break;
		case 15:
			_ = item.itemRef;
			if (item is AugmentData)
			{
				AugmentData obj = item as AugmentData;
				num = obj.getRankMax();
				num2 = obj.getRank(GameData.instance.PROJECT.character.familiarStable);
				frame = 3;
			}
			break;
		}
		if (num > 0)
		{
			int num3 = _rankTiles.Length;
			for (int i = 0; i < num && i < num3; i++)
			{
				_rankTiles[i].gameObject.SetActive(value: true);
				_rankTiles[i].LoadDetails(num2 > i, frame);
			}
		}
	}

	private void ClearRankObjects()
	{
		if (_rankTiles != null && _rankTiles.Length != 0)
		{
			for (int i = 0; i < _rankTiles.Length; i++)
			{
				_rankTiles[i].gameObject.SetActive(value: false);
			}
		}
	}
}
