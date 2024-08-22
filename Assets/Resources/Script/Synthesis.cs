using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Synthesis : MonoBehaviour
{
    [SerializeField]
    GameObject[] CaseItem;

    GameObject[] itemslotTemp;
    public SynthesisRow[] SynthesisItemCase;

    int temp = 0;

    public void Init()
    {
        StartCoroutine("FindItemSlot");
    }

    IEnumerator FindItemSlot()
    {
        while (true)
        {
            if (itemslotTemp == null)
                itemslotTemp = ItemManager.ItemManager_Instance.GetItemSlot();
            else if (itemslotTemp != null) 
            {
                //StartCoroutine("SynthesisModule");
                yield break;
            }
            yield return null;
        }
    }
    public void SynthesisModule()
    {
        itemslotTemp = ItemManager.ItemManager_Instance.GetItemSlot();
        int SynthesisItemCaseSize = SynthesisItemCase.Count();
        int itemslotTempSize = itemslotTemp.Count();
        int findSynthesisItem = 0;

        List<string> TargetItemNames = new List<string>();

        if (itemslotTemp != null)
        {
            findSynthesisItem = 0;
            for (int i = 0; i < SynthesisItemCaseSize; i++)
            {
                int RowCount = SynthesisItemCase[i].ItemNameStirngRow.Length;
                for (int k = 0; k < RowCount;k++)
                {
                    for (int j = 0; j < itemslotTempSize; j++)
                    {
                        if (itemslotTemp[j].transform.childCount != 0)
                        {
                            if (SynthesisItemCase[i].ItemNameStirngRow[k] == itemslotTemp[j].transform.GetChild(0).name)
                            {
                                findSynthesisItem++;
                                TargetItemNames.Add(SynthesisItemCase[i].ItemNameStirngRow[k]);
                            }
                        }
                    }
                }
            }

            if(findSynthesisItem >= 2)
            {
                foreach(string s in TargetItemNames)
                {
                    ItemManager.ItemManager_Instance.DeleteItem(s);
                    Debug.Log("ÇÕ¼º");
                }
                if (CaseItem[temp] != null)
                {
                    temp++;
                    ItemManager.ItemManager_Instance.StartCoroutine("ItemViewSpawn", "B_Rockpick");
                }
            }

        }
    }
}
