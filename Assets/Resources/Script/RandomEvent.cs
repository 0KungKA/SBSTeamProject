using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEvent : MonoBehaviour
{
    private bool onBolt = false;
    private GameObject[] gm;

    private void Start()
    {
        gm = GameObject.FindGameObjectsWithTag("BoltLight");
    }

    void Update()
    {
        int Boltrandom = Random.Range(0, 1000);//번개 이벤트 확률
        int BoltEventRandomValue = 1;//번개 이벤트 확률
        int BoltPlayValue = Random.Range(1, 3);//번개 몇번 치게할건지 랜덤값

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
