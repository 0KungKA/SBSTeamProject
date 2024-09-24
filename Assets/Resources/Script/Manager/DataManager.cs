using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class Detail//��Ʈ�� ���̺� �̸�+����
{
    public string _name;
    public string _detail;

    public Detail(string name, string detail) { _name = name; _detail = detail; }
}
public class BGDetail//��� ������Ʈ ���̺� ����
{
    public int _OriginIndex;//������ȣ
    public int _ObjectType;//������Ʈ Ÿ��
    public string _ObjectName;//������Ʈ �̸�(��Ʈ�� ���̺��� ������) = �̸� ���Ͻ����ִ� �Լ� �ۼ��ص���
    public string _ObjectFileName;//������Ʈ ���ϸ�(������ ��ġ�� ���� ������ �ִ� �̸�)
    public int _RotationView;//(3D Object View�� �����ִ��� 0 = false / 1 = true)
    public int _GetInventory;//�κ��丮 ȹ�� ���(0 = false / 1 = true)
    public int _OnHide;//������ �ִ��� (0 = false / 1 = true)
    public string _SoundFIleName1;//���� ������ null
    public string _SoundFIleName2;//���� ������ null
    public string _Desctiption1;//���� ������ null
    public string _Desctiption2;//���� ������ null
    public string _Desctiption3;//���� ������ null

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
public class BalanceClass//������ ���̺� Ÿ�� + ����
{
    public int index;//��ȣ
    public int type;//��ġ�� Ÿ��
    public int value;//��ġ��

    public BalanceClass(int _index, int _type, int _value)
    { index = _index; type = _type; value = _value; }
}
public class CutSceneClass//�ƾ� ���̺� + ����� �ҽ� �̸�
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
public class NPCTalkClass//NPC��ȭ ������ ���̺�
{
    public int index;
    /// <summary>
    /// ȸ�� ĳ���� = ĳ���� �̸�  / 20002 = �Ʊ׳׽� /  20001 = ���Ϸ� / 10001 = ��
    /// </summary>
    public string name;
    /// <summary>
    /// ��� ���� / 1 = ���� �����ҋ� / 2 = �ȹ濡 ���� ���� ��� �Ǵ� ħ�� �ؿ� ��������
    /// </summary>
    public int type;
    /// <summary>
    /// ��ȭ ����
    /// </summary>
    public string DialogText;
    public Sprite Left2DSprite;
    /// <summary>
    /// ��ȭ ���� / 1 = Ȱ�� / 2 = ��Ȱ�� / 1�϶��� ����Ʈ 2�϶��� �ణ ��Ӱ�
    /// </summary>
    public int LeftHighlight;
    public Sprite Right2DSprite;
    /// <summary>
    /// ��ȭ ���� / 1 = Ȱ�� / 2 = ��Ȱ�� / 1�϶��� ����Ʈ 2�϶��� �ణ ��Ӱ�
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

    //��Ʈ�� ���̺�
    List<Detail> KOobjectName = new List<Detail>();
    List<Detail> KOCharacterName = new List<Detail>();
    List<Detail> KOInteractionDetail = new List<Detail>();
    List<Detail> KOchatDetail = new List<Detail>();
    List<Detail> KOUIDetail = new List<Detail>();

    //��ȭ ���̺�
    List<NPCTalkClass> NPCTalkList = new List<NPCTalkClass>();

    //������ ���̺�
    List<BalanceClass> BalanceList = new List<BalanceClass>();

    //�ƾ� ���̺�
    List<CutSceneClass> CutSceneLists = new List<CutSceneClass>();

    List<BGDetail> bGDetails = new List<BGDetail>();

    private void Awake()
    {
        Data = CSVReader.Read(CSVPath + "StringTable");

        //��Ʈ�� ���̺� ����
        SetObjectNameList();//������Ʈ �з�
        SetCharacterNameList();//ĳ���� �̸� �з�
        SetObjectInteractionList();//������Ʈ ��ȣ�ۿ� ���� �з�
        SetChatDetailList();//��ȭ���� ����
        SetUIDetailList();//UI �з�

        //��������Ʈ ���̺� ����
        SetBGObjectTable();

        //npc ��ȭ ������ ���̺� ����
        SetNPCTalkList();

        //������ ���̺� ����
        SetValueBalanceTable();

        //�ƾ� ���̺� ����
        SetCutSceneIMG();
    }

    //��Ʈ�����̺� ����

    //������Ʈ �̸�
    public void SetObjectNameList()
    {
        for (int i = 2; i < Data.Count; i++)
        {
            string name = Data[i]["Ű�ؽ�Ʈ"].ToString();
            if (name.Contains("Obj_name"))
            {
                Detail temp = new Detail(name, Data[i]["�ѱ��� ��� �ؽ�Ʈ"].ToString());
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

    //ĳ���� �̸�
    public void SetCharacterNameList()
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
    public string GetCharacterName(string _key)
    {
        for (int i = 0; i < KOCharacterName.Count; i++)
        {
            if (_key == KOCharacterName[i]._name)
                return KOCharacterName[i]._detail;
        }

        return "...";
    }

    //������ ��ȣ�ۿ� ����
    public void SetObjectInteractionList()
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
    /// <summary>
    /// ������ ���̺� ACT�ӽñ� ���� ���� ���ڸ� �־��ָ�� �� 001 = 1
    /// </summary>
    /// <param name="numbrt"></param>
    public string GetObjectInteractionString(int number)
    {
        string temp = KOInteractionDetail[number]._detail;

        if (temp != null)
            return temp;

        else
            return "����";
    }

    //��ȭ����
    public void SetChatDetailList()
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
    public string GetChatDetail(string _key)
    {
        for (int i = 0; i < KOchatDetail.Count; i++)
        {
            if (_key == KOchatDetail[i]._name)
                return KOchatDetail[i]._detail;
        }

        return "...";
    }

    //UI ������
    public void SetUIDetailList()
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


    //��ȭ���� ��ü ����
    public void SetNPCTalkList()
    {
        Data = CSVReader.Read(CSVPath + "NPC_Talk");
        Sprite[] npcImages = Resources.LoadAll<Sprite>("Material/ObjectTexture/NPC/NPCImage");

        for (int i = 2; i < Data.Count; i++)
        {
            int _index = int.Parse(Data[i]["��ȣ"].ToString());

            string name = "";
            switch (int.Parse(Data[i]["ȭ�� ĳ����"].ToString()))
            {
                case 10001:
                    name = "������";
                    break;
                case 20001:
                    name = "���Ϸ�";
                    break;
                case 20002:
                    name = "�Ʊ׳׽�";
                    break;

                default:
                    name = "";
                    break;
            }

            //ȭ�� ĳ���� �̸� �н�
            int _type = int.Parse(Data[i]["��� ��Ʈ"].ToString());

            string dialog = GetChatDetail(Data[i]["��ȭ �ؽ�Ʈ"].ToString());

            Sprite LCharacter = null;
            for (int j = 0; j <  npcImages.Length; j++)
            {
                if (Data[i]["���� 2D �Ϸ���Ʈ ���ϸ�"].ToString() == npcImages[j].name)
                {
                    LCharacter = npcImages[j];
                }
            }

            int LeftHeighlignt = int.Parse(Data[i]["���� 2D �Ϸ���Ʈ ����"].ToString());

            Sprite TCharacter = null; ;
            for (int j = 0; j < npcImages.Length; j++)
            {
                if (Data[i]["���� 2D �Ϸ���Ʈ ���ϸ�"].ToString() == npcImages[j].name)
                {
                    TCharacter = npcImages[j];
                }
            }

            int RightHeighlignt = int.Parse(Data[i]["���� 2D �Ϸ���Ʈ ����"].ToString());

            NPCTalkClass temp = new NPCTalkClass(_index, name, _type, dialog, LCharacter, LeftHeighlignt, TCharacter, RightHeighlignt);


            NPCTalkList.Add(temp);
        }
    }
    public List<NPCTalkClass> GetCPNTalk() { return NPCTalkList; }

    //��� ������Ʈ ���̺� ����
    public void SetBGObjectTable()
    {
        Data = CSVReader.Read(CSVPath + "BackgroundObjectTable");

        for(int i = 0; i < Data.Count; i++)
        {
            string name = Data[i]["������Ʈ ���ϸ�"].ToString();

            if (name != "ObjectFileName" && name != "String")
            {
               BGDetail temp = new BGDetail(int.Parse(Data[i]["������ȣ"].ToString()),
                    int.Parse(Data[i]["������Ʈ Ÿ��"].ToString()),
                    ReturnObjName(Data[i]["������Ʈ �̸�"].ToString()), 
                    Data[i]["������Ʈ ���ϸ�"].ToString(),
                    Data[i]["3D ȸ�� �� ���"].ToString(),
                    Data[i]["�κ��丮 ȹ�� ���"].ToString(),
                    Data[i]["���� ���"].ToString(),
                    Data[i]["���� ���ϸ�1"].ToString(),
                    Data[i]["���� ���ϸ�2"].ToString(),
                    Data[i]["����1"].ToString(),
                    Data[i]["����2"].ToString(),
                    Data[i]["����3"].ToString());

                if(temp != null)
                {
                    //���� ���� �κ� ����
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
                    //���� ���� �κ� ��
                }

                bGDetails.Add(temp);
            }
        }
    }


    //�뷱�� ���̺� ����
    public void SetValueBalanceTable()
    {
        Data = CSVReader.Read(CSVPath + "ValueBalanceTable");

        for (int i = 2; i < Data.Count; i++)
        {
            BalanceClass temp = new BalanceClass(int.Parse(Data[i]["��ȣ"].ToString()),
                int.Parse(Data[i]["��ġ�� Ÿ��"].ToString()),
                int.Parse(Data[i]["��ġ��"].ToString()));

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
        Debug.Log("������ ���̺� ���� ����");
        return 0;
    }


    //CutScene ���̺� ��������
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
                if (cutSceneArr[j].name == Data[i]["�̹��� ���ϸ�"].ToString())
                {
                    cutSceneNum = j;
                }
            }

            for (int j = 0; j < audioclipArr.Length; j++)
            {
                if(Data[i]["���� ���ϸ�"].ToString() == "NULL")
                {
                    audioclipArr[j].name = "NULL";
                    break;
                }
                if (audioclipArr[j].name == Data[i]["���� ���ϸ�"].ToString())
                {
                    cutSceneAidioClipNum = j;
                }
            }
            CutSceneClass temp = new CutSceneClass(int.Parse(Data[i]["��ȣ"].ToString()),
                int.Parse(Data[i]["�̹��� Ÿ��"].ToString()),
                cutSceneArr[cutSceneNum],//�̹��� ��������Ʈ ��������
                audioclipArr[cutSceneAidioClipNum]);//�̹��� ����Ŭ�� ��������

            CutSceneLists.Add(temp);
        }
    }
    public List<CutSceneClass> GetCutSceneLists() { return CutSceneLists; }

    //GetDataObject �ѱ��̸� ������ ���� �κ� = ��������
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
