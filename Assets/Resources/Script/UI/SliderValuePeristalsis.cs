using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour
{
    //�ؽ�Ʈ�� value������ ��½����ִ� ��ũ��Ʈ
    //�������� ������Ʈ�� ��������

    public Slider TargetSlider;

    private Text text;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        text.text = TargetSlider.value.ToString();
    }
}
