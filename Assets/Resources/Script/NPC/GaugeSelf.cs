using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeSelf : MonoBehaviour
{
    Image GaugeOBJself;
    NPC_GaugeUI NPC_GaugeUI;

    void Start()
    {
        NPC_GaugeUI = GameObject.Find("EventSystem").GetComponent<NPC_GaugeUI>();
        GaugeOBJself = GetComponent<Image>();
        GaugeOBJself.fillAmount = 0;
    }

    void Update()
    {
        if(transform.name == "CM_M_Gauge")
            GaugeOBJself.fillAmount = NPC_GaugeUI.GetGaugeM();
        else if(transform.name == "CM_F_Gauge")
            GaugeOBJself.fillAmount = NPC_GaugeUI.GetGaugeF();
    }
}
