using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    [SerializeField]
    [Header("True = 포인터 숨김 / False = 포인터 안숨김 (기본 True)")]
    private bool MouseClose = true;
    public void Start()
    {
        if (gameObject.layer == (int)Layer_Enum.LayerInfo.System_UI)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Manager.CM_Instance.SetMoveState(false);
        }
        Manager.UIManager_Instance.UIPush(this.gameObject);
        Debug.Log(gameObject.name + " : UIBase call ");
    }

    public void SetSortOrdernumber()
    {
        //현재 Canvas의 sort order를 UIManager에서 배정받을거임
        Manager.UIManager_Instance.GetOrdernumber(gameObject);
    }

    private void OnDisable()
    {
        if(MouseClose)
        {
            if (Manager.UIManager_Instance.GetUILisetStackCount() >= 1)
            {
                if (Manager.UIManager_Instance.UIListsStackReturn().Peek().transform.tag != "OnMouse" )
                    Manager.CM_Instance.OffMouseCursor();
            }

            if (gameObject.layer != (int)Layer_Enum.LayerInfo.System_UI)
                Manager.UIManager_Instance.CloseUI();
        }
    }
}
