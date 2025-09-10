using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


// 모든 인증서를 무조건 신뢰하는 커스텀 CertificateHandler
// 개발 및 테스트 환경에서만 사용 (보안상 위험하므로 운영 시 사용 X)
public class TrustAllCertificates : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        // 여기서는 모든 인증서를 무조건 '유효하다'고 판단합니다.
        // 실제 운영 환경에서는 서버의 인증서를 검증하는 로직이 필요합니다.
        return true;
    }
}


/* --- API 요청 & JSON 응답 저장 클래스 --- */
public class FastApiClient : MonoBehaviour
{
    // FastAPI 서버 주소
    [SerializeField] private string baseURL = "https://222.234.38.97:8443";

    // 마지막으로 받은 JSON 응답을 저장 (외부에서 읽기만 가능)
    public string LastJsonResponse { get; private set; } = string.Empty;

    // 선택 사항: 요청이 진행 중인지 여부를 나타내는 플래그
    public bool IsRequesting { get; private set; } = false;

    // 현재 실행 중인 요청 코루틴 참조 (이전 요청 중지를 위함)
    private Coroutine curRequestCoroutine;


    /// <summary>
    /// GET 요청 코루틴
    /// </summary>
    /// <param name="apiPath"> baseURL 뒤에 붙을 api 경로 </param>
    public IEnumerator GetRequestCoroutine(string apiPath)
    {
        // 이전 요청이 있다면 중지
        if (curRequestCoroutine != null)
        {
            StopCoroutine(curRequestCoroutine);
            Debug.LogWarning("APIClient: Previous request stopped to start a new one.");
        }

        IsRequesting = true;  // 요청 시작 플래그 설정
        LastJsonResponse = string.Empty;  // 이전 응답 초기화

        string fullUrl = $"{baseURL}{apiPath}";
        Debug.Log($"Requesting from: {fullUrl}");

        using (UnityWebRequest webRequest = UnityWebRequest.Get(fullUrl))
        {
            // 인증서 핸들러 설정 (UnityWebRequest 객체가 생성된 직후에 설정)
            webRequest.certificateHandler = new TrustAllCertificates();

            // 웹 요청을 보내고 응답을 기다림
            yield return webRequest.SendWebRequest();

            // 요청 결과에 따라 분기하여 처리
            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                // Connection Error : JSON 응답은 비어있음
                Debug.LogError($"Connection Error: {webRequest.error}");
            }
            else if (webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                // HTTP Error : 응답 본문이 있을 수 있으므로 저장
                Debug.LogError($"HTTP Error ({webRequest.responseCode}): {webRequest.downloadHandler.text}");
                LastJsonResponse = webRequest.downloadHandler.text; 
            }
            else if (webRequest.result == UnityWebRequest.Result.Success)
            {
                // Success : 요청이 성공적으로 완료되면 JSON 응답을 저장
                LastJsonResponse = webRequest.downloadHandler.text;
                Debug.Log($"Received Raw JSON Response: {LastJsonResponse}");
            }
            else
            {
                // 기타 오류 : 응답 본문이 있다면 저장
                Debug.LogError($"Unhandled WebRequest Result: {webRequest.result} - {webRequest.error}");
                LastJsonResponse = webRequest.downloadHandler.text;
            }
        }
        IsRequesting = false; // 요청 완료 플래그 해제
    }


    // JSON 응답 문자열 반환 (다른 스크립트에서 사용)
    public string GetLastJsonResponse()
    {
        Debug.Log("========== LastJsonResponse ==========");
        Debug.Log(LastJsonResponse);
        Debug.Log("======================================");
        return LastJsonResponse;
    }
}
