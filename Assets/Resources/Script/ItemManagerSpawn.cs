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
        GameObject go = Instantiate(prefepPath);//일단 Instantiate로 만듦
        int SCindex = go.name.IndexOf("(Clone)");//이름중에 (Clone) 을 찾아서 몇번쨰 인덱스부터 시작하는지 가져옴
        if (SCindex > 0)//만약 (Clone) 이란 단어가 들어가서 받아온 값이 있으면 아래쪽 코드를 실행
            go.name = go.name.Substring(0, SCindex);//서브스트링으로 (Clone) 단어 시작부터 짤라서 이름 박아버림

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
