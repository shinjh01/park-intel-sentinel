using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RobotController : MonoBehaviour
{
    [SerializeField] RobotMove robotMove;
    // [SerializeField] RobotSensorManager robotSensorManager;

    private PlayerInput playerInput;
    private Vector2 inputVec;

    private void Awake() {
        robotMove = GetComponent<RobotMove>();
        playerInput = GetComponent<PlayerInput>();
    }

    // InputActions : Robot - Move
    public void OnMove(InputValue value) {
        inputVec = value.Get<Vector2>();
        robotMove.SetDir(inputVec);   
    }
    
}
