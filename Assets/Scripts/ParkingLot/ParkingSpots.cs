using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SensorData
{
    public GameObject rfidTag;
    public List<GameObject> zones;
}

public class ParkingSpots : MonoBehaviour
{
    public List<SensorData> allSensors;
    private Dictionary<string, SensorData> sensorIdToData;

    private Dictionary<GameObject, Color> originalColors = new Dictionary<GameObject, Color>();
    private Dictionary<GameObject, Coroutine> blinkingCoroutines = new Dictionary<GameObject, Coroutine>();

    private Dictionary<string, List<GameObject>> allVehicles = new Dictionary<string, List<GameObject>>();
    public List<GameObject> carObjects;

    private List<string> sensorIDs = new List<string>
    {
        "0643E69B", "0642919D", "09BBBC4B1", "0643A29A", "06441B01", "0642CD01", "09491A91"
    };

    private void Awake()
    {
        InitSensorMap();
        SaveOriginColor();
        InitVehicles();
    }

    // Sensor와 Zone의 정보를 초기에 저장
    private void InitSensorMap()
    {
        sensorIdToData = new Dictionary<string, SensorData>();

        // Inspector에서 할당한 allSensors 리스트를 사용하여 Dictionary 채우기
        for (int i = 0; i < sensorIDs.Count && i < allSensors.Count; i++)
        {
            string id = sensorIDs[i];
            SensorData data = allSensors[i];

            // 키-값 쌍으로 추가
            if (!sensorIdToData.ContainsKey(id))
            {
                sensorIdToData.Add(id, data);
            }
            else
            {
                Debug.LogWarning($"중복된 센서 ID: {id}");
            }
        }
    }

    // 주차칸의 색상 정보를 초기에 저장
    private void SaveOriginColor()
    {
        Debug.LogWarning($"색상 저장");
        foreach (var sensorData in allSensors)
        {
            if (sensorData.zones != null)
            {
                foreach (var zoneObject in sensorData.zones)
                {
                    if (zoneObject != null)
                    {
                        MeshRenderer mr = zoneObject.GetComponent<MeshRenderer>();
                        // 키를 오브젝트 자체로 변경
                        if (mr != null && !originalColors.ContainsKey(zoneObject))
                        {
                            originalColors.Add(zoneObject, mr.material.color);
                        }
                    }
                }
            }
        }
    }

    // 모든 차량 오브젝트 초기화
    private void InitVehicles()
    {
        allVehicles.Clear();

        int carIndex = 0;
        foreach (string rfidTagId in sensorIDs) // sensorIDs 리스트 순서 기준으로 순회
        {
            // sensorIdToData 딕셔너리에서 해당 rfidTagId의 SensorData를 찾아옴
            if (sensorIdToData.TryGetValue(rfidTagId, out SensorData sensorData))
            {
                if (sensorData.zones != null && sensorData.zones.Count > 0)
                {
                    List<GameObject> vehiclesForSensor = new List<GameObject>();
                    
                    // 각 zone에 해당하는 차량 오브젝트를 리스트에 추가
                    for (int i = 0; i < sensorData.zones.Count; i++)
                    {
                        if (carIndex < carObjects.Count)
                        {
                            GameObject vehicle = carObjects[carIndex];
                            vehiclesForSensor.Add(vehicle);
                            vehicle.SetActive(false); // 초기 상태는 비활성화
                            carIndex++;
                        }
                    }
                    
                    // rfidTagId를 키로 하여 딕셔너리에 추가
                    allVehicles.Add(rfidTagId, vehiclesForSensor);
                }
            }
        }
    }

    // 깜빡이는 효과를 위한 코루틴 메소드
    private IEnumerator BlinkColor(GameObject targetZone, Color blinkColor, float interval)
    {
        MeshRenderer meshRenderer = targetZone.GetComponent<MeshRenderer>();
        Color originalColor = originalColors[targetZone]; // 미리 저장해둔 원래 색상

        if (meshRenderer == null) yield break;

        while (true)
        {
            // 빨간색으로 변경
            meshRenderer.material.color = blinkColor;
            yield return new WaitForSeconds(interval); // 지정된 시간(interval)만큼 대기

            // 원래 색상으로 변경
            meshRenderer.material.color = originalColor;
            yield return new WaitForSeconds(interval);
        }
    }

    // 차량 주차된 칸 빨간색 표시
    public void SetParkingSpotColor(int floor, string zone_name, string rfid_tag)
    {
        Debug.Log($"floor: {floor}, zone_name: {zone_name}, rfid_tag: {rfid_tag}");

        // 딕셔너리에서 센서 ID(rfid_tag)를 키로 사용하여 SensorData를 바로 찾음
        if (sensorIdToData.TryGetValue(rfid_tag, out SensorData sensor))
        {
            int zoneNumber;
            if (int.TryParse(zone_name.Replace("ZONE", ""), out zoneNumber))
            {
                int zoneIndex = zoneNumber - 1;

                if (zoneIndex >= 0 && zoneIndex < sensor.zones.Count)
                {
                    GameObject targetZone = sensor.zones[zoneIndex];

                    // 기존 코루틴이 있다면 중지
                    if (blinkingCoroutines.ContainsKey(targetZone) && blinkingCoroutines[targetZone] != null)
                    {
                        StopCoroutine(blinkingCoroutines[targetZone]);
                    }
                    
                    // 새로운 코루틴 시작
                    Coroutine newCoroutine = StartCoroutine(BlinkColor(targetZone, Color.red, 0.5f));
                    blinkingCoroutines[targetZone] = newCoroutine; // 딕셔너리에 저장
                    
                    // MeshRenderer meshRenderer = targetZone.GetComponent<MeshRenderer>();
                    // if (meshRenderer != null)
                    // {
                    //     // 파라미터 대신 메서드 내에서 직접 색상 정의
                    //     meshRenderer.material.color = Color.red; 
                    // }
                }
            }
        }
        else
        {
            Debug.LogWarning($"RFID 태그 '{rfid_tag}'에 해당하는 센서를 찾을 수 없습니다.");
        }
    }

    // 빨간색을 원래 색으로 되돌리기
    public void ResetAllParkingSpotsColor()
    {
        foreach (var entry in blinkingCoroutines)
        {
            if (entry.Value != null)
            {
                StopCoroutine(entry.Value);
            }
        }
        blinkingCoroutines.Clear();

        foreach (var originalColor in originalColors)
        {
            GameObject zoneObject = originalColor.Key;
            Color colorToRestore = originalColor.Value;

            if (zoneObject != null)
            {
                MeshRenderer mr = zoneObject.GetComponent<MeshRenderer>();
                if (mr != null)
                {
                    mr.material.color = colorToRestore;
                }
            }
        }
    }

    // 차량 오브젝트 활성화
    public void DisplayParkingStatus(RobotPosData robotPos)
    {
        if (robotPos?.vehicles != null)
        {
            foreach (var vehicleEntry in robotPos.vehicles)
            {
                string rfid_tag = vehicleEntry.Key;
                List<VehicleData> vehicleList = vehicleEntry.Value;
                
                foreach (var vehicle in vehicleList)
                {
                    // plate_text가 비어있지 않으면 (차량이 있으면)
                    if (!string.IsNullOrEmpty(vehicle.plate_text))
                    {
                        string zone_name = vehicle.name;
                        
                        // 차량 오브젝트 활성화
                        ActivateVehicle(rfid_tag, zone_name);
                    }
                }
            }
        }
    }

    // 특정 위치의 차량 오브젝트 활성화
    private void ActivateVehicle(string rfid_tag, string zone_name)
    {
        if (allVehicles.TryGetValue(rfid_tag, out List<GameObject> vehicleList))
        {
            int zoneNumber;
            if (int.TryParse(zone_name.Replace("ZONE", ""), out zoneNumber))
            {
                int zoneIndex = zoneNumber - 1;
                
                if (zoneIndex >= 0 && zoneIndex < vehicleList.Count)
                {
                    GameObject vehicle = vehicleList[zoneIndex];
                    if (vehicle != null)
                    {
                        vehicle.SetActive(true);
                    }
                }
            }
        }
    }

}
