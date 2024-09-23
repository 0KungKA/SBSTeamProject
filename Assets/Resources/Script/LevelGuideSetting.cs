using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelGuideSetting : MonoBehaviour
{
    [SerializeField]
    string GuideString;

    [SerializeField]
    string MissionString;

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


    private void StartSetting()
    {
        GuideGO = GameObject.Find("UI_Scene_Main").transform.Find("Guide").gameObject;
        MissionGO = GameObject.Find("UI_Scene_Main").transform.Find("Mission").gameObject;

        GuideString = GuideString.Replace("\\n", "\n");
        MissionString = MissionString.Replace("\\n", "\n");
    }

    private void OnTriggerEnter(Collider other)
    {
        StartSetting();
        if (other.transform.name == "Player_Camera")
        {
            GuideGO.GetComponent<Text>().text = GuideString;
            MissionGO.GetComponent<Text>().text = MissionString;
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
