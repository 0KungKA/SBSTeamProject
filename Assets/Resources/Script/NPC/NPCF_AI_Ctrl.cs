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
    float Speed = 5.0f;

    NavMeshAgent agent;

    RaycastHit hit;

    StateInfo state;//현재 상태

    //agent.destination 목표지점까지 이동
    GameObject Player;
    GameObject Target;

    bool ChoiceTarget = false;//타겟을 지정할때 분기점으로 씀
    bool OnMove = false;

    Animator anim;

    public void Start()
    {
        GameObject.Find("EventSystem").GetComponent<NPC_GaugeUI>().SetOnGauge(false);

        //Todo:탐지 거리에 대한 데이터 테이블 수정필요함
        MaxRayDistance = Manager.DataManager_Instance.GetBalanceValue(6);
        MaxRayDistance = 45;

        MaxWaitingTime = Manager.DataManager_Instance.GetBalanceValue(9);

        //Todo:공격 거리에 대한 데이터 테이블 수정필요함
        attackRange = Manager.DataManager_Instance.GetBalanceValue(11);
        attackRange = 10.0f;

        VecOffSet = 10.0f;

        anim = GetComponent<Animator>();
        anim.SetBool("Attack", false);
        anim.SetBool("Move", true);


        agent = GetComponent<NavMeshAgent>();
        Player = GameObject.Find("Player_Camera").gameObject;//맨처음 플레이어의 게임오브젝트를 가져옴 / 후에 플레이어 추격에 쓰임

        agent.destination = Player.transform.position;

        state = StateInfo.AttackMove;
    }

    public void Update()
    {
        Vector3 offsetpos = transform.position;//포지션 0이 땅에 박혀있으니까 위로 올려줌
        offsetpos.y += 8.0f;

        int layerMask = (1 << LayerMask.NameToLayer("Glass"));//Everything에서 Glass 만 제외하고 충돌 체크함
        layerMask = ~layerMask;//레이어 마스크 제작

        if (OnMove == false)
            waitingDuration += Time.deltaTime;

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
                    if (waitingDuration > MaxWaitingTime)
                    {
                        OnMove = true;
                        waitingDuration = 0;
                        Destroy(gameObject);
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

                    agent.destination = Target.transform.position;
                    break;

                case StateInfo.AttackMove:
                    Debug.Log("State : AttackMove");

                    agent.destination = Player.transform.position;
                    if (Vector3.Distance(transform.position, Player.transform.position) < attackRange && hit.transform.name == "Player_Camera")
                    {
                        OnMove = false;
                        anim.SetBool("Attack", true);
                        anim.SetBool("Move", false);

                        if (Manager.CM_Instance.GetDebugFalse())
                            Manager.UIManager_Instance.UIPopup("Scene_UI/UI_Scene_GameOver");
                    }
                    else
                    {
                        OnMove = true;
                        ChoiceTarget = false;
                        anim.SetBool("Attack", false);
                        anim.SetBool("Move", true);
                    }
                    break;

                default:
                    break;
            }

        }
    }

    private void OnDisable()
    {
        GameObject.Find("EvenySystem").GetComponent<NPC_GaugeUI>().SetOnGauge(true);
    }
}
