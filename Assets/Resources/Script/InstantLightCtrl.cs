using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class InstantLightCtrl : MonoBehaviour
{
    private Light theLight;

    private float targetIntensity;
    private float currentIntensity;

    private bool CMmove = false;

    [SerializeField]
    private float MaxRot;
    [SerializeField]
    private float RotSpeed;

    private void Start()
    {
        theLight = GetComponent<Light>();
        currentIntensity = theLight.intensity;
        targetIntensity = Random.Range(0.4f, 1f);
    }

    private void Update()
    {
        if (transform.tag == "Respawn")
        {
            Vector3 Rot = new Vector3(0, 0, 0);
            if (CMmove == false)
            {
                Rot.x = 0f;
                Rot.y += RotSpeed * Time.deltaTime;
                Rot.z = 0f;
                transform.Rotate(Rot, Space.World);

                float rotY = transform.localEulerAngles.y;
                rotY = (rotY >= 180) ? transform.localEulerAngles.y - 360 : transform.localEulerAngles.y;

                if (rotY >= MaxRot)
                    CMmove = true;
            }
            else if (CMmove == true)
            {
                Rot.x = 0f;
                Rot.y -= RotSpeed * Time.deltaTime;
                Rot.z = 0f;
                transform.Rotate(Rot, Space.World);

                float rotY = transform.localEulerAngles.y;
                rotY = (rotY >= 180) ? transform.localEulerAngles.y - 360 : transform.localEulerAngles.y;

                if (rotY <= -MaxRot)
                    CMmove = false;
            }
        }
        else if (Mathf.Abs(targetIntensity - currentIntensity) >= 0.01)
        {
            if(targetIntensity - currentIntensity >= 0)
            {
                currentIntensity += Time.deltaTime * 3f;
            }
            else
            {
                currentIntensity -= Time.deltaTime * 3f;

                theLight.intensity = currentIntensity;
                //theLight.range = currentIntensity + 300;
            }
        }
        else
        {
            targetIntensity = Random.Range(0.4f, 1f);
        }
    }
}
