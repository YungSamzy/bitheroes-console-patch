using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace com.ultrabit.bitheroes.ui.character;

public class CharacterPuppetDefault : CharacterPuppet
{
	public static string[] HAIR_COLORS = new string[11]
	{
		"#FFDC81", "#FFD075", "#9C6F4E", "#FFA37F", "#FFA0B0", "#FFA5E6", "#C0F160", "#9DE8FF", "#FF8D75", "#E5E9F2",
		"#64615A"
	};

	public static double[][] HAIR_COLOR_FILTERS = new double[11][]
	{
		new double[20]
		{
			1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0,
			0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0
		},
		new double[20]
		{
			1.2441108226776123, 0.10939215123653412, -0.35350295901298523, 0.0, 0.0, -0.13309432566165924, 1.052661418914795, 0.08043288439512253, 0.0, 0.0,
			0.200151264667511, -0.40301764011383057, 1.2028663158416748, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0
		},
		new double[20]
		{
			0.6456354260444641, 0.33453458547592163, -0.18016992509365082, 0.0, -59.29999923706055, 0.05225643888115883, 0.6629353165626526, 0.08480831980705261, 0.0, -59.29999923706055,
			0.2557492256164551, 0.02163708582520485, 0.5226137042045593, 0.0, -59.29999923706055, 0.0, 0.0, 0.0, 1.0, 0.0
		},
		new double[20]
		{
			1.1460480690002441, 0.6321127414703369, -0.7781606912612915, 0.0, -30.0, -0.14729191362857819, 0.9135121703147888, 0.23377977311611176, 0.0, -29.999998092651367,
			0.6298382878303528, -0.5628293752670288, 0.932991087436676, 0.0, -30.0, 0.0, 0.0, 0.0, 1.0, 0.0
		},
		new double[20]
		{
			1.0080269575119019, 1.1419968605041504, -1.1500238180160522, 0.0, -20.0, -0.16401538252830505, 0.7933418154716492, 0.3706735372543335, 0.0, -19.999998092651367,
			1.0038199424743652, -0.6537068486213684, 0.6498869061470032, 0.0, -20.000001907348633, 0.0, 0.0, 0.0, 1.0, 0.0
		},
		new double[20]
		{
			0.5215581655502319, 1.4782971143722534, -0.9998554587364197, 0.0, -19.999996185302734, 0.013486988842487335, 0.5990179181098938, 0.3874950408935547, 0.0, -19.999998092651367,
			1.078917145729065, -0.1599448174238205, 0.08102760463953018, 0.0, -20.0, 0.0, 0.0, 0.0, 1.0, 0.0
		},
		new double[20]
		{
			0.8915532827377319, -0.5391061902046204, 0.6475529074668884, 0.0, 0.0, 0.053340520709753036, 1.1565316915512085, -0.20987224578857422, 0.0, 0.0,
			-0.6051281094551086, 0.47337770462036133, 1.131750226020813, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0
		},
		new double[20]
		{
			-0.5993884205818176, 1.1423358917236328, 0.45705240964889526, 0.0, 0.0, 0.46206340193748474, 0.49507033824920654, 0.04286620765924454, 0.0, 0.0,
			0.14398479461669922, 1.6314244270324707, -0.7754093408584595, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0
		},
		new double[20]
		{
			1.103558897972107, 0.746679961681366, -0.8502387404441833, 0.0, -49.999996185302734, -0.14292900264263153, 0.879969596862793, 0.262959361076355, 0.0, -49.999996185302734,
			0.7119631171226501, -0.5678313970565796, 0.855868399143219, 0.0, -50.000003814697266, 0.0, 0.0, 0.0, 1.0, 0.0
		},
		new double[20]
		{
			0.22033999860286713, 0.6914600133895874, 0.08819998800754547, 0.0, 10.0, 0.32033997774124146, 0.5914599299430847, 0.08819999545812607, 0.0, 10.0,
			0.32034000754356384, 0.6914598941802979, -0.011799998581409454, 0.0, 9.999999046325684, 0.0, 0.0, 0.0, 1.0, 0.0
		},
		new double[20]
		{
			0.30885303020477295, 0.5210369825363159, 0.07011000066995621, 0.0, -83.6500015258789, 0.2638530135154724, 0.5660369992256165, 0.07011000066995621, 0.0, -83.6500015258789,
			0.2638530135154724, 0.5210369825363159, 0.11511000990867615, 0.0, -83.6500015258789, 0.0, 0.0, 0.0, 1.0, 0.0
		}
	};

	public static uint[] SKIN_COLORS = new uint[20]
	{
		16762768u, 13412723u, 10583131u, 7557953u, 15190701u, 14537932u, 10524700u, 6138052u, 14255882u, 14565922u,
		7030585u, 12178878u, 7842048u, 7161378u, 15600972u, 15637005u, 9951522u, 2248153u, 2279385u, 13378265u
	};

	public static string[] SKIN_COLORS_STRING = new string[20]
	{
		"#ffc790", "#cca973", "#a17c5b", "#735341", "#e7caad", "#ddd4cc", "#a0981c", "#5da8c4", "#d9870a", "#e42222",
		"#6b4739", "#b9d5be", "#77a900", "#6d4622", "#ee0d4c", "#ee9a0d", "#97d922", "#224dd9", "#22c7d9", "#cc22d9"
	};

	public const int SKIN_COLORS_NORMAL = 6;

	public const int SKIN_COLORS_SPECIAL = 20;

	public const int HAIR_STYLES_NORMAL = 17;

	public const int HAIR_STYLES_SPECIAL = 17;

	public const int HAIR_COLORS_NORMAL = 11;

	public const int HAIR_COLORS_SPECIAL = 11;

	public const string SUBPART_EYESMOUTH = "eyesMouth";

	public static string BP_HEAD_EYESMOUTH = "head_eyesMouth";

	[Header("Character Puppet Default References")]
	public Transform maleSkin;

	public Transform femaleSkin;

	[SerializeField]
	protected SpriteRenderer headEyesMouth;

	[SerializeField]
	protected CharacterPuppetHead headManager;

	private CharacterPuppetInfoDefault _puppetInfoDefault;

	public override Gender gender
	{
		get
		{
			return base.gender;
		}
		set
		{
			base.gender = value;
			maleSkin.gameObject.SetActive(value == Gender.MALE);
			femaleSkin.gameObject.SetActive(value != Gender.MALE);
			headManager.SetGenre(value);
		}
	}

	public int skinColor
	{
		get
		{
			return headManager.skinColor;
		}
		set
		{
			headManager.skinColor = value;
			if (maleSkin.TryGetComponent<CharacterPuppetSkin>(out var component))
			{
				component.SetSkinColor(value);
			}
			if (femaleSkin.TryGetComponent<CharacterPuppetSkin>(out component))
			{
				component.SetSkinColor(value);
			}
			if (rightHandSkin.TryGetComponent<CharacterPuppetSkin>(out component))
			{
				component.SetSkinColor(value);
			}
			if (leftHandSkin.TryGetComponent<CharacterPuppetSkin>(out component))
			{
				component.SetSkinColor(value);
			}
			if (rightFootSkin.TryGetComponent<CharacterPuppetSkin>(out component))
			{
				component.SetSkinColor(value);
			}
			if (leftFootSkin.TryGetComponent<CharacterPuppetSkin>(out component))
			{
				component.SetSkinColor(value);
			}
		}
	}

	public Transform activeSkin
	{
		get
		{
			if (!maleSkin.gameObject.activeSelf)
			{
				return femaleSkin;
			}
			return maleSkin;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		characterPuppetEquipmentReference.Add(CharacterPuppet.BP_RIGHT_HAND_SKIN, new CharacterPuppetEquipmentRef((SpriteRenderer)rightHandSkin, Vector3.zero));
		characterPuppetEquipmentReference.Add(CharacterPuppet.BP_RIGHT_FOOT_SKIN, new CharacterPuppetEquipmentRef((SpriteRenderer)rightFootSkin, Vector3.zero));
		characterPuppetEquipmentReference.Add(CharacterPuppet.BP_LEFT_FOOT_SKIN, new CharacterPuppetEquipmentRef((SpriteRenderer)leftFootSkin, Vector3.zero));
		characterPuppetEquipmentReference.Add(CharacterPuppet.BP_TORSO_SKIN, new CharacterPuppetEquipmentRef((SortingGroup)torsoSkin, Vector3.zero));
		characterPuppetEquipmentReference.Add(CharacterPuppet.BP_HEAD_SKIN, new CharacterPuppetEquipmentRef((SortingGroup)headSkin, Vector3.zero));
		characterPuppetEquipmentReference.Add(BP_HEAD_EYESMOUTH, new CharacterPuppetEquipmentRef(headEyesMouth, Vector3.zero));
		characterPuppetEquipmentReference.Add(CharacterPuppet.BP_LEFT_HAND_SKIN, new CharacterPuppetEquipmentRef((SpriteRenderer)leftHandSkin, Vector3.zero));
	}

	protected override void Start()
	{
		base.Start();
	}

	public override void ShowHair(bool show)
	{
		headHair.gameObject.SetActive(show);
	}

	public override void ShowHeadSkin(bool show)
	{
		headSkin.gameObject.SetActive(show);
		headEyesMouth.gameObject.SetActive(show);
	}

	public override void SetCharacterPuppet(CharacterPuppetInfo puppetInfo)
	{
		if (!(puppetInfo is CharacterPuppetInfoDefault))
		{
			throw new Exception(string.Format("{0}.{1}() :: Trying to set with {2}.", GetType(), "SetCharacterPuppet", puppetInfo.GetType()));
		}
		_puppetInfoDefault = (CharacterPuppetInfoDefault)puppetInfo;
		_enableLoading = _puppetInfoDefault.enableLoading;
		gender = _puppetInfoDefault.gender;
		skinColor = _puppetInfoDefault.skinColorID;
		headManager.hairID = Math.Max(_puppetInfoDefault.hairID, 1);
		headManager.hairColor = _puppetInfoDefault.hairColorID;
		headManager.scale = _puppetInfoDefault.headScale;
		_showHelm = _puppetInfoDefault.showHelm;
		_showMount = _puppetInfoDefault.showMount;
		_showBody = _puppetInfoDefault.showBody;
		_showAccessory = _puppetInfoDefault.showAccessory;
		CharacterPuppetEquipmentRef value = new CharacterPuppetEquipmentRef(torso, CharacterPuppet.OFFSET_TORSO);
		if (characterPuppetEquipmentReference.ContainsKey("torso"))
		{
			characterPuppetEquipmentReference["torso"] = value;
		}
		else
		{
			characterPuppetEquipmentReference.Add("torso", value);
		}
		CharacterPuppetEquipmentRef value2 = new CharacterPuppetEquipmentRef(head, CharacterPuppet.OFFSET_HEAD);
		if (characterPuppetEquipmentReference.ContainsKey("head"))
		{
			characterPuppetEquipmentReference["head"] = value2;
		}
		else
		{
			characterPuppetEquipmentReference.Add("head", value2);
		}
		base.transform.localScale *= _puppetInfoDefault.scale;
		SetEquipment(_puppetInfoDefault.equipment, _puppetInfoDefault.equipmentOverride);
		_childRenderers = base.transform.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
	}

	public static string GetHairColor(int id)
	{
		if (id < 0 || id >= HAIR_COLORS.Length)
		{
			return "#FFFFFF";
		}
		return HAIR_COLORS[id];
	}

	public static string GetSkinColor(int id)
	{
		if (id < 0 || id >= SKIN_COLORS.Length)
		{
			return "#FFFFFF";
		}
		return SKIN_COLORS_STRING[id];
	}
}
