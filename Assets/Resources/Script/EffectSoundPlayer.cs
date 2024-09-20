using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSoundPlayer : MonoBehaviour
{
    AudioSource effects;

    private void Start()
    {
        effects = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!effects.isPlaying)
        {
            effects.clip = null;
        }
    }

    public void EffectSoundPlay(AudioClip source)
    {
        effects.clip = source;
        effects.Play();
    }
}
