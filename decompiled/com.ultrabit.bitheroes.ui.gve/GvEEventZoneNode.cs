using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.model.gve;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.utility;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.gve;

public class GvEEventZoneNode : MonoBehaviour
{
	public GameObject progressBar;

	public Image progressBarBg;

	public Image progressBarBorder;

	public Image placeholderAsset;

	public Button nodeButton;

	public Gradient colorGradient;

	private GvEEventRef _eventRef;

	private GvEZoneNodeRef _nodeRef;

	private GvEEventZoneWindow _gvEEventZoneWindow;

	private GvEEventZoneData _zoneData;

	private HoverImages hoverImages;

	private Image progressBarImage;

	private bool _assetLoaded;

	public GvEZoneNodeRef zoneNodeRef => _nodeRef;

	public bool assetLoaded => _assetLoaded;

	public void LoadDetails(GvEEventRef eventRef, GvEZoneNodeRef nodeRef, GvEEventZoneWindow gvEEventZoneWindow)
	{
		_eventRef = eventRef;
		_nodeRef = nodeRef;
		_gvEEventZoneWindow = gvEEventZoneWindow;
		placeholderAsset.GetComponent<Image>().overrideSprite = GameData.instance.main.assetLoader.GetSpriteAsset(AssetURL.ZONE_NODE, _nodeRef.asset);
		nodeButton = GetComponent<Button>();
		hoverImages = GetComponent<HoverImages>();
		hoverImages.ForceStart();
		progressBarImage = progressBar.GetComponent<Image>();
		DoUpdate();
	}

	public void OnNodeClick()
	{
		ShowNodeWindow();
	}

	public void SetZoneData(GvEEventZoneData zoneData)
	{
		_zoneData = zoneData;
		DoUpdate();
	}

	private void DoUpdate()
	{
		if (_zoneData == null)
		{
			if (base.gameObject != null)
			{
				base.gameObject.SetActive(value: false);
			}
		}
		else
		{
			if (zoneNodeRef == null || _nodeRef == null)
			{
				return;
			}
			bool flag = _zoneData.nodeIsUnlocked(zoneNodeRef);
			bool flag2 = _zoneData.nodeIsCompleted(zoneNodeRef);
			if (_nodeRef.hidden)
			{
				if (base.gameObject != null)
				{
					base.gameObject.SetActive(flag2 || flag);
				}
			}
			else if (base.gameObject != null)
			{
				base.gameObject.SetActive(value: true);
			}
			if (!base.gameObject.activeSelf)
			{
				return;
			}
			if (!flag)
			{
				if (nodeButton != null)
				{
					nodeButton.enabled = false;
				}
				if (placeholderAsset != null)
				{
					Util.SetImageAlpha(placeholderAsset, alpha: true);
				}
				if (progressBarImage != null)
				{
					Util.SetImageAlpha(progressBarImage, alpha: true);
				}
				if (progressBarBorder != null)
				{
					Util.SetImageAlpha(progressBarBorder, alpha: true);
				}
				if (progressBarBg != null)
				{
					Util.SetImageAlpha(progressBarBg, alpha: true);
				}
				if (hoverImages != null)
				{
					hoverImages.OnExit();
					hoverImages.enabled = false;
				}
			}
			else
			{
				if (nodeButton != null)
				{
					nodeButton.enabled = true;
				}
				if (placeholderAsset != null)
				{
					Util.SetImageAlpha(placeholderAsset, alpha: false);
				}
				if (progressBarImage != null)
				{
					Util.SetImageAlpha(progressBarImage, alpha: false);
				}
				if (progressBarBorder != null)
				{
					Util.SetImageAlpha(progressBarBorder, alpha: false);
				}
				if (progressBarBg != null)
				{
					Util.SetImageAlpha(progressBarBg, alpha: false);
				}
				if (hoverImages != null)
				{
					hoverImages.enabled = true;
				}
			}
			UpdateProgressBar();
		}
	}

	public void ShowNodeWindow()
	{
		if (nodeButton.enabled && nodeButton.interactable && _zoneData.nodeIsUnlocked(zoneNodeRef))
		{
			GameData.instance.audioManager.PlaySoundLink("buttonclick");
			GameData.instance.windowGenerator.NewGvEEventZoneNodeWindow(_eventRef, _nodeRef, _gvEEventZoneWindow.gameObject);
		}
	}

	private void OnNodeAssetLoaded()
	{
		_assetLoaded = true;
		_gvEEventZoneWindow.CheckLoaded();
	}

	private void UpdateProgressBar()
	{
		if (_zoneData == null)
		{
			progressBar.gameObject.SetActive(value: false);
			return;
		}
		float num = Mathf.Clamp01((float)_zoneData.getNodePoints(_nodeRef.nodeID) / (float)_nodeRef.points);
		progressBarImage.fillAmount = num;
		progressBarImage.color = colorGradient.Evaluate(1f - num);
		progressBar.gameObject.SetActive(num > 0f);
	}
}
