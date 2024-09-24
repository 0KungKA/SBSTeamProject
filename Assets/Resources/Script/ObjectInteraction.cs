using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

[AddComponentMenu("devchanho/ObjectInteraction")]//������ ������Ʈ�� ������Ʈ�� �߰��ϱ����� AddComponent �޴��� �߰��ص�
public class ObjectInteraction : MonoBehaviour
{
    int interactionGaugeValue;

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

    [Header("�¿� ����(��� ��) / ������ �̵�(������) / ���ο���(������ ��)")]
    [SerializeField]
    bool Rot;
    [SerializeField]
    bool Move;
    [SerializeField]
    bool Open;
    [SerializeField]
    bool hide;

    [SerializeField]
    [Header("Pos Speed / Rot Speed")]
    Vector2 Speed = new Vector2(80.0f,80.0f);

    [SerializeField]
    AudioClip objectInteractionSount;
    AudioSource audioSorce;

    //���� ���尰�� ������ ���� ������ �Ҷ� ���� ������ �����ؼ� ��ũ��Ʈ�� �ۼ���
    Vector3 SavePos;//���� Position
    Vector3 SaveRot;//���� Rotation

    public bool thisMove = false;//�̰� �̹� ������ ������Ʈ�ΰ��� ���� bool ��
    bool OnMove = false;//�����̰� �ִ��� �ƴ��� Ȯ���ϴ� bool ��

    bool OnHide = false;//���� �������� �ƴ��� Ȯ���ϴ� bool��

    /*[Space(20)]
    //���� ���尰�� �������ִ� ������ ��ȣ�ۿ��Ұ�� �ش� ���� ���ʿ� ī�޶� �̵���Ű�� ���� Vector3������ ������ ����
    [SerializeField]
    Vector3 CameraPos;*/
    private void Start()
    {
        interactionGaugeValue = Manager.DataManager_Instance.GetBalanceValue(3);//������ ���̺��� ��ȣ�ۿ�� �󸶳� ������ ������

        //Todo:���� ��ġ �������� �����ٰ�
        interactionGaugeValue = 5;

        if (gameObject.GetComponent<MeshCollider>() == null)//Mesh collider�� ���°��
        {
            //Mesh Collider�� �߰��ؾ������� �Ǻ��� ��߳��� MT�� ���� ����� ��찡 ������ ����ó��
            //��ȣ�ۿ� ������ ������Ʈ�� ��� Mesh Fillter�� Mesh Renderer�� ������ �̰ɷ� ����ó�� ����
            if (gameObject.GetComponent<MeshFilter>() != null && gameObject.GetComponent<MeshRenderer>() != null)
            {
                //����ϸ� MeshCollider�� �߰�
                gameObject.AddComponent<MeshCollider>();
                gameObject.tag = "IObject";
            }
        }

        if(objectInteractionSount != null)
        {
            audioSorce = transform.GetComponent<AudioSource>();
            if(audioSorce == null)
            {
                audioSorce = transform.AddComponent<AudioSource>();
                //Todo:���� �������� ���߿� ����â�̶� �����Ҳ��� ������ ���� �ε��� �����̶� �װ� ȯ�漳���� �����ϰ� ȯ�漳���� �����;���
                audioSorce.volume = 1.0f;
            }

            audioSorce.loop = false;
            audioSorce.playOnAwake = false;
            audioSorce.clip = objectInteractionSount;
            audioSorce.maxDistance = 20.0f;
        }

        //�����̵� ������ ��¶�� �ٽ� �����·� �������ϱ⶧���� �ΰ��� �޾���
        SavePos = transform.localPosition;

        SaveRot.x = (transform.localEulerAngles.x >= 180) ? transform.localEulerAngles.x - 360 : transform.localEulerAngles.x;
        SaveRot.y = (transform.localEulerAngles.y >= 180) ? transform.localEulerAngles.y - 360 : transform.localEulerAngles.y;
        SaveRot.z = (transform.localEulerAngles.z >= 180) ? transform.localEulerAngles.z - 360 : transform.localEulerAngles.z;
    }

    //sendMessage�� ȣ���Ұ���
    void InteractionStart()
    {
        if (OnMove) return;//������Ʈ�� �̹� ��ȣ�ۿ��� �ؼ� �����̰��ִ� �����̸� ���ϹھƼ� �� �������� ����

        if(GameObject.Find("EventSystem").GetComponent<NPC_GaugeUI>().GetOnGauge())
            GameObject.Find("EventSystem").GetComponent<NPC_GaugeUI>().UpGauge(interactionGaugeValue);

        if (audioSorce != null)
        {
            audioSorce.Play();
        }
        
        if(Move)
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
        else if(Rot)
        {
            if (L) 
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
            else if (R)
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
        else if(Open)//�����Է� ���� �ݱ�
        {
            if (thisMove == false)
            {
                Debug.Log("Open");
                StartCoroutine(RotOpen(TargetRot));
            }
            else if (thisMove == true)
            {
                Debug.Log("Close");
                StartCoroutine(RotClose(SaveRot));
            }
        }
        else //ȿ������ ���
        {
            Manager.Effect_SoundPlayer.EffectSoundPlay(audioSorce);
        }
        
    }

    IEnumerator MovePos(Vector3 TargetPos)//����������Ʈ �͸����� ���ִ� �ڵ�
    {
        Vector3 Pos;
        while (true)
        {
            OnMove = true;

            if(Vector3.Distance(transform.localPosition,TargetPos) < 0.01f)
            {
                thisMove = !thisMove;
                OnMove = false;
                yield break;
            }

            if (Mathf.Lerp(transform.localPosition.x, TargetPos.x, 0.5f) < 0.01f)
            {
                Pos.x = TargetPos.x;
            }
            Pos.x = Mathf.Lerp(transform.localPosition.x, TargetPos.x, Speed.x);

            if (Mathf.Lerp(transform.localPosition.y, TargetPos.y, 0.5f) < 0.01f)
            {
                Pos.y = TargetPos.y;
            }
            Pos.y = Mathf.Lerp(transform.localPosition.y, TargetPos.y, Speed.x);

            if (Mathf.Lerp(transform.localPosition.z, TargetPos.z, 0.5f) < 0.01f)
            {
                Pos.z = TargetPos.z;
            }
            Pos.z = Mathf.Lerp(transform.localPosition.z, TargetPos.z, Speed.x);



            transform.localPosition = new Vector3(Pos.x, Pos.y, Pos.z);

            yield return null;
        }
    }

    IEnumerator RotL(Vector3 TargetRot)//Y��(Vector3.up)���� �������� �����ִ� �ڵ�
    {
        Debug.Log("Start Coroutine RotL");
        while (true)
        {
            OnMove = true;

            Vector3 Rot = transform.localEulerAngles;
            float rotY = transform.localEulerAngles.y % 360;

            if (thisMove == false)
            {
                if (Rot.y >= 180)
                    rotY = rotY - 360;

                rotY = (rotY > 180) ? rotY - 360 : rotY;

                if (rotY < TargetRot.y)
                {
                    thisMove = !thisMove;
                    OnMove = false;
                    Debug.Log("End Coroutine RotL S");
                    yield break;
                }

                transform.Rotate(new Vector3(0, -Speed.y * Time.deltaTime, 0));
            }
            else if (thisMove == true)
            {
                if (Rot.y >= 180)
                    rotY = rotY - 360;

                rotY = (rotY > 180) ? rotY - 360 : rotY;

                if (rotY < SaveRot.y)
                {
                    thisMove = !thisMove;
                    OnMove = false;
                    Debug.Log("End Coroutine RotL F");
                    yield break;
                }
                transform.Rotate(new Vector3(0, -Speed.y * Time.deltaTime, 0));
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

            Vector3 Rot = transform.localEulerAngles;
            float rotY = transform.localEulerAngles.y % 360;

            if (thisMove == false)
            {
                if (Rot.y >= 180)
                    rotY = (rotY * -1) + 180;

                rotY = (rotY > 180) ? rotY - 360 : rotY;

                if (rotY > TargetRot.y)
                {
                    thisMove = !thisMove;
                    OnMove = false;
                    Debug.Log("End Coroutine RotR F");
                    yield break;
                }
                transform.Rotate(new Vector3(0, +Speed.y * Time.deltaTime , 0)); 
            }
            else if(thisMove == true)
            {
                if (Rot.y >= 180)
                    rotY = rotY - 360;

                rotY = (rotY > 180) ? rotY - 360 : rotY;

                if (rotY > SaveRot.y)
                {
                    thisMove = !thisMove;
                    OnMove = false;
                    Debug.Log("End Coroutine RotR S");
                    yield break;
                }
                transform.Rotate(new Vector3(0, +Speed.y * Time.deltaTime, 0));
            }
            else
            {
                Debug.Log(transform.name + "Object Interaction Error");
            }

            yield return null;
        }
    }

    IEnumerator RotOpen(Vector3 TargetRot)//X�� �������� ���� �����ִ� �ڵ�
    {
        while (true)
        {
            OnMove = true;
            Vector3 Rot = transform.localEulerAngles;
            float rotX = transform.localEulerAngles.x % 360;

            if (Rot.y >= 180)
                rotX = (rotX * -1) + 180;

            rotX = (rotX > 180) ? rotX - 360 : rotX;

            if(rotX < TargetRot.x)
            {
                thisMove = true;
                OnMove = false;
                yield break;
            }

            transform.Rotate(-(Speed.y * Time.deltaTime), 0, 0);
            yield return null;
        }
    }

    IEnumerator RotClose(Vector3 TargetRot)//X�� �������� ���� �����ִ� �ڵ�
    {
        while (true)
        {
            OnMove = true;
            Vector3 Rot = transform.localEulerAngles;
            float rotX = transform.localEulerAngles.x % 360;

            if (Rot.y >= 180)
                rotX = (rotX * -1) + 180;

            rotX = (rotX > 180) ? rotX - 360 : rotX;

            if (rotX > SaveRot.x)
            {
                thisMove = false;
                OnMove = false;
                yield break;
            }

            transform.Rotate(Speed.y * Time.deltaTime, 0, 0);
            yield return null;
        }
    }

    IEnumerator Hide()
    {
        GameObject gm = GameObject.Find("Player_Camera").gameObject;
        SavePos = gm.transform.position;
        bool goSavePos = false;

        while (true)
        {
            if (OnHide == false)
            {
                Manager.CM_Instance.SetMoveState(false);

                gm.transform.position = new Vector3(Mathf.Lerp(gm.transform.position.x, transform.position.x, 0.5f),
                    Mathf.Lerp(gm.transform.position.y, transform.position.y, 0.5f),
                    Mathf.Lerp(gm.transform.position.z, transform.position.z, 0.5f));

                gm.transform.rotation = transform.rotation;

                if (Mathf.Abs(Vector3.Distance(gm.transform.position, transform.position)) <= 0.1f)
                {
                    gm.transform.Rotate(new Vector3(0, 90, 0));
                    OnHide = true;
                    Manager.CM_Instance.OnHide = OnHide;
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
                        Manager.CM_Instance.OnHide = OnHide;
                        yield break;
                    }
                }
                yield return null;
            }
        }
    }
}
