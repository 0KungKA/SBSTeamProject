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
            Debug.Log(transform.name + "while 작동중");
            if (itemslotTemp == null)
                itemslotTemp = ItemManager.ItemManager_Instance.GetItemSlot();
            else if (itemslotTemp != null) 
            {
                yield break;
            }

            yield return null;
        }
    }

    internal void SelectItemSynthesis(string itamName)
    {
        foreach (var itemCase in SynthesisItemCase)
        {
            if (itemCase.Check || itamName != itemCase.SynthesisItem.name)//아이템이 합성이 되었다면 다음 아이템 체크 + 내가 찾는 아이템인지 체크
                continue;

            int Find = 0;//이름이 같은 아이템이 있는지 여부 체크

            string[] items = new string[itemCase.ItemNameStirngRow.Length];
            GameObject[] currentItemSlot = ItemManager.ItemManager_Instance.GetItemSlot();

            for (int i = 0; i < currentItemSlot.Length; i++)
            {
                for (int j = 0; j < items.Length; j++)
                {
                    if (currentItemSlot[i].transform.childCount != 0)
                    {
                        if (itemslotTemp[i].transform.GetChild(0).name == itemCase.ItemNameStirngRow[j])
                            Find++;
                    }
                }
            }

            if (Find == itemCase.ItemNameStirngRow.Length)//합성조건
            {
                foreach (string s in itemCase.ItemNameStirngRow)
                {
                    ItemManager.ItemManager_Instance.DeleteItem(s);
                    Debug.Log("합성");
                }

                if (itemCase.RenderObj == null)
                {
                    object[] param = new object[2];
                    param[0] = itemCase.SynthesisItem.name;
                    param[1] = itemCase.RenderObj;

                    ItemManager.ItemManager_Instance.StartCoroutine("ItemViewSpawn", param);//해당 아이템 지급
                }
                else if (itemCase.RenderObj != null)
                {
                    ItemManager.ItemManager_Instance.StartCoroutine("ItemViewSpawn", itemCase.SynthesisItem.name);
                }
            }
        }
    }

    public void SynthesisModule()
    {
        foreach (var itemCase in SynthesisItemCase)
        {
            if (itemCase.Check)//아이템이 합성이 되었다면 다음 아이템 체크
                continue;

            int Find = 0;//이름이 같은 아이템이 있는지 여부 체크

            string[] items = new string[itemCase.ItemNameStirngRow.Length];
            GameObject[] currentItemSlot = ItemManager.ItemManager_Instance.GetItemSlot();

            for(int i = 0; i < currentItemSlot.Length; i++)
            {
                for(int j = 0; j < items.Length; j++)
                {
                    if(currentItemSlot[i].transform.childCount != 0)
                    {
                        if (itemslotTemp[i].transform.GetChild(0).name == itemCase.ItemNameStirngRow[j])
                            Find++;
                    }
                }
            }

            if(Find == itemCase.ItemNameStirngRow.Length)//합성조건
            {
                foreach (string s in itemCase.ItemNameStirngRow)
                {
                    ItemManager.ItemManager_Instance.DeleteItem(s);
                    Debug.Log("합성");
                }

                ItemManager.ItemManager_Instance.StartCoroutine("ItemViewSpawn", itemCase.SynthesisItem.name);//해당 아이템 지급
            }
        }
    }
}
