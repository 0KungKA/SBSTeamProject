using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class NPCM_AI_Ctrl : MonoBehaviour
{
    enum StateInfo
    {
        State,
        AttackMove,
        Move
    }

    [SerializeField]
    [Tooltip("NPC가 플레이어를 인식할수있는 최대 거리")]
    int MaxRayDistance = 30;//NPC가 플레이어를 인식할수있는 최대 거리

    [SerializeField]
    [Tooltip("한 공간에서 순찰하는 시간 = 데이터 테이블 = 60초")]
    float MaxWaitingTime = 60;//한 공간에서 순찰하는 시간 60초

    [SerializeField]
    [ReadOnly(true)]
    [Tooltip("현재 순찰을 얼마나 하였는지에 대한 시간")]
    float waitingDuration = 0;//현재 순찰을 얼마나 하였는지에 대한 시간

    [SerializeField]
    [Tooltip("NPC가 플레이어를 인식할수있는 최대 거리")] 
    float attackRange = 10.0f;

    [SerializeField]
    [Tooltip("순찰 지점에서 대충 얼마나 떨어질것인지")] 
    float VecOffSet = 3.0f;

    [SerializeField]
    [Tooltip("NPC 이동속도")]
    float Speed = 3.0f;

    NavMeshAgent agent;

    GameObject[] Ways;
    GameObject[] FWays;

    RaycastHit hit;

    StateInfo state;//현재 상태

    //agent.destination 목표지점까지 이동
    GameObject Player;
    Vector3 TargetPos;
    
    bool ChoiceTarget = false;

    Vector3 YameOriginVector;

    public void Start()
    {
        YameOriginVector = transform.position;

        agent = GetComponent<NavMeshAgent>();
        Player = GameObject.FindWithTag("MainCamera").gameObject;//맨처음 플레이어의 게임오브젝트를 가져옴 / 후에 플레이어 추격에 쓰임
        //웨이포인트들의 게임오브젝트를 전부 가져옴
        GameObject wayP = GameObject.Find("WayPoint").gameObject;
        Ways = new GameObject[wayP.transform.childCount];
        for (int i = 0; i < wayP.transform.childCount; i ++ )
        {
            Ways[i] = wayP.transform.GetChild(i).gameObject;
        }

        FWays = new GameObject[wayP.transform.childCount-1];
        for (int i = 0; i < wayP.transform.childCount; i++)
        {
            if(i == 1)
                continue;

            Ways[i] = wayP.transform.GetChild(i).gameObject;
        }
        state = StateInfo.State;
    }

    internal void YameSetting()
    {
        state = StateInfo.State;
        transform.position = YameOriginVector;
    }

    public void Update()
    {
        Vector3 offsetpos = transform.position;
        offsetpos.y += 8.0f;

        if (Physics.Raycast(offsetpos, (Player.transform.position - offsetpos), out hit, MaxRayDistance))
        {
            if (hit.transform.tag == "MainCamera")
            {
                state = StateInfo.AttackMove;
            }
        }

        switch (state)
        {
            case StateInfo.State://대기상태일때
                Debug.Log("State : State");
                waitingDuration += Time.deltaTime;//duration을 더해주고 만일 대기시간이 지났을때 다른곳 순찰하러감
                if (waitingDuration > MaxWaitingTime)
                {
                    state = StateInfo.Move;
                    ChoiceTarget = false;
                }
                break;

            case StateInfo.Move:
                Debug.Log("State : Move");

                if (ChoiceTarget == false)
                {
                    if(GameObject.Find("First_Safe").gameObject != null)
                    {
                        TargetPos = FWays[Random.Range(0, Ways.Length)].transform.position;
                    }
                    else
                    {
                        TargetPos = Ways[Random.Range(0, Ways.Length)].transform.position;
                    }
                    ChoiceTarget = true;
                }
                agent.destination = TargetPos;

                if (Vector3.Distance(offsetpos, TargetPos) < VecOffSet)
                {
                    state = StateInfo.State;
                }
                break;

            case StateInfo.AttackMove:
                Debug.Log("State : AttackMove");

                agent.destination = Player.transform.position;

                if (Vector3.Distance(offsetpos, Player.transform.position) < attackRange)
                {
                    Debug.Log("Attack");
                    //Manager.UIManager_Instance.UIPopup("Scene_UI/UI_Scene_GameOver");
                }
                break;

            default:
                break;
        }
    }
}
