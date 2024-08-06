using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        GameObject gm = GameObject.FindWithTag("Target").transform.GetChild(0).gameObject;
        gm.transform.parent = null;
        Destroy(gm);
    }

    public void CheckItem()
    {
        GameObject.Find("EventSystem").GetComponent<Synthesis>().SynthesisModule();
    }
}
