using com.ultrabit.bitheroes.model.character;
using com.ultrabit.bitheroes.model.language;
using TMPro;
using UnityEngine;

public class HeroStats : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI rarityTxt;

	[SerializeField]
	private TextMeshProUGUI heroTypeTxt;

	[SerializeField]
	private TextMeshProUGUI generationTxt;

	[SerializeField]
	private TextMeshProUGUI levelTierTxt;

	[SerializeField]
	private TextMeshProUGUI powerTxt;

	[SerializeField]
	private TextMeshProUGUI staminaTxt;

	[SerializeField]
	private TextMeshProUGUI agilitylTxt;

	private CharacterData _characterData;

	public void SetStats(CharacterData data)
	{
		if (data == null)
		{
			SetBasicHero(empty: true);
			return;
		}
		_characterData = data;
		if (!data.isIMXG0)
		{
			SetBasicHero(empty: false);
		}
		else
		{
			SetIMXHero();
		}
	}

	private void SetBasicHero(bool empty)
	{
		generationTxt.gameObject.SetActive(value: false);
		rarityTxt.gameObject.SetActive(empty);
		rarityTxt.text = Language.GetString("ui_new");
		heroTypeTxt.text = Language.GetString("ui_basic_hero");
		SetCommonData(empty);
	}

	private void SetIMXHero()
	{
		generationTxt.gameObject.SetActive(value: true);
		generationTxt.text = Language.GetString("ui_generation_count", new string[1] { _characterData.nftGeneration.ToString() });
		heroTypeTxt.text = Language.GetString("ui_bitverse_hero");
		rarityTxt.gameObject.SetActive(value: true);
		rarityTxt.text = _characterData.nftRarity.coloredName;
		SetCommonData(_characterData.nftState == Character.NFTState.bitverseAvatar);
	}

	private void SetCommonData(bool empty)
	{
		string text = Language.GetString("ui_level_count", new string[1] { empty ? "1" : _characterData.level.ToString() }) + "\n";
		text += Language.GetString("ui_tier_count", new string[1] { empty ? "1" : _characterData.tier.ToString() });
		levelTierTxt.text = text;
		powerTxt.text = (empty ? "6" : _characterData.totalPower.ToString());
		staminaTxt.text = (empty ? "6" : _characterData.totalStamina.ToString());
		agilitylTxt.text = (empty ? "6" : _characterData.totalAgility.ToString());
		Transform[] array = new Transform[3]
		{
			powerTxt.transform.parent,
			staminaTxt.transform.parent,
			agilitylTxt.transform.parent
		};
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(value: true);
		}
	}
}
