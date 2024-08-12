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

        //스트링테이블 시작
        SetObjectNameList();
        SetCharacterNameList();
        SetObjectInteractionList();
        SetChatDetailList();
        SetUIDetailList();
        //스트링테이블 끝
        int a = 10;
    }

    //스트링테이블 시작
    public void SetObjectNameList()//오브젝트 이름
    {
        for (int i = 0; i < Data.Count; i++)
        {
            string name = Data[i]["키텍스트"].ToString();
            if (name.Contains("Obj_name"))
            {
                Detail temp = new Detail(name, Data[i]["한국어 출력 텍스트"].ToString());
                KOobjectName.Add(temp);
            }
        }
    }

    public void SetCharacterNameList()//캐릭터 이름
    {
        for (int i = 0; i < Data.Count; i++)
        {
            string name = Data[i]["키텍스트"].ToString();
            if (name.Contains("Char_info"))
            {
                Detail temp = new Detail(name, Data[i]["한국어 출력 텍스트"].ToString());
                KOCharacterName.Add(temp);
            }
        }
    }

    public void SetObjectInteractionList()//아이탬 상호작용 내용
    {
        for (int i = 0; i < Data.Count; i++)
        {
            string name = Data[i]["키텍스트"].ToString();
            if (name.Contains("Obj_act"))
            {
                Detail temp = new Detail(name, Data[i]["한국어 출력 텍스트"].ToString());
                KOInteractionDetail.Add(temp);
            }
        }
    }

    public void SetChatDetailList()//대화내용
    {
        for (int i = 0; i < Data.Count; i++)
        {
            string name = Data[i]["키텍스트"].ToString();
            if (name.Contains("Dlg_text"))
            {
                Detail temp = new Detail(name, Data[i]["한국어 출력 텍스트"].ToString());
                KOchatDetail.Add(temp);
            }
        }
    }

    public void SetUIDetailList()//UI 데이터
    {
        for (int i = 0; i < Data.Count; i++)
        {
            string name = Data[i]["키텍스트"].ToString();
            if (name.Contains("UI_text"))
            {
                Detail temp = new Detail(name, Data[i]["한국어 출력 텍스트"].ToString());
                KOUIDetail.Add(temp);
            }
        }
    }
    //스트링테이블 끝
}
