using System;
using System.Collections.Generic;
using com.ultrabit.bitheroes.core;
using com.ultrabit.bitheroes.model.audio;
using com.ultrabit.bitheroes.model.music;
using com.ultrabit.bitheroes.model.sound;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Audio;

namespace com.ultrabit.bitheroes.ui.audio;

public class AudioManager : MonoBehaviour
{
	private const int POOL_INITIAL_OBJECTS = 10;

	private AudioMixer _audioMixer;

	private AudioSource _musicChannel;

	private MusicRef _musicRef;

	private bool _loop;

	private bool _paused;

	private List<AudioRef> pendingDownload = new List<AudioRef>();

	private AudioSourcePool _audioSourcePool;

	private TweenerCore<float, float, FloatOptions> _musicTween;

	private TweenerCore<float, float, FloatOptions> _soundTween;

	public void LoadDetails(AudioMixer audioMixer)
	{
		_audioMixer = audioMixer;
		GetComponent<AudioSource>().enabled = false;
		_musicChannel = base.gameObject.AddComponent<AudioSource>();
		_musicChannel.outputAudioMixerGroup = _audioMixer.FindMatchingGroups("Master/Music")[0];
		GameObject gameObject = new GameObject();
		gameObject.name = "AudioSourcePool";
		gameObject.transform.SetParent(base.transform, worldPositionStays: false);
		gameObject.transform.localPosition = Vector3.zero;
		_audioSourcePool = gameObject.AddComponent<AudioSourcePool>();
		_audioSourcePool.LoadDetails(10, _audioMixer);
	}

	public void PauseMusic()
	{
		if (!(_musicChannel == null))
		{
			_paused = true;
			TweenStop(_musicChannel);
			_musicChannel.Pause();
		}
	}

	public void UnpauseMusic()
	{
		if (_musicRef != null && !(_musicChannel == null))
		{
			_paused = false;
			TweenStop(_musicChannel);
			_musicChannel.UnPause();
		}
	}

	public void StopMusic(bool tween = true)
	{
		if (_musicChannel != null && _musicChannel.time > 0f)
		{
			if (tween)
			{
				TweenVolume(_musicChannel.volume, 0f, _musicChannel, stop: true);
			}
			else
			{
				_musicChannel.Stop();
			}
		}
	}

	public void UpdateVolume(float musicVolume = 1f)
	{
		if (!(_musicChannel == null) && _musicRef != null)
		{
			TweenStop(_musicChannel);
			_musicChannel.volume = musicVolume;
		}
	}

	public void UpdateMusic()
	{
		UpdateVolume(_paused ? 0f : GameData.instance.SAVE_STATE.musicVolume);
	}

	public void PlaySoundPoolLink(string link, float volume = 1f)
	{
		if (!GameData.instance.main.ACTIVE || link == null || GameData.instance.SAVE_STATE.soundVolume <= 0f || volume <= 0f)
		{
			return;
		}
		SoundPoolRef soundPoolRef = SoundBook.LookupPoolLink(link);
		if (soundPoolRef != null)
		{
			SoundRef randomSound = soundPoolRef.getRandomSound();
			if (randomSound != null)
			{
				PlaySound(randomSound, volume);
			}
		}
	}

	public void PlaySoundLink(string link, float volume = 1f)
	{
		if (GameData.instance.main.ACTIVE && link != null && !(GameData.instance.SAVE_STATE.soundVolume <= 0f) && !(volume <= 0f))
		{
			PlaySound(SoundBook.Lookup(link), volume);
		}
	}

	public void PlaySound(SoundRef soundRef, float volume = 1f)
	{
		if (!GameData.instance.main.ACTIVE || soundRef == null || GameData.instance.SAVE_STATE.soundVolume <= 0f || volume <= 0f || _paused)
		{
			return;
		}
		if (soundRef.loaded)
		{
			if (soundRef.musicVolume != 1f)
			{
				_ = soundRef.duration;
				_ = 0f;
			}
			AudioSource audioSource = GetAudioSource(soundRef, volume);
			audioSource.clip = soundRef.sound;
			audioSource.Play();
		}
		else
		{
			soundRef.Load();
			PlaySound(soundRef);
		}
	}

	private AudioSource GetAudioSource(SoundRef soundRef, float volume = 1f)
	{
		return _audioSourcePool.GetAvailableAudioSource(GetSoundVolume(soundRef) * volume);
	}

	private float GetSoundVolume(SoundRef soundRef)
	{
		return GameData.instance.SAVE_STATE.soundVolume * soundRef.volume;
	}

	public void PlayMusicLink(string link, bool loop = true, bool tween = true, int position = 0)
	{
		if (link != null)
		{
			PlayMusic(MusicBook.Lookup(link), loop, tween, position);
		}
	}

	public void PlayMusic(MusicRef musicRef, bool loop = true, bool tween = true, int position = 0)
	{
		StopMusic();
		if (musicRef == null)
		{
			return;
		}
		_musicRef = musicRef;
		_loop = loop;
		if (!musicRef.loaded)
		{
			musicRef.Load();
			PlayMusic(musicRef, loop: true, tween: false);
		}
		else
		{
			if (!(musicRef.sound != null))
			{
				return;
			}
			_musicChannel.loop = loop;
			float num = (float)position * 0.001f;
			if (num >= musicRef.sound.length)
			{
				num = 0f;
			}
			try
			{
				if (tween && !_paused)
				{
					_musicChannel.clip = _musicRef.sound;
					_musicChannel.time = num;
					_musicChannel.Play();
					TweenVolume(0f, GetMusicVolume(musicRef), _musicChannel);
				}
				else
				{
					_musicChannel.clip = _musicRef.sound;
					_musicChannel.time = num;
					_musicChannel.Play();
				}
				UpdateMusic();
			}
			catch (Exception)
			{
			}
		}
	}

	private void TweenStop(AudioSource channel)
	{
		if (channel.outputAudioMixerGroup == _audioMixer.FindMatchingGroups("Master/Music")[0])
		{
			_musicTween?.Kill();
		}
		if (channel.outputAudioMixerGroup == _audioMixer.FindMatchingGroups("Master/Sound")[0])
		{
			_soundTween?.Kill();
		}
	}

	private void TweenVolume(float volumeStart, float volumeEnd, AudioSource channel, bool stop = false)
	{
		TweenStop(channel);
		channel.volume = volumeStart;
		if (channel.outputAudioMixerGroup == _audioMixer.FindMatchingGroups("Master/Music")[0])
		{
			TweenMusic(volumeStart, volumeEnd, channel, stop);
		}
		if (channel.outputAudioMixerGroup == _audioMixer.FindMatchingGroups("Master/Sound")[0])
		{
			TweenSound(volumeStart, volumeEnd, channel, stop);
		}
	}

	private void TweenMusic(float volumeStart, float volumeEnd, AudioSource channel, bool stop = false)
	{
		_musicTween = DOTween.To(() => volumeStart, delegate(float x)
		{
			volumeStart = x;
		}, volumeEnd, 5f).SetEase(Ease.Linear).OnUpdate(delegate
		{
			channel.volume = volumeStart;
		})
			.OnComplete(delegate
			{
				if (stop)
				{
					channel.Stop();
				}
			});
	}

	private void TweenSound(float volumeStart, float volumeEnd, AudioSource channel, bool stop = false)
	{
		_soundTween = DOTween.To(() => volumeStart, delegate(float x)
		{
			volumeStart = x;
		}, volumeEnd, 5f).SetEase(Ease.Linear).OnUpdate(delegate
		{
			channel.volume = volumeStart;
		})
			.OnComplete(delegate
			{
				if (stop)
				{
					channel.Stop();
				}
			});
	}

	private float GetMusicVolume(MusicRef musicRef)
	{
		float num = musicRef.volume;
		if (_paused)
		{
			num = 0f;
		}
		return GameData.instance.SAVE_STATE.musicVolume * num;
	}

	public int GetMusicPosition()
	{
		if (!(_musicChannel != null))
		{
			return 0;
		}
		return (int)(_musicChannel.time * 1000f);
	}
}
