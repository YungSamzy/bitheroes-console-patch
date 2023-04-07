using System.Collections.Generic;
using com.ultrabit.bitheroes.model.item;
using com.ultrabit.bitheroes.model.utility;

namespace com.ultrabit.bitheroes.model.assets;

public class AssetURL
{
	public const int ASSET_TYPE_AUTO = -1;

	public const int ASSET_TYPE_PREFAB = 1;

	public const int ASSET_TYPE_SPRITE = 2;

	public const int ASSET_TYPE_UI_SPRITE = 3;

	public const string SUFFIX_SWF = ".swf";

	public const string SUFFIX_PREFAB = ".prefab";

	public const string SUFFIX_PNG = ".png";

	public const string SUFFIX_JPG = ".jpg";

	public static int ABILITY_EFFECT = 0;

	public static int ABILITY_ICON = 1;

	public static int ABILITY_PROJECTILE = 2;

	public static int AUDIO_MUSIC = 3;

	public static int AUDIO_SOUND = 4;

	public static int AUGMENT = 5;

	public static int AUGMENT_ICON = 6;

	public static int BAIT_ICON = 7;

	public static int BATTLE = 8;

	public static int BATTLE_OTHER = 9;

	public static int BOBBER_ICON = 10;

	public static int CONSUMABLE_ICON = 11;

	public static int CURRENCY_ICON = 12;

	public static int DAILY_ICON = 13;

	public static int DUNGEON = 14;

	public static int DUNGEON_OBJECT = 15;

	public static int DUNGEON_OBJECT_ICON = 16;

	public static int DUNGEON_OVERLAY = 17;

	public static int ENCHANT_ICON = 18;

	public static int EQUIPMENT = 19;

	public static int EQUIPMENT_ICON = 20;

	public static int FAMILIAR_ICON = 21;

	public static int FISH_ICON = 22;

	public static int FUSION_ICON = 23;

	public static int GUILD_HALL_COSMETIC_ICON = 24;

	public static int GUILD_ITEM_ICON = 25;

	public static int GUILD_PERK_ICON = 26;

	public static int INSTANCE = 27;

	public static int INSTANCE_OBJECT = 28;

	public static int MATERIAL_ICON = 29;

	public static int MOUNT = 30;

	public static int MOUNT_ICON = 31;

	public static int NPC = 32;

	public static int PROMO = 33;

	public static int RUNE_ICON = 34;

	public static int SERVICE_ICON = 35;

	public static int ZONE = 36;

	public static int ZONE_NODE = 37;

	public static int PROMO_OBJECT = 38;

	public static int BOBBER = 39;

	public static int CUSTOM_UI = 40;

	public static int OFFERWALL_LOGO = 41;

	public static int IMX_SKIN = 42;

	public static int IMX_OUTFIT = 43;

	public static int IMX_EYES = 44;

	public static int IMX_MASK = 45;

	public static int IMX_HAIR = 46;

	public static int IMX_HAT = 47;

	public static int IMX_HALO = 48;

	public static int IMX_HORN = 49;

	public static int IMX_BACKGROUND = 50;

	public static int IMX_FRAME = 51;

	public static Dictionary<int, string> TYPES = new Dictionary<int, string>
	{
		[1] = "ability_icon",
		[2] = "ability_projectile",
		[3] = "audio_music",
		[4] = "audio_sound",
		[5] = "augment",
		[6] = "augment_icon",
		[7] = "bait_icon",
		[8] = "battle",
		[9] = "battle_other",
		[10] = "bobber_icon",
		[11] = "consumable_icon",
		[12] = "currency_icon",
		[13] = "daily_icon",
		[14] = "dungeon",
		[15] = "dungeon_object",
		[16] = "dungeon_object_icon",
		[17] = "dungeon_overlay",
		[18] = "enchant_icon",
		[19] = "equipment",
		[20] = "equipment_icon",
		[21] = "familiar_icon",
		[22] = "fish_icon",
		[23] = "fusion_icon",
		[24] = "guild_hall_cosmetic_icon",
		[25] = "guild_item_icon",
		[26] = "guild_perk_icon",
		[27] = "instance",
		[28] = "instance_object",
		[29] = "material_icon",
		[30] = "mount",
		[31] = "mount_icon",
		[32] = "npc",
		[33] = "promo",
		[34] = "rune_icon",
		[35] = "service_icon",
		[36] = "zone",
		[37] = "zone_node",
		[38] = "promo_object",
		[39] = "bobber",
		[40] = "custom_ui",
		[41] = "offerwall_logo",
		[42] = "imx_skin",
		[43] = "imx_outfit",
		[44] = "imx_eyes",
		[45] = "imx_mask",
		[46] = "imx_hair",
		[47] = "imx_hat",
		[48] = "imx_halo",
		[49] = "imx_horn",
		[50] = "imx_background",
		[51] = "imx_frame"
	};

	public const string RESOURCES_PREFIX = "assets/";

	public const string RESOURCES_TEMP_PREFIX = "_testing-dlc/";

	public const string BUNDLED_RESOURCES_PREFIX = "Assets/BundledResources";

	public static int GetAssetType(ItemRef item, bool icon)
	{
		if (item != null)
		{
			return GetAssetTypeByItemType(item.itemType, icon);
		}
		return -1;
	}

	public static int GetAssetTypeByItemType(int itemType, bool icon)
	{
		switch (itemType)
		{
		case 5:
			return SERVICE_ICON;
		case 15:
			return AUGMENT_ICON;
		case 13:
			return BAIT_ICON;
		case 14:
			if (!icon)
			{
				return BOBBER;
			}
			return BOBBER_ICON;
		case 4:
			return CONSUMABLE_ICON;
		case 3:
			return CURRENCY_ICON;
		case 11:
		case 18:
			return ENCHANT_ICON;
		case 1:
		case 16:
			if (!icon)
			{
				return EQUIPMENT;
			}
			return EQUIPMENT_ICON;
		case 6:
			if (!icon)
			{
				return NPC;
			}
			return FAMILIAR_ICON;
		case 12:
			if (!icon)
			{
				return FISH_ICON;
			}
			return FISH_ICON;
		case 7:
			return FUSION_ICON;
		case 2:
			return MATERIAL_ICON;
		case 8:
		case 17:
			if (!icon)
			{
				return MOUNT;
			}
			return MOUNT_ICON;
		case 9:
		case 19:
			return RUNE_ICON;
		default:
			D.LogError("david", "AssetLoader::GetAssetType: " + itemType + " not found");
			return -1;
		}
	}

	public static bool IsPrefab(string file)
	{
		if (!file.EndsWith(".swf"))
		{
			return file.EndsWith(".prefab");
		}
		return true;
	}

	public static bool IsSpriteRenderer(string file)
	{
		return !file.EndsWith(".swf");
	}

	public static int GetAssetFileType(string file)
	{
		if (IsPrefab(file))
		{
			return 1;
		}
		return 2;
	}

	public static string GetPath(int type, string asset)
	{
		if (type > 0 && asset != null)
		{
			return TYPES[type] + "/" + GetUrlWithFixedSuffix(asset);
		}
		return null;
	}

	private static string GetUrlWithFixedSuffix(string asset)
	{
		if (asset == null)
		{
			return null;
		}
		if (IsPrefab(asset))
		{
			return asset.Replace(".swf", ".prefab");
		}
		return asset;
	}

	public static string GetZonePath(int zoneID)
	{
		return GetPath(ZONE, "Zone" + zoneID + ".prefab");
	}
}
