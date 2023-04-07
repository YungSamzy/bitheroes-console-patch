using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.book;
using UnityEngine;

namespace com.ultrabit.bitheroes.model.server;

public class ServerRef : BaseRef
{
	private string _url;

	private bool _recommended;

	private bool _newer;

	private List<ServerInstanceRef> _instances;

	public string url => _url;

	public bool recommended => _recommended;

	public bool newer => _newer;

	public List<ServerInstanceRef> instances => _instances;

	public ServerRef(int id, string url, bool recommended, bool newer, List<ServerInstanceRef> instances)
		: base(id)
	{
		_url = url;
		_recommended = recommended;
		_newer = newer;
		_instances = instances;
	}

	public ServerInstanceRef GetInstance(string id)
	{
		foreach (ServerInstanceRef instance in _instances)
		{
			if (instance.id.Equals(id))
			{
				return instance;
			}
		}
		return null;
	}

	public override Sprite GetSpriteIcon()
	{
		throw new NotImplementedException();
	}
}
