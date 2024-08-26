using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
}
