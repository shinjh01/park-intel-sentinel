using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* --- 전체 UI 관리 클래스 --- */
public class UIManager : MonoBehaviour
{
    // UI 스크립트 인스턴스
    [Header("UI Component")]
    [SerializeField] GameObject menuUI;
    [SerializeField] NewNotiUI newNotiUI;
    [SerializeField] NotiListUI notiListUI;
    [SerializeField] NotiDetailUI notiDetailUI;
    [SerializeField] GameObject popupUI;
    [SerializeField] RobotPosUI robotPosUI;
    [SerializeField] GameObject popupBlocker;
    [SerializeField] GameObject resetButton;

    private void Awake()
    {
        ShowMenuUI();
    }

    /* 1. 새 알림 UI */
    public void NewNotiUI(NewNotiListData newNotiList)
    {
        ShowMenuUI();
        newNotiUI.NewNoti(newNotiList);
    }

    /* 2. 주차 위반 리스트 UI */
    public void NotiListUI(AllNotiListData allNotiList)
    {
        ShowNotiListUI();
        notiListUI.NotiList(allNotiList);
    }
    public void OnClickNotiListBtn()
    {
        APIManager.Instance?.RequestNotiList();
    }
    
    /* 3. 위반 차량 상세 UI */
    public void NotiDetailUI(NotiDetailData notiDetail)
    {
        ShowNotiDetailUI();
        notiDetailUI.NotiDetail(notiDetail);
    }

    /* 4. 위반 차량 삭제 UI */
    public void PatchNotiUI(int alertID)
    {
        // patchNotiUI.PatchNoti(alertID);
    }
    
    /* 5. 로봇 위치 조회 UI */
    public void RobotPosUI(RobotPosData robotPos)
    {
        robotPosUI.RobotPos(robotPos);
    }
    public void RobotPosPopupUI()
    {
        popupUI.SetActive(true);
        popupBlocker.SetActive(true);
        resetButton.SetActive(true);
    }
    public void OnClickRobotPosBtn()
    {
        APIManager.Instance?.RequestRobotPos();
    }

    // 차량 상세 - 차량 위치 표시
    public void VehiclePosUI(int floor, string zone_name, string rfid_tag)
    {
        popupUI.SetActive(true);
        popupBlocker.SetActive(true);
        resetButton.SetActive(false);
        robotPosUI.VehiclePos(floor, zone_name, rfid_tag);
        
    }

    private void ShowMenuUI()
    {
        menuUI.SetActive(true);
        notiListUI.gameObject.SetActive(false);
        notiDetailUI.gameObject.SetActive(false);
        popupUI.SetActive(false);
        popupBlocker.SetActive(false);
    }

    private void ShowNotiListUI()
    {
        menuUI.SetActive(false);
        notiListUI.gameObject.SetActive(true);
        notiDetailUI.gameObject.SetActive(false);
        popupUI.SetActive(false);
        popupBlocker.SetActive(false);
    }

    private void ShowNotiDetailUI()
    {
        menuUI.SetActive(false);
        notiListUI.gameObject.SetActive(false);
        notiDetailUI.gameObject.SetActive(true);
        popupUI.SetActive(false);
        popupBlocker.SetActive(false);
    }

    public void ClosePopupUI()
    {
        popupUI.SetActive(false);
        popupBlocker.SetActive(false);
        robotPosUI.ResetColor();
    }

}
