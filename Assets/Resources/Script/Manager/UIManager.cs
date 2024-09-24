using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    //UI�� ���� ���� + �ý���UI�� �ε� + �ݱ�
    //static Stack<GameObject> UIListsStack = new Stack<GameObject>();
    static List<GameObject> UIListsStack = new List<GameObject>();
    //public Stack<GameObject> UIListsStackReturn() {  return UIListsStack; }

    public int GetUILisetStackCount() { return UIListsStack.Count; }

    [Header("�ִ� UI ����\n(�� �̻��̸� ���׷� �Ǵ��ϰ� ���� ��������)")]
    public int MaximumUICount = 10;

    /// <summary>
    /// UI�� �⺻�ּ�
    /// </summary>
    string defualPath = "Prefep/UI_Prefep/";

    private void Start()
    {
        CloseAllUI();

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

    public string DeletClone(string name)
    {
        int SCindex = name.IndexOf("(Clone)");
        if (SCindex > 0)
            name = name.Substring(0, SCindex);

        return name;
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
        GameObject go = GameObject.Find(prefep.name);
        if (go == null)
        {
            if (prefep != null)
            {
                foreach (GameObject obj in UIListsStack)//Ȥ�ø� ��Ȳ�� ����ؼ� �����̸����� UI�� ������ �ٷ� return�ھƹ���
                {
                    if (obj != null)
                        if (obj.name == prefep.name)//�̷����� ������ �����Ҷ�(Clone)����
                            return;
                }
                if (prefep.name == "UI_Instant_Popup") //ErrorInfo�ʿ��� ������ UI�� �ڵ������Ǳ⶧���� ����Push������
                {
                    if (GameObject.Find("UI_Instant_Popup"))
                        return;

                    SpawnUI(prefep);
                    return;
                }
                SortUI();
                GameObject createUI = SpawnUI(prefep);
                createUI.transform.GetComponent<Canvas>().sortingOrder = UIListsStack.Count - 1;
                UIListsStack.Add(createUI);

                if (UIListsStack[UIListsStack.Count - 1].GetComponent<UIBase>().MouseClose == false)
                {
                    Manager.CM_Instance.OnMouseCursor();
                }
                else
                    Manager.CM_Instance.OffMouseCursor();
            }
            else
                Debug.Log("UIPopup : " + "Error path" + UI_Path);//�������� ��ã���� � �������� ��ã�Ҵ��� Ȯ���ϱ����� �ּ���ü�� ����׷α׿� �����
        }
    }

    private void SortUI()
    {
        for (int i = UIListsStack.Count; i < 0; i--) 
        {
            if (UIListsStack[i - 1] == null) 
            {
                UIListsStack.RemoveAt(i-1);
            }
        }
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
                GetOrdernumber(obj);
                return;
            }
        }
        UIListsStack.Add(go);
        GetOrdernumber(go);
    }

    public GameObject ReturnUIObj(string name)
    {
        for(int i = 0; i <  UIListsStack.Count; i++)
        {
            if (UIListsStack[i].name == name)
                return UIListsStack[i];
        }

        return null;
    }

    public void GetOrdernumber(GameObject go)
    {
        if (go != null)
            go.GetComponent<Canvas>().sortingOrder = UIListsStack.Count-1;//�Ŵ������� �ڵ������� ȣ���� �־������
    }

    private void Update()
    {
        
    }

    public void CloseAllUI()//�� ��ȯ �Ҷ��� ȣ��
    {
        for(int i = UIListsStack.Count; i < 0 ; i --)
        {
            Destroy(UIListsStack[i-1].gameObject);
            UIListsStack.RemoveAt(i-1);
        }

        UIListsStack.Clear();
        Init();
    }

    public void CloseUI(string name)
    {
        for (int i = UIListsStack.Count; i > 0; i--)
        {
            if (UIListsStack[i - 1].name == name)
            {
                GameObject go = UIListsStack[i - 1].gameObject;

                if (UIListsStack[i - 2].GetComponent<UIBase>().MouseClose == true)
                    Manager.CM_Instance.OffMouseCursor();
                else
                    Manager.CM_Instance.OnMouseCursor();

                UIListsStack.RemoveAt(i - 1);
                Destroy(go);
            }
        }
    }

    public void CloseUI()
    {
        int currentUINumber = UIListsStack.Count;
        if(currentUINumber == 0)
        {
            return;
            
        }
        else if(currentUINumber == 1)
        {
            GameObject go = UIListsStack[currentUINumber - 1];
            UIListsStack.RemoveAt(currentUINumber - 1);
            Destroy(go);
        }
        else
        {
            GameObject go = UIListsStack[currentUINumber - 1];

            if(UIListsStack[currentUINumber - 2].GetComponent<UIBase>().MouseClose == true)
            {
                Manager.CM_Instance.OffMouseCursor();
            }
            else
                Manager.CM_Instance.OnMouseCursor();

            UIListsStack.RemoveAt(currentUINumber - 1);
            Destroy(go);
        }
    }
}
