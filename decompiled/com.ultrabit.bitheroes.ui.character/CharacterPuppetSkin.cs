using UnityEngine;

namespace com.ultrabit.bitheroes.ui.character;

public class CharacterPuppetSkin : MonoBehaviour
{
	public Sprite[] skins;

	private int _skinID;

	public int skinID => _skinID;

	public void SetSkinColor(int idx)
	{
		_skinID = idx;
		if (_skinID < 0)
		{
			_skinID = 0;
		}
		if (skins != null && skins.Length != 0)
		{
			SpriteRenderer component = GetComponent<SpriteRenderer>();
			if (component != null)
			{
				component.sprite = skins[_skinID];
			}
		}
		else
		{
			if (base.transform.childCount <= 0)
			{
				return;
			}
			for (int i = 0; i < base.transform.childCount; i++)
			{
				GameObject gameObject = base.transform.GetChild(i).gameObject;
				if (i == _skinID - 1)
				{
					gameObject.SetActive(value: true);
				}
				else
				{
					gameObject.SetActive(value: false);
				}
			}
		}
	}

	public void ChangeSortingOrder(int layer)
	{
		if (skins != null && skins.Length != 0)
		{
			SpriteRenderer component = GetComponent<SpriteRenderer>();
			if (component != null)
			{
				component.sortingOrder = layer;
			}
		}
		else
		{
			if (base.transform.childCount <= 0)
			{
				return;
			}
			for (int i = 0; i < base.transform.childCount; i++)
			{
				SpriteRenderer component2 = base.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>();
				if (component2 != null)
				{
					component2.sortingOrder = layer;
				}
			}
		}
	}
}
