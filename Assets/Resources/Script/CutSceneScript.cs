using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutSceneScript : MonoBehaviour
{
    List<CutSceneClass> cutScenes = new List<CutSceneClass>();
    AudioClip[] audioclipArr;

    [SerializeField]
    float CutTime = 3.0f;
    [SerializeField]
    float CutDuration = 0.0f;

    [SerializeField]
    GameObject _Canvas;

    private void Start()
    {
        cutScenes = Manager.DataManager_Instance.GetCutSceneLists();
        audioclipArr = Resources.LoadAll<AudioClip>("0.Sound");
        PlayeCutScene(1);
    }

    /// <summary>
    /// 1 = ������ , 2 = ����
    /// </summary>
    /// <param name="value"></param>
    public void PlayeCutScene(int value)
    {
        StartCoroutine(CutScene(value));
    }

    IEnumerator CutScene(int value)
    {
        int startPoint = -1;
        int number = 0;
        int cutSceneCount = 0;

        GameObject canvas = Instantiate(_Canvas);
        canvas.name = Manager.UIManager_Instance.DeletClone(_Canvas.name);
        canvas.gameObject.SetActive(true);
        canvas.transform.SetAsLastSibling();

        // Canvas�� Image ������Ʈ�� �̸� ĳ��
        Image cutSceneImage = canvas.transform.GetChild(0).GetComponent<Image>();

        EffectSoundPlayer soundPlayer = Manager.Effect_SoundPlayer;

        // cutScenes���� �־��� value Ÿ���� �ƽ� ã��
        for (int i = 0; i < cutScenes.Count; i++)
        {
            if (cutScenes[i].type == value)
            {
                if (startPoint == -1)
                    startPoint = i; // ù ��°�� ��ġ�ϴ� �ε��� ����
                cutSceneCount++;
            }
        }

        // ���� ó��: �ش� Ÿ���� �ƽ��� ���� ��� �ڷ�ƾ ����
        if (cutSceneCount == 0)
        {
            yield break;
        }


        // �ƽ� ��� ����
        while (number < cutSceneCount)
        {
            // �� �ƽ��� ���� ������ �ð� ��� ó��
            cutSceneImage.sprite = cutScenes[startPoint + number].img;

            if (cutScenes[startPoint + number].soundSourceName != "Null")
            {
                for (int i = 0; i < audioclipArr.Length; i++)
                {
                    if (cutScenes[startPoint + number].soundSourceName == audioclipArr[i].name)
                    {
                        soundPlayer.EffectSoundPlay(audioclipArr[i]);
                    }
                }
            }

            while (CutDuration < CutTime)
            {

                // Space Ű�� �ƽ� ��ŵ ó��
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    CutDuration = CutTime + Time.deltaTime;
                }
                else
                {
                    CutDuration += Time.deltaTime;
                }

                yield return null;  // ���� ���������� �ѱ��
            }

            // ���� �ƽ����� �Ѿ��
            number++;
            CutDuration = 0.0f;  // ���� �ƽ��� ���� �ʱ�ȭ
            yield return null;  // �� ������ ���
        }

        bool EndingScenePlay = false;
        if(value == 1)
        {
            transform.GetComponent<Synthesis>().Init();
            GetComponent<SceneInit>().StartCoroutine("SceneFade");
        }
        else if(value == 2)
        {
            float endingDuration = 0;
            EndingScenePlay = true;
            while (true)
            {
                if (endingDuration > 5)
                {
                    Manager.UIManager_Instance.UIPopup("Scene_UI/UI_Scene_Credit");
                    Destroy(canvas);
                    yield break;
                }
                else
                    endingDuration += Time.deltaTime;

                yield return null;
            }
        }

        // Canvas ����
        if(EndingScenePlay == true)
        {
            yield break;
        }
        else
        {
            Destroy(canvas);
            yield break;  // �ڷ�ƾ ����
        }
        
    }
}
