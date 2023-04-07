using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.language;

public class AsianLanguageManager
{
	public const string LANG_CHINESE = "zh";

	public const string LANG_JAPANESE = "ja";

	public const string LANG_KOREAN = "ko";

	public const string LANG_THAI = "th";

	public const string LANG_RUSSIAN = "ru";

	private string[] languajes = new string[5] { "zh", "ja", "ko", "th", "ru" };

	private TMP_FontAsset useFont;

	private string _lang;

	private bool _isAsian;

	public bool isAsian => _isAsian;

	public string lang => _lang;

	public AsianLanguageManager(string lang)
	{
		_lang = lang;
		if (IsAsianLang(_lang))
		{
			useFont = GetAsianFont(_lang);
			_isAsian = useFont != null;
		}
	}

	public TMP_FontAsset GetAsianFont()
	{
		return useFont;
	}

	public TMP_FontAsset GetAsianFont(string lang)
	{
		string text = "";
		text = lang switch
		{
			"zh" => "fonts/yaheiSDF", 
			"ja" => "fonts/jackeySDF", 
			"ko" => "fonts/blackHanSans", 
			"th" => "fonts/tahomaSDF", 
			"ru" => "fonts/editundoRussianSDF", 
			_ => "fonts/tahomaSDF", 
		};
		TMP_FontAsset tMP_FontAsset = Resources.Load<TMP_FontAsset>(text);
		if (tMP_FontAsset == null)
		{
			Object @object = SingletonMonoBehaviour<AssetManager>.instance.LoadAsset(text, null, typeof(TMP_FontAsset));
			if (@object != null)
			{
				tMP_FontAsset = (TMP_FontAsset)@object;
			}
		}
		return tMP_FontAsset;
	}

	public bool IsAsianLang(string lang)
	{
		for (int i = 0; i < languajes.Length; i++)
		{
			if (languajes[i].Equals(lang))
			{
				return true;
			}
		}
		return false;
	}
}
