using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGrounded : MonoBehaviour
{
    [SerializeField] Transform _raycastRoot;
    [SerializeField] Vector3 _raycastDirection;


    void Update()
    {
        if(Physics.Raycast(_raycastRoot.position, _raycastDirection, out RaycastHit hit, _raycastDirection.magnitude))
        {
            Debug.DrawLine(_raycastRoot.position, _raycastRoot.position + _raycastDirection, Color.magenta);
        }
        else
        {
            Debug.DrawLine(_raycastRoot.position, _raycastRoot.position + _raycastDirection, Color.red);

        }

    }



}
