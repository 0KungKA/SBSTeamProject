using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Synthesis : MonoBehaviour
{
    [SerializeField]
    SynthesisRow[] SynthesisItemCase;

    GameObject[] itemslotTemp;//비어있는 아이템 슬롯 리턴

    int temp = 0;

    public void Init()
    {
        StartCoroutine("FindItemSlot");
    }

    IEnumerator FindItemSlot()
    {
        while (true)
        {
            if (itemslotTemp == null)
                itemslotTemp = ItemManager.ItemManager_Instance.GetItemSlot();
            else if (itemslotTemp != null) 
            {
                yield break;
            }

            yield return null;
        }
    }
    public void SynthesisModule()
    {
        foreach (var itemCase in SynthesisItemCase)
        {
            if(itemCase.Check == false)
            {
               
            }
        }
    }
}
