using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemInteraction : MonoBehaviour
{
    string DefaultPath = "0.UI/Item_UI/Icon";

    public void ItemUISpawn0()//RenderTexture로 아이템 이미지 띄우는 방식
    {
        GameObject Pgo = ItemManager.ItemManager_Instance.GetNullItemSlot();
        if(Pgo != null )
        {
            GameObject Cgo = Manager.Instance.Instantiate(Resources.Load<GameObject>(DefaultPath + "/" + transform.name));
            Cgo.transform.parent = Pgo.transform;
            Cgo.GetComponent<Transform>().localPosition = Vector3.zero;
            Cgo.GetComponent<Transform>().localScale = new Vector3(0.1f,0.1f,0.1f);
            Cgo.transform.localRotation = Quaternion.identity;
            if (transform.gameObject.layer == (int)Layer_Enum.LayerInfo.ViewItem)
            {
                Manager.UIManager_Instance.UIPopup("UI_Item_View");
            }
            else
            {
                Debug.Log(transform.name + "의 레이어가 ViewItem이 아닙니다 설정 확인해주세요");
            }

            GameObject Cgo2 = Manager.Instance.Instantiate(Resources.Load<GameObject>(DefaultPath + "/" + transform.name));
            GameObject.FindWithTag("Target").GetComponent<RenderViewObj>().Targetset(Cgo2);
            /*Cgo2.transform.parent = GameObject.FindWithTag("Target").transform;
            Cgo2.transform.parent.GetComponent<RenderViewObj>().Target = Cgo2.transform.gameObject;
            Cgo2.transform.localPosition= Vector3.zero;
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
            Destroy(gameObject);
        }
        else
        {
            Manager.ErrorInfo_Instance.ErrorEnqueue("더이상 물건을 집을 손이 없어...");
        }
        
    }

    public void ItemUISpawn() //이미지로 Item UI 띄우는 코드 (약간 수정 필요)
    {
        GameObject Pgo = ItemManager.ItemManager_Instance.GetNullItemSlot();
        if (Pgo != null)
        {
            GameObject Cgo = Manager.Instance.Instantiate(Resources.Load<GameObject>(DefaultPath + "/" + transform.name));
            Cgo.transform.parent = Pgo.transform;
            Cgo.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            Cgo.GetComponent<RectTransform>().localScale = Vector3.one;

            if (transform.gameObject.layer == (int)Layer_Enum.LayerInfo.ViewItem)
            {
                Manager.UIManager_Instance.UIPopup("UI_Item_View");
            }
            else
            {
                Debug.Log(transform.name + "의 레이어가 ViewItem이 아닙니다 설정 확인해주세요");
            }

            GameObject Cgo2 = gameObject;
            RenderViewObj Rvo = GameObject.FindWithTag("Target").GetComponent<RenderViewObj>();
            Rvo.Targetset(Cgo2);
            /*Cgo2.transform.parent = GameObject.FindWithTag("Target").transform;
            Cgo2.transform.parent.GetComponent<RenderViewObj>().Target = Cgo2.transform.gameObject;
            Cgo2.transform.localPosition = Vector3.zero;*/
            GameObject gm = GameObject.Find("UI_Item_View");
            gm = gm.transform.Find("Item_Explanation_BGImg").gameObject;
            gm = gm.transform.GetChild(0).gameObject;
            string[] st = gm.GetComponent<TypeWriterEffect>().fulltext;

            if (Cgo2.GetComponent<ItemInfo>() != null)
            {
                st[0] = new string(Cgo2.GetComponent<ItemInfo>().ItemExplanatino);
                //Manager.ErrorInfo_Instance.ErrorEnqueue(st[0]);
            }

            //아래 디스트로이로 지우긴하는데 지우기보단 오브젝트를 타겟으로 옮겨서 렌더타깃시킬려고했는데 크기가 너무 작음
            //크기를 자동으로 설정해주는 코드를 짜야할듯

            Debug.Log("ItemInteraction 코드 수정 필요");
            //Destroy(gameObject);
        }
        else
        {
            Manager.ErrorInfo_Instance.ErrorEnqueue("더이상 물건을 집을 손이 없어...");
            Manager.UIManager_Instance.UIPopup("UI_Instant_Popup");
        }

    }


    public void ObjectUISpawn(GameObject go)//ITObject태그
    {
        RenderViewObj Rvo = GameObject.FindWithTag("Target").GetComponent<RenderViewObj>();
        if (Rvo.GetRenderInteraction() == true) return;
        GameObject newGo = Manager.Instance.Instantiate(go);
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Target");

        if(temp.Length > 1)
        {
            foreach(GameObject go2 in temp)
            {
                Debug.Log("태그가 Target으로 설정된 오브젝트 : " + go2.name);
            }
        }

        Rvo.Targetset(newGo);

        if (transform.gameObject.layer == (int)Layer_Enum.LayerInfo.ViewItem)
            Manager.UIManager_Instance.UIPopup("UI_Item_View");
        else
            Debug.Log(transform.name + "의 레이어가 ViewItem이 아닙니다 설정 확인해주세요");
        newGo.transform.parent.GetComponent<RenderViewObj>().Target = newGo.transform.gameObject;


        GameObject sgm = GameObject.Find("UI_Item_View");
        sgm = sgm.transform.Find("Item_Explanation_BGImg").gameObject;
        sgm = sgm.transform.GetChild(0).gameObject;
        string[] st = sgm.GetComponent<TypeWriterEffect>().fulltext;

        if (newGo.GetComponent<ItemInfo>() != null)
            st[0] = new string(newGo.GetComponent<ItemInfo>().ItemExplanatino);
        else
            st[0] = transform.name;
    
    
    }
}
