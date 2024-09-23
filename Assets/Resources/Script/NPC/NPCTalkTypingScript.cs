using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//�� �ڵ�� ������ ���̺� �������� ���� �ڵ���
/*
Ŭ���� �������
int index
string �̸�
int ��ȭ������ ���� type
string ��ȭ����

sprite ���� NPC �̹���
int ���� npc�̹��� ���̶���Ʈ (0 = ��¦ ��ο� / 1 = �⺻)

sprite ������ NPC �̹���
int ���� npc�̹��� ���̶���Ʈ (0 = ��¦ ��ο� / 1 = �⺻)
*/


//��� NPC Chat UI�� ��
//UI�� ���������� ���� �̰� ȣ�����ִ� ��ũ��Ʈ ���� ¥����
public class NPCTalkTypingScript : MonoBehaviour
{
    [SerializeField]
    AudioSource effectAudio;

    //������ ����
    [Tooltip("����ؾ��� �ؽ��İ� �ڵ����� �㶧 �󸶳� ���� �߰� �Ұ�����")]
    [Range(0.01f,0.1f)]
    public float delay;
    [Tooltip("����ؾ��� �ؽ��İ� �ٶ߰� ������ư�� ������ ������� ���۾��ϰ� �Ұ��� / 0 = ����")]
    [Range(0, 5.0f)]
    public float Skip_delay;

    //����ؾ��� ������ �� �� ��
    int cnt;

    //Ÿ����ȿ�� ����
    string[] fulltext;

    //����ؾ��� ������ �� �� ��
    int dialog_cnt;
    string[]npcName;
    //public string[] fulltext; <- �ܺο��� �۾��ҋ� ��
    //public string[] npcName; <- �ܺο��� �۾��ҋ� ��
    //�ܺο������ٰ� �۾��ϸ� ��ȸ������ �ۿ� ������

    [SerializeField]
    GameObject PObject;
    [SerializeField]
    GameObject LeftNPC;
    [SerializeField]
    GameObject RightNPC;

    string currentText;
    

    //Ÿ����Ȯ�� ����
    //����ؾ��� ������ �� ���Դ��� ����
    public bool text_exit;
    //�ؽ�Ʈ�� ���� ������ ����(���� �� �ȉ�µ� ��ư ������ ��ü ����ҷ��� ���� bool ����)
    public bool text_full;
    public bool text_cut;

    //�߰�����
    int type;
    int PlayerTalkSize;//�̹� UI���� ��� ����ؾ��ϴ��������� ����
    int temp;//���° ���� �����ϴ��������� ����

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
                temp = i;//��������� �����ϴ��� �˱����� ���������� �����
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
        Debug.Log("�� Ÿ���� ��ŸƮ");
        Debug.Log(dialog_cnt+" ������");
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


    //��� �ؽ�Ʈ ȣ��Ϸ�� Ż��
    void Update()
    {
        if (text_exit == true && this != null)
        {
            Debug.Log("�ڷ�ƾ ����");
            Manager.CM_Instance.SetMoveState(true);
            Manager.CM_Instance.SetRotState(true);
            Manager.CM_Instance.OffMouseCursor();

            if(type == 1)
            {
                //���� �߰��Ұ� �־��ֱ�
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

    //������ư�Լ�
    public void End_Typing()
    {
        Debug.Log("���� ���");
        //���� �ؽ�Ʈ ȣ��
        if (text_full == true)
        {
            cnt++;
            text_full = false;
            text_cut = false;
            StartCoroutine(ShowText(fulltext));
        }
        //�ؽ�Ʈ Ÿ���� ����
        else
        {
            text_cut = true;
        }
    }

    //�ؽ�Ʈ ����ȣ��
    public void Get_Typing(int _dialog_cnt, string[] _fullText)
    {
        //������ ���� �����ʱ�ȭ
        text_exit = false;
        text_full = false;
        text_cut = false;
        cnt = 0;

        //���� �ҷ�����
        dialog_cnt = _dialog_cnt;
        fulltext = new string[dialog_cnt];
        fulltext = _fullText;

        //Ÿ���� �ڷ�ƾ����
        //StartCoroutine(ShowText(fulltext));
        Debug.Log("�ڷ�ƾ ����");
        StartCoroutine (ShowText(fulltext));
    }

    IEnumerator ShowText(string[] _fullText)
    {
        Debug.Log("�ؽ�Ʈ ���");
        //����ؽ�Ʈ ����

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

            //��������clear
            currentText = "";

            //Ÿ���� ����
            for (int i = 0; i < _fullText[cnt].Length; i++)
            {
                if (effectAudio != null)
                    effectAudio.Play();

                //Ÿ�����ߵ�Ż��
                if (text_cut == true)
                {
                    break;
                }
                //�ܾ��ϳ������
                currentText = _fullText[cnt].Substring(0, i + 1);
                this.GetComponent<Text>().text = currentText;
                yield return new WaitForSeconds(delay);
            }
            //Ż��� ��� �������
            Debug.Log("Typing ����");
            this.GetComponent<Text>().text = _fullText[cnt];
            yield return new WaitForSeconds(Skip_delay);

            //��ŵ_������ ����
            Debug.Log("Enter ���");
            text_full = true;
        }
    }
}
