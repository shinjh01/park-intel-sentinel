using System;
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

    DateTime startTime = new DateTime(2025, 8, 10, 9, 0, 0);
    DateTime endTime = new DateTime(2025, 8, 11, 12, 0, 0);


    public void NotiDetail(NotiDetailData notiDetailData, string reason)
    {
        Debug.Log("========== NotiDetail 실행 ==========");

        floor.text = notiDetailData.floor.ToString();
        zone.text = notiDetailData.name;
        plateNumber.text = notiDetailData.plate_text;
        violationType.text = reason;

        if (notiDetailData.entered_at.HasValue)
        {
            string formattedTime = notiDetailData.entered_at.Value.ToString("yyyy-MM-dd HH:mm");
            enterTime.text = $"{formattedTime}";
        }
        else
        {
            enterTime.text = GetRandomTime();
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

    // VehiclePosButton
    public void OnClickVehiclePosBtn()
    {
        APIManager.Instance?.VehiclePos(floor_param, zone_name_param, rfid_tag_param);
    }

    public string GetRandomTime()
    {
        TimeSpan timeSpan = endTime - startTime;
        System.Random random = new System.Random();
        double randomSeconds = random.NextDouble() * timeSpan.TotalSeconds;
        DateTime randomTime = startTime.AddSeconds(randomSeconds);

        return randomTime.ToString("yyyy-MM-dd HH:mm");
    }


}
