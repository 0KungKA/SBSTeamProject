using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ButtonInteraction : MonoBehaviour
{
    //������ ������ ���̺� �������Ѵٰ��ϴ� �����ϰ� �����ϰ� ���߿� �����ϰڴٰ��ϸ� ���� �ڵ� �����ʿ�
    [SerializeField]
    [Header("UI�̸� �Ǵ� ��������/UI�̸� �������� �־��ּ���\n.~ �ʿ���� (�⺻�ּ� Prefep/UI_Prefep/) ")]
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

    public void ClockBTN()
    {
        GameObject H_Obj = GameObject.Find("H_Pivot").gameObject;
        GameObject M_Obj = GameObject.Find("M_Pivot").gameObject;
        float RotateAngles = 30;

        int H = 0;
        int M = 0;

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

        H = Mathf.FloorToInt(H_Obj.transform.rotation.eulerAngles.z % 30);
        M = Mathf.FloorToInt(M_Obj.transform.rotation.eulerAngles.z % 3);

        if (H >= 13) H = 0;
        if (M >= 13) M = 0;

        if( H == 10 && M == 10)
        {
            for(int i = 0; i < Manager.Call_Object.Length; i ++)
            {
                Destroy(Manager.Call_Object[i]);
                Manager.Call_Object = null;
            }
        }
        //Manager.Call_Object;
    }
}
