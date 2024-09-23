using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Todo:이거 뭔지 모르겠는데 주석처리함
        /*Debug.Log("Test Sound Play : " + transform.name);
        transform.GetComponent<AudioSource>().Play();*/

        if(other.transform.name == "Player_Camera" && transform.name == "G_MainGate_Exit")
        {
            GameObject.Find("EventSystem").GetComponent<CutSceneScript>().PlayeCutScene(2);
            Manager.CM_Instance.OffMouseCursor();
            Manager.CM_Instance.SetRotState(false);
            Manager.CM_Instance.SetMoveState(false);

            Destroy(gameObject);
        }

        if(this.transform.name == "NPCStateStay")
        {
            if(other.transform.name == "Player_Camera")
            {
                GameObject.Find("NPC_CM_M").GetComponent<NPCM_AI_Ctrl>().SetState(0);
                Destroy(GameObject.Find("NPC_CM_M").gameObject);

                Destroy(gameObject);
            }
        }

        if(transform.name == "Spawn_NPC_CM_M" && other.transform.name == "Player_Camera")
        {
            GameObject.Find("EventSystem").GetComponent<NPC_GaugeUI>().SetOnGauge(true);
            GameObject npc_cm_m = Resources.Load<GameObject>("Prefep/Object/NPC/NPC_CM_M");
            npc_cm_m = Instantiate(npc_cm_m).gameObject;
            npc_cm_m.transform.position = GameObject.Find("NPC_Spawn_Point").transform.position;
            npc_cm_m.GetComponent<NPCM_AI_Ctrl>().SetState(1);
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(transform.name == "C_Room_HideCheck" && other.transform.name == "Player_Camera" && Manager.CM_Instance.OnHide == true)
        {
            CallSecondNPCTalk();
        }
    }

    private void CallSecondNPCTalk()
    {
        GameObject.Find("EventSystem").GetComponent<NPCTalk>().StartNPCTalk(2);
        Destroy(gameObject);
    }
}
