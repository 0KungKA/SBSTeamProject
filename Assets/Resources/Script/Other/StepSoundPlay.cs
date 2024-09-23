using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSoundPlay : MonoBehaviour
{
    [SerializeField]
    AudioClip water_Layer;
    [SerializeField]
    AudioClip NormalStepSound;

    private void Update()
    {
        Vector3 pos = GameObject.Find("Player_Camera").transform.position;
        pos.y = transform.position.y;

        transform.position = pos;
        pos = -transform.up;

        int layerMask = (1 << LayerMask.NameToLayer("Water")) | (1 << LayerMask.NameToLayer("A"));

        float H = Input.GetAxis("Horizontal");
        float V = Input.GetAxis("Vertical");
        if(H != 0 || V != 0  )
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, pos, out hit, 5.0f, layerMask))
            {
                Debug.DrawLine(transform.position, hit.point, Color.red, 0.1f);
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Water"))
                {
                    Manager.Effect_SoundPlayer.EffectSoundPlay(water_Layer);
                }
                else
                {
                    Manager.Effect_SoundPlayer.EffectSoundPlay(NormalStepSound);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.name == "D_GuideAndMission")
        {
            GameObject.Find("EventSystem").transform.GetComponent<NPC_GaugeUI>().SetOnGauge(true);
            GameObject.Find("NPC_CM_M").transform.GetComponent<NPCM_AI_Ctrl>().saveMove = false;
        }
    }
}
