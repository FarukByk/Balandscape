using System;
using System.Collections.Generic;
using UnityEngine;

public class mySoundSystem : MonoBehaviour
{	
    public List<mySounds> sounds;
    public List<Audios> audios;

    private void Update()
    {
        foreach (Audios audio in audios)
        {
            audio.AudioSource.volume = audio.volume * myMath.LoadFloat(audio.name,0.5f);
        }
    }
    public mySounds getSound(string name)
    {
        mySounds s = null;
        foreach (mySounds mySounds in sounds)
        {
            if (mySounds.name == name)
            {
                s = mySounds;
            }
        }
        return s;   
    }
    public static void PlaySoundEffect(string name, Vector3 pos,float time = 5)
    {
        mySoundSystem soundSystem = FindAnyObjectByType<mySoundSystem>();
        mySounds ms = soundSystem.getSound(name);
        if (ms != null)
        {
            if (pos == Vector3.zero)
            {
                pos = soundSystem.transform.position;
            }
            GameObject go = new GameObject($"soundEffect-{name}");
            Destroy(go, time);
            AudioSource aus = go.AddComponent<AudioSource>();
            aus.clip = ms.sound;
            aus.spatialBlend = 0.5f;
            aus.volume = ms.volume * myMath.LoadFloat("Sound-Effects-Value");
            aus.Play();
        }
    }
}
[Serializable]
public class mySounds
{
    public string name;
    public AudioClip sound;
    [Range(0f,1f)]
    public float volume;
}

[Serializable]
public class Audios
{
    public AudioSource AudioSource;
    public string name;
    [Range(0f, 1f)]
    public float volume;
}