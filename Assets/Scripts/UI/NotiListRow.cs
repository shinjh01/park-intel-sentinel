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
    private string reason;
    private string[] reasonSplit;
    private string reason_param;

    public void SetData(NotiData data)
    {
        floor.text = data.floor.ToString();
        zone.text = data.name;
        plateNumber.text = data.plate_text;

        Debug.Log($"=========================={data.reason}=========================");

        SplitReason(data.reason);
        
        violationType.text = ReturnReason();

        alertID = data.id;
        reason_param = ReturnReason();
    }

    private void SplitReason(string reason)
    {
        reasonSplit = reason.Split(' ');
    }

    private string ReturnReason()
    {
        Debug.Log($"zone: {reasonSplit[4]}, vehicle: {reasonSplit[1]}");

        if (reasonSplit[4] == "EV")
        {
            if (reasonSplit[1] == "NORMAL" || reasonSplit[1] == "DISABLED")
            {
                return "전기차 구역 점유";
            }
            else
            {
                return null;
            }
        }
        else if (reasonSplit[4] == "DISABLED")
        {
            if (reasonSplit[1] == "NORMAL" || reasonSplit[1] == "EV")
            {
                return "장애인 구역 위반";
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }

    public void OnClickNotiDetailBtn()
    {
        APIManager.Instance?.RequestNotiDetail(alertID, reason_param);
    }


}
