using UnityEngine;

namespace com.ultrabit.bitheroes.ui.tutorial;

public class TutorialPopUpSettings
{
	private string _text;

	private int _arrowPosition;

	private object _target;

	private float _offset;

	private bool _indicator;

	private bool _button;

	private bool _glow;

	private int _width;

	private Vector2? _position;

	public string text => _text;

	public int arrowPosition => _arrowPosition;

	public object target => _target;

	public float offset => _offset;

	public bool indicator => _indicator;

	public bool button => _button;

	public bool glow => _glow;

	public int width => _width;

	public Vector2? position => _position;

	public TutorialPopUpSettings(string text, int arrowPosition = 0, object target = null, float offset = 0f, bool indicator = false, bool button = false, bool glow = true, int width = 300, Vector2? position = null)
	{
		_text = text;
		_arrowPosition = arrowPosition;
		_target = target;
		_offset = offset;
		_indicator = indicator;
		_button = button;
		_glow = glow;
		_width = width;
		_position = position;
	}
}
