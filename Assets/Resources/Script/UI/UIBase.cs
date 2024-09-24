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
        Manager.UIManager_Instance.UIPush(this.gameObject);//Todo:오류남 Fuck
    }

    public void SetSortOrdernumber()
    {
        //현재 Canvas의 sort order를 UIManager에서 배정받을거임
        Manager.UIManager_Instance.GetOrdernumber(gameObject);
    }
}
