using System.ComponentModel;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
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

    public bool saveMove = true;//C�� D���� �ȵ����� �������ִ� bool��

    [SerializeField]
    [Tooltip("NPC�� �÷��̾ �ν��Ҽ��ִ� �ִ� �Ÿ�")]
    int MaxRayDistance = 30;//NPC�� �÷��̾ �ν��Ҽ��ִ� �ִ� �Ÿ�

    [SerializeField]
    [Tooltip("�� �������� �����ϴ� �ð� = ������ ���̺� = 60��")]
    float MaxWaitingTime = 60;//�� �������� �����ϴ� �ð� 60��

    [SerializeField]
    [ReadOnly(true)]
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
    float Speed = 5.0f;

    NavMeshAgent agent;

    GameObject[] Ways;

    RaycastHit hit;

    StateInfo state;//���� ����

    //agent.destination ��ǥ�������� �̵�
    GameObject Player;
    GameObject Target;
    
    bool ChoiceTarget = false;//Ÿ���� �����Ҷ� �б������� ��
    bool OnMove = false;

    Animator anim;

    public void Start()
    {
        //Todo:Ž�� �Ÿ��� ���� ������ ���̺� �����ʿ���
        MaxRayDistance = Manager.DataManager_Instance.GetBalanceValue(6);
        MaxRayDistance = 45;

        MaxWaitingTime = Manager.DataManager_Instance.GetBalanceValue(10);

        //Todo:���� �Ÿ��� ���� ������ ���̺� �����ʿ���
        attackRange = Manager.DataManager_Instance.GetBalanceValue(11);
        attackRange = 10.0f;

        VecOffSet = 10.0f;

        anim = GetComponent<Animator>();
        anim.SetBool("Attack", false);
        anim.SetBool("Move", true);

        agent = GetComponent<NavMeshAgent>();
        Player = GameObject.Find("Player_Camera").gameObject;//��ó�� �÷��̾��� ���ӿ�����Ʈ�� ������ / �Ŀ� �÷��̾� �߰ݿ� ����

        //��������Ʈ���� ���ӿ�����Ʈ�� ���� ������
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
    /// true = �������() / false = ���� ����
    /// </summary>
    /// <param name="value"></param>
    public void SetSaveState(bool value)
    {
        saveMove = value;
    }

    public void Update()
    {
        Vector3 offsetpos = transform.position;//������ 0�� ���� ���������ϱ� ���� �÷���
        offsetpos.y += 8.0f;

        int layerMask = (1 << LayerMask.NameToLayer("Glass"));//Everything���� Glass �� �����ϰ� �浹 üũ��
        layerMask = ~layerMask;//���̾� ����ũ ����

        if(OnMove == false)
            waitingDuration += Time.deltaTime;

        if (Physics.Raycast(offsetpos, (Player.transform.position - offsetpos), out hit, MaxRayDistance, layerMask))//���� hit�Ѱ� Glass ���̾ �ƴ϶��
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
                case StateInfo.State://�������϶�
                    Debug.Log("State : State");
                    //duration�� �����ְ� ���� ���ð��� �������� �ٸ��� �����Ϸ���
                    if (waitingDuration > MaxWaitingTime)
                    {
                        OnMove = true;
                        waitingDuration = 0;
                        state = StateInfo.Move;
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
                    agent.destination = Target.transform.position;

                    break;

                case StateInfo.AttackMove:
                    Debug.Log("State : AttackMove");

                    agent.destination = Player.transform.position;
                    if (Vector3.Distance(transform.position,Player.transform.position) < attackRange && hit.transform.name == "Player_Camera")
                    {
                        OnMove = false;
                        anim.SetBool("Attack", true);
                        anim.SetBool("Move", false);

                        if(Manager.CM_Instance.GetDebugFalse())
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

    private GameObject selectTarget()
    {
        if(saveMove)
        {
            GameObject temp = Ways[Random.Range(0, Ways.Length - 1)];

            while (temp == Target && temp.name != "C")
            {
                temp = Ways[Random.Range(0, Ways.Length - 1)];
            }
            return temp;
        }
        else
        {
            GameObject temp = Ways[Random.Range(0, Ways.Length - 1)];

            while (temp == Target)
            {
                temp = Ways[Random.Range(0, Ways.Length - 1)];
            }
            return temp;
        }


    }
}
