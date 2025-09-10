using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PatchNotiUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title_text;
    [SerializeField] TextMeshProUGUI content;


    public void PatchNoti(int alertID)
    {
        Debug.Log("PatchNoti 실행");
        // content.text = alertID;

    }

}
