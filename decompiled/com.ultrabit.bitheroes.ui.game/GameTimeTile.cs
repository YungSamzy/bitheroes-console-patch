using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.consumable;
using com.ultrabit.bitheroes.ui.utility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.game;

public class GameTimeTile : MonoBehaviour
{
	public Image timeBar;

	public Image asset;

	public Image loadingIcon;

	public Gradient colorGradient;

	private Button tileButton;

	private float _perc;

	[HideInInspector]
	public UnityEvent CHANGE = new UnityEvent();

	public virtual void LoadDetails(bool clickable = true)
	{
		if (clickable)
		{
			tileButton = GetComponent<Button>();
			tileButton.onClick.AddListener(OnTileClick);
			tileButton.gameObject.AddComponent<HoverImages>();
		}
	}

	public virtual void LoadDetails(ConsumableModifierData consumableData, bool clickable = true)
	{
	}

	public virtual void OnTileClick()
	{
		GameData.instance.audioManager.PlaySoundLink("buttonclick");
	}

	public void SetTime(double currentMilliseconds, double totalMilliseconds)
	{
		timeBar.GetComponent<RegularBarFill>().UpdateBar(currentMilliseconds, totalMilliseconds);
		float num = (float)(currentMilliseconds / totalMilliseconds);
		if (num < 0f)
		{
			num = 0f;
		}
		if (num > 1f)
		{
			num = 1f;
		}
		_perc = num;
		ReAnimateBar();
	}

	private void ReAnimateBar()
	{
		timeBar.color = colorGradient.Evaluate(1f - _perc);
	}

	public void SetAsset(Sprite sprite)
	{
		asset.enabled = true;
		asset.overrideSprite = sprite;
		loadingIcon.gameObject.SetActive(value: false);
	}

	public void SetAsset(GameObject prefab)
	{
		RectTransform obj = (RectTransform)Object.Instantiate(prefab, asset.transform.parent).transform;
		obj.name = asset.name;
		obj.SetSiblingIndex(asset.rectTransform.GetSiblingIndex());
		obj.anchorMin = asset.rectTransform.anchorMin;
		obj.anchorMax = asset.rectTransform.anchorMax;
		obj.anchoredPosition = asset.rectTransform.anchoredPosition;
		obj.sizeDelta = asset.rectTransform.sizeDelta;
		Object.Destroy(asset.gameObject);
	}

	public virtual void DoUpdate()
	{
	}

	private void OnDestroy()
	{
		if (tileButton != null)
		{
			tileButton.onClick.RemoveListener(OnTileClick);
		}
	}

	private void OnEnable()
	{
		ReAnimateBar();
	}
}
