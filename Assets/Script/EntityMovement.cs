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

    [Header("Jump")]
    [SerializeField] float _gravity = -9.81f;
    [SerializeField] float _jumpHeight = 1f;
    [SerializeField] float _mysteriousNumber = -3f;

    Vector3 _directionFromBrain;
    Vector3 _calculatedDirection;

    public event UnityAction<Vector3> OnMove;

    public Vector3 Direction
    {
        get => _directionFromBrain;
        set => _directionFromBrain = (value).normalized;
    }

    public bool Jump { get; set; }

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

            }
        }
        else // Keep only Y axis for gravity acceleration
        {
            _calculatedDirection.x = 0;
            _calculatedDirection.z = 0;
        }

        // Ground check
        if (_controller.isGrounded)
        {
            _calculatedDirection.y = 0;
        }

        if (Jump)
        {
            Jump = false;
            if(_controller.isGrounded)
            {
                _calculatedDirection.y += Mathf.Sqrt(_jumpHeight * _mysteriousNumber * _gravity);
            }
        }

        // Apply gravity
        _calculatedDirection.y += _gravity * Time.deltaTime;

        _controller.Move(_calculatedDirection * Time.deltaTime);
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
