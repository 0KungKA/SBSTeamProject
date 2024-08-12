using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    float timeScaleTemp;
    //bool onPuase = false;

    public void Start()
    {
        timeScaleTemp = Time.timeScale;
        //onPuase = true;
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        //onPuase = false;
        Time.timeScale = 1;
    }
}
