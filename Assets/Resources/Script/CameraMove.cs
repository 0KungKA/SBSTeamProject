using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

public class CameraMove : MonoBehaviour
{
    //Todo:나중에 데이터 테이블로 수치값 지정하기
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

    float MaxDistance = 13.0f;
    public float GetMaxDistance() { return MaxDistance; }

    float Horizontal;
    float Vertical;
    Vector3 Movement;

    Rigidbody rb;
    [SerializeField]
    AudioSource audio;

    GameObject[] npcs;
    public bool PlayHeart = false;

    [SerializeField]
    float Speed = 1.0f;
    public void AddSpeed(float speed) { Speed += Speed; }
    public void RemoveSpeed(float speed) { Speed -= Speed; }

    protected internal void Init()
    {
        audio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        Manager.Effect_SoundPlayer = transform.GetComponentInChildren<EffectSoundPlayer>();
    }

    void Update()
    {
        GameObject anithing = GameObject.FindWithTag("OnMouse");
        if (anithing != null)
            return;
        if (PlayHeart)
        {
            npcs = GameObject.FindGameObjectsWithTag("NPC");
            if (npcs.Length != 0)
            {
                Vector3 closestPosition = Vector3.zero;
                float closestDistance = Mathf.Infinity;

                for (int i = 0; i < npcs.Length; i++)
                {
                    float distance = Vector3.Distance(transform.position, npcs[i].transform.position);

                    if (distance < closestDistance)
                    {
                        closestPosition = npcs[i].transform.position;
                        closestDistance = distance;
                    }
                }

                if (closestDistance < 30)
                {
                    audio.volume = 1;
                    audio.pitch = 1.3f;
                    audio.pitch -= closestDistance * 0.01f;

                    if(audio.pitch < 1)
                        audio.pitch = 1;

                    if (audio.isPlaying == false)
                        audio.Play();
                }
            }
        }
        
        if (Input.GetMouseButtonDown((int)UnityEngine.UIElements.MouseButton.LeftMouse)) 
        {
            ObjInteraction();
        }
        else
        {
            RaycastHit hit;
            int layerMask = (-1) - (1 << LayerMask.NameToLayer("Glass"));
            if (Physics.Raycast(transform.position, transform.forward, out hit, MaxDistance, layerMask))
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
        int layerMask = (-1) - (1 << LayerMask.NameToLayer("Glass"));

        if (Physics.Raycast(transform.position, transform.forward, out hit, MaxDistance, layerMask))
        {
            if (hit.transform.GetComponent<ItemInfo>() != null)
            {
                if(hit.transform.GetComponent<ItemInfo>().effectSound)
                    Manager.Effect_SoundPlayer.EffectSoundPlay(hit.transform.GetComponent<ItemInfo>().effectSound);
            }

            if (hit.transform.GetComponent<Mission>() != null)
            {
                hit.transform.GetComponent<Mission>().SendMSG();
            }
            else if(hit.transform.tag == "IObject")//상호작용 가능한 유동 오브젝트
            {
                Debug.Log("Ray : IObject");
                //SendMessage로 함수를 호출해주고 반환은 필요없으니까 안하는옵션을 넣어줌
                hit.transform.GetComponent<ObjectInteraction>().SendMessage("InteractionStart", SendMessageOptions.DontRequireReceiver);
            }
            else if(hit.transform.tag == "GetItem")//아이템 습득
            {
                Debug.Log("Ray : GetItem");
                if(hit.transform.GetComponent<ItemInteraction>() != null)
                {
                    Manager.Origin_Object = hit.transform.gameObject;

                    
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "B_Trigger_CutScene")
        {
            GameObject.Find("EventSystem").GetComponent<SceneInit>().StartCoroutine("SceneFade");
        }

        if(other.gameObject.name == "First_Safe")
        {
            GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");
            npcs[0].AddComponent<NPCM_AI_Ctrl>();
            PlayHeart = true;
            Destroy(other.gameObject);
        }

        if (other.gameObject.name == "Speed_UP")
        {
            Speed += 0.25f;
        }

        if( other.gameObject.name == "Step_UP")
        {
            Hight = 13.0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Speed_UP")
        {
            Speed = 0.24f;
            Destroy(other.gameObject);
        }

        if (other.gameObject.name == "Step_UP")
        {
            Hight = 9.0f;
        }
    }
}
