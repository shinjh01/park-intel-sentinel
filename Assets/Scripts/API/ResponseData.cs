using System;
using System.Collections.Generic;
using UnityEngine;


// 전체 JSON 응답을 위한 일반적인 컨테이너
[Serializable]
public class ApiResponse<T>
{
    public string result;
    public T data;
}

// 위반 차량 모델 (기본)
[Serializable]
public class NotiData
{
    public int id;                          // 알림 id
    public int floor;                       // 차량 위치 (층)
    public string name;                     // 차량 위치 (구역)
    public string rfid_tag;                 // 차량 위치 (센서 위치)
    public string plate_text;               // 차량 번호
    public string reason;                   // 위반 유형
    public DateTime? created_at;            // 위반 시간
    public bool is_checked;                 // 새 알림 확인 여부
}

// 새 알림 조회 응답 모델 (확인하지 않은 리스트)
[Serializable]
public class NewNotiListData
{
    public List<NotiData> logs;
}

// 주차 위반 리스트 조회 응답 모델 (모든 리스트)
[Serializable]
public class AllNotiListData
{
    public List<NotiData> data;
}

// 알림 상세 응답 모델
[Serializable]
public class NotiDetailData
{
    public int id;                          // 알림 id
    public int floor;                       // 차량 위치 (층)
    public string name;                     // 차량 위치 (구역)
    public string rfid_tag;                 // 차량 위치 (센서 위치)
    public string plate_text;               // 차량 번호
    public string phone_number;             // 차주 전화번호
    public DateTime? entered_at;            // 입차 시간
    public string reason;                   // 위반 유형
    public DateTime? created_at;            // 위반 시간
    public bool is_checked;                 // 새 알림 확인 여부
}

// 로봇 위치 조회 응답 모델
[Serializable]
public class RobotPosData
{
    public int robot_id;            // 로봇카 id
    public int floor;               // 로봇카 위치 (층)
    public string rfid_tag;         // 태그된 센서 id
    public DateTime? created_at;    // 센서에 태그된 시간
    public string message;          // 메세지
    public Dictionary<string, List<VehicleData>> vehicles;  // 차량 정보
}

// 각 RFID 태그에 연결된 차량 정보 응답 모델
[Serializable]
public class VehicleData
{
    public string name;         // 차량 위치 (구역)
    public string plate_text;   // 차량 번호
    public string car_type;     // 차량 종류
}



