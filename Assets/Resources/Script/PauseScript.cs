using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    float timeScaleTemp;

    public void Start()
    {
        timeScaleTemp = Time.timeScale;
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Manager.CM_Instance.OffMouseCursor();
        Time.timeScale = 1;
    }
}
