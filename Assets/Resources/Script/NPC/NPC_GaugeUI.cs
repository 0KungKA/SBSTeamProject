using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC_GaugeUI : MonoBehaviour
{
    //이벤트 시스템에 배치할거임
    bool OnGauge = false;//B방에서는 게이지가 차면 안되서 분리
    public void SetOnGauge(bool value) {  OnGauge = value; }
    public bool GetOnGauge() {  return OnGauge; }

    float NPC_CM_F_Gauge;//아그네스 전용 게이지
    public float GetGaugeF() { return NPC_CM_F_Gauge; }//가져오기
    public void SetGaugeF(float value) {  NPC_CM_F_Gauge = value;}//넣기 <-얘는 쓸일없음

    float NPC_CM_M_Gauge;//케일럽 전용 게이지
    public float GetGaugeM() { return NPC_CM_M_Gauge; }//가져오기
    public void SetGaugeM(float value) { NPC_CM_M_Gauge = value;}//넣기 <- 얘는 트리거이벤트 스크립트에서 1번씀

    float TimePoint = 4;//목표값
    float GaugeTimeDuration = 0;//현재시간
    int tickTimeGauge;//증가

    //현재시간이 4초 이상이 되면 게이지를 증가시킴 증가값은 데이터 테이블에서 끌어옴

    // Start is called before the first frame update
    void Start()
    {
        tickTimeGauge = Manager.DataManager_Instance.GetBalanceValue(2);
    }

    // Update is called once per frame
    void Update()
    {
        if(OnGauge == false) { return; }

        if (GaugeTimeDuration > TimePoint)
        {
            UpGauge(tickTimeGauge);
            GaugeTimeDuration = 0;
        }
        else
            GaugeTimeDuration += Time.deltaTime;

        if (NPC_CM_F_Gauge >= 1)
        {

            GameObject npc_cm_f = Resources.Load<GameObject>("Prefep/Object/NPC/NPC_CM_M");
            npc_cm_f = Instantiate(npc_cm_f).gameObject;
            npc_cm_f.transform.position = GameObject.Find("NPC_Spawn_Point").transform.position;
            npc_cm_f.GetComponent<NPCM_AI_Ctrl>().SetState(2);
            NPC_CM_F_Gauge = 0;
        }
    }

    public void UpGauge(int value)
    {
        NPC_CM_F_Gauge += value * 0.01f;
    }

}
