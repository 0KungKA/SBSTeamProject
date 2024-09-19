using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    //UI의 스택 관리 + 시스템UI의 로드 + 닫기
    static Stack<GameObject> UIListsStack = new Stack<GameObject>();
    public Stack<GameObject> UIListsStackReturn() {  return UIListsStack; }
    public int GetUILisetStackCount() { return UIListsStack.Count; }

    [Header("최대 UI 갯수\n(이 이상이면 버그로 판단하고 게임 강제종료)")]
    public int MaximumUICount = 10;

    /// <summary>
    /// UI의 기본주소
    /// </summary>
    string defualPath = "Prefep/UI_Prefep/";

    private void Start()
    {
        if (UIListsStack.Count != 0)
            UIListsStack.Clear();
    }

    protected internal void Init()
    {
        string SystemUIName = "Scene_UI/" + "UI_Scene_" + SceneManager.GetActiveScene().name;
        if (SystemUIName == "Scene_UI/UI_Scene_/SLobby")
        {
            UIPopup("Scene_UI/UI_Scene_/Lobby");
        }
        else
        {
            UIPopup(SystemUIName);
        }
    }

    public GameObject SpawnUI(GameObject prefepPath)
    {
        GameObject go = Instantiate(prefepPath);//일단 Instantiate로 만듦
        int SCindex = go.name.IndexOf("(Clone)");//이름중에 (Clone) 을 찾아서 몇번쨰 인덱스부터 시작하는지 가져옴
        if (SCindex > 0)//만약 (Clone) 이란 단어가 들어가서 받아온 값이 있으면 아래쪽 코드를 실행
            go.name = go.name.Substring(0, SCindex);//서브스트링으로 (Clone) 단어 시작까지를 짤라서 이름 박아버림

        return go;
    }

    protected internal void SpawnRenderView()//특수성때문에 따로 관리
    {
        GameObject prefep = Resources.Load<GameObject>(defualPath + "UI_Item_Render");
        if(prefep == null)
        {
            Debug.Log("UI_Render error");
            return;
        }
        prefep = SpawnUI(prefep);
        DontDestroyOnLoad(prefep);
    }
    

    /// <summary>
    /// UI를 생성해야할떄 호출하는 함수
    /// 팝업하는 UI는 기본적으로 Canvas 컴포넌트와 UIBase 컴포넌트를 포함하고있다고 가정함
    /// 기본적인 주소는 자기고있어서 UI_Prefep까진 자기가 알아서 접근하니 UI의 이름만 매개변수로 넣을것
    /// 만약 하위폴더가 존재하면 폴더이름/UI이름 넣을것
    /// </summary>
    /// <param name="UI_Path"></param>
    public void UIPopup(string UI_Path)
    {
        
        GameObject prefep = Resources.Load<GameObject>(defualPath + UI_Path);
        if (prefep != null)
        {
            foreach(GameObject obj in UIListsStack)//혹시모를 상황을 대비해서 같은이름으로 UI가 들어오면 바로 return박아버림
            {
                if(obj != null)
                    if(obj.name == prefep.name)//이럴려고 위에서 스폰할때(Clone)지움
                        return;
            }
            if (prefep.name == "UI_Instant_Popup") //ErrorInfo쪽에서 들어오는 UI는 자동삭제되기때문에 따로Push안해줌
            {
                if (GameObject.Find("UI_Instant_Popup"))
                    return;

                SpawnUI(prefep);
                return;
            }
            UIListsStack.Push(SpawnUI(prefep));//스택 Push

        }
        else
            Debug.Log("UIPopup : " + "Error path" + UI_Path);//프리팹을 못찾으면 어떤 프리팹을 못찾았는지 확인하기위해 주소전체를 디버그로그에 출력함
    }
    /// <summary>
    /// UI가 Popup을 거치지 않고 생성된 경우 호출하는 함수
    /// </summary>
    /// <param name="go"></param>
    public void UIPush(GameObject go)
    {
        foreach (GameObject obj in UIListsStack)//혹시모르니 일단 검사
        {
            if (obj.name == go.name)//검사해서 걸리면 Push는 스킵하고 order 번호만 위로 올림
            {
                GetOrdernumber(go);
                return;
            }
        }
        UIListsStack.Push(go);
        GetOrdernumber(go);
    }

    public void GetOrdernumber(GameObject go)
    {
        if (go != null)
            go.GetComponent<Canvas>().sortingOrder = UIListsStack.Count-1;//매니저에서 코드쪽으로 호출을 넣었을경우
    }

    private void Update()
    {
        //Debug.Log(UIListsStack.Count);
    }

    public void CheackUICount()
    {
        if (UIListsStack.Count >= MaximumUICount)
        {
            //System.Diagnostics.Process.GetCurrentProcess().Kill();
            Debug.Log("Kill"); //지금당장은 끌 이유가 없어서 일단 디버그로그로 출력만해서 버그가 터졌는지 안터졌는지 확인하기위함
        }
    }

    public void CloseAllUI()
    {
        while( UIListsStack.Count > 0 )
        {
            Debug.Log(transform.name + "while 작동중");
            GameObject go = UIListsStack.Pop();
            Destroy(go);
        }
        Init();
    }

    public void CloseUI()
    {
        if (UIListsStack.Peek().gameObject.tag == "LevelUI")
            return;

        GameObject go = UIListsStack.Pop();

        if(go !=null)
        {
            //Debug.Log(transform.name + "StopAllCoroutines 작동");
            //StopAllCoroutines();
            Destroy(go);
        }
    }
}
