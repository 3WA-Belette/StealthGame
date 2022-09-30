using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    PlayerTag _target;
        
    public PlayerTag Target => _target;

    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerTag target))
        {
            _target = target;
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
