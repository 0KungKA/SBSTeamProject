using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    float Hight = 8f;//ī�޶� ����

    [SerializeField]
    int MouseSensitivity = 100;//���콺 ����
    public void SetSensitivity(int value) { MouseSensitivity = value; }

    float MouseX;
    float MouseY;

    [SerializeField]
    float MaxAngle = 40;
    [SerializeField]
    float MinAngle = -40;

    float Horizontal;
    float Vertical;
    Vector3 Movement;

    //�׽�Ʈ ����
    float OD;

    Rigidbody rb;

    [SerializeField]
    float Speed = 1.0f;

    protected internal void Init()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse))
        {
            ObjInteraction();
        }
        else
        {
            RaycastHit hit;
            float MaxDistance = 10.0f;
            if (Physics.Raycast(transform.position, transform.forward, out hit, MaxDistance))
            {
                if (hit.transform.tag == "Hide")
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        hit.transform.GetComponent<ObjectInteraction>().StartCoroutine("Hide");
                    }
                }
            }
        }
    }

    private void ObjInteraction()
    {
        RaycastHit hit;
        float MaxDistance = 10.0f;

        if(Physics.Raycast(transform.position, transform.forward, out hit, MaxDistance))
        {
            if(hit.transform.GetComponent<Mission>() != null)
            {
                hit.transform.GetComponent<Mission>().SendMSG();
            }
            else if(hit.transform.tag == "IObject")//��ȣ�ۿ� ������ ���� ������Ʈ
            {
                Debug.Log("Ray : IObject");
                //SendMessage�� �Լ��� ȣ�����ְ� ��ȯ�� �ʿ�����ϱ� ���ϴ¿ɼ��� �־���
                hit.transform.GetComponent<ObjectInteraction>().SendMessage("InteractionStart", SendMessageOptions.DontRequireReceiver);

                if(hit.transform.GetComponent<SoundPlayer>() != null)
                    hit.transform.GetComponent<SoundPlayer>().SendMessage("PlaySound",SendMessageOptions.DontRequireReceiver);
            }
            else if(hit.transform.tag == "GetItem")//
            {
                Debug.Log("Ray : GetItem");
                if(hit.transform.GetComponent<ItemInteraction>() != null)
                {
                    hit.transform.GetComponent<ItemInteraction>().SendMessage("ItemUISpawn", SendMessageOptions.DontRequireReceiver);
                }
            }
            else if(hit.transform.tag == "ITObject")//����X ����X ������ 3DIObjectView�� ��������ϴ� ������Ʈ��
            {
                Debug.Log("Ray : ITObject");
                hit.transform.GetComponent<ItemInteraction>().SendMessage
                    ("ObjectUISpawn",hit.transform.gameObject, SendMessageOptions.DontRequireReceiver);
            }
            if (hit.transform.tag == "CutScene")
            {
                GetComponent<PlayableDirector>().Play();
            }

        }
    }

    protected internal void Move()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");

        Movement = transform.forward * Vertical;
        Movement += transform.right * Horizontal;
        Movement.y = 0;
        GetComponent<CharacterController>().Move(Movement.normalized * Speed);
        transform.position = new Vector3(transform.position.x, Hight, transform.position.z);
    }

    protected internal void CameraRot()
    {
        //���콺 XY�� ������ �ڵ�
        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");

        //������ �������ָ鼭 ������ �־��ִ� �ڵ�
        transform.Rotate(-MouseY * MouseSensitivity * 10 * Time.deltaTime, 0f, 0);//���콺 Y������ ���� ����
        transform.Rotate(0f, MouseX * MouseSensitivity * 10 * Time.deltaTime, 0, Space.World);//���콺 X������ ���� ����

        //Ư�� �����̻��� �Ǹ� ������ �̻��� �������� �����Ǵ� ������ �ذ��ϱ� ���� �ڵ�
        //��Ȯ�ϰ� eulerAngles �� 0 ~ 180�� ���� ��ȯ���ִ°Ŷ� 180�� �̻��� �Ǹ� ������ �ɷ��� 360�� ���ָ鼭 clamp�� ������ �ɾ��
        Vector3 rot = transform.rotation.eulerAngles;
        rot.x = (rot.x > 180) ? rot.x - 360 : rot.x;
        rot.x = Mathf.Clamp(rot.x, MinAngle, MaxAngle);

        transform.rotation = Quaternion.Euler(rot.x, rot.y, 0);
    }

    protected internal void DebugMode()
    {
        //������ ��ȯ
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");
        OD = Input.GetAxisRaw("UpDown");

        if(Input.GetKeyDown(KeyCode.Equals))
        {
            Speed += 0.1f;
        }
        else if (Input.GetKeyDown(KeyCode.Minus))
        {
            Speed -= 0.1f;

            if(Speed < 0)
            {
                Speed = 1;
            }
        }


        Vector3 pos = Vector3.zero;

        pos.x = Horizontal;
        pos.y = OD;
        pos.z = Vertical;

        transform.Translate(pos * Speed ,Space.Self);

        //������ȯ

        //���콺 XY�� ������ �ڵ�
        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");

        //������ �������ָ鼭 ������ �־��ִ� �ڵ�
        transform.Rotate(-MouseY * MouseSensitivity * 10 * Time.deltaTime, 0f, 0);//���콺 Y������ ���� ����
        transform.Rotate(0f, MouseX * MouseSensitivity * 10 * Time.deltaTime, 0, Space.World);//���콺 X������ ���� ����

        //Ư�� �����̻��� �Ǹ� ������ �̻��� �������� �����Ǵ� ������ �ذ��ϱ� ���� �ڵ�
        //��Ȯ�ϰ� eulerAngles �� 0 ~ 180�� ���� ��ȯ���ִ°Ŷ� 180�� �̻��� �Ǹ� ������ �ɷ��� 360�� ���ָ鼭 clamp�� ������ �ɾ��
        Vector3 rot = transform.rotation.eulerAngles;
        rot.x = (rot.x > 180) ? rot.x - 360 : rot.x;
        rot.x = Mathf.Clamp(rot.x, MinAngle, MaxAngle);

        transform.rotation = Quaternion.Euler(rot.x, rot.y, 0);
    }
}
