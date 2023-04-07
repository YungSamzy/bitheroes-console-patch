using System.Collections;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.battle;
using com.ultrabit.bitheroes.model.equipment;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.utility;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.battle;

public class BattleProjectile : MonoBehaviour
{
	[HideInInspector]
	public UnityCustomEvent COMPLETE = new UnityCustomEvent();

	private BattleProjectileRef _projectileRef;

	private float _speed;

	private Vector2 _startPoint;

	private Vector2 _endPoint;

	private BattleEntity _originEntity;

	private BattleEntity _sourceEntity;

	private EquipmentRef _equipmentRef;

	private CharacterDisplay _display;

	private GameObject _asset;

	private Coroutine _timer;

	public void LoadDetails(BattleProjectileRef projectileRef, float speed, Vector2 startPoint, Vector2 endPoint, BattleEntity originEntity, BattleEntity sourceEntity)
	{
		_projectileRef = projectileRef;
		_speed = speed;
		_startPoint = startPoint;
		_endPoint = endPoint;
		_originEntity = originEntity;
		_sourceEntity = sourceEntity;
		if (_projectileRef.weapon && _originEntity.characterData != null)
		{
			_equipmentRef = _originEntity.characterData.equipment.getDisplaySlot(0);
			_display = _originEntity.asset.GetComponent<CharacterDisplay>();
			if (_display != null)
			{
				_display.SetEquipmentVisibility(_equipmentRef, vis: false);
			}
		}
		if (_asset == null)
		{
			_asset = _projectileRef.getAsset(center: true, 2f);
		}
		if (_asset != null)
		{
			_asset.transform.SetParent(base.transform, worldPositionStays: false);
			_asset.transform.localPosition = Vector3.zero;
		}
		base.transform.localPosition = new Vector3(_startPoint.x, _startPoint.y, base.transform.localPosition.z);
		if (_asset != null)
		{
			if (_projectileRef.rotate)
			{
				_asset.transform.rotation = Quaternion.Euler(0f, 0f, 0f - Mathf.Round(Util.getRotation(_startPoint.x, _startPoint.y, _endPoint.x, _endPoint.y)));
			}
			Vector3 eulerAngles = _asset.transform.eulerAngles;
			eulerAngles += new Vector3(0f, 0f, 0f - _projectileRef.rotation);
			_asset.transform.eulerAngles = eulerAngles;
		}
		base.gameObject.SetActive(value: false);
		GameData.instance.main.DISPATCHER.FRAME_UPDATE.AddListener(OnUpdate);
	}

	public void OnAddedToStage()
	{
		base.gameObject.SetActive(value: true);
		if (_projectileRef.trailEffectRef != null)
		{
			_timer = GameData.instance.main.coroutineTimer.AddTimer(base.gameObject, _projectileRef.trailDelay, CoroutineTimer.TYPE.MILLISECONDS, 0, null, OnTrailTimer);
		}
		float distance = Util.GetDistance(base.transform.localPosition.x, base.transform.localPosition.y, _endPoint.x, _endPoint.y);
		float num = 50f / distance;
		float num2 = 1f / (_projectileRef.speed / 50f) / num / _speed;
		if (AppInfo.TESTING)
		{
			num2 /= 3f;
		}
		if (num2 >= 0.1f)
		{
			Tween.StartLocalMovement(base.gameObject, _endPoint.x, _endPoint.y, num2, 0f, Complete);
			StartCoroutine(CheckComplete(num2));
		}
		else
		{
			Complete(new List<object>());
		}
	}

	private IEnumerator CheckComplete(float duration)
	{
		yield return new WaitForSeconds(duration + 0.02f);
		Complete(new List<object>());
	}

	private void OnUpdate(object e)
	{
		if (_projectileRef.spin != 0f)
		{
			Vector3 eulerAngles = base.transform.eulerAngles;
			eulerAngles += new Vector3(0f, 0f, _sourceEntity.focused ? _projectileRef.spin : (0f - _projectileRef.spin));
			base.transform.eulerAngles = eulerAngles;
		}
	}

	private void OnTrailTimer()
	{
	}

	private void Complete(List<object> parameters)
	{
		GameData.instance.main.DISPATCHER.FRAME_UPDATE.RemoveListener(OnUpdate);
		StopCoroutine("CheckComplete");
		if (_equipmentRef != null && _display != null)
		{
			_display.SetEquipmentVisibility(_equipmentRef, vis: true);
		}
		GameData.instance.main.coroutineTimer.StopTimer(ref _timer);
		COMPLETE.Invoke(this);
		Object.Destroy(base.gameObject);
	}
}
