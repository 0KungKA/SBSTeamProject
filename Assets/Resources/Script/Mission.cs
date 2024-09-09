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

    [SerializeField]
    string keyName;

    public GameObject[] ClearTarget;

    public GameObject[] OnEnableTarge;

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
            setManager();
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

    void ItemCheck()
    {
        if (GameObject.FindWithTag("MainCamera").GetComponent<InputManager>().CurrentSelectItem != null)
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

    void BRoomDoorLock()
    {
        ItemCheck();
    }

    void CRoomDoorLock()
    {
        ItemCheck();
    }

    public void BRoomClosetLock()
    {
        setManager();
        Manager.UIManager_Instance.UIPopup("UI_B_Closet_Lock");
    }

    

    void CRoomSafeDoorLock()
    {
        setManager();
        Manager.UIManager_Instance.UIPopup("UI_C_Safe_Lock");
    }

    void CJewelCaseLock()
    {
        string CurrentItemName = ItemManager.ItemManager_Instance.GetCurrentItem();
        if(CurrentItemName == null)
            Manager.ErrorInfo_Instance.ErrorEnqueue(FalseMissionInfo);

        else if (Key != null)
        {
            if (ItemManager.ItemManager_Instance.GetCurrentItem() == Key.name)
            {
                ItemManager.ItemManager_Instance.DeleteItem(Key.name);
                Manager.ErrorInfo_Instance.ErrorEnqueue(CompleteMissionInfo);
                MissionDelete();
            }
        }
        else if (keyName != null)
        {
            if (ItemManager.ItemManager_Instance.GetCurrentItem() == keyName)
            {
                ItemManager.ItemManager_Instance.DeleteItem(keyName);
                Manager.ErrorInfo_Instance.ErrorEnqueue(CompleteMissionInfo);
                MissionDelete();
            }
        }
        else
        {
            Manager.ErrorInfo_Instance.ErrorEnqueue(FalseMissionInfo);
        }
    }

    void D_Door_Lock()
    {
        string CurrentItemName = ItemManager.ItemManager_Instance.GetCurrentItem();
        if (CurrentItemName == null)
            Manager.ErrorInfo_Instance.ErrorEnqueue(FalseMissionInfo);

        else if (Key != null)
        {
            if (ItemManager.ItemManager_Instance.GetCurrentItem() == Key.name)
            {
                ItemManager.ItemManager_Instance.DeleteItem(Key.name);
                Manager.ErrorInfo_Instance.ErrorEnqueue(CompleteMissionInfo);
                MissionDelete();
            }
        }
        else if (keyName != null)
        {
            if (ItemManager.ItemManager_Instance.GetCurrentItem() == keyName)
            {
                ItemManager.ItemManager_Instance.DeleteItem(keyName);
                Manager.ErrorInfo_Instance.ErrorEnqueue(CompleteMissionInfo);
                MissionDelete();
            }
        }
        else
        {
            Manager.ErrorInfo_Instance.ErrorEnqueue(FalseMissionInfo);
        }

    }

    void D_SuitCase_Lock()
    {
        setManager();
        Manager.UIManager_Instance.UIPopup("UI_D_SuitCase_Lock");
    }

    void D_Shelf_Interaction()
    {
        setManager();
        Manager.UIManager_Instance.UIPopup("UI_D_Room_Shelf_Interaction");
    }

    void D_Document_Interaction()
    {
        setManager();
        Manager.UIManager_Instance.UIPopup("UI_D_Room_Shelf_Interaction");
    }

    void Clear_Pice()
    {
        string CurrentItemName = ItemManager.ItemManager_Instance.GetCurrentItem();

        if (CurrentItemName == null)
            Manager.ErrorInfo_Instance.ErrorEnqueue(FalseMissionInfo);

        else if (Key != null)
        {
            if (ItemManager.ItemManager_Instance.GetCurrentItem() == Key.name)
            {
                ItemManager.ItemManager_Instance.DeleteItem(Key.name);
                Manager.ErrorInfo_Instance.ErrorEnqueue(CompleteMissionInfo);
                MissionOnEnable();
                MissionDelete();
            }
        }
        else if (keyName != null)
        {
            if (ItemManager.ItemManager_Instance.GetCurrentItem() == keyName)
            {
                ItemManager.ItemManager_Instance.DeleteItem(keyName);
                Manager.ErrorInfo_Instance.ErrorEnqueue(CompleteMissionInfo);
                MissionOnEnable();
                MissionDelete();
            }
        }
        else
        {
            Manager.ErrorInfo_Instance.ErrorEnqueue(FalseMissionInfo);
        }
    }

    private void setManager()
    {
        Manager.Origin_Object = transform.gameObject;
        Manager.Call_Object = ClearTarget;
    }

    

    public void MissionClearSelf()
    {
        GameObject[] SelfTarget = Manager.Call_Object;
        for (int i = 0; i < SelfTarget.Length; i++)
        {
            Destroy(SelfTarget[i]);
        }
        Manager.ErrorInfo_Instance.ErrorEnqueue(CompleteMissionInfo);
    }

    public void MissionOnEnable()
    {
        if(OnEnableTarge != null)
        {
            for (int i = 0; i < OnEnableTarge.Length; i++)
            {
                OnEnableTarge[i].active = true;
            }
        }
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
