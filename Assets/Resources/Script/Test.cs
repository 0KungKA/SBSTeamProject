using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    
    void Start()
    {
        List<int> score = new List<int>();
        score.Add(8);
        score.Add(21);
        score.Add(8);
        score.Add(15);
        score.Add(5);
        score.Add(30);
        score.Add(21);
        int guilder_count = 2;
        int k = 3;
        sumVips(score, guilder_count, k);
    }

    public static long sumVips(List<int> score, int guilder_count, int k)
    {
        int[] selectNum = new int[guilder_count];
        int leftIndex = 0;
        int rightIndex = score.Count - 1;

        for (int i = 0; i < guilder_count; i++)
        {
            int leftMax = int.MinValue;
            int rightMax = int.MinValue;
            int leftMaxIndex = -1;
            int rightMaxIndex = -1;

            for (int j = 0; j < k && leftIndex + j <= rightIndex; j++)
            {
                if (score[leftIndex + j] > leftMax)
                {
                    leftMax = score[leftIndex + j];
                    leftMaxIndex = leftIndex + j;
                }

                if (score[rightIndex - j] > rightMax)
                {
                    rightMax = score[rightIndex - j];
                    rightMaxIndex = rightIndex - j;
                }
            }

            if (leftMax >= rightMax)
            {
                selectNum[i] = leftMax;
                score.RemoveAt(leftMaxIndex);
                rightIndex--;
            }
            else
            {
                selectNum[i] = rightMax;
                score.RemoveAt(rightMaxIndex);
                rightIndex--;
            }
        }

        long sum = 0;
        foreach (int val in selectNum)
        {
            sum += val;
        }

        return sum;
    }

    // Update is called once per frame
    void Update()
    {
       

    }
}
