using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSoundPlayer : MonoBehaviour
{
    AudioSource[] As;//원본 오디오 사운드 효과음

    AudioSource[] PlayerList;//컴포넌트에 추가한 오디오 사운드(플레이중일땐 오디오가 끊기니까 새로 만들어서 집어넣음

    void Start()
    {
        
    }
    public void PlaySound(string SoundName)
    {
        for(int i = 0; i < As.Length; i++)
        {
            if(As[i].clip.ToString() == SoundName)
            {
                //AudioSource temp = 
            }
        }
    }
}
