using System.Collections;
using System.Collections.Generic;
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

    public ChatData()
    {
    }

    public ChatData(int number, string name, string detail, Sprite Limg, int Lhighlight, Sprite Rimg, int Rhighlight)
    {
        Number = number;
        Name = name;
        Detail = detail;
        LImg = Limg;
        LHighlight = Lhighlight;
        RImg = Rimg;
        RHighlight = Rhighlight;
    }
}
