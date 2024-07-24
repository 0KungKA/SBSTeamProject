using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Test Sound Play : " + transform.name);
        transform.GetComponent<AudioSource>().Play();
    }
}
