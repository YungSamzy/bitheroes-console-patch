using UnityEngine;
using UnityEngine.Events;

namespace com.ultrabit.bitheroes.ui.assets;

public class SWFAsset : Asset
{
	public class SWFAssetAnimationEvent : UnityEvent<SWFAsset>
	{
	}

	public const string ANIMATION_IDLE = "idle";

	public const string ANIMATION_WALK = "walk";

	public const string ANIMATION_HIT = "hit";

	public const string ANIMATION_JUMP = "jump";

	public const string ANIMATION_ATTACK_1 = "attack1";

	public const string ANIMATION_ATTACK_2 = "attack2";

	public const string ANIMTION_FISHING_CASTING_START = "fishingCastingStart";

	public const string ANIMTION_FISHING_CASTING_IDLE = "fishingCastingIdle";

	public const string ANIMTION_FISHING_CAST = "fishingCast";

	public const string ANIMTION_FISHING_CATCHING_START = "fishingCatchingStart";

	public const string ANIMTION_FISHING_CATCHING_IDLE = "fishingCatchingIdle";

	public const string LABEL_ACTIVE = "active";

	public const string LABEL_INACTIVE = "inactive";

	public const string LABEL_TRIGGER = "trigger";

	public const string LABEL_ATTACK = "attack";

	public const float REGULAR_SPEED = 1f;

	public const string LABEL_ATTACK_STAFF = "attackStaff";

	public const string LABEL_ATTACK_STAFF1 = "attackStaff1";

	public const string LABEL_ATTACK_STAFF2 = "attackStaff2";

	[HideInInspector]
	public SWFAssetAnimationEvent ANIMATION_END = new SWFAssetAnimationEvent();

	[HideInInspector]
	public SWFAssetAnimationEvent ANIMATION_TRIGGER = new SWFAssetAnimationEvent();

	public int entityID;

	private string _label = "";

	private float _speed = 1f;

	private string _endLabel = "";

	private Animator _animator;

	private NPCAsset _npcAsset;

	public string label
	{
		get
		{
			if (_label == null || _label.Trim().Equals(""))
			{
				return "idle";
			}
			return _label;
		}
	}

	public override void Awake()
	{
		base.Awake();
		_animator = GetComponentInChildren<Animator>();
		_npcAsset = _animator.GetComponent<NPCAsset>();
		if (_npcAsset == null)
		{
			_npcAsset = _animator.gameObject.AddComponent<NPCAsset>();
		}
		_npcAsset?.SetSWFAsset(this);
	}

	public void OnTriggerAttack()
	{
		ANIMATION_TRIGGER.Invoke(this);
	}

	private AnimationClip GetAnimationClip(string label)
	{
		if (_animator != null && _animator.runtimeAnimatorController.animationClips.Length != 0)
		{
			for (int i = 0; i < _animator.runtimeAnimatorController.animationClips.Length; i++)
			{
				if (label.Trim().ToLowerInvariant().Equals(_animator.runtimeAnimatorController.animationClips[i].name.Trim().ToLowerInvariant()))
				{
					return _animator.runtimeAnimatorController.animationClips[i];
				}
			}
		}
		return null;
	}

	public void PlayAnimationClip(AnimationClip clip, bool randomize)
	{
		if (_animator.gameObject.activeInHierarchy)
		{
			float normalizedTimeOffset = (randomize ? Random.Range(0f, clip.length) : 0f);
			_animator.CrossFade(clip.name, 0f, -1, normalizedTimeOffset);
		}
	}

	public void ChangeLayer(string lName)
	{
		ChangeLayerRecursive(base.transform, lName);
	}

	private void ChangeLayerRecursive(Transform tr, string lName)
	{
		SpriteRenderer component = tr.GetComponent<SpriteRenderer>();
		if ((bool)component)
		{
			component.sortingLayerName = lName;
		}
		for (int i = 0; i < tr.childCount; i++)
		{
			ChangeLayerRecursive(tr.GetChild(i), lName);
		}
	}

	private void AnimationEnd()
	{
		string text = ((_endLabel != null && !_endLabel.Trim().Equals("")) ? _endLabel : "idle");
		PlayAnimation(text);
		if (ANIMATION_END != null)
		{
			ANIMATION_END.Invoke(this);
		}
	}

	public bool PlayAnimation(string label, float speed = 1f, string endLabel = null, bool randomFrame = false)
	{
		if (_animator == null)
		{
			return false;
		}
		if (label == "attackStaff")
		{
			label = "attackStaff1";
		}
		AnimationClip animationClip = GetAnimationClip(label);
		if (animationClip == null)
		{
			return false;
		}
		_label = animationClip.name;
		PlayAnimationClip(animationClip, randomFrame);
		_animator.speed = 1f * speed;
		_speed = speed;
		_endLabel = endLabel;
		_speed = speed;
		if (InvokeAnimationEnd())
		{
			Invoke("AnimationEnd", animationClip.length / _animator.speed);
		}
		return true;
	}

	private bool InvokeAnimationEnd()
	{
		if (!_label.Trim().ToLowerInvariant().Equals("idle"))
		{
			return !_label.Trim().ToLowerInvariant().Equals("walk");
		}
		return false;
	}

	public void StopAnimation()
	{
		_label = "";
		_speed = 1f;
		if (_animator != null)
		{
			_animator.speed = _speed;
		}
	}

	public bool IsIdle()
	{
		return label.Trim().ToLowerInvariant().Equals("idle".Trim().ToLowerInvariant());
	}
}
