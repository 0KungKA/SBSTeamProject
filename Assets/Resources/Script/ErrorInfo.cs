using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorInfo : MonoBehaviour
{
    Queue<string> ErrorCode = new Queue<string>();

    public void ErrorEnqueue(string errorInfo)//���� ���� ����ֱ�
    {
        errorInfo = errorInfo.Replace("\\n", "\n");

        ErrorCode.Clear();
        ErrorCode.Enqueue(errorInfo);

        GameObject go = GameObject.Find("UI_Instant_Popup");
        string go_string = null;

        if (go != null)
        {
            go_string = go.transform.Find("Field").GetComponent<Text>().text;
        }

        if (go_string != errorInfo)
        {
            StartCoroutine(ErrorInput());
        }
        else if (go_string == null)
        {
            if (go_string == errorInfo)
                return;

            if (ErrorCode.Count > 0)
            {
                StartCoroutine(ErrorInput());
            }
            else
                Manager.UIManager_Instance.UIPopup("UI_Instant_Popup");
        }
        
        else
            return;

        /* GameObject go = GameObject.Find("UI_Instant_Popup");
         if (go == null)
             Manager.UIManager_Instance.UIPopup("UI_Instant_Popup");
         else
             StartCoroutine(ErrorInput());*/
    }

    //���� ���� �������ְ� ����
    public string ErrorDequeue()
    {
        string temp = "";
        if (ErrorCode.Count > 0)
        {
            temp = ErrorCode.Dequeue();
        }
        
        if (temp != null)
        {
            return temp;
        }
        else
            return "";
    }

    //���� ���� ���� (����Ȳ���� �Ⱦ�)
    /*public string ErrorPeek()
    {
        return ErrorCode.Peek();
    }*/

    IEnumerator ErrorInput()
    {
        while(true)
        {
            GameObject go = GameObject.Find("UI_Instant_Popup");
            if (go == null)
            {
                Manager.UIManager_Instance.UIPopup("UI_Instant_Popup");
                yield break;
            }
            else
                yield return null;
        }
    }

    public int ErrorCount { get { return ErrorCode.Count; } }
}
