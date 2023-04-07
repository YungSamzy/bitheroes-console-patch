using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.assets;
using UnityEngine;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui;

[RequireComponent(typeof(Button))]
public class OverrideUIButtonColor : MonoBehaviour
{
	public ButtonSquareColor color;

	private Button _buttonComponent;

	private SpriteState _spriteState;

	private Sprite _up;

	private Sprite _down;

	private Sprite _over;

	private void Awake()
	{
		_buttonComponent = GetComponent<Button>();
		FindReferences();
	}

	private void OnEnable()
	{
		if (_up != null)
		{
			_buttonComponent.image.sprite = _up;
		}
		_spriteState = _buttonComponent.spriteState;
		if (_down != null)
		{
			_spriteState.pressedSprite = _down;
		}
		if (_over != null)
		{
			_spriteState.highlightedSprite = _over;
		}
		_buttonComponent.spriteState = _spriteState;
	}

	private void FindReferences()
	{
		_up = ((color == ButtonSquareColor.Default) ? null : GameData.instance.main.assetLoader.GetButtonSprite(color, ButtonSquareState.Up));
		_down = ((color == ButtonSquareColor.Default) ? null : GameData.instance.main.assetLoader.GetButtonSprite(color, ButtonSquareState.Down));
		_over = ((color == ButtonSquareColor.Default) ? null : GameData.instance.main.assetLoader.GetButtonSprite(color, ButtonSquareState.Over));
	}

	public void SetCustom(ButtonSquareColor customColor)
	{
		if (customColor != ButtonSquareColor.Default)
		{
			color = customColor;
			FindReferences();
			OnEnable();
		}
	}
}
