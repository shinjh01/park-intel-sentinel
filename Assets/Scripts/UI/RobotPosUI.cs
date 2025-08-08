using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RobotPosUI : MonoBehaviour
{
    [SerializeField] ParkingSpots parkingSpots;

    // 조회된 차량 상세 - 위치 표시
    public void VehiclePos(int floor, string zone_name, string rfid_tag)
    {
        parkingSpots.SetParkingSpotColor(floor, zone_name, rfid_tag);
    }

    // 주차장 주차 현황 및 로봇 위치 표시
    public void RobotPos(RobotPosData robotPos)
    {
        parkingSpots.DisplayParkingStatus(robotPos);
    }

    public void ResetColor()
    {
        parkingSpots.ResetAllParkingSpotsColor();
    }



}
