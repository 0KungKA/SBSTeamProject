using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class NPCF_AI_Ctrl : MonoBehaviour
{
    [SerializeField]
    [Tooltip("NPC�� �÷��̾ �ν��Ҽ��ִ� �ִ� �Ÿ�")]
    int MaxRayDistance = 30;//NPC�� �÷��̾ �ν��Ҽ��ִ� �ִ� �Ÿ�

    [SerializeField]
    [Tooltip("�� �������� �����ϴ� �ð� = ������ ���̺� = 60��")]
    float MaxWaitingTime = 60;//�� �������� �����ϴ� �ð� 60��

    [SerializeField]
    [Tooltip("���� ������ �󸶳� �Ͽ������� ���� �ð�")]
    float waitingDuration = 0;//���� ������ �󸶳� �Ͽ������� ���� �ð�

    [SerializeField]
    [Tooltip("NPC�� �÷��̾ �ν��Ҽ��ִ� �ִ� �Ÿ�")]
    float attackRange = 10.0f;

    [SerializeField]
    [Tooltip("���� �������� ���� �󸶳� ������������")]
    float VecOffSet = 3.0f;

    [SerializeField]
    [Tooltip("NPC �̵��ӵ�")]
    float Speed;

    enum StateInfo
    {
        State,
        AttackMove,
        Move
    }

    NavMeshAgent agent;
    RaycastHit hit;
    StateInfo state;//���� ����

    //agent.destination ��ǥ�������� �̵�
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

        //Todo:Ž�� �Ÿ��� ���� ������ ���̺� �����ʿ���
        MaxRayDistance = Manager.DataManager_Instance.GetBalanceValue(6);
        MaxWaitingTime = Manager.DataManager_Instance.GetBalanceValue(9);

        //Todo:���� �Ÿ��� ���� ������ ���̺� �����ʿ���
        attackRange = Manager.DataManager_Instance.GetBalanceValue(11);
        attackRange = 20;

        VecOffSet = 10.0f;

        anim = GetComponent<Animator>();
        anim.SetBool("Attack", false);
        anim.SetBool("Move", true);

        agent = GetComponent<NavMeshAgent>();
        Player = GameObject.Find("Player_Camera").gameObject;//��ó�� �÷��̾��� ���ӿ�����Ʈ�� ������ / �Ŀ� �÷��̾� �߰ݿ� ����
        TargetPos = Player.transform.position;
        //������� ����

        agent.destination = TargetPos;
        OnMove = true;

        state = StateInfo.Move;
    }

    public void Update()
    {
        Vector3 offsetpos = transform.position;//������ 0�� ���� ���������ϱ� ���� �÷���
        offsetpos.y += 10.0f;

        int layerMask = (1 << LayerMask.NameToLayer("Glass"));//Everything���� Glass �� �����ϰ� �浹 üũ��
        layerMask = ~layerMask;//���̾� ����ũ ����
        //������� ����

        if (Physics.Raycast(offsetpos, (Player.transform.position - offsetpos), out hit, MaxRayDistance, layerMask))//���� hit�Ѱ� Glass ���̾ �ƴ϶��
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
                case StateInfo.State://�������϶�
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
                    
                    if (Vector3.Distance(transform.position, Player.transform.position) < attackRange &&//NPC���� ���� �ȿ��ְ�
                        hit.transform.name == "Player_Camera" &&//���� ���� ����� �÷��̾� �̸�
                        Manager.CM_Instance.OnHide == false)//���� ���°� �ƴѻ��¿��߸� ������ ��
                    {
                        OnMove = false;
                        anim.SetBool("Attack", true);
                        anim.SetBool("Move", false);

                        Manager.UIManager_Instance.UIPopup("Scene_UI/UI_Scene_GameOver");
                    }
                    else
                    {
                        agent.destination = Player.transform.position;//�÷��̾ �����ϰ��ִ� ���¶� �׳� ��������
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
        Vector3 offsetpos = transform.position;//������ 0�� ���� ���������ϱ� ���� �÷���
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
