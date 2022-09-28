using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBrain : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] InputActionReference _moveInput;

    [Header("Actions")]
    [SerializeField] EntityMovement _movement;

    private void Start()
    {
        _moveInput.action.started += MoveUpdate;
        _moveInput.action.performed += MoveUpdate;
        _moveInput.action.canceled += MoveStop;
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
