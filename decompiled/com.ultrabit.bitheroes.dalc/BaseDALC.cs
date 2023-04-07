using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.extensions;
using com.ultrabit.bitheroes.model.events;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using UnityEngine;

namespace com.ultrabit.bitheroes.dalc;

public class BaseDALC
{
	public int id;

	protected EventDispatcher _dispatcher = new EventDispatcher();

	public void Init(int id)
	{
		this.id = id;
	}

	public virtual void parse(SFSObject sfsob)
	{
		int @int = sfsob.GetInt("act0");
		dispatch(CustomSFSXEvent.getEvent(@int), sfsob);
	}

	protected void dispatch(string e, SFSObject sfsob)
	{
		DispatchEvent(new DALCEvent(e, sfsob));
	}

	protected virtual void send(SFSObject sfsob)
	{
		sfsob.PutInt("dal0", id);
		sfsob.PutUtfString("cli1", Application.version);
		ServerExtension.instance.Send(sfsob);
	}

	protected virtual void send(SFSObject sfsob, bool idleTimer)
	{
		sfsob.PutInt("dal0", id);
		sfsob.PutUtfString("cli1", Application.version);
		ServerExtension.instance.Send(sfsob, null, "ServerExtension", idleTimer);
	}

	public void AddEventListener(string type, EventListenerDelegate listener)
	{
		_dispatcher.AddEventListener(type, listener);
	}

	public void RemoveEventListener(string type, EventListenerDelegate listener)
	{
		_dispatcher.RemoveEventListener(type, listener);
	}

	public void DispatchEvent(BaseEvent e)
	{
		_dispatcher.DispatchEvent(e);
	}
}
