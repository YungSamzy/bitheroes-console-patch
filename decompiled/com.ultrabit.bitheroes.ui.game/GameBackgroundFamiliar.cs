using UnityEngine;

namespace com.ultrabit.bitheroes.ui.game;

public class GameBackgroundFamiliar : MonoBehaviour
{
	private int _index;

	public int index
	{
		get
		{
			return _index;
		}
		set
		{
			_index = value;
		}
	}
}
