using System.Collections;
using System.Collections.Generic;
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
        if (other.transform.tag == "MainCamera")
        {
            GuideGO.GetComponent<Text>().text = GuideString;
            MissionGO.GetComponent<Text>().text = MissionString;
        }
    }
}
