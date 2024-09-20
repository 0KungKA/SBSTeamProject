using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEvent : MonoBehaviour
{
    private GameObject[] gm;

    private void Start()
    {
        gm = GameObject.FindGameObjectsWithTag("BoltLight");
    }

    void Update()
    {
        int Boltrandom = Random.Range(0, 5000);//���� �̺�Ʈ Ȯ��
        int BoltEventRandomValue = 1;//���� �̺�Ʈ Ȯ��
        int BoltPlayValue = Random.Range(1, 2);//���� ��� ġ���Ұ��� ������

        if (Boltrandom <= BoltEventRandomValue)
        {
            foreach(GameObject go in gm)
            {
                go.GetComponent<Bolt>().BoltEventStart(BoltPlayValue);
                AudioSource As = GameObject.Find("WorldSound").GetComponent<AudioSource>();
                if (As.isPlaying==false)
                {
                    As.Play();
                }
            }
        }
    }
}
