using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannequinCheck : MonoBehaviour
{
    internal bool mannequinCheck = false;//각 마네킹이 기믹을 수행했는지 판단하기위한 bool

    ObjectInteraction[] M_Check = null;//마네킹에 포함된 기믹을 수행했는지 판단하기위한 bool배열

    private void Start()
    {
        M_Check = transform.GetComponentsInChildren<ObjectInteraction>();
    }

    private void Update()
    {
        int value = 0;
        for (int i = 0; i < 2; i++)
        {
            if (M_Check[i].thisMove)
            {
                value++;
            }
        }

        if (value >= 2)
        {
            mannequinCheck = true;
        }
        else
            mannequinCheck = false;

    }
}
