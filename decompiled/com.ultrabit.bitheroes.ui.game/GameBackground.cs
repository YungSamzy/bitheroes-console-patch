using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.assets;
using com.ultrabit.bitheroes.ui.utility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace com.ultrabit.bitheroes.ui.game;

public class GameBackground : MonoBehaviour
{
	private const float DELAY_MIN = 1f;

	private const float DELAY_MAX = 6f;

	private const float DELAY_CHECK = 1f;

	private const float ASSET_SCALE = 2f;

	private static readonly Vector2 ASSET_OFFSET = new Vector2(100f, -40f);

	private static readonly Vector2 ASSET_SPREAD = new Vector2(0f, 20f);

	private const float ASSET_DURATION_LEFT = 10f;

	private const float ASSET_DURATION_RIGHT = 10f;

	private const int ASSET_MAX = 3;

	public const float BG_ALPHA = 0.4f;

	public List<SWFAsset> _assets = new List<SWFAsset>();

	private CoroutineTimer _currentTimer;

	private GameObject _holder;

	private int index;

	private Transform _sizer;

	private void Start()
	{
		Canvas canvas = base.gameObject.AddComponent<Canvas>();
		canvas.overrideSorting = true;
		canvas.sortingOrder = -Mathf.FloorToInt(ASSET_SPREAD.x * 2f + 1f);
	}

	public void LoadDetails()
	{
		_holder = new GameObject();
		_holder.name = "FamiliarsHolder";
		_holder.transform.SetParent(GameData.instance.main.uiCamera.transform);
		_holder.transform.position = GameData.instance.main.uiCamera.transform.position + Vector3.forward * (GameData.instance.main.uiCamera.nearClipPlane + 1f);
		_holder.transform.localScale = GameData.instance.windowGenerator.canvas.transform.localScale;
	}

	public void StartFamiliarsParade()
	{
		Invoke("DoCheckFamiliar", 1f);
	}

	private void DoCheckFamiliar()
	{
		if (!base.gameObject.activeSelf)
		{
			return;
		}
		List<string> familiarAssets = GameData.instance.SAVE_STATE.familiarAssets;
		if (familiarAssets == null || familiarAssets.Count <= 0 || _assets.Count >= 3)
		{
			Invoke("DoCheckFamiliar", 1f);
			return;
		}
		string text = familiarAssets[Util.randomInt(0, familiarAssets.Count - 1)];
		if (text != null)
		{
			SWFAsset swfAsset = null;
			Transform transform = null;
			GameObject gameObjectAsset = GameData.instance.main.assetLoader.GetGameObjectAsset(text);
			if (gameObjectAsset != null)
			{
				transform = gameObjectAsset.transform;
			}
			if (transform != null)
			{
				GameObject obj = transform.gameObject;
				index++;
				transform.SetParent(_holder.transform, worldPositionStays: false);
				swfAsset = obj.AddComponent<SWFAsset>();
				obj.AddComponent<GameBackgroundFamiliar>().index = index;
				obj.transform.localScale = new Vector3(2f, 2f, 1f);
				Util.ChangeLayer(obj.transform, "UI");
				obj.AddComponent<SortingGroup>().sortingLayerName = "UI";
			}
			DoAssetMovement(swfAsset);
		}
	}

	private void DoAssetMovement(SWFAsset swfAsset)
	{
		if (swfAsset != null)
		{
			bool flag = Util.randomBoolean();
			Vector2 vector = new Vector2(Util.RandomNumber(0f - ASSET_SPREAD.x, ASSET_SPREAD.x), Util.RandomNumber(0f - ASSET_SPREAD.y, ASSET_SPREAD.y));
			Vector2 vector2 = new Vector2(flag ? (0f - ASSET_OFFSET.x - Main.BOUNDS.width / 2f) : (Main.BOUNDS.width / 2f + ASSET_OFFSET.x + vector.x), (0f - Main.DEFAULT_BOUNDS.height) / 2f - ASSET_OFFSET.y - vector.y);
			Vector2 vector3 = new Vector2((!flag) ? (0f - ASSET_OFFSET.x - Main.BOUNDS.width / 2f) : (Main.BOUNDS.width / 2f + ASSET_OFFSET.x + vector.x), vector2.y);
			float duration = (flag ? 10f : 10f);
			swfAsset.GetComponent<SortingGroup>().sortingOrder = Mathf.FloorToInt(vector.y - ASSET_SPREAD.y);
			swfAsset.transform.localScale = new Vector3(flag ? swfAsset.transform.localScale.x : (0f - swfAsset.transform.localScale.x), swfAsset.transform.localScale.y, 1f);
			swfAsset.transform.localPosition = new Vector3(vector2.x, vector2.y, swfAsset.transform.localPosition.z + 50f);
			AddAsset(swfAsset);
			swfAsset.PlayAnimation("walk");
			UnityAction<List<object>> onComplete = delegate
			{
				OnAssetMovementComplete(swfAsset);
			};
			Tween.StartLocalMovement(swfAsset.gameObject, vector3.x, vector3.y, duration, 0f, onComplete);
		}
		Invoke("DoCheckFamiliar", Util.RandomNumber(1f, 6f));
	}

	private void OnAssetMovementComplete(SWFAsset swfAsset)
	{
		RemoveAsset(swfAsset);
	}

	private void AddAsset(SWFAsset asset)
	{
		_assets.Add(asset);
	}

	private void RemoveAsset(SWFAsset swfAsset)
	{
		_assets.Remove(swfAsset);
		swfAsset.StopAnimation();
		Tween.StopMovement(swfAsset.gameObject);
		Object.Destroy(swfAsset.gameObject.gameObject);
	}

	private void OnDestroy()
	{
		CancelInvoke("DoCheckFamiliar");
		for (int i = 0; i < _assets.Count; i++)
		{
			SWFAsset sWFAsset = _assets[i];
			if (sWFAsset != null)
			{
				RemoveAsset(sWFAsset);
			}
		}
		_assets.Clear();
		Object.Destroy(_holder);
	}
}
