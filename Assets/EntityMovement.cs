using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] Rigidbody _rb;
    [SerializeField] float _speed;

    Vector3 _direction;

    public Vector3 Direction
    {
        get => _direction;
        set => _direction = (value).normalized;
    }

    void FixedUpdate()
    {
        // Move rigidbody
        var calculatedDirection = (_direction * _speed * Time.fixedDeltaTime);
        calculatedDirection = _camera.transform.TransformDirection(calculatedDirection);
        
        _rb.MovePosition(_rb.transform.position + calculatedDirection);

        // Look At
        var lookAtDirection = new Vector3(_camera.transform.forward.x, 0, _camera.transform.forward.z);
        _rb.transform.LookAt(_rb.transform.position + lookAtDirection);
    }


}
