using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBrain : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] InputActionReference _moveInput;
    [SerializeField] InputActionReference _jumpInput;

    [Header("Actions")]
    [SerializeField] EntityMovement _movement;

    private void Start()
    {
        _moveInput.action.started += MoveUpdate;
        _moveInput.action.performed += MoveUpdate;
        _moveInput.action.canceled += MoveStop;

        _jumpInput.action.started += LaunchJump;
    }

    internal void StopInput()
    {
        _movement.Direction = Vector3.zero;
        _moveInput.action.actionMap.Disable();
    }

    private void LaunchJump(InputAction.CallbackContext obj)
    {
        _movement.Jump = true;
    }

    private void OnDestroy()
    {
        _moveInput.action.started -= MoveUpdate;
        _moveInput.action.performed -= MoveUpdate;
        _moveInput.action.canceled -= MoveStop;
    }

    private void MoveUpdate(InputAction.CallbackContext obj)
    {
        var joystick = obj.ReadValue<Vector2>();
        _movement.Direction = new Vector3(joystick.x, 0, joystick.y);
    }

    private void MoveStop(InputAction.CallbackContext obj)
    {
        _movement.Direction = Vector3.zero;

    }

}
