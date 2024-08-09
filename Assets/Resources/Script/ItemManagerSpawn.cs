using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemManagerSpawn : MonoBehaviour
{
    GameObject[] gmItemSlot = null;

    public GameObject Spawn(GameObject prefepPath)
    {
        GameObject go = Instantiate(prefepPath);//�ϴ� Instantiate�� ����
        int SCindex = go.name.IndexOf("(Clone)");//�̸��߿� (Clone) �� ã�Ƽ� ����� �ε������� �����ϴ��� ������
        if (SCindex > 0)//���� (Clone) �̶� �ܾ ���� �޾ƿ� ���� ������ �Ʒ��� �ڵ带 ����
            go.name = go.name.Substring(0, SCindex);//���꽺Ʈ������ (Clone) �ܾ� ���ۺ��� ©�� �̸� �ھƹ���

        return go;
    }

    void Start()
    {
        if (GameObject.Find("@ItemManager") == null)
        {
            GameObject IM = Resources.Load<GameObject>("Prefep/@ItemManager");
            if(IM != null )
            {
                IM = Spawn(IM);
                DontDestroyOnLoad(IM);
            }
        }

        Init();
    }

    public void Init()
    {
        gmItemSlot = GameObject.FindGameObjectsWithTag("Slot");
        GameObject[] temp = new GameObject[gmItemSlot.Length];
        int Rvalue = 0;
        while (true) 
        {
            if(Rvalue >= gmItemSlot.Length)
                break;
            else
            {
                for(int i = 0; i <  gmItemSlot.Length; i++)
                {
                    int numtemp = Rvalue + 1;
                    if (gmItemSlot[i].name.Contains(numtemp.ToString()))
                    {
                        temp[Rvalue] = gmItemSlot[i];
                        Rvalue++;
                    }
                }
            }
        }

        ItemManager.ItemManager_Instance.SetItemSlot(temp);
    }
}
