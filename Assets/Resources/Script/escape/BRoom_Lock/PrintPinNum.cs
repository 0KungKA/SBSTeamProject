using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PrintPinNum : MonoBehaviour
{
    public GameObject[] PinText;

    [SerializeField]
    string Vpin;

    [SerializeField]
    string key = "1011";

    public bool PinBool = true;//자기가 입력을 받을 준비가 됬는지 알려주는 변수(true = 비밀번호 안적힘 / false = 비밀번호 적힘)

    public void setPin(string val) { Vpin = val; }
    public void PushPin() { transform.parent.transform.parent.GetComponent<PrintPinNum>().setPin(Vpin); PinUpdate(); }

    private void OnEnable()
    {
        if(transform.name == "PinBtn")
        {
            for (int i = 0; i < PinText.Length; i++)
            {
                PinText[i] = transform.GetChild(0).transform.GetChild(i).transform.GetChild(1).gameObject;
            }
        }
    }

    public void PinUpdate()
    {
        PrintPinNum PinParent = transform.parent.transform.parent.GetComponent<PrintPinNum>();
        for(int i = 0; i <  PinText.Length; i++)
        {
            if(PinParent.PinText[i].GetComponent<PrintPinNum>().PinBool)
            {
                PinParent.PinText[i].GetComponent<Text>().text = Vpin[0].ToString();
                PinParent.PinText[i].GetComponent<PrintPinNum>().PinBool = false;
                break;
            }
        }
    }

    public void PinCheak()
    {
        string temp = "";
        for(int i = 0; i < PinText.Length; i ++)
        {
            temp += PinText[i].GetComponent<Text>().text;
        }
        if(temp == key)
        {
            GameObject.Find("BRoomClosetLock").GetComponent<Mission>().MissionDelete();
            Manager.UIManager_Instance.CloseUI();
        }
    }

    public void PinReset()
    {
        for (int i = 0; i < PinText.Length; i++)
        {
            PinText[i].GetComponent<Text>().text = "0";
            PinText[i].GetComponent<PrintPinNum>().PinBool = true;
        }
    }
}
