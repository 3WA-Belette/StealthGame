using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovement : MonoBehaviour
{
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

        var calculatedDirection = (_direction * _speed * Time.fixedDeltaTime);
        _rb.MovePosition(_rb.transform.position + calculatedDirection);

    }


}
