using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLineSystem : MonoBehaviour
{
    public void CallTimeline()
    {
        PlayCutScene();
    }

    private void PlayCutScene()
    {
        GetComponent<PlayableDirector>().Play();
    }
}
