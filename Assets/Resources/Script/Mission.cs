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

    [SerializeField]
    string keyName;

    [Tooltip("������ų�� ��")]
    public GameObject[] ClearTarget;
    [Tooltip("Ȱ��ȭ�� �ҋ� ��")]
    public GameObject[] OnEnableTarge;
    [Tooltip("Ȱ�� ��Ȱ�� �Ѵ� �Ǿ��Ҷ� ��")]
    public GameObject[] OnDisableAndEnableTarge;

    [Tooltip("�̼� ������Ʈ �����Ҷ� ȿ����")]
    public AudioClip StartAudioClip;

    [Tooltip("�̼� ������Ʈ ������ ȿ����")]
    public AudioClip effectSoundEnd;

    [SerializeField]
    [Tooltip("���� ���̵� �̼� �ٲܶ� �����")]
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

#pragma warning disable IDE0051 // ������ �ʴ� private ��� ����

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
            Debug.Log(transform.name + "Mission�� MissionName�� �����Ǿ� �����ʽ��ϴ�");
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

    //�������� Ȱ��ȭ ��Ȱ��ȭ�� ���Ͽ� ���������� �Լ��� �������ִ� �Լ�
    //������ ��Ȱ�� ���� Ȱ���Ҷ� ����� ����
    //�������� ����־���ϸ� Ȱ��ȭ ��Ű�� �������� ������
    //�������� �����ؾ��ϴ� ��Ȳ�̸� �����Ŵ
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
