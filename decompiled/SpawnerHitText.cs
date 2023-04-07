using System.Collections.Generic;
using com.ultrabit.bitheroes.ui.battle;
using UnityEngine;

public class SpawnerHitText : MonoBehaviour
{
	public GameObject textHit;

	private float countdown;

	private List<GameObject> textsExisting = new List<GameObject>();

	public void Spawn(string txt, string color)
	{
		if (textsExisting.Count == 0)
		{
			textsExisting.Add(Object.Instantiate(textHit, base.transform));
			textsExisting[textsExisting.Count - 1].GetComponent<TextOnStage>().Set(txt, color);
			return;
		}
		for (int i = 0; i < textsExisting.Count; i++)
		{
			if (!textsExisting[i].activeInHierarchy)
			{
				textsExisting[i].GetComponent<TextOnStage>().Set(txt, color);
				textsExisting[i].SetActive(value: true);
				return;
			}
		}
		textsExisting.Add(Object.Instantiate(textHit, base.transform));
		textsExisting[textsExisting.Count - 1].GetComponent<TextOnStage>().Set(txt, color);
	}

	private void Update()
	{
		countdown += Time.deltaTime;
		if (!(countdown >= 1f))
		{
			return;
		}
		float num = Mathf.Round(Random.Range(0, 5));
		if (num != 0f)
		{
			if (num != 1f)
			{
				if (num != 2f)
				{
					if (num != 3f)
					{
						if (num != 4f)
						{
							if (num == 5f)
							{
								Spawn("+125", BattleText.COLOR_GREEN);
							}
						}
						else
						{
							Spawn("Electrical", BattleText.COLOR_YELLOW);
						}
					}
					else
					{
						Spawn("Evade!", BattleText.COLOR_BLUE);
					}
				}
				else
				{
					Spawn("-4052", BattleText.COLOR_RED);
				}
			}
			else
			{
				Spawn("-2321", BattleText.COLOR_RED);
			}
		}
		else
		{
			Spawn("+232", BattleText.COLOR_GREEN);
		}
		num = Mathf.Round(Random.Range(0, 3));
		if (num != 0f)
		{
			if (num != 1f)
			{
				if (num == 2f)
				{
					countdown = 0.7f;
				}
				else
				{
					countdown = 0.9f;
				}
			}
			else
			{
				countdown = 0.3f;
			}
		}
		else
		{
			countdown = 0f;
		}
	}
}
