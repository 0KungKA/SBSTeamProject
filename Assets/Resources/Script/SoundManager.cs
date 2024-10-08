using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EffectSound : MonoBehaviour
{
    public AudioSource effects;

    private GameObject Player;

    private void Start()
    {
        Player = GameObject.Find("Player_Camera");
        effects = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Manager.Effect_SoundPlayer.StopBackgroundSound) return;

        transform.position = Player.transform.position;
        if (!effects.isPlaying)
        {
            effects.clip = null;
        }
    }
}
