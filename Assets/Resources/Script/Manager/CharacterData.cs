using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    public class TData
    {
        public int index;
        public int OriginalNumber;
        public int CharacterType;
        public string CharacterName;
        public GameObject Modeling;
        public Sprite Char2DFile1;
        public Sprite Char2DFile2;
        public Sprite Char2DFile3;
    }

    List<TData> Tdata;

    List<Dictionary<string, object>> Data;
    string NPCpath = "Assets/Resources/Material/ObjectTexture/NPC/";

    public Sprite ImgLoad(string path)
    {
        return (Sprite)Resources.Load(NPCpath + path);
    }

    internal void Init()
    {
        Data = CSVReader.Read(Manager.DataManager_Instance.CSVPath + "CharacterTable");

        for(int i = 0; i < Data.Count; i++)
        {
            for(int j = 0; j < Data[i].Count;j++)
            {
                string temp = (string)Data[i]["��ȹ�� ������"];
                if (temp != "")
                {
                    TData addTemp = new TData();

                    addTemp.index = (int)Data[i]["��ȣ"];
                    addTemp.OriginalNumber = (int)Data[i]["ĳ���� Ÿ��"];
                    addTemp.CharacterType = (int)Data[i]["ĳ���� �̸�"];
                    addTemp.CharacterName = (string)Data[i]["ĳ���� �̸�"];
                    addTemp.Modeling = (GameObject)Data[i]["ĳ���� 3D �𵨸� ���ϸ�"];
                    addTemp.Char2DFile1 = (Sprite)Data[i]["ĳ���� 2D �Ϸ���Ʈ ���ϸ�1"];
                    addTemp.Char2DFile2 = (Sprite)Data[i]["ĳ���� 2D �Ϸ���Ʈ ���ϸ�2"];
                    addTemp.Char2DFile3 = (Sprite)Data[i]["ĳ���� 2D �Ϸ���Ʈ ���ϸ�3"];

                    Tdata.Add(addTemp);
                }
            }
        }
    }
}
