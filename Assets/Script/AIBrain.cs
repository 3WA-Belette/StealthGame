using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIBrain : MonoBehaviour
{
    [Header("Base")]
    [SerializeField] Transform _root;

    [Header("Patrol Conf")]
    [SerializeField] Transform[] _patrolPath;
    [SerializeField] float _destinationDistance;

    [Header("Actions")]
    [SerializeField] EntityMovement _movement;
    [SerializeField] DetectPlayer _vision;

    [Header("Animator")]
    [SerializeField] Animator _aiGraph;
    [SerializeField, AnimatorParam(nameof(_aiGraph))] string _isInVisionBoolParam;
    [SerializeField, AnimatorParam(nameof(_aiGraph))] string _distanceToTargetFloatParam;

    int _patrolCurrentIndex;
    float _patrolDistanceToDestination;

    public Vector3 CurrentDestination => SampledPath[_patrolCurrentIndex];
    public Vector3[] SampledPath { get; private set; }

    void Reset()
    {
        _destinationDistance = 0.5f;
    }

    void Awake()
    {
        // Prepare a list (version avancée)
        SampledPath = _patrolPath.Select(i => i.position).ToArray();
    }  

    void Update()
    {
        if(_vision.Target != null)
        {
            _aiGraph.SetBool(_isInVisionBoolParam, true);
            _aiGraph.SetFloat(_distanceToTargetFloatParam, Vector3.Distance(transform.position, _vision.Target.transform.position));
        }
        else
        {
            _aiGraph.SetBool(_isInVisionBoolParam, false);
            _aiGraph.SetFloat(_distanceToTargetFloatParam, 1000f);
        }
    }

    public void Patrol()
    {
        // Move to
        _movement.Direction = CurrentDestination - transform.position;

        // Estimate distance to change destination
        _patrolDistanceToDestination = Vector3.Distance(transform.position, CurrentDestination);
        if (_patrolDistanceToDestination < _destinationDistance)
        {
            _patrolCurrentIndex++;
            // On a dépassé la taille du tableau donc on retourne vers l'élément 0
            if (_patrolCurrentIndex >= SampledPath.Length)
            {
                _patrolCurrentIndex = 0;
            }
        }
    }

    public void Chase()
    {

    }

    public void Attack()
    {

    }

    #region EDITOR
#if UNITY_EDITOR
    [SerializeField, Foldout("Editor")] float _radiusGizmos = 0.5f;
    private void OnDrawGizmos()
    {
        if (_patrolPath == null || _patrolPath.Length == 0) return;

        if(Application.isEditor)
        {
            Gizmos.color = Color.blue;
            if(Application.isPlaying)
            {
                DrawTransforms(SampledPath);
            }
            else
            {
                DrawTransforms(_patrolPath.Select(i => i.position).ToArray());
            }
        }
    }

    void DrawTransforms(Vector3[] pos)
    {
        // Draw Lines
        Gizmos.color = Color.yellow;
        for (int i = 0; i < pos.Length - 1; i++)
        {
            Gizmos.DrawLine(pos[i], pos[i + 1]);
        }
        Gizmos.DrawLine(pos[0], pos[pos.Length - 1]);

        // Then draw spheres
        Gizmos.color = Color.blue;
        foreach (var el in pos)
        {
            Gizmos.DrawSphere(el, _radiusGizmos);
        }
    }
#endif

    #endregion

}
