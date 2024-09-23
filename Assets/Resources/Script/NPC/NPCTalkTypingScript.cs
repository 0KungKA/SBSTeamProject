using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//이 코드는 데이터 테이블 연동으로 만든 코드임
/*
클래스 구성요소
int index
string 이름
int 대화구분을 위한 type
string 대화내용

sprite 왼쪽 NPC 이미지
int 왼쪽 npc이미지 하이라이트 (0 = 살짝 어두움 / 1 = 기본)

sprite 오른쪽 NPC 이미지
int 왼쪽 npc이미지 하이라이트 (0 = 살짝 어두움 / 1 = 기본)
*/


//얘는 NPC Chat UI에 들어감
//UI에 직접적으로 들어가고 이걸 호출해주는 스크립트 따로 짜야함
public class NPCTalkTypingScript : MonoBehaviour
{
    [SerializeField]
    AudioSource effectAudio;

    //변경할 변수
    [Tooltip("출력해야할 텍스쳐가 자동으로 뜰때 얼마나 빨리 뜨게 할것인지")]
    [Range(0.01f,0.1f)]
    public float delay;
    [Tooltip("출력해야할 텍스쳐가 다뜨고 다음버튼을 눌러도 다음출력 동작안하게 할건지 / 0 = 안함")]
    [Range(0, 5.0f)]
    public float Skip_delay;

    //출력해야할 내용의 총 줄 수
    int cnt;

    //타이핑효과 변수
    string[] fulltext;

    //출력해야할 내용의 총 줄 수
    int dialog_cnt;
    string[]npcName;
    //public string[] fulltext; <- 외부에서 작업할떄 씀
    //public string[] npcName; <- 외부에서 작업할떄 씀
    //외부에서쓴다고 작업하면 일회용으로 밖에 사용못함

    [SerializeField]
    GameObject PObject;
    [SerializeField]
    GameObject LeftNPC;
    [SerializeField]
    GameObject RightNPC;

    string currentText;
    

    //타이핑확인 변수
    //출력해야할 내용이 다 나왔는지 여부
    public bool text_exit;
    //텍스트가 전부 출력됬는지 여부(아직 다 안됬는데 버튼 누르면 전체 출력할려고 만든 bool 변수)
    public bool text_full;
    public bool text_cut;

    //추가변수
    int type;
    int PlayerTalkSize;//이번 UI에서 몇개를 출력해야하는지에대한 변수
    int temp;//몇번째 부터 시작하는지에대한 변수

    List<NPCTalkClass> npcTalks = new List<NPCTalkClass>();

    [SerializeField]
    Text npcNameTextField;

    public void StartTalk(List<NPCTalkClass> _npcTalks, int _type,GameObject _go)//,int _talkSize
    {
        //Manager.UIManager_Instance.UIPush(_go);
        Manager.CM_Instance.SetMoveState(false);
        Manager.CM_Instance.SetRotState(false);
        Manager.CM_Instance.OnMouseCursor();

        for (int i = 0; i < _npcTalks.Count; i++)
        {
            if (_npcTalks[i].type == _type)
                PlayerTalkSize++;
        }

        Init(_npcTalks, _type);
    }


    void Init(List<NPCTalkClass> _npcTalks, int _type)
    {
        temp = 0;
        for (int i = 0; i < _npcTalks.Count; i ++)
        {
            if (_npcTalks[i].type == _type)
            {
                temp = i;//몇번쨰부터 시작하는지 알기위해 지역변수에 담아줌
                break;
            }
        }

        fulltext = new string[PlayerTalkSize];
        npcName = new string[PlayerTalkSize];

        if (_npcTalks != null)
        {
            npcTalks = _npcTalks;

            for (int i = 0; i < PlayerTalkSize; i++)
            {
                if (_npcTalks[temp + i].type == _type)
                {
                    fulltext[i] = _npcTalks[temp + i].DialogText;
                    npcName[i] = _npcTalks[temp + i].name;
                }
            }
        }
        Debug.Log("겟 타이핑 스타트");
        Debug.Log(dialog_cnt+" 사이즈");
        dialog_cnt = fulltext.Length;
        Get_Typing(dialog_cnt, fulltext);
    }

    /*void TextClassification()
    {
        for (int i = 0; i < fulltext.Length; i++)
        {
            fulltext[i] = fulltext[i].Replace("\\n", "\n");

            npcName[i] = npcTalks[i].name;
        }
    }*/


    //모든 텍스트 호출완료시 탈출
    void Update()
    {
        if (text_exit == true && this != null)
        {
            Debug.Log("코루틴 중지");
            Manager.CM_Instance.SetMoveState(true);
            Manager.CM_Instance.SetRotState(true);
            Manager.CM_Instance.OffMouseCursor();

            if(type == 1)
            {
                //연출 추가할것 넣어주기
            }
            else if (type == 2)
            {
                ObjectInteraction objI = GameObject.Find("C_Room_Door_Pivot").GetComponent<ObjectInteraction>();
                if (objI.thisMove)
                    objI.SendMessage("InteractionStart");
            }
            else if (type == 3)
            {
                Manager.CM_Instance.SetMoveState(false);
                Manager.CM_Instance.SetRotState(false);
                Manager.CM_Instance.OffMouseCursor();
                Manager.UIManager_Instance.UIPopup("Scene_UI/UI_Scene_GameOver");
            }

            StopAllCoroutines();
            Manager.UIManager_Instance.CloseUI();
        }
    }

    //다음버튼함수
    public void End_Typing()
    {
        Debug.Log("다음 출력");
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
        //StartCoroutine(ShowText(fulltext));
        Debug.Log("코루틴 시작");
        StartCoroutine (ShowText(fulltext));
    }

    IEnumerator ShowText(string[] _fullText)
    {
        Debug.Log("텍스트 출력");
        //모든텍스트 종료

        if (npcTalks[temp + cnt].LeftHighlight == 1)
            LeftNPC.GetComponent<Image>().color = Color.white;
        else
            LeftNPC.GetComponent<Image>().color = Color.gray;


        if (npcTalks[temp + cnt].RightHighlight == 1)
            RightNPC.GetComponent<Image>().color = Color.white;
        else
            RightNPC.GetComponent<Image>().color = Color.gray;


        if (cnt >= dialog_cnt)
        {
            text_exit = true;
            StopCoroutine("showText");
        }
        else
        {
            if (npcTalks[temp + cnt].Left2DSprite != null)
            {
                if (LeftNPC.GetComponent<Image>().sprite.name != npcTalks[temp + cnt].Left2DSprite.name)
                {
                    LeftNPC.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    LeftNPC.GetComponent<Image>().sprite = npcTalks[temp + cnt].Left2DSprite;
                }
            }
            else
                LeftNPC.GetComponent<Image>().color = new Color(0, 0, 0, 0);

            if (npcTalks[temp + cnt].Right2DSprite != null)
            {
                if (RightNPC.GetComponent<Image>().sprite.name != npcTalks[temp + cnt].Right2DSprite.name)
                {
                    RightNPC.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    RightNPC.GetComponent<Image>().sprite = npcTalks[temp + cnt].Right2DSprite;
                }
            }
            else
                RightNPC.GetComponent<Image>().color = new Color(0, 0, 0, 0);

            if (npcName != null)
            {
                npcNameTextField.text = npcName[cnt];
            }

            //기존문구clear
            currentText = "";

            //타이핑 시작
            for (int i = 0; i < _fullText[cnt].Length; i++)
            {
                if (effectAudio != null)
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
