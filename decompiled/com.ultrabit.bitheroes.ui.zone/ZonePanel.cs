using System.Collections.Generic;
using com.ultrabit.bitheroes.model.zone;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.zone;

public class ZonePanel : MonoBehaviour
{
	private List<ZoneNode> _nodes = new List<ZoneNode>();

	public List<ZoneNode> nodes => _nodes;

	public void LoadDetails(ZoneRef zoneRef, ZoneWindow zoneWindow)
	{
		Transform transform = base.transform.Find("nodes");
		int childCount = transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = transform.GetChild(i);
			if (!child.TryGetComponent<ZoneNode>(out var component))
			{
				component = child.gameObject.AddComponent<ZoneNode>();
				_nodes.Add(component);
			}
			ZoneNodeRef nodeRef = zoneRef.getNodeRef(i + 1);
			component.Setup(nodeRef, this, zoneWindow);
		}
	}
}
