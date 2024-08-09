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


    static GameObject Manager_Obj;//매니저가 추가될 오브젝트
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
        // 씬 매니저의 sceneLoaded에 호출 예약함
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 호출을 예약해서 이 함수는 매 씬마다 호출된다.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {//제대로 실행되는지 확인함
        if(_UIManager.GetUILisetStackCount() >= 1)
            _UIManager.CloseAllUI();
    }

    void OnDisable()
    {
        //호출됬으니까 예약 풀어줌
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public GameObject Instantiate(GameObject got)
    {
        GameObject go = Object.Instantiate(got);//일단 Instantiate로 만듦
        int SCindex = go.name.IndexOf("(Clone)");//이름중에 (Clone) 을 찾아서 몇번쨰 인덱스부터 시작하는지 가져옴
        if (SCindex > 0)//만약 (Clone) 이란 단어가 들어가서 받아온 값이 있으면 아래쪽 코드를 실행
            go.name = go.name.Substring(0, SCindex);//서브스트링으로 (Clone) 단어 시작까지를 짤라서 이름 박아버림

        return go;
    }
}

