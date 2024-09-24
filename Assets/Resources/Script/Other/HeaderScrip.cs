using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeaderScrip : MonoBehaviour
{
    [SerializeField]
    MannequinCheck[] mannequins;

    [SerializeField]
    MannequinCheck[] nonmannequins;


    [SerializeField]
    Material[] matGroup;

    [SerializeField]
    AudioClip[] Clips;

    GameObject[] lightGo;
    [SerializeField]
    Light[] lightGroup;

    [SerializeField]
    GameObject[] GM;//���̵�� �̼� ���� Ż���ؾ��Ѵ� �� �ٲܰ���

    private void Start()
    {
        lightGo = GameObject.FindGameObjectsWithTag("Light");
        lightGroup = new Light[lightGo.Length];
        for (int i = 0; i < lightGo.Length; i++)
        {
            lightGroup[i] = lightGo[i].transform.GetComponent<Light>();
        }
    }

    private void Update()
    {

        int mValue = 0;
        for(int i = 0; i < mannequins.Length; i ++)
        {
            if (mannequins[i].mannequinCheck)
                mValue++;
        }

        int nmValue = 0;
        for (int i = 0; i < nonmannequins.Length; i++)
        {
            if (nonmannequins[i].mannequinCheck == true)
                nmValue++;
        }

        if (nmValue > 0)
        {
            return;
            
        }
        else if (mValue == mannequins.Length)
        {
            Manager.Origin_Object = this.gameObject;
            Manager.Call_Object = gameObject.GetComponent<Mission>().ClearTarget;
            transform.GetComponent<Mission>().MissionClearSelf();
            GameObject PC = GameObject.Find("Player_Camera").gameObject;
            PC.GetComponent<CameraMove>().AddSpeed(0.25f);

            AudioSource PCAS = PC.GetComponent<AudioSource>();
            PCAS.loop = true;
            PCAS.pitch = 1.4f;
            PCAS.volume = 1.0f;
            PCAS.Play();

            //���������� �κ�
            for (int i = 0; i < Clips.Length; i++)
            {
                Manager.Effect_SoundPlayer.EffectSoundPlay(Clips[i]);
            }

            foreach (Light light in lightGroup)
            {
                Destroy(light.GetComponent<InstantLightCtrl>());
                light.color = Color.red;

                if(light.range != 0)
                    light.range = 80;

                light.intensity = 3;
            }
            foreach (Material material in matGroup)
            {
                material.color = Color.red;
            }
            

            int temp = GameObject.Find("GuideAndMissionGroup").transform.childCount;
            GM = new GameObject[temp];
            for (int i = 0; i < temp; i++)
            {
                GM[i] = GameObject.Find("GuideAndMissionGroup").transform.GetChild(i).gameObject;
                GM[i].GetComponent<LevelGuideSetting>().SetGuideString("Ż �� �� �� �� ��");
                GM[i].GetComponent<LevelGuideSetting>().SetMissionString("Ż �� �� �� �� ��");
            }
        }

    }
}
