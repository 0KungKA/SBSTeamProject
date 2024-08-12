using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class Detail
{
    string _name;
    string _detail;

    public Detail(string name, string detail) { _name = name; _detail = detail; }
}

public class DataManager : MonoBehaviour
{
    public string CSVPath = "Script/Systems/CSV_File/";

    List<Dictionary<string, object>> Data;

    ChatData chatList = new ChatData();

    List<Detail> KOobjectName = new List<Detail>();
    List<Detail> KOCharacterName = new List<Detail>();
    List<Detail> KOInteractionDetail = new List<Detail>();
    List<Detail> KOchatDetail = new List<Detail>();
    List<Detail> KOUIDetail = new List<Detail>();

    private void Awake()
    {
        Data = CSVReader.Read(CSVPath + "StringTable");

        //��Ʈ�����̺� ����
        SetObjectNameList();
        SetCharacterNameList();
        SetObjectInteractionList();
        SetChatDetailList();
        SetUIDetailList();
        //��Ʈ�����̺� ��
        int a = 10;
    }

    //��Ʈ�����̺� ����
    public void SetObjectNameList()//������Ʈ �̸�
    {
        for (int i = 0; i < Data.Count; i++)
        {
            string name = Data[i]["Ű�ؽ�Ʈ"].ToString();
            if (name.Contains("Obj_name"))
            {
                Detail temp = new Detail(name, Data[i]["�ѱ��� ��� �ؽ�Ʈ"].ToString());
                KOobjectName.Add(temp);
            }
        }
    }

    public void SetCharacterNameList()//ĳ���� �̸�
    {
        for (int i = 0; i < Data.Count; i++)
        {
            string name = Data[i]["Ű�ؽ�Ʈ"].ToString();
            if (name.Contains("Char_info"))
            {
                Detail temp = new Detail(name, Data[i]["�ѱ��� ��� �ؽ�Ʈ"].ToString());
                KOCharacterName.Add(temp);
            }
        }
    }

    public void SetObjectInteractionList()//������ ��ȣ�ۿ� ����
    {
        for (int i = 0; i < Data.Count; i++)
        {
            string name = Data[i]["Ű�ؽ�Ʈ"].ToString();
            if (name.Contains("Obj_act"))
            {
                Detail temp = new Detail(name, Data[i]["�ѱ��� ��� �ؽ�Ʈ"].ToString());
                KOInteractionDetail.Add(temp);
            }
        }
    }

    public void SetChatDetailList()//��ȭ����
    {
        for (int i = 0; i < Data.Count; i++)
        {
            string name = Data[i]["Ű�ؽ�Ʈ"].ToString();
            if (name.Contains("Dlg_text"))
            {
                Detail temp = new Detail(name, Data[i]["�ѱ��� ��� �ؽ�Ʈ"].ToString());
                KOchatDetail.Add(temp);
            }
        }
    }

    public void SetUIDetailList()//UI ������
    {
        for (int i = 0; i < Data.Count; i++)
        {
            string name = Data[i]["Ű�ؽ�Ʈ"].ToString();
            if (name.Contains("UI_text"))
            {
                Detail temp = new Detail(name, Data[i]["�ѱ��� ��� �ؽ�Ʈ"].ToString());
                KOUIDetail.Add(temp);
            }
        }
    }
    //��Ʈ�����̺� ��
}
