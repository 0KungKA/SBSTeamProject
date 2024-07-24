using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCDetail : MonoBehaviour
{
    AudioSource[] StepSoundEffect;
    void Start()
    {
        StepSoundEffect = GetComponents<AudioSource>();
    }

    private void Update()
    {
        foreach(AudioSource audioSource in StepSoundEffect)
        {
            if(audioSource.isPlaying)
            {
                return;
            }
            else
            {
                Debug.Log("Test Sound Play : " + transform.name);
                StepSoundEffect[Random.Range(0, StepSoundEffect.Length)].Play();
            }
        }
    }
}
