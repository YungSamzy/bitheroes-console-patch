using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.model.xml;

namespace com.ultrabit.bitheroes.model.server;

public class ServerBook
{
	private static List<ServerRef> _SERVERS;

	public static void Init()
	{
		_SERVERS = new List<ServerRef>();
		foreach (ServerBookData.Server item in XMLBook.instance.serverBookData.servers.server)
		{
			List<ServerInstanceRef> list = new List<ServerInstanceRef>();
			foreach (ServerBookData.ServerInstance item2 in item.lstInstance)
			{
				list.Add(new ServerInstanceRef(item2.id, item2.url));
			}
			_SERVERS.Add(new ServerRef(item.id, item.url, Util.GetBoolFromStringProperty(item.recommended), newer: false, list));
		}
	}

	public static ServerRef GetRecommendedServer()
	{
		foreach (ServerRef sERVER in _SERVERS)
		{
			if (sERVER != null && sERVER.recommended)
			{
				return sERVER;
			}
		}
		return GetFirstServer();
	}

	public static List<ServerRef> GetOtherServers()
	{
		List<ServerRef> list = new List<ServerRef>();
		foreach (ServerRef sERVER in _SERVERS)
		{
			if (sERVER != null && !sERVER.recommended)
			{
				list.Add(sERVER);
			}
		}
		return list;
	}

	public static ServerRef GetFirstServer()
	{
		foreach (ServerRef sERVER in _SERVERS)
		{
			if (sERVER != null)
			{
				return sERVER;
			}
		}
		return null;
	}

	public static ServerRef GetRandomServer()
	{
		Random random = new Random();
		return _SERVERS[random.Next(0, _SERVERS.Count - 1)];
	}

	public static ServerRef Lookup(int id)
	{
		foreach (ServerRef sERVER in _SERVERS)
		{
			if (sERVER != null && sERVER.id == id)
			{
				return sERVER;
			}
		}
		return null;
	}

	public static int size()
	{
		return _SERVERS.Count;
	}
}
