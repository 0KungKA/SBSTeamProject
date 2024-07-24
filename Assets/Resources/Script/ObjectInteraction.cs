using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[AddComponentMenu("devchanho/ObjectInteraction")]//������ ������Ʈ�� ������Ʈ�� �߰��ϱ����� AddComponent �޴��� �߰��ص�
public class ObjectInteraction : MonoBehaviour
{
    [Header("���� : �������������� �о����")]
    [SerializeField]
    [Header("������Ʈ�� ��ȣ�ۿ��Ҷ� ��ǥ�� ���� ������")]
    Vector3 TargetPos;

    [Space(20)]

    [SerializeField]
    [Header("������Ʈ�� ��ȣ�ۿ��Ҷ� ��ǥ�� ���� �����̼�")]
    Vector3 TargetRot;

    [Header("������ ������Ʈ�� ������ �������� üũ\n�� : �ش������Ʈ�������� ��ó�� ����������� �������� üũ")]
    [SerializeField]
    bool L;
    [SerializeField]
    bool R;

    [SerializeField]
    [Header("Pos Speed / Rot Speed")]
    Vector2 Speed = new Vector2(300.0f,80.0f);

    [SerializeField]
    AudioClip objectInteractionSount;
    AudioSource audioSorce;


    //���� ���尰�� ������ ���� ������ �Ҷ� ���� ������ �����ؼ� ��ũ��Ʈ�� �ۼ���

    Vector3 SavePos;//���� Position
    Vector3 SaveRot;//���� Rotation

    bool thisMove = false;//�̰� �̹� ������ ������Ʈ�ΰ��� ���� bool ��
    bool OnMove = false;//�����̰� �ִ��� �ƴ��� Ȯ���ϴ� bool ��

    [Space(20)]
    //���� ���尰�� �������ִ� ������ ��ȣ�ۿ��Ұ�� �ش� ���� ���ʿ� ī�޶� �̵���Ű�� ���� Vector3������ ������ ����
    [SerializeField]
    Vector3 CameraPos;

    private void Start()
    {
        if (gameObject.GetComponent<MeshCollider>() == null)//Mesh collider�� ���°��
        {
            //Mesh Collider�� �߰��ؾ������� �Ǻ��� ��߳��� MT�� ���� ����� ��찡 ������ ����ó��
            //��ȣ�ۿ� ������ ������Ʈ�� ��� Mesh Fillter�� Mesh Renderer�� ������ �̰ɷ� ����ó�� ����
            if (gameObject.GetComponent<MeshFilter>() != null && gameObject.GetComponent<MeshRenderer>() != null)
            {
                //����ϸ� MeshCollider�� �߰�
                gameObject.AddComponent<MeshCollider>();
                gameObject.tag = "IObject";
                Debug.Log(gameObject.name + " : Warring");
            }
        }

        if(objectInteractionSount != null)
        {
            audioSorce = transform.GetComponent<AudioSource>();
            if(audioSorce == null)
                audioSorce = transform.AddComponent<AudioSource>();

            audioSorce.playOnAwake = false;
            audioSorce.maxDistance = 10.0f;
            audioSorce.clip = objectInteractionSount;
        }

        if(TargetPos ==  Vector3.zero && TargetRot == Vector3.zero)
            Debug.Log(gameObject.name + " : ������ ����");

        //�����̵� ������ ��¶�� �ٽ� �����·� �������ϱ⶧���� �ΰ��� �޾���
        SavePos = transform.localPosition;

        SaveRot.x = transform.rotation.x;

        if (transform.rotation.y >= 180)
            SaveRot.y = 0;
        else
            SaveRot.y = transform.rotation.y;

        SaveRot.z = transform.rotation.z;
    }

    private void Update()
    {
        if(transform.name == "123123")
        {
            /*Debug.Log("Transform Rotation : " + transform.rotation);
            Debug.Log("Transform LocalRotation : " + transform.localRotation);
            Debug.Log("Transform EulerAngles : " + transform.rotation.eulerAngles);
            Debug.Log("Quaternion Euler (euler angles) : " + Quaternion.Euler(transform.rotation.eulerAngles));
            Debug.Log("SaveRot : " + SaveRot);*/
        }
    }

    //sendMessage�� ȣ���Ұ���
    void InteractionStart()
    {
        if (OnMove) return;//������Ʈ�� �̹� ��ȣ�ۿ��� �ؼ� �����̰��ִ� �����̸� ���ϹھƼ� �� �������� ����

        if(audioSorce != null)
        {
            audioSorce.Play();
        }
        
        if(L != true && R != true)
        {
            if (thisMove == false)
            {
                StartCoroutine(MovePos(TargetPos));
            }
            else if (thisMove == true)
            {
                StartCoroutine(MovePos(SavePos));
            }
        }
        else if(L)
        {
            if (thisMove == false)
            {
                StartCoroutine(RotL(TargetRot));//������ ���ʿ� �����ϱ� �������� RotL �������� RotR
            }
            else if (thisMove == true)
            {
                StartCoroutine(RotR(SaveRot));
            }
            
        }
        else if(R)
        {
            if (thisMove == false)//������ �����ʿ� �����ϱ� �������� RotR �������� RotL
            {
                StartCoroutine(RotR(TargetRot));
            }
            else if (thisMove == true)
            {
                StartCoroutine(RotL(SaveRot));
            }
        }

    }

    IEnumerator MovePos(Vector3 TargetPos)//����������Ʈ �͸����� ���ִ� �ڵ�
    {
        while(true)
        {
            OnMove = true;
            if (transform.localPosition == TargetPos)
            {
                thisMove = !thisMove;
                OnMove = false;
                yield break;
            }
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, TargetPos, Speed.x * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator RotL(Vector3 TargetRot)//Y��(Vector3.up)���� �������� �����ִ� �ڵ�
    {
        Debug.Log("Start Coroutine RotL");
        while (true)
        {
            OnMove = true;
            Quaternion q1 = Quaternion.Euler(TargetRot);

            if (thisMove == true)
            {
                if (transform.rotation.y < SaveRot.y)
                {
                    thisMove = !thisMove;
                    OnMove = false;
                    Debug.Log("End Coroutine RotL F");
                    yield break;
                }
                transform.Rotate(new Vector3(q1.x, -Speed.y * Time.deltaTime, q1.z));
            }
            else if (thisMove == false)
            {
                if (transform.localRotation.y < q1.y)
                {
                    thisMove = !thisMove;
                    OnMove = false;
                    Debug.Log("End Coroutine RotL S");
                    yield break;
                }

                transform.Rotate(new Vector3(q1.x, -Speed.y * Time.deltaTime, q1.z));
            }

            else
            {
                Debug.Log(transform.name + "Object Interaction Error");
            }
            yield return null;
        }
    }

    IEnumerator RotR(Vector3 TargetRot)//Y��(Vector3.up)���� ���������� �����ִ� �ڵ�
    {
        Debug.Log("Start Coroutine RotR");
        while (true)
        {
            OnMove = true;
            Quaternion q1 = Quaternion.Euler(TargetRot);

            

            if (thisMove == false)
            {

                if (transform.localRotation.y > q1.y)
                {
                    thisMove = !thisMove;
                    OnMove = false;
                    Debug.Log("End Coroutine RotR F");
                    yield break;
                }
                transform.Rotate(new Vector3(q1.x, +Speed.y * Time.deltaTime , q1.z)); 
            }
            else if(thisMove == true)
            {
                if (transform.rotation.y > SaveRot.y)
                {
                    thisMove = !thisMove;
                    OnMove = false;
                    Debug.Log("End Coroutine RotR S");
                    yield break;
                }
                transform.Rotate(new Vector3(q1.x, +Speed.y * Time.deltaTime, q1.z));
            }
            else
            {
                Debug.Log(transform.name + "Object Interaction Error");
            }

            yield return null;
        }
    }
}
