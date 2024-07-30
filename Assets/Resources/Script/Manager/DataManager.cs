using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class DataManager : MonoBehaviour
{
    List<ObjectData> objectList = new List<ObjectData>();
    List<ChatLoader> chatList = new List<ChatLoader>();

    public string CSVPath = "Script/Systems/CSV_File/";

    private void Awake()
    {
        
    }

    public ObjectData GetObjectData(GameObject gameObject)
    {
        return null;
    }
}
