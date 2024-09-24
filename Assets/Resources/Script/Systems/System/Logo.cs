using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logo : MonoBehaviour
{
    [SerializeField]
    CanvasRenderer[] Fades;

    [SerializeField]
    float fadeTime = 5.0f;

    [SerializeField]
    float fadeDuration = 0.0f;

    [SerializeField]
    GameObject BG;

    int FadeNumber = 0;

    void Awake()
    {
        for(int i = 0; i < Fades.Length; i++)
        {
            Fades[i].GetComponent<CanvasRenderer>().SetAlpha(0);
        }
        StartCoroutine("SceneFade");
    }

    IEnumerator SceneFade()
    { 
        while (true)
        {
            while (fadeDuration < fadeTime)
            {
                Fades[FadeNumber].transform.parent.GetComponent<Canvas>().sortingOrder = 1;
                //Fades[FadeNumber].SetAlpha(Mathf.Lerp(1f, 0f, fadeDuration / fadeTime));
                Fades[FadeNumber].SetAlpha(Mathf.Lerp(0f, 1f, fadeDuration / fadeTime));
                fadeDuration += Time.deltaTime;
                yield return null;
            }

            if (fadeDuration > fadeTime)
            {
                fadeDuration = 0;
                Destroy(Fades[FadeNumber].transform.parent.gameObject);
                FadeNumber++;

                if (FadeNumber >= Fades.Length)
                {
                    Destroy(BG);
                    SceneManager.LoadScene("Lobby");
                    yield break;
                }
                else
                    yield return null;
            }
        }
    }
}
