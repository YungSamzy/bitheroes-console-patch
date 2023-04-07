using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.assets;
using com.ultrabit.bitheroes.ui.assets;
using com.ultrabit.bitheroes.ui.dungeon;
using com.ultrabit.bitheroes.ui.grid;
using com.ultrabit.bitheroes.ui.utility;
using DG.Tweening;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.instance;

public class InstanceFishingBobber : GridObject
{
	private const float SPEED = 0.001f;

	private const float DISTANCE_THRESHOLD = 0.8f;

	private const string LINE_COLOR = "#B5C1CA";

	public const float SLOPE_GAIN = 0.035f;

	public const float SLOPE_TARGET_MIN = 0.5f;

	public const float SLOPE_TARGET_MAX = 1f;

	public const float SLOPE_TIGHT_SPEED = 4f;

	public DungeonExclamationPoint exclamationIcon;

	private InstancePlayer _player;

	private List<Vector3> _path;

	private GameObject _assetb;

	private float _speedb;

	private GameObject _guide;

	private BezierLineRenderer _guideLine;

	private GameObject _line;

	private BezierLineRenderer _lineRenderer;

	private List<Vector3> _linePath;

	private float _perc;

	private bool _movingb;

	private float _slopeTarget = 1f;

	private float _slopeSpeed = 1f;

	private float _slope = 1f;

	public void CreateInstanceFishingBobber(Instance instance, InstancePlayer player, List<Vector3> path = null)
	{
		base.transform.SetParent(player.transform, worldPositionStays: false);
		LoadDetails(instance, null);
		_player = player;
		_path = path;
		_speedb = 0.001f + 0.8f / (float)_player.fishingData.distance;
		_linePath = new List<Vector3>();
		RemoveExclamation();
		LoadAssets();
		UpdateLine();
		GameData.instance.main.DISPATCHER.FRAME_UPDATE.RemoveListener(OnUpdate);
		GameData.instance.main.DISPATCHER.FRAME_UPDATE.AddListener(OnUpdate);
		if (_path != null)
		{
			StartPath();
			DoUpdate();
		}
	}

	public void LoadAssets()
	{
		if (!(_assetb != null))
		{
			Transform transform = null;
			GameObject gameObjectAsset = GameData.instance.main.assetLoader.GetGameObjectAsset(AssetURL.BOBBER, _player.fishingData.bobberRef.asset, instantiate: false);
			if (gameObjectAsset != null)
			{
				transform = gameObjectAsset.transform;
			}
			if (transform != null)
			{
				Transform transform2 = Object.Instantiate(transform, base.transform);
				transform2.localScale = new Vector3(200f, 200f, 1f);
				_assetb = transform2.gameObject;
				_assetb.AddComponent<SWFAsset>();
			}
			OnAssetLoaded();
		}
	}

	private void OnAssetLoaded()
	{
		SetAsset(_assetb);
		UpdateAnimation();
	}

	public void UpdateAnimation()
	{
		if (_movingb)
		{
			PlayAnimation("walk");
		}
		else
		{
			PlayAnimation("idle");
		}
	}

	private void StartPath()
	{
		if (!_movingb)
		{
			_movingb = true;
			UpdateAnimation();
		}
	}

	private void StopPath()
	{
		if (_movingb)
		{
			RemoveGuide();
			_movingb = false;
			PlayAnimation("hit", loop: false, "idle");
			_player.BOBBER_COMPLETE.Invoke(null);
		}
	}

	private void CreateGuide()
	{
		if (!(_guide != null))
		{
			_guide = new GameObject();
			_guide.name = "GUIDE";
			_guide.transform.SetParent(_player.transform, worldPositionStays: false);
			_guide.AddComponent<LineRenderer>();
			_guideLine = _guide.AddComponent<BezierLineRenderer>();
			_guideLine.LoadDetails(Color.red);
			_guideLine.DrawCurve(_path);
		}
	}

	private void RemoveGuide()
	{
		if (_guide != null)
		{
			Object.Destroy(_guide);
			_guide = null;
		}
	}

	private void OnUpdate(object e)
	{
		DoUpdate();
	}

	public void DoUpdate()
	{
		if (_movingb)
		{
			_perc += _speedb;
			if (_perc >= 1f)
			{
				_perc = 1f;
				StopPath();
			}
			UpdatePosition();
		}
		UpdateLine();
	}

	private void UpdatePosition()
	{
		Vector2 vector = _path[(int)(_perc * 100f)];
		base.x = vector.x;
		base.y = vector.y;
	}

	private void UpdateLine()
	{
		if (_line == null)
		{
			_line = new GameObject();
			_line.name = "LINE";
			_line.transform.SetParent(_player.transform, worldPositionStays: false);
			_line.AddComponent<LineRenderer>();
			_lineRenderer = _line.AddComponent<BezierLineRenderer>();
			_lineRenderer.LoadDetails("#B5C1CA");
		}
		if (_slope != _slopeTarget)
		{
			float num = 0.035f * _slopeSpeed;
			if (_slopeTarget > _slope)
			{
				_slope += num;
				if (_slope >= _slopeTarget)
				{
					_slope = _slopeTarget;
				}
			}
			else
			{
				_slope -= num;
				if (_slope <= _slopeTarget)
				{
					_slope = _slopeTarget;
				}
			}
		}
		Vector2 rodPosition = _player.GetRodPosition();
		_lineRenderer.lineRenderer.positionCount = 0;
		_linePath.Clear();
		_linePath = BezierLineRenderer.CalculateCurve(_player.transform.TransformPoint(rodPosition), _player.transform.TransformPoint(new Vector2(base.localX * _slope, base.localY * _slope)), _player.transform.TransformPoint(new Vector2(base.localX * _slope, base.localY * _slope)), _player.transform.TransformPoint(new Vector2(base.localX, base.localY)));
		_lineRenderer.DrawCurve(_linePath);
	}

	private void RemoveLine()
	{
		if (_line != null)
		{
			Object.Destroy(_line);
			_line = null;
		}
	}

	public void SetSlopeTarget(float slope, float speed, bool tween = true)
	{
		_slopeTarget = slope;
		_slopeSpeed = speed;
		if (!tween)
		{
			_slope = _slopeTarget;
		}
	}

	public override void SetExclamation(bool enabled = false)
	{
		base.SetExclamation(enabled);
		if (enabled)
		{
			exclamationIcon.gameObject.SetActive(value: true);
			exclamationIcon.DoFlash();
			exclamationIcon.ZoomIn(HideExclamation);
		}
		else
		{
			RemoveExclamation();
		}
	}

	private void HideExclamation()
	{
		float num = 1f;
		float num2 = 1f;
		if (AppInfo.TESTING)
		{
			num /= 3f;
			num2 /= 3f;
		}
		exclamationIcon.ChangeAlpha(null, 0f, num2, Ease.Unset, num, RemoveExclamation);
	}

	private void RemoveExclamation()
	{
		exclamationIcon.gameObject.SetActive(value: false);
	}

	public override void OnDestroy()
	{
		GameData.instance.main.DISPATCHER.FRAME_UPDATE.RemoveListener(OnUpdate);
		StopPath();
		RemoveLine();
	}
}
