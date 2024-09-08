using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SynthesisRow
{
    [Tooltip("�ռ� ������ ������ ������")]
    public GameObject SynthesisItem;

    [Tooltip("�ռ��� ������ �־���� ������")]
    public string[] ItemNameStirngRow;

    [Tooltip("�ڵ� �ռ� ����")]
    public bool AutoSynthesis = true;

    [Tooltip("�ռ� ���� (�ռ� ������ True�� �ٲٰ� ���̻� �ռ� ����)")]
    internal bool Check = false;
}
