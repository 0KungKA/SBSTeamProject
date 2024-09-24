using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //컷씬 재생될때 카메라가 움직이면 안되니까 움직임을 제한하는 변수
    bool MoveState = true;
    bool RotState = true;

    public bool OnHide = false;

    [SerializeField]//디버그 전용
    bool DebugModeFalse = true;
    public bool GetDebugFalse() {  return DebugModeFalse; }

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
        DebugModeFalse = true;
    }

    public void OnMouseCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        MoveState = false;
        RotState = false;
    }

    private void Update()
    {
        GameObject.Find("UI_Scene_Main").transform.GetChild(GameObject.Find("UI_Scene_Main").transform.childCount - 1).gameObject.SetActive(DebugModeFalse);
        if (Input.GetKeyDown(KeyCode.F12))
        {
            DebugModeFalse = !DebugModeFalse;
            GameObject.Find("UI_Scene_Main").transform.GetChild(GameObject.Find("UI_Scene_Main").transform.childCount - 1).gameObject.SetActive(!DebugModeFalse);
        }

        if (CM == null)
            CM = GameObject.Find("Player_Camera").gameObject.GetComponent<CameraMove>();
        
        if (MoveState && OnHide == false)
            CM.Move();

        if (RotState)
            CM.CameraRot();
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
