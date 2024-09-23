using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;

public class SurgicalToolsInteractionScript : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetMouseButtonDown((int)MouseButton.LeftMouse))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 10.0f))
            {
                if (hit.transform.parent.name == "E_SurgicalTools")
                {
                    hit.transform.GetComponent<ItemInteraction>().ItemUISpawn();
                }
            }
        }
    }
}
