using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.UI.CanvasScaler;

public class SceneInit : MonoBehaviour
{
    [SerializeField]
    CanvasRenderer Fade;

    [SerializeField]
    float fadeTime = 5.0f;

    [SerializeField]
    float fadeDuration = 0.0f;

    void Start()
    {

    }

    IEnumerator SceneFade()
    {
        while (fadeDuration < fadeTime)
        {
            Fade.SetAlpha(Mathf.Lerp(1f, 0f, fadeDuration / fadeTime));
            fadeDuration += Time.deltaTime;
            yield return null;
        }

        Manager.Instance.Setting();
        transform.GetComponent<Synthesis>().Init();
        Manager.UIManager_Instance.SpawnRenderView();
        Manager.UIManager_Instance.UIPopup("UI_ChatNPC");

        //뭔가 더 추가할것들 추가하기
        Destroy(Fade.transform.parent);
        yield break;
    }
}
