using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RenderViewObj : MonoBehaviour
{
    [SerializeField]
    public GameObject Target;

    [SerializeField]
    float RotSpeed = 300;

    GameObject RenderCamera;//렌더 카메라 오브젝트
    RectTransform RenderCameraTransform;//렌더 카메라 RectTransform 컴포넌트
    Camera RenderCameraComponent;//카메라 컴포넌트

    bool RenderInteraction = false;
    public void SetRenderInteraction(bool value) {  RenderInteraction = value; }
    public bool GetRenderInteraction() { return RenderInteraction; }


    private void Start()
    {
        RenderCamera = transform.parent.Find("RenderCamera").gameObject;
        RenderCameraTransform = RenderCamera.GetComponent<RectTransform>();
        RenderCameraComponent = RenderCamera.GetComponent<Camera>();
    }

    public void Targetset(GameObject gm)
    {
        RenderCamera.GetComponent<Camera>().orthographicSize = 100;
        RenderInteraction = true;
        if (transform.childCount != 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject != gm)
                    Destroy(transform.GetChild(i).gameObject);
            }
        }

        Target = gm.transform.gameObject;
        Target.transform.parent = transform;
        Target.transform.localPosition = Vector3.zero;
        if(Target.transform.localScale.x <= 1.0f || Target.transform.localScale.y <= 1.0f || Target.transform.localScale.z <= 1.0f)
        {
            Target.transform.localScale = Vector3.one;
        }
    }

    public void Update()
    {
        if (Target != null)
        {
            float scrolVale = Input.GetAxisRaw("Mouse ScrollWheel");
            float MouseX = Input.GetAxisRaw("Mouse X");
            float MouseY = Input.GetAxisRaw("Mouse Y");
            
            if (Input.GetMouseButton((int)MouseButton.Middle))
            {
                RenderCameraTransform.anchoredPosition3D += new Vector3(MouseX * -2, MouseY * -2, 0);
            }
            else if (scrolVale != 0)
            {
                scrolVale = scrolVale  * -1;
                RenderCameraComponent.orthographicSize += scrolVale * 5;
            }
            else if (Input.GetMouseButton((int)MouseButton.Left))
            {
                Vector3 rot = new Vector3(MouseY * 1 * RotSpeed * Time.deltaTime,MouseX * -1 * RotSpeed * Time.deltaTime, 0);

                Target.transform.Rotate(rot);
            }
        }
    }

    public void DestroyTarget()
    {
        Target.transform.parent = null;
        Destroy(Target.gameObject);
        RenderInteraction = false;
    }
}
