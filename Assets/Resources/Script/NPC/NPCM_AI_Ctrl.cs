using Cinemachine.Utility;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class NPCM_AI_Ctrl : MonoBehaviour
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
    float Speed = 5.0f;

    NavMeshAgent agent;

    GameObject[] Ways;

    RaycastHit hit;

    StateInfo state;//현재 상태

    //agent.destination 목표지점까지 이동
    GameObject Player;
    Vector3 Target;
    
    bool ChoiceTarget = false;//타겟을 지정할때 분기점으로 씀
    bool OnMove = false;
    bool PatrolMove = false;

    Animator anim;

    public void Start()
    {
        //Todo:탐지 거리에 대한 데이터 테이블 수정필요함
        MaxRayDistance = Manager.DataManager_Instance.GetBalanceValue(6);
        MaxRayDistance = 45;

        MaxWaitingTime = Manager.DataManager_Instance.GetBalanceValue(10);

        //Todo:공격 거리에 대한 데이터 테이블 수정필요함
        attackRange = Manager.DataManager_Instance.GetBalanceValue(11);
        attackRange = 20;
        VecOffSet = 10.0f;

        anim = GetComponent<Animator>();
        anim.SetBool("Attack", false);
        anim.SetBool("Move", true);

        agent = GetComponent<NavMeshAgent>();
        Player = GameObject.Find("Player_Camera").gameObject;//맨처음 플레이어의 게임오브젝트를 가져옴 / 후에 플레이어 추격에 쓰임

        //웨이포인트들의 게임오브젝트를 전부 가져옴
        GameObject wayP = GameObject.Find("WayPoint").gameObject;
        Ways = new GameObject[wayP.transform.childCount];
        for (int i = 0; i < wayP.transform.childCount; i ++ )
        {
            Ways[i] = wayP.transform.GetChild(i).gameObject;
        }

        state = StateInfo.AttackMove;
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
                Target = transform.position;
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
        Vector3 offsetpos = transform.position;//포지션 0이 땅에 박혀있으니까 위로 올려줌
        offsetpos.y += 8.0f;

        int layerMask = (1 << LayerMask.NameToLayer("Glass"));//Everything에서 Glass 만 제외하고 충돌 체크함
        layerMask = ~layerMask;//레이어 마스크 제작

        if (Physics.Raycast(offsetpos, (Player.transform.position - offsetpos), out hit, MaxRayDistance, layerMask))//만약 hit한게 Glass 레이어가 아니라면
        {
            if (hit.transform.name == "Player_Camera")
            {
                Debug.DrawRay(offsetpos, (Player.transform.position - offsetpos) * hit.distance, Color.red, 5.0f);
                state = StateInfo.AttackMove;
                ChoiceTarget = true;
            }

            else if (OnMove == false)
                state = StateInfo.State;

            else
                state = StateInfo.Move;

            switch (state)
            {
                case StateInfo.State://대기상태일때
                    Debug.Log("State : State");
                    //duration을 더해주고 만일 대기시간이 지났을때 다른곳 순찰하러감
                    if (waitingDuration > MaxWaitingTime)
                    {
                        OnMove = true;
                        waitingDuration = 0;
                        state = StateInfo.Move;
                    }

                    if (agent.remainingDistance < VecOffSet)
                    {
                        PatrolMove = false;
                    }

                    if (PatrolMove == false)
                    {
                        float range_X = 60.0f;
                        float range_Z = 60.0f;

                        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
                        range_Z = Random.Range((range_Z / 2) * -1, range_Z / 2);
                        Vector3 PatrolPoint = new Vector3(Target.x + range_X, 0f, Target.z + range_Z);

                        NavMeshHit checkPatrolhit;
                        if (NavMesh.SamplePosition(PatrolPoint, out checkPatrolhit, 20f, NavMesh.AllAreas))
                        {
                            agent.destination = checkPatrolhit.position;
                            PatrolMove = true;
                        }
                        else
                        {
                            agent.destination = transform.position;
                        }
                    }
                    break;

                case StateInfo.Move:
                    Debug.Log("State : Move");

                    if (agent.remainingDistance < VecOffSet && ChoiceTarget == true)
                    {
                        OnMove = false;
                        ChoiceTarget = false;
                        state = StateInfo.State;
                    }
                    else if (ChoiceTarget == false)
                    {
                        Target = selectTarget();
                        ChoiceTarget = true;
                    }

                    agent.destination = Target;
                    break;

                case StateInfo.AttackMove:
                    Debug.Log("State : AttackMove");

                    //if(OnMove == true)
                    if (Vector3.Distance(transform.position,Player.transform.position) < attackRange &&//캐릭터가 공격범위 안에있으면서
                        hit.transform.name == "Player_Camera" &&//레이케스팅을 했을때 플레이어가 잡혀야되고
                        Manager.CM_Instance.OnHide == false)//숨은 상태가 아닐때만 공격함
                    {
                        agent.destination = transform.position;
                        OnMove = false;
                        anim.SetBool("Attack", true);
                        anim.SetBool("Move", false);
                    }
                    else
                    {
                        OnMove = true;
                        Target = Player.transform.position;
                        agent.destination = Player.transform.position;
                        //ChoiceTarget = false;
                        anim.SetBool("Attack", false);
                        anim.SetBool("Move", true);
                    }
                    break;

                default:
                    break;
            }
        }

        if (OnMove == false)
            waitingDuration += Time.deltaTime;
    }

    private void StartAttack()
    {
        Vector3 offsetpos = transform.position;//포지션 0이 땅에 박혀있으니까 위로 올려줌
        offsetpos.y += 16.0f;

        GameObject.Find("Player_Camera").transform.LookAt(offsetpos);
        Manager.CM_Instance.SetMoveState(false);
        Manager.CM_Instance.SetRotState(false);
    }

    private void EndAttack()
    {
        Manager.UIManager_Instance.UIPopup("Scene_UI/UI_Scene_GameOver");
    }

    private Vector3 selectTarget()
    {
        if(saveMove)
        {
            GameObject temp = Ways[Random.Range(0, Ways.Length - 1)];

            while (temp.transform.position == Target && temp.name != "C")
            {
                temp = Ways[Random.Range(0, Ways.Length - 1)];
            }
            return temp.transform.position;
        }
        else
        {
            GameObject temp = Ways[Random.Range(0, Ways.Length - 1)];

            while (temp.transform.position == Target)
            {
                temp = Ways[Random.Range(0, Ways.Length - 1)];
            }
            return temp.transform.position;
        }
    }
}
