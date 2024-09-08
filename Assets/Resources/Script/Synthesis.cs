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

    GameObject[] itemslotTemp;//����ִ� ������ ���� ����

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
            if (itemCase.Check)//�������� �ռ��� �Ǿ��ٸ� ���� ������ üũ
                continue;

            int Find = 0;//�̸��� ���� �������� �ִ��� ���� üũ

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

            if(Find == itemCase.ItemNameStirngRow.Length)//�ռ�����
            {
                foreach (string s in itemCase.ItemNameStirngRow)
                {
                    ItemManager.ItemManager_Instance.DeleteItem(s);
                    Debug.Log("�ռ�");
                }

                ItemManager.ItemManager_Instance.StartCoroutine("ItemViewSpawn", itemCase.SynthesisItem.name);//�ش� ������ ����

            }

        }
    }
}
