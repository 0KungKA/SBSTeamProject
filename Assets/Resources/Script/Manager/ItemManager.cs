using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    static ItemManager M_ItemManager;
    public static ItemManager ItemManager_Instance { get { return M_ItemManager; } }

    GameObject[] itemSlot;
    public void SetItemSlot(GameObject[] slot) { ItemManager_Instance.itemSlot = slot; }
    public GameObject[] GetItemSlot() {  return ItemManager_Instance.itemSlot; }

    InputManager PC;

    bool[] SelectItemSlot = new bool[6] { false, false, false, false, false, false };

    string DefaultPath = "0.UI/Item_UI/Icon";

    public void Awake()
    {
        transform.tag = "Managers";
        PC = GameObject.FindWithTag("MainCamera").GetComponent<InputManager>();

        if (M_ItemManager == null)
            M_ItemManager = transform.GetComponent<ItemManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            int temp = 0;
            if (itemSlot[temp].transform.childCount != 0)
            {
                PC.SelectItem(itemSlot[temp].transform.GetChild(0).gameObject);

                if (SelectItemSlot[temp] == false)
                {
                    SelectItemSlot[temp] = !SelectItemSlot[temp];
                }
                else if(SelectItemSlot[temp] == true)
                {
                    SelectItemSlot[temp] = !SelectItemSlot[temp];
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            int temp = 1;
            if (itemSlot[temp].transform.childCount != 0)
            {
                PC.SelectItem(itemSlot[temp].transform.GetChild(0).gameObject);

                if (SelectItemSlot[temp] == false)
                {
                    SelectItemSlot[temp] = !SelectItemSlot[temp];
                }
                else if (SelectItemSlot[temp] == true)
                {
                    SelectItemSlot[temp] = !SelectItemSlot[temp];
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            int temp = 2;
            if (itemSlot[temp].transform.childCount != 0)
            {
                PC.SelectItem(itemSlot[temp].transform.GetChild(0).gameObject);

                if (SelectItemSlot[temp] == false)
                {
                    SelectItemSlot[temp] = !SelectItemSlot[temp];
                }
                else if (SelectItemSlot[temp] == true)
                {
                    SelectItemSlot[temp] = !SelectItemSlot[temp];
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            int temp = 3;
            if (itemSlot[temp].transform.childCount != 0)
            {
                PC.SelectItem(itemSlot[temp].transform.GetChild(0).gameObject);

                if (SelectItemSlot[temp] == false)
                {
                    SelectItemSlot[temp] = !SelectItemSlot[temp];
                }
                else if (SelectItemSlot[temp] == true)
                {
                    SelectItemSlot[temp] = !SelectItemSlot[temp];
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            int temp = 4;
            if (itemSlot[temp].transform.childCount != 0)
            {
                PC.SelectItem(itemSlot[temp].transform.GetChild(0).gameObject);

                if (SelectItemSlot[temp] == false)
                {
                    SelectItemSlot[temp] = !SelectItemSlot[temp];
                }
                else if (SelectItemSlot[temp] == true)
                {
                    SelectItemSlot[temp] = !SelectItemSlot[temp];
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            int temp = 5;
            if (itemSlot[temp].transform.childCount != 0)
            {
                PC.SelectItem(itemSlot[temp].transform.GetChild(0).gameObject);

                if (SelectItemSlot[temp] == false)
                {
                    SelectItemSlot[temp] = !SelectItemSlot[temp];
                }
                else if (SelectItemSlot[temp] == true)
                {
                    SelectItemSlot[temp] = !SelectItemSlot[temp];
                }
            }
        }

    }

    internal string GetCurrentItem()
    {
        GameObject currentItem = PC.CurrentSelectItem;

        if (currentItem != null)
            return currentItem.name;
        else
            return null;
    }

    public GameObject GetNullItemSlot()
    {
        foreach (GameObject item in itemSlot)
        {
            if(item.transform.childCount == 0)
            {
                Debug.Log(item.name + "호출");
                return item;
            }
        }

        return null;
    }

    public void DeleteItem(string name)
    {
        for(int i = 0; i < itemSlot.Count(); i ++)
        {
            if (itemSlot[i].transform.childCount != 0 && itemSlot[i].transform.GetChild(0).name == name)
            {
                Destroy(itemSlot[i].transform.GetChild(0).gameObject);
            }
        }
    }

    IEnumerator ItemViewSpawn(string path)
    {
        while (true)
        {
            if (GameObject.Find("UI_Item_View") == null)
            {
                CreateItem(path);
                yield break;
            }
            else
                yield return null;

        }
    }

    public void CreateItem(string path)
    {
        GameObject Pgo = GetNullItemSlot();
        if (Pgo != null)
        {
            GameObject Cgo = Manager.Instance.Instantiate(Resources.Load<GameObject>(DefaultPath + "/" + path));
            Cgo.transform.parent = Pgo.transform;
            Cgo.GetComponent<Transform>().localPosition = Vector3.zero;
            //Cgo.GetComponent<Transform>().localScale = new Vector3(0.1f, 0.1f, 0.1f);
            Cgo.transform.localRotation = Quaternion.identity;
            if (Cgo.transform.gameObject.layer == (int)Layer_Enum.LayerInfo.ViewItem)
            {
                Manager.UIManager_Instance.UIPopup("UI_Item_View");
            }
            else
            {
                Debug.Log(transform.name + "의 레이어가 ViewItem이 아닙니다. 설정 확인해주세요");
            }

            GameObject Cgo2 = Manager.Instance.Instantiate(Resources.Load<GameObject>(DefaultPath + "/" + path));
            Cgo2.transform.parent = GameObject.FindWithTag("Target").transform;
            Cgo2.transform.parent.GetComponent<RenderViewObj>().Targetset(Cgo2);
            /*Cgo2.transform.parent.GetComponent<RenderViewObj>().Target = Cgo2.transform.gameObject;
            Cgo2.transform.localPosition = Vector3.zero;
            Cgo2.transform.localScale = Vector3.one;
            Cgo2.transform.localRotation = Quaternion.identity;*/

            GameObject gm = GameObject.Find("UI_Item_View");
            gm = gm.transform.Find("Item_Explanation_BGImg").gameObject;
            gm = gm.transform.GetChild(0).gameObject;
            string[] st = gm.GetComponent<TypeWriterEffect>().fulltext;
            if (Cgo2.GetComponent<ItemInfo>() != null)
            {
                st[0] = new string(Cgo2.GetComponent<ItemInfo>().ItemExplanatino);
            }
            /*gameObject.layer = (int)Layer_Enum.LayerInfo.ViewItem;
            gameObject.transform.parent = GameObject.FindWithTag("Target").transform;
            gameObject.transform.localPosition = Vector3.zero;*/
            //아래 디스트로이로 지우긴하는데 지우기보단 오브젝트를 타겟으로 옮겨서 렌더타깃시킬려고했는데 크기가 너무 작음
            //크기를 자동으로 설정해주는 코드를 짜야할듯

            Debug.Log("ItemInteraction 코드 수정 필요");
            //Destroy(gameObject);
        }
        else
        {
            Manager.ErrorInfo_Instance.ErrorEnqueue("더이상 물건을 집을 손이 없어...");
        }
    }

    public void CreateItem(string path ,GameObject obj)
    {
        GameObject Pgo = GetNullItemSlot();
        if (Pgo != null)
        {
            GameObject Cgo = Manager.Instance.Instantiate(Resources.Load<GameObject>(DefaultPath + "/" + path));
            Cgo.transform.parent = Pgo.transform;
            Cgo.GetComponent<Transform>().localPosition = Vector3.zero;

            Cgo.transform.localRotation = Quaternion.identity;
            if (Cgo.transform.gameObject.layer == (int)Layer_Enum.LayerInfo.ViewItem)
            {
                Manager.UIManager_Instance.UIPopup("UI_Item_View");
            }
            else
            {
                Debug.Log(transform.name + " 의 레이어가 ViewItem이 아닙니다. 설정 확인해주세요");
            }

            GameObject Cgo2 = Manager.Instance.Instantiate(obj);
            Cgo2.transform.parent = GameObject.FindWithTag("Target").transform;
            Cgo2.transform.parent.GetComponent<RenderViewObj>().Targetset(Cgo2);
            /*Cgo2.transform.parent.GetComponent<RenderViewObj>().Target = Cgo2.transform.gameObject;
            Cgo2.transform.localPosition = Vector3.zero;
            Cgo2.transform.localScale = Vector3.one;
            Cgo2.transform.localRotation = Quaternion.identity;*/

            GameObject gm = GameObject.Find("UI_Item_View");
            gm = gm.transform.Find("Item_Explanation_BGImg").gameObject;
            gm = gm.transform.GetChild(0).gameObject;
            string[] st = gm.GetComponent<TypeWriterEffect>().fulltext;
            if (Cgo2.GetComponent<ItemInfo>() != null)
            {
                st[0] = new string(Cgo2.GetComponent<ItemInfo>().ItemExplanatino);
            }
            /*gameObject.layer = (int)Layer_Enum.LayerInfo.ViewItem;
            gameObject.transform.parent = GameObject.FindWithTag("Target").transform;
            gameObject.transform.localPosition = Vector3.zero;*/
            //아래 디스트로이로 지우긴하는데 지우기보단 오브젝트를 타겟으로 옮겨서 렌더타깃시킬려고했는데 크기가 너무 작음
            //크기를 자동으로 설정해주는 코드를 짜야할듯

            Debug.Log("ItemInteraction 코드 수정 필요");
            //Destroy(gameObject);
        }
        else
        {
            Manager.ErrorInfo_Instance.ErrorEnqueue("더이상 물건을 집을 손이 없어...");
        }
    }
}
