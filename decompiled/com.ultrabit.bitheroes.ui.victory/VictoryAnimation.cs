using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.application;
using com.ultrabit.bitheroes.model.language;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace com.ultrabit.bitheroes.ui.victory;

[RequireComponent(typeof(Animator))]
public class VictoryAnimation : MonoBehaviour
{
	private const string ANIMATION_TRIGGER = "Animation";

	private const string ANIMATION_SHORT_TRIGGER = "QuickAnimation";

	private const string ANIMATION_EXIT_TRIGGER = "Exit";

	private const string ANIMATION_VICTORY_BOOL = "Victory";

	private const string SPEED_FLOAT = "AnimationSpeed";

	private const string TWEEN_COMPLETE_MESSAGE = "OnAnimationComplete";

	public TextMeshProUGUI text;

	public Image lBlueRibbon;

	public Image redRibbon;

	public UnityEvent ON_SHIELD_ANIMATION_END;

	private GameObject _parent;

	private Animator _animator;

	public void ShieldAnimation(GameObject parent, bool animate = true, string customShieldText = null, bool isVictorious = true)
	{
		_parent = parent;
		_animator = GetComponent<Animator>();
		text.text = customShieldText ?? Language.GetString("battle_victory_results");
		_animator.SetBool("Victory", isVictorious);
		if (AppInfo.TESTING)
		{
			_animator.SetFloat("AnimationSpeed", 3f);
		}
		if (animate)
		{
			_animator.SetTrigger("Animation");
		}
		else
		{
			_animator.SetTrigger("QuickAnimation");
		}
	}

	public void ForceTweenUp()
	{
		_animator.SetTrigger("Exit");
	}

	public void AnimationEnd()
	{
		_parent.BroadcastMessage("OnAnimationComplete");
	}

	public void SwordSound()
	{
		GameData.instance.audioManager.PlaySoundLink("victorysword");
	}

	public void ShieldSound()
	{
		GameData.instance.audioManager.PlaySoundLink("victoryshield");
	}

	private void OnShieldAnimationEnd()
	{
		ON_SHIELD_ANIMATION_END?.Invoke();
	}

	public void ToggleAnimation(bool enabled)
	{
		_animator.enabled = enabled;
	}
}
