using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    static Manager M_instance;
    public static Manager Instance { get { return M_instance; } }

    DataManager _DataManager = new DataManager();
    public static DataManager DataManager_Instance { get { return Instance._DataManager; } }

    ErrorInfo _errorInfo = new ErrorInfo();
    public static ErrorInfo ErrorInfo_Instance { get { return Instance._errorInfo; } }

    UIManager _UIManager = new UIManager();
    public static UIManager UIManager_Instance { get { return Instance._UIManager; } }

    CameraManager _CameraManager = new CameraManager();
    public static CameraManager CM_Instance { get { return Instance._CameraManager; } }

    GameObject _OriginObject = null;
    public static GameObject Origin_Object {
        get
        {
            if (Instance._OriginObject != null)
                return Instance._OriginObject;
            else
                return null;
        }
        set
        {
            Instance._OriginObject = value;
        }
    }

    GameObject[] _CallObject = null;
    public static GameObject[] Call_Object
    { 
        set{ Instance._CallObject = value; }

        get {
            if (Instance._CallObject != null)
                return Instance._CallObject;
            else
                return null;
        }
    }

    static GameObject Manager_Obj;//�Ŵ����� �߰��� ������Ʈ
    //public GameObject GetManager() { return Manager_Obj; }

    private void Awake()
    {
        Init();
        _UIManager.Init();
    }

    static void Init()
    {

        if (M_instance == null)
        {
            Manager_Obj = GameObject.Find("@Managers");
            if (Manager_Obj == null)
            {
                Manager_Obj = new GameObject { name = "@Managers" };
                Manager_Obj.AddComponent<Manager>();
            }
            DontDestroyOnLoad(Manager_Obj);
            M_instance = Manager_Obj.GetComponent<Manager>();
        }

        Instance._errorInfo = Instance.GetComponent<ErrorInfo>();

        if (Instance._UIManager == null)
            Instance._UIManager = Manager_Obj.AddComponent<UIManager>();
    }

    private void Update()
    {

    }

    protected internal void Setting()
    {
        if(GetComponentInChildren<CameraManager>() == null)
            Instance._CameraManager = Manager_Obj.AddComponent<CameraManager>();
        else
            Instance._CameraManager = Manager_Obj.GetComponentInChildren<CameraManager>();

        if (GetComponentInChildren<DataManager>() == null)
            Instance._DataManager = Manager_Obj.AddComponent<DataManager>();
        else
            Instance._DataManager = Manager_Obj.GetComponentInChildren<DataManager>();

    }

    void OnEnable()
    {
        // �� �Ŵ����� sceneLoaded�� ȣ�� ������
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // ȣ���� �����ؼ� �� �Լ��� �� ������ ȣ��ȴ�.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {//����� ����Ǵ��� Ȯ����
        if(_UIManager.GetUILisetStackCount() >= 1)
            _UIManager.CloseAllUI();
    }

    void OnDisable()
    {
        //ȣ������ϱ� ���� Ǯ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public GameObject Instantiate(GameObject got)
    {
        GameObject go = UnityEngine.Object.Instantiate(got);//�ϴ� Instantiate�� ����
        int SCindex = go.name.IndexOf("(Clone)");//�̸��߿� (Clone) �� ã�Ƽ� ����� �ε������� �����ϴ��� ������
        if (SCindex > 0)//���� (Clone) �̶� �ܾ ���� �޾ƿ� ���� ������ �Ʒ��� �ڵ带 ����
            go.name = go.name.Substring(0, SCindex);//���꽺Ʈ������ (Clone) �ܾ� ���۱����� ©�� �̸� �ھƹ���

        return go;
    }
}

