using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorInfo : MonoBehaviour
{
    Queue<string> ErrorCode = new Queue<string>();

    public void ErrorEnqueue(string errorInfo)//에러 내용 집어넣기
    {
        errorInfo = errorInfo.Replace("\\n", "\n");
        ErrorCode.Clear();
        ErrorCode.Enqueue(errorInfo);
        Manager.UIManager_Instance.UIPopup("UI_Instant_Popup");
    }

    //에러 내용 리턴해주고 삭제
    public string ErrorDequeue()
    {
        return ErrorCode.Dequeue();
    }

    //에러 내용 리턴 (현상황에선 안씀)
    /*public string ErrorPeek()
    {
        return ErrorCode.Peek();
    }*/

    public int ErrorCount { get { return ErrorCode.Count; } }
}
