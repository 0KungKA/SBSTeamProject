using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mapping : MonoBehaviour
{
    //Todo:나중에 데이터 테이블로 수치값 지정하기
    float MaxDistance;

    int FindMap = 0;

    GameObject[] Maps = new GameObject[4];//자식오브젝트인 찢어진 지도들

    void Start()
    {
        MaxDistance = GameObject.Find("Player_Camera").GetComponent<CameraMove>().GetMaxDistance();
        for (int i = 0; i < transform.childCount; i++)
        {
            Maps[i] = transform.GetChild(i).gameObject;
            Maps[i].SetActive(false);
        }

    }

    private void Update()
    {
        if (FindMap >= Maps.Length && GameObject.Find("B_Door_Lock") == null)
        {
            GameObject.Find("B_Exit").transform.GetChild(0).gameObject.SetActive(true);
            GameObject.Find("Player_Camera").GetComponent<InputManager>().StartOnMap();
            Destroy(transform.GetComponent<Mapping>());
        }

        if(Input.GetMouseButtonDown((int)UnityEngine.UIElements.MouseButton.LeftMouse))
        {
            RaycastHit hit;
            int layerMask = (-1) - (1 << LayerMask.NameToLayer("Glass"));
            if (Physics.Raycast(GameObject.Find("Player_Camera").transform.position, GameObject.Find("Player_Camera").transform.forward,
                out hit, MaxDistance, layerMask))
            {
                if(hit.transform.name == "map")
                {
                    hit.transform.GetComponent<Mission>().SendMSG();
                    Maps[FindMap].SetActive(true);
                    FindMap++;
                }
            }
        }
    }
}
