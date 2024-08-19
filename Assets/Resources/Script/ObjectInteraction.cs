using System;
using System.Collections;
using System.Collections.Generic;
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

    bool OnHide = false;//���� �������� �ƴ��� Ȯ���ϴ� bool��

    /*[Space(20)]
    //���� ���尰�� �������ִ� ������ ��ȣ�ۿ��Ұ�� �ش� ���� ���ʿ� ī�޶� �̵���Ű�� ���� Vector3������ ������ ����
    [SerializeField]
    Vector3 CameraPos;*/

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
        {
            if(transform.parent != null)
                Debug.Log(transform.parent.name + " " + gameObject.name + " : ������ ����");

            else if (transform.parent != null)
                Debug.Log(gameObject.name + " : ������ ����");
        }

        //�����̵� ������ ��¶�� �ٽ� �����·� �������ϱ⶧���� �ΰ��� �޾���
        SavePos = transform.localPosition;

        SaveRot.x = transform.rotation.x;

        if (transform.rotation.y >= 180)
            SaveRot.y = 0;
        else
            SaveRot.y = transform.rotation.y;

        SaveRot.z = transform.rotation.z;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if(transform.name == "JewelCaseDoor_Pivit" && thisMove == false)
            {
                StartCoroutine("RotOpen", TargetRot);
            }
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

    Stack<float> temp1Log = new Stack<float>();
    Stack<float> temp2Log = new Stack<float>();
    IEnumerator RotOpen(Vector3 TargetRot)//X�� �������� ���� �����ִ� �ڵ�
    {
        while (true)
        {
            thisMove = true;

            float rotX = transform.localEulerAngles.x;
            rotX = (rotX >= 180) ? rotX - 360 : rotX;

            if (rotX <= TargetRot.x)
            {
                OnMove = true;
                yield break;
            }

            //rotX = Speed.y * Time.deltaTime;

            transform.Rotate(new Vector3(-10 * Time.deltaTime, 0, 0));

            yield return null;
            
        }
    }

    IEnumerator Hide()
    {
        Manager.CM_Instance.SetMoveState(false);
        GameObject gm = GameObject.FindWithTag("MainCamera").gameObject;
        SavePos = gm.transform.position;
        bool goSavePos = false;
        while (true)
        {
            if (OnHide == false)
            {
                gm.transform.position = new Vector3(Mathf.Lerp(gm.transform.position.x, transform.position.x, 0.5f),
                    Mathf.Lerp(gm.transform.position.y, transform.position.y, 0.5f),
                    Mathf.Lerp(gm.transform.position.z, transform.position.z, 0.5f));

                gm.transform.rotation = transform.rotation;

                if (Mathf.Abs(Vector3.Distance(gm.transform.position, transform.position)) <= 0.1f)
                {
                    gm.transform.Rotate(new Vector3(0, 90, 0));
                    OnHide = true;
                }
                yield return null;
            }
            else if (OnHide == true) 
            {
                if(Input.GetKeyDown(KeyCode.E))
                    goSavePos = true;

                if(goSavePos)
                {
                    gm.transform.position = new Vector3(Mathf.Lerp(gm.transform.position.x, SavePos.x, 0.5f),
                    Mathf.Lerp(gm.transform.position.y, SavePos.y, 0.5f),
                    Mathf.Lerp(gm.transform.position.z, SavePos.z, 0.5f));

                    if (Mathf.Abs(Vector3.Distance(gm.transform.position, SavePos)) <= 0.1f)
                    {
                        Manager.CM_Instance.SetMoveState(true);
                        OnHide = false;
                        yield break;
                    }
                }
                yield return null;
            }
        }
    }
}
