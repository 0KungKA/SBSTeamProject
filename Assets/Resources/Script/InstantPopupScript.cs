using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstantPopupScript : MonoBehaviour
{
    [SerializeField]
    Text text;

    string stext;

    private void Start()
    {
        if(text == null)
            text = transform.GetComponent<Text>();

        if(text != null)
        {
            SetText();
            Invoke("AutoClose", 2f);
        }
    }

    private void SetText()
    {
        text.text = Manager.ErrorInfo_Instance.ErrorDequeue();
        if(text == null || text.text == "")
        {
            AutoClose();
        }
    }

    void AutoClose()
    {
        Destroy(transform.parent.gameObject);
    }
}
