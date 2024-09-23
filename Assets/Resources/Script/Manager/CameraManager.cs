using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //�ƾ� ����ɶ� ī�޶� �����̸� �ȵǴϱ� �������� �����ϴ� ����
    bool MoveState = true;
    bool RotState = true;

    public bool OnHide = false;

    [SerializeField]//����� ����
    bool DebugMode = false;

    protected internal void SetMoveState(bool value) { MoveState = value; }
    protected internal void SetRotState(bool value) { RotState = value; }

    static CameraMove CM;

    public void Start()
    {
        CM = GameObject.Find("Player_Camera").gameObject.GetComponent<CameraMove>();
        CM.Init();
    }

    public void OffMouseCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        MoveState = true;
        RotState = true;
        DebugMode = false;
    }

    public void OnMouseCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
        MoveState = false;
        RotState = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            DebugMode = !DebugMode;

        if (CM == null)
            CM = GameObject.Find("Player_Camera").gameObject.GetComponent<CameraMove>();

        if (DebugMode == true)
        {
            CM.DebugMode();
        }
        else
        {
            if (MoveState)
                CM.Move();

            if (RotState)
                CM.CameraRot();
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
