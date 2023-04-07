using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.language;
using com.ultrabit.bitheroes.model.xml.zone;
using com.ultrabit.bitheroes.ui;

namespace com.ultrabit.bitheroes.model.zone;

public class ZoneNotification
{
	public const int TYPE_COMPLETE = 1;

	private static Dictionary<string, int> TYPES = new Dictionary<string, int> { ["complete"] = 1 };

	private ZoneXMLData.Notification notificationData;

	public int type
	{
		get
		{
			if (!TYPES.ContainsKey(notificationData.type))
			{
				return 0;
			}
			return TYPES[notificationData.type];
		}
	}

	public string name => notificationData.name;

	public string desc => notificationData.desc;

	public ZoneNotification(ZoneXMLData.Notification notificationData)
	{
		this.notificationData = notificationData;
	}

	public DialogWindow DoNotification()
	{
		return GameData.instance.windowGenerator.NewConfirmMessageWindow(Language.GetString(name), Language.GetString(desc));
	}

	public static int getType(string type)
	{
		return TYPES[type.ToLowerInvariant()];
	}
}
