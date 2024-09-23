using System.ComponentModel;
using System.Runtime.CompilerServices;
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
    float Speed = 3.0f;

    NavMeshAgent agent;

    GameObject[] Ways;
    GameObject[] FWays;

    RaycastHit hit;

    StateInfo state;//���� ����

    //agent.destination ��ǥ�������� �̵�
    GameObject Player;
    GameObject Target;
    
    bool ChoiceTarget = false;//Ÿ���� �����Ҷ� �б������� ��
    bool OnMove = false;

    Vector3 YameOriginVector;

    Animator anim;

    public void Start()
    {
        MaxRayDistance = Manager.DataManager_Instance.GetBalanceValue(6);
        MaxWaitingTime = Manager.DataManager_Instance.GetBalanceValue(10);

        //Todo:���� �Ÿ��� ���� ������ ���̺� �����ʿ���
        attackRange = Manager.DataManager_Instance.GetBalanceValue(11);
        attackRange = 10.0f;
        VecOffSet = 3.0f;

        anim = GetComponent<Animator>();
        anim.SetBool("Attack", false);
        anim.SetBool("Move", true);

        YameOriginVector = transform.position;

        agent = GetComponent<NavMeshAgent>();
        Player = GameObject.FindWithTag("MainCamera").gameObject;//��ó�� �÷��̾��� ���ӿ�����Ʈ�� ������ / �Ŀ� �÷��̾� �߰ݿ� ����
        //��������Ʈ���� ���ӿ�����Ʈ�� ���� ������
        GameObject wayP = GameObject.Find("WayPoint").gameObject;
        Ways = new GameObject[wayP.transform.childCount];
        for (int i = 0; i < wayP.transform.childCount; i ++ )
        {
            Ways[i] = wayP.transform.GetChild(i).gameObject;
        }

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
    /// true = �������() / false = ���� ����
    /// </summary>
    /// <param name="value"></param>
    public void SetSaveState(bool value)
    {
        saveMove = value;
    }

    public void Update()
    {
        Vector3 offsetpos = transform.position;
        offsetpos.y += 8.0f;

        if (Physics.Raycast(offsetpos, (Player.transform.position - offsetpos), out hit, MaxRayDistance))
        {
            Debug.Log(hit.transform.name);
            Debug.DrawLine(offsetpos, (Player.transform.position - offsetpos) * MaxRayDistance, Color.blue, 0.1f);

            if (hit.transform.tag == "MainCamera")
            {
                state = StateInfo.AttackMove;
            }
            else if(OnMove == false)
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
            case StateInfo.State://�������϶�
                Debug.Log("State : State");
                waitingDuration += Time.deltaTime;//duration�� �����ְ� ���� ���ð��� �������� �ٸ��� �����Ϸ���
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
                    Target = selectTarget();
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

                agent.destination = Player.transform.position;

                if (agent.remainingDistance < attackRange)
                {
                    anim.SetBool("Attack", true);
                    anim.SetBool("Move", false);

                    Debug.Log("���� Attack�κ� ui����ó���� ���߿� ���ֱ�");
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
