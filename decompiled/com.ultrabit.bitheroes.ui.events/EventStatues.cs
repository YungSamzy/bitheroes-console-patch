using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.events;
using com.ultrabit.bitheroes.model.leaderboard;
using com.ultrabit.bitheroes.model.utility;
using com.ultrabit.bitheroes.ui.character;
using com.ultrabit.bitheroes.ui.instance;
using UnityEngine;

namespace com.ultrabit.bitheroes.ui.events;

public class EventStatues : MonoBehaviour
{
	public GameObject placeholderFirst;

	public GameObject placeholderSecond;

	public GameObject placeholderThird;

	private InstanceObject _instanceObject;

	private List<LeaderboardData> _leaders;

	private EventRef _eventRef;

	private Shader _grayscaleShader;

	public void LoadDetails(InstanceObject instanceObject, List<LeaderboardData> leaders, EventRef eventRef)
	{
		_instanceObject = instanceObject;
		_leaders = leaders;
		_eventRef = eventRef;
		_grayscaleShader = GameData.instance.main.assetLoader.GetAsset<Shader>("Shader/Grayscale");
		D.Log($"EventStatues::Load Details {leaders != null} - {eventRef != null}");
		if (_leaders == null || _eventRef == null)
		{
			if (placeholderFirst != null)
			{
				placeholderFirst.gameObject.SetActive(value: false);
			}
			if (placeholderSecond != null)
			{
				placeholderSecond.gameObject.SetActive(value: false);
			}
			if (placeholderThird != null)
			{
				placeholderThird.gameObject.SetActive(value: false);
			}
		}
		else
		{
			CreateDisplays();
		}
	}

	private void CreateDisplays()
	{
		LeaderboardData data = ((_leaders.Count > 0) ? _leaders[0] : null);
		SetDisplay(data, placeholderFirst, 1.25f);
	}

	private void SetDisplay(LeaderboardData data, GameObject placeholder, float scale = 1f, bool flip = false)
	{
		if (placeholder == null)
		{
			return;
		}
		if (data != null && data.data != null)
		{
			CharacterDisplay characterDisplay = data.data.toCharacterDisplay(scale, displayMount: false, null, enableLoading: false);
			characterDisplay.characterPuppet.PlayAnimation("idle");
			characterDisplay.characterPuppet.StopAllAnimations();
			if (flip)
			{
				characterDisplay.transform.localScale = new Vector3(0f - characterDisplay.transform.localScale.x, characterDisplay.transform.localScale.y, characterDisplay.transform.localScale.z);
			}
			characterDisplay.transform.SetParent(placeholder.transform, worldPositionStays: false);
			characterDisplay.characterPuppet.transform.localPosition = new Vector3(0f, 25f, 0f);
			_instanceObject.AddCollidersAndHover();
			ApplyFilter();
		}
		else
		{
			placeholder.gameObject.SetActive(value: false);
		}
	}

	private void OnDisplayLoaded()
	{
		_instanceObject.AddCollidersAndHover();
		ApplyFilter();
	}

	private void ApplyFilter()
	{
		if (_instanceObject.hoverSprites != null)
		{
			_instanceObject.hoverSprites.MakeGray();
		}
	}
}
