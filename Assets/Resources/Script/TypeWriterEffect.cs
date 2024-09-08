using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TypeWriterEffect : MonoBehaviour
{
    [SerializeField]
    AudioSource effectAudio;

    //변경할 변수
	public float delay;
    public float Skip_delay;
    public int cnt;

    //타이핑효과 변수
    public string[] fulltext;
    public int dialog_cnt;
    string currentText;

    //타이핑확인 변수
    public bool text_exit;
    public bool text_full;
    public bool text_cut;

    [SerializeField]
    Text npcNameTextField;
    string[] npcName;

    //시작과 동시에 text분류하고 타이핑시작
    void Start()
    {
        GameObject origin = Manager.Origin_Object;
        if(origin != null )
        {
            if(origin.GetComponent<ItemInfo>().ItemExplanatino != null)
                fulltext[0] = origin.GetComponent<ItemInfo>().ItemExplanatino;
        }
        if (fulltext.Length > 1)
        {
            npcName = new string[fulltext.Length];
            TextClassification();
        }
        else
        {
            for (int i = 0; i < fulltext.Length; i++)
                fulltext[i] = fulltext[i].Replace("\\n", "\n");
        }
        dialog_cnt = fulltext.Length;
        Get_Typing(dialog_cnt,fulltext);
    }

    void TextClassification()
    {
        for(int i = 0; i < fulltext.Length; i++)
        {
            fulltext[i] = fulltext[i].Replace("\\n", "\n");

            int subnum = fulltext[i].IndexOf(":");
            if(subnum != -1) 
            {
                string SubnpcName = fulltext[i].Substring(0, subnum);
                fulltext[i] = fulltext[i].Substring(subnum + 1, fulltext[i].Length - subnum - 1);
                if (SubnpcName == "null")
                {
                    npcName[i] = "";
                }
                npcName[i] = SubnpcName;
            }
        }
    }


    //모든 텍스트 호출완료시 탈출
    void Update()
    {
        if(text_exit==true && this != null)
        {
            //gameObject.SetActive(false);
            StopAllCoroutines();
            Manager.UIManager_Instance.CloseUI();
        }
    }

    //다음버튼함수
    public void End_Typing()
    {
        //다음 텍스트 호출
        if (text_full == true)
        {
            cnt++;
            text_full = false;
            text_cut = false;
            StartCoroutine(ShowText(fulltext));
        }
        //텍스트 타이핑 생략
        else
        {
            text_cut = true;
        }
    }

    //텍스트 시작호출
    public void Get_Typing(int _dialog_cnt, string[] _fullText)
    {
        //재사용을 위한 변수초기화
        text_exit = false;
        text_full = false;
        text_cut = false;
        cnt = 0;

        //변수 불러오기
        dialog_cnt = _dialog_cnt;
        fulltext = new string[dialog_cnt];
        fulltext = _fullText;

        //타이핑 코루틴시작
        StartCoroutine(ShowText(fulltext));
    }

    IEnumerator ShowText(string[] _fullText)
    {
        //모든텍스트 종료
        if (cnt >= dialog_cnt)
        {
            text_exit = true;
            StopCoroutine("showText");
        }
        else
        {
            if(npcName != null)
                npcNameTextField.text = npcName[cnt];

            //기존문구clear
            currentText = "";
            //타이핑 시작
            for (int i = 0; i < _fullText[cnt].Length; i++)
            {
                if(effectAudio != null)
                    effectAudio.Play();

                //타이핑중도탈출
                if (text_cut == true)
                {
                    break;
                }
                //단어하나씩출력
                currentText = _fullText[cnt].Substring(0, i + 1);
                this.GetComponent<Text>().text = currentText;
                yield return new WaitForSeconds(delay);
            }
            //탈출시 모든 문자출력
            Debug.Log("Typing 종료");
            this.GetComponent<Text>().text = _fullText[cnt];
            yield return new WaitForSeconds(Skip_delay);

            //스킵_지연후 종료
            Debug.Log("Enter 대기");
            text_full = true;
        }
    }
}
