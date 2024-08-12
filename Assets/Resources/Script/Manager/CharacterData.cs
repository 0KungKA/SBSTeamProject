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
                string temp = (string)Data[i]["기획자 참조용"];
                if (temp != "")
                {
                    TData addTemp = new TData();

                    addTemp.index = (int)Data[i]["번호"];
                    addTemp.OriginalNumber = (int)Data[i]["캐릭터 타입"];
                    addTemp.CharacterType = (int)Data[i]["캐릭터 이름"];
                    addTemp.CharacterName = (string)Data[i]["캐릭터 이름"];
                    addTemp.Modeling = (GameObject)Data[i]["캐릭터 3D 모델링 파일명"];
                    addTemp.Char2DFile1 = (Sprite)Data[i]["캐릭터 2D 일러스트 파일명1"];
                    addTemp.Char2DFile2 = (Sprite)Data[i]["캐릭터 2D 일러스트 파일명2"];
                    addTemp.Char2DFile3 = (Sprite)Data[i]["캐릭터 2D 일러스트 파일명3"];

                    Tdata.Add(addTemp);
                }
            }
        }
    }
}
