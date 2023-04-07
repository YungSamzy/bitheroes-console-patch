using UnityEngine;

namespace com.ultrabit.bitheroes.ui.character;

public class CharacterPuppetHead : MonoBehaviour
{
	private int _skinColor;

	private int _hairColor;

	private int _hairID;

	private float _scale;

	private CharacterPuppet.Gender _genre;

	private GameObject overlay;

	public Transform[] hairs;

	public Transform[] skins;

	public int skinColor
	{
		get
		{
			return _skinColor;
		}
		set
		{
			_skinColor = value;
			UpdateSkinColor();
		}
	}

	public int hairColor
	{
		get
		{
			return _hairColor;
		}
		set
		{
			_hairColor = value;
			UpdateHairColor();
		}
	}

	public int hairID
	{
		get
		{
			return _hairID;
		}
		set
		{
			_hairID = value;
			UpdateHairID();
		}
	}

	public float scale
	{
		get
		{
			return _scale;
		}
		set
		{
			_scale = value;
			Vector3 localScale = base.transform.localScale;
			localScale *= scale;
			base.transform.localScale = localScale;
		}
	}

	public CharacterPuppet.Gender genre => _genre;

	public Transform activeSkin => base.transform;

	public void UpdateHairColor()
	{
		for (int i = 0; i < hairs.Length; i++)
		{
			Color color = default(Color);
			if (_hairColor >= CharacterPuppetDefault.HAIR_COLORS.Length)
			{
				_hairColor = CharacterPuppetDefault.HAIR_COLORS.Length - 1;
			}
			ColorUtility.TryParseHtmlString(CharacterPuppetDefault.HAIR_COLORS[_hairColor], out color);
			for (int j = 0; j < hairs[i].childCount; j++)
			{
				hairs[i].GetChild(j).GetComponent<SpriteRenderer>().color = color;
			}
		}
	}

	private void UpdateHairID()
	{
		for (int i = 0; i < hairs.Length; i++)
		{
			if (hairs[i].TryGetComponent<CharacterPuppetSkin>(out var component))
			{
				component.SetSkinColor(_hairID);
			}
		}
	}

	private void UpdateSkinColor()
	{
		for (int i = 0; i < skins.Length; i++)
		{
			if (skins[i].TryGetComponent<CharacterPuppetSkin>(out var component))
			{
				component.SetSkinColor(_skinColor);
			}
		}
	}

	public void ChangeHairSortingOrder(int order)
	{
		for (int i = 0; i < hairs.Length; i++)
		{
			if (hairs[i].TryGetComponent<CharacterPuppetSkin>(out var component))
			{
				component.ChangeSortingOrder(order);
			}
		}
	}

	public void ShowSkin(bool show)
	{
		if (_genre == CharacterPuppet.Gender.MALE)
		{
			skins[0].gameObject.SetActive(show);
		}
		else
		{
			skins[1].gameObject.SetActive(show);
		}
		overlay = base.transform.Find(CharacterPuppetDefault.BP_HEAD_EYESMOUTH).gameObject;
		overlay.SetActive(show);
	}

	public void ShowHair(bool show)
	{
		if (_genre == CharacterPuppet.Gender.MALE)
		{
			hairs[0].gameObject.SetActive(show);
		}
		else
		{
			hairs[1].gameObject.SetActive(show);
		}
	}

	public void SetGenre(CharacterPuppet.Gender pGenre)
	{
		_genre = pGenre;
		skins[0].gameObject.SetActive(_genre == CharacterPuppet.Gender.MALE);
		hairs[0].gameObject.SetActive(_genre == CharacterPuppet.Gender.MALE);
		skins[1].gameObject.SetActive(_genre != CharacterPuppet.Gender.MALE);
		hairs[1].gameObject.SetActive(_genre != CharacterPuppet.Gender.MALE);
	}

	public int getHighestSortingOrder()
	{
		return hairs[0].GetChild(0).GetComponent<SpriteRenderer>().sortingOrder + 1;
	}
}
