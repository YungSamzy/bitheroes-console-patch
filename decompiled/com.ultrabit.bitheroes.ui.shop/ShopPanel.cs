using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.booster;
using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.consumable;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.shop;
using com.ultrabit.bitheroes.model.variable;
using com.ultrabit.bitheroes.ui.language;
using com.ultrabit.bitheroes.ui.tutorial;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.shop;

public class ShopPanel : MonoBehaviour
{
	public Transform featuredListPrefab;

	public Transform generalListPrefab;

	public Transform shopItemTilePrefab;

	private ShopTabRef _tabRef;

	private ShopWindow _shopWindow;

	private List<GameObject> _tiles;

	private ShopPanelList _shopPanelList;

	private AsianLanguageFontManager asianLangManager;

	private ShopRotationTile _shopRotationTile;

	private ShopPromoTile _shopPromoTile;

	public ShopTabRef tabRef => _tabRef;

	public void LoadDetails(ShopWindow shopWindow, ShopTabRef tabRef = null)
	{
		_tabRef = tabRef;
		_shopWindow = shopWindow;
		Transform transform = ((_tabRef != null) ? Object.Instantiate(generalListPrefab) : Object.Instantiate(featuredListPrefab));
		transform.SetParent(base.transform, worldPositionStays: false);
		_shopPanelList = transform.GetComponent<ShopPanelList>();
		CreateTiles();
	}

	public void CheckTutorial()
	{
		if (!GameData.instance.tutorialManager.hasPopup && base.gameObject.activeSelf && !GameData.instance.PROJECT.character.tutorial.GetState(29))
		{
			ShopItemTile tutorialItemTile = GetTutorialItemTile();
			if (tutorialItemTile != null)
			{
				tutorialItemTile.setShine(vis: false);
				GameData.instance.PROJECT.character.tutorial.SetState(29);
				GameData.instance.tutorialManager.ShowTutorialForButton(tutorialItemTile.gameObject, new TutorialPopUpSettings(Tutorial.GetText(29), 4, tutorialItemTile.gameObject), stageTrigger: false, null, funcSameAsTargetFunc: false, null, shadow: true, tween: true);
			}
		}
	}

	public ShopItemTile GetTutorialItemTile()
	{
		foreach (GameObject tile in _tiles)
		{
			if (tile.GetComponent<ShopItemTile>() != null)
			{
				ShopItemTile component = tile.GetComponent<ShopItemTile>();
				if (component.itemRef == VariableBook.tutorialShopItem)
				{
					return component;
				}
			}
		}
		return null;
	}

	private void ClearTiles()
	{
		if (_tiles == null)
		{
			return;
		}
		foreach (GameObject tile in _tiles)
		{
			Object.Destroy(tile);
		}
		_tiles.Clear();
	}

	public void CreateTiles()
	{
		ClearTiles();
		if (_tiles == null)
		{
			_tiles = new List<GameObject>();
		}
		List<ItemRef> list = new List<ItemRef>();
		if (_tabRef == null)
		{
			_shopPromoTile = _shopPanelList.shopPromoTile;
			if (_shopPromoTile != null)
			{
				_shopPromoTile.LoadDetails(_shopWindow);
			}
			_shopRotationTile = _shopPanelList.shopRotationTile;
			if (_shopRotationTile != null)
			{
				_shopRotationTile.LoadDetails(_shopWindow);
			}
			for (int i = 0; i < ShopBook.salesSize; i++)
			{
				ShopSaleRef shopSaleRef = ShopBook.LookupSale(i);
				if (shopSaleRef.getActive(_shopWindow.debugDate) || AppInfo.TESTING)
				{
					list.Add(shopSaleRef.itemRef);
				}
			}
		}
		else
		{
			list = _tabRef.items;
			if (tabRef.id == 2)
			{
				for (int j = 0; j < ShopBook.progressionOfferSize; j++)
				{
					ShopSaleRef shopSaleRef2 = ShopBook.LookupProgressionOffer(j);
					ConsumableRef consumableRef = shopSaleRef2.itemRef as ConsumableRef;
					if (!(consumableRef == null) && consumableRef.eventRequired != 0 && (consumableRef.eventRequired == 0 || GameData.instance.PROJECT.character.eventsWon.Contains(consumableRef.eventRequired)) && !list.Contains(shopSaleRef2.itemRef))
					{
						list.Add(shopSaleRef2.itemRef);
					}
				}
				foreach (BoosterRef passiveBooster in GameData.instance.PROJECT.character.passiveBoosters)
				{
					foreach (ItemData cosmetic in passiveBooster.cosmetics)
					{
						if (cosmetic != null && !(cosmetic.itemRef == null) && !list.Contains(cosmetic.itemRef))
						{
							list.Add(cosmetic.itemRef);
						}
					}
				}
			}
		}
		foreach (ItemRef item in list)
		{
			if (item.getPurchasable())
			{
				Transform obj = Object.Instantiate(shopItemTilePrefab);
				obj.SetParent(_shopPanelList.gridContent.transform, worldPositionStays: false);
				ShopItemTile component = obj.GetComponent<ShopItemTile>();
				component.Init(item, _shopWindow);
				_tiles.Add(component.gameObject);
			}
		}
		asianLangManager = base.gameObject.GetComponent<AsianLanguageFontManager>();
		if (asianLangManager == null)
		{
			asianLangManager = base.gameObject.AddComponent<AsianLanguageFontManager>();
		}
		if (asianLangManager != null)
		{
			asianLangManager.SetAsianFontsIfNeeded();
		}
	}

	public void DoUpdate()
	{
		if (!base.gameObject.activeSelf)
		{
			return;
		}
		if (_shopPromoTile != null)
		{
			_shopPromoTile.DoUpdate();
		}
		if (_shopRotationTile != null)
		{
			_shopRotationTile.DoUpdate();
		}
		foreach (GameObject tile in _tiles)
		{
			if (tile.GetComponent<ShopItemTile>() != null)
			{
				tile.GetComponent<ShopItemTile>().DoUpdate();
			}
		}
	}

	public void DoShow()
	{
		base.gameObject.SetActive(value: true);
	}

	public void DoHide()
	{
		base.gameObject.SetActive(value: false);
	}
}
