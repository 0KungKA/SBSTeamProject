using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSoundPlayer : MonoBehaviour
{
    AudioSource[] As;//���� ����� ���� ȿ����

    AudioSource[] PlayerList;//������Ʈ�� �߰��� ����� ����(�÷������϶� ������� ����ϱ� ���� ���� �������

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
