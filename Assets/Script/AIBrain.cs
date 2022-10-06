using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class AIBrain : MonoBehaviour
{
    enum AIState { PATROL = 1,CHASE=2, CATCH = 3 }

    [Header("Base")]
    [SerializeField] Transform _root;

    [Header("Patrol Conf")]
    [SerializeField] Transform[] _patrolPath;
    [SerializeField] float _destinationDistance;

    [Header("Chase Conf")]
    [SerializeField] float _loseDistance;

    [Header("Actions")]
    [SerializeField] NavMeshAgent _agent;
    [SerializeField] EntityLooseGame _lose;
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
        _agent.SetDestination(_patrolPath[_patrolCurrentIndex].position);
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
                
                if(distanceToPlayer < _loseDistance) // Transition to Attack
                {
                    _internalState = AIState.CATCH;
                }

                break;
            case AIState.CATCH:
                if (_detectPlayer.Target == null)   // Transition to Patrol
                {
                    _internalState = AIState.PATROL;
                    break;
                }
                // Actions
                Catch();

                // Transitions
                var distanceToPlayer2 = Vector3.Distance(_root.transform.position, _detectPlayer.Target.transform.position);
                if (distanceToPlayer2 > _loseDistance) // Transition to Chase
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
        // Estimate distance to change destination
        var patrolDestination = _patrolPath[_patrolCurrentIndex].position;
        _patrolDistanceToDestination = Vector3.Distance(_root.transform.position, patrolDestination);
        if (_patrolDistanceToDestination < _destinationDistance)
        {
            _patrolCurrentIndex++;
            // On a dépassé la taille du tableau donc on retourne vers l'élément 0
            if (_patrolCurrentIndex >= _patrolPath.Length)
            {
                _patrolCurrentIndex = 0;
            }

            _agent.SetDestination(_patrolPath[_patrolCurrentIndex].position);
        }
    }

    public void Chase()
    {
        // Move to
        _agent.SetDestination(_detectPlayer.Target.transform.position);
    }

    public void Catch()
    {
        _agent.isStopped = true;
        _lose.Activate();
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
            var pos = _patrolPath.Select(i => i.position).ToArray();
            // Draw Lines
            Gizmos.color = Color.yellow;
            for (int i = 0; i < _patrolPath.Length - 1; i++)
            {
                Gizmos.DrawLine(_patrolPath[i].position, _patrolPath[i + 1].position);
            }
            Gizmos.DrawLine(_patrolPath[0].position, _patrolPath[pos.Length - 1].position);

            // Then draw spheres
            Gizmos.color = Color.blue;
            foreach (var el in pos)
            {
                Gizmos.DrawSphere(el, _radiusGizmos);
            }
        }
    }
#endif

    #endregion

}
