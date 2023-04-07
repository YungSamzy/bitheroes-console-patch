using UnityEngine;

namespace com.ultrabit.bitheroes.ui.avatar;

public class AvatarBackground : MonoBehaviour
{
	[SerializeField]
	private RectTransform backgroundContainer;

	[SerializeField]
	private RectTransform frameContainer;

	[SerializeField]
	private RectTransform frameSeparatorContainer;

	[Header("Settings")]
	[SerializeField]
	private bool _disableBackground;

	[SerializeField]
	private bool _disableFrame;

	[SerializeField]
	private bool _disableFrameSeparator;

	private GameObject _background;

	private GameObject _frame;

	private GameObject _frameSeparator;

	private GameObject _backgroundInstance;

	private GameObject _frameInstance;

	private GameObject _frameSeparatorInstance;

	public GameObject backgroundGameObject => _backgroundInstance;

	public GameObject frameGameObject => _frameInstance;

	public void LoadDetails(GameObject background = null, GameObject frame = null, GameObject frameSeparator = null)
	{
		_background = background;
		_frame = frame;
		_frameSeparator = frameSeparator;
		Initialize();
	}

	private void Initialize()
	{
		if (_backgroundInstance != null)
		{
			Object.Destroy(_backgroundInstance);
		}
		if (_frameInstance != null)
		{
			Object.Destroy(_frameInstance);
		}
		if (_frameSeparatorInstance != null)
		{
			Object.Destroy(_frameSeparatorInstance);
		}
		backgroundContainer.gameObject.SetActive(!_disableBackground);
		frameContainer.gameObject.SetActive(!_disableFrame);
		frameSeparatorContainer.gameObject.SetActive(!_disableFrameSeparator);
		if (_background != null && !_disableBackground)
		{
			_backgroundInstance = Object.Instantiate(_background, backgroundContainer);
		}
		if (_frame != null && !_disableFrame)
		{
			_frameInstance = Object.Instantiate(_frame, frameContainer);
		}
		if (_frameSeparator != null && !_disableFrameSeparator)
		{
			_frameSeparatorInstance = Object.Instantiate(_frameSeparator, frameSeparatorContainer);
		}
	}
}
