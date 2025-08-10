using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewNotiUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI content;


    public void NewNoti(NewNotiListData newNotiListData)
    {
        Debug.Log("========== NewNoti 실행 ==========");

        List<NotiData> list = newNotiListData?.logs;

        if (list != null && list.Count > 0)
        {
            content.text = $"새로운 알림 {list.Count}개";
        }
        else
        {
            content.text = "새로운 알림이 없습니다.";
        }
    }

}
