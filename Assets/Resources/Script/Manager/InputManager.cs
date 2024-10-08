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
            GameObject cutScene = GameObject.Find("UI_CutScene");

            if (npcTalk == null && Fade == null && cutScene == null)
                Manager.UIManager_Instance.UIPopup("UI_Puase");
        }

        if (Input.GetKeyDown(KeyCode.M) && OnMap)
        {
            if (GameObject.Find("UI_Map"))
                Manager.UIManager_Instance.CloseUI("UI_Map");
            else
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
