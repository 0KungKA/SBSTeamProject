using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneScript : MonoBehaviour
{
    Sprite[] CutSceneLists;

    List<Sprite> OpCutScenes = new List<Sprite>();
    List<Sprite> EdCutScenes = new List<Sprite>();

    [SerializeField]
    float CutTime = 5.0f;
    [SerializeField]
    float CutDuration = 0.0f;

    [SerializeField]
    GameObject Canvas;

    int number = 0;

    private void Start()
    {
        CutSceneLists = GameObject.Find("EventSystem").GetComponent<DataManager>().GetCutSceneLists();
        ClassifyCutScene();//�ƾ� �з��۾� ����
        StartCoroutine(OpCutScene());
    }

    private void CreateCutSceneCanvas()
    {
        Instantiate(Canvas);
    }

    private void ClassifyCutScene()//�ƽ� �з��۾�
    {
        for (int i = 0; i < CutSceneLists.Length; i++)
        {
            if (CutSceneLists[i].name.Contains("Op"))
            {
                OpCutScenes.Add(CutSceneLists[i]);
            }
            else if (CutSceneLists[i].name.Contains("Ed"))
            {
                EdCutScenes.Add(CutSceneLists[i]);
            }
        }
        NumberingClassify();
    }

    public void StartCutScene(string Tag)
    {
        if(Tag == "Op")
        {
            StartCoroutine(OpCutScene());
        }
    }

    private void NumberingClassify()//������ �ƾ� ������� ����
    {
        List<Sprite> temp = new List<Sprite>();

        for(int i = 0; i < OpCutScenes.Count; i++)
        {
            for (int j = 0; j < OpCutScenes.Count; j++) 
            {
                string demper = Regex.Replace(OpCutScenes[j].name, @"\D", "");

                bool Search = (i == int.Parse(demper)) ? true : false;

                if (Search)
                {
                    temp.Add(OpCutScenes[j]);
                    break;
                }
            }
        }
        OpCutScenes = temp;

        temp = null;
        for (int i = 0; i < EdCutScenes.Count; i++)
        {
            for (int j = 0; j < EdCutScenes.Count; j++)
            {
                if (EdCutScenes[j].name.Contains(j.ToString()))
                {
                    temp.Add(EdCutScenes[j]);
                }
            }
        }
        EdCutScenes = temp;
    }

    IEnumerator OpCutScene()
    {
        while (number < OpCutScenes.Count)
        {
            Debug.Log(transform.name + "while �۵���");
            while (CutDuration < CutTime)
            {
                Debug.Log(transform.name + "while �۵���");
                Canvas.transform.GetChild(0).GetComponent<Image>().sprite = OpCutScenes[number];
                CutDuration += Time.deltaTime;
                yield return null;
            }
            number++;
            CutDuration = 0.0f;
            yield return null;
        }

        transform.GetComponent<Synthesis>().Init();
        GetComponent<SceneInit>().StartCoroutine("SceneFade");

        //���� �� �߰��Ұ͵� �߰��ϱ�
        Destroy(Canvas);
        yield break;
    }
}
