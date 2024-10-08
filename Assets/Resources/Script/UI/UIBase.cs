using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIBase : MonoBehaviour
{
    [SerializeField]
    [Header("True = 포인터 숨김 / False = 포인터 안숨김 (기본 True)")]
    public bool MouseClose = true;
    public bool StopSound;
    public void Start()
    {
        if (StopSound)
            Manager.Effect_SoundPlayer.EffectSoundEnd();
        if(transform.name == "UI_Scene_Credit")
        {
            Invoke("Credit", 7.0f);
        }
        if(transform.name != "UI_Scene_Credit2")
        {
            Debug.Log(gameObject.name + " : UIBase call ");
            if (gameObject.layer == (int)Layer_Enum.LayerInfo.System_UI)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Manager.CM_Instance.SetMoveState(false);
                Manager.CM_Instance.SetRotState(false);
            }
            Manager.UIManager_Instance.UIPush(this.gameObject);
        }
    }

    public void SetSortOrdernumber()
    {
        //현재 Canvas의 sort order를 UIManager에서 배정받을거임
        Manager.UIManager_Instance.GetOrdernumber(gameObject);
    }

    public void Credit()
    {
        Manager.UIManager_Instance.UIPopup("Scene_UI/UI_Scene_Credit2");
        Invoke("End", 7.0f);
    }

    public void End()
    {
        SceneManager.LoadScene("Lobby");
    }

    private void OnDestroy()
    {
        Manager.Effect_SoundPlayer.PlaySound = true;
    }
}
