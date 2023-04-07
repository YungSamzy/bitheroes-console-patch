using UnityEngine;

namespace com.ultrabit.bitheroes.charactermov;

public class ObstacleZ : MonoBehaviour
{
	private void Start()
	{
		base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, 2.1f);
		base.transform.localScale = new Vector3(base.transform.localScale.x, base.transform.localScale.y, 1f);
	}
}
