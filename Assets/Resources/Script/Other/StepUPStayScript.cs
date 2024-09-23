using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StepUPStayScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.name == "Player_Camera")
        {
            other.transform.AddComponent<SurgicalToolsInteractionScript>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.name == "Player_Camera")
        {
            Destroy(other.transform.GetComponent<SurgicalToolsInteractionScript>());
        }
    }
}
