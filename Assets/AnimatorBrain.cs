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
        _movement.OnMove += _movement_OnMove;


    }

    private void _movement_OnMove(Vector3 arg0)
    {
        _animator.SetFloat(_speedNameFloat, arg0.magnitude);


    }
}
