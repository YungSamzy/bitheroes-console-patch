using com.ultrabit.bitheroes.ui.lists.brawllist;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.brawl;

public class BrawlDotClick : MonoBehaviour
{
	public BrawlCreateList brawlCreateList;

	public int id;

	public void SetID(int _id)
	{
		id = _id;
	}

	public void Click()
	{
		brawlCreateList.ScrollToPage(id, directo: true);
	}
}
