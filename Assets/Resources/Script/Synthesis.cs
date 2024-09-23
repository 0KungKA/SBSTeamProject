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
            Debug.Log(transform.name + "while �۵���");
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
            if (itemCase.Check || itamName != itemCase.SynthesisItem.name)//�������� �ռ��� �Ǿ��ٸ� ���� ������ üũ + ���� ã�� ���������� üũ
                continue;

            int Find = 0;//�̸��� ���� �������� �ִ��� ���� üũ

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

            if (Find == itemCase.ItemNameStirngRow.Length)//�ռ�����
            {
                foreach (string s in itemCase.ItemNameStirngRow)
                {
                    ItemManager.ItemManager_Instance.DeleteItem(s);
                    Debug.Log("�ռ�");
                }

                if (itemCase.RenderObj == null)
                {
                    object[] param = new object[2];
                    param[0] = itemCase.SynthesisItem.name;
                    param[1] = itemCase.RenderObj;

                    ItemManager.ItemManager_Instance.StartCoroutine("ItemViewSpawn", param);//�ش� ������ ����
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
