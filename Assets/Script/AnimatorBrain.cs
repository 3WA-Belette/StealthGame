using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimatorBrain : MonoBehaviour
{
    [SerializeField] Animator _animator;

    [SerializeField, Foldout("Actions")] EntityMovement _movement;

    [SerializeField, Foldout("Params"), AnimatorParam(nameof(_animator))] string _speedNameFloat;
    [SerializeField, Foldout("Params"), AnimatorParam(nameof(_animator))] string _xDirectionNameFloat;
    [SerializeField, Foldout("Params"), AnimatorParam(nameof(_animator))] string _zDirectionNameFloat;

    void Start()
    {
        _movement.OnMove += MovementInjection;
    }

    private void MovementInjection(Vector3 dir)
    {
        // Create a vector from X and Z
        var fakeDirection = new Vector3(dir.x, 0, dir.z);

        // Use that for speed
        if(fakeDirection.magnitude > 0.005f)
        {
            _animator.SetFloat(_speedNameFloat, 1f);
        }
        else
        {
            _animator.SetFloat(_speedNameFloat, 0f);
        }

        // and normalize vector and inject to X and Y animator
        fakeDirection.Normalize();
        _animator.SetFloat(_xDirectionNameFloat, fakeDirection.x);
        _animator.SetFloat(_zDirectionNameFloat, fakeDirection.z);

    }
}
