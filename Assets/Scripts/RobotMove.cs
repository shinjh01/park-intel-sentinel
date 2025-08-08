using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 로봇의 물리적인 이동 */
[RequireComponent(typeof(Rigidbody))]
public class RobotMove : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] float moveForce = 0.05f;       // 이동 가속도 크기
    // [SerializeField] float maxSpeed = 0.1f;         // 최대 이동 속도
    [SerializeField] float rotateSpeed = 0.5f;      // 회전 속도
    // [SerializeField] float decFactor = 0.9f;        // 감속 계수

    [Header("Raycast")]
    [SerializeField] Transform frontRaycast;        // Raycast 시작 위치
    [SerializeField] float raycastDist = 5f;
    [SerializeField] Color rayColor = Color.red;

    private Rigidbody rigid;
    private Vector2 moveDir;    // Input System에서 받은 입력 (WASD)
    //public Transform target;

    private void Awake() {
        rigid = GetComponent<Rigidbody>();
    }

    private void Update() {
        Rotate();
    }

    private void FixedUpdate() {
        Move();

        // Raycast 시각화
        Debug.DrawRay(frontRaycast.position, frontRaycast.forward * raycastDist, rayColor);

        //transform.position = target.position;
    }

    public void SetDir(Vector2 dir) {
        moveDir = dir;
    }

    private void Move() {
        Vector3 forwardForce = transform.forward * moveDir.y * moveForce;
        rigid.AddForce(forwardForce);
    }

    private void Rotate() {
        transform.Rotate(Vector3.up, moveDir.x * rotateSpeed * Time.deltaTime, Space.World);
    }
}
