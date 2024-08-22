using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Mission : MonoBehaviour
{


    [SerializeField]
    string MissionName;

    [SerializeField]
    string anyString;

    [Header("실패했을때 팝업시킬 내용")]
    [SerializeField]
    string FalseMissionInfo;

    [Header("성공했을때 팝업시킬 내용")]
    [SerializeField]
    string CompleteMissionInfo;

    [SerializeField]
    private GameObject Key;

    public GameObject[] ClearTarget;

    public bool Test = false;

    public void Update()
    {
        if(Test)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                SendMSG();
            }
        }
        
    }

    public void SendMSG()
    {
        if(MissionName != null)
        {
            SendMessage(MissionName);
        }
        else
        {
            Debug.Log(transform.name + "Mission의 MissionName이 지정되어 있지않습니다");
        }
    }

    public void stringCall()
    {
        if(anyString != null)
        {
            Manager.ErrorInfo_Instance.ErrorEnqueue(anyString);
        }
    }

    void BRoomDoorLock()
    {
        if(GameObject.FindWithTag("MainCamera").GetComponent<InputManager>().CurrentSelectItem != null)
        {
            if (GameObject.FindWithTag("MainCamera").GetComponent<InputManager>().CurrentSelectItem.name == Key.name)
            {
                ItemManager.ItemManager_Instance.DeleteItem(Key.name);
                Manager.ErrorInfo_Instance.ErrorEnqueue(CompleteMissionInfo);
                MissionDelete();
            }
        }
        else
        {
            Manager.ErrorInfo_Instance.ErrorEnqueue(FalseMissionInfo);
        }
    }

    void FRoomSafeDoorLock()
    {
        Manager.UIManager_Instance.UIPopup("UI_C_Safe_Lock");
    }

    public void BRoomClosetLock()
    {
        Manager.UIManager_Instance.UIPopup("UI_B_Closet_Lock");
    }

    public void MissionClearSelf()
    {
        GameObject SelfTarget = GameObject.Find("ClosetLock").gameObject;
        for (int i = 0; i < SelfTarget.GetComponent<Mission>().ClearTarget.Length; i++)
        {
            Destroy(SelfTarget.GetComponent<Mission>().ClearTarget[i].gameObject);
        }
        Manager.ErrorInfo_Instance.ErrorEnqueue(CompleteMissionInfo);
    }

    public void MissionDelete()
    {
        for(int i = 0; i < ClearTarget.Length;i++)
        {
            Destroy(ClearTarget[i].gameObject);
        }
        Manager.ErrorInfo_Instance.ErrorEnqueue(CompleteMissionInfo);
        //Destroy(transform.gameObject);
    }
}
