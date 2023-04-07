using com.ultrabit.bitheroes.ui.lists.news;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.news;

public class NewsDotClick : MonoBehaviour
{
	public NewsList newsList;

	public int id;

	public void SetID(int _id)
	{
		id = _id;
	}

	public void Click()
	{
		newsList.ScrollToPage(id, directo: true);
	}
}
