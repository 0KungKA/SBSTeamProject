using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    [SerializeField]
    [Header("True = ������ ���� / False = ������ �ȼ��� (�⺻ True)")]
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
        //���� Canvas�� sort order�� UIManager���� ������������
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
