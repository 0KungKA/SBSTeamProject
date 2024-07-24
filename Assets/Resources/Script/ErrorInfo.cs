using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorInfo : MonoBehaviour
{
    Queue<string> ErrorCode = new Queue<string>();

    public void ErrorEnqueue(string errorInfo)//���� ���� ����ֱ�
    {
        errorInfo = errorInfo.Replace("\\n", "\n");
        ErrorCode.Clear();
        ErrorCode.Enqueue(errorInfo);
        Manager.UIManager_Instance.UIPopup("UI_Instant_Popup");
    }

    //���� ���� �������ְ� ����
    public string ErrorDequeue()
    {
        return ErrorCode.Dequeue();
    }

    //���� ���� ���� (����Ȳ���� �Ⱦ�)
    /*public string ErrorPeek()
    {
        return ErrorCode.Peek();
    }*/

    public int ErrorCount { get { return ErrorCode.Count; } }
}
