using com.ultrabit.bitheroes.extensions;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.ui.language;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingWindow : MonoBehaviour
{
	public TextMeshProUGUI nameTxt;

	public TextMeshProUGUI descTxt;

	public GameObject loadingAnimation;

	public Image background;

	private bool _visible;

	private AsianLanguageFontManager asianLangManager;

	private const float bgAlpha = 0.75f;

	private const bool timeout = true;

	public bool visible => _visible;

	public void ShowLoading(bool visible)
	{
		_visible = visible;
		base.gameObject.SetActive(visible);
		if (visible)
		{
			TweenBg();
		}
	}

	public void ShowPercentage(bool show)
	{
		descTxt.gameObject.SetActive(show);
	}

	public void SetText(string text, bool showLoadingAnimation = true, string name = null)
	{
		nameTxt.text = ((name == null) ? Language.GetString("ui_loading") : name);
		if (text != null && text.Length > 0)
		{
			descTxt.gameObject.SetActive(value: false);
			descTxt.text = text;
			descTxt.gameObject.SetActive(value: true);
		}
		else
		{
			descTxt.gameObject.SetActive(value: false);
		}
		loadingAnimation.SetActive(showLoadingAnimation);
		asianLangManager = base.gameObject.GetComponent<AsianLanguageFontManager>();
		if (asianLangManager != null)
		{
			asianLangManager.SetAsianFontsIfNeeded();
		}
	}

	private void TweenBg()
	{
		if (!(background == null))
		{
			background.DOColor(new Color(0f, 0f, 0f, 0.75f), 1f);
		}
	}

	private void OnDestroy()
	{
		ServerExtension.instance.stopTimeoutTimer();
	}
}
