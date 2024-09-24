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
    public bool MouseClose = true;
    public void Start()
    {
        Debug.Log(gameObject.name + " : UIBase call ");
        if (gameObject.layer == (int)Layer_Enum.LayerInfo.System_UI)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Manager.CM_Instance.SetMoveState(false);
            Manager.CM_Instance.SetRotState(false);
        }
        Manager.UIManager_Instance.UIPush(this.gameObject);//Todo:������ Fuck
    }

    public void SetSortOrdernumber()
    {
        //���� Canvas�� sort order�� UIManager���� ������������
        Manager.UIManager_Instance.GetOrdernumber(gameObject);
    }
}
