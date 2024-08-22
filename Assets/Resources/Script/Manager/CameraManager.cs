using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //컷씬 재생될때 카메라가 움직이면 안되니까 움직임을 제한하는 변수
    bool MoveState = true;
    bool RotState = true;

    [SerializeField]//디버그 전용
    bool DebugMode = false;

    protected internal void SetMoveState(bool value) { MoveState = value; }
    protected internal void SetRotState(bool value) { RotState = value; }

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

        if(DebugMode == true)
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
