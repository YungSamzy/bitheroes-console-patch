using com.ultrabit.bitheroes.ui.lists.raidlist;
using UnityEngine;

public class RaidDotClick : MonoBehaviour
{
	public RaidList raidList;

	public int id;

	public void SetID(int _id)
	{
		id = _id;
	}

	public void Click()
	{
		raidList.ScrollToPage(id, directo: true);
	}
}
