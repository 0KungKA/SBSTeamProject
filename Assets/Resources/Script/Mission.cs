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

    [Tooltip("삭제시킬때 씀")]
    public GameObject[] ClearTarget;
    [Tooltip("활성화만 할떄 씀")]
    public GameObject[] OnEnableTarge;
    [Tooltip("활성 비활성 둘다 되야할때 씀")]
    public GameObject[] OnDisableAndEnableTarge;

    [Tooltip("미션 오브젝트 시작할때 효과음")]
    public AudioClip StartAudioClip;

    [Tooltip("미션 오브젝트 끝날때 효과음")]
    public AudioClip effectSoundEnd;

    [SerializeField]
    [Tooltip("게임 가이드 미션 바꿀때 사용함")]
    GameObject GM;

    public bool Test = false;

    public void Update()
    {
        if (Test)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                SendMSG();
            }
        }
    }

#pragma warning disable IDE0051 // 사용되지 않는 private 멤버 제거

    public void SendMSG()
    {
        if (MissionName != null)
        {
            if(StartAudioClip != null)
                Manager.Effect_SoundPlayer.EffectSoundPlay(StartAudioClip);

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
        if (anyString != null)
        {
            Manager.ErrorInfo_Instance.ErrorEnqueue(anyString);
        }
    }

    void ItemCheck()
    {
        if (GameObject.FindWithTag("MainCamera").GetComponent<InputManager>().CurrentSelectItem != null)
        {
            if(Key != null)
            {
                if (GameObject.FindWithTag("MainCamera").GetComponent<InputManager>().CurrentSelectItem.name == Key.name)
                {
                    ItemManager.ItemManager_Instance.DeleteItem(Key.name);
                    Manager.ErrorInfo_Instance.ErrorEnqueue(CompleteMissionInfo);
                    MissionOnEnable();
                    MissionDelete();
                }
            }
            else if (keyName != null)
            {
                if (GameObject.FindWithTag("MainCamera").GetComponent<InputManager>().CurrentSelectItem.name == keyName)
                {
                    ItemManager.ItemManager_Instance.DeleteItem(keyName);
                    Manager.ErrorInfo_Instance.ErrorEnqueue(CompleteMissionInfo);
                    MissionOnEnable();
                    MissionDelete();
                }
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
        ItemCheck();

        /*string CurrentItemName = ItemManager.ItemManager_Instance.GetCurrentItem();
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
        }*/
    }

    void D_Door_Lock()
    {
        ItemCheck();
        /*string CurrentItemName = ItemManager.ItemManager_Instance.GetCurrentItem();
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
        }*/

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
        ItemCheck();
        /*string CurrentItemName = ItemManager.ItemManager_Instance.GetCurrentItem();

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
        }*/
    }

    void F_RoomDoor_Mission()
    {
        ItemCheck();
    }

    void F_Room_Watch()
    {
        setManager();
        GameObject.FindWithTag("MainCamera").transform.GetChild(0).gameObject.SetActive(false);
        Manager.UIManager_Instance.UIPopup("UI_F_GrandfatherClock");
    }

    void F_Telephone()
    {
        setManager();
        Manager.UIManager_Instance.UIPopup("UI_F_Phone");
    }

    void F_Drop_Key()
    {
        setManager();
        MissionClearSelf();
        MissionOnEnable();
    }

    void H_Door001_Massion()
    {
        ItemCheck();
        MissionGMChanged();
    }

    void H_Door002_Massion()
    {
        ItemCheck();
    }

    void H_Mannequin_Popup()
    {
        setManager();
        Manager.UIManager_Instance.UIPopup("UI_H_Mannequin");
    }

    void G_MainGate_Door_Mittion()
    {
        setManager();
        ItemCheck();
    }
#pragma warning disable IDE0051

    void MissionGMChanged()
    {
        if(GM!=null)
        {
            GM.GetComponent<LevelGuideSetting>().GMChanged();
        }
    }

    private void setManager()
    {
        Manager.Origin_Object = transform.gameObject;
        Manager.Call_Object = ClearTarget;
    }

    public void MissionClearSelf()
    {
        MissionOnEnable();
        MissionDelete();

        Manager.ErrorInfo_Instance.ErrorEnqueue(CompleteMissionInfo);
    }

    public void MissionOnEnable()
    {
        if(OnEnableTarge != null)
        {
            for (int i = 0; i < OnEnableTarge.Length; i++)
            {
                OnEnableTarge[i].SetActive(true);
            }
        }
    }

    public void TableTool_Mission()
    {
        ItemCheck();
    }

    //아이탬의 활성화 비활성화를 비교하여 선택적으로 함수를 실행해주는 함수
    //아이템 비활성 이후 활성할때 여기로 들어옴
    //아이템을 들고있어야하며 활성화 시키면 아이템이 삭제됨
    //아이템을 습득해야하는 상황이면 습득시킴
    public void MissionSelectOnDisableAndEnable()
    {
        string selectItemName = ItemManager.ItemManager_Instance.GetCurrentItem();
        GameObject[] itemslot = ItemManager.ItemManager_Instance.GetItemSlot();

        for(int i = 0; i <  itemslot.Length; i++)
        {
            for (int j = 0; i < OnDisableAndEnableTarge.Length; j++)
            {
                if (itemslot[i].name == OnDisableAndEnableTarge[j].name)
                {
                    if (OnDisableAndEnableTarge[j].transform.name == selectItemName)
                    {
                        OnDisableAndEnableTarge[j].SetActive(!OnDisableAndEnableTarge[j].activeSelf);
                        MissionClearPrint();
                        return;
                    }
                }
            }
        }

        MissionFail();
    }

    /*public void MissionSelectOnEnable(string itemName)
    {
        for (int i = 0; i < OnDisableTarge.Length; i++)
        {
            if (OnDisableTarge[i].transform.name == itemName)
                OnDisableTarge[i].SetActive(true);
        }
    }

    public void MissionSelectOnDisable(string itemName)
    {
        for(int i = 0; i < OnDisableTarge.Length; i++)
        {
            if(OnDisableTarge[i].transform.name == itemName)
                OnDisableTarge[i].SetActive(false);
        }
    }*/

    public void MissionFail()
    {
        Manager.ErrorInfo_Instance.ErrorEnqueue(FalseMissionInfo);
    }
    public void MissionClearPrint()
    {
        Manager.ErrorInfo_Instance.ErrorEnqueue(CompleteMissionInfo);
    }

    public void MissionDelete()
    {
        GameObject[] SelfTarget = Manager.Call_Object;

        for (int i = 0; i < SelfTarget.Length;i++)
        {
            Destroy(SelfTarget[i].gameObject);
        }
        if(effectSoundEnd != null)
            Manager.Effect_SoundPlayer.EffectSoundPlay(effectSoundEnd);
        Manager.ErrorInfo_Instance.ErrorEnqueue(CompleteMissionInfo);
    }
}
