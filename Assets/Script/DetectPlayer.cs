using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    [SerializeField, Layer] string _mapLayer;

    PlayerTag _target;
        
    public PlayerTag Target => _target;

    void OnTriggerEnter(Collider other)
    {
        CheckPlayer(other);
    }
    void OnTriggerStay(Collider other)
    {
        CheckPlayer(other);
    }

    void CheckPlayer(Collider other)
    {
        if (other.TryGetComponent(out PlayerTag target))
        {
            var origin = transform.position;
            var direction = target.transform.position - transform.position;
            if (Physics.Raycast(origin, direction, direction.magnitude, LayerMask.GetMask(_mapLayer)))
            {
                Debug.DrawLine(origin, origin + direction, Color.red, 1f);
            }
            else
            {
                Debug.DrawLine(origin, origin + direction, Color.green, 1f);
                _target = target;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerTag target) &&  target == _target)
        {
            _target = null;
        }
    }

}
