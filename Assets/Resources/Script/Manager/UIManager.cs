using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    //UI�� ���� ���� + �ý���UI�� �ε� + �ݱ�
    static Stack<GameObject> UIListsStack = new Stack<GameObject>();
    public Stack<GameObject> UIListsStackReturn() {  return UIListsStack; }
    public int GetUILisetStackCount() { return UIListsStack.Count; }

    [Header("�ִ� UI ����\n(�� �̻��̸� ���׷� �Ǵ��ϰ� ���� ��������)")]
    public int MaximumUICount = 10;

    /// <summary>
    /// UI�� �⺻�ּ�
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
        GameObject go = Instantiate(prefepPath);//�ϴ� Instantiate�� ����
        int SCindex = go.name.IndexOf("(Clone)");//�̸��߿� (Clone) �� ã�Ƽ� ����� �ε������� �����ϴ��� ������
        if (SCindex > 0)//���� (Clone) �̶� �ܾ ���� �޾ƿ� ���� ������ �Ʒ��� �ڵ带 ����
            go.name = go.name.Substring(0, SCindex);//���꽺Ʈ������ (Clone) �ܾ� ���۱����� ©�� �̸� �ھƹ���

        return go;
    }

    protected internal void SpawnRenderView()//Ư���������� ���� ����
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
    /// UI�� �����ؾ��ҋ� ȣ���ϴ� �Լ�
    /// �˾��ϴ� UI�� �⺻������ Canvas ������Ʈ�� UIBase ������Ʈ�� �����ϰ��ִٰ� ������
    /// �⺻���� �ּҴ� �ڱ���־ UI_Prefep���� �ڱⰡ �˾Ƽ� �����ϴ� UI�� �̸��� �Ű������� ������
    /// ���� ���������� �����ϸ� �����̸�/UI�̸� ������
    /// </summary>
    /// <param name="UI_Path"></param>
    public void UIPopup(string UI_Path)
    {
        
        GameObject prefep = Resources.Load<GameObject>(defualPath + UI_Path);
        if (prefep != null)
        {
            foreach(GameObject obj in UIListsStack)//Ȥ�ø� ��Ȳ�� ����ؼ� �����̸����� UI�� ������ �ٷ� return�ھƹ���
            {
                if(obj != null)
                    if(obj.name == prefep.name)//�̷����� ������ �����Ҷ�(Clone)����
                        return;
            }
            if (prefep.name == "UI_Instant_Popup") //ErrorInfo�ʿ��� ������ UI�� �ڵ������Ǳ⶧���� ����Push������
            {
                if (GameObject.Find("UI_Instant_Popup"))
                    return;

                SpawnUI(prefep);
                return;
            }
            UIListsStack.Push(SpawnUI(prefep));//���� Push

        }
        else
            Debug.Log("UIPopup : " + "Error path" + UI_Path);//�������� ��ã���� � �������� ��ã�Ҵ��� Ȯ���ϱ����� �ּ���ü�� ����׷α׿� �����
    }
    /// <summary>
    /// UI�� Popup�� ��ġ�� �ʰ� ������ ��� ȣ���ϴ� �Լ�
    /// </summary>
    /// <param name="go"></param>
    public void UIPush(GameObject go)
    {
        foreach (GameObject obj in UIListsStack)//Ȥ�ø𸣴� �ϴ� �˻�
        {
            if (obj.name == go.name)//�˻��ؼ� �ɸ��� Push�� ��ŵ�ϰ� order ��ȣ�� ���� �ø�
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
            go.GetComponent<Canvas>().sortingOrder = UIListsStack.Count-1;//�Ŵ������� �ڵ������� ȣ���� �־������
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
            Debug.Log("Kill"); //���ݴ����� �� ������ ��� �ϴ� ����׷α׷� ��¸��ؼ� ���װ� �������� ���������� Ȯ���ϱ�����
        }
    }

    public void CloseAllUI()
    {
        while( UIListsStack.Count > 0 )
        {
            Debug.Log(transform.name + "while �۵���");
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
            //Debug.Log(transform.name + "StopAllCoroutines �۵�");
            //StopAllCoroutines();
            Destroy(go);
        }
    }
}
