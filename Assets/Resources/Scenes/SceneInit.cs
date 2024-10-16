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
    GameObject Fade;
    CanvasRenderer _Fade;
    //CanvasRenderer Fade;

    [SerializeField]
    float fadeTime = 5.0f;

    [SerializeField]
    float fadeDuration = 0.0f;

    void Start()
    {

    }

    IEnumerator SceneFade()
    {
        Manager.UIManager_Instance.UIPopup("Scene_UI/" + Fade.name);
        _Fade = Manager.UIManager_Instance.ReturnUIObj(Fade.name).GetComponent<CanvasRenderer>();

        fadeDuration = 0;
        while (fadeDuration < fadeTime)
        {
            _Fade.SetAlpha(Mathf.Lerp(1f, 0f, fadeDuration / fadeTime));
            fadeDuration += Time.deltaTime;
            yield return null;
        }

        transform.GetComponent<Synthesis>().Init();
        Manager.Instance.Setting();
        Manager.UIManager_Instance.SpawnRenderView();
        GameObject.Find("EventSystem").GetComponent<NPCTalk>().StartNPCTalk(1);

        //뭔가 더 추가할것들 추가하기
        Manager.UIManager_Instance.CloseUI(_Fade.name);
        yield break;
    }
}
