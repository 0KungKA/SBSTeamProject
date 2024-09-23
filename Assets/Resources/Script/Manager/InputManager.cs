using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //�÷��̾� ����
    //��) ESC�������� �Ͻ�����ȭ�� ��°����� ���⼭ ó���� ����

    public GameObject CurrentSelectItem;

    bool OnMap = false;
    public void StartOnMap() { OnMap = true; }

    void Start()
    {
        CurrentSelectItem = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject npcTalk = GameObject.Find("UI_ChatNPC");
            GameObject Fade = GameObject.Find("_Canvas");

            if (npcTalk == null && Fade == null)
                Manager.UIManager_Instance.UIPopup("UI_Puase");
        }
        else if(Input.GetKeyDown(KeyCode.M) && OnMap)
        { 
            Manager.UIManager_Instance.UIPopup("UI_Map");
        }
    }

    public void SelectItem(GameObject item)
    {
        CurrentSelectItem = item;

        if(item.GetComponent<ItemInfo>() != null)
        {
            Manager.ErrorInfo_Instance.ErrorEnqueue(item.GetComponent<ItemInfo>().ItemExplanatino);
        }
    }
}
