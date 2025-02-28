using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
	[Range(0f, 1f)]

	public float GlobalSound;
	public static AudioManager instance;

	public AudioMixerGroup mixerGroup;

	public Sound[] sounds;

	public bool stopRea;

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;
			s.source.volume = s.volume;
			s.source.outputAudioMixerGroup = mixerGroup;
		}
	}

	public void Play(string sound, float volume)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (!s.source.isPlaying || !s.loop)
		{
			if (s == null)
			{
				Debug.LogWarning("Sound: " + name + " not found!");
				return;
			}
			print(s.name);
			s.volume = volume;
			s.source.volume = GlobalSound* s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
			s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
			stopRea = true;
			s.source.Play();
		}
	}
    public void Stop(string sound, bool progressif)
    {
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s.source.isPlaying)
		{
			if (progressif) { StartCoroutine(progressifEnum(sound)); }
			else
			{
				s.source.Stop();
			}
		}
		//if(s.end != "" && stopRea)
		//{
		//	Sound a = Array.Find(sounds, item => item.name == s.end);
		//	a.source.Play();
		//	stopRea = false;

		//}

	}
		IEnumerator progressifEnum(string name)
	{
		print("aaa");
		Sound s = Array.Find(sounds, item => item.name == name);
		for(float i = s.source.volume; i > 0; i = i-0.035f)
		{
			s.source.volume = i;
			print(i);
			yield return new WaitForSeconds(0.01f);
		}

		s.source.Stop();

		yield return null;
	}

}

[System.Serializable]
public class Sound
{

	public string name;

	public AudioClip clip;
	public string end;
	[Range(0f, 1f)]
	public float volume = .75f;
	[Range(0f, 1f)]
	public float volumeVariance = .1f;

	[Range(.1f, 3f)]
	public float pitch = 1f;
	[Range(0f, 1f)]
	public float pitchVariance = .1f;

	public bool loop = false;

	public AudioMixerGroup mixerGroup;

	[HideInInspector]
	public AudioSource source;

}