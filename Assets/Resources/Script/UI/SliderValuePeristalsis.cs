using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour
{
    //텍스트를 value값으로 출력시켜주는 스크립트
    //슬라이터 컴포넌트만 쓸수있음

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
