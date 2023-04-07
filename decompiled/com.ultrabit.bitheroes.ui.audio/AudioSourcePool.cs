using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace com.ultrabit.bitheroes.ui.audio;

public class AudioSourcePool : MonoBehaviour
{
	private int _initialObjects;

	private int _currentObjects;

	private AudioMixer _audioMixer;

	private List<AudioSource> sources = new List<AudioSource>();

	public void LoadDetails(int initialObjects, AudioMixer audioMixer)
	{
		_initialObjects = initialObjects;
		_audioMixer = audioMixer;
		for (int i = 0; i < initialObjects; i++)
		{
			AddAudioSource();
		}
	}

	public AudioSource GetAvailableAudioSource(float volume)
	{
		foreach (AudioSource source in sources)
		{
			if (!source.isPlaying)
			{
				source.volume = volume;
				return source;
			}
		}
		return AddAudioSource(volume);
	}

	public AudioSource AddAudioSource(float volume = 1f)
	{
		GameObject obj = new GameObject();
		obj.name = "AudioObj-" + _currentObjects;
		obj.transform.SetParent(base.transform, worldPositionStays: false);
		obj.transform.localPosition = Vector3.zero;
		AudioSource audioSource = obj.AddComponent<AudioSource>();
		audioSource.outputAudioMixerGroup = _audioMixer.FindMatchingGroups("Master/Sound")[0];
		audioSource.loop = false;
		audioSource.volume = volume;
		sources.Add(audioSource);
		_currentObjects++;
		return audioSource;
	}
}
