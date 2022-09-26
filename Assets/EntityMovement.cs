using NaughtyAttributes;
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

    Vector3 _direction;
    Vector3 _calculatedDirection;

    public event UnityAction<Vector3> OnMove;

    public Vector3 Direction
    {
        get => _direction;
        set => _direction = (value).normalized;
    }

    void FixedUpdate()
    {
        // Move character controller
        if(_followCameraOrientation)
        {
            var tmpDirection = (_direction * _speed * Time.fixedDeltaTime);
            _calculatedDirection = _camera.transform.TransformDirection(tmpDirection);
        }

        _controller.Move(_calculatedDirection);
        OnMove?.Invoke(_calculatedDirection);

        // Look At
        if (_followCameraOrientation)
        {
            var lookAtDirection = new Vector3(_camera.transform.forward.x, 0, _camera.transform.forward.z);
            _controller.transform.LookAt(_controller.transform.position + lookAtDirection);
        }
    }


}
