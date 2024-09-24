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
public class BalanceClass//벨런스 테이블 타입 + 내용
{
    public int index;//번호
    public int type;//수치값 타입
    public int value;//수치값

    public BalanceClass(int _index, int _type, int _value)
    { index = _index; type = _type; value = _value; }
}
public class CutSceneClass//컷씬 테이블 + 오디오 소스 이름
{
    public int index;
    public int type;
    public Sprite img;
    public AudioClip soundSource;

    public CutSceneClass(int _index, int _type, Sprite _img, AudioClip _soundSource)
    {
        this.index = _index;
        this.type = _type;
        this.img = _img;
        this.soundSource = _soundSource;
    }   
}
public class NPCTalkClass//NPC대화 데이터 테이블
{
    public int index;
    /// <summary>
    /// 회자 캐릭터 = 캐릭터 이름  / 20002 = 아그네스 /  20001 = 케일럽 / 10001 = 나
    /// </summary>
    public string name;
    /// <summary>
    /// 사용 구분 / 1 = 게임 시작할떄 / 2 = 안방에 진입 이후 장농 또는 침대 밑에 숨었을때
    /// </summary>
    public int type;
    /// <summary>
    /// 대화 내용
    /// </summary>
    public string DialogText;
    public Sprite Left2DSprite;
    /// <summary>
    /// 대화 주최 / 1 = 활성 / 2 = 비활성 / 1일때는 디폴트 2일때는 약간 어둡게
    /// </summary>
    public int LeftHighlight;
    public Sprite Right2DSprite;
    /// <summary>
    /// 대화 주최 / 1 = 활성 / 2 = 비활성 / 1일때는 디폴트 2일때는 약간 어둡게
    /// </summary>
    public int RightHighlight;

    public NPCTalkClass(int index, string name, int type, string dialogText,
        Sprite left2DSprite, int leftHighlight,
        Sprite right2DSprite, int rightHighlight)
    {
        this.index = index;
        this.name = name;
        this.type = type;
        DialogText = dialogText;
        Left2DSprite = left2DSprite;
        LeftHighlight = leftHighlight;
        Right2DSprite = right2DSprite;
        RightHighlight = rightHighlight;
    }
}

public class DataManager : MonoBehaviour
{
    public string CSVPath = "Script/Systems/CSV_File/";

    List<Dictionary<string, object>> Data;

    ChatData chatList = new ChatData();

    //스트링 테이블
    List<Detail> KOobjectName = new List<Detail>();
    List<Detail> KOCharacterName = new List<Detail>();
    List<Detail> KOInteractionDetail = new List<Detail>();
    List<Detail> KOchatDetail = new List<Detail>();
    List<Detail> KOUIDetail = new List<Detail>();

    //대화 테이블
    List<NPCTalkClass> NPCTalkList = new List<NPCTalkClass>();

    //벨런스 테이블
    List<BalanceClass> BalanceList = new List<BalanceClass>();

    //컷씬 테이블
    List<CutSceneClass> CutSceneLists = new List<CutSceneClass>();

    List<BGDetail> bGDetails = new List<BGDetail>();

    private void Awake()
    {
        Data = CSVReader.Read(CSVPath + "StringTable");

        //스트링 테이블 시작
        SetObjectNameList();//오브젝트 분류
        SetCharacterNameList();//캐릭터 이름 분류
        SetObjectInteractionList();//오브젝트 상호작용 내용 분류
        SetChatDetailList();//대화내용 분휴
        SetUIDetailList();//UI 분류

        //배경오브젝트 테이블 시작
        SetBGObjectTable();

        //npc 대화 데이터 테이블 시작
        SetNPCTalkList();

        //벨런스 테이블 시작
        SetValueBalanceTable();

        //컷씬 테이블 시작
        SetCutSceneIMG();
    }

    //스트링테이블 시작

    //오브젝트 이름
    public void SetObjectNameList()
    {
        for (int i = 2; i < Data.Count; i++)
        {
            string name = Data[i]["키텍스트"].ToString();
            if (name.Contains("Obj_name"))
            {
                Detail temp = new Detail(name, Data[i]["한국어 출력 텍스트"].ToString());
                KOobjectName.Add(temp);
            }
        }
    }
    public string GetObjectName(GameObject _this)
    {
        for(int i = 0; i < KOobjectName.Count; i++  )
        {
            if(_this.name == KOobjectName[i]._name)
                return KOobjectName[i]._detail;
        }

        return "objectName";
    }

    //캐릭터 이름
    public void SetCharacterNameList()
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
    public string GetCharacterName(string _key)
    {
        for (int i = 0; i < KOCharacterName.Count; i++)
        {
            if (_key == KOCharacterName[i]._name)
                return KOCharacterName[i]._detail;
        }

        return "...";
    }

    //아이탬 상호작용 내용
    public void SetObjectInteractionList()
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
    /// <summary>
    /// 데이터 테이블에 ACT머시기 뺴고 위데 숫자만 넣어주면됨 예 001 = 1
    /// </summary>
    /// <param name="numbrt"></param>
    public string GetObjectInteractionString(int number)
    {
        string temp = KOInteractionDetail[number]._detail;

        if (temp != null)
            return temp;

        else
            return "오류";
    }

    //대화내용
    public void SetChatDetailList()
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
    public string GetChatDetail(string _key)
    {
        for (int i = 0; i < KOchatDetail.Count; i++)
        {
            if (_key == KOchatDetail[i]._name)
                return KOchatDetail[i]._detail;
        }

        return "...";
    }

    //UI 데이터
    public void SetUIDetailList()
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


    //대화내용 합체 시작
    public void SetNPCTalkList()
    {
        Data = CSVReader.Read(CSVPath + "NPC_Talk");
        Sprite[] npcImages = Resources.LoadAll<Sprite>("Material/ObjectTexture/NPC/NPCImage");

        for (int i = 2; i < Data.Count; i++)
        {
            int _index = int.Parse(Data[i]["번호"].ToString());

            string name = "";
            switch (int.Parse(Data[i]["화자 캐릭터"].ToString()))
            {
                case 10001:
                    name = "엔서니";
                    break;
                case 20001:
                    name = "케일럽";
                    break;
                case 20002:
                    name = "아그네스";
                    break;

                default:
                    name = "";
                    break;
            }

            //화자 캐릭터 이름 패스
            int _type = int.Parse(Data[i]["사용 파트"].ToString());

            string dialog = GetChatDetail(Data[i]["대화 텍스트"].ToString());

            Sprite LCharacter = null;
            for (int j = 0; j <  npcImages.Length; j++)
            {
                if (Data[i]["좌측 2D 일러스트 파일명"].ToString() == npcImages[j].name)
                {
                    LCharacter = npcImages[j];
                }
            }

            int LeftHeighlignt = int.Parse(Data[i]["좌측 2D 일러스트 강조"].ToString());

            Sprite TCharacter = null; ;
            for (int j = 0; j < npcImages.Length; j++)
            {
                if (Data[i]["우측 2D 일러스트 파일명"].ToString() == npcImages[j].name)
                {
                    TCharacter = npcImages[j];
                }
            }

            int RightHeighlignt = int.Parse(Data[i]["우측 2D 일러스트 강조"].ToString());

            NPCTalkClass temp = new NPCTalkClass(_index, name, _type, dialog, LCharacter, LeftHeighlignt, TCharacter, RightHeighlignt);


            NPCTalkList.Add(temp);
        }
    }
    public List<NPCTalkClass> GetCPNTalk() { return NPCTalkList; }

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


    //밸런스 테이블 시작
    public void SetValueBalanceTable()
    {
        Data = CSVReader.Read(CSVPath + "ValueBalanceTable");

        for (int i = 2; i < Data.Count; i++)
        {
            BalanceClass temp = new BalanceClass(int.Parse(Data[i]["번호"].ToString()),
                int.Parse(Data[i]["수치값 타입"].ToString()),
                int.Parse(Data[i]["수치값"].ToString()));

            BalanceList.Add(temp);
        }
    }
    public int GetBalanceValue(int index)
    {
        for(int i = 0; i < BalanceList.Count; i++)
        {
            if (BalanceList[i].index == index)
                return BalanceList[i].value;
        }
        Debug.Log("데이터 테이블 리턴 오류");
        return 0;
    }


    //CutScene 테이블 가져오기
    public void SetCutSceneIMG()
    {
        Data = CSVReader.Read(CSVPath + "CutSceneTable");

        Sprite[] cutSceneArr = Resources.LoadAll<Sprite>("0.UI/UITex/CutScene");
        AudioClip[] audioclipArr = Resources.LoadAll<AudioClip>("0.Sound");


        int cutSceneNum = 0;
        int cutSceneAidioClipNum = 0;

        for (int i = 2; i < Data.Count; i++)
        {
            for(int j = 0; j < cutSceneArr.Length; j++)
            {
                if (cutSceneArr[j].name == Data[i]["이미지 파일명"].ToString())
                {
                    cutSceneNum = j;
                }
            }

            for (int j = 0; j < audioclipArr.Length; j++)
            {
                if(Data[i]["사운드 파일명"].ToString() == "NULL")
                {
                    audioclipArr[j].name = "NULL";
                    break;
                }
                if (audioclipArr[j].name == Data[i]["사운드 파일명"].ToString())
                {
                    cutSceneAidioClipNum = j;
                }
            }
            CutSceneClass temp = new CutSceneClass(int.Parse(Data[i]["번호"].ToString()),
                int.Parse(Data[i]["이미지 타입"].ToString()),
                cutSceneArr[cutSceneNum],//이미지 스프라이트 넣을거임
                audioclipArr[cutSceneAidioClipNum]);//이미지 사운드클립 넣을거임

            CutSceneLists.Add(temp);
        }
    }
    public List<CutSceneClass> GetCutSceneLists() { return CutSceneLists; }

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
