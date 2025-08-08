using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotiListUI : MonoBehaviour
{
    [SerializeField] GameObject rowPrefab;
    [SerializeField] Transform rowParent;  // 프리팹을 자식으로 둘 부모 오브젝트


    public void NotiList(AllNotiListData allNotiListData)
    {
        Debug.Log("NotiList 실행");

        foreach (Transform child in rowParent)
        {
            Destroy(child.gameObject);
        }

        List<NotiData> list = allNotiListData?.data;
        
        if (list != null && list.Count > 0)
        {
            foreach (var notiData in list)
            {
                GameObject newRow = Instantiate(rowPrefab, rowParent);

                NotiListRow row = newRow.GetComponent<NotiListRow>();
                if (row != null)
                {
                    row.SetData(notiData);
                }
            }
        }
        else
        {
            Debug.Log("표시할 데이터 없음");
        }
    }

    // BackButton, HomeButton
    public void OnClickBtn()
    {
        APIManager.Instance?.RequestNewNoti();
    }

}
