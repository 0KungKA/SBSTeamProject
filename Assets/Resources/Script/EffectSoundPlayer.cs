using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class EffectSoundPlayer : MonoBehaviour
{
    //AudioSource[] audioArr;
    List<AudioSource> audioArr = new List<AudioSource>();

    public bool StopBackgroundSound = false;
    public bool StopPlaySound = false;

    float soundValue;

    private void Start()
    {
        setting();
    }

    private void Update()
    {
        EffectSoundEnd();
    }

    public void setting()
    {
        AudioSource temp = transform.AddComponent<AudioSource>();
        temp.clip = null;
        temp.volume = 1.0f;
        temp.playOnAwake = false;
        temp.maxDistance = 20.0f;
        temp.loop = false;

        audioArr.Add(temp);
    }

    public void setting(AudioClip clip)
    {
        AudioSource temp = transform.AddComponent<AudioSource>();
        temp.clip = clip;
        temp.volume = 1.0f;
        temp.playOnAwake = false;
        temp.maxDistance = 20.0f;
        temp.loop = false;
        temp.Play();
        audioArr.Add(temp);
    }

    public void setting(AudioClip _clip,float _volume,bool _playOnAwake,bool _loop)//사실상 심장소리만 쓸거
    {
        AudioSource temp = transform.AddComponent<AudioSource>();
        temp.clip = _clip;
        temp.volume = _volume;
        temp.playOnAwake = _playOnAwake;
        temp.maxDistance = 20.0f;
        temp.loop = _loop;
        temp.Play();
        audioArr.Add(temp);
    }

    public void EffectSoundPlay(AudioSource source)
    {
        if (StopPlaySound) return;
        for (int i = 0; i < audioArr.Count; i++)
        {
            if (audioArr[i].clip == source && audioArr[i].isPlaying == true)
            {
                return;
            }
            else if (audioArr[i].clip == source && audioArr[i].isPlaying == false)
            {
                audioArr[i].Play();
                return;
            }
            else if (audioArr[i].clip != source && audioArr[i].isPlaying == false)
            {
                audioArr[i].clip = source.clip;
                audioArr[i].Play();
                return;
            }
        }
        setting(source.clip);
    }

    public void EffectSoundPlay(AudioClip source)
    {
        if (StopPlaySound) return;
        for (int i = 0; i < audioArr.Count; i++) 
        {
            if (audioArr[i].clip == source && audioArr[i].isPlaying == true)
            {
                return;
            }
            else if (audioArr[i].clip == source && audioArr[i].isPlaying == false)
            {
                audioArr[i].Play();
                return;
            }
            else if (audioArr[i].clip != source && audioArr[i].isPlaying == false)
            {
                audioArr[i].clip = source;
                audioArr[i].Play();
                return;
            }
        }
        setting(source);
    }

    public void EffectSoundEnd()
    {
        StopPlaySound = false;
        for (int i = audioArr.Count - 1; i >= 1; i--)
        {
            if (audioArr[i].isPlaying == false)
            {
                AudioSource temp = audioArr[i];
                audioArr.Remove(audioArr[i]);
                Destroy(temp);
            }
        }
    }
}
