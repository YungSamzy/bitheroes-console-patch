using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.service;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

public class ServiceBook
{
	private static Dictionary<int, ServiceRef> _services;

	public static int size => _services.Count;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_services = new Dictionary<int, ServiceRef>();
		foreach (ServiceBookData.Service item in XMLBook.instance.serviceBook.services.lstService)
		{
			_services.Add(item.id, new ServiceRef(item.id, item));
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static List<ServiceRef> GetAllPossibleServices()
	{
		return new List<ServiceRef>(_services.Values);
	}

	public static ServiceRef Lookup(int id)
	{
		if (_services.ContainsKey(id))
		{
			return _services[id];
		}
		return null;
	}

	public static ServiceRef GetFirstServiceByType(int serviceType)
	{
		foreach (KeyValuePair<int, ServiceRef> service in _services)
		{
			if (service.Value.serviceType == serviceType)
			{
				return service.Value;
			}
		}
		return null;
	}

	public static List<ServiceRef> GetServicesByType(int serviceType)
	{
		List<ServiceRef> list = new List<ServiceRef>();
		foreach (KeyValuePair<int, ServiceRef> service in _services)
		{
			if (service.Value.serviceType == serviceType)
			{
				list.Add(service.Value);
			}
		}
		return list;
	}

	public static List<ServiceRef> GetOtherServices()
	{
		List<ServiceRef> list = new List<ServiceRef>();
		foreach (KeyValuePair<int, ServiceRef> service in _services)
		{
			if (!service.Value.visible)
			{
				continue;
			}
			switch (service.Value.serviceType)
			{
			case 1:
			case 2:
			case 3:
			case 6:
			case 10:
			case 11:
			case 15:
				continue;
			}
			if (service.Value.serviceType != 5 || GameData.instance.PROJECT.character.imxG0Data == null)
			{
				list.Add(service.Value);
			}
		}
		return list;
	}
}
