using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class NPCF_AI_Ctrl : MonoBehaviour
{
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
    float Speed;

    enum StateInfo
    {
        State,
        AttackMove,
        Move
    }

    NavMeshAgent agent;
    RaycastHit hit;
    StateInfo state;//현재 상태

    //agent.destination 목표지점까지 이동
    GameObject Player;
    Vector3 TargetPos;

    bool OnMove = false;
    bool PatrolMove = false;

    bool FindKid = true;

    Animator anim;

    public void Start()
    {
        GameObject.Find("EventSystem").GetComponent<NPC_GaugeUI>().SetOnGauge(false);

        transform.GetComponent<NavMeshAgent>().speed = Speed;

        //Todo:탐지 거리에 대한 데이터 테이블 수정필요함
        MaxRayDistance = Manager.DataManager_Instance.GetBalanceValue(6);
        MaxWaitingTime = Manager.DataManager_Instance.GetBalanceValue(9);

        //Todo:공격 거리에 대한 데이터 테이블 수정필요함
        attackRange = Manager.DataManager_Instance.GetBalanceValue(11);
        attackRange = 20;

        VecOffSet = 10.0f;

        anim = GetComponent<Animator>();
        anim.SetBool("Attack", false);
        anim.SetBool("Move", true);

        agent = GetComponent<NavMeshAgent>();
        Player = GameObject.Find("Player_Camera").gameObject;//맨처음 플레이어의 게임오브젝트를 가져옴 / 후에 플레이어 추격에 쓰임
        TargetPos = Player.transform.position;
        //여기까진 셋팅

        agent.destination = TargetPos;
        OnMove = true;

        state = StateInfo.Move;
    }

    public void Update()
    {
        Vector3 offsetpos = transform.position;//포지션 0이 땅에 박혀있으니까 위로 올려줌
        offsetpos.y += 10.0f;

        int layerMask = (1 << LayerMask.NameToLayer("Glass"));//Everything에서 Glass 만 제외하고 충돌 체크함
        layerMask = ~layerMask;//레이어 마스크 제작
        //여기까진 셋팅

        if (Physics.Raycast(offsetpos, (Player.transform.position - offsetpos), out hit, MaxRayDistance, layerMask))//만약 hit한게 Glass 레이어가 아니라면
        {
            if (hit.transform.name == "Player_Camera")
            {
                state = StateInfo.AttackMove;
                TargetPos = Player.transform.position;
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
                        TargetPos = GameObject.Find("NPC_Spawn_Point").transform.position;
                        OnMove = true;
                        FindKid = false;
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
                        Vector3 PatrolPoint = new Vector3(TargetPos.x + range_X, 0f, TargetPos.z + range_Z);

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

                    if (agent.remainingDistance < VecOffSet)
                    {
                        if(Vector3.Distance(GameObject.Find("NPC_Spawn_Point").transform.position, transform.position) < 5.0f &&
                            FindKid == false)
                        {
                            GameObject.Find("EventSystem").GetComponent<NPC_GaugeUI>().SetOnGauge(true);
                            Destroy(gameObject);
                            return;
                        }
                        OnMove = false;
                        state = StateInfo.State;
                    }

                    agent.destination = TargetPos;
                    break;

                case StateInfo.AttackMove:
                    Debug.Log("State : AttackMove");
                    
                    if (Vector3.Distance(transform.position, Player.transform.position) < attackRange &&//NPC공격 범위 안에있고
                        hit.transform.name == "Player_Camera" &&//레이 맞은 대상이 플레이어 이며
                        Manager.CM_Instance.OnHide == false)//숨은 상태가 아닌상태여야만 공격을 함
                    {
                        OnMove = false;
                        anim.SetBool("Attack", true);
                        anim.SetBool("Move", false);

                        Manager.UIManager_Instance.UIPopup("Scene_UI/UI_Scene_GameOver");
                    }
                    else
                    {
                        agent.destination = Player.transform.position;//플레이어를 포착하고있는 상태라 그냥 떄리박음
                        TargetPos = Player.transform.position;
                        OnMove = true;
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
}
