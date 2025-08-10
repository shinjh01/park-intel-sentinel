using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotiDetailUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI floor;
    [SerializeField] TextMeshProUGUI zone;
    [SerializeField] TextMeshProUGUI plateNumber;
    [SerializeField] TextMeshProUGUI enterTime;
    [SerializeField] TextMeshProUGUI violationType;

    private int floor_param;
    private string zone_name_param;
    private string rfid_tag_param;

    public void NotiDetail(NotiDetailData notiDetailData)
    {
        Debug.Log("NotiDetail 실행");

        floor.text = $"{notiDetailData.floor.ToString()}";
        zone.text = $"{notiDetailData.name}";
        plateNumber.text = $"{notiDetailData.plate_text}";
        violationType.text = $"{notiDetailData.reason}";

        if (notiDetailData.entered_at.HasValue)
        {
            string formattedTime = notiDetailData.entered_at.Value.ToString("yyyy-MM-dd HH:mm");
            enterTime.text = $"{formattedTime}";
        }
        else
        {
            enterTime.text = "null";
        }

        floor_param = notiDetailData.floor;
        zone_name_param = notiDetailData.name;
        rfid_tag_param = notiDetailData.rfid_tag;
    }

    // BackButton
    public void OnClickBackBtn()
    {
        APIManager.Instance?.RequestNotiList();
    }

    // HomeButton
    public void OnClickHomeBtn()
    {
        APIManager.Instance?.RequestNewNoti();
    }

    // VehiclePosButton
    public void OnClickVehiclePosBtn()
    {
        APIManager.Instance?.RequestRobotPos();
        APIManager.Instance?.VehiclePos(floor_param, zone_name_param, rfid_tag_param);
    }


}
