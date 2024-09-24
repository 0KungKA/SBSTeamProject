using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannequinCheck : MonoBehaviour
{
    internal bool mannequinCheck = false;//�� ����ŷ�� ����� �����ߴ��� �Ǵ��ϱ����� bool

    ObjectInteraction[] M_Check = null;//����ŷ�� ���Ե� ����� �����ߴ��� �Ǵ��ϱ����� bool�迭

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
