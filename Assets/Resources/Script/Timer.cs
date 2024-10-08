using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    Text gameTime;

    [SerializeField]
    float setTime;

    int min = 0;
    float sec = 0;

    private void Start()
    {
        setTime = Manager.DataManager_Instance.GetBalanceValue(1);
    }

    internal void StartTimer()
    {
        StartCoroutine(TimerSet());
    }

    IEnumerator TimerSet()
    {
        while (true)
        {
            if (GameObject.Find("UI_ChatNPC") != null && GameObject.Find("_Canvas") != null && GameObject.Find("UI_CutScene") != null)
            {
                yield return null;
            }

            setTime -= Time.deltaTime;

            // 전체 시간이 60초 보다 클 때
            if (setTime >= 60f)
            {
                // 60으로 나눠서 생기는 몫을 분단위로 변경
                min = (int)setTime / 60;
                // 60으로 나눠서 생기는 나머지를 초단위로 설정
                sec = setTime % 60;

                if (sec < 10)
                {
                    gameTime.text = min + " : 0" + (int)sec;
                }
                else
                {
                    // UI를 표현해준다
                    gameTime.text = min + " : " + (int)sec;
                }

            }

            // 전체시간이 60초 미만일 때
            if (setTime < 60f)
            {
                // 분 단위는 필요없어지므로 초단위만 남도록 설정
                /*if (sec < 10)
                {
                    gameTime.text = min + " : 0" + (int)setTime;
                    yield return null;
                }*/
                gameTime.text = ((int)setTime).ToString();
            }

            // 남은 시간이 0보다 작아질 때
            if (setTime <= 0)
            {
                // UI 텍스트를 0초로 고정시킴.
                gameTime.text = "0";
                GameObject.Find("EventSystem").GetComponent<NPCTalk>().StartNPCTalk(3);

                yield break;
            }
            yield return null;
        }
    }
}
