using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Windows;

public class ChatData : MonoBehaviour
{
    //대화 구분 / 이름 / 대화내용 / 좌측NPC이미지 / 좌측강조 / 우측NPC이미지 / 우측강조)
    //public List<int, string, string, Sprite, int, Sprite, int> ChatDetail;

    int Number;
    string Name;
    string Detail;
    Sprite LImg;
    int LHighlight;
    Sprite RImg;
    int RHighlight;

    string NPCpath = "Assets/Resources/Material/ObjectTexture/NPC/";

    //CharacterData CharacterData;
    CharacterData CharacterData = new CharacterData();

    private void Start()
    {
        //CharacterData = new CharacterData();
        CharacterData.Init();
    }

    public void SetChatData(int number, string name, string detail, string LimgName, int Lhighlight, string RimgName, int Rhighlight)
    {
        /*Number = number;
        Name = name;
        Detail = detail;
        LImg = ImgLoad(LimgName);
        LHighlight = Lhighlight;
        RImg = ImgLoad(RimgName);
        RHighlight = Rhighlight;*/
    }

}
