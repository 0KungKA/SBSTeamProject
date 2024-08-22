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

    [Header("���������� �˾���ų ����")]
    [SerializeField]
    string FalseMissionInfo;

    [Header("���������� �˾���ų ����")]
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
            Debug.Log(transform.name + "Mission�� MissionName�� �����Ǿ� �����ʽ��ϴ�");
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
