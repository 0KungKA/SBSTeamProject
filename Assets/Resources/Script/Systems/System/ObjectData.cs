using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData : MonoBehaviour
{
    public string Divistoin;//�з� F = ���� / P = ��ǰ / I = ������
    public string Name;//�ҷ��� ������Ʈ �̸�
    public bool View_Object;//3D�� �𵨸��� ������� �ϴ��������� ����
    public bool Get_Item;//�κ��丮 ���� ����
    public string Description;//������Ʈ ����

    public ObjectData(string divistoin, string name, bool view_Object, bool get_Item, string description)
    {
        Divistoin = divistoin;
        Name = name;
        View_Object = view_Object;
        Get_Item = get_Item;
        Description = description;
    }
}
