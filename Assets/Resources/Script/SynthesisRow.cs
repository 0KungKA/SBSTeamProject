using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SynthesisRow
{
    public GameObject SynthesisItem;
    public string[] ItemNameStirngRow;

    public bool AutoSynthesis = true;
    internal bool Check = false;
}
