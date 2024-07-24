using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //플레이어 조작
    //예) ESC눌렀을때 일시정지화면 출력같은거 여기서 처리할 예정

    internal GameObject CurrentSelectItem;

    void Start()
    {
        CurrentSelectItem = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Manager.UIManager_Instance.UIPopup("UI_Puase");
        }
    }

    public void SelectItem(GameObject item)
    {
        if (CurrentSelectItem == item)
        {
            CurrentSelectItem = null;
            return;
        }
        else
            CurrentSelectItem = item;

        if(item.GetComponent<ItemInfo>() != null)
        {
            Manager.ErrorInfo_Instance.ErrorEnqueue(item.GetComponent<ItemInfo>().ItemExplanatino);
        }
    }
}
