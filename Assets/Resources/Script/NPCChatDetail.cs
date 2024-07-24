using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCChatDetail : MonoBehaviour
{
    private void OnDisable()
    {
        GameObject.Find("UI_Scene_Main").transform.Find("Timer").GetComponent<Timer>().StartTimer();
    }
}
