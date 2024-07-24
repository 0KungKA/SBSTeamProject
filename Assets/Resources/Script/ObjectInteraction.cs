using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

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

    [SerializeField]
    [Header("Pos Speed / Rot Speed")]
    Vector2 Speed = new Vector2(300.0f,80.0f);

    [SerializeField]
    AudioClip objectInteractionSount;
    AudioSource audioSorce;


    //만일 옷장같은 가구의 문이 열려야 할때 왼쪽 오른쪽 구분해서 스크립트를 작성함

    Vector3 SavePos;//원래 Position
    Vector3 SaveRot;//원래 Rotation

    bool thisMove = false;//이게 이미 움직인 오브젝트인가에 대한 bool 값
    bool OnMove = false;//움직이고 있는지 아닌지 확인하는 bool 값

    [Space(20)]
    //만일 옷장같은 숨을수있는 가구와 상호작용할경우 해당 가구 앞쪽에 카메라를 이동시키기 위해 Vector3값으로 포지션 셋팅
    [SerializeField]
    Vector3 CameraPos;

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
            Debug.Log(gameObject.name + " : 설정값 오류");

        //움직이든 돌리든 어쨋든 다시 원상태로 돌려야하기때문에 두개다 받아줌
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

    //sendMessage로 호출할거임
    void InteractionStart()
    {
        if (OnMove) return;//오브젝트가 이미 상호작용을 해서 움직이고있는 상태이면 리턴박아서 더 못들어오게 설정

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

    }

    IEnumerator MovePos(Vector3 TargetPos)//유동오브젝트 와리가리 해주는 코드
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

    IEnumerator RotL(Vector3 TargetRot)//Y축(Vector3.up)기준 왼쪽으로 돌려주는 코드
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

    IEnumerator RotR(Vector3 TargetRot)//Y축(Vector3.up)기준 오른쪽으로 돌려주는 코드
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
