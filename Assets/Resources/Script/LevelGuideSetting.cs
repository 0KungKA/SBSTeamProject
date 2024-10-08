using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelGuideSetting : MonoBehaviour
{
    GameObject GuideGO;
    GameObject MissionGO;

    [Tooltip("만약 방에 도착했을때 바꿔야하는 가이드 또는 미션이 있으면 교체")]
    [SerializeField]
    GameObject ChangeTarget;
    [Tooltip("교체할 가이드 내용")]
    [SerializeField]
    string ChangeTargetGuideString;

    [Tooltip("교체할 미션 내용")]
    [SerializeField]
    string ChangeTargetMissionString;

    [SerializeField]
    string GuideString;
    public void SetGuideString(string value) { GuideGO.GetComponent<Text>().text = value; }

    [SerializeField]
    string MissionString;
    public void SetMissionString(string value) { MissionGO.GetComponent<Text>().text = value; }

    private void Start()
    {
        GuideGO = GameObject.Find("UI_Scene_Main").transform.Find("Guide").gameObject;
        MissionGO = GameObject.Find("UI_Scene_Main").transform.Find("Mission").gameObject;
    }

    private void StartSetting()
    {
        GuideGO = GameObject.Find("UI_Scene_Main").transform.Find("Guide").gameObject;
        MissionGO = GameObject.Find("UI_Scene_Main").transform.Find("Mission").gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        StartSetting();
        if (other.transform.name == "Player_Camera")
        {
            GuideGO.GetComponent<Text>().text = GuideString;
            MissionGO.GetComponent<Text>().text = MissionString;

            GuideGO.GetComponent<Text>().text = GuideGO.GetComponent<Text>().text.Replace("\\n","\n");
            MissionGO.GetComponent<Text>().text = MissionGO.GetComponent<Text>().text.Replace("\\n","\n");
        }

        if(ChangeTarget != null)
        {
            if(ChangeTargetGuideString != null)
                ChangeTarget.GetComponent<LevelGuideSetting>().GuideString = ChangeTargetGuideString;
            if(ChangeTargetMissionString != null)
                ChangeTarget.GetComponent<LevelGuideSetting>().MissionString = ChangeTargetMissionString;
        }
    }

    public void GMChanged()
    {
        if (ChangeTarget != null)
        {
            if (ChangeTargetGuideString != null)
                ChangeTarget.GetComponent<LevelGuideSetting>().GuideString = ChangeTargetGuideString;
            if (ChangeTargetMissionString != null)
                ChangeTarget.GetComponent<LevelGuideSetting>().MissionString = ChangeTargetMissionString;
        }
    }
}
