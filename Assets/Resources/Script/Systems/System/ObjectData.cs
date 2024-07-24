using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData : MonoBehaviour
{
    public string Divistoin;//분류 F = 가구 / P = 소품 / I = 아이템
    public string Name;//불러온 오브젝트 이름
    public bool View_Object;//3D로 모델링을 보여줘야 하는지에대한 여부
    public bool Get_Item;//인벤토리 습득 여부
    public string Description;//오브젝트 설명

    public ObjectData(string divistoin, string name, bool view_Object, bool get_Item, string description)
    {
        Divistoin = divistoin;
        Name = name;
        View_Object = view_Object;
        Get_Item = get_Item;
        Description = description;
    }
}
