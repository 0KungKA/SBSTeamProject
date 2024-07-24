using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public void PlaySound()
    {
        if (transform.GetComponent<AudioSource>() != null)
        {
            Debug.Log("Test Sound Play : " + transform.name);
            transform.GetComponent<AudioSource>().Play();
        }
    }
}
