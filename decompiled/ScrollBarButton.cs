using frame8.Logic.Misc.Visual.UI.MonoBehaviours;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBarButton : MonoBehaviour
{
	private bool MouseOverButton;

	private float countdown;

	private ScrollbarFixer8 scrollBar;

	public Button upBtn;

	public Button downBtn;

	private void Awake()
	{
		if ((bool)base.transform.GetChild(0).GetComponent<Scrollbar>())
		{
			scrollBar = base.transform.GetChild(0).GetComponent<ScrollbarFixer8>();
			if (upBtn != null && upBtn.gameObject != null)
			{
				scrollBar.OnScrollBarShowChanged.AddListener(upBtn.gameObject.SetActive);
			}
			if (downBtn != null && downBtn.gameObject != null)
			{
				scrollBar.OnScrollBarShowChanged.AddListener(downBtn.gameObject.SetActive);
			}
		}
	}

	public void ClickButtonScroll(bool toUp)
	{
		if (!base.transform.parent.GetComponent<ScrollRect>())
		{
			if (toUp)
			{
				base.transform.parent.BroadcastMessage("OnScroll", 1);
			}
			else
			{
				base.transform.parent.BroadcastMessage("OnScroll", -1);
			}
		}
		else if (toUp)
		{
			scrollBar.SetNormalizedPosition(scrollBar.GetNormalizedPosition() + (double)(1f / (scrollBar.viewport.GetChild(0).GetComponent<RectTransform>().rect.height / (scrollBar.viewport.rect.height / 2f))));
		}
		else
		{
			scrollBar.SetNormalizedPosition(scrollBar.GetNormalizedPosition() - (double)(1f / (scrollBar.viewport.GetChild(0).GetComponent<RectTransform>().rect.height / (scrollBar.viewport.rect.height / 2f))));
		}
	}

	private void Update()
	{
		if (MouseOverButton)
		{
			countdown += Time.deltaTime;
			if (countdown > 1f)
			{
				base.transform.parent.BroadcastMessage("OnScroll", 1);
				countdown = 0f;
			}
		}
	}
}
