using System.Collections.Generic;
using com.ultrabit.bitheroes.model.rarity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.avatar;

public class AvatarGenerationBanner : MonoBehaviour
{
	public enum Frame
	{
		orange,
		red
	}

	public enum Ornament
	{
		none,
		sword
	}

	[SerializeField]
	private RectTransform bannerBase;

	[SerializeField]
	private RectTransform framesContainer;

	[SerializeField]
	private RectTransform ornamentsContainer;

	[SerializeField]
	private TextMeshProUGUI generationNumber;

	[SerializeField]
	private SpriteMask spriteMask;

	private Dictionary<Frame, RectTransform> frames;

	[Header("Frames")]
	[SerializeField]
	private RectTransform redFrame;

	private Dictionary<Ornament, RectTransform> ornaments;

	[Header("Ornaments")]
	[SerializeField]
	private RectTransform swordOrnament;

	private bool _isFreezed;

	public void LoadDetails(string generation, string rarity, bool isFreezed = false)
	{
		int.TryParse(generation, out var result);
		RarityRef rarity2 = RarityBook.Lookup(rarity);
		LoadDetails(result, rarity2, isFreezed);
	}

	public void LoadDetails(int generation, RarityRef rarity, bool isFreezed = false)
	{
		_isFreezed = isFreezed;
		Initialize();
		SetGeneration(generation);
		SetRarity(rarity);
	}

	private void Initialize()
	{
		frames = new Dictionary<Frame, RectTransform>
		{
			{
				Frame.orange,
				null
			},
			{
				Frame.red,
				redFrame
			}
		};
		ornaments = new Dictionary<Ornament, RectTransform>
		{
			{
				Ornament.none,
				null
			},
			{
				Ornament.sword,
				swordOrnament
			}
		};
	}

	public void SetSpriteMaskRange(int front, int back)
	{
		spriteMask.gameObject.SetActive(value: true);
		spriteMask.isCustomRangeActive = true;
		SpriteMask obj = spriteMask;
		int frontSortingLayerID = (spriteMask.backSortingLayerID = SortingLayer.NameToID("UI"));
		obj.frontSortingLayerID = frontSortingLayerID;
		spriteMask.frontSortingOrder = front;
		spriteMask.backSortingOrder = back;
	}

	private void SetGeneration(int generation)
	{
		generationNumber.SetText(generation.ToString());
	}

	private void SetRarity(RarityRef rarity)
	{
		SetOrnament(rarity);
		SetFrame(rarity);
	}

	private void SetFrame(RarityRef rarity)
	{
		Frame frame = ((rarity.id == 6) ? Frame.red : Frame.orange);
		foreach (KeyValuePair<Frame, RectTransform> frame2 in frames)
		{
			if (frame2.Key == frame)
			{
				framesContainer.gameObject.SetActive(frame2.Value != null);
			}
			if (frame2.Value != null)
			{
				SetGameObjectValue(frame2.Value.gameObject, frame2.Key == frame);
			}
		}
	}

	private void SetGameObjectValue(GameObject item, bool status)
	{
		item.SetActive(status);
		if (_isFreezed && status)
		{
			Image component = item.GetComponent<Image>();
			if (component != null)
			{
				Material material = Object.Instantiate(component.material);
				material.shader = Shader.Find("UI/Grayscale");
				component.material = material;
			}
		}
	}

	private void SetOrnament(RarityRef rarity)
	{
		Ornament ornament = ((rarity.id == 6) ? Ornament.sword : Ornament.none);
		foreach (KeyValuePair<Ornament, RectTransform> ornament2 in ornaments)
		{
			if (ornament2.Key == ornament)
			{
				ornamentsContainer.gameObject.SetActive(ornament2.Value != null);
			}
			if (ornament2.Value != null)
			{
				SetGameObjectValue(ornament2.Value.gameObject, ornament2.Key == ornament);
			}
		}
	}
}
