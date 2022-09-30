using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

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
            Vector3 tmpDirection = (_directionFromBrain * _speed * Time.deltaTime);
            if (_followCameraOrientation)   // Camera based algo
            {
                tmpDirection = _camera.transform.TransformDirection(tmpDirection);
            }
            
            _calculatedDirection.x = tmpDirection.x;
            _calculatedDirection.z = tmpDirection.z;
        }
        else // Keep only Y axis for gravity acceleration
        {
            _calculatedDirection.x = 0;
            _calculatedDirection.z = 0;
        }

        #region Y axis
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
        #endregion

        _controller.Move(_calculatedDirection * Time.deltaTime);
        OnMove?.Invoke(_calculatedDirection);

        // Look At
        if (_followCameraOrientation)   // Follow camera orientation
        {
            var cameraYRotation = _camera.transform.rotation.eulerAngles.y;
            _controller.transform.rotation = Quaternion.Euler(0, cameraYRotation, 0);
        }
        else  // Follow direction applied
        {
            _controller.transform.LookAt(_controller.transform.position + 
                new Vector3(_calculatedDirection.x, 0, _calculatedDirection.z));
        }
        
    }

}
