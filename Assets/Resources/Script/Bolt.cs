using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    Light theLight;
    bool Elight = false;
    bool Event = false;
    private void Start()
    {
        theLight = GetComponent<Light>();
    }

    public void BoltEventStart(int value)//value는 몇번 작동하게 할건지
    {
        StartCoroutine("BoltEvent", value);
    }

    void Update()
    {
        
    }

    IEnumerator BoltEvent(int value)
    {
        int temp = 0;
        while (true)
        {
            //if(theLight.intensity < 10 && light == false)
            if (Elight == false)
            {
                if (theLight.intensity >= 9.5f)
                {
                    Elight = true;
                    yield return null;
                }

                theLight.intensity = Mathf.Lerp(theLight.intensity, 10, 0.5f);
                yield return null;
            }
            else if (Elight == true)
            {
                if (theLight.intensity <= 0.5f)
                {
                    Elight = false;
                    temp++;
                    if (temp == value)
                    {
                        yield break;
                    }
                    yield return null;
                }
                theLight.intensity = Mathf.Lerp(theLight.intensity, 0, 0.5f);
                yield return null;
            }
            yield return null;
        }
    }
}
