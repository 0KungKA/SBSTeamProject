using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //�ƾ� ����ɶ� ī�޶� �����̸� �ȵǴϱ� �������� �����ϴ� ����
    bool MoveState = true;
    protected internal void SetMoveState(bool value) { this.MoveState = value; }

    static CameraMove CM;

    public void Start()
    {
        OffMouseCursor();

        CM = CM = GameObject.FindWithTag("MainCamera").gameObject.GetComponent<CameraMove>();
        CM.Init();
    }

    public void OffMouseCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        MoveState = true;
    }

    public void OnMouseCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
        MoveState = false;
    }

    private void Update()
    {
        if (MoveState)
        {
            CM.CameraUpdate();
        }
    }

    protected internal void print(string msg)
    {
        Debug.Log(msg);
    }

    private void OnEnable()
    {
        CM = GameObject.FindWithTag("MainCamera").gameObject.GetComponent<CameraMove>();
        CM.Init();
    }
}
