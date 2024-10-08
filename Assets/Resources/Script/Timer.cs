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

            // ��ü �ð��� 60�� ���� Ŭ ��
            if (setTime >= 60f)
            {
                // 60���� ������ ����� ���� �д����� ����
                min = (int)setTime / 60;
                // 60���� ������ ����� �������� �ʴ����� ����
                sec = setTime % 60;

                if (sec < 10)
                {
                    gameTime.text = min + " : 0" + (int)sec;
                }
                else
                {
                    // UI�� ǥ�����ش�
                    gameTime.text = min + " : " + (int)sec;
                }

            }

            // ��ü�ð��� 60�� �̸��� ��
            if (setTime < 60f)
            {
                // �� ������ �ʿ�������Ƿ� �ʴ����� ������ ����
                /*if (sec < 10)
                {
                    gameTime.text = min + " : 0" + (int)setTime;
                    yield return null;
                }*/
                gameTime.text = ((int)setTime).ToString();
            }

            // ���� �ð��� 0���� �۾��� ��
            if (setTime <= 0)
            {
                // UI �ؽ�Ʈ�� 0�ʷ� ������Ŵ.
                gameTime.text = "0";
                GameObject.Find("EventSystem").GetComponent<NPCTalk>().StartNPCTalk(3);

                yield break;
            }
            yield return null;
        }
    }
}
