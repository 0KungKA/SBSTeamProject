using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC_GaugeUI : MonoBehaviour
{
    //�̺�Ʈ �ý��ۿ� ��ġ�Ұ���
    bool OnGauge = false;//B�濡���� �������� ���� �ȵǼ� �и�
    public void SetOnGauge(bool value) {  OnGauge = value; }
    public bool GetOnGauge() {  return OnGauge; }

    float NPC_CM_F_Gauge;//�Ʊ׳׽� ���� ������
    public float GetGaugeF() { return NPC_CM_F_Gauge; }//��������
    public void SetGaugeF(float value) {  NPC_CM_F_Gauge = value;}//�ֱ� <-��� ���Ͼ���

    float NPC_CM_M_Gauge;//���Ϸ� ���� ������
    public float GetGaugeM() { return NPC_CM_M_Gauge; }//��������
    public void SetGaugeM(float value) { NPC_CM_M_Gauge = value;}//�ֱ� <- ��� Ʈ�����̺�Ʈ ��ũ��Ʈ���� 1����

    float TimePoint = 4;//��ǥ��
    float GaugeTimeDuration = 0;//����ð�
    int tickTimeGauge;//����

    //����ð��� 4�� �̻��� �Ǹ� �������� ������Ŵ �������� ������ ���̺��� �����

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
