using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCF_AI_Ctrl : MonoBehaviour
{
    enum StateInfo
    {
        State,
        AttackMove,
        Move
    }

    public bool saveMove = true;//C랑 D방을 안들어가도록 설정해주는 bool값

    [SerializeField]
    [Tooltip("NPC가 플레이어를 인식할수있는 최대 거리")]
    int MaxRayDistance = 30;//NPC가 플레이어를 인식할수있는 최대 거리

    [SerializeField]
    [Tooltip("한 공간에서 순찰하는 시간 = 데이터 테이블 = 60초")]
    float MaxWaitingTime = 60;//한 공간에서 순찰하는 시간 60초

    [SerializeField]
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

    RaycastHit hit;

    StateInfo state;//현재 상태

    //agent.destination 목표지점까지 이동
    Vector3 PlayerPos;
    GameObject Target;

    bool ChoiceTarget = false;//타겟을 지정할때 분기점으로 씀
    bool OnMove = false;

    Animator anim;

    Vector3 YameOriginVector;

    public void Start()
    {
        MaxRayDistance = Manager.DataManager_Instance.GetBalanceValue(6);
        MaxWaitingTime = Manager.DataManager_Instance.GetBalanceValue(9);

        //Todo:공격 거리에 대한 데이터 테이블 수정필요함
        attackRange = Manager.DataManager_Instance.GetBalanceValue(11);
        VecOffSet = 3.0f;

        anim = GetComponent<Animator>();
        anim.SetBool("Attack", false);
        anim.SetBool("Move", true);

        PlayerPos = GameObject.Find("Player_Camera").transform.position;

        agent = GetComponent<NavMeshAgent>();
        agent.destination = PlayerPos;

        state = StateInfo.AttackMove;
    }

    internal void YameSetting()
    {
        state = StateInfo.AttackMove;
        transform.position = YameOriginVector;
    }

    /// <summary>
    /// 0 = State / 1 = Move / 2 = AttackMove
    /// </summary>
    /// <param name="value"></param>
    public void SetState(int value)
    {
        switch (value)
        {
            case 0:
                waitingDuration = 0.0f;
                state = StateInfo.State;
                Target = transform.gameObject;
                break;

            case 1:
                state = StateInfo.Move;
                waitingDuration = MaxWaitingTime + Time.deltaTime;
                break;

            case 2:
                state = StateInfo.AttackMove;
                break;

            default:
                break;
        }

    }

    /// <summary>
    /// true = 안전모드() / false = 모든방 뒤짐
    /// </summary>
    /// <param name="value"></param>
    public void SetSaveState(bool value)
    {
        saveMove = value;
    }

    public void Update()
    {
        Debug.Log(PlayerPos);

        Vector3 offsetpos = transform.position;
        offsetpos.y += 8.0f;

        if (Physics.Raycast(offsetpos, (PlayerPos - offsetpos), out hit, MaxRayDistance))
        {
            Debug.DrawLine(offsetpos, (PlayerPos - offsetpos) * MaxRayDistance, Color.red, 0.1f);

            if (hit.transform.tag == "MainCamera")
            {
                state = StateInfo.AttackMove;
            }
            else if (OnMove == false)
            {
                state = StateInfo.State;
            }
            else
            {
                state = StateInfo.Move;
            }
        }

        switch (state)
        {
            case StateInfo.State:
                Debug.Log("State : State");
                waitingDuration += Time.deltaTime;
                if (waitingDuration > MaxWaitingTime)
                {
                    OnMove = true;
                    waitingDuration = 0;
                    state = StateInfo.Move;
                }
                break;

            case StateInfo.Move:
                Debug.Log("State : Move");
                //OnMove = true;

                if (ChoiceTarget == false)
                {
                    //Target = selectTarget();
                    ChoiceTarget = true;
                }

                agent.destination = Target.transform.position;
                if (agent.remainingDistance < VecOffSet)
                {
                    OnMove = false;
                    ChoiceTarget = false;
                    state = StateInfo.State;
                }
                break;

            case StateInfo.AttackMove:
                Debug.Log("State : AttackMove");

                agent.destination = PlayerPos;

                if (agent.remainingDistance < attackRange)
                {
                    anim.SetBool("Attack", true);
                    anim.SetBool("Move", false);

                    Debug.Log("여자 Attack부분 ui예외처리함 나중에 켜주기");
                    //Manager.UIManager_Instance.UIPopup("Scene_UI/UI_Scene_GameOver");
                }
                else
                {
                    anim.SetBool("Attack", true);
                    anim.SetBool("Move", false);
                }
                break;

            default:
                break;
        }
    }
}
