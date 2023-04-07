using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.wallet;

public class WalletBook
{
	private static List<NFTItemRef> _nftItems;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		if (XMLBook.instance.walletBookData == null)
		{
			yield break;
		}
		_nftItems = new List<NFTItemRef>();
		foreach (WalletBookData.NFT item in XMLBook.instance.walletBookData.nftItems.nft)
		{
			string identifier = item.identifier;
			string source = item.source;
			List<ItemData> list = new List<ItemData>();
			foreach (WalletBookData.Item item2 in item.items)
			{
				ItemRef itemRef = ItemBook.Lookup(item2.id, item2.type);
				list.Add(new ItemData(itemRef, 1));
			}
			_nftItems.Add(new NFTItemRef(identifier, source, list));
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static bool isNFT(ItemRef itemRef)
	{
		if (_nftItems == null || _nftItems.Count <= 0)
		{
			return false;
		}
		int itemType = itemRef.itemType;
		int id = itemRef.id;
		foreach (NFTItemRef nftItem in _nftItems)
		{
			foreach (ItemData item in nftItem.getItems())
			{
				if (item.itemRef.id == id && item.itemRef.itemType == itemType)
				{
					return true;
				}
			}
		}
		return false;
	}
}
