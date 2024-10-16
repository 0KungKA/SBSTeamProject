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
        transform.position = Player.transform.position;
        if (!effects.isPlaying && Manager.Effect_SoundPlayer.StopBackgroundSound == false)
        {
            effects.clip = null;
        }
    }
}
