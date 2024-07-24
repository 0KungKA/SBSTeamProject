using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PropInit : MonoBehaviour
{
    ObjectData myData;

    string Description;

    void Start()
    {
        myData = null;
        myData = Manager.DataManager_Instance.GetObjectData(this.gameObject);
        Init();
    }

    void Init()
    {
        if(myData.View_Object == true)
            gameObject.layer = (int)Layer_Enum.LayerInfo.ViewItem;

        if(myData.Get_Item == true)
        {
            gameObject.tag = "GetItem";

            ItemInfo myIteminfo = gameObject.AddComponent<ItemInfo>();
            myIteminfo.ItemExplanatino = Description;

            gameObject.AddComponent<ItemInteraction>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(Description != null)
            Manager.ErrorInfo_Instance.ErrorEnqueue(Description);
    }

    void Update()
    {
        
    }
}
