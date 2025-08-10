using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotiListRow : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI floor;
    [SerializeField] TextMeshProUGUI zone;
    [SerializeField] TextMeshProUGUI plateNumber;
    [SerializeField] TextMeshProUGUI violationType;
    [SerializeField] Button detailButton;

    private int alertID;

    public void SetData(NotiData data)
    {
        floor.text = data.floor.ToString();
        zone.text = data.name;
        plateNumber.text = data.plate_text;
        violationType.text = data.reason;

        alertID = data.id;
    }

    public void OnClickNotiDetailBtn()
    {
        // APIManager.Instance?.RequestRobotPos();
        APIManager.Instance?.RequestNotiDetail(alertID);
    }


}
