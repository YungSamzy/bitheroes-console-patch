using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StressTest : MonoBehaviour
{
	public GameObject btnPrefab;

	public GameObject mask;

	public TextAsset json;

	public RectTransform grid;

	public RectTransform animationPanel;

	private string currentCat = "npc";

	private Resos ress;

	private GameObject lastPrefab;

	private Transform holder;

	private List<Animator> anims;

	private float scaleFactor = 1f;

	private void parse()
	{
		if (ress == null)
		{
			ress = new Resos();
			JsonUtility.FromJsonOverwrite(json.text, ress);
		}
	}

	private void OnEnable()
	{
		Invoke("showButtons", 0.1f);
	}

	public void showButtons()
	{
		mask.SetActive(value: true);
		parse();
		changeCategories(currentCat);
	}

	private void clearBtns()
	{
		foreach (Transform item in grid)
		{
			Object.Destroy(item.gameObject);
		}
	}

	private void clearHolder()
	{
		if (holder == null)
		{
			holder = GameObject.Find("MainHolder").transform;
		}
		foreach (Transform item in holder)
		{
			Object.Destroy(item.gameObject);
		}
	}

	public void createFirstPrefab(string path)
	{
		clearHolder();
		anims = new List<Animator>();
		addPrefab(path);
		createAnimButtons();
	}

	public void clearAnimButtons()
	{
		foreach (Transform item in animationPanel)
		{
			Object.Destroy(item.gameObject);
		}
	}

	private void playAnim(string clip)
	{
		foreach (Animator anim in anims)
		{
			if (anim != null)
			{
				anim.Play(clip, 0, 0f);
			}
		}
	}

	public void createAnimButtons()
	{
		clearAnimButtons();
		if (anims.Count == 0)
		{
			return;
		}
		AnimationClip[] animationClips = anims[0].runtimeAnimatorController.animationClips;
		foreach (AnimationClip ac in animationClips)
		{
			GameObject obj = Object.Instantiate(btnPrefab, animationPanel);
			obj.transform.Find("Text").GetComponent<Text>().text = ac.name;
			obj.GetComponent<Button>().onClick.AddListener(delegate
			{
				playAnim(ac.name);
			});
		}
	}

	public void plusFive()
	{
		for (int i = 0; i < 5; i++)
		{
			GameObject obj = addFromPrefab();
			Vector3 zero = Vector3.zero;
			zero.x = Random.Range(-380f, 380f);
			zero.y = Random.Range(-170f, 250f);
			obj.transform.localPosition = zero;
		}
	}

	public void addPrefab(string path)
	{
		mask.SetActive(value: false);
		lastPrefab = Resources.Load<GameObject>(path);
		addFromPrefab();
	}

	private GameObject addFromPrefab()
	{
		GameObject gameObject = Object.Instantiate(lastPrefab, holder);
		gameObject.transform.position = Vector3.zero;
		gameObject.transform.localScale = Vector3.one * scaleFactor;
		Animator animator = gameObject.GetComponent<Animator>();
		if (animator == null)
		{
			Transform transform = gameObject.transform.Find(gameObject.name);
			animator = ((!(transform != null)) ? gameObject.GetComponentInChildren<Animator>() : transform.GetComponent<Animator>());
		}
		if (animator != null)
		{
			anims.Add(animator);
		}
		return gameObject;
	}

	public void changeCategories(string cat)
	{
		currentCat = cat;
		switch (cat)
		{
		case "mount":
			scaleFactor = 100f;
			createButtons(ress.mount);
			break;
		case "npc":
			scaleFactor = 1f;
			createButtons(ress.npc);
			break;
		case "battle":
			scaleFactor = 1f;
			createButtons(ress.battle);
			break;
		}
	}

	private void createButtons(List<ResItem> list)
	{
		clearBtns();
		float num = 0f;
		foreach (ResItem it in list)
		{
			string text = currentCat + "_" + it.name;
			int @int = PlayerPrefs.GetInt(text, 0);
			Debug.Log($"Status: {text} -> {@int}");
			Color color = new Color(0.2892934f, 0.2917984f, 0.3113208f);
			switch (@int)
			{
			case 1:
				color = new Color(1f, 0f, 0.1301498f);
				break;
			case 2:
				color = new Color(0.1382794f, 61f / 106f, 0.07329121f);
				break;
			}
			GameObject obj = Object.Instantiate(btnPrefab, grid);
			obj.GetComponent<Image>().color = color;
			obj.transform.Find("Text").GetComponent<Text>().text = it.name;
			obj.GetComponent<Button>().onClick.AddListener(delegate
			{
				createFirstPrefab(it.path);
			});
			num += 43f;
		}
		num += 215f;
		Vector2 sizeDelta = grid.sizeDelta;
		sizeDelta.y = num / 5f;
		grid.sizeDelta = sizeDelta;
		grid.anchoredPosition = Vector2.zero;
	}

	public void markAnimation(int mark)
	{
		string text = currentCat + "_" + lastPrefab.name;
		PlayerPrefs.SetInt(text, mark);
		Debug.Log($"Save: {text} : {mark}");
		PlayerPrefs.Save();
	}
}
