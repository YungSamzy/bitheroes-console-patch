using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.model.xml;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.model.material;

public class MaterialBook
{
	private static Dictionary<int, MaterialRef> _materials;

	public static int size => _materials.Count;

	public static IEnumerator Init(UnityAction<float> onUpdatedProgress)
	{
		_materials = new Dictionary<int, MaterialRef>();
		foreach (MaterialBookData.Material item in XMLBook.instance.materialBook.lstMaterial)
		{
			_materials.Add(item.id, new MaterialRef(item.id, item));
		}
		yield return null;
		onUpdatedProgress?.Invoke(XMLBook.instance.UpdateProgress());
	}

	public static List<MaterialRef> GetAllPossibleMaterials()
	{
		return new List<MaterialRef>(_materials.Values);
	}

	public static MaterialRef Lookup(int id)
	{
		if (_materials.ContainsKey(id))
		{
			return _materials[id];
		}
		return null;
	}
}
