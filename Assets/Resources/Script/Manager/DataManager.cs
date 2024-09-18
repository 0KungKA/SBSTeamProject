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

public class DataManager : MonoBehaviour
{
    public string CSVPath = "Script/Systems/CSV_File/";

    List<Dictionary<string, object>> Data;

    ChatData chatList = new ChatData();

    //��Ʈ�� ���̺� Start
    List<Detail> KOobjectName = new List<Detail>();
    List<Detail> KOCharacterName = new List<Detail>();
    List<Detail> KOInteractionDetail = new List<Detail>();
    List<Detail> KOchatDetail = new List<Detail>();
    List<Detail> KOUIDetail = new List<Detail>();
    //��Ʈ�� ���̺� End

    Sprite[] CutSceneLists;
    public Sprite[] GetCutSceneLists() {  return CutSceneLists; }

    List<BGDetail> bGDetails = new List<BGDetail>();

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

        //��������Ʈ ���̺� ����
        SetBGObjectTable();
        //��������Ʈ ���̺� ��

        SetCutSceneIMG();//�ƾ� ��������Ʈ ��������
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
    //��� ������Ʈ ���̺� ��

    //������Ʈ �̸�
    //������Ʈ ����
    //ĳ���� �̸�
    //ĳ���� ��ȭ ����
    //UI

    //CutScene ���̺� ��������
    public void SetCutSceneIMG()
    {
        CutSceneLists = null;
        CutSceneLists = Resources.LoadAll<Sprite>("0.UI/UITex/CutScene");
    }

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
