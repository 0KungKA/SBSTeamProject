using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField]
    AudioClip clip;

    public void PlaySound()
    {
        if (clip == null) return;

        else
            Manager.Effect_SoundPlayer.EffectSoundPlay(clip);
    }

    /*AudioSource source;

    public void Setting()
    {
        source = GetComponent<AudioSource>();
        if(source == null )
        {
            source = transform.AddComponent<AudioSource>();
            source.Stop();
            source.clip = null;
            //Todo:���� �������� ���߿� ����â�̶� �����Ҳ��� ������ ���� �ε��� �����̶� �װ� ȯ�漳���� �����ϰ� ȯ�漳���� �����;���
            source.volume = 0.1f;
            source.playOnAwake = false;
            source.maxDistance = 20.0f;
            source.loop = false;
        }
    }

    public void PlaySound()
    {
        if (clip != null)
        {
            Setting();
            if (source != null)
            {
                source.Play();
            }
            else
            {
                Setting();
                source.Play();
            }
        }
    }*/
}
