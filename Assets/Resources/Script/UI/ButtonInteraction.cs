using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ButtonInteraction : MonoBehaviour
{
    //지금은 데이터 테이블 연동안한다고하니 간단하게 제작하고 나중에 연동하겠다고하면 따로 코드 수정필요
    [SerializeField]
    [Header("UI이름 또는 하위폴더/UI이름 형식으로 넣어주세요\n.~ 필요없음 (기본주소 Prefep/UI_Prefep/) ")]
    string OpenPrefepPath;

    public void Open()
    {
        if(OpenPrefepPath != null)
            Manager.UIManager_Instance.UIPopup(OpenPrefepPath);
    }

    public void OpenLevel()
    {
        SceneManager.LoadScene(OpenPrefepPath);
    }

    public void OpenLobby()
    {
        GameObject render = GameObject.Find("UI_Item_Render");
        if (render != null)
            Destroy(render);

        GameObject[] @Obj = GameObject.FindGameObjectsWithTag("Managers");
        foreach (GameObject obj in @Obj)
        {
            Destroy(obj);
        }

        SceneManager.LoadScene("Lobby");
    }

    public void Restart()
    {
        OpenLobby();
        SceneManager.LoadScene("Main");
    }

    public void Close()
    {
        Manager.UIManager_Instance.CloseUI();
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ItemRenderClose()
    {
        RenderViewObj gm = GameObject.FindWithTag("Target").GetComponent<RenderViewObj>();
        gm.DestroyTarget();

    }

    public void CheckItem()
    {
        GameObject.Find("EventSystem").GetComponent<Synthesis>().SynthesisModule();
    }

    int H = 0;
    int M = 0;

    public void ClockBTN()
    {
        GameObject H_Obj = GameObject.Find("H_Pivot").gameObject;
        GameObject M_Obj = GameObject.Find("M_Pivot").gameObject;
        float RotateAngles = 30;

        if(this.name == "H_P")
        {
            H_Obj.transform.Rotate(0, 0, RotateAngles);
        }
        else if (this.name == "H_M")
        {
            H_Obj.transform.Rotate(0, 0, -RotateAngles);
        }
        else if (this.name == "M_P")
        {
            M_Obj.transform.Rotate(0, 0, RotateAngles);
        }
        else if (this.name == "M_M")
        {
            M_Obj.transform.Rotate(0, 0, -RotateAngles);
        }
    }

    public void ClockCheck()
    {
        H = Mathf.FloorToInt(GameObject.Find("H_Pivot").gameObject.transform.rotation.eulerAngles.z / 30);
        M = Mathf.FloorToInt(GameObject.Find("M_Pivot").gameObject.transform.rotation.eulerAngles.z / 6);

        H = (H >= 13) ? H = 1 : H;
        M = (M >= 13) ? M = 10 : M;

        if (H == 10 && M == 10)
        {
            Manager.Origin_Object.GetComponent<Mission>().MissionOnEnable();
            Manager.Origin_Object.GetComponent<Mission>().MissionDelete();
        }
        else
        {
            Manager.Origin_Object.GetComponent<Mission>().MissionFail();
        }
    }

    public void OnCamera()
    {
        GameObject.FindWithTag("MainCamera").transform.GetChild(0).gameObject.SetActive(true);
    }
}
