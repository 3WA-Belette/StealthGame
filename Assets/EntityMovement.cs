using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityMovement : MonoBehaviour
{
    [SerializeField] bool _followCameraOrientation;
    [SerializeField, ShowIf(nameof(_followCameraOrientation))] Camera _camera;
    [SerializeField] CharacterController _controller;
    [SerializeField] float _speed;

    Vector3 _directionFromBrain;
    Vector3 _calculatedDirection;

    public event UnityAction<Vector3> OnMove;

    public Vector3 Direction
    {
        get => _directionFromBrain;
        set => _directionFromBrain = (value).normalized;
    }

    void Update()
    {
        // Move character controller
        if (_directionFromBrain.magnitude > 0.01f)
        {
            if (_followCameraOrientation)   // Camera based algo
            {
                var tmpDirection = (_directionFromBrain * _speed * Time.deltaTime);
                var forwardForCamera = _camera.transform.TransformDirection(tmpDirection);
                _calculatedDirection.x = forwardForCamera.x;
                _calculatedDirection.z = forwardForCamera.z;
            }
            else
            {
                // Enemy algo
            }
        }
        else // Keep only Y axis for gravity acceleration
        {
            _calculatedDirection.x = 0;
            _calculatedDirection.z = 0;
        }

        // Apply gravity
        if(_controller.isGrounded)
        {
            _calculatedDirection.y = 0;
        }
        else
        {
            _calculatedDirection.y += (Physics.gravity.y/3) * Time.deltaTime;
        }

        _controller.Move(_calculatedDirection);
        OnMove?.Invoke(_calculatedDirection);

        // Look At
        if (_followCameraOrientation)   // Follow camera orientation
        {
            var lookAtDirection = new Vector3(_camera.transform.forward.x, 0, _camera.transform.forward.z);
            _controller.transform.LookAt(_controller.transform.position + lookAtDirection);
        }
        else  // Follow direction applied
        {

        }
        
    }

}
