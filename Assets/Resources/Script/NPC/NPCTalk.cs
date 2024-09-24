using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//이번트 시스템에 넣을거임
public class NPCTalk : MonoBehaviour
{
    List<NPCTalkClass> npcTalks = new List<NPCTalkClass>();

    [SerializeField]
    GameObject NPCTalkCanvas;

    //Start is called before the first frame update
    void Start()
    {
    }

    public void StartNPCTalk(int val)
    {
        if(npcTalks.Count == 0)
            npcTalks = Manager.DataManager_Instance.GetCPNTalk();

        int talksize = 0;

        for(int i = 0; i < npcTalks.Count; i++)
        {
            if (npcTalks[i].type == val)
                talksize++;
        }

        Manager.UIManager_Instance.UIPopup("UI_ChatNPC");
        GameObject go = GameObject.Find("UI_ChatNPC");
        go.transform.SetAsLastSibling();
        go.transform.gameObject.SetActive(true);
        go = go.transform.Find("Chat_Window").transform.GetChild(0).gameObject;
        go.GetComponent<NPCTalkTypingScript>().StartTalk(npcTalks, val, go);//, talksize
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
