using com.ultrabit.bitheroes.dalc;
using com.ultrabit.bitheroes.events.dalcs;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.instance;
using com.ultrabit.bitheroes.model.xml;
using Sfs2X.Core;
using UnityEngine;

public class TestInstance : MonoBehaviour
{
	private void Start()
	{
		InstanceRef instanceRef = new InstanceRef(0, XMLBook.instance.instanceBook.lstInstance[0]);
		GameDALC.instance.AddEventListener(CustomSFSXEvent.getEvent(2), OnEnterInstanceResponse);
		GameDALC.instance.doEnterInstance(instanceRef);
		Debug.Log("TestInstance -> Entering Instance");
	}

	private void OnEnterInstanceResponse(BaseEvent baseEvent)
	{
		Debug.Log("OnEnterInstanceResponse -> Obtained information from server");
		DALCEvent obj = baseEvent as DALCEvent;
		UserDALC.instance.RemoveEventListener(CustomSFSXEvent.getEvent(2), OnEnterInstanceResponse);
		if (obj.sfsob.ContainsKey("err0"))
		{
			Debug.Log("OnEnterInstanceResponse -> ServerConstants.ERROR_CODE ");
		}
		else
		{
			Debug.Log("OnEnterInstanceResponse -> SUCCESS ");
		}
	}
}
