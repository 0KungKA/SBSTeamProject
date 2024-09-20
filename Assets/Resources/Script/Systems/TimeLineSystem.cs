using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimeLineSystem : MonoBehaviour
{
    public void CallTimeline()
    {
        GameObject.FindWithTag("NPC").AddComponent<NPCM_AI_Ctrl>();
        PlayCutScene();
    }

    private void PlayCutScene()
    {
        GetComponent<PlayableDirector>().Play();
    }
}
