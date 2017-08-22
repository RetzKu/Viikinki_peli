using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {
    public Sound[] GeneralSounds;
    public Sound[] WolfBarks;
    public Sound[] CombatHits;
    // Use this for initialization

    [HideInInspector]
    public static AudioManager instance;

	void Awake () {
		foreach(Sound s in GeneralSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
        foreach (Sound s in WolfBarks)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
        foreach (Sound s in CombatHits)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
        if (instance == null)
        {
            instance = this;
        }
        Play("Ambient",true);
	}
	
    public void Play(string name,bool loop = false)
    {
        Sound s = Array.Find(GeneralSounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning(name + "  not found");
            return;
        }
        if(s.pitchRandomizer == 0)
        {            
            s.source.loop = loop;
            s.source.Play();
            return;
        }
        playRandomPitch(s);
    }
    public void Bark()
    {
        if (WolfBarks.Length == 0)
        {
            Debug.LogWarning("no barks avaivable");
            return;
        }
        playRandomPitch(WolfBarks[UnityEngine.Random.Range(0, WolfBarks.Length)]);
    }
    void playRandomPitch(Sound s)
    {
        s.source.pitch = UnityEngine.Random.Range(s.pitch - s.pitchRandomizer, s.pitch + s.pitchRandomizer);
        s.source.Play();
    }
}
