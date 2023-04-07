using System.Collections.Generic;
using com.ultrabit.bitheroes.model.utility;
using TMPro;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.language;

public class AsianLanguageFontManager : MonoBehaviour
{
	private bool isResized;

	public void SetAsianFontsIfNeeded(bool debug = false)
	{
		if (Util.asianLangManager != null && Util.asianLangManager.isAsian)
		{
			TextMeshProUGUI[] componentsInChildren = base.transform.GetComponentsInChildren<TextMeshProUGUI>(includeInactive: true);
			TextMeshPro[] componentsInChildren2 = base.transform.GetComponentsInChildren<TextMeshPro>(includeInactive: true);
			List<TMP_Text> list = new List<TMP_Text>();
			TextMeshProUGUI[] array = componentsInChildren;
			foreach (TMP_Text item in array)
			{
				list.Add(item);
			}
			TextMeshPro[] array2 = componentsInChildren2;
			foreach (TMP_Text item2 in array2)
			{
				list.Add(item2);
			}
			SetTMP(list);
		}
	}

	private void SetTMP(List<TMP_Text> components)
	{
		for (int i = 0; i < components.Count; i++)
		{
			if (components[i] != null && components[i].font != null && !components[i].font.name.Contains("amiga4ever") && !isResized)
			{
				if (Util.asianLangManager.lang != "ko" && Util.asianLangManager.lang != "ja")
				{
					components[i].fontSize += 3f;
				}
				else
				{
					components[i].fontSize -= 3f;
					components[i].characterSpacing = 3f;
				}
			}
			components[i].font = Util.asianLangManager.GetAsianFont();
			components[i].fontStyle = FontStyles.Normal;
		}
		isResized = true;
	}
}
