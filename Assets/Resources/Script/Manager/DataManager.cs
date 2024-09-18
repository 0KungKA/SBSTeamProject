using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class Detail//스트링 테이블 이름+내용
{
    public string _name;
    public string _detail;

    public Detail(string name, string detail) { _name = name; _detail = detail; }
}
public class BGDetail//배경 오브젝트 테이블 내용
{
    public int _OriginIndex;//고유번호
    public int _ObjectType;//오브젝트 타입
    public string _ObjectName;//오브젝트 이름(스트링 테이블에서 가져옴) = 이름 리턴시켜주는 함수 작성해뒀음
    public string _ObjectFileName;//오브젝트 파일명(엔진상에 배치된 본래 가지고 있는 이름)
    public int _RotationView;//(3D Object View로 보여주는지 0 = false / 1 = true)
    public int _GetInventory;//인벤토리 획득 기능(0 = false / 1 = true)
    public int _OnHide;//숨기기능 있는지 (0 = false / 1 = true)
    public string _SoundFIleName1;//사운드 없으면 null
    public string _SoundFIleName2;//사운드 없으면 null
    public string _Desctiption1;//설명 없으면 null
    public string _Desctiption2;//설명 없으면 null
    public string _Desctiption3;//설명 없으면 null

    public BGDetail(int originIndex, int objectType, string objectName, string objectFileName,
        string rotationView, string getInventory, string onHide,
        string soundFIleName1, string soundFIleName2,
        string desctiption1, string desctiption2, string desctiption3)
    {
        _OriginIndex = originIndex;
        _ObjectType = objectType;
        _ObjectName = objectName;
        _ObjectFileName = objectFileName;
        _RotationView = int.Parse(rotationView);
        _GetInventory = int.Parse(getInventory);
        _OnHide = int.Parse(onHide);
        _SoundFIleName1 = soundFIleName1;
        _SoundFIleName2 = soundFIleName2;
        _Desctiption1 = desctiption1;
        _Desctiption2 = desctiption2;
        _Desctiption3 = desctiption3;
    }
}

public class DataManager : MonoBehaviour
{
    public string CSVPath = "Script/Systems/CSV_File/";

    List<Dictionary<string, object>> Data;

    ChatData chatList = new ChatData();

    //스트링 테이블 Start
    List<Detail> KOobjectName = new List<Detail>();
    List<Detail> KOCharacterName = new List<Detail>();
    List<Detail> KOInteractionDetail = new List<Detail>();
    List<Detail> KOchatDetail = new List<Detail>();
    List<Detail> KOUIDetail = new List<Detail>();
    //스트링 테이블 End

    Sprite[] CutSceneLists;
    public Sprite[] GetCutSceneLists() {  return CutSceneLists; }

    List<BGDetail> bGDetails = new List<BGDetail>();

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

        //배경오브젝트 테이블 시작
        SetBGObjectTable();
        //배경오브젝트 테이블 끝

        SetCutSceneIMG();//컷씬 스프라이트 가져오기
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

    //배경 오브젝트 테이블 시작
    public void SetBGObjectTable()
    {
        Data = CSVReader.Read(CSVPath + "BackgroundObjectTable");

        for(int i = 0; i < Data.Count; i++)
        {
            string name = Data[i]["오브젝트 파일명"].ToString();

            if (name != "ObjectFileName" && name != "String")
            {
               BGDetail temp = new BGDetail(int.Parse(Data[i]["고유번호"].ToString()),
                    int.Parse(Data[i]["오브젝트 타입"].ToString()),
                    ReturnObjName(Data[i]["오브젝트 이름"].ToString()), 
                    Data[i]["오브젝트 파일명"].ToString(),
                    Data[i]["3D 회전 뷰 기능"].ToString(),
                    Data[i]["인벤토리 획득 기능"].ToString(),
                    Data[i]["숨기 기능"].ToString(),
                    Data[i]["사운드 파일명1"].ToString(),
                    Data[i]["사운드 파일명2"].ToString(),
                    Data[i]["설명1"].ToString(),
                    Data[i]["설명2"].ToString(),
                    Data[i]["설명3"].ToString());

                if(temp != null)
                {
                    //설명 수정 부분 시작
                    if(temp._Desctiption1 != "" &&  temp._Desctiption1 != "Null" && temp._Desctiption1 != "null")
                    {
                        temp._Desctiption1 = ReturnObjACT(temp._Desctiption1);
                    }

                    if (temp._Desctiption2 != "" && temp._Desctiption2 != "Null" && temp._Desctiption2 != "null")
                    {
                        temp._Desctiption2 = ReturnObjACT(temp._Desctiption2);
                    }

                    if (temp._Desctiption3 != "" && temp._Desctiption3 != "Null" && temp._Desctiption3 != "null")
                    {
                        temp._Desctiption3 = ReturnObjACT(temp._Desctiption3);
                    }
                    //설명 수정 부분 끝
                }

                bGDetails.Add(temp);
            }
        }
    }
    //배경 오브젝트 테이블 끝

    //오브젝트 이름
    //오브젝트 설명
    //캐릭터 이름
    //캐릭터 대화 내용
    //UI

    //CutScene 테이블 가져오기
    public void SetCutSceneIMG()
    {
        CutSceneLists = null;
        CutSceneLists = Resources.LoadAll<Sprite>("0.UI/UITex/CutScene");
    }

    //GetDataObject 한글이름 가지고 오는 부분 = 삭제예정
    public string ReturnObjName(string objName)
    {
        foreach (Detail str in KOobjectName)
        {
            if (str._name == objName)
                return str._detail;
        }
        return "null";
    }

    //GetDataObjectDetail
    public string ReturnObjACT(string ACT)
    {
        foreach (Detail str in KOInteractionDetail)
        {
            if (str._name == ACT)
                return str._detail;
        }
        return "null";
    }

    public AudioClip ReturnAudioFile(string sundName)
    {
        return Resources.Load<AudioClip>("Assets/Resources/0.Sound/"+sundName);
    }
}
