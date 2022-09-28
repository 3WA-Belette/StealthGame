using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AISensors : MonoBehaviour
{
    [SerializeField] Transform _root;
    [SerializeField] Transform[] _patrolPath;
    [SerializeField] EntityMovement _movement;
    [SerializeField] float _destinationDistance = 0.5f;

    int _currentIndex;

    public Vector3 CurrentDestination => _patrolPath[_currentIndex].position;
    public IEnumerable<Vector3> SampledPath { get; private set; }

    private void Awake()
    {
        SampledPath = _patrolPath.Select(i => i.position);

    }

    private void Update()
    {
        // Move to
        _movement.Direction = CurrentDestination - transform.position;

        // Estimate distance to change destination
        var distance = Vector3.Distance(transform.position, CurrentDestination);
        if(distance < _destinationDistance)
        {
            _currentIndex++;
            if(_currentIndex >= _patrolPath.Length)
            {
                _currentIndex = 0;
            }
        }
    }


    #region EDITOR
#if UNITY_EDITOR
    [SerializeField] float _radiusGizmos = 0.5f;
    private void OnDrawGizmos()
    {
        if (_patrolPath == null || _patrolPath.Length == 0) return;

        if(Application.isEditor)
        {
            Gizmos.color = Color.blue;
            if(Application.isPlaying)
            {

            }
            else
            {
                // Draw Lines
                Gizmos.color = Color.yellow;
                for (int i = 0; i < _patrolPath.Length - 1; i++)
                {
                    Gizmos.DrawLine(_patrolPath[i].position, _patrolPath[i + 1].position);
                }
                Gizmos.DrawLine(_patrolPath[0].position, _patrolPath[_patrolPath.Length - 1].position);
                
                // Then draw spheres
                Gizmos.color = Color.blue;
                foreach(var el in _patrolPath)
                {
                    Gizmos.DrawSphere(el.position, _radiusGizmos);
                }
                
            }
        }
    }
#endif

    #endregion

}

public static class IEnumerableExtension
{
    public static IEnumerable<T> InfiniteLoop<T>(this IEnumerable<T> @this)
    {
        while(true)
        {
            var enumerator = @this.GetEnumerator();
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }
    }
}
