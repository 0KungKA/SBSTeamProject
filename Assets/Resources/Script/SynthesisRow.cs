using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SynthesisRow
{
    [Tooltip("합성 성공시 지급할 아이템")]
    public GameObject SynthesisItem;

    [Tooltip("합성시 가지고 있어야할 아이템")]
    public string[] ItemNameStirngRow;

    [Tooltip("자동 합성 여부")]
    public bool AutoSynthesis = true;

    [Tooltip("합성 여부 (합성 했을시 True로 바꾸고 더이상 합성 안함)")]
    internal bool Check = false;
}
