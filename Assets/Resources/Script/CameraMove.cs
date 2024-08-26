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
    float Hight = 8f;//카메라 높이

    [SerializeField]
    int MouseSensitivity = 100;//마우스 감도
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

    //테스트 전용
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
            else if(hit.transform.tag == "IObject")//상호작용 가능한 유동 오브젝트
            {
                Debug.Log("Ray : IObject");
                //SendMessage로 함수를 호출해주고 반환은 필요없으니까 안하는옵션을 넣어줌
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
            else if(hit.transform.tag == "ITObject")//유동X 습득X 없지만 3DIObjectView로 보여줘야하는 오브젝트들
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
        //마우스 XY축 얻어오는 코드
        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");

        //각도를 제한해주면서 시점을 넣어주는 코드
        transform.Rotate(-MouseY * MouseSensitivity * 10 * Time.deltaTime, 0f, 0);//마우스 Y축으로 시점 조작
        transform.Rotate(0f, MouseX * MouseSensitivity * 10 * Time.deltaTime, 0, Space.World);//마우스 X축으로 시점 조작

        //특정 각도이상이 되면 시점이 이상한 각도에서 고정되는 문제를 해결하기 위한 코드
        //정확하겐 eulerAngles 은 0 ~ 180도 까지 반환해주는거라 180도 이상이 되면 제한이 걸려서 360도 빼주면서 clamp로 제한을 걸어둠
        Vector3 rot = transform.rotation.eulerAngles;
        rot.x = (rot.x > 180) ? rot.x - 360 : rot.x;
        rot.x = Mathf.Clamp(rot.x, MinAngle, MaxAngle);

        transform.rotation = Quaternion.Euler(rot.x, rot.y, 0);
    }

    protected internal void DebugMode()
    {
        //포지션 변환
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

        //시점변환

        //마우스 XY축 얻어오는 코드
        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");

        //각도를 제한해주면서 시점을 넣어주는 코드
        transform.Rotate(-MouseY * MouseSensitivity * 10 * Time.deltaTime, 0f, 0);//마우스 Y축으로 시점 조작
        transform.Rotate(0f, MouseX * MouseSensitivity * 10 * Time.deltaTime, 0, Space.World);//마우스 X축으로 시점 조작

        //특정 각도이상이 되면 시점이 이상한 각도에서 고정되는 문제를 해결하기 위한 코드
        //정확하겐 eulerAngles 은 0 ~ 180도 까지 반환해주는거라 180도 이상이 되면 제한이 걸려서 360도 빼주면서 clamp로 제한을 걸어둠
        Vector3 rot = transform.rotation.eulerAngles;
        rot.x = (rot.x > 180) ? rot.x - 360 : rot.x;
        rot.x = Mathf.Clamp(rot.x, MinAngle, MaxAngle);

        transform.rotation = Quaternion.Euler(rot.x, rot.y, 0);
    }
}
