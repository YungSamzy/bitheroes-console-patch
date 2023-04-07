using UnityEngine;

namespace com.ultrabit.bitheroes.ui.lists.servicelist;

public class ServiceListItemModel
{
	public int id;

	public string name;

	public string description;

	public int cost;

	public int currencyID;

	public bool highlighted;

	public bool canGlow = true;

	public Sprite icon;
}
