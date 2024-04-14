using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayAreaCheck : MonoBehaviour
{
    [Header("Gizmos")]
    [SerializeField] private bool drawGizmos = true;
    [SerializeField] private Color gizmosColor = Color.red;

    [Header("Detection")]
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float range = 1;
    [SerializeField] private Vector3 originOffset = Vector3.zero;
    [SerializeField] private Vector3 direction = Vector3.forward;

    private const int MAX_COLLISIONS = 1;
    private readonly RaycastHit[] collisions = new RaycastHit[MAX_COLLISIONS];

    public Vector3 Direction => transform.TransformDirection(direction).normalized;

    public int CheckArea()
    {
        return Physics.RaycastNonAlloc(transform.position + originOffset, Direction, collisions, range, layerMask);
    }

    public RaycastHit GetHit()
    {
        return collisions[0];
    }

    public void OnDrawGizmosSelected()
    {
        if (!drawGizmos) 
            return;

        Gizmos.color = gizmosColor;
        Gizmos.DrawRay(transform.position + originOffset, Direction * range);
    }
}