using System;
using System.Collections;
using System.Collections.Generic;
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
        ItemManager.ItemManager_Instance.SetItemSlot(gmItemSlot);
    }
}
