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
    Vector3 TargetPos;
    
    bool ChoiceTarget = false;

    Vector3 YameOriginVector;

    public void Start()
    {
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
            case StateInfo.State://�������϶�
                Debug.Log("State : State");
                waitingDuration += Time.deltaTime;//duration�� �����ְ� ���� ���ð��� �������� �ٸ��� �����Ϸ���
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
