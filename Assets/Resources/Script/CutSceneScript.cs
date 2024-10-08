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
    /// 1 = 오프닝 , 2 = 엔딩
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

        // Canvas의 Image 컴포넌트를 미리 캐싱
        Image cutSceneImage = canvas.transform.GetChild(0).GetComponent<Image>();

        EffectSoundPlayer soundPlayer = Manager.Effect_SoundPlayer;

        // cutScenes에서 주어진 value 타입의 컷신 찾기
        for (int i = 0; i < cutScenes.Count; i++)
        {
            if (cutScenes[i].type == value)
            {
                if (startPoint == -1)
                    startPoint = i; // 첫 번째로 일치하는 인덱스 저장
                cutSceneCount++;
            }
        }

        // 예외 처리: 해당 타입의 컷신이 없는 경우 코루틴 종료
        if (cutSceneCount == 0)
        {
            yield break;
        }


        // 컷신 재생 루프
        while (number < cutSceneCount)
        {
            // 각 컷신이 끝날 때까지 시간 경과 처리
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

                // Space 키로 컷신 스킵 처리
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    CutDuration = CutTime + Time.deltaTime;
                }
                else
                {
                    CutDuration += Time.deltaTime;
                }

                yield return null;  // 다음 프레임으로 넘기기
            }

            // 다음 컷신으로 넘어가기
            number++;
            CutDuration = 0.0f;  // 다음 컷신을 위해 초기화
            yield return null;  // 한 프레임 대기
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

        // Canvas 삭제
        if(EndingScenePlay == true)
        {
            yield break;
        }
        else
        {
            Destroy(canvas);
            yield break;  // 코루틴 종료
        }
        
    }
}
