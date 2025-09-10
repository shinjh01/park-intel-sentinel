using System.Collections;
using System.Collections.Generic;  // List<T>를 사용하기 위함
using UnityEngine;
using Newtonsoft.Json;  // Newtonsoft.Json 라이브러리 사용

/* --- JSON 문자열을 C# 객체로 역직렬화(파싱)하는 클래스 --- */
public static class JsonParser
{
    /// <summary>
    /// JSON 문자열을 지정된 C# 타입의 객체로 역직렬화(파싱)
    /// 'where T : class' : T가 참조 타입(클래스)임을 제약
    /// </summary>
    /// <typeparam name="T"> 파싱할 대상 C# 클래스 타입</typeparam>
    /// <param name="jsonToParse"> JSON 문자열 </param>
    /// <returns> 성공 시 T 타입의 객체 반환, 실패 시 null 반환 </returns>
    public static T ParseJson<T>(string jsonToParse) where T : class
    {
        // 입력 문자열이 유효한지 확인
        if (string.IsNullOrEmpty(jsonToParse))
        {
            Debug.LogError($"JsonParser: JSON string for type {typeof(T).Name} is null or empty. Cannot parse.");
            return null;
        }

        try
        {
            // JSON 문자열을 C# 객체로 변환
            T parsedObject = JsonConvert.DeserializeObject<T>(jsonToParse);
            return parsedObject;
        }
        catch (JsonException ex)
        {
            // JSON 형식 오류 등 Newtonsoft.Json 관련 예외 처리
            Debug.LogError($"JsonParser: Failed to parse JSON to type {typeof(T).Name}. Json Error: {ex.Message}");
            return null; // 파싱 실패 시 null 반환
        }
        catch (System.Exception ex)
        {
            // 그 외 예상치 못한 일반적인 예외 처리
            Debug.LogError($"JsonParser: An unexpected error occurred during JSON parsing to type {typeof(T).Name}. General Error: {ex.Message}");
            return null;
        }
    }
}
