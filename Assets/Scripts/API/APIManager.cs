using System;  // Action 델리게이트 사용
using System.Collections;  // IEnumerator 사용
using System.Collections.Generic;
using UnityEngine;


/* --- API 요청 중앙 제어 클래스 --- */
/* FastApiClient에 API 요청 위임, JSONParser에 응답 파싱 위임 */
public class APIManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static APIManager Instance;

    // 종속성 (Inspector에서 할당)
    [SerializeField] FastApiClient fastApiClient;               // FastApiClient 할당
    // [SerializeField] JsonParser jsonParser;                     // JsonParser 할당
    [SerializeField] UIManager uiManager;                       // UIManager 할당

    private Coroutine newNotiRequestCoroutine;
    private Coroutine notiListRequestCoroutine;
    private Coroutine notiDetailRequestCoroutine;
    private Coroutine patchNotiRequestCoroutine;
    private Coroutine robotPosRequestCoroutine;

    private const string BASE_URL = "https://222.234.38.97:8443";   // FastAPI 서버 주소
    private string apiPath;             // api 경로 변수
    private string jsonResponse;        // 응답 문자열 변수

    private const float newNotiInterval = 5f;      // 새 알림 자동 조회 간격 (초)
    private Coroutine newNotiCoroutine;             // 새 알림 코루틴

    private int alert_id;  // 삭제할 차량 알림 id

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        if (newNotiCoroutine != null)
        {
            StopCoroutine(newNotiCoroutine);
        }
        newNotiCoroutine = StartCoroutine(CheckNewNotiCoroutine());
    }

    /// <summary>
    /// 새 알림을 5초마다 자동 조회하는 코루틴
    /// </summary>
    private IEnumerator CheckNewNotiCoroutine()
    {
        while (true)
        {
            RequestNewNoti();
            RequestRobotPos();
            yield return new WaitForSeconds(newNotiInterval);
        }
    }

    // 아래 1~5는 UI 버튼 OnClick에 할당하면 됨

    /* --- 1. 새 알림 조회 API 요청 (GET) --- */
    public void RequestNewNoti()
    {
        if (newNotiRequestCoroutine != null)
        {
            StopCoroutine(newNotiRequestCoroutine);
        }

        apiPath = ApiPaths.NEW_NOTI_LIST_API_PATH;

        newNotiRequestCoroutine = StartCoroutine(RequestCoroutine(apiPath, jsonResponse => {
            var response = JsonParser.ParseJson<ApiResponse<NewNotiListData>>(jsonResponse);
            if (response != null && response.data != null)
            {
                uiManager.NewNotiUI(response.data);
            }
        }));
    }
    
    /* --- 2. 주차 위반 리스트 조회 API 요청 (GET) --- */
    public void RequestNotiList()
    {
        if (notiListRequestCoroutine != null)
        {
            StopCoroutine(notiListRequestCoroutine);
        }

        apiPath = ApiPaths.ALL_NOTI_LIST_API_PATH;

        notiListRequestCoroutine = StartCoroutine(RequestCoroutine(apiPath, jsonResponse => {
            var response = JsonParser.ParseJson<ApiResponse<AllNotiListData>>(jsonResponse);
            if (response != null && response.data != null)
            {
                uiManager.NotiListUI(response.data);
            }
        }));
    }

    /* --- 3. 알림 상세 API 요청 (GET) --- */
    public void RequestNotiDetail(int alertID, string reason)
    {
        if (notiDetailRequestCoroutine != null)
        {
            StopCoroutine(notiDetailRequestCoroutine);
        }

        apiPath = $"{ApiPaths.NOTI_DETAIL_API_PATH}{alertID}";  // "/api/notification/detail?id="

        notiDetailRequestCoroutine = StartCoroutine(RequestCoroutine(apiPath, jsonResponse => {
            var response = JsonParser.ParseJson<ApiResponse<NotiDetailData>>(jsonResponse);
            if (response != null && response.data != null)
            {
                uiManager.NotiDetailUI(response.data, reason);
            }
        }));
    }

    /* --- 4. 알림 삭제 API 요청 (PATCH) --- */
    public void RequestPatchNoti(int alertID)
    {
        if (patchNotiRequestCoroutine != null)
        {
            StopCoroutine(patchNotiRequestCoroutine);
        }

        apiPath = $"{ApiPaths.PATCH_NOTI_API_PATH}{alertID}";  // "/api/notification?id="

        patchNotiRequestCoroutine = StartCoroutine(RequestCoroutine(apiPath, jsonResponse => {
            uiManager.PatchNotiUI(alertID);
        }));
    }

    /* --- 5. 로봇 위치 조회 API 요청 (GET) --- */
    public void RequestRobotPos()
    {
        if (robotPosRequestCoroutine != null)
        {
            StopCoroutine(robotPosRequestCoroutine);
        }

        apiPath = $"{ApiPaths.ROBOT_POS_API_PATH}";  // "/api/robot/?id=1"

        robotPosRequestCoroutine = StartCoroutine(RequestCoroutine(apiPath, jsonResponse => {
            var response = JsonParser.ParseJson<ApiResponse<RobotPosData>>(jsonResponse);
            if (response != null && response.data != null)
            {
                uiManager.RobotPosUI(response.data);
            }
        }));
    }


    /// <summary>
    /// 공통 요청 코루틴 : API 요청 및 응답 처리
    /// </summary>
    /// <param name="apiPath"> 요청할 API 경로 </param>
    /// <param name="callback"> 요청 성공 시 호출할 콜백 함수 </param>
    private IEnumerator RequestCoroutine(string apiPath, Action<string> callback)
    {
        // FastApiClient의 GET 요청 코루틴 시작 및 완료 대기
        yield return StartCoroutine(fastApiClient.GetRequestCoroutine(apiPath));
        jsonResponse = fastApiClient.LastJsonResponse;

        Debug.Log($"응답원본 : {jsonResponse}");

        if (!string.IsNullOrEmpty(jsonResponse))
        {
            callback?.Invoke(jsonResponse);  // action callback이 null이 아닐 때 invoke 되도록
        }
        else
        {
            Debug.LogError($"APIManager: API request for path '{apiPath}' failed or returned an empty response.");
        }
    }

    // 조회된 차량 상세 - 위치 표시
    public void VehiclePos(int floor, string zone_name, string rfid_tag)
    {
        uiManager.VehiclePosUI(floor, zone_name, rfid_tag);
    }


}
