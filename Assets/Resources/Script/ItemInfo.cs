using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    [SerializeField]
    public string ItemExplanatino;//아이탬 설명

    [SerializeField]
    public int AddRenderCameraSize = 0;//3D ObjectView 로 보여줄때 작으면 잘 안보이니까 여기서 수치 조절해서 렌더카메라의 Size를 조정함
}
