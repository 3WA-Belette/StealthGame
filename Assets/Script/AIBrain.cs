using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIBrain : MonoBehaviour
{
    enum AIState { PATROL = 1,CHASE=2,ATTACK = 3 }

    [Header("Base")]
    [SerializeField] Transform _root;

    [Header("Patrol Conf")]
    [SerializeField] Transform[] _patrolPath;
    [SerializeField] float _destinationDistance;

    [Header("Chase Conf")]
    [SerializeField] float _attackThreshold;

    [Header("Actions")]
    [SerializeField] EntityMovement _movement;
    [SerializeField] DetectPlayer _detectPlayer;

    [ShowNonSerializedField] AIState _internalState;
    [ShowNonSerializedField] int _patrolCurrentIndex;
    [ShowNonSerializedField] float _patrolDistanceToDestination;

    void Reset()
    {
        _destinationDistance = 0.5f;
    }

    void Awake()
    {
        _internalState = AIState.PATROL;
    }  

    void Update()
    {
        //State Machine implementation
        switch (_internalState)
        {
            case AIState.PATROL:
                // Actions
                Patrol();

                // Transitions
                if(_detectPlayer.Target != null) // Transition to Chase 
                {
                    _internalState = AIState.CHASE;
                }
                break;
            case AIState.CHASE:
                if (_detectPlayer.Target == null)   // Transition to Patrol
                {
                    _internalState = AIState.PATROL;
                    break;
                }

                // Actions
                Chase();

                // Transitions
                var distanceToPlayer = Vector3.Distance(_root.transform.position, _detectPlayer.Target.transform.position);
                
                if(distanceToPlayer < _attackThreshold) // Transition to Attack
                {
                    _internalState = AIState.ATTACK;
                }
                break;
            case AIState.ATTACK:
                if (_detectPlayer.Target == null)   // Transition to Patrol
                {
                    _internalState = AIState.PATROL;
                    break;
                }
                // Actions
                Attack();

                // Transitions
                var distanceToPlayer2 = Vector3.Distance(_root.transform.position, _detectPlayer.Target.transform.position);
                if (distanceToPlayer2 > _attackThreshold) // Transition to Chase
                {
                    _internalState = AIState.CHASE;

                }
                break;
            default:
                break;
        }
    }

    public void Patrol()
    {
        // Move to
        var patrolDestination = _patrolPath[_patrolCurrentIndex].position;
        _movement.Direction = patrolDestination - transform.position;

        // Estimate distance to change destination
        _patrolDistanceToDestination = Vector3.Distance(_root.transform.position, patrolDestination);
        if (_patrolDistanceToDestination < _destinationDistance)
        {
            _patrolCurrentIndex++;
            // On a dépassé la taille du tableau donc on retourne vers l'élément 0
            if (_patrolCurrentIndex >= _patrolPath.Length)
            {
                _patrolCurrentIndex = 0;
            }
        }
    }

    public void Chase()
    {
        // Move to
        _movement.Direction = _detectPlayer.Target.transform.position - _root.transform.position;
    }

    public void Attack()
    {
        _movement.Direction = Vector3.zero;
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
            DrawTransforms(_patrolPath.Select(i => i.position).ToArray());
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
