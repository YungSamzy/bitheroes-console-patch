using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.fromflash;
using com.ultrabit.bitheroes.model.ability;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.augment;
using com.ultrabit.bitheroes.model.battle;
using com.ultrabit.bitheroes.model.daily;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.familiar;
using com.ultrabit.bitheroes.model.game;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.rarity;
using com.ultrabit.bitheroes.model.rune;
using com.ultrabit.bitheroes.ui.utility;
using DG.Tweening;
using Sfs2X.Util;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.model.utility;

public class Util
{
	public const long DAY_SECONDS = 86400L;

	public const long DAY_MILLISECONDS = 86400000L;

	public const float TWEEN_POSITION_OFFSET = 20f;

	public const float TWEEN_SCALE_OFFSET = 0.15f;

	public const float TWEEN_DURATION = 0.4f;

	private const int DECIMALS = 2;

	private const int THOUSAND = 1000;

	private const int MILLION = 1000000;

	private const int BILLION = 1000000000;

	private const string LETTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzÂÃÄÀÁÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõöøùúûüýþÿАОУИЕЁЭЮЯЫБВГДЖЗЙКЛМНПРСТФХЦЧШЩЬЪаоуиеёэюяыбвгджзйклмнпрстфхцчшщьъ";

	private const string NUMBERS = "0123456789";

	private const string DATE_FORMATTER = "MM/dd/yyyy hh:mm tt";

	public const int POSITION_TOP_LEFT = 0;

	public const int POSITION_TOP_CENTER = 1;

	public const int POSITION_TOP_RIGHT = 2;

	public const int POSITION_CENTER_RIGHT = 3;

	public const int POSITION_BOTTOM_RIGHT = 4;

	public const int POSITION_BOTTOM_CENTER = 5;

	public const int POSITION_BOTTOM_LEFT = 6;

	public const int POSITION_LEFT_CENTER = 7;

	public const int POSITION_CENTER = 8;

	public const string COLOR_HIGH = "#00FF00";

	public const string COLOR_MEDIUM = "#FFFFFF";

	public const string COLOR_LOW = "#FF0000";

	public const string BG_NAME = "GameSpriteBG";

	public const string NAME_RESTRICTIONS = "\\-_ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzÂÃÄÀÁÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõöøùúûüýþÿАОУИЕЁЭЮЯЫБВГДЖЗЙКЛМНПРСТФХЦЧШЩЬЪаоуиеёэюяыбвгджзйклмнпрстфхцчшщьъ0123456789";

	public const string EMAIL_RESTRICTIONS = "\\-_ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzÂÃÄÀÁÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõöøùúûüýþÿАОУИЕЁЭЮЯЫБВГДЖЗЙКЛМНПРСТФХЦЧШЩЬЪаоуиеёэюяыбвгджзйклмнпрстфхцчшщьъ0123456789.@!#$%&'*+-/=?^_`{|}~";

	public const string GUILD_NAME_RESTRICTIONS = "\\-_ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzÂÃÄÀÁÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõöøùúûüýþÿАОУИЕЁЭЮЯЫБВГДЖЗЙКЛМНПРСТФХЦЧШЩЬЪаоуиеёэюяыбвгджзйклмнпрстфхцчшщьъ0123456789 ";

	public const string INITIAL_RESTRICTIONS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzÂÃÄÀÁÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõöøùúûüýþÿАОУИЕЁЭЮЯЫБВГДЖЗЙКЛМНПРСТФХЦЧШЩЬЪаоуиеёэюяыбвгджзйклмнпрстфхцчшщьъ0123456789";

	public const string NUMBER_RESTRICTIONS = "0123456789";

	private static int totalCompressed = 0;

	private static int totalUncompressed = 0;

	public static Color WHITE_ALPHA_0 = new Color(1f, 1f, 1f, 0f);

	public static Color WHITE_ALPHA_50 = new Color(1f, 1f, 1f, 0.5f);

	public static int ARRAY_ASCENDING = 0;

	public static int ARRAY_DESCENDING = 1;

	public static float SEARCHBOX_ACTION_DELAY = 0.5f;

	public static AsianLanguageManager asianLangManager;

	public const string FORBIDDEN_NAME_CHARS = " '.,;:<>[]()|&`\u00b4*¿?¡!\u00a8/@<>#*+$%~{}=€^\"\\";

	public const string FORBIDDEN_GUILDNAME_CHARS = "-_'.,;:<>[]()|&`\u00b4*¿?¡!\u00a8/@<>#*+$%~{}=€^\"\\";

	public const string FORBIDDEN_GUILDINITIALS_CHARS = " -_'.,;:<>[]()|&`\u00b4*¿?¡!\u00a8/@<>#*+$%~{}=€^\"\\";

	public const string FORBIDDEN_EMAIL_CHARS = " ',;:<>[]()|&`\u00b4*¿?¡!\u00a8/<>#*+$%~{}=€^\"\\";

	public const string FORBIDDEN_HEROTAG = " '.,;:<>[]()|&`\u00b4*¿?¡!\u00a8/@<>*+$%~{}=€^\"\\";

	public static readonly UnicodeCategory[] FORBIDDEN_NAME_CHAR_CATEGORIES = new UnicodeCategory[7]
	{
		UnicodeCategory.SpaceSeparator,
		UnicodeCategory.OtherPunctuation,
		UnicodeCategory.MathSymbol,
		UnicodeCategory.OpenPunctuation,
		UnicodeCategory.ClosePunctuation,
		UnicodeCategory.ModifierSymbol,
		UnicodeCategory.CurrencySymbol
	};

	public static readonly UnicodeCategory[] FORBIDDEN_GUILDNAME_CHAR_CATEGORIES = new UnicodeCategory[8]
	{
		UnicodeCategory.DashPunctuation,
		UnicodeCategory.ConnectorPunctuation,
		UnicodeCategory.OtherPunctuation,
		UnicodeCategory.MathSymbol,
		UnicodeCategory.OpenPunctuation,
		UnicodeCategory.ClosePunctuation,
		UnicodeCategory.ModifierSymbol,
		UnicodeCategory.CurrencySymbol
	};

	public static readonly UnicodeCategory[] FORBIDDEN_GUILDINITIALS_CHAR_CATEGORIES = new UnicodeCategory[9]
	{
		UnicodeCategory.SpaceSeparator,
		UnicodeCategory.DashPunctuation,
		UnicodeCategory.ConnectorPunctuation,
		UnicodeCategory.OtherPunctuation,
		UnicodeCategory.MathSymbol,
		UnicodeCategory.OpenPunctuation,
		UnicodeCategory.ClosePunctuation,
		UnicodeCategory.ModifierSymbol,
		UnicodeCategory.CurrencySymbol
	};

	public static readonly UnicodeCategory[] FORBIDDEN_EMAIL_CHAR_CATEGORIES = new UnicodeCategory[7]
	{
		UnicodeCategory.SpaceSeparator,
		UnicodeCategory.OtherPunctuation,
		UnicodeCategory.MathSymbol,
		UnicodeCategory.OpenPunctuation,
		UnicodeCategory.ClosePunctuation,
		UnicodeCategory.ModifierSymbol,
		UnicodeCategory.CurrencySymbol
	};

	public static readonly UnicodeCategory[] FORBIDDEN_HERO_CHAR_CATEGORIES = new UnicodeCategory[7]
	{
		UnicodeCategory.SpaceSeparator,
		UnicodeCategory.OtherPunctuation,
		UnicodeCategory.MathSymbol,
		UnicodeCategory.OpenPunctuation,
		UnicodeCategory.ClosePunctuation,
		UnicodeCategory.ModifierSymbol,
		UnicodeCategory.CurrencySymbol
	};

	private static List<object> _glowobjects = new List<object>();

	private static List<float> _glowbrightness = new List<float>();

	private static List<float> _glowspeed = new List<float>();

	private static List<bool> _glowflipped = new List<bool>();

	public static Color brightnessColor;

	public static string DecompressString(string input)
	{
		using MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(input));
		using MemoryStream memoryStream = new MemoryStream();
		using (GZipStream src = new GZipStream(stream, CompressionMode.Decompress))
		{
			CopyTo(src, memoryStream);
		}
		return Encoding.UTF8.GetString(memoryStream.ToArray());
	}

	public static void CopyTo(Stream src, Stream dest)
	{
		byte[] array = new byte[4096];
		int count;
		while ((count = src.Read(array, 0, array.Length)) != 0)
		{
			dest.Write(array, 0, count);
		}
	}

	public static byte[] Decompress(byte[] input)
	{
		using MemoryStream memoryStream = new MemoryStream(input);
		byte[] array = new byte[4];
		memoryStream.Read(array, 0, 4);
		int num = BitConverter.ToInt32(array, 0);
		using GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress);
		byte[] array2 = new byte[num];
		gZipStream.Read(array2, 0, num);
		return array2;
	}

	public static DateTime GetDateFromString(string date)
	{
		return DateTime.Parse(date, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
	}

	public static string Base64Decode(string content)
	{
		return UncompressString(new ByteArray(Convert.FromBase64String(content)));
	}

	public static string UncompressString(ByteArray bytes)
	{
		try
		{
			bytes.Uncompress();
			return HTMLSpecialChars(Encoding.UTF8.GetString(bytes.ReadBytes(bytes.Length)));
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
		return "";
	}

	public static ByteArray compressString(string theString)
	{
		ByteArray byteArray = new ByteArray();
		byteArray.WriteUTF(theString);
		byteArray.Compress();
		return byteArray;
	}

	public static string bytesToHex(ByteArray thearray, bool colons = false)
	{
		string text = "";
		for (uint num = 0u; num < thearray.Length; num++)
		{
			text += ("0" + thearray.Bytes[num]).Substring(-2, 2);
			if (colons && num < thearray.Length - 1)
			{
				text += ":";
			}
		}
		return text;
	}

	public static Transform RecursiveFindChild(Transform parent, string childName)
	{
		Transform transform = null;
		for (int i = 0; i < parent.childCount; i++)
		{
			transform = parent.GetChild(i);
			if (transform.name == childName)
			{
				break;
			}
			transform = RecursiveFindChild(transform, childName);
			if (transform != null)
			{
				break;
			}
		}
		return transform;
	}

	public static List<GameObject> GetChildsWithTag(Transform parent, string tag)
	{
		List<GameObject> list = new List<GameObject>();
		foreach (Transform item in parent)
		{
			if (item.tag == tag)
			{
				list.Add(item.gameObject);
			}
		}
		return list;
	}

	public static string getIPAddress()
	{
		return "";
	}

	public static string getAddressableNameFromURL(string url)
	{
		string text = "";
		string[] array = url.Split(char.Parse("."));
		for (int i = 0; i < array.Length - 1; i++)
		{
			text += array[i];
		}
		return text;
	}

	public static Vector2 getScreenSize()
	{
		return new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
	}

	public static string getStrippedNameString(string theString)
	{
		if (theString == null || theString.Length <= 0)
		{
			return theString;
		}
		string text = "\\-_ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzÂÃÄÀÁÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõöøùúûüýþÿАОУИЕЁЭЮЯЫБВГДЖЗЙКЛМНПРСТФХЦЧШЩЬЪаоуиеёэюяыбвгджзйклмнпрстфхцчшщьъ0123456789";
		int num = 0;
		while (num < theString.Length)
		{
			string text2 = theString[num].ToString();
			if (text.IndexOf(text2) < 0)
			{
				theString = theString.Replace(text2, "");
			}
			else
			{
				num++;
			}
		}
		return theString;
	}

	public static string getStringBreak(string theString)
	{
		string stringShift = getStringShift(theString, '_');
		if (stringShift != null)
		{
			return stringShift;
		}
		string stringShift2 = getStringShift(theString, '-');
		if (stringShift2 != null)
		{
			return stringShift2;
		}
		return theString;
	}

	private static string getStringShift(string theString, char shift)
	{
		List<string> list = new List<string>();
		for (int num = 0; num < theString.Split(shift).Length; num++)
		{
			list.Add(theString.Split(shift)[num]);
		}
		list.RemoveAt(0);
		theString = "";
		for (int i = 0; i < list.Count; i++)
		{
			theString += list[i];
		}
		if (list.Count > 1)
		{
			return theString;
		}
		return null;
	}

	public static bool multiple(float number, float multiple)
	{
		return number % multiple == 0f;
	}

	public static int getWordCount(string theString)
	{
		return removeExtraWhiteSpace(theString).Split(' ').Length;
	}

	public static string ParseName(string name, string initials = "", bool color = true)
	{
		if (initials == null || initials.Length <= 0)
		{
			return name;
		}
		return ParseGuildInitials(initials, color) + " " + ParseString(name);
	}

	public static string ParseGuildInitials(string initials, bool color = true)
	{
		if (initials == null || initials.Length <= 0)
		{
			return "";
		}
		string text = "[" + initials + "]";
		if (!color)
		{
			return text;
		}
		return ParseString("{" + text + "}");
	}

	public static string ParseModifierString(GameModifier modifier, object data = null)
	{
		string text = modifier.desc;
		if (data is AugmentData)
		{
			AugmentData augmentData = data as AugmentData;
			int power = 0;
			float bonus = 0f;
			if (augmentData.familiarRef != null)
			{
				int totalStats = GameData.instance.PROJECT.character.getTotalStats();
				power = augmentData.familiarRef.getPower(totalStats);
				bonus = GameModifier.getTypeTotal(augmentData.familiarRef.modifiers, 17);
			}
			text = ParseItemString(text, augmentData.augmentRef, power, bonus, color: true, augmentData);
		}
		return ParseString(text);
	}

	public static string ParseString(string text, bool color = true)
	{
		if (text == null)
		{
			return null;
		}
		text = ReplaceStringsWithColor("^", "^", text, color ? "green" : null);
		text = ReplaceStringsWithColor("{", "}", text, color ? "yellow" : null);
		text = ReplaceStringsWithColor("~", "~", text, color ? "#FF0000" : null);
		text = ReplaceStringsWithColor("¬", "¬", text, color ? "#D6A71C" : null);
		text = ReplaceStringsWithColor("\u00b4", "\u00b4", text, color ? "#FE33FF" : null);
		text = ReplaceStringsWithItems(text);
		text = ReplaceStringsWithRarities(text);
		text = ReplaceStringsWithString(text, "!break!", "\n");
		return text;
	}

	public static string parseStringSize(string text, int size)
	{
		return "<size='" + size + "'>" + text + "</size>";
	}

	public static string ParseItemString(string text, ItemRef itemRef, int power, float bonus, bool color = true, object data = null)
	{
		List<BattleTriggerRef> list = new List<BattleTriggerRef>();
		AbilityRef abilityRef = null;
		switch (itemRef.itemType)
		{
		case 1:
		case 16:
		{
			EquipmentRef obj2 = itemRef as EquipmentRef;
			list = GameModifier.getFirstTriggers(obj2.modifiers);
			abilityRef = GameModifier.getFirstAbility(obj2.modifiers);
			break;
		}
		case 9:
		{
			RuneRef runeRef = itemRef as RuneRef;
			abilityRef = GameModifier.getFirstAbility(runeRef.modifiers);
			text = ParseStringValues(text, runeRef.values, color);
			break;
		}
		case 6:
		{
			FamiliarRef obj3 = itemRef as FamiliarRef;
			list = GameModifier.getFirstTriggers(obj3.modifiers);
			abilityRef = GameModifier.getFirstAbility(obj3.modifiers);
			break;
		}
		case 15:
			if (data is AugmentData)
			{
				AugmentData obj = data as AugmentData;
				List<GameModifier> gameModifiers = obj.getGameModifiers(obj.getRank(GameData.instance.PROJECT.character.familiarStable));
				GameModifier firstModifier = GameModifier.getFirstModifier(gameModifiers);
				list = GameModifier.getFirstTriggers(gameModifiers);
				abilityRef = GameModifier.getFirstAbility(gameModifiers);
				if (firstModifier != null && firstModifier.values != null && firstModifier.values.Count > 0)
				{
					text = ParseStringValues(text, firstModifier.values, color);
				}
				text = ParseStringValues(text, GameModifier.getFirstConditionModifierValues(gameModifiers), color);
			}
			break;
		}
		if (list != null && list.Count > 0)
		{
			text = ParseTriggerString(text, list);
		}
		if (abilityRef != null)
		{
			text = ParseAbilityString(text, abilityRef, power, bonus);
		}
		return ParseString(text);
	}

	public static string ParseTriggerString(string text, List<BattleTriggerRef> triggers, bool color = true)
	{
		string text2 = "#";
		for (int i = 0; i < triggers.Count; i++)
		{
			BattleTriggerRef battleTriggerRef = triggers[i];
			string stringSearch = text2 + "perc" + (i + 1) + text2;
			string text3 = battleTriggerRef.perc.ToString();
			string stringReplace = (color ? ("^" + text3 + "^") : text3);
			text = ReplaceStringsWithString(text, stringSearch, stringReplace);
		}
		return ParseString(text);
	}

	public static string ParseAbilityString(string text, AbilityRef abilityRef, int power, float bonus, bool color = true)
	{
		if (abilityRef == null)
		{
			return text;
		}
		bonus = 0f;
		string text2 = "#";
		for (int i = 0; i < abilityRef.actions.Count; i++)
		{
			AbilityActionRef abilityActionRef = abilityRef.actions[i];
			int num = i + 1;
			string stringSearch = text2 + "value" + num + text2;
			Vector2 totalValueRange = abilityActionRef.getTotalValueRange(power, 1f + bonus);
			float num2 = 0f;
			switch (abilityActionRef.getType())
			{
			case 11:
				num2 += GameModifier.getTypeTotal(GameData.instance.PROJECT.character.getModifiers(), 84);
				break;
			case 12:
				num2 += GameModifier.getTypeTotal(GameData.instance.PROJECT.character.getModifiers(), 86);
				break;
			case 13:
				num2 += GameModifier.getTypeTotal(GameData.instance.PROJECT.character.getModifiers(), 88);
				break;
			case 14:
				num2 += GameModifier.getTypeTotal(GameData.instance.PROJECT.character.getModifiers(), 90);
				break;
			case 15:
				num2 += GameModifier.getTypeTotal(GameData.instance.PROJECT.character.getModifiers(), 92);
				break;
			}
			if (num2 > 0f)
			{
				num2 = Mathf.Floor(num2);
			}
			totalValueRange.x += num2;
			totalValueRange.y += num2;
			string text3 = "";
			text3 = ((totalValueRange.x != totalValueRange.y) ? (NumberFormat(totalValueRange.x) + "-" + NumberFormat(totalValueRange.y)) : NumberFormat(totalValueRange.x));
			if (totalValueRange.x == 0f && totalValueRange.y == 0f)
			{
				text3 = "?";
			}
			string stringReplace = (color ? ("^" + text3 + "^") : text3);
			text = ReplaceStringsWithString(text, stringSearch, stringReplace);
			string stringSearch2 = text2 + "pierce" + num + text2;
			string text4 = (abilityActionRef.pierce + 1).ToString();
			string stringReplace2 = (color ? ("^" + text4 + "^") : text4);
			text = ReplaceStringsWithString(text, stringSearch2, stringReplace2);
			string stringSearch3 = text2 + "random" + num + text2;
			string text5 = abilityActionRef.random.ToString();
			string stringReplace3 = (color ? ("^" + text5 + "^") : text5);
			text = ReplaceStringsWithString(text, stringSearch3, stringReplace3);
			string stringSearch4 = text2 + "bounces" + num + text2;
			string text6 = abilityActionRef.bounces.ToString();
			string stringReplace4 = (color ? ("^" + text6 + "^") : text6);
			text = ReplaceStringsWithString(text, stringSearch4, stringReplace4);
		}
		return ParseString(text);
	}

	public static void GetSharedObjects()
	{
		string text = null;
		switch (AppInfo.platform)
		{
		case 1:
			text = "/data/user/0/" + Application.identifier + "/" + Application.identifier + "/Local Store/#SharedObjects/#pixelquest";
			break;
		case 2:
			text = Directory.GetParent(Application.persistentDataPath).ToString() + "/Library/Application Support/" + Application.identifier + "/Local Store/#SharedObjects/#pixelquest";
			break;
		}
		if (!Directory.Exists(text))
		{
			return;
		}
		Directory.GetDirectories(text);
		Directory.GetFiles(text);
		text += "/local.sol";
		if (File.Exists(text))
		{
			SharedObject sharedObject;
			try
			{
				sharedObject = SharedObjectParser.Parse(text);
			}
			catch (Exception e)
			{
				D.LogException("Problem parsing the .so file", e);
				throw new Exception("Problem parsing the .so file");
			}
			SOValue sOValue = sharedObject.Get("UID");
			if (sOValue.type == 0 || sOValue.string_val == null || sOValue.string_val.Equals(""))
			{
				D.LogError("Abort: UID not found in shared object");
				throw new Exception("Abort: UID not found in shared object");
			}
			GameData.instance.SAVE_STATE.uid = sharedObject.Get("UID").string_val;
			if (sharedObject.Get("SYSTEM").type != 0)
			{
				GameData.instance.SAVE_STATE.system = sharedObject.Get("SYSTEM").string_val;
			}
			if (sharedObject.Get("EMAIL").type != 0)
			{
				GameData.instance.SAVE_STATE.email = sharedObject.Get("EMAIL").string_val;
			}
			if (sharedObject.Get("PASSWORD").type != 0)
			{
				GameData.instance.SAVE_STATE.password = sharedObject.Get("PASSWORD").string_val;
			}
			if (sharedObject.Get("LANGUAGE").type != 0)
			{
				GameData.instance.SAVE_STATE.language = sharedObject.Get("LANGUAGE").string_val;
			}
			if (sharedObject.Get("ADMIN_PASSWORD").type != 0)
			{
				GameData.instance.SAVE_STATE.adminPassword = sharedObject.Get("ADMIN_PASSWORD").string_val;
			}
			if (sharedObject.Get("MUSIC_VOLUME").type != 0)
			{
				GameData.instance.SAVE_STATE.musicVolume = (float)sharedObject.Get("MUSIC_VOLUME").double_val;
			}
			if (sharedObject.Get("SOUND_VOLUME").type != 0)
			{
				GameData.instance.SAVE_STATE.soundVolume = (float)sharedObject.Get("SOUND_VOLUME").double_val;
			}
			if (sharedObject.Get("CHARACTER_ID").type != 0)
			{
				GameData.instance.SAVE_STATE.characterID = sharedObject.Get("CHARACTER_ID").int_val;
			}
			if (sharedObject.Get("CHARACTER_GUILD_ID").type != 0)
			{
				GameData.instance.SAVE_STATE.characterGuildID = sharedObject.Get("CHARACTER_GUILD_ID").int_val;
			}
			if (sharedObject.Get("CHARACTER_GOLD").type != 0)
			{
				GameData.instance.SAVE_STATE.characterGold = sharedObject.Get("CHARACTER_GOLD").int_val;
			}
			if (sharedObject.Get("CHARACTER_CREDITS").type != 0)
			{
				GameData.instance.SAVE_STATE.characterCredits = sharedObject.Get("CHARACTER_CREDITS").int_val;
			}
			if (sharedObject.Get("CHARACTER_LEVEL").type != 0)
			{
				GameData.instance.SAVE_STATE.characterLevel = sharedObject.Get("CHARACTER_LEVEL").int_val;
			}
			if (sharedObject.Get("CHARACTER_ENERGY").type != 0)
			{
				GameData.instance.SAVE_STATE.characterEnergy = sharedObject.Get("CHARACTER_ENERGY").int_val;
			}
			if (sharedObject.Get("CHARACTER_ENERGY_MAX").type != 0)
			{
				GameData.instance.SAVE_STATE.characterEnergyMax = sharedObject.Get("CHARACTER_ENERGY_MAX").int_val;
			}
			if (sharedObject.Get("CHARACTER_TICKETS").type != 0)
			{
				GameData.instance.SAVE_STATE.characterTickets = sharedObject.Get("CHARACTER_TICKETS").int_val;
			}
			if (sharedObject.Get("CHARACTER_TICKETS_MAX").type != 0)
			{
				GameData.instance.SAVE_STATE.characterTicketsMax = sharedObject.Get("CHARACTER_TICKETS_MAX").int_val;
			}
			if (sharedObject.Get("CHARACTER_TOTAL_STATS").type != 0)
			{
				GameData.instance.SAVE_STATE.characterTotalStats = sharedObject.Get("CHARACTER_TOTAL_STATS").int_val;
			}
			if (sharedObject.Get("CHARACTER_TOTAL_POWER").type != 0)
			{
				GameData.instance.SAVE_STATE.characterTotalPower = sharedObject.Get("CHARACTER_TOTAL_POWER").int_val;
			}
			if (sharedObject.Get("CHARACTER_TOTAL_STAMINA").type != 0)
			{
				GameData.instance.SAVE_STATE.characterTotalStamina = sharedObject.Get("CHARACTER_TOTAL_STAMINA").int_val;
			}
			if (sharedObject.Get("CHARACTER_TOTAL_AGILITY").type != 0)
			{
				GameData.instance.SAVE_STATE.characterTotalAgility = sharedObject.Get("CHARACTER_TOTAL_AGILITY").int_val;
			}
			if (sharedObject.Get("CHARACTER_NAME").type != 0)
			{
				GameData.instance.SAVE_STATE.characterName = sharedObject.Get("CHARACTER_NAME").string_val;
			}
			if (sharedObject.Get("BATTLE_SPEED").type != 0)
			{
				GameData.instance.SAVE_STATE.SetBattleSpeed(GameData.instance.SAVE_STATE.characterID, sharedObject.Get("BATTLE_SPEED").int_val);
			}
			if (sharedObject.Get("BATTLE_TEXT").type != 0)
			{
				GameData.instance.SAVE_STATE.battleText = sharedObject.Get("BATTLE_TEXT").bool_val;
			}
			if (sharedObject.Get("BATTLE_BAR_OVERLAY").type != 0)
			{
				GameData.instance.SAVE_STATE.battleBarOverlay = sharedObject.Get("BATTLE_BAR_OVERLAY").bool_val;
			}
			if (sharedObject.Get("DROPDOWN_INDEX_INVENTORY").type != 0)
			{
				GameData.instance.SAVE_STATE.SetDropdownIndexInventory(GameData.instance.SAVE_STATE.characterID, sharedObject.Get("DROPDOWN_INDEX_INVENTORY").int_val);
			}
			if (sharedObject.Get("SERVER_ID").type != 0)
			{
				GameData.instance.SAVE_STATE.serverID = sharedObject.Get("SERVER_ID").int_val;
			}
			if (sharedObject.Get("NEWS_VERSION").type != 0)
			{
				GameData.instance.SAVE_STATE.newsVersion = sharedObject.Get("NEWS_VERSION").string_val;
			}
			if (sharedObject.Get("NOTIFICATIONS_DISABLED").type != 0)
			{
				GameData.instance.SAVE_STATE.notificationsDisabled = sharedObject.Get("NOTIFICATIONS_DISABLED").bool_val;
			}
			if (sharedObject.Get("NOTIFICATIONS_FRIEND").type != 0)
			{
				GameData.instance.SAVE_STATE.notificationsFriend = sharedObject.Get("NOTIFICATIONS_FRIEND").bool_val;
			}
			if (sharedObject.Get("NOTIFICATIONS_GUILD").type != 0)
			{
				GameData.instance.SAVE_STATE.notificationsGuild = sharedObject.Get("NOTIFICATIONS_GUILD").bool_val;
			}
			if (sharedObject.Get("NOTIFICATIONS_OTHER").type != 0)
			{
				GameData.instance.SAVE_STATE.notificationsGuild = sharedObject.Get("NOTIFICATIONS_OTHER").bool_val;
			}
			if (sharedObject.Get("APP_NOTIFICATIONS_DISABLED").type != 0)
			{
				GameData.instance.SAVE_STATE.appNotificationsDisabled = sharedObject.Get("APP_NOTIFICATIONS_DISABLED").bool_val;
			}
			if (sharedObject.Get("FILTER_DISABLED").type != 0)
			{
				GameData.instance.SAVE_STATE.filterDisabled = sharedObject.Get("FILTER_DISABLED").bool_val;
			}
			if (sharedObject.Get("CHAT_AGE_VERIFIED").type != 0)
			{
				GameData.instance.SAVE_STATE.chatAgeVerified = sharedObject.Get("CHAT_AGE_VERIFIED").bool_val;
			}
			if (sharedObject.Get("CHAT_TOS_VERIFIED").type != 0)
			{
				GameData.instance.SAVE_STATE.chatTosVerified = sharedObject.Get("CHAT_TOS_VERIFIED").bool_val;
			}
			if (sharedObject.Get("ANIMATIONS").type != 0)
			{
				GameData.instance.SAVE_STATE.animations = sharedObject.Get("ANIMATIONS").bool_val;
			}
			if (sharedObject.Get("EULA_VERIFIED").type != 0)
			{
				GameData.instance.SAVE_STATE.eulaVerified = sharedObject.Get("EULA_VERIFIED").bool_val;
			}
			if (sharedObject.Get("IGNORE_SHRINES").type != 0)
			{
				GameData.instance.SAVE_STATE.ignoreShrines = sharedObject.Get("IGNORE_SHRINES").bool_val;
			}
			if (sharedObject.Get("IGNORE_BOSS").type != 0)
			{
				GameData.instance.SAVE_STATE.ignoreBoss = sharedObject.Get("IGNORE_BOSS").bool_val;
			}
			if (sharedObject.Get("BRAWL_REQUESTS").type != 0)
			{
				GameData.instance.SAVE_STATE.brawlRequests = sharedObject.Get("BRAWL_REQUESTS").bool_val;
			}
			if (sharedObject.Get("BRAWL_REQUESTS_FRIEND").type != 0)
			{
				GameData.instance.SAVE_STATE.brawlRequestsFriend = sharedObject.Get("BRAWL_REQUESTS_FRIEND").bool_val;
			}
			if (sharedObject.Get("BRAWL_REQUESTS_GUILD").type != 0)
			{
				GameData.instance.SAVE_STATE.brawlRequestsGuild = sharedObject.Get("BRAWL_REQUESTS_GUILD").bool_val;
			}
			if (sharedObject.Get("BRAWL_REQUESTS_OTHER").type != 0)
			{
				GameData.instance.SAVE_STATE.brawlRequestsOther = sharedObject.Get("BRAWL_REQUESTS_OTHER").bool_val;
			}
			if (sharedObject.Get("AUTO_PILOT_DEATH_DISABLE").type != 0)
			{
				GameData.instance.SAVE_STATE.autoPilotDeathDisable = sharedObject.Get("AUTO_PILOT_DEATH_DISABLE").bool_val;
			}
			if (sharedObject.Get("AUTO_ENRAGE").type != 0)
			{
				GameData.instance.SAVE_STATE.autoEnrage = sharedObject.Get("AUTO_ENRAGE").bool_val;
			}
			if (sharedObject.Get("ADS_DISABLED").type != 0)
			{
				GameData.instance.SAVE_STATE.adsDisabled = sharedObject.Get("ADS_DISABLED").bool_val;
			}
			if (sharedObject.Get("RIFT_EVENT_DIFFICULTY").type != 0)
			{
				GameData.instance.SAVE_STATE.SetRiftEventDifficulty(GameData.instance.SAVE_STATE.characterID, sharedObject.Get("RIFT_EVENT_DIFFICULTY").int_val);
			}
			if (sharedObject.Get("GAUNTLET_EVENT_DIFFICULTY").type != 0)
			{
				GameData.instance.SAVE_STATE.SetGauntletEventDifficulty(GameData.instance.SAVE_STATE.characterID, sharedObject.Get("GAUNTLET_EVENT_DIFFICULTY").int_val);
			}
			if (sharedObject.Get("INVASION_EVENT_DIFFICULTY").type != 0)
			{
				GameData.instance.SAVE_STATE.SetInvasionEventDifficulty(GameData.instance.SAVE_STATE.characterID, sharedObject.Get("INVASION_EVENT_DIFFICULTY").int_val);
			}
			if (sharedObject.Get("GVE_EVENT_DIFFICULTY").type != 0)
			{
				GameData.instance.SAVE_STATE.SetGvEEventDifficulty(GameData.instance.SAVE_STATE.characterID, sharedObject.Get("GVE_EVENT_DIFFICULTY").int_val);
			}
			if (sharedObject.Get("PVP_EVENT_BONUS").type != 0)
			{
				GameData.instance.SAVE_STATE.SetPvPEventBonus(GameData.instance.SAVE_STATE.characterID, sharedObject.Get("PVP_EVENT_BONUS").int_val);
			}
			if (sharedObject.Get("PVP_EVENT_SORT").type != 0)
			{
				GameData.instance.SAVE_STATE.SetPvPEventSort(GameData.instance.SAVE_STATE.characterID, sharedObject.Get("PVP_EVENT_SORT").bool_val);
			}
			if (sharedObject.Get("RIFT_EVENT_BONUS").type != 0)
			{
				GameData.instance.SAVE_STATE.SetRiftEventBonus(GameData.instance.SAVE_STATE.characterID, sharedObject.Get("RIFT_EVENT_BONUS").int_val);
			}
			if (sharedObject.Get("GAUNTLET_EVENT_BONUS").type != 0)
			{
				GameData.instance.SAVE_STATE.SetGauntletEventBonus(GameData.instance.SAVE_STATE.characterID, sharedObject.Get("GAUNTLET_EVENT_BONUS").int_val);
			}
			if (sharedObject.Get("INVASION_EVENT_BONUS").type != 0)
			{
				GameData.instance.SAVE_STATE.SetInvasionEventBonus(GameData.instance.SAVE_STATE.characterID, sharedObject.Get("INVASION_EVENT_BONUS").int_val);
			}
			if (sharedObject.Get("GVG_EVENT_BONUS").type != 0)
			{
				GameData.instance.SAVE_STATE.SetGvGEventBonus(GameData.instance.SAVE_STATE.characterID, sharedObject.Get("GVG_EVENT_BONUS").int_val);
			}
			if (sharedObject.Get("GVG_EVENT_SORT").type != 0)
			{
				GameData.instance.SAVE_STATE.SetGvGEventSort(GameData.instance.SAVE_STATE.characterID, sharedObject.Get("GVG_EVENT_SORT").bool_val);
			}
			if (sharedObject.Get("RAID_SELECTED").type != 0)
			{
				GameData.instance.SAVE_STATE.SetRaidSelected(GameData.instance.SAVE_STATE.characterID, sharedObject.Get("RAID_SELECTED").int_val);
			}
			if (sharedObject.Get("BRAWL_SELECTED").type != 0)
			{
				GameData.instance.SAVE_STATE.SetBrawlSelected(GameData.instance.SAVE_STATE.characterID, sharedObject.Get("BRAWL_SELECTED").int_val);
			}
			if (sharedObject.Get("BRAWL_TIER_SELECTED").type != 0)
			{
				GameData.instance.SAVE_STATE.SetBrawlTierSelected(GameData.instance.SAVE_STATE.GetBrawlSelected(GameData.instance.SAVE_STATE.characterID), GameData.instance.SAVE_STATE.characterID, sharedObject.Get("BRAWL_TIER_SELECTED").int_val);
			}
			if (sharedObject.Get("BRAWL_DIFFICULTY_SELECTED").type != 0)
			{
				GameData.instance.SAVE_STATE.SetBrawlDifficultySelected(GameData.instance.SAVE_STATE.GetBrawlSelected(GameData.instance.SAVE_STATE.characterID), GameData.instance.SAVE_STATE.characterID, sharedObject.Get("BRAWL_DIFFICULTY_SELECTED").int_val);
			}
			if (sharedObject.Get("BRAWL_PUBLIC_SELECTED").type != 0)
			{
				GameData.instance.SAVE_STATE.SetBrawlPublicSelected(GameData.instance.SAVE_STATE.characterID, sharedObject.Get("BRAWL_PUBLIC_SELECTED").bool_val);
			}
			if (sharedObject.Get("BRAWL_FILTER").type != 0)
			{
				GameData.instance.SAVE_STATE.SetBrawlFilter(GameData.instance.SAVE_STATE.characterID, sharedObject.Get("BRAWL_FILTER").int_val);
			}
			if (sharedObject.Get("BRAWL_TIER_FILTER").type != 0)
			{
				GameData.instance.SAVE_STATE.SetBrawlTierFilter(GameData.instance.SAVE_STATE.characterID, sharedObject.Get("BRAWL_TIER_FILTER").int_val);
			}
			if (sharedObject.Get("BRAWL_DIFFICULTY_FILTER").type != 0)
			{
				GameData.instance.SAVE_STATE.SetBrawlDifficultyFilter(GameData.instance.SAVE_STATE.characterID, sharedObject.Get("BRAWL_DIFFICULTY_FILTER").int_val);
			}
			if (sharedObject.Get("FREIND_REQUEST_SORT").type != 0)
			{
				GameData.instance.SAVE_STATE.SetFriendRequestSort(sharedObject.Get("FREIND_REQUEST_SORT").int_val, GameData.instance.SAVE_STATE.characterID);
			}
			if (sharedObject.Get("GUILD_APPLICANT_SORT").type != 0)
			{
				GameData.instance.SAVE_STATE.SetGuildApplicantSort(GameData.instance.SAVE_STATE.characterID, sharedObject.Get("GUILD_APPLICANT_SORT").int_val);
			}
			if (sharedObject.Get("ENCHANT_SELECT_SORT").type != 0)
			{
				GameData.instance.SAVE_STATE.SetEnchantSelectSort(GameData.instance.SAVE_STATE.characterID, sharedObject.Get("ENCHANT_SELECT_SORT").int_val);
			}
			if (sharedObject.Get("MOUNT_SELECT_SORT").type != 0)
			{
				GameData.instance.SAVE_STATE.SetMountSelectSort(GameData.instance.SAVE_STATE.characterID, sharedObject.Get("MOUNT_SELECT_SORT").int_val);
			}
			if (sharedObject.Get("FISHING_ROD").type != 0)
			{
				GameData.instance.SAVE_STATE.SetFishingRod(GameData.instance.SAVE_STATE.characterID, sharedObject.Get("FISHING_ROD").int_val);
			}
			if (sharedObject.Get("FISHING_BOBBER").type != 0)
			{
				GameData.instance.SAVE_STATE.SetFishingBobber(GameData.instance.SAVE_STATE.characterID, sharedObject.Get("FISHING_BOBBER").int_val);
			}
			if (sharedObject.Get("FISHING_BAIT").type != 0)
			{
				GameData.instance.SAVE_STATE.SetFishingBait(GameData.instance.SAVE_STATE.characterID, sharedObject.Get("FISHING_BAIT").int_val);
			}
			if (sharedObject.Get("PLAYER_ID").type != 0)
			{
				GameData.instance.SAVE_STATE.playerID = sharedObject.Get("PLAYER_ID").int_val;
			}
		}
	}

	public static string ParseStringValues(string text, List<string> values, bool color = true)
	{
		if (values == null || values.Count <= 0)
		{
			return ParseString(text);
		}
		string text2 = "#";
		for (int i = 0; i < values.Count; i++)
		{
			string text3 = values[i];
			if (text3 != null)
			{
				string stringSearch = text2 + "value" + (i + 1) + text2;
				string text4 = text3;
				string stringReplace = (color ? ("^" + text4 + "^") : text4);
				text = ReplaceStringsWithString(text, stringSearch, stringReplace);
			}
		}
		return ParseString(text);
	}

	public static string parseDailyQuestString(string text, DailyQuestRef dailyQuestRef, bool html = true)
	{
		string text2 = (html ? "^" : "");
		text = ReplaceStringsWithString(text, "[value]", text2 + NumberFormat(dailyQuestRef.amount) + text2);
		return ParseString(text);
	}

	private static string ReplaceStringsWithColor(string start, string end, string input, string color)
	{
		string pattern = "\\" + start + "(.*?)\\" + end;
		string text = null;
		text = ((color == null) ? "$1" : ("<COLOR=" + color + ">$1</COLOR>"));
		RegexOptions options = RegexOptions.Multiline;
		return new Regex(pattern, options).Replace(input, text);
	}

	private static string ReplaceStringsWithItems(string text)
	{
		for (int i = 0; i < 22; i++)
		{
			string itemLink = ItemRef.getItemLink(i);
			string pattern = "\\!" + itemLink + "!(.*?)\\!" + itemLink + "!";
			RegexOptions options = RegexOptions.Multiline;
			foreach (Match item in Regex.Matches(text, pattern, options))
			{
				if (int.TryParse(item.Value.Trim().Replace("!" + itemLink + "!", ""), out var result))
				{
					ItemRef itemRef = ItemBook.Lookup(result, i);
					text = text.Replace(item.Value, itemRef.coloredName);
				}
			}
		}
		return text;
	}

	private static string ReplaceStringsWithRarities(string text)
	{
		string pattern = "\\*(.*?)\\*";
		RegexOptions options = RegexOptions.Multiline;
		foreach (Match item in Regex.Matches(text, pattern, options))
		{
			RarityRef rarityRef = RarityBook.Lookup(item.Value.Replace('*', ' ').Trim());
			if (rarityRef != null)
			{
				text = text.Replace(item.Value, rarityRef.coloredName);
				continue;
			}
			return text;
		}
		return text;
	}

	private static string ReplaceStringsWithString(string text, string stringSearch, string stringReplace)
	{
		int num = 0;
		int num2 = 500;
		int num3 = text.ToLower().IndexOf(stringSearch);
		while (num3 >= 0 && num < num2)
		{
			text = text.Substring(0, num3) + stringReplace + text.Substring(num3 + stringSearch.Length);
			num3 = text.ToLower().IndexOf(stringSearch);
			num++;
		}
		return text;
	}

	public static DateTime localizeDate(DateTime date)
	{
		return date.ToLocalTime();
	}

	public static string trimSpacing(string thestring)
	{
		if (thestring == null)
		{
			return "";
		}
		string[] value = thestring.Split(' ');
		return string.Join(" ", value);
	}

	public static string cacheBreak(string thestring)
	{
		return thestring + "?v=" + randomInt(0, 9999999);
	}

	public static bool emailIsValid(string email)
	{
		return Regex.Match(email, "^[A-Z0-9._%+-]+@(?:[A-Z0-9-]+\\.)+[A-Z]{2,4}$", RegexOptions.IgnoreCase).Success;
	}

	public static void pauseChildren(object theobject, bool deep = true, Class cl = null)
	{
	}

	public static void resumeChildren(object theobject, bool deep = true, Class cl = null)
	{
	}

	public static void stopChildren(object theobject, bool random = false, bool deep = true, Class cl = null)
	{
	}

	public static void PlayChildren(GameObject theobject, bool random = false, bool loop = true)
	{
		if (theobject == null || !(theobject.GetComponent<FrameNavigator>() != null))
		{
			return;
		}
		FrameNavigator component = theobject.GetComponent<FrameNavigator>();
		for (int i = 0; i < component.transform.childCount; i++)
		{
			Transform child = component.transform.GetChild(i);
			Transform transform = ((child.childCount > 0) ? child.GetChild(0) : null);
			if (transform != null && transform.GetComponent<FrameNavigator>() != null)
			{
				FrameNavigator component2 = transform.GetComponent<FrameNavigator>();
				component2.GoToAndStop((!random) ? 1 : randomInt(1, component2.totalFrames));
			}
			if (transform != null && loop)
			{
				PlayChildren(transform.gameObject, random, loop);
			}
		}
	}

	public static void RotateChildrenByName(GameObject theobject, int rotation, string name)
	{
		if (theobject == null)
		{
			return;
		}
		foreach (GameObject item in GetChildrenByName(theobject, name))
		{
			item.transform.rotation = Quaternion.Euler(item.transform.rotation.x, item.transform.rotation.y, rotation);
		}
	}

	public static DisplayObject getChildWithName(object theobject, string name)
	{
		return null;
	}

	public static List<GameObject> GetChildrenByName(GameObject obj, string name)
	{
		List<GameObject> list = new List<GameObject>();
		if (obj == null)
		{
			return list;
		}
		for (int i = 0; i < obj.transform.childCount; i++)
		{
			Transform child = obj.transform.GetChild(i);
			if (child.gameObject.name == name)
			{
				list.Add(child.gameObject);
			}
		}
		return list;
	}

	public static Array getChildrenByClass(object theobject, Class cl, bool loop = false)
	{
		return null;
	}

	public static int randomInt(int min, int max)
	{
		if (min == max)
		{
			return max;
		}
		return Mathf.RoundToInt(UnityEngine.Random.Range(min, max + 1));
	}

	public static float RandomNumber(float min, float max)
	{
		if (min == max)
		{
			return max;
		}
		return UnityEngine.Random.Range(min, max);
	}

	public static bool randomBoolean()
	{
		return randomInt(0, 1) == 1;
	}

	public static string RandomString(float length = 30f)
	{
		string text = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
		string text2 = "";
		for (float num = 0f; num < length; num += 1f)
		{
			text2 += text[Mathf.FloorToInt(UnityEngine.Random.Range(0, text.Length))];
		}
		return text2;
	}

	public static Ease RandomEase()
	{
		return randomInt(1, 11) switch
		{
			1 => Ease.InOutBack, 
			2 => Ease.InOutBounce, 
			3 => Ease.InOutCirc, 
			4 => Ease.InOutCubic, 
			5 => Ease.InOutElastic, 
			6 => Ease.InOutExpo, 
			7 => Ease.Linear, 
			8 => Ease.InOutQuad, 
			9 => Ease.InOutQuart, 
			10 => Ease.InOutQuint, 
			11 => Ease.InOutSine, 
			_ => Ease.Linear, 
		};
	}

	public static Array randomize(Array array)
	{
		return null;
	}

	public static float roundToNearest(float roundTo, float value)
	{
		return Mathf.Round(value / roundTo) * roundTo;
	}

	public static List<string> arrayToStringVector(string[] array, bool language = false)
	{
		if (array == null)
		{
			return null;
		}
		List<string> list = new List<string>();
		for (int i = 0; i < array.Length; i++)
		{
			list.Add(array[i]);
		}
		return list;
	}

	public static string[] stringVectorToArray(List<string> vector)
	{
		string[] array = new string[vector.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = vector[i];
		}
		return array;
	}

	public static List<int> arrayToIntegerVector(int[] array)
	{
		List<int> list = new List<int>();
		if (array == null || array.Length == 0)
		{
			return list;
		}
		for (int i = 0; i < array.Length; i++)
		{
			list.Add(array[i]);
		}
		return list;
	}

	public static int[] intVectorToArray(List<int> vector)
	{
		int[] array = new int[vector.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = vector[i];
		}
		return array;
	}

	public static bool intVectorContainsInt(List<int> vector, int integer)
	{
		foreach (int item in vector)
		{
			if (item == integer)
			{
				return true;
			}
		}
		return false;
	}

	public static List<float> arrayToNumberVector(float[] array)
	{
		List<float> list = new List<float>();
		if (array == null)
		{
			return list;
		}
		for (int i = 0; i < array.Length; i++)
		{
			list.Add(array[i]);
		}
		return list;
	}

	public static List<long> arrayToNumberVector(long[] array)
	{
		List<long> list = new List<long>();
		if (array == null)
		{
			return list;
		}
		for (int i = 0; i < array.Length; i++)
		{
			list.Add(array[i]);
		}
		return list;
	}

	public static List<int> StringToIntList(string content, char separator = ',')
	{
		List<int> list = new List<int>();
		if (content != null && !content.Equals(""))
		{
			string[] array = content.Split(separator);
			for (int i = 0; i < array.Length; i++)
			{
				list.Add(int.Parse(array[i]));
			}
		}
		return list;
	}

	public static List<Vector2> StringToVector2List(string content, char itemSeparator = ':', char coordSeparator = ',')
	{
		List<Vector2> list = new List<Vector2>();
		if (content != null && !content.Equals(""))
		{
			string[] array = content.Split(itemSeparator);
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(coordSeparator);
				if (array2.Length >= 2)
				{
					list.Add(new Vector2(int.Parse(array2[0]), int.Parse(array2[1])));
				}
			}
		}
		return list;
	}

	public static float GetFloatFromStringProperty(string property, float defaultValue = 0f)
	{
		if (property == null)
		{
			return defaultValue;
		}
		if (!property.ToLower().Equals(""))
		{
			try
			{
				return ParseFloat(property);
			}
			catch (FormatException ex)
			{
				D.LogWarning("Error Parsing float from string: " + property + " - " + ex.Message);
				return 0f;
			}
		}
		return 0f;
	}

	public static int GetIntFromStringProperty(string property, int defaultValue = 0)
	{
		if (property == null)
		{
			return defaultValue;
		}
		if (!property.ToLower().Equals(""))
		{
			try
			{
				return int.Parse(property);
			}
			catch (FormatException ex)
			{
				D.LogWarning("Error Parsing int from string: " + property + " - " + ex.Message);
				return 0;
			}
		}
		return 0;
	}

	public static bool GetBoolFromStringProperty(string property, bool defaultValue = false)
	{
		if (property == null)
		{
			return defaultValue;
		}
		if (property.ToLower().Equals("true"))
		{
			return true;
		}
		return false;
	}

	public static string GetStringFromStringProperty(string property, string defaultValue = "")
	{
		if (property == null)
		{
			return defaultValue;
		}
		return property;
	}

	public static Vector2 GetVector2FromStringProperty(string property)
	{
		if (property == null)
		{
			return Vector2.zero;
		}
		string[] array = property.Split(',');
		if (array.Length != 2)
		{
			return Vector2.zero;
		}
		return new Vector2(ParseFloat(array[0]), ParseFloat(array[1]));
	}

	public static List<string> GetStringListFromStringProperty(string property, char separator = ',', bool language = false)
	{
		List<string> list = new List<string>();
		string[] stringArrayFromStringProperty = GetStringArrayFromStringProperty(property, separator);
		if (stringArrayFromStringProperty != null)
		{
			for (int i = 0; i < stringArrayFromStringProperty.Length; i++)
			{
				if (!language)
				{
					list.Add(stringArrayFromStringProperty[i]);
				}
				else
				{
					list.Add(Language.GetString(stringArrayFromStringProperty[i]));
				}
			}
		}
		return list;
	}

	public static List<int> GetIntListFromStringProperty(string property, char separator = ',')
	{
		List<int> list = new List<int>();
		string[] stringArrayFromStringProperty = GetStringArrayFromStringProperty(property, separator);
		if (stringArrayFromStringProperty != null)
		{
			for (int i = 0; i < stringArrayFromStringProperty.Length; i++)
			{
				int.TryParse(stringArrayFromStringProperty[i], out var result);
				list.Add(result);
			}
		}
		return list;
	}

	public static int[] GetIntArrayFromStringProperty(string property, char separator = ',')
	{
		if (property != null && !property.Equals(""))
		{
			string[] array = property.Split(separator);
			int[] array2 = new int[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = int.Parse(array[i]);
			}
			return array2;
		}
		return null;
	}

	public static string[] GetStringArrayFromStringProperty(string property, char separator = ',')
	{
		if (property != null && !property.Equals(""))
		{
			return property.Split(separator);
		}
		return null;
	}

	public static Color GetColorFromHex(string hex)
	{
		Color color = Color.white;
		if (hex[0] != '#')
		{
			hex = "#" + hex;
		}
		ColorUtility.TryParseHtmlString(hex, out color);
		return color;
	}

	public static T GetOrAddComponent<T>(GameObject parent) where T : Component
	{
		Component component = parent.GetComponent<T>();
		if (component == null)
		{
			component = parent.AddComponent<T>();
		}
		return (T)component;
	}

	public static Component GetParentClass(GameObject obj, Type cl)
	{
		return obj.GetComponentInParent(cl);
	}

	public static bool isOdd(int num)
	{
		if ((float)(num % 2) == 1f)
		{
			return true;
		}
		return false;
	}

	public static bool isEven(int num)
	{
		if ((float)(num % 2) == 0f)
		{
			return true;
		}
		return false;
	}

	public static bool isNaNOrInfinity(float val)
	{
		return val * 0f != 0f;
	}

	public static bool stringToBoolean(string thestring)
	{
		if (thestring.ToLower() == "false")
		{
			return false;
		}
		return true;
	}

	public static List<GameObject> GetObjectsUnderPointByName(Transform local, Vector2 point, string n, int max = 0, List<GameObject> exclude = null)
	{
		RaycastHit2D[] array = Physics2D.RaycastAll(new Vector3(point.x, point.y, GameData.instance.main.mainCamera.transform.position.z), Vector2.zero);
		List<GameObject> list = new List<GameObject>();
		if (exclude == null)
		{
			exclude = new List<GameObject>();
		}
		for (int num = array.Length - 1; num >= 0; num--)
		{
			RaycastHit2D raycastHit2D = array[num];
			GameObject gameObject = raycastHit2D.collider.gameObject;
			while (gameObject != null)
			{
				bool flag = false;
				foreach (GameObject item in exclude)
				{
					if (item.GetInstanceID() == gameObject.GetInstanceID())
					{
						flag = true;
					}
				}
				if (!flag && gameObject.name == n)
				{
					list.Add(gameObject);
					if (max > 0 && list.Count == max)
					{
						return list;
					}
					exclude.Add(gameObject);
				}
				gameObject = ((gameObject.transform.parent != null) ? gameObject.transform.parent.gameObject : null);
			}
		}
		return list;
	}

	public static List<GameObject> GetObjectsUnderPointByClass(Transform local, Vector2 point, Type c, int max = 0, List<GameObject> exclude = null)
	{
		RaycastHit2D[] array = Physics2D.RaycastAll(new Vector3(point.x, point.y, GameData.instance.main.mainCamera.transform.position.z), Vector2.zero);
		List<GameObject> list = new List<GameObject>();
		if (exclude == null)
		{
			exclude = new List<GameObject>();
		}
		for (int num = array.Length - 1; num >= 0; num--)
		{
			RaycastHit2D raycastHit2D = array[num];
			GameObject gameObject = raycastHit2D.collider.gameObject;
			while (gameObject != null)
			{
				bool flag = false;
				foreach (GameObject item in exclude)
				{
					if (item.GetInstanceID() == gameObject.GetInstanceID())
					{
						flag = true;
					}
				}
				if (!flag && gameObject.GetComponent(c) != null)
				{
					list.Add(gameObject);
					if (max > 0 && list.Count == max)
					{
						return list;
					}
					exclude.Add(gameObject);
				}
				gameObject = ((gameObject.transform.parent != null) ? gameObject.transform.parent.gameObject : null);
			}
		}
		return list;
	}

	public static List<int> getIntVectorFromString(string thestring)
	{
		List<int> list = new List<int>();
		if (thestring == null || thestring.Length <= 0)
		{
			return list;
		}
		Array array = thestring.Split(',');
		if (array.Length <= 0)
		{
			return list;
		}
		foreach (string item in array)
		{
			list.Add(int.Parse(item));
		}
		return list;
	}

	public static List<string> getStringVectorFromString(string thestring)
	{
		List<string> list = new List<string>();
		if (thestring == null || thestring.Length <= 0)
		{
			return list;
		}
		Array array = thestring.Split(',');
		if (array.Length <= 0)
		{
			return list;
		}
		foreach (string item in array)
		{
			list.Add(item);
		}
		return list;
	}

	public static List<bool> getBooleanVectorFromArray(Array array)
	{
		if (array == null)
		{
			return null;
		}
		List<bool> list = new List<bool>();
		foreach (bool item in array)
		{
			list.Add(item);
		}
		return list;
	}

	public static bool[] getArrayFromBooleanVector(List<bool> vector)
	{
		if (vector == null)
		{
			return null;
		}
		bool[] array = new bool[vector.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = vector[i];
		}
		return array;
	}

	public static Rect? getRectangleFromString(string thestring)
	{
		List<int> intVectorFromString = getIntVectorFromString(thestring);
		if (intVectorFromString.Count < 4)
		{
			return null;
		}
		int num = intVectorFromString[0];
		int num2 = intVectorFromString[1];
		int num3 = intVectorFromString[2];
		int num4 = intVectorFromString[3];
		return new Rect(num, num2, num3, num4);
	}

	public static string GetNumberColor(int first, int second, int third)
	{
		if (first > second && first > third)
		{
			return "#00FF00";
		}
		if (first < second && first < third)
		{
			return "#FF0000";
		}
		return "#FFFFFF";
	}

	public static string getCurrentColor(int highest, int current, int lowest)
	{
		if (current >= highest)
		{
			return "#00FF00";
		}
		if (current <= lowest)
		{
			return "#FF0000";
		}
		return "#FFFFFF";
	}

	public static string colorString(string text, string color)
	{
		if (color != null && color.Length > 0)
		{
			if (color[0] != '#')
			{
				color = "#" + color;
			}
			return "<color=" + color + ">" + text + "</color>";
		}
		return "<color=white>" + text + "</color>";
	}

	public static uint colorUint(string color)
	{
		return Convert.ToUInt32("0x" + color.ToLower(), 16);
	}

	public static string FormatStrings(List<string> strings)
	{
		if (strings == null || strings.Count <= 0)
		{
			return "";
		}
		return string.Join(", ", strings.ToArray());
	}

	public static string dateFormat(DateTime date)
	{
		return date.ToString("MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);
	}

	public static string NumberFormat(float number, bool abbreviate = true, bool shortbool = false, float divide = 100f)
	{
		if (abbreviate && number >= 1E+09f)
		{
			return Language.GetString("number_abbreviation_billion", new string[1] { (Math.Floor(number / 1E+09f * divide) / (double)divide).ToString() });
		}
		if (abbreviate && number >= 1000000f)
		{
			return Language.GetString("number_abbreviation_million", new string[1] { (Math.Floor(number / 1000000f * divide) / (double)divide).ToString() });
		}
		if (abbreviate && shortbool && number >= 1000f)
		{
			return Language.GetString("number_abbreviation_thousand", new string[1] { (Math.Floor(number / 1000f * divide) / (double)divide).ToString() });
		}
		return number.ToString("#,##0.##", new CultureInfo("en-US"));
	}

	public static string NumberColor(string thestring)
	{
		return "^" + thestring + "^";
	}

	public static float roundedValue(float value, float decimals = 100f)
	{
		return Mathf.Round(value * decimals) / decimals;
	}

	public static string TimeFormat(int seconds, bool isLong = false)
	{
		float num = seconds % 60;
		float num2 = Mathf.Floor(seconds % 3600 / 60);
		float num3 = Mathf.Floor(seconds / 3600);
		float num4 = Mathf.Floor(num3 / 24f);
		if (num4 > 0f)
		{
			num3 -= num4 * 24f;
		}
		string text = ((num4 <= 0f) ? "" : (num4 + ":"));
		string text2 = ((num3 <= 0f) ? "" : (num3 + ":"));
		string text3 = DoubleDigitFormat((uint)num2) + ":";
		string text4 = DoubleDigitFormat((uint)num);
		if (isLong && num4 > 0f)
		{
			text = Language.GetString((num4 == 1f) ? "time_format_day" : "time_format_days", new string[1] { NumberFormat(Mathf.Round(num4)) }) + " ";
		}
		else if (isLong && num3 > 0f)
		{
			text = "";
			text2 = Language.GetString((num3 == 1f) ? "time_format_hour" : "time_format_hours", new string[1] { NumberFormat(Mathf.Round(num3)) }) + " ";
		}
		else if (isLong && num2 > 0f)
		{
			text2 = "";
			text3 = Language.GetString((num2 == 1f) ? "time_format_minute" : "time_format_minutes", new string[1] { NumberFormat(Mathf.Round(num2)) }) + " ";
		}
		else if (isLong && num > 0f)
		{
			text3 = "";
			text4 = Language.GetString((num == 1f) ? "time_format_second" : "time_format_seconds", new string[1] { NumberFormat(Mathf.Round(num)) }) + " ";
		}
		string text5 = text + text2 + text3 + text4;
		if (text5.Substring(0, 1) == "0")
		{
			text5 = text5.Substring(1, text5.Length - 1);
		}
		return text5;
	}

	private static string DoubleDigitFormat(uint num)
	{
		if (num < 10)
		{
			return "0" + num;
		}
		return num.ToString();
	}

	public static string TimeFormatShort(int time)
	{
		if (isNaNOrInfinity(time))
		{
			return Language.GetString("ui_question_mark");
		}
		float num = time % 60;
		float num2 = Mathf.Floor(time % 3600 / 60);
		float num3 = Mathf.Floor(time / 3600);
		float num4 = Mathf.Floor(num3 / 24f);
		float num5 = Mathf.Floor(num4 / 7f);
		float num6 = Mathf.Floor(num4 / 30f);
		float num7 = Mathf.Floor(num4 / 365f);
		if (num7 > 0f)
		{
			return Language.GetString((num7 == 1f) ? "time_format_year" : "time_format_years", new string[1] { NumberFormat(Mathf.Round(num7)) });
		}
		if (num6 > 0f)
		{
			return Language.GetString((num6 == 1f) ? "time_format_month" : "time_format_months", new string[1] { NumberFormat(Mathf.Round(num6)) });
		}
		if (num5 > 0f)
		{
			return Language.GetString((num5 == 1f) ? "time_format_week" : "time_format_weeks", new string[1] { NumberFormat(Mathf.Round(num5)) });
		}
		if (num4 > 0f)
		{
			return Language.GetString((num4 == 1f) ? "time_format_day" : "time_format_days", new string[1] { NumberFormat(Mathf.Round(num4)) });
		}
		if (num3 > 0f)
		{
			return Language.GetString((num3 == 1f) ? "time_format_hour" : "time_format_hours", new string[1] { NumberFormat(Mathf.Round(num3)) });
		}
		if (num2 > 0f)
		{
			return Language.GetString((num2 == 1f) ? "time_format_minute" : "time_format_minutes", new string[1] { NumberFormat(Mathf.Round(num2)) });
		}
		if (num < 0f)
		{
			num = 0f;
		}
		if (isNaNOrInfinity(num))
		{
			return Language.GetString("ui_question_mark");
		}
		return Language.GetString((num == 1f) ? "time_format_second" : "time_format_seconds", new string[1] { NumberFormat(Mathf.Round(num)) });
	}

	public static string TimeFormatClean(float seconds)
	{
		string text = "";
		if (seconds <= 0f)
		{
			return text;
		}
		seconds = Mathf.Round(seconds);
		float num = seconds;
		seconds = num % 60f;
		float num2 = num / 60f;
		int num3 = (int)num2 % 60;
		float num4 = num2 / 60f;
		int num5 = (int)num4 % 24;
		int num6 = (int)(num4 / 24f);
		if (num6 > 0)
		{
			text += Language.GetString((num6 == 1) ? "time_format_day" : "time_format_days", new string[1] { num6.ToString() });
			if (num5 > 0)
			{
				text = text + " " + Language.GetString((num5 == 1) ? "time_format_hour" : "time_format_hours", new string[1] { num5.ToString() });
			}
			else if (num3 > 0)
			{
				text = text + " " + Language.GetString((num3 == 1) ? "time_format_minute_short" : "time_format_minutes_short", new string[1] { num3.ToString() });
			}
			else if (seconds > 0f)
			{
				text = text + " " + Language.GetString((seconds == 1f) ? "time_format_second_short" : "time_format_seconds_short", new string[1] { seconds.ToString() });
			}
			return text;
		}
		if (num5 > 0)
		{
			text += Language.GetString((num5 == 1) ? "time_format_hour" : "time_format_hours", new string[1] { num5.ToString() });
			if (num3 > 0)
			{
				text = text + " " + Language.GetString((num3 == 1) ? "time_format_minute_short" : "time_format_minutes_short", new string[1] { num3.ToString() });
			}
			else if (seconds > 0f)
			{
				text = text + " " + Language.GetString((seconds == 1f) ? "time_format_second_short" : "time_format_seconds_short", new string[1] { seconds.ToString() });
			}
			return text;
		}
		if (num3 > 0)
		{
			text += Language.GetString((num3 == 1) ? "time_format_minute_short" : "time_format_minutes_short", new string[1] { num3.ToString() });
			if (seconds > 0f)
			{
				text = text + " " + Language.GetString((seconds == 1f) ? "time_format_second_short" : "time_format_seconds_short", new string[1] { seconds.ToString() });
			}
			return text;
		}
		if (seconds > 0f)
		{
			return text + Language.GetString((seconds == 1f) ? "time_format_second" : "time_format_seconds", new string[1] { seconds.ToString() });
		}
		return text;
	}

	public static Vector2 pointFromString(string str)
	{
		if (str == null)
		{
			return new Vector2(0f, 0f);
		}
		string[] array = str.Split(',');
		if (array.Length == 0)
		{
			return default(Vector2);
		}
		int num = int.Parse(array[0]);
		array = array.Skip(1).ToArray();
		if (array.Length == 0)
		{
			return new Vector2(num, 0f);
		}
		int num2 = int.Parse(array[0]);
		array = array.Skip(1).ToArray();
		return new Vector2(num, num2);
	}

	public static Vector2 numberPointFromString(string str)
	{
		if (str == null)
		{
			return default(Vector2);
		}
		string[] array = str.Split(',');
		if (array.Length == 0)
		{
			return default(Vector2);
		}
		float x = ParseFloat(array[0]);
		if (array.Length == 0)
		{
			return new Vector2(0f, 0f);
		}
		float y = ParseFloat(array[0]);
		return new Vector2(x, y);
	}

	public static string removeExtraWhiteSpace(string thestring)
	{
		thestring = removeRepeatedWhiteSpace(thestring);
		thestring = removeBeginningEndWhiteSpace(thestring);
		return thestring;
	}

	public static string RemoveWhiteSpace(string str)
	{
		return str.Replace(" ", "");
	}

	public static string removeRepeatedWhiteSpace(string thestring)
	{
		for (int i = 0; i < 5; i++)
		{
			thestring = RemoveWhiteSpace(thestring);
		}
		return thestring;
	}

	public static string removeBeginningEndWhiteSpace(string thestring)
	{
		return thestring.Trim();
	}

	public static void SetTab(Button tab, bool enabled = true, bool useInteractable = false)
	{
		SetInteractable(tab, !enabled, enabled, useInteractable);
	}

	public static void SetButton(Button button, bool enabled = true, bool useInteractable = false)
	{
		SetInteractable(button, enabled, enabled, useInteractable);
	}

	private static void SetInteractable(Button interactable, bool behaviorEnabled, bool colorEnabled, bool useInteractable)
	{
		if (!(interactable == null))
		{
			TextMeshProUGUI componentInChildren = interactable.GetComponentInChildren<TextMeshProUGUI>();
			if (interactable.image != null)
			{
				interactable.image.color = new Color(interactable.image.color.r, interactable.image.color.g, interactable.image.color.b, colorEnabled ? 1f : 0.5f);
			}
			if (componentInChildren != null)
			{
				componentInChildren.color = new Color(componentInChildren.color.r, componentInChildren.color.g, componentInChildren.color.b, colorEnabled ? 1f : 0.5f);
			}
			if (useInteractable)
			{
				interactable.interactable = behaviorEnabled;
			}
			else
			{
				interactable.enabled = behaviorEnabled;
			}
		}
	}

	public static void SetRenderers(GameObject gameObject, bool enabled = true, bool checkDisabledChildren = false)
	{
		Image[] componentsInChildren = gameObject.GetComponentsInChildren<Image>(checkDisabledChildren);
		foreach (Image image in componentsInChildren)
		{
			image.color = new Color(image.color.r, image.color.g, image.color.b, enabled ? 1f : 0.5f);
		}
		TextMeshProUGUI[] componentsInChildren2 = gameObject.GetComponentsInChildren<TextMeshProUGUI>(checkDisabledChildren);
		foreach (TextMeshProUGUI textMeshProUGUI in componentsInChildren2)
		{
			textMeshProUGUI.color = new Color(textMeshProUGUI.color.r, textMeshProUGUI.color.g, textMeshProUGUI.color.b, enabled ? 1f : 0.5f);
		}
	}

	public static void SetImageAlpha(Image image, bool alpha)
	{
		if (!image)
		{
			if (alpha)
			{
				image.color = new Color(image.color.r, image.color.g, image.color.b, 0.5f);
			}
			else
			{
				image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
			}
		}
	}

	public static void SetImageAlpha(Image image, float alphaQty)
	{
		image.color = new Color(image.color.r, image.color.g, image.color.b, alphaQty);
	}

	public static void adjustBrightness(Image image, float colorValue = 1f)
	{
		brightnessColor = new Color(colorValue, colorValue, colorValue);
		if (image != null)
		{
			image.material.color = brightnessColor;
		}
	}

	public static void adjustTextSaturation(TextMeshProUGUI text, float colorValue = 0f)
	{
		brightnessColor = new Color(colorValue, colorValue, colorValue);
		Color.RGBToHSV(text.color, out var H, out var S, out var V);
		text.outlineColor = new Color(0f - colorValue * 0.5f, 0f - colorValue * 0.5f, 0f - colorValue * 0.5f);
		text.color = Color.HSVToRGB(H, S + colorValue, V);
		text.UpdateFontAsset();
	}

	public static void adjustBrightness(SpriteRenderer image, float colorValue = 1f)
	{
		brightnessColor = new Color(colorValue, colorValue, colorValue);
		image.material.SetColor("_RendererColor", brightnessColor);
	}

	public static void Shake(GameObject obj, float speed = 1f, Vector2? point = null)
	{
		Shake shake = obj.GetComponent<Shake>();
		if (shake == null)
		{
			shake = obj.AddComponent<Shake>();
		}
		shake.StartShake(speed, point);
	}

	public static List<Vector2> stringToPoints(string str)
	{
		List<Vector2> list = new List<Vector2>();
		if (str.Length <= 0)
		{
			return list;
		}
		string[] array = str.Split(',');
		if (array.Length == 0)
		{
			return list;
		}
		foreach (string text in array)
		{
			if (text == null || text.Length <= 0)
			{
				list.Add(new Vector2(0f, 0f));
				continue;
			}
			string[] array2 = text.Split(':');
			if (array2.Length <= 1)
			{
				list.Add(new Vector2(0f, 0f));
			}
			else
			{
				list.Add(new Vector2(int.Parse(array2[0]), int.Parse(array2[1])));
			}
		}
		return list;
	}

	public static string pointVectorsToString(List<List<Vector2>> vectors)
	{
		string text = "";
		foreach (List<Vector2> vector in vectors)
		{
			if (text.Length > 0)
			{
				text += " ";
			}
			text = text + "[" + pointsToString(vector) + "]";
		}
		return text;
	}

	public static string pointToString(Vector2 point)
	{
		return point.x + "," + point.y;
	}

	public static string pointsToString(List<Vector2> points)
	{
		string text = "";
		for (int i = 0; i < points.Count; i++)
		{
			if (i > 0)
			{
				text += ",";
			}
			Vector2 vector = points[i];
			text = text + vector.x + ":" + vector.y;
		}
		return text;
	}

	public static bool parseBoolean(string str, bool defaultVal = true)
	{
		if (str == null)
		{
			str = "";
		}
		str = str.ToLower();
		if (!defaultVal)
		{
			if (str == "true" || str == "1")
			{
				return true;
			}
			return false;
		}
		if (str == "false" || str == "0")
		{
			return false;
		}
		return true;
	}

	public static string parseMultiLine(string str)
	{
		string text = "";
		string[] array = new string[1] { "\n" };
		string[] array2 = str.Split(array, StringSplitOptions.None);
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i] = removeBeginningEndWhiteSpace(array2[i]);
			if (array2[i].Length > 0)
			{
				text = text + array2[i] + array[0];
			}
		}
		return text;
	}

	public static string nullIfEmpty(string str)
	{
		if (str != null && !(str == ""))
		{
			return str;
		}
		return null;
	}

	public static List<T> SortVector<T>(List<T> vector, string[] names, int options = 0) where T : class
	{
		if (options == ARRAY_ASCENDING)
		{
			if (names.Length == 0)
			{
				return vector;
			}
			if (names.Length == 1)
			{
				return vector.OrderBy((T a) => GetFieldValue(a, names[0])).ToList();
			}
			if (names.Length == 2)
			{
				return (from a in vector
					orderby GetFieldValue(a, names[0]), GetFieldValue(a, names[1])
					select a).ToList();
			}
			if (names.Length == 3)
			{
				return (from a in vector
					orderby GetFieldValue(a, names[0]), GetFieldValue(a, names[1]), GetFieldValue(a, names[2])
					select a).ToList();
			}
			if (names.Length == 4)
			{
				return (from a in vector
					orderby GetFieldValue(a, names[0]), GetFieldValue(a, names[1]), GetFieldValue(a, names[2]), GetFieldValue(a, names[3])
					select a).ToList();
			}
			if (names.Length == 5)
			{
				return (from a in vector
					orderby GetFieldValue(a, names[0]), GetFieldValue(a, names[1]), GetFieldValue(a, names[2]), GetFieldValue(a, names[3]), GetFieldValue(a, names[4])
					select a).ToList();
			}
			return (from a in vector
				orderby GetFieldValue(a, names[0]), GetFieldValue(a, names[1]), GetFieldValue(a, names[2]), GetFieldValue(a, names[3]), GetFieldValue(a, names[4]), GetFieldValue(a, names[5])
				select a).ToList();
		}
		if (names.Length == 0)
		{
			return vector;
		}
		if (names.Length == 1)
		{
			return vector.OrderByDescending((T a) => GetFieldValue(a, names[0])).ToList();
		}
		if (names.Length == 2)
		{
			return (from a in vector
				orderby GetFieldValue(a, names[0]) descending, GetFieldValue(a, names[1]) descending
				select a).ToList();
		}
		if (names.Length == 3)
		{
			return (from a in vector
				orderby GetFieldValue(a, names[0]) descending, GetFieldValue(a, names[1]) descending, GetFieldValue(a, names[2]) descending
				select a).ToList();
		}
		if (names.Length == 4)
		{
			return (from a in vector
				orderby GetFieldValue(a, names[0]) descending, GetFieldValue(a, names[1]) descending, GetFieldValue(a, names[2]) descending, GetFieldValue(a, names[3]) descending
				select a).ToList();
		}
		if (names.Length == 5)
		{
			return (from a in vector
				orderby GetFieldValue(a, names[0]) descending, GetFieldValue(a, names[1]) descending, GetFieldValue(a, names[2]) descending, GetFieldValue(a, names[3]) descending, GetFieldValue(a, names[4]) descending
				select a).ToList();
		}
		return (from a in vector
			orderby GetFieldValue(a, names[0]) descending, GetFieldValue(a, names[1]) descending, GetFieldValue(a, names[2]) descending, GetFieldValue(a, names[3]) descending, GetFieldValue(a, names[4]) descending, GetFieldValue(a, names[5]) descending
			select a).ToList();
	}

	public static object GetFieldValue(object obj, string fieldName)
	{
		FieldInfo field = obj.GetType().GetField(fieldName);
		PropertyInfo property = obj.GetType().GetProperty(fieldName);
		if (field == null && property == null)
		{
			D.LogWarning($"Util.GetFieldValue() :: Field or Property named {fieldName} in {obj} not found.");
			return null;
		}
		if (!(field != null))
		{
			return property.GetValue(obj);
		}
		return field.GetValue(obj);
	}

	public static uint InterpolateColor(uint fromColor, uint toColor, float progress)
	{
		float num = 1f - progress;
		uint num2 = (fromColor >> 24) & 0xFF;
		uint num3 = (fromColor >> 16) & 0xFFu;
		uint num4 = (fromColor >> 8) & 0xFFu;
		uint num5 = fromColor & 0xFFu;
		uint num6 = (toColor >> 24) & 0xFFu;
		uint num7 = (toColor >> 16) & 0xFFu;
		uint num8 = (toColor >> 8) & 0xFFu;
		uint num9 = toColor & 0xFFu;
		uint num10 = (uint)((float)num2 * num + (float)num6 * progress);
		uint num11 = (uint)((float)num3 * num + (float)num7 * progress);
		uint num12 = (uint)((float)num4 * num + (float)num8 * progress);
		uint num13 = (uint)((float)num5 * num + (float)num9 * progress);
		return (num10 << 24) | (num11 << 16) | (num12 << 8) | num13;
	}

	public static Vector2 GetCenter(Transform transform, bool position = true)
	{
		Vector2 result = default(Vector2);
		result.x = transform.position.x;
		result.y = transform.position.y + transform.localScale.y;
		return result;
	}

	public static Vector2 spreadPoint(Vector2 point, float spreadX, float spreadY)
	{
		spreadX = RandomNumber(0f - spreadX / 2f, spreadX / 2f);
		spreadY = RandomNumber(0f - spreadY / 2f, spreadY / 2f);
		return new Vector2(point.x + spreadX, point.y + spreadY);
	}

	public static float GetDistance(float startX, float startY, float endX, float endY)
	{
		float num = startX - endX;
		float num2 = startY - endY;
		return Mathf.Sqrt(num * num + num2 * num2);
	}

	public static float getRotation(float startX, float startY, float endX, float endY)
	{
		float y = startY - endY;
		float x = startX - endX;
		return Mathf.Atan2(y, x) * 180f / (float)Math.PI - 90f;
	}

	public void OpenURLInNewTab(string url)
	{
	}

	public static void OpenURL(string url, bool tab = true)
	{
		Application.OpenURL(url);
	}

	public static bool CharacterNameAllowed(char theChar)
	{
		return " '.,;:<>[]()|&`\u00b4*¿?¡!\u00a8/@<>#*+$%~{}=€^\"\\".IndexOf(theChar) == -1;
	}

	public static bool CharacterNameAllowedWithHashtag(char theChar)
	{
		return " '.,;:<>[]()|&`\u00b4*¿?¡!\u00a8/@<>*+$%~{}=€^\"\\".IndexOf(theChar) == -1;
	}

	public static bool GuildNameAllowed(char theChar)
	{
		return "-_'.,;:<>[]()|&`\u00b4*¿?¡!\u00a8/@<>#*+$%~{}=€^\"\\".IndexOf(theChar) == -1;
	}

	public static bool GuildInitialsAllowed(char theChar)
	{
		return " -_'.,;:<>[]()|&`\u00b4*¿?¡!\u00a8/@<>#*+$%~{}=€^\"\\".IndexOf(theChar) == -1;
	}

	public static bool EmailAllowed(char theChar)
	{
		return " ',;:<>[]()|&`\u00b4*¿?¡!\u00a8/<>#*+$%~{}=€^\"\\".IndexOf(theChar) == -1;
	}

	public static void AddGrayscale(Image image, Shader grayscaleShader, float alphaValue = 0.5f)
	{
		image.material.shader = grayscaleShader;
		image.color = new Color(image.color.r, image.color.g, image.color.b, alphaValue);
	}

	public static void ClearGrayscale(Image image, float alphaValue = 1f)
	{
		image.material.shader = Canvas.GetDefaultCanvasMaterial().shader;
		image.color = new Color(image.color.r, image.color.g, image.color.b, alphaValue);
	}

	public static Color UIntToUnityColor(uint color)
	{
		byte b = (byte)(color >> 24);
		byte num = (byte)(color >> 16);
		byte b2 = (byte)(color >> 8);
		byte b3 = (byte)color;
		return new Color((int)num, (int)b2, (int)b3, (int)b);
	}

	public static string UIntToHTMLString(uint color)
	{
		string text = color.ToString();
		text.Replace("0x", "#");
		return text;
	}

	public static string HTMLSpecialChars(string html)
	{
		if (html == null)
		{
			return "";
		}
		html = html.Replace(" & ", " &amp; ");
		return html;
	}

	public static void SetDropdown(Image dropdown, bool enabled)
	{
		TextMeshProUGUI componentInChildren = dropdown.GetComponentInChildren<TextMeshProUGUI>();
		if (enabled)
		{
			dropdown.color = Color.white;
			componentInChildren.color = new Color(componentInChildren.color.r, componentInChildren.color.g, componentInChildren.color.b, 1f);
		}
		else
		{
			dropdown.color = WHITE_ALPHA_50;
			componentInChildren.color = new Color(componentInChildren.color.r, componentInChildren.color.g, componentInChildren.color.b, 0.5f);
		}
		dropdown.GetComponent<EventTrigger>().enabled = enabled;
	}

	public static T GetComponent<T>(GameObject source) where T : Component
	{
		T component = source.GetComponent<T>();
		if ((UnityEngine.Object)component == (UnityEngine.Object)null)
		{
			return source.AddComponent<T>();
		}
		return component;
	}

	public static string MD5(string str)
	{
		MD5 mD = System.Security.Cryptography.MD5.Create();
		byte[] bytes = Encoding.ASCII.GetBytes(str);
		byte[] array = mD.ComputeHash(bytes);
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("X2"));
		}
		return stringBuilder.ToString().ToLower();
	}

	public static void ChangeLayer(Transform trans, string layer)
	{
		trans.gameObject.layer = LayerMask.NameToLayer(layer);
		for (int i = 0; i < trans.childCount; i++)
		{
			trans.GetChild(i).gameObject.layer = LayerMask.NameToLayer(layer);
			ChangeLayer(trans.GetChild(i), layer);
		}
	}

	public static void ChangeChildrenSpriteRendererMaskInteraction(GameObject root, SpriteMaskInteraction spriteMaskInteraction)
	{
		SpriteRenderer[] componentsInChildren = root.GetComponentsInChildren<SpriteRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].maskInteraction = spriteMaskInteraction;
		}
	}

	public static void ChangeChildrenParticleSystemMaskInteraction(GameObject root, SpriteMaskInteraction spriteMaskInteraction)
	{
		ParticleSystem[] componentsInChildren = root.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].GetComponent<ParticleSystemRenderer>().maskInteraction = spriteMaskInteraction;
		}
	}

	public static bool IsEmptyString(string str)
	{
		return str?.Trim().Equals("") ?? true;
	}

	public static string GetRawString(string html)
	{
		return Regex.Replace(html, "<.+?>", string.Empty);
	}

	public static float ParseFloat(string value)
	{
		return float.Parse(value, CultureInfo.InvariantCulture);
	}

	public static string ConvertFlashHTML(string html)
	{
		string text = "";
		string replacement = "";
		RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Multiline;
		text = new Regex("\\<textformat.*?\\>", options).Replace(html, replacement);
		text = new Regex("\\<p.*?\\>", options).Replace(text, replacement);
		replacement = "<COLOR=$1>";
		text = new Regex("\\<FONT.*?COLOR=\"(.*?)\".*?\\>", options).Replace(text, replacement);
		replacement = "<link=\"\\1\">";
		text = new Regex("\\<A.*?href=\"event:(.*?)\".*?\\>", options).Replace(text, replacement);
		text = text.Replace("</FONT>", "</color>");
		text = text.Replace("</TEXTFORMAT>", "");
		text = text.Replace("</P>", "\n");
		return text.Replace("</A>", "</link>");
	}

	public static string FirstCharToUpper(string input)
	{
		if (string.IsNullOrEmpty(input))
		{
			return "";
		}
		return input.First().ToString().ToUpper() + input.Substring(1);
	}

	public static int versionCompare(string v1, string v2)
	{
		int num = 0;
		int num2 = 0;
		int i = 0;
		for (int j = 0; i < v1.Length || j < v2.Length; j++)
		{
			for (; i < v1.Length && v1[i] != '.'; i++)
			{
				num = num * 10 + (v1[i] - 48);
			}
			for (; j < v2.Length && v2[j] != '.'; j++)
			{
				num2 = num2 * 10 + (v2[j] - 48);
			}
			if (num > num2)
			{
				return 1;
			}
			if (num2 > num)
			{
				return -1;
			}
			num = (num2 = 0);
			i++;
		}
		return 0;
	}

	public static string FilterUnicodeCharacters(string input, string extraChars = null, UnicodeCategory[] extraCategories = null)
	{
		string text = string.Empty;
		UnicodeCategory[] source = new UnicodeCategory[3]
		{
			UnicodeCategory.OtherSymbol,
			UnicodeCategory.Surrogate,
			UnicodeCategory.NonSpacingMark
		};
		for (int i = 0; i < input.Length; i++)
		{
			char c = input[i];
			UnicodeCategory unicodeCategory = char.GetUnicodeCategory(c);
			if (!source.Contains(unicodeCategory) && (extraCategories == null || !extraCategories.Contains(unicodeCategory)) && (extraChars == null || !extraChars.ToCharArray().Contains(c)))
			{
				text += c;
			}
		}
		return text;
	}
}
