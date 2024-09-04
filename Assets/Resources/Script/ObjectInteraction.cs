using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

[AddComponentMenu("devchanho/ObjectInteraction")]//빠르게 오브젝트에 컴포넌트를 추가하기위해 AddComponent 메뉴에 추가해둠
public class ObjectInteraction : MonoBehaviour
{
    [Header("주의 : 로컬포지션으로 밀어버림")]
    [SerializeField]
    [Header("오브젝트를 상호작용할때 목표로 잡은 포지션")]
    Vector3 TargetPos;

    [Space(20)]

    [SerializeField]
    [Header("오브젝트를 상호작용할때 목표로 잡은 로테이션")]
    Vector3 TargetRot;

    [Header("열리는 오브젝트면 열려할 방향으로 체크\n팁 : 해당오브젝트기준으로 맨처음 어느방향으로 열리는지 체크")]
    [SerializeField]
    bool L;
    [SerializeField]
    bool R;

    [Header("좌우 열림(장농문 류) / 포지션 이동(서랍류) / 위로열림(보석함 류)")]
    [SerializeField]
    bool Rot;
    [SerializeField]
    bool Move;
    [SerializeField]
    bool Open;

    [SerializeField]
    [Header("Pos Speed / Rot Speed")]
    Vector2 Speed = new Vector2(80.0f,80.0f);

    [SerializeField]
    AudioClip objectInteractionSount;
    AudioSource audioSorce;

    //만일 옷장같은 가구의 문이 열려야 할때 왼쪽 오른쪽 구분해서 스크립트를 작성함
    Vector3 SavePos;//원래 Position
    Vector3 SaveRot;//원래 Rotation

    bool thisMove = false;//이게 이미 움직인 오브젝트인가에 대한 bool 값
    bool OnMove = false;//움직이고 있는지 아닌지 확인하는 bool 값

    bool OnHide = false;//숨은 상태인지 아닌지 확인하는 bool값

    [SerializeField]
    bool OnTest = false;

    /*[Space(20)]
    //만일 옷장같은 숨을수있는 가구와 상호작용할경우 해당 가구 앞쪽에 카메라를 이동시키기 위해 Vector3값으로 포지션 셋팅
    [SerializeField]
    Vector3 CameraPos;*/
    private void Start()
    {
        if (gameObject.GetComponent<MeshCollider>() == null)//Mesh collider가 없는경우
        {
            //Mesh Collider를 추가해야하지만 피봇이 어긋나서 MT로 따로 잡아준 경우가 있으니 예외처리
            //상호작용 가능한 오브젝트는 모두 Mesh Fillter랑 Mesh Renderer가 있으니 이걸로 예외처리 실행
            if (gameObject.GetComponent<MeshFilter>() != null && gameObject.GetComponent<MeshRenderer>() != null)
            {
                //통과하면 MeshCollider를 추가
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
                Debug.Log(transform.parent.name + " " + gameObject.name + " : 설정값 오류");

            else if (transform.parent != null)
                Debug.Log(gameObject.name + " : 설정값 오류");
        }

        if (transform.name == "L_Door_Pivot")
        {
            int a = 10;
        }

        //움직이든 돌리든 어쨋든 다시 원상태로 돌려야하기때문에 두개다 받아줌
        SavePos = transform.localPosition;

        SaveRot.x = (transform.localEulerAngles.x >= 180) ? transform.localEulerAngles.x - 360 : transform.localEulerAngles.x;
        SaveRot.y = (transform.localEulerAngles.y >= 180) ? transform.localEulerAngles.y - 360 : transform.localEulerAngles.y;
        SaveRot.z = (transform.localEulerAngles.z >= 180) ? transform.localEulerAngles.z - 360 : transform.localEulerAngles.z;
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            OnTest = !OnTest;
            Manager.ErrorInfo_Instance.ErrorEnqueue("모든 유동 오브젝트 테스트 모드 켜짐");
        }
        if (Input.GetKeyDown(KeyCode.P) && OnTest == true)
        {
            Manager.ErrorInfo_Instance.ErrorEnqueue("모든 유동 오브젝트 테스트 모드 켜짐");
            InteractionStart();
        }
    }

    //sendMessage로 호출할거임
    void InteractionStart()
    {
        if (OnMove) return;//오브젝트가 이미 상호작용을 해서 움직이고있는 상태이면 리턴박아서 더 못들어오게 설정

        if(audioSorce != null)
        {
            audioSorce.Play();
        }
        
        if(L != true && R != true && Open == false)
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
                StartCoroutine(RotL(TargetRot));//힌지가 왼쪽에 있으니까 열릴떄는 RotL 닫힐때는 RotR
            }
            else if (thisMove == true)
            {
                StartCoroutine(RotR(SaveRot));
            }
            
        }
        else if(R)
        {
            if (thisMove == false)//힌지가 오른쪽에 있으니까 열릴떄는 RotR 닫힐때는 RotL
            {
                StartCoroutine(RotR(TargetRot));
            }
            else if (thisMove == true)
            {
                StartCoroutine(RotL(SaveRot));
            }
        }
        else if(Open)//보석함류 열기 닫기
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
    }

    IEnumerator MovePos(Vector3 TargetPos)//유동오브젝트 와리가리 해주는 코드
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

    IEnumerator RotL(Vector3 TargetRot)//Y축(Vector3.up)기준 왼쪽으로 돌려주는 코드
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

    IEnumerator RotR(Vector3 TargetRot)//Y축(Vector3.up)기준 오른쪽으로 돌려주는 코드
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

    IEnumerator RotOpen(Vector3 TargetRot)//X축 기준으로 위로 열어주는 코드
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

    IEnumerator RotClose(Vector3 TargetRot)//X축 기준으로 위로 열어주는 코드
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
