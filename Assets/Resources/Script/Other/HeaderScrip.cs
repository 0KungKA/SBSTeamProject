using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeaderScrip : MonoBehaviour
{
    [SerializeField]
    MannequinCheck[] mannequins;

    private void Start()
    {
        //mannequins = transform.parent.transform.Find("M").GetComponentsInChildren<MannequinCheck>();
    }

    private void Update()
    {
        int value = 0;
        for(int i = 0; i < mannequins.Length; i ++)
        {
            if (mannequins[i].mannequinCheck)
                value++;
        }

        if(value >= mannequins.Length)
        {
            Manager.Origin_Object = this.gameObject;
            Manager.Call_Object = gameObject.GetComponent<Mission>().ClearTarget;
            transform.GetComponent<Mission>().MissionClearSelf();
        }
    }
}
