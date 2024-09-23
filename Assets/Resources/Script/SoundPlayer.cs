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
            //Todo:여기 볼륨설정 나중에 설정창이랑 연동할꺼면 데이터 저장 로드기능 구현이랑 그거 환경설정에 적용하고 환경설정값 가져와야함
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
