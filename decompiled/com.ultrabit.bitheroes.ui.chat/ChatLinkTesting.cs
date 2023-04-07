using com.ultrabit.bitheroes.core;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace com.ultrabit.bitheroes.ui.chat;

public class ChatLinkTesting : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	private TMP_InputField pTextMeshPro;

	private int m_startSelectIndex;

	private int m_endSelectIndex;

	private void Start()
	{
		pTextMeshPro = GetComponent<TMP_InputField>();
		pTextMeshPro.onTextSelection.AddListener(OnTextSelection);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		int num = TMP_TextUtilities.FindIntersectingLink(pTextMeshPro.textComponent, eventData.position, null);
		if (num != -1 && m_endSelectIndex == m_startSelectIndex)
		{
			_ = ref pTextMeshPro.textComponent.textInfo.linkInfo[num];
			GameData.instance.windowGenerator.NewCharacterProfileWindow(GameData.instance.PROJECT.character.toCharacterData());
		}
	}

	private void OnTextSelection(string text, int startSelectionIndex, int endSelectionIndex)
	{
		m_startSelectIndex = startSelectionIndex;
		m_endSelectIndex = endSelectionIndex;
	}

	private void OnDestroy()
	{
		pTextMeshPro.onTextSelection.RemoveListener(OnTextSelection);
	}
}
